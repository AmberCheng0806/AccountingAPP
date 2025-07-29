using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 記帳APP.Models;

namespace 記帳APP.Contract
{
    internal class LedgerContract
    {
        public interface ILedgerPresenter
        {
            void SearchByDate(DateTime start, DateTime end);
            void UpdateData(RecordModel recordModel);

            void DeleteData(RecordModel recordModel);
        }

        public interface ILedgerView
        {
            void RenderDatas(List<RecordModel> records);
        }
    }
}
