using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 記帳APP.Models.DTOs;

namespace 記帳APP.Contract
{
    internal class AnalysisContract
    {
        public interface IAnalysisPresenter
        {
            void GetExpenseOptions();

            void GetAnalysisData(DateTime start, DateTime end, List<string> groupByList, Dictionary<string, List<string>> keyValuePairs);
        }

        public interface IAnalysisView
        {
            void CheckBoxesResponse(AnalysisDTO analysisDTO);
            void RenderAnalysisData(List<AnalysisDataDTO> analysisDataDTOs);
        }
    }
}
