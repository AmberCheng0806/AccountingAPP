using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using 記帳APP.Attributes;

namespace 記帳APP.Repository.Entities
{
    public class RecordEntity
    {
        public string Date { get; set; }
        public string Money { get; set; }
        [Record("類型")]
        public string Type { get; set; }
        [Record("對象")]
        public string People { get; set; }
        [Record("付款方式")]
        public string Pay { get; set; }
        [Record("類型細項")]
        public string Detail { get; set; }

        public string Img1 { get; set; }

        public string Img2 { get; set; }

        public RecordEntity(string date, string money, string type, string people, string pay, string datail, string img1, string img2)
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

        public RecordEntity() { }
    }
}
