using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 記帳APP.Models.DTOs
{
    public class LineChartDataDTO
    {
        public List<string> Date { get; set; }
        public List<Dictionary<string, List<string>>> MoneyListForMonth { get; set; }
        public LineChartDataDTO(List<string> date, List<Dictionary<string, List<string>>> moneyListForMonth)
        {
            Date = date;
            MoneyListForMonth = moneyListForMonth;
        }
    }
}
