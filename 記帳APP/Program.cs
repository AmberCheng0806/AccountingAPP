using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using 記帳APP.Forms;

namespace 記帳APP
{
    internal static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //TODO: 希望只有預設的 Form 也就是 Application.Run 不能被Close 其他可以使用Close 而不是Hide
            Form form = SingletonFormFactory.GetForm(FormType.圖表分析);
            Application.Run(form);
        }
    }
}
