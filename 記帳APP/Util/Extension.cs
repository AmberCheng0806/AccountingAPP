using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 記帳APP.Util
{
    internal static class Extension
    {
        static Form currentForm;
        static System.Threading.Timer timer;
        static Action currentAction;
        public static void Debounce(this Form form, Action action, int time)
        {
            currentAction = action;
            timer?.Change(time, 0);
            currentForm = form;
            if (timer != null) { return; }
            timer = new System.Threading.Timer(Callback, null, time, 0);

        }

        private static void Callback(object state)
        {
            currentForm.Invoke(new Action(() =>
            {
                currentAction.Invoke();
            }));
        }
    }
}
