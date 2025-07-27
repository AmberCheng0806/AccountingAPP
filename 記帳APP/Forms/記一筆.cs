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
using 記帳APP.Util;
using static System.Net.Mime.MediaTypeNames;
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
            payCombo.DataSource = DataModel.Pay;

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
            openFileDialog.InitialDirectory = @"C:\Users\user\Desktop\";
            openFileDialog.Filter = "圖片檔|*.jpg;*.png";
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                string originPath = openFileDialog.FileName;
                pictureBox.Image = System.Drawing.Image.FromFile(originPath);
                pictureBox.Tag = originPath;
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

                CompressImg(pictureBox1.Image, pictureBox1.Tag.ToString(), imgPath1);
                CompressImg(pictureBox2.Image, pictureBox2.Tag.ToString(), imgPath2);

                RecordModel recordModel = new RecordModel(date, money, type, people, pay, detail, imgPath1, imgPath2);
                string path = Path.Combine(FilePath, $"{date}\\record.csv");
                CSV_Library.CSVHelper.Write<RecordModel>(path, recordModel);
            }, 400);
        }

        private void CompressImg(System.Drawing.Image image, string originPath, string path)
        {
            ImageCodecInfo codec = ImageCodecInfo.GetImageEncoders()
                .FirstOrDefault(x => x.FormatID == ImageFormat.Jpeg.Guid);
            EncoderParameters encoderParams = new EncoderParameters(1);
            FileInfo fileInfo = new FileInfo(originPath);
            long quality = fileInfo.Length < 1000000 ? 50L : 5L;
            encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            image.Save(path.Replace("40x40", ""), codec, encoderParams);

            Bitmap bitmap = new Bitmap(40, 40);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(image, 0, 0, 40, 40);
            }
            bitmap.Save(path, ImageFormat.Jpeg);
        }
    }
}
