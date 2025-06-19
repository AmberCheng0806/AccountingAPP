using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 記帳APP.Models
{
    internal class RecordModel
    {
        public string Date { get; set; }
        public string Money { get; set; }
        public string Type { get; set; }
        public string People { get; set; }
        public string Pay { get; set; }
        public string Detail { get; set; }

        public string Img1 { get; set; }

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


    }
}
