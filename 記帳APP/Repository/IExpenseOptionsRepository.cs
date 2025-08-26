using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 記帳APP.Repository
{
    internal interface IExpenseOptionsRepository
    {
        Dictionary<string, List<string>> GetExpenseCategoryMap();
        List<string> GetTypes();
        List<string> GetDetails(string type);
        List<string> GetPeople();
        List<string> GetPayMethods();
    }
}
