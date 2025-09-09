using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 記帳APP.Util
{
    internal class DateUtil
    {
        public static List<string> GetAllDates(DateTime start, DateTime end)
        {
            List<string> AllDates = new List<string>();
            TimeSpan timeSpan = end - start;
            for (int i = 0; i <= timeSpan.Days; i++)
            {
                string date = start.AddDays(i).ToString("yyyy-MM-dd");
                AllDates.Add(date);
            }

            if (start.Year == end.Year && end.Month - start.Month > 0)
            {
                int count = end.Month - start.Month;
                DateTime dateTime = start;
                for (int i = 0; i < count; i++)
                {
                    dateTime = dateTime.AddMonths(i);
                    List<string> missingDays = GetMissingDays(dateTime);
                    AllDates = CheckDayExist(missingDays, AllDates);
                }
            }

            if (end.Year - start.Year > 0)
            {
                int startCount = 12 - start.Month + 1;
                DateTime dateTime = start;
                for (int i = 0; i < startCount; i++)
                {
                    dateTime = dateTime.AddMonths(i);
                    List<string> missingDays = GetMissingDays(dateTime);
                    AllDates = CheckDayExist(missingDays, AllDates);
                }

                int endCount = end.Month - 1;
                DateTime day = DateTime.Parse(end.Year.ToString() + "-01-01");
                dateTime = day;
                for (int i = 0; i < endCount; i++)
                {
                    dateTime = dateTime.AddMonths(i);
                    List<string> missingDays = GetMissingDays(dateTime);
                    AllDates = CheckDayExist(missingDays, AllDates);
                }
            }
            List<string> orderedDates = AllDates.OrderBy(x => x).ToList();
            return orderedDates;
        }

        public static List<string> GetMissingDays(DateTime dateTime)
        {
            List<string> days = new List<string>();
            string date = dateTime.ToString("yyyy-MM");
            if (dateTime.Month != 2)
            {
                days.Add(date + "-31");
                return days;
            }
            days.Add(date + "-29");
            days.Add(date + "-30");
            days.Add(date + "-31");
            return days;
        }

        //補上list中缺失日期
        public static List<string> CheckDayExist(List<string> missingDays, List<string> dates)
        {
            foreach (var item in missingDays)
            {
                if (dates.Contains(item))
                {
                    continue;
                }
                dates.Add(item);
            }
            return dates;
        }
    }
}
