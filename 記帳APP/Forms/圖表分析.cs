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
        public 圖表分析()
        {
            InitializeComponent();
            chartPresenter = new ChartPresenter(this);
            chartPresenter.GetExpenseOptions();
            chartPresenter.GetComboboxData();

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


        private void 圖表分析_Load(object sender, EventArgs e)
        {
            //RenderChart(dropDownSelections[0]);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Reset();
            if (comboBox1.Text == "Line")
            {
                numberBox.Visible = true;
            }
        }

        private void Reset()
        {
            flowLayoutPanel2.Controls.Clear();
            if (comboBox1.Text != "Line")
            {
                numberBox.Visible = false;
            }
            chartPresenter.GetChartData(dateTimePicker.Value.Date, dateTimePickerEnd.Value.Date, groupByList, keyValuePairs, comboBox1.Text, flowLayoutPanel1.Width, flowLayoutPanel1.Height, 1);
        }

        private void numberBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (comboBox.SelectedValue is int month)
            {
                flowLayoutPanel2.Controls.Clear();
                chartPresenter.GetChartData(dateTimePicker.Value.Date, dateTimePickerEnd.Value.Date, groupByList, keyValuePairs, comboBox1.Text, flowLayoutPanel1.Width, flowLayoutPanel1.Height, month);
            }
        }

        //建造者模式
        public void RenderChartData(Chart chart)
        {
            flowLayoutPanel2.Controls.Clear();
            flowLayoutPanel2.Controls.Add(chart);
        }

        public void InitializeComboBox(ChartComboBoxModel chartComboBoxModel)
        {
            comboBox1.DataSource = chartComboBoxModel.dropDownSelections;
            numberBox.DataSource = chartComboBoxModel.numberSelections;
            numberBox.DisplayMember = "Key";
            numberBox.ValueMember = "Value";
        }
    }
}
