using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static 記帳APP.Contract.AnalysisContract;
using 記帳APP.Repository;
using 記帳APP.Service;
using static 記帳APP.Contract.ChartContract;
using 記帳APP.Models.DTOs;
using System.IO;
using 記帳APP.Repository.Entities;
using System.Security.Policy;

namespace 記帳APP.Presenter
{
    internal class ChartPresenter : IChartPresenter
    {
        private IChartView chartView;
        private IExpenseOptionsRepository data;
        private IRecordRepository recordRepository;
        private DataAnalysisService dataAnalysisService;

        public ChartPresenter(IChartView chartView)
        {
            this.chartView = chartView;
            data = new ExpenseOptionsRepository();
            recordRepository = new RecordRepository();
            dataAnalysisService = new DataAnalysisService();
        }
        public void GetChartData(DateTime start, DateTime end, List<string> groupByList, Dictionary<string, List<string>> keyValuePairs, string chartType, int month = 1)
        {
            if (chartType == "Pie")
            {
                var groupByResult = dataAnalysisService.GetAnalysisData(start, end, groupByList, keyValuePairs);
                var key = groupByResult.Select(x => x.Key).ToList();
                var money = groupByResult.Select(x => x.Sum(y => int.Parse(y.Money)).ToString()).ToList();
                chartView.RenderChartData(new PieChartDataDTO(key, money));
                return;
            }

            // 日期list
            List<Dictionary<string, List<string>>> moneyListForMonth = new List<Dictionary<string, List<string>>>();
            if (chartType == "Line")
            {
                for (int i = 0; i < month; i++)
                {
                    DateTime newStart = start.AddMonths(-i);
                    DateTime newEnd = end.AddMonths(-i);
                    List<string> Dates = GetAllDates(newStart, newEnd);
                    List<string> money = GetMoneyForMonth(newStart, newEnd, keyValuePairs, Dates);
                    Dictionary<string, List<string>> moneyDictionaryForMonth = new Dictionary<string, List<string>>();
                    moneyDictionaryForMonth.Add(i.ToString(), money);
                    moneyListForMonth.Add(moneyDictionaryForMonth);
                }
                chartView.RenderChartData(new LineChartDataDTO(GetAllDates(start, end), moneyListForMonth));
                return;
            }



            //if (chartType == "Line")
            //{
            //    var groupByResult = dataAnalysisService.GetLineAnalysisData(start, end, keyValuePairs);
            //    var key = groupByResult.Select(x => x.Key).ToList();
            //    var money = groupByResult.Select(x => x.Sum(y => int.Parse(y.Money)).ToString()).ToList();
            //    Dictionary<string, string> lineChartData = new Dictionary<string, string>();
            //    foreach (var date in AllDates)
            //    {
            //        if (key.Contains(date))
            //            lineChartData[date] = money[key.IndexOf(date)];
            //        else
            //            lineChartData[date] = "0";
            //    }
            //    money = lineChartData.Select(x => x.Value).ToList();
            //    chartView.RenderChartData(new LineChartDataDTO(AllDates, money));
            //    return;
            //}
            var groupByResult2 = dataAnalysisService.GetStackAnalysisData(start, end, groupByList, keyValuePairs);
            var names = groupByResult2.Select(x => x.Key.Item2).Distinct().ToList();
            //var key2 = groupByResult2.Select(x => x.Key).ToList();
            //var money2 = groupByResult2.Select((x => x.Sum(y => int.Parse(y.Money)).ToString())).ToList();
            List<string> AllDates = GetAllDates(start, end);
            Dictionary<string, Dictionary<string, string>> stackChartData = new Dictionary<string, Dictionary<string, string>>();
            foreach (var date in AllDates)
            {
                stackChartData[date] = new Dictionary<string, string>();
                foreach (var name in names)
                {
                    var grouping = groupByResult2
                         .FirstOrDefault(x => x.Key.Item1 == date && x.Key.Item2 == name);
                    stackChartData[date][name] = grouping != null ? grouping.Sum(x => int.Parse(x.Money)).ToString() : "0";
                }
            }
            List<Dictionary<string, string>> yDatas = AllDates
                .Select(date => stackChartData[date]).ToList();
            List<string> total = yDatas
                .Select(x => x.Values.Sum(y => int.Parse(y)).ToString())
                .ToList();
            chartView.RenderChartData(new StackedColumnChartDTO(AllDates, names, yDatas, total));
        }

