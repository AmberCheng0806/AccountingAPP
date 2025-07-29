using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 記帳APP.Models;

namespace 記帳APP.Repository
{
    internal class DataRepository : IDataRepository
    {
        public List<string> GetDetails(string type)
        {
            return DataModel.keyValuePairs[type];
        }

        public List<string> GetPayMethods()
        {
            return DataModel.Pay;
        }

        public List<string> GetPeople()
        {
            return DataModel.People;
        }

        public List<string> GetTypes()
        {
            return DataModel.Type;
        }
    }
}
