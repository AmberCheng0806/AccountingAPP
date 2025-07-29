using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 記帳APP.Models;

namespace 記帳APP.Repository
{
    internal interface IRecordRepository
    {
        void Insert(RecordModel record);
        List<RecordModel> GetDatas(DateTime start, DateTime end);
        List<RecordModel> GetDatas(DateTime date);
        List<RecordModel> Delete(RecordModel record);
        void Update(RecordModel record);
    }
}
