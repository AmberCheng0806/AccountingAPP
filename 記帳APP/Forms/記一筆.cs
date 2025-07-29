using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 記帳APP.Attributes;
using 記帳APP.Models;
using 記帳APP.Models.DTOs;
using 記帳APP.Presenter;
using 記帳APP.Util;
using static 記帳APP.Contract.AddRecordContract;
namespace 記帳APP.Forms
{
    [DisplayName("記一筆2")]
    [Order(1)]
    public partial class 記一筆 : Form, IAddRecordView
    {
        private IAddRecordPresenter presenter;
        public 記一筆()
        {
            InitializeComponent();
            presenter = new AddRecordPresenter(this);
            presenter.GetComboboxList();
            Initialize();
        }

        public void Initialize()
        {
            moneyBox1.Text = "";
            typeCombo.SelectedIndex = 0;
            peopleCombo.SelectedIndex = 0;
            payCombo.SelectedIndex = 0;
            string path = "D:\\photo\\circle-upload-icon-button-isolated-on-white-background-vector.jpg";
            pictureBox1.Image = Image.FromFile(path);
            pictureBox1.Tag = path;
            pictureBox2.Image = Image.FromFile(path);
            pictureBox2.Tag = path;
        }

        public void ComboboxResponse(DataDto dataDto)
        {
            typeCombo.DataSource = dataDto.types;
            detailCombo.DataSource = dataDto.details;
            peopleCombo.DataSource = dataDto.people;
            payCombo.DataSource = dataDto.pays;
        }

        public void DetailResponse(List<string> detail)
        {
            detailCombo.DataSource = detail;
        }

        private void 記一筆_Load(object sender, EventArgs e)
        {
            //typeCombo.DataSource = DataModel.Type;
            //detailCombo.DataSource = DataModel.keyValuePairs["食"];
            //peopleCombo.DataSource = DataModel.People;
            //payCombo.DataSource = DataModel.Pay;

        }

        private void typeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = typeCombo.Text;
            presenter.GetDetailResponse(type);
            //detailCombo.DataSource = DataModel.keyValuePairs[type];
        }

        private void ImageUpload_Click(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"C:\Users\user\Desktop\";
            openFileDialog.Filter = "圖片檔|*.jpg;*.png";
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                string originPath = openFileDialog.FileName;
                pictureBox.Image = System.Drawing.Image.FromFile(originPath);
                pictureBox.Tag = originPath;
            }
        }

        private void AddNewOne_Click(object sender, EventArgs e)
        {
            //Entity Framework => 用LINQ 操作資料庫
            //ORM(Object Relaction Mapping) => 使用物件導向的方式來操作/管理資料
            this.Debounce(() =>
            {
                string date = dateTimePicker.Value.ToString("yyyy-MM-dd");
                string money = moneyBox1.Text;
                string type = typeCombo.Text.Trim();
                string people = peopleCombo.Text.Trim();
                string pay = payCombo.Text.Trim();
                string detail = detailCombo.Text.Trim();
                string FilePath = ConfigurationManager.AppSettings["FilePath"];
                string folder = Path.Combine(FilePath, date);
                Directory.CreateDirectory(folder);
                string imgPath1 = Path.Combine(folder, $"40x40{Guid.NewGuid().ToString()}.jpg");
                string imgPath2 = Path.Combine(folder, $"40x40{Guid.NewGuid().ToString()}.jpg");

                presenter.CompressImg(pictureBox1.Image, pictureBox1.Tag.ToString(), imgPath1);
                presenter.CompressImg(pictureBox2.Image, pictureBox2.Tag.ToString(), imgPath2);
                presenter.AddRecord(new RecordDto(date, money, type, people, pay, detail, imgPath1, imgPath2));
            }, 400);
        }

    }
}
