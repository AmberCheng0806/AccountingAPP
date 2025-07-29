using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 記帳APP.Attributes;

namespace 記帳APP.Models
{
    public class RecordModel
    {
        [DisplayName("日期")]
        public string Date { get; set; }
        public string Money { get; set; }
        [ComboBoxColumn]
        public string Type { get; set; }
        [ComboBoxColumn]
        public string People { get; set; }
        [ComboBoxColumn]
        public string Pay { get; set; }
        [ComboBoxColumn]
        public string Detail { get; set; }
        [ImgColumn]
        public string Img1 { get; set; }
        [ImgColumn]
        public string Img2 { get; set; }

        public RecordModel(string date, string money, string type, string people, string pay, string datail, string img1, string img2)
        {
            Date = date;
            Money = money;
            Type = type;
            People = people;
            Pay = pay;
            Detail = datail;
            Img1 = img1;
            Img2 = img2;
        }

        public RecordModel() { }

    }
}
