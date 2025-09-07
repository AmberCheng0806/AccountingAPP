using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 記帳APP.Models.DTOs;

namespace 記帳APP.Contract
{
    internal class ChartContract
    {
        public interface IChartPresenter
        {
            void GetExpenseOptions();

            void GetChartData(DateTime start, DateTime end, List<string> groupByList, Dictionary<string, List<string>> keyValuePairs, string chartType, int month);
        }

        public interface IChartView
        {
            void CheckBoxesResponse(AnalysisDTO analysisDTO);
            void RenderChartData(Object obj);
        }
    }
}
