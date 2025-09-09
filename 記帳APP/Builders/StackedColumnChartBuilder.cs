using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;
using 記帳APP.Attributes;
using 記帳APP.Repository.Entities;
using 記帳APP.Repository;
using System.Xml.Linq;
using 記帳APP.Util;

namespace 記帳APP.Builders
{
    [ChartType("StackedColumn")]
    internal class StackedColumnChartBuilder : ChartBuilder
    {
        private List<IGrouping<(string, string), RecordEntity>> groupByList;
        List<Dictionary<string, string>> yDatas = new List<Dictionary<string, string>>();
        List<string> totalPerDay = new List<string>();

        public StackedColumnChartBuilder(int width, int height) : base(width, height)
        {
        }

        public override ChartBuilder SetGroupBy(List<string> groupByList)
        {
            //從原本四筆props中根據groupByList的內容篩選出指定要group by的props
            var propName = typeof(RecordEntity).GetProperties()
                 .Where(x => groupByList.Contains(x.GetCustomAttribute<RecordAttribute>()?.Name))
                 .ToList();
            var groupByResult = filterRecords.GroupBy(x =>
            {
                string date = DateTime.Parse(x.Date).ToString("yyyy-MM-dd");
                var list = propName.Select(y => y.GetValue(x).ToString());
                return (date, string.Join(",", list));
            }).ToList();
            this.groupByList = groupByResult;
            return this;
        }

        public override ChartBuilder SetSeries(int month)
        {
            var names = groupByList.Select(a => a.Key.Item2).Distinct().ToList();
            Dictionary<string, Dictionary<string, string>> stackChartData = new Dictionary<string, Dictionary<string, string>>();
            foreach (var date in allDates)
            {
                stackChartData[date] = new Dictionary<string, string>();
                foreach (var name in names)
                {
                    var grouping = groupByList
                         .FirstOrDefault(a => a.Key.Item1 == date && a.Key.Item2 == name);
                    stackChartData[date][name] = grouping != null ? grouping.Sum(a => int.Parse(a.Money)).ToString() : "0";
                }
            }
            yDatas = allDates
                .Select(date => stackChartData[date]).ToList();
            totalPerDay = yDatas
                .Select(a => a.Values.Sum(b => int.Parse(b)).ToString())
                .ToList();

            for (int i = 0; i < names.Count; i++)
            {
                string groupName = names[i];
                List<string> yValues = yDatas.Select(dic => dic[groupName]).ToList();

                Series stackSeries = new Series(groupName);
                stackSeries.ChartType = SeriesChartType.StackedColumn100;
                stackSeries.XValueType = ChartValueType.String;
                var xValues = allDates.Select(a => a.Remove(0, 5)).ToList();
                stackSeries.Points.DataBindXY(xValues, yValues);

                for (int j = 0; j < yValues.Count; j++)
                {
                    double value = int.Parse(yValues[j]);
                    double total = int.Parse(totalPerDay[j]);
                    double percent = total == 0 ? 0 : (value / total * 100);
                    string percentStr = percent.ToString("F2") + "%";
                    stackSeries.Points[j].Label = percentStr; // int => D2 00 01 02 , D3 001 002 003
                    // Tooltip 
                    stackSeries.Points[j].ToolTip = $"#VALX: #VAL 元 ({percentStr})";
                }
                // 圖例 (Legend)
                stackSeries.LegendText = groupName;
                stackSeries.Legend = "legend1";
                // 加入到圖表
                chart.Series.Add(stackSeries);
            }
            return this;
        }
    }
}
