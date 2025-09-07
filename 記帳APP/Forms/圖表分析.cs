using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Linq;
using 記帳APP.Attributes;
using 記帳APP.Models;
using 記帳APP.Models.DTOs;
using 記帳APP.Presenter;
using 記帳APP.Util;
using static AutoMapper.Internal.ExpressionFactory;
using static 記帳APP.Contract.AnalysisContract;
using static 記帳APP.Contract.ChartContract;

namespace 記帳APP.Forms
{
    [DisplayName("圖表2")]
    [Order(4)]
    public partial class 圖表分析 : Form, IChartView
    {
        private IChartPresenter chartPresenter;
        Dictionary<string, List<string>> keyValuePairs = new Dictionary<string, List<string>>();
        List<string> groupByList = new List<string>();
        List<string> dropDownSelections = new List<string>() { "Pie", "Line", "StackedColumn" };
        List<ComboBoxModel> numberSelections = new List<ComboBoxModel>()
        {
            new ComboBoxModel("當月資料",1),
            new ComboBoxModel("與上個月相比",2),
            new ComboBoxModel("與兩個月前相比",3)
        };
        public 圖表分析()
        {
            InitializeComponent();
            chartPresenter = new ChartPresenter(this);
            chartPresenter.GetExpenseOptions();
            comboBox1.DataSource = dropDownSelections;

        }
        public void CheckBoxesResponse(AnalysisDTO analysisDTO)
        {
            flowLayoutPanel1.GenerateCheckboxs(analysisDTO, typeCheckBox_CheckedChanged);
        }
        private void typeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.Parent.Name == "分類依據")
            {
                if (checkBox.Checked)
                {
                    groupByList.Add(checkBox.Text.ToString());
                }
                else
                {
                    groupByList.Remove(checkBox.Text.ToString());
                }
                Console.WriteLine($"{string.Join(",", groupByList)}");
                return;
            }


            //把資料送回dictionary
            FlowLayoutPanel container = (FlowLayoutPanel)checkBox.Parent;
            string key = container.Name;
            var value = container.Controls.OfType<CheckBox>()
                .Where(x => x.Checked && x.Text != "不限")
                .Select(y => y.Text)
                .ToList();
            if (value.Count > 0)
            {
                keyValuePairs[key] = value;
            }
            else
            {
                keyValuePairs.Remove(key);
            }

