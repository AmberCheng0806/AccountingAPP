using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 記帳APP.Models.DTOs;

namespace 記帳APP.Contract
{
    internal class AddRecordContract
    {
        public interface IAddRecordView
        {
            void ComboboxResponse(DataDto dataDto);
            void DetailResponse(List<string> detail);
            void Initialize();
        }

        public interface IAddRecordPresenter
        {
            void GetComboboxList();
            void GetDetailResponse(string type);
            void AddRecord(RecordDto recordDto);
            void CompressImg(Image image, string originPath, string path);
        }
    }
}
