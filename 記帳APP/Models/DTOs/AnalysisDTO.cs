using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 記帳APP.Models.DTOs
{
    public class AnalysisDTO
    {
        public Dictionary<string, List<string>> KeyValuePairs { get; set; }
        public List<string> People { get; set; }
        public List<string> Pay { get; set; }
        public AnalysisDTO(Dictionary<string, List<string>> keyValuePairs, List<string> people, List<string> pay)
        {
            KeyValuePairs = keyValuePairs;
            People = people;
            Pay = pay;
        }

        public Dictionary<string, List<string>> InitialAnalysisTypes()
        {
            return new Dictionary<string, List<string>>
            {
                {"類型",KeyValuePairs.Keys.ToList()} ,
                {"類型細項",KeyValuePairs.First().Value },
                {"對象",People },
                {"付款方式", Pay }
            };
        }
    }
}
