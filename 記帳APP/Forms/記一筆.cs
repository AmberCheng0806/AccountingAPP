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
    [DisplayName("記一筆2")]
    [Order(1)]
    public partial class 記一筆 : Form
    {
        public 記一筆()
        {
            InitializeComponent();
            Console.WriteLine("form:" + this.Width);
        }

        private void 記一筆_Load(object sender, EventArgs e)
        {
            //Console.WriteLine("userControl11:" + userControl11.Width);
            Console.WriteLine("form:" + this.Width);
            typeCombo.DataSource = DataModel.Type;
            detailCombo.DataSource = DataModel.keyValuePairs["食"];
            peopleCombo.DataSource = DataModel.People;
            payCombo.DataSource = DataModel.PaymentType;

        }

        private void typeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = typeCombo.Text;
            detailCombo.DataSource = DataModel.keyValuePairs[type];
        }

        private void ImageUpload_Click(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"C:\Users\user\Desktop\campaigns";
            openFileDialog.Filter = "圖片檔|*.jpg;*.png";
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                string originPath = openFileDialog.FileName;
                pictureBox.Image = Image.FromFile(originPath);
                //string imgName = originPath.Split('\\').Last();
                //string newPath = "C:\\Users\\user\\source\\repos\\記帳APP\\記帳APP\\Img\\" + imgName;
                //// pictureBox.Image.Save(filepath)
                //File.Copy(originPath, newPath, true);
                //if (pictureBox == pictureBox1) { imgPath1 = newPath; }
                //else { imgPath2 = newPath; }
            }
        }

        private void AddNewOne_Click(object sender, EventArgs e)
        {
            //Entity Framework => 用LINQ 操作資料庫
            //ORM(Object Relaction Mapping) => 使用物件導向的方式來操作/管理資料


            string date = dateTimePicker.Value.ToString("yyyy-MM-dd");
            string money = moneyBox1.Text;
            string type = typeCombo.Text.Trim();
            string people = peopleCombo.Text.Trim();
            string pay = payCombo.Text.Trim();
            string detail = detailCombo.Text.Trim();
            string imgPath1 = $"C:\\Users\\user\\source\\repos\\記帳APP\\記帳APP\\Img\\{Guid.NewGuid().ToString()}.jpg";
            string imgPath2 = $"C:\\Users\\user\\source\\repos\\記帳APP\\記帳APP\\Img\\{Guid.NewGuid().ToString()}.jpg";
            pictureBox1.Image.Save(imgPath1);
            pictureBox2.Image.Save(imgPath2);
            RecordModel recordModel = new RecordModel(date, money, type, people, pay, detail, imgPath1, imgPath2);
            CSV_Library.CSVHelper.Write<RecordModel>("C:\\Users\\user\\Desktop\\campaigns\\record.csv", recordModel);

        }
    }
}
