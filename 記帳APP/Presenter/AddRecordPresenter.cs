using AutoMapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 記帳APP.Models;
using 記帳APP.Models.DTOs;
using 記帳APP.Repository;
using 記帳APP.Util;
using static 記帳APP.Contract.AddRecordContract;
namespace 記帳APP.Presenter
{
    internal class AddRecordPresenter : IAddRecordPresenter
    {
        private IAddRecordView addRecordView;
        private IDataRepository data;
        private IRecordRepository record;

        public AddRecordPresenter(IAddRecordView view)
        {
            addRecordView = view;
            data = new DataRepository();
            record = new RecordRepository();
        }

        public void AddRecord(RecordDto recordDto)
        {
            //var config = new MapperConfiguration(cfg =>
            //    cfg.CreateMap<RecordDto, RecordModel>()
            //.ForMember(x => x.Img1, y => y.MapFrom(o => o.imgPath1))
            //.ForMember(x => x.Img2, y => y.MapFrom(o => o.imgPath2))
            //);
            //var mapper = config.CreateMapper();
            //RecordModel model = mapper.Map<RecordModel>(recordDto);
            RecordModel model = Util.Mapper.Map<RecordDto, RecordModel>(recordDto, x =>
            {
                x.ForMember(z => z.Img1, y => y.MapFrom(o => o.imgPath1));
                x.ForMember(z => z.Img2, y => y.MapFrom(o => o.imgPath2));
            });
            record.Insert(model);
            addRecordView.Initialize();
        }

        public void CompressImg(Image image, string originPath, string path)
        {
            Bitmap compress = ImgCompress.Compress(image, originPath, path);
            compress.Save(path.Replace("40x40", ""));
            compress.Dispose();

            Bitmap bitmap = ImgCompress.Compress(image, 40, 40);
            bitmap.Save(path, ImageFormat.Jpeg);
            bitmap.Dispose();

            image.Dispose();
        }

        public void GetComboboxList()
        {
            var types = data.GetTypes();
            var details = data.GetDetails(types[0]);
            var people = data.GetPeople();
            var pay = data.GetPayMethods();
            addRecordView.ComboboxResponse(new DataDto(types, details, people, pay));
        }

        public void GetDetailResponse(string type)
        {
            var details = data.GetDetails(type);
            addRecordView.DetailResponse(details);
        }
    }
}
