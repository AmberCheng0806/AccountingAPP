using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 記帳APP.Models.DTOs
{
    public class PieChartDataDTO
    {
        public List<string> Key { get; set; }
        public List<string> Money { get; set; }
        public PieChartDataDTO(List<string> key, List<string> money)
        {
            Key = key;
            Money = money;
        }
    }
}
