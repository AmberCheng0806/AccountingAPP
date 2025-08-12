using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using 記帳APP.Models;
using 記帳APP.Models.DTOs;

namespace 記帳APP.Contract
{
    internal class LedgerContract
    {
        public interface ILedgerPresenter
        {
            void SearchByDate(DateTime start, DateTime end);
            void UpdateData(RecordDTO recordDto);

            void DeleteData(RecordDTO recordDto);

            void GetDetailResponse(string type);
            List<string> GetDetailList(string type);

            PropertyInfo[] GetRecordProperties();
        }

        public interface ILedgerView
        {
            void RenderDatas(List<RecordDTO> dtos);
            void DetailResponse(List<string> detail);
        }
    }
}
