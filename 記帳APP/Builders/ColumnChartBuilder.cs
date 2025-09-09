using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using 記帳APP.Attributes;
using 記帳APP.Repository.Entities;

namespace 記帳APP.Builders
{
    [ChartType("Column")]
    internal class ColumnChartBuilder : ChartBuilder
    {
        private List<IGrouping<string, RecordEntity>> groupByList;
        public ColumnChartBuilder(int width, int height) : base(width, height)
        {
        }

        public override ChartBuilder SetGroupBy(List<string> groupByList)
        {
            var propName = typeof(RecordEntity).GetProperties()
              .Where(x => groupByList.Contains(x.GetCustomAttribute<RecordAttribute>()?.Name))
              .ToList();
            this.groupByList = filterRecords.GroupBy(x => DateTime.Parse(x.Date).ToString("yyyy-MM-dd"))
            .OrderBy(x => x.Key).ToList();
            return this;
        }

        public override ChartBuilder SetSeries(int month = 1)
        {
            var key = groupByList.Select(a => a.Key).ToList();
            var money = groupByList.Select(a => a.Sum(b => int.Parse(b.Money)).ToString()).ToList();
            Series series = new Series();
            series.ChartType = SeriesChartType.Column;
            series.XValueType = ChartValueType.String;
            // 顯示數值在圖上
            series.IsValueShownAsLabel = true;
            series.Points.DataBindXY(key, money);
            // 顯示名稱 + 數值 + 百分比
            series.Label = "#VAL (#PERCENT{P2})";
            // Tooltip
            series.ToolTip = "#VALX: #VAL 元 (#PERCENT{P2})";

            // 圖例 (Legend)
            series.LegendText = "花費日趨勢";
            series.Legend = "legend1";
            // 加入到圖表
            chart.Series.Add(series);
            return this;
        }
    }
}
