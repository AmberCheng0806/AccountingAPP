using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 記帳APP.Models;

namespace 記帳APP.Repository
{
    internal class ExpenseOptionsRepository : IExpenseOptionsRepository
    {

        public Dictionary<string, List<string>> GetExpenseCategoryMap()
        {
            return ExpenseOptions.ExpenseCategoryMap;
        }
        public List<string> GetTypes()
        {
            return ExpenseOptions.Type.ToList();
        }
        public List<string> GetDetails(string type)
        {
            return ExpenseOptions.ExpenseCategoryMap[type].ToList();
        }

        public List<string> GetPayMethods()
        {
            return ExpenseOptions.Pay.ToList();
        }

        public List<string> GetPeople()
        {
            return ExpenseOptions.People.ToList();
        }

    }
}
