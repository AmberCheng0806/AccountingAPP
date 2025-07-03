using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 記帳APP.Forms
{
    public partial class ImgForm : Form
    {

        public ImgForm(string path)
        {
            InitializeComponent();

            pictureBox1.Image = Image.FromFile(path);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
