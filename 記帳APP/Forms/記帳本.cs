using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 記帳APP.Attributes;
using 記帳APP.Models;

namespace 記帳APP.Forms
{
    [DisplayName("記帳本2")]
    [Order(2)]
    public partial class 記帳本 : Form
    {
        List<RecordModel> records = CSV_Library.CSVHelper.Read<RecordModel>("C:\\Users\\user\\Desktop\\campaigns\\record.csv");
        bool tag = true;
        public 記帳本()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Reload();
            //直行(Column)橫列(Row)
            //DataGridTextBoxColumn (單一欄位)
            //DataGridTextBoxCell (單一欄位下的儲存格)
            //dataGridView.Rows[3].Cells[2].Value = 0;
        }
        void dataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView.CurrentCell is DataGridViewComboBoxCell comboBoxCell && dataGridView.IsCurrentCellDirty)
            {
                dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView.CurrentCell.ColumnIndex == 2 && dataGridView.CurrentCell is DataGridViewComboBoxCell comboBoxCell)
            {
                var types = DataModel.keyValuePairs[comboBoxCell.Value.ToString()];
                DataGridViewComboBoxCell detailCell = (DataGridViewComboBoxCell)dataGridView.Rows[e.RowIndex].Cells[8];
                detailCell.DataSource = types;
                detailCell.Value = types[0];
            }
            Reload();
            File.Delete("C:\\Users\\user\\Desktop\\campaigns\\record.csv");
            CSV_Library.CSVHelper.Write<RecordModel>("C:\\Users\\user\\Desktop\\campaigns\\record.csv", records);
        }

        private void dataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 10 || e.ColumnIndex == 12)
            {
                string path = dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value.ToString();
                ImgForm form = new ImgForm(path);
                form.Show();
            }
            if (e.ColumnIndex == 14)
            {
                DialogResult result = MessageBox.Show("確認刪除", "系統通知", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    return;
                }
                string img1 = records[e.RowIndex].Img1;
                string img2 = records[e.RowIndex].Img2;
                dataGridView.Rows[e.RowIndex].Cells.OfType<DataGridViewImageCell>().ToList().ForEach(x => ((Bitmap)x.Value).Dispose());
                records.RemoveAt(e.RowIndex);
                dataGridView.DataSource = null;
                dataGridView.Columns.Clear();
                File.Delete(img1);
                File.Delete(img2);
                File.Delete("C:\\Users\\user\\Desktop\\campaigns\\record.csv");

                CSV_Library.CSVHelper.Write<RecordModel>("C:\\Users\\user\\Desktop\\campaigns\\record.csv", records);
                Reload();

            }
        }

        private void Reload()
        {
            dataGridView.CellValueChanged -= dataGridView_CellValueChanged;
            dataGridView.CurrentCellDirtyStateChanged -= dataGridView_CurrentCellDirtyStateChanged;
            dataGridView.DataSource = null;
            dataGridView.Columns.Clear();
            dataGridView.DataSource = records; // 透過反射將每一筆資料創建欄位
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            DataGridViewComboBoxColumn TypeComboBoxColumn = new DataGridViewComboBoxColumn();
            TypeComboBoxColumn.HeaderText = "Type";
            TypeComboBoxColumn.DataPropertyName = "Type";
            TypeComboBoxColumn.DataSource = DataModel.Type;
            dataGridView.Columns.Insert(2, TypeComboBoxColumn);

            dataGridView.Columns[3].Visible = false;

            DataGridViewComboBoxColumn PeopleComboBoxColumn = new DataGridViewComboBoxColumn();
            PeopleComboBoxColumn.HeaderText = "People";
            PeopleComboBoxColumn.DataPropertyName = "People";
            PeopleComboBoxColumn.DataSource = DataModel.People;
            dataGridView.Columns.Insert(4, PeopleComboBoxColumn);

            dataGridView.Columns[5].Visible = false;

            DataGridViewComboBoxColumn PayComboBoxColumn = new DataGridViewComboBoxColumn();
            PayComboBoxColumn.HeaderText = "Pay";
            PayComboBoxColumn.DataSource = DataModel.PaymentType;
            PayComboBoxColumn.DataPropertyName = "Pay";
            dataGridView.Columns.Insert(6, PayComboBoxColumn);

            dataGridView.Columns[7].Visible = false;

            DataGridViewComboBoxColumn DetailComboBoxColumn = new DataGridViewComboBoxColumn();
            DetailComboBoxColumn.HeaderText = "Detail";
            DetailComboBoxColumn.DataPropertyName = "Detail";
            dataGridView.Columns.Insert(8, DetailComboBoxColumn);

            dataGridView.Columns[9].Visible = false;

            DataGridViewImageColumn dataGridViewImageColumn = new DataGridViewImageColumn();
            dataGridViewImageColumn.HeaderText = "收據1";
            dataGridViewImageColumn.Name = "收據1";
            dataGridViewImageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            dataGridView.Columns.Insert(10, dataGridViewImageColumn);
            dataGridView.Columns["img1"].Visible = false;

            DataGridViewImageColumn dataGridViewImageColumn2 = new DataGridViewImageColumn();
            dataGridViewImageColumn2.HeaderText = "收據2";
            dataGridViewImageColumn2.ImageLayout = DataGridViewImageCellLayout.Zoom;
            dataGridView.Columns.Insert(12, dataGridViewImageColumn2);

            dataGridView.Columns["img2"].Visible = false;

            DataGridViewImageColumn delete = new DataGridViewImageColumn();
            delete.HeaderText = "Delete";
            delete.ImageLayout = DataGridViewImageCellLayout.Zoom;
            dataGridView.Columns.Insert(14, delete);
            for (int row = 0; row < dataGridView.Rows.Count; row++)
            {
                string type = records[row].Type;
                DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)dataGridView.Rows[row].Cells[8];
                cell.DataSource = DataModel.keyValuePairs[type];
                cell.Value = records[row].Detail;
                Bitmap bitmap = new Bitmap(records[row].Img1);
                dataGridView.Rows[row].Cells[10].Value = bitmap;
                Bitmap bitmap2 = new Bitmap(records[row].Img2);
                dataGridView.Rows[row].Cells[12].Value = bitmap2;
                Bitmap bitmap3 = new Bitmap(@"C:\Users\user\Desktop\campaigns\istockphoto-928418914-612x612.jpg");
                dataGridView.Rows[row].Cells[14].Value = bitmap3;

            }
            dataGridView.CellValueChanged += dataGridView_CellValueChanged;
            dataGridView.CurrentCellDirtyStateChanged += dataGridView_CurrentCellDirtyStateChanged;
        }

    }
}
