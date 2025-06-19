using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 記帳APP.Models
{
    internal class DataModel
    {
        public static List<string> Type = new List<string>() { "食", "衣", "住", "行", "樂" };
        public static List<string> People = new List<string>() { "家人", "朋友", "自己", "同事" };
        public static List<string> PaymentType = new List<string>() { "現金", "信用卡", "line pay" };
        public static List<string> food = new List<string>() { "正餐", "甜點", "飲料" };
        public static List<string> cloth = new List<string>() { "上衣", "下身", "配飾" };
        public static List<string> living = new List<string>() { "房租", "水電" };
        public static List<string> traffic = new List<string>() { "計程車", "高鐵", "火車" };
        public static List<string> entertainment = new List<string>() { "運動", "遊戲", "串流服務" };
        public static Dictionary<string, List<string>> keyValuePairs = new Dictionary<string, List<string>>() {
            {"食",food },
            {"衣",cloth },
            {"住",living },
            {"行",traffic },
            {"樂",entertainment }
        };
    }
}
