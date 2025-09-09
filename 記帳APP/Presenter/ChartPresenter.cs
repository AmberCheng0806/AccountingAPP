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
using System.Windows.Forms.DataVisualization.Charting;
using 記帳APP.Builders;
using System.Reflection;
using System.Windows.Forms;
using 記帳APP.Attributes;
using 記帳APP.Models;

namespace 記帳APP.Presenter
{
    internal class ChartPresenter : IChartPresenter
    {
        private IChartView chartView;
        private IExpenseOptionsRepository data;
        private IRecordRepository recordRepository;

        public ChartPresenter(IChartView chartView)
        {
            this.chartView = chartView;
            data = new ExpenseOptionsRepository();
            recordRepository = new RecordRepository();
        }

        public void GetChartData(DateTime start, DateTime end, List<string> groupByList, Dictionary<string, List<string>> keyValuePairs, string chartType, int width, int height, int month = 1)
        {
            string name = $"記帳APP.Builders.{chartType}ChartBuilder";
            ChartBuilder chartBuilder = (ChartBuilder)Activator.CreateInstance(Type.GetType(name), width, height);
            Chart chart = chartBuilder
                .SetDateRange(start, end)
                .SetFilter(keyValuePairs)
                .SetGroupBy(groupByList)
                .SetSeries(month)
                .SetLegend()
                .Build();
            chartView.RenderChartData(chart);
        }


        public void GetExpenseOptions()
        {
            var people = data.GetPeople();
            var pay = data.GetPayMethods();
            chartView.CheckBoxesResponse(new AnalysisDTO(data.GetExpenseCategoryMap(), people, pay));
        }

        public void GetComboboxData()
        {
            List<string> dropDownSelections = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.BaseType == typeof(ChartBuilder) && x.GetCustomAttribute<ChartTypeAttribute>() != null)
                .Select(x => x.GetCustomAttribute<ChartTypeAttribute>().ChartName)
                .ToList();
            ChartComboBoxModel chartComboBoxModel = new ChartComboBoxModel();
            chartComboBoxModel.dropDownSelections = dropDownSelections;
            chartView.InitializeComboBox(chartComboBoxModel);
        }
    }
}
