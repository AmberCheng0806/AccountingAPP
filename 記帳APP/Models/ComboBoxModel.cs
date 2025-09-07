using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 記帳APP.Models
{
    internal class ComboBoxModel
    {
        public string Key { get; set; }
        public int Value { get; set; }
        public ComboBoxModel(string key, int value)
        {
            Key = key;
            Value = value;
        }
    }
}
