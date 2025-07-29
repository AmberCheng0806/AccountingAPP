using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 記帳APP.Models;

namespace 記帳APP.Repository
{
    internal class RecordRepository : IRecordRepository
    {
        private string FilePath;
        public RecordRepository()
        {
            FilePath = ConfigurationManager.AppSettings["FilePath"];
        }
        public List<RecordModel> Delete(RecordModel record)
        {
            List<RecordModel> records = GetDatas(DateTime.Parse(record.Date));
            records.RemoveAll(x => x.Img1 == record.Img1);
            File.Delete(record.Img1);
            File.Delete(record.Img2);
            string path = Path.Combine(FilePath, record.Date, "record.csv");
            File.Delete(path);
            CSV_Library.CSVHelper.Write<List<RecordModel>>(path, records);
            return records;
        }

        public List<RecordModel> GetDatas(DateTime date)
        {
            string path = Path.Combine(FilePath, date.ToString("yyyy-MM-dd"), "record.csv");
            return CSV_Library.CSVHelper.Read<RecordModel>(path);
        }

        public List<RecordModel> GetDatas(DateTime start, DateTime end)
        {
            List<RecordModel> records = new List<RecordModel>();
            TimeSpan timeSpan = end - start;
            for (int i = 0; i <= timeSpan.Days; i++)
            {
                string date = start.AddDays(i).ToString("yyyy-MM-dd");
                string path = Path.Combine(FilePath, date, "record.csv");
                if (!File.Exists(path))
                {
                    continue;
                }
                List<RecordModel> recordByDay = CSV_Library.CSVHelper.Read<RecordModel>(path);
                records.AddRange(recordByDay);
            }
            return records;
        }

        public void Insert(RecordModel record)
        {
            string folder = Path.Combine(FilePath, record.Date);
            Directory.CreateDirectory(folder);
            string path = Path.Combine(folder, "record.csv");
            CSV_Library.CSVHelper.Write<RecordModel>(path, record);
        }

        public void Update(RecordModel record)
        {
            List<RecordModel> records = GetDatas(DateTime.Parse(record.Date));
            int index = records.FindIndex(x => x.Img1 == record.Img1);
            records.RemoveAll(x => x.Img1 == record.Img1);
            records.Insert(index, record);
            string path = Path.Combine(FilePath, record.Date, "record.csv");
            File.Delete(path);
            CSV_Library.CSVHelper.Write<List<RecordModel>>(path, records);
        }
    }
}

