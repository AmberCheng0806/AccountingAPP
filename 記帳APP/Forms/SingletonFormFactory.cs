using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 記帳APP.Components;

namespace 記帳APP.Forms
{
    internal static class SingletonFormFactory
    {
        private static Form lastForm;
        private static FormType defaultForm;
        private static Dictionary<FormType, Form> forms = new Dictionary<FormType, Form>();
        public static Form GetForm(FormType formType)
        {
            if (lastForm == null)
            {
                defaultForm = formType;
            }
            if (lastForm != null)
            {
                if (lastForm.Name == defaultForm.ToString())
                {
                    lastForm.Hide();
                }
                else
                {
                    lastForm.Close();
                    forms.Remove((FormType)Enum.Parse(typeof(FormType), lastForm.Name));
                }
            }

            if (!forms.ContainsKey(formType))
            {
                //switch (formType)
                //{
                //    case FormType.記一筆:
                //        lastForm = new 記一筆();
                //        break;
                //    case FormType.圖表分析:
                //        lastForm = new 圖表分析();
                //        break;
                //    case FormType.帳戶分析:
                //        lastForm = new 帳戶分析();
                //        break;
                //    case FormType.記帳本:
                //        lastForm = new 記帳本();
                //        break;
                //}

                Type type = Type.GetType("記帳APP.Forms." + formType.ToString());
                lastForm = (Form)Activator.CreateInstance(type);
                forms.Add(formType, lastForm);
            }
            lastForm = forms[formType];

            //FieldInfo[] fields = lastForm.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            //foreach (var field in fields)
            //{
            //    if (field.FieldType == typeof(Navbar))
            //    {
            //        Navbar navbar = (Navbar)field.GetValue(lastForm);
            //        navbar.DisableBtn(formType);
            //    }
            //}

            //Type type = lastForm.GetType();
            //var attribute = type.GetCustomAttribute<DisplayNameAttribute>();
            //formType = attribute.DisplayName;

            ((Navbar)lastForm.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .FirstOrDefault(x => x.FieldType == typeof(Navbar))
                ?.GetValue(lastForm))
               ?.DisableBtn(formType);

            //FieldInfo field = lastForm.GetType().GetField("userControl11", BindingFlags.Instance | BindingFlags.NonPublic);
            //Navbar navbar = (Navbar)field.GetValue(lastForm);
            //navbar.DisableBtn(formType);

            return lastForm;
        }
    }
}
