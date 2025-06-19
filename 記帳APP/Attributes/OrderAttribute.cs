using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 記帳APP.Attributes
{
    internal class OrderAttribute : Attribute
    {
        public int num { get; set; }
        public OrderAttribute(int number) { num = number; }
    }
}
