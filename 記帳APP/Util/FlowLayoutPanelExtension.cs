using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 記帳APP.Models.DTOs;

namespace 記帳APP.Util
{
    internal static class FlowLayoutPanelExtension
    {
        static EventHandler Checkchange;
        static Dictionary<string, List<string>> KeyValuePairs;
        public static void GenerateCheckboxs(this FlowLayoutPanel flowLayoutPanel, AnalysisDTO analysisDTO, EventHandler checkchange)
        {
            KeyValuePairs = analysisDTO.KeyValuePairs;
            Checkchange = checkchange;
            var analysisTypes = analysisDTO.InitialAnalysisTypes();

            FlowLayoutPanel groupByPanel = new FlowLayoutPanel();
            groupByPanel.Width = flowLayoutPanel.Width;
            groupByPanel.Height = 80;
            //groupByPanel.AutoSize = true;
            Label groupByLabel = new Label();
            groupByLabel.Text = "分類依據";
            groupByPanel.Name = "分類依據";
            groupByLabel.Font = new Font("Arial", 10F, FontStyle.Bold);
            groupByPanel.Controls.Add(groupByLabel);
            foreach (var analysisType in analysisTypes)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Text = analysisType.Key;
                checkBox.AutoSize = true;
                checkBox.CheckedChanged += checkchange;
                groupByPanel.Controls.Add((checkBox));
            }
            flowLayoutPanel.Controls.Add(groupByPanel);

            foreach (var analysisType in analysisTypes)
            {
                FlowLayoutPanel analysisContainer = new FlowLayoutPanel();
                analysisContainer.Width = flowLayoutPanel.Width;
                analysisContainer.Height = 150;
                //analysisContainer.AutoSize = true;
                analysisContainer.Name = analysisType.Key;
                Label label = new Label();
                label.Text = analysisType.Key;
                label.Font = new Font("Arial", 10F, FontStyle.Bold);
                analysisContainer.Controls.Add(label);
                CheckBox defaultCheckBox = new CheckBox();
                defaultCheckBox.Text = "不限";
                defaultCheckBox.CheckedChanged += SelectAll_CheckedChanged;
                analysisContainer.Controls.Add(defaultCheckBox);
                List<string> valueList = analysisTypes[analysisType.Key];
                RenderCheckBox(analysisType.Key, analysisContainer, valueList);
                flowLayoutPanel.Controls.Add(analysisContainer);
            }
        }

        private static void RenderCheckBox(string type, FlowLayoutPanel analysisContainer, List<string> list)
        {
            foreach (var value in list)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Text = value;
                checkBox.Checked = checkBox.Text == "食" ? true : false;
                if (type == "類型")
                    checkBox.CheckedChanged += CheckBox_CheckedChanged;
                checkBox.CheckedChanged += Checkchange;
                checkBox.CheckedChanged += SelectAll_CheckedChanged;
                analysisContainer.Controls.Add(checkBox);
            }
        }

        private static void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // 只做UI刷新
            CheckBox box = (CheckBox)sender;
            string type = box.Text.ToString().Trim();
            List<string> detail = KeyValuePairs[type];
            FlowLayoutPanel detailPanel = box.Parent.Parent.Controls.OfType<FlowLayoutPanel>().FirstOrDefault(x => x.Name == "類型細項");
            if (box.Checked)
            {
                RenderCheckBox("類型細項", detailPanel, detail);
            }
            else
            {
                var delete = detailPanel.Controls
                    .OfType<CheckBox>()
                    .Where(x => detail.Contains(x.Text))
                    .ToList();
                delete.ForEach(x => detailPanel.Controls.Remove(x));
            }
        }

        private static void SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox box = (CheckBox)sender;
            if (box.Text.ToString() == "不限")
            {
                box.Parent.Controls
                    .OfType<CheckBox>()
                    .Where(x => x != box)
                    .ToList()
                    .ForEach(x => x.Checked = true);
            }
            var selectAllBox = box.Parent.Controls
               .OfType<CheckBox>()
               .Where(x => x.Text.ToString() == "不限").First();
            if (selectAllBox.Checked && !box.Checked)
            {
                selectAllBox.Checked = false;
            }
        }
    }
}
