using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using 記帳APP.Models;
using 記帳APP.Models.DTOs;

namespace 記帳APP.Contract
{
    internal class ChartContract
    {
        public interface IChartPresenter
        {
            void GetExpenseOptions();

            void GetChartData(DateTime start, DateTime end, List<string> groupByList, Dictionary<string, List<string>> keyValuePairs, string chartType, int month, int width, int height);
            void GetComboboxData();
        }

        public interface IChartView
        {
            void CheckBoxesResponse(AnalysisDTO analysisDTO);

            //建造者模式
            void RenderChartData(Chart chart);

            void InitializeComboBox(ChartComboBoxModel chartComboBoxModel);
        }
    }
}
