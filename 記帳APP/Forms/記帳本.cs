using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using 記帳APP.Models.DTOs;
using 記帳APP.Presenter;
using 記帳APP.Repository.Entities;
using 記帳APP.Util;
using static 記帳APP.Contract.LedgerContract;
namespace 記帳APP.Forms
{
    [DisplayName("記帳本2")]
    [Order(2)]
    public partial class 記帳本 : Form, ILedgerView
    {
        BindingList<RecordModel> records = new BindingList<RecordModel>();
        Queue<Bitmap> bitmaps = new Queue<Bitmap>();
        ILedgerPresenter ledgerPresenter;

        public 記帳本()
        {
            InitializeComponent();
            ledgerPresenter = new LedgerPresenter(this);
            dataGridView.AllowUserToAddRows = false;

            //直行(Column)橫列(Row)
            //DataGridTextBoxColumn (單一欄位)
            //DataGridTextBoxCell (單一欄位下的儲存格)
            //dataGridView.Rows[3].Cells[2].Value = 0;
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            this.Debounce(() =>
            {
                ledgerPresenter.SearchByDate(dateTimePicker.Value.Date, dateTimePickerEnd.Value);
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
                ledgerPresenter.GetDetailResponse(comboBoxCell.Value.ToString());
            }
            RecordDTO dto = Util.Mapper.Map<RecordModel, RecordDTO>(records[e.RowIndex], x =>
            {
                x.ForMember(z => z.imgPath1, y => y.MapFrom(o => o.Img1));
                x.ForMember(z => z.imgPath2, y => y.MapFrom(o => o.Img2));
            });
            ledgerPresenter.UpdateData(dto);
        }
        public void DetailResponse(List<string> detail)
        {
            int row = dataGridView.CurrentCell.RowIndex;
            DataGridViewComboBoxCell detailCell = (DataGridViewComboBoxCell)dataGridView.Rows[row].Cells[dataGridView.Columns["DetailCombo"].Index];
            detailCell.DataSource = detail;
            detailCell.Value = detail[0];
        }
        private void dataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == dataGridView.Columns["Delete"].Index)
            {
                DialogResult result = MessageBox.Show("確認刪除", "系統通知", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                    return;
                dataGridView.Rows[e.RowIndex].Cells
                   .OfType<DataGridViewImageCell>()
                   .Where(x => x.Value != null)
                   .ToList()
                   .ForEach(x =>
                    {
                        var t = x.Value;
                        ((Bitmap)t).Dispose();
                        x.Dispose();
                    });

                var record = records[e.RowIndex];
                dataGridView.Rows.RemoveAt(e.RowIndex);


                RecordDTO dto = Mapper.Map<RecordModel, RecordDTO>(record, x =>
                {
                    x.ForMember(z => z.imgPath1, y => y.MapFrom(o => o.Img1));
                    x.ForMember(z => z.imgPath2, y => y.MapFrom(o => o.Img2));
                });
                ledgerPresenter.DeleteData(dto);
                return;

            }

            if (dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewImageCell imageCell)
            {
                string path = imageCell.Tag.ToString();
                ImgForm form = new ImgForm(path);
                form.Show();
            }
        }

        public void RenderDatas(List<RecordDTO> dtos)
        {
            List<RecordModel> records = Mapper.Map<RecordDTO, RecordModel>(dtos, x =>
            {
                x.ForMember(z => z.Img1, y => y.MapFrom(o => o.imgPath1));
                x.ForMember(z => z.Img2, y => y.MapFrom(o => o.imgPath2));
            }).ToList();
            this.records = new BindingList<RecordModel>(records);
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
            dataGridView.DataSource = this.records; // 透過反射將每一筆資料創建欄位
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            foreach (var prop in typeof(RecordModel).GetProperties())
            {
                var attribute = prop.GetCustomAttributes().FirstOrDefault();
                if (attribute == null)
                    continue;
                if (attribute is ComboBoxColumnAttribute)
                {
                    dataGridView.CreateComboBoxColumn(prop);
                }
                if (attribute is ImgColumnAttribute)
                {
                    dataGridView.CreateImageColumn(prop);
                }
            }

            DataGridViewImageColumn delete = new DataGridViewImageColumn()
            {
                HeaderText = "Delete",
                Name = "Delete",
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };
            Bitmap trash = new Bitmap(@"Img/trash.jpg");
            delete.DefaultCellStyle.NullValue = trash;
            bitmaps.Enqueue(trash);
            dataGridView.Columns.Add(delete);


            for (int i = 0; i < this.records.Count; i++)
            {
                var record = this.records[i];
                int DetailComboIndex = dataGridView.Columns["DetailCombo"].Index;

                var types = ledgerPresenter.GetDetailList(record.Type);
                DataGridViewComboBoxCell detailCell = (DataGridViewComboBoxCell)dataGridView.Rows[i].Cells[DetailComboIndex];
                detailCell.DataSource = types;

                dataGridView.Rows[i].Cells
                                    .OfType<DataGridViewImageCell>()
                                    .Where(x => x.OwningColumn.Name != "Delete")
                                    .ToList()
                                    .ForEach(y =>
                                    {
                                        string columnName = y.OwningColumn.Tag.ToString();
                                        string imagePath = dataGridView.Rows[i].Cells[columnName].Value.ToString();
                                        string originImgPath = imagePath.Replace("40x40", "");
                                        y.Tag = originImgPath;
                                        byte[] bytes = File.ReadAllBytes(imagePath);
                                        using (MemoryStream ms = new MemoryStream(bytes))
                                        {
                                            Bitmap bitmap = new Bitmap(ms); // 不再鎖住硬碟上的圖片檔案
                                            y.Value = new Bitmap(bitmap);   // 複製一份給 DataGridView，避免 MemoryStream 被釋放後還用到
                                            //bitmaps.Enqueue(bitmap);
                                        }

                                    });
            }
            dataGridView.CellValueChanged += dataGridView_CellValueChanged;
            dataGridView.CurrentCellDirtyStateChanged += dataGridView_CurrentCellDirtyStateChanged;
        }

    }
}
