using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 記帳APP.Models;
using 記帳APP.Repository.Entities;

namespace 記帳APP.Repository
{
    internal interface IRecordRepository
    {
        void Insert(RecordEntity record);
        List<RecordEntity> GetDatas(DateTime start, DateTime end);
        List<RecordEntity> GetDatas(DateTime date);
        void Delete(RecordEntity record);
        void Update(RecordEntity record);
    }
}
