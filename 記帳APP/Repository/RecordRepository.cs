using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 記帳APP.Models;
using 記帳APP.Repository.Entities;

namespace 記帳APP.Repository
{
    internal class RecordRepository : IRecordRepository
    {
        private string FilePath;
        public RecordRepository()
        {
            FilePath = ConfigurationManager.AppSettings["FilePath"];
        }
        public void Delete(RecordEntity record)
        {
            List<RecordEntity> records = GetDatas(DateTime.Parse(record.Date));
            records.RemoveAll(x => x.Img1 == record.Img1);
            File.Delete(record.Img1);
            File.Delete(record.Img1.Replace("40x40", ""));
            File.Delete(record.Img2);
            File.Delete(record.Img2.Replace("40x40", ""));
            string path = Path.Combine(FilePath, record.Date, "record.csv");
            File.Delete(path);
            CSV_Library.CSVHelper.Write<RecordEntity>(path, records);
        }

        public List<RecordEntity> GetDatas(DateTime date)
        {
            string path = Path.Combine(FilePath, date.ToString("yyyy-MM-dd"), "record.csv");
            return CSV_Library.CSVHelper.Read<RecordEntity>(path);
        }

        public List<RecordEntity> GetDatas(DateTime start, DateTime end)
        {
            List<RecordEntity> records = new List<RecordEntity>();
            TimeSpan timeSpan = end - start;
            for (int i = 0; i <= timeSpan.Days; i++)
            {
                string date = start.AddDays(i).ToString("yyyy-MM-dd");
                string path = Path.Combine(FilePath, date, "record.csv");
                if (!File.Exists(path))
                {
                    continue;
                }
                List<RecordEntity> recordByDay = CSV_Library.CSVHelper.Read<RecordEntity>(path);
                records.AddRange(recordByDay);
            }
            return records;
        }

        public void Insert(RecordEntity record)
        {
            string folder = Path.Combine(FilePath, record.Date);
            Directory.CreateDirectory(folder);
            string path = Path.Combine(folder, "record.csv");
            CSV_Library.CSVHelper.Write<RecordEntity>(path, record);
        }

        public void Update(RecordEntity record)
        {
            List<RecordEntity> records = GetDatas(DateTime.Parse(record.Date));
            int index = records.FindIndex(x => x.Img1 == record.Img1);
            records.RemoveAll(x => x.Img1 == record.Img1);
            records.Insert(index, record);
            string path = Path.Combine(FilePath, record.Date, "record.csv");
            File.Delete(path);
            CSV_Library.CSVHelper.Write<RecordEntity>(path, records);
        }
    }
}

