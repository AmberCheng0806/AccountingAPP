using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using 記帳APP.Models;
using 記帳APP.Models.DTOs;
using 記帳APP.Repository;
using 記帳APP.Repository.Entities;
using static 記帳APP.Contract.LedgerContract;

namespace 記帳APP.Presenter
{
    internal class LedgerPresenter : ILedgerPresenter
    {
        private ILedgerView view;
        private IExpenseOptionsRepository data;
        private IRecordRepository record;
        public LedgerPresenter(ILedgerView view)
        {
            this.view = view;
            data = new ExpenseOptionsRepository();
            record = new RecordRepository();
        }

        public void DeleteData(RecordDTO recordDto)
        {
            RecordEntity entity = Util.Mapper.Map<RecordDTO, RecordEntity>(recordDto, x =>
            {
                x.ForMember(z => z.Img1, y => y.MapFrom(o => o.imgPath1));
                x.ForMember(z => z.Img2, y => y.MapFrom(o => o.imgPath2));
            });
            record.Delete(entity);
        }

        public List<string> GetDetailList(string type)
        {
            return data.GetDetails(type);
        }

        public void GetDetailResponse(string type)
        {
            List<string> details = data.GetDetails(type);
            view.DetailResponse(details);
        }

        public PropertyInfo[] GetRecordProperties()
        {
            return typeof(RecordModel).GetProperties();
        }

        public void SearchByDate(DateTime start, DateTime end)
        {
            List<RecordEntity> records = record.GetDatas(start, end);
            List<RecordDTO> dtos = Util.Mapper.Map<RecordEntity, RecordDTO>(records, x =>
            {
                x.ForMember(z => z.imgPath1, y => y.MapFrom(o => o.Img1));
                x.ForMember(z => z.imgPath2, y => y.MapFrom(o => o.Img2));
            }).ToList();
            view.RenderDatas(dtos);
        }

        public void UpdateData(RecordDTO recordDto)
        {
            RecordEntity entity = Util.Mapper.Map<RecordDTO, RecordEntity>(recordDto, x =>
            {
                x.ForMember(z => z.Img1, y => y.MapFrom(o => o.imgPath1));
                x.ForMember(z => z.Img2, y => y.MapFrom(o => o.imgPath2));
            });
            record.Update(entity);
        }


    }
}
