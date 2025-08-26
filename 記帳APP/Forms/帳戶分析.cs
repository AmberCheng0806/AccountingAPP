using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 記帳APP.Attributes;
using 記帳APP.Models.DTOs;
using 記帳APP.Presenter;
using 記帳APP.Util;
using static 記帳APP.Contract.AnalysisContract;

namespace 記帳APP.Forms
{
    [DisplayName("分析2")]
    [Order(3)]
    public partial class 帳戶分析 : Form, IAnalysisView
    {
        private IAnalysisPresenter analysisPresenter;
        Dictionary<string, List<string>> keyValuePairs = new Dictionary<string, List<string>>();
        List<string> groupByList = new List<string>();
        public 帳戶分析()
        {
            InitializeComponent();
            analysisPresenter = new AnalysisPresenter(this);
            analysisPresenter.GetExpenseOptions();
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
            analysisPresenter.GetAnalysisData(dateTimePicker.Value.Date, dateTimePickerEnd.Value.Date, groupByList, keyValuePairs);
        }

        public void RenderAnalysisData(List<AnalysisDataDTO> analysisDataDTOs)
        {
            dataGridView.DataSource = analysisDataDTOs;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.Columns["Key"].HeaderText = "分類依據";
            dataGridView.Columns["Money"].HeaderText = "加總";
        }
    }
}
