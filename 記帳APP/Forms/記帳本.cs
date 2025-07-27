using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 記帳APP.Attributes;
using 記帳APP.Models;
using 記帳APP.Util;

namespace 記帳APP.Forms
{
    [DisplayName("記帳本2")]
    [Order(2)]
    public partial class 記帳本 : Form
    {
        List<RecordModel> records = new List<RecordModel>();
        Queue<Bitmap> bitmaps = new Queue<Bitmap>();

        public 記帳本()
        {
            InitializeComponent();
            //直行(Column)橫列(Row)
            //DataGridTextBoxColumn (單一欄位)
            //DataGridTextBoxCell (單一欄位下的儲存格)
            //dataGridView.Rows[3].Cells[2].Value = 0;
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            this.Debounce(() =>
            {
                ChangeRecordsByDate();
                Reload();
            }, 400);
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
            if (dataGridView.CurrentCell.ColumnIndex == dataGridView.Columns["TypeCombo"].Index && dataGridView.CurrentCell is DataGridViewComboBoxCell comboBoxCell)
            {
                var types = DataModel.keyValuePairs[comboBoxCell.Value.ToString()];
                DataGridViewComboBoxCell detailCell = (DataGridViewComboBoxCell)dataGridView.Rows[e.RowIndex].Cells[dataGridView.Columns["DetailCombo"].Index];
                detailCell.DataSource = types;
                detailCell.Value = types[0];
            }
            Reload();
            string date = records[e.RowIndex].Date;
            string FilePath = ConfigurationManager.AppSettings["FilePath"];
            string path = Path.Combine(FilePath, $"{date}\\record.csv");
            File.Delete(path);
            CSV_Library.CSVHelper.Write<RecordModel>(path, records);
        }

