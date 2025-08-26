using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 記帳APP.Attributes
{
    internal class RecordAttribute : Attribute
    {
        public string Name { get; set; }
        public RecordAttribute(string name)
        {
            Name = name;
        }
    }
}
