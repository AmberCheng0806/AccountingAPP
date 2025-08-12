using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 記帳APP.Models.DTOs
{
    public class DataDTO
    {
        public List<string> types { get; set; }
        public List<string> details { get; set; }
        public List<string> people { get; set; }
        public List<string> pays { get; set; }

        public DataDTO(List<string> types, List<string> details, List<string> people, List<string> pays)
        {
            this.types = types;
            this.details = details;
            this.people = people;
            this.pays = pays;
        }
    }
}
