using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using 記帳APP.Repository.Entities;
using 記帳APP.Repository;
using 記帳APP.Util;
using System.Reflection;
using 記帳APP.Attributes;
using System.Windows.Forms;

namespace 記帳APP.Builders
{
    internal abstract class ChartBuilder
    {
        protected IRecordRepository recordRepository = new RecordRepository();
        protected List<RecordEntity> rawDataRecords;
        protected List<RecordEntity> filterRecords;
        protected Chart chart = new Chart();
        protected List<string> allDates;
        protected Dictionary<string, List<string>> filterConditions;

        protected ChartBuilder(int width, int height)
        {
            chart.Width = width;
            chart.Height = height;
            // 圖表區域
            ChartArea chartArea = new ChartArea();
            chart.ChartAreas.Add(chartArea);
        }
        public virtual ChartBuilder SetDateRange(DateTime start, DateTime end)
        {
            List<RecordEntity> records = recordRepository.GetDatas(start, end);
            this.rawDataRecords = records;
            allDates = DateUtil.GetAllDates(start, end);
            return this;
        }
        public virtual ChartBuilder SetFilter(Dictionary<string, List<string>> filterConditions)
        {
            this.filterConditions = filterConditions;
            var props = typeof(RecordEntity).GetProperties().Where(y =>
            {
                var attribute = y.GetCustomAttribute<RecordAttribute>();
                if (attribute != null && filterConditions.ContainsKey(attribute.Name)) return true;
                return false;
            }).ToList();
            var filrerList = filterConditions.Values.SelectMany(a => a).ToList();

            //將raw data 透過 dictionary條件篩List<RecordEntity> records
            var result = rawDataRecords
                .Where(x =>
                //改版後的寫法all:集合內所有符合條件才回傳true
                //先將dict keys中的prop篩選出來(在這只剩確定有用到的prop) 再從prop中取出物件對應屬性的value值,如果所有篩選後的props都能與值符合
                //代表該筆資料可以被篩選出來
                props.All(y => filrerList.Contains(y.GetValue(x).ToString())))
                .ToList();
            filterRecords = result;
            return this;
        }
        public abstract ChartBuilder SetGroupBy(List<string> groupByList);
        public abstract ChartBuilder SetSeries(int month = 1);
        public virtual ChartBuilder SetLegend()
        {
            // 圖例 (Legend)
            Legend legend = new Legend();
            legend.Name = "legend1";
            legend.Docking = Docking.Top;
            chart.Legends.Add(legend);
            chart.ChartAreas[0].AxisX.LabelStyle.Interval = 1;  // 每一個都顯示
            chart.ChartAreas[0].AxisX.LabelStyle.IsStaggered = true; // 交錯顯示，避免重疊
            chart.Dock = DockStyle.Bottom;
            return this;
        }
        public virtual Chart Build()
        {
            return chart;
        }
    }
}