        private void dataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView.Columns["Img1ImageColumn"].Index || e.ColumnIndex == dataGridView.Columns["Img2ImageColumn"].Index)
            {
                string path = dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value.ToString();
                ImgForm form = new ImgForm(path.Replace("40x40", ""));
                form.Show();
            }
            if (e.ColumnIndex == dataGridView.Columns["Delete"].Index)
            {
                DialogResult result = MessageBox.Show("確認刪除", "系統通知", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    return;
                }
                string img1 = records[e.RowIndex].Img1;
                string img2 = records[e.RowIndex].Img2;
                string date = records[e.RowIndex].Date;
                var temp = dataGridView.Rows[e.RowIndex].Cells.OfType<DataGridViewImageCell>().Where(x => x.Value != null).ToList();
                temp.ForEach(x =>
                {
                    var t = x.Value;
                    ((Bitmap)t).Dispose();
                });
                records.RemoveAt(e.RowIndex);
                dataGridView.DataSource = null;
                dataGridView.Columns.Clear();
                GC.Collect();
                File.Delete(img1);
                File.Delete(img2);
                string FilePath = ConfigurationManager.AppSettings["FilePath"];
                string path = Path.Combine(FilePath, $"{date}\\record.csv");
                File.Delete(path);
                List<RecordModel> recordForDeleteDate = records.Where(x => x.Date == date).ToList();
                CSV_Library.CSVHelper.Write<RecordModel>(path, recordForDeleteDate);
                Reload();
            }
        }

        private void Reload()
        {

            dataGridView.CellValueChanged -= dataGridView_CellValueChanged;
            dataGridView.CurrentCellDirtyStateChanged -= dataGridView_CurrentCellDirtyStateChanged;

            while (bitmaps.Count != 0)
            {
                Bitmap bitmap = bitmaps.Dequeue();
                bitmap.Dispose();
            }
            dataGridView.DataSource = null;
            dataGridView.Columns.Clear();
            GC.Collect();
            dataGridView.DataSource = records; // 透過反射將每一筆資料創建欄位
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            foreach (var prop in typeof(RecordModel).GetProperties())
            {
                var attribute = prop.GetCustomAttributes().FirstOrDefault();
                if (attribute == null)
                    continue;
                if (attribute is ComboBoxColumnAttribute)
                {
                    DataGridViewComboBoxColumn ComboBoxColumn = new DataGridViewComboBoxColumn();
                    ComboBoxColumn.HeaderText = prop.Name;
                    ComboBoxColumn.DataPropertyName = prop.Name;
                    ComboBoxColumn.Name = prop.Name + "Combo";
                    ComboBoxColumn.Tag = prop.Name;
                    ComboBoxColumn.DataSource = prop.Name == "Detail" ?
                        null : typeof(DataModel).GetField(prop.Name, BindingFlags.Public | BindingFlags.Static).GetValue(null);
                    int index = dataGridView.Columns[prop.Name].Index;
                    dataGridView.Columns.Insert(index, ComboBoxColumn);
                    dataGridView.Columns[prop.Name].Visible = false;
                }
                if (attribute is ImgColumnAttribute)
                {
                    DataGridViewImageColumn imgColumn = new DataGridViewImageColumn();
                    imgColumn.HeaderText = prop.Name;
                    imgColumn.Name = prop.Name + "ImageColumn";
                    imgColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
                    imgColumn.Tag = prop.Name;
                    //imgColumn.DefaultCellStyle
                    int index = dataGridView.Columns[prop.Name].Index;
                    dataGridView.Columns.Insert(index, imgColumn);
                    dataGridView.Columns[prop.Name].Visible = false;
                }

            }

            DataGridViewImageColumn delete = new DataGridViewImageColumn();
            delete.HeaderText = "Delete";
            delete.Name = "Delete";
            delete.ImageLayout = DataGridViewImageCellLayout.Zoom;
            delete.DefaultCellStyle.NullValue = new Bitmap(@"Img/trash.jpg");
            dataGridView.Columns.Add(delete);

            //for (int i = 0; i < dataGridView.Rows.Count; i++)
            //{
            //    detail Datasource
            //    int TypeIndex = dataGridView.Columns["Type"].Index;
            //    var types = DataModel.keyValuePairs[dataGridView.Rows[i].Cells[TypeIndex].Value.ToString()];
            //    int DetailComboIndex = dataGridView.Columns["Detail"].Index - 1;
            //    DataGridViewComboBoxCell detailCell = (DataGridViewComboBoxCell)dataGridView.Rows[i].Cells[DetailComboIndex];
            //    detailCell.DataSource = types;

            //    img path:去掉imgagecolumn用反射
            //    int imgIndex = dataGridView.Columns["Img1"].Index;
            //    Bitmap bitmap = new Bitmap(dataGridView.Rows[i].Cells[imgIndex].Value.ToString());
            //    bitmaps.Enqueue(bitmap);
            //    dataGridView.Rows[i].Cells[imgIndex - 1].Value = bitmap;
            //    int imgIndex2 = dataGridView.Columns["Img2"].Index;
            //    Bitmap bitmap2 = new Bitmap(dataGridView.Rows[i].Cells[imgIndex2].Value.ToString());
            //    bitmaps.Enqueue(bitmap2);
            //    dataGridView.Rows[i].Cells[imgIndex2 - 1].Value = bitmap2;
            //}

            //
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                var record = records[i];
                //detail Datasource
                int DetailComboIndex = dataGridView.Columns["DetailCombo"].Index;
                var types = DataModel.keyValuePairs[record.Type];
                DataGridViewComboBoxCell detailCell = (DataGridViewComboBoxCell)dataGridView.Rows[i].Cells[DetailComboIndex];
                detailCell.DataSource = types;

                dataGridView.Rows[i].Cells
                                    .OfType<DataGridViewImageCell>()
                                    .Where(x => x.OwningColumn.Name != "Delete")
                                    .ToList()
                                    .ForEach(y =>
                                    {
                                        string columnName = y.OwningColumn.Tag.ToString();
                                        string value = dataGridView.Rows[i].Cells[columnName].Value.ToString();
                                        Bitmap bitmap = new Bitmap(value);
                                        y.Value = bitmap;
                                        bitmaps.Enqueue(bitmap);
                                    });
            }
            dataGridView.CellValueChanged += dataGridView_CellValueChanged;
            dataGridView.CurrentCellDirtyStateChanged += dataGridView_CurrentCellDirtyStateChanged;
        }

        private void dateTimePicker_ValueChangd(object sender, EventArgs e)
        {
            ChangeRecordsByDate();
        }

        private void ChangeRecordsByDate()
        {
            records.Clear();
            if (dateTimePicker.Value.Date > dateTimePickerEnd.Value.Date)
            {
                MessageBox.Show("開始日期不能大於結束日期", "系統通知");
                return;
            }
            TimeSpan timeSpan = dateTimePickerEnd.Value - dateTimePicker.Value;
            //dateTimePicker.Value.AddDays(timeSpan.Days);
            for (int i = 0; i <= timeSpan.Days; i++)
            {
                string date = dateTimePicker.Value.AddDays(i).ToString("yyyy-MM-dd");
                string FilePath = ConfigurationManager.AppSettings["FilePath"];
                string path = Path.Combine(FilePath, $"{date}\\record.csv");
                if (!File.Exists(path))
                {
                    continue;
                }
                List<RecordModel> recordByDay = CSV_Library.CSVHelper.Read<RecordModel>(path);
                records.AddRange(recordByDay);
            }
        }

    }
}
