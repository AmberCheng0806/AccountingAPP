using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 記帳APP.Attributes;

namespace 記帳APP.Forms
{
    [DisplayName("記帳本2")]
    [Order(2)]
    public partial class 記帳本 : Form
    {
        public 記帳本()
        {
            InitializeComponent();
        }

        private void 記帳本_Load(object sender, EventArgs e)
        {

        }
    }
}
