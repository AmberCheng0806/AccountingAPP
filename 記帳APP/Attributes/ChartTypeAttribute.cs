using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 記帳APP.Attributes
{
    internal class ChartTypeAttribute : Attribute
    {
        public string ChartName { get; }
        public ChartTypeAttribute(string name)
        {
            ChartName = name;
        }

    }
}
