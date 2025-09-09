using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using 記帳APP.Repository.Entities;
using 記帳APP.Repository;
using System.Reflection;
using 記帳APP.Attributes;
using System.Windows.Forms;

namespace 記帳APP.Builders
{
    [ChartType("Pie")]
    internal class PieChartBuilder : ChartBuilder
    {
        private List<IGrouping<string, RecordEntity>> groupByList;

        public PieChartBuilder(int width, int height) : base(width, height)
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
                // 將每一筆prop的屬性都轉成要群組的值,並用string.join 串成字串作為群組的key
                var list = propName.Select(y => y.GetValue(x).ToString());
                return string.Join(",", list);

                // 原版寫法
                //string key = "";
                //foreach (var item in groupByList)
                //{
                //    var prop = props.Where(z => z.GetCustomAttribute<RecordAttribute>().Name == item).First();
                //    key = key + prop.GetValue(x) + ",";
                //}
                //key = key.Remove(key.Length - 1);
                //return key;
            }).ToList();
            this.groupByList = groupByResult;
            return this;
        }

        public override ChartBuilder SetSeries(int month)
        {
            var key = groupByList.Select(a => a.Key).ToList();
            var money = groupByList.Select(a => a.Sum(b => int.Parse(b.Money)).ToString()).ToList();
            Series series = new Series();
            series.ChartType = SeriesChartType.Pie;
            series.XValueType = ChartValueType.String;
            // 顯示數值在圓餅圖上
            series.IsValueShownAsLabel = true;
            series.Points.DataBindXY(key, money);
            // 顯示名稱 + 數值 + 百分比

            series.Label = "#VAL (#PERCENT{P2})";
            // Tooltip (滑鼠移到扇區顯示提示)
            series.ToolTip = "#VALX: #VAL 元 (#PERCENT{P2})";

            // 圖例 (Legend)
            series.LegendText = "#VALX";
            series.Legend = "legend1";
            // 加入到圖表
            chart.Series.Add(series);
            return this;
        }
    }
}
