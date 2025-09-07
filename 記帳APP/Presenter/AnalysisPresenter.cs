using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using 記帳APP.Attributes;
using 記帳APP.Models.DTOs;
using 記帳APP.Repository;
using 記帳APP.Repository.Entities;
using 記帳APP.Service;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static 記帳APP.Contract.AddRecordContract;
using static 記帳APP.Contract.AnalysisContract;

namespace 記帳APP.Presenter
{
    internal class AnalysisPresenter : IAnalysisPresenter
    {
        private IAnalysisView analysisView;
        private IExpenseOptionsRepository data;
        private IRecordRepository recordRepository;
        private DataAnalysisService dataAnalysisService;
        public AnalysisPresenter(IAnalysisView analysisView)
        {
            this.analysisView = analysisView;
            data = new ExpenseOptionsRepository();
            recordRepository = new RecordRepository();
            dataAnalysisService = new DataAnalysisService();
        }
        public void GetExpenseOptions()
        {
            var people = data.GetPeople();
            var pay = data.GetPayMethods();
            analysisView.CheckBoxesResponse(new AnalysisDTO(data.GetExpenseCategoryMap(), people, pay));
        }

        public void GetAnalysisData(DateTime start, DateTime end, List<string> groupByList, Dictionary<string, List<string>> keyValuePairs)
        {
            //List<RecordEntity> records = recordRepository.GetDatas(start, end);
            //var props = typeof(RecordEntity).GetProperties().Where(y =>
            //{
            //    var attribute = y.GetCustomAttribute<RecordAttribute>();
            //    if (attribute != null && keyValuePairs.ContainsKey(attribute.Name)) return true;
            //    return false;
            //}).ToList();
            //var filrerList = keyValuePairs.Values.SelectMany(a => a).ToList();

            ////將raw data 透過 dictionary條件篩List<RecordEntity> records
            //var result = records.Where(x =>
            //    //改版後的寫法all:集合內所有符合條件才回傳true
            //    //先將dict keys中的prop篩選出來(在這只剩確定有用到的prop) 再從prop中取出物件對應屬性的value值,如果所有篩選後的props都能與值符合
            //    //代表該筆資料可以被篩選出來
            //    props.All(y => filrerList.Contains(y.GetValue(x).ToString()))

            ////原版foreach寫法:先將使用者傳入的條件(現金/交通/信用卡/父母) 從dict裡面找出對應的prop,再透過反射檢查dict.values內有沒有任一筆資料符合條件
            ////符合條件就取出來
            ////foreach (var keyValuePair in keyValuePairs)
            ////{
            ////    // 找對應中文prop
            ////    var prop = props
            ////                .First(p => p.GetCustomAttribute<RecordAttribute>().Name == keyValuePair.Key);
            ////    // 檢查值是否在dictionary
            ////    var value = prop.GetValue(x).ToString();
            ////    if (!keyValuePair.Value.Contains(value))
            ////        return false;
            ////}
            ////return true;
            //);


            ////從原本四筆props中根據groupByList的內容篩選出指定要group by的props
            //var propName = typeof(RecordEntity).GetProperties()
            //     .Where(x => groupByList.Contains(x.GetCustomAttribute<RecordAttribute>()?.Name))
            //     .ToList();
            ////gropuby
            //var groupByResult = result.GroupBy(x =>
            //{
            //    // 將每一筆prop的屬性都轉成要群組的值,並用string.join 串成字串作為群組的key
            //    var list = propName.Select(y => y.GetValue(x).ToString());
            //    return string.Join(",", list);

            //    // 原版寫法
            //    //string key = "";
            //    //foreach (var item in groupByList)
            //    //{
            //    //    var prop = props.Where(z => z.GetCustomAttribute<RecordAttribute>().Name == item).First();
            //    //    key = key + prop.GetValue(x) + ",";
            //    //}
            //    //key = key.Remove(key.Length - 1);
            //    //return key;
            //}).ToList();

            var groupByResult = dataAnalysisService.GetAnalysisData(start, end, groupByList, keyValuePairs);

            //dto
            List<AnalysisDataDTO> dtos = groupByResult.Select(x => new AnalysisDataDTO
            {
                Key = x.Key,
                Money = x.Sum(y => int.Parse(y.Money)).ToString()
            }).ToList();
            analysisView.RenderAnalysisData(dtos);
        }
    }
}
