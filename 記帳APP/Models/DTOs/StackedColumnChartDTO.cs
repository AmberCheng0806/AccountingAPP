using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 記帳APP.Models.DTOs
{
    public class StackedColumnChartDTO
    {
        public List<string> Date { get; set; }
        public List<string> Names { get; set; }
        public List<Dictionary<string, string>> YDatas { get; set; }

        public List<string> totalPerDate { get; set; }
        public StackedColumnChartDTO(List<string> date, List<string> names, List<Dictionary<string, string>> yDatas, List<string> totalPerDate)
        {
            Date = date;
            Names = names;
            YDatas = yDatas;
            this.totalPerDate = totalPerDate;
        }
    }
}
