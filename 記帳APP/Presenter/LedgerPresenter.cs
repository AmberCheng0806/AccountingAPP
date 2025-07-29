using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 記帳APP.Models;
using 記帳APP.Repository;
using static 記帳APP.Contract.LedgerContract;

namespace 記帳APP.Presenter
{
    internal class LedgerPresenter : ILedgerPresenter
    {
        private ILedgerView view;
        private IDataRepository data;
        private IRecordRepository record;
        public LedgerPresenter(ILedgerView view)
        {
            this.view = view;
            data = new DataRepository();
            record = new RecordRepository();
        }

        public void DeleteData(RecordModel recordModel)
        {
            List<RecordModel> recordModels = record.Delete(recordModel);
            view.RenderDatas(recordModels);
        }

        public void SearchByDate(DateTime start, DateTime end)
        {
            List<RecordModel> records = record.GetDatas(start, end);
            view.RenderDatas(records);
        }

        public void UpdateData(RecordModel recordModel)
        {
            record.Update(recordModel);
        }


    }
}