        private List<string> GetAllDates(DateTime start, DateTime end)
        {
            List<string> AllDates = new List<string>();
            TimeSpan timeSpan = end - start;
            for (int i = 0; i <= timeSpan.Days; i++)
            {
                string date = start.AddDays(i).ToString("yyyy-MM-dd");
                AllDates.Add(date);
            }

            if (start.Year == end.Year && end.Month - start.Month > 0)
            {
                int count = end.Month - start.Month;
                DateTime dateTime = start;
                for (int i = 0; i < count; i++)
                {
                    dateTime = dateTime.AddMonths(i);
                    List<string> missingDays = GetMissingDays(dateTime);
                    AllDates = CheckDayExist(missingDays, AllDates);
                }
            }

            if (end.Year - start.Year > 0)
            {
                int startCount = 12 - start.Month + 1;
                DateTime dateTime = start;
                for (int i = 0; i < startCount; i++)
                {
                    dateTime = dateTime.AddMonths(i);
                    List<string> missingDays = GetMissingDays(dateTime);
                    AllDates = CheckDayExist(missingDays, AllDates);
                }

                int endCount = end.Month - 1;
                DateTime day = DateTime.Parse(end.Year.ToString() + "-01-01");
                dateTime = day;
                for (int i = 0; i < endCount; i++)
                {
                    dateTime = dateTime.AddMonths(i);
                    List<string> missingDays = GetMissingDays(dateTime);
                    AllDates = CheckDayExist(missingDays, AllDates);
                }
            }
            List<string> orderedDates = AllDates.OrderBy(x => x).ToList();
            return orderedDates;
        }

        //拿到該月份缺失的日期(固定31天)
        private List<string> GetMissingDays(DateTime dateTime)
        {
            List<string> days = new List<string>();
            string date = dateTime.ToString("yyyy-MM");
            if (dateTime.Month != 2)
            {
                days.Add(date + "-31");
                return days;
            }
            days.Add(date + "-29");
            days.Add(date + "-30");
            days.Add(date + "-31");
            return days;
        }

        //補上list中缺失日期
        private List<string> CheckDayExist(List<string> missingDays, List<string> dates)
        {
            foreach (var item in missingDays)
            {
                if (dates.Contains(item))
                {
                    continue;
                }
                dates.Add(item);
            }
            return dates;
        }

        private List<string> GetMoneyForMonth(DateTime start, DateTime end, Dictionary<string, List<string>> keyValuePairs, List<string> Dates)
        {
            var groupByResult = dataAnalysisService.GetLineAnalysisData(start, end, keyValuePairs);
            var key = groupByResult.Select(x => x.Key).ToList();
            var money = groupByResult.Select(x => x.Sum(y => int.Parse(y.Money)).ToString()).ToList();
            Dictionary<string, string> lineChartData = new Dictionary<string, string>();
            foreach (var date in Dates)
            {
                if (key.Contains(date))
                    lineChartData[date] = money[key.IndexOf(date)];
                else
                    lineChartData[date] = "0";
            }
            money = lineChartData.Select(x => x.Value).ToList();
            return money;
        }

        public void GetExpenseOptions()
        {
            var people = data.GetPeople();
            var pay = data.GetPayMethods();
            chartView.CheckBoxesResponse(new AnalysisDTO(data.GetExpenseCategoryMap(), people, pay));
        }


    }
}
