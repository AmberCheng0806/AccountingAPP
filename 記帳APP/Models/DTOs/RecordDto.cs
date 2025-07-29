using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Windows.Forms;

namespace 記帳APP.Models.DTOs
{
    public class RecordDto
    {
        public string date { get; set; }
        public string money { get; set; }
        public string type { get; set; }
        public string people { get; set; }
        public string pay { get; set; }
        public string detail { get; set; }
        public string imgPath1 { get; set; }
        public string imgPath2 { get; set; }
        public RecordDto(string date, string money, string type, string people, string pay, string detail, string imgPath1, string imgPath2)
        {
            this.date = date;
            this.money = money;
            this.type = type;
            this.people = people;
            this.pay = pay;
            this.detail = detail;
            this.imgPath1 = imgPath1;
            this.imgPath2 = imgPath2;
        }
    }
}
