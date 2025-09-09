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
using System.Drawing;
using 記帳APP.Service;
using System.Xml.Linq;
using 記帳APP.Util;
using 記帳APP.Models;

namespace 記帳APP.Builders
{
    [ChartType("Line")]
    internal class LineChartBuilder : ChartBuilder
    {
        private List<IGrouping<string, RecordEntity>> groupByList;
        private ChartComboBoxModel chartComboBoxModel;

        public LineChartBuilder(int width, int height) : base(width, height)
        {
            chartComboBoxModel = new ChartComboBoxModel();
        }

        public override ChartBuilder SetGroupBy(List<string> groupByList)
        {
            //從原本四筆props中根據groupByList的內容篩選出指定要group by的props
            var propName = typeof(RecordEntity).GetProperties()
                 .Where(x => groupByList.Contains(x.GetCustomAttribute<RecordAttribute>()?.Name))
                 .ToList();
            this.groupByList = filterRecords.GroupBy(x => DateTime.Parse(x.Date).ToString("yyyy-MM-dd"))
            .OrderBy(x => x.Key).ToList();
            return this;
        }


        public override ChartBuilder SetSeries(int month)
        {
            for (int i = 0; i < month; i++)
            {
                DateTime start = DateTime.Parse(allDates.First()).AddMonths(-i);
                DateTime end = DateTime.Parse(allDates.Last()).AddMonths(-i);
                SetDateRange(start, end);
                SetFilter(filterConditions);
                this.groupByList = filterRecords.GroupBy(x => DateTime.Parse(x.Date).ToString("yyyy-MM-dd")).OrderBy(x => x.Key).ToList();
                var groupByDictionary = groupByList.ToDictionary(x => x.Key, x => x.Sum(y => int.Parse(y.Money)).ToString());
                List<string> values = allDates.Select(x => groupByDictionary.ContainsKey(x) ? groupByDictionary[x] : "0").ToList();

                Series series = new Series();
                series.ChartType = SeriesChartType.Line;
                series.XValueType = ChartValueType.String;
                var xData = allDates.Select(x => x.Remove(0, 5)).ToList();
                series.Points.DataBindXY(xData, values);
                series.Label = "#VAL";
                // Tooltip 
                series.ToolTip = "#VALX: #VAL 元";
                // 圖例 (Legend)
                series.LegendText = chartComboBoxModel.GetNumberSelectionName(i);
                series.Legend = "legend1";
                // 加入到圖表
                chart.Series.Add(series);
            }
            return this;
        }
    }
}