            Console.WriteLine($"{key}：{string.Join(",", value)}");
        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            Reset();
        }

        public void RenderChartData(Object obj)
        {
            if (obj is PieChartDataDTO pie)
            {
                RenderPieChart(pie.Key, pie.Money);
                return;
            }
            if (obj is LineChartDataDTO line)
            {
                RendeLineChart(line.Date, line.MoneyListForMonth);
                return;
            }
            if (obj is StackedColumnChartDTO stack)
            {
                RenderStackChart(stack.Date, stack.YDatas, stack.totalPerDate, stack.Names);
            }
        }


        private void 圖表分析_Load(object sender, EventArgs e)
        {
            //RenderChart(dropDownSelections[0]);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Reset();
            if (comboBox1.Text != "Line")
            {
                flowLayoutPanel3.Controls.Clear();
            }
        }

        private void Reset()
        {
            flowLayoutPanel2.Controls.Clear();
            if (comboBox1.Text == "Line")
            {
                ComboBox numberBox = new ComboBox();
                numberBox.DataSource = numberSelections;
                numberBox.DisplayMember = "Key";
                numberBox.ValueMember = "Value";
                flowLayoutPanel3.Controls.Add(numberBox);
                numberBox.SelectedValueChanged += numberBox_SelectedIndexChanged;
                chartPresenter.GetChartData(dateTimePicker.Value.Date, dateTimePickerEnd.Value.Date, groupByList, keyValuePairs, comboBox1.Text, (int)numberBox.SelectedValue);
                return;
            }
            chartPresenter.GetChartData(dateTimePicker.Value.Date, dateTimePickerEnd.Value.Date, groupByList, keyValuePairs, comboBox1.Text, 1);
        }

        private void numberBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            flowLayoutPanel2.Controls.Clear();
            chartPresenter.GetChartData(dateTimePicker.Value.Date, dateTimePickerEnd.Value.Date, groupByList, keyValuePairs, comboBox1.Text, (int)comboBox.SelectedValue);
        }


        private void RenderPieChart(List<string> X, List<string> Y)
        {
            flowLayoutPanel2.Controls.Clear();
            Chart chart = new Chart();
            chart.Width = flowLayoutPanel1.Width;
            chart.Height = flowLayoutPanel1.Height;

            // 圖表區域
            ChartArea chartArea = new ChartArea();
            chart.ChartAreas.Add(chartArea);

            //// 建立系列 
            ChartSeriesSetting(chart, X, Y);

            // 圖例 (Legend)
            Legend legend = new Legend();
            legend.Name = "legend1";
            legend.Docking = Docking.Top;
            chart.Legends.Add(legend);
            chart.ChartAreas[0].AxisX.LabelStyle.Interval = 1;  // 每一個都顯示
            chart.ChartAreas[0].AxisX.LabelStyle.IsStaggered = true; // 交錯顯示，避免重疊
            flowLayoutPanel2.Controls.Add(chart);
            chart.Dock = DockStyle.Bottom;
        }

        private void RendeLineChart(List<string> X, List<Dictionary<string, List<string>>> Y)
        {
            flowLayoutPanel2.Controls.Clear();
            Chart chart = new Chart();
            chart.Width = flowLayoutPanel1.Width;
            chart.Height = flowLayoutPanel1.Height;

            // 圖表區域
            ChartArea chartArea = new ChartArea();
            chart.ChartAreas.Add(chartArea);

            //// 建立系列 
            int count = Y.Count;
            for (int i = 0; i < count; i++)
            {
                ChartSeriesSetting(chart, X, Y[i].Values.ToList().First(), Y[i].Keys.ToList().First());
            }

            // 圖例 (Legend)
            Legend legend = new Legend();
            legend.Name = "legend1";
            legend.Docking = Docking.Top;
            chart.Legends.Add(legend);
            chart.ChartAreas[0].AxisX.LabelStyle.Interval = 1;  // 每一個都顯示
            chart.ChartAreas[0].AxisX.LabelStyle.IsStaggered = true; // 交錯顯示，避免重疊
            flowLayoutPanel2.Controls.Add(chart);
            chart.Dock = DockStyle.Bottom;
        }

        private void RenderStackChart(List<string> X, List<Dictionary<string, string>> Y, List<string> totalPerDay, List<string> names)
        {
            flowLayoutPanel2.Controls.Clear();
            Chart chart = new Chart();
            chart.Width = flowLayoutPanel1.Width;
            chart.Height = flowLayoutPanel1.Height;

            // 圖表區域
            ChartArea chartArea = new ChartArea();
            chart.ChartAreas.Add(chartArea);

            // series
            foreach (string name in names)
            {
                List<string> yValues = new List<string>();
                foreach (var dic in Y)
                {
                    string value = dic[name];
                    yValues.Add(value);
                }
                ChartSeriesSetting(chart, X, yValues, totalPerDay, name);
            }

            // 圖例 (Legend)
            Legend legend = new Legend();
            legend.Name = "legend1";
            legend.Docking = Docking.Top;
            chart.Legends.Add(legend);
            chart.ChartAreas[0].AxisX.LabelStyle.Interval = 1;  // 每一個都顯示
            chart.ChartAreas[0].AxisX.LabelStyle.IsStaggered = true; // 交錯顯示，避免重疊
            flowLayoutPanel2.Controls.Add(chart);
            chart.Dock = DockStyle.Bottom;

        }

        private void ChartSeriesSetting(Chart chart, List<string> X, List<string> Y, string name)
        {
            Series series = new Series();
            series.ChartType = SeriesChartType.Line;
            series.XValueType = ChartValueType.String;
            X = X.Select(x => x.Remove(0, 5)).ToList();
            series.Points.DataBindXY(X, Y);


            series.Label = "#VAL";
            // Tooltip 
            series.ToolTip = "#VALX: #VAL 元";


            // 圖例 (Legend)
            series.LegendText = numberSelections[int.Parse(name)].Key;
            series.Legend = "legend1";
            // 加入到圖表
            chart.Series.Add(series);
        }


        private void ChartSeriesSetting(Chart chart, List<string> X, List<string> Y, List<string> totalPerDay, string name)
        {
            Series series = new Series();
            series.ChartType = SeriesChartType.StackedColumn100;
            series.XValueType = ChartValueType.String;
            X = X.Select(x => x.Remove(0, 5)).ToList();
            series.Points.DataBindXY(X, Y);

            int count = series.Points.Count;
            for (int i = 0; i < count; i++)
            {
                double value = int.Parse(Y[i]);
                double percent = (value / int.Parse(totalPerDay[i])) * 100;
                string percentStr = percent.ToString("F2") + "%";
                series.Points[i].Label = percentStr; // int => D2 00 01 02 , D3 001 002 003
                // Tooltip 
                series.Points[i].ToolTip = $"#VALX: #VAL 元 ({percentStr})";
            }

            // 圖例 (Legend)
            series.LegendText = name;
            series.Legend = "legend1";
            // 加入到圖表
            chart.Series.Add(series);

        }

        private void ChartSeriesSetting(Chart chart, List<string> X, List<string> Y)
        {
            Series series = new Series();
            series.ChartType = SeriesChartType.Pie;
            series.XValueType = ChartValueType.String;
            // 顯示數值在圓餅圖上
            series.IsValueShownAsLabel = true;
            series.Points.DataBindXY(X, Y);
            // 顯示名稱 + 數值 + 百分比

            series.Label = "#VAL (#PERCENT{P2})";
            // Tooltip (滑鼠移到扇區顯示提示)
            series.ToolTip = "#VALX: #VAL 元 (#PERCENT{P2})";

            // 圖例 (Legend)
            series.LegendText = "#VALX";
            series.Legend = "legend1";
            // 加入到圖表
            chart.Series.Add(series);
        }
    }
}
