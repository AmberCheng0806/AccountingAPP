using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 記帳APP.Models
{
    public class ChartComboBoxModel
    {
        public List<string> dropDownSelections { get; set; }
        public List<ComboBoxModel> numberSelections = new List<ComboBoxModel>()
        {
            new ComboBoxModel("當月資料",1),
            new ComboBoxModel("與上個月相比",2),
            new ComboBoxModel("與兩個月前相比",3)
        };

        public string GetNumberSelectionName(int i)
        {
            return numberSelections[i].Key;
        }
    }
}
