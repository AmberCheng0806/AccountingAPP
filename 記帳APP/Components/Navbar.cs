using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 記帳APP.Attributes;
using 記帳APP.Forms;

namespace 記帳APP.Components
{
    public partial class Navbar : UserControl
    {
        public Navbar()
        {
            InitializeComponent();

            // GetExecutingAssembly: 取得組件，其中含有目前正在執行的程式碼。
            // GetCallingAssembly: 傳回方法的 Assembly，其叫用目前執行的方法。
            // GetEntryAssembly: 取得預設應用程式定義域中的處理序可執行檔。(從 Unmanaged 程式碼呼叫時，可能會傳回 null。)
            // Type.GetType: 取得具有指定名稱的 Type

            // | File | Method | Result |
            // | -----------------------------------------------------------|
            // | Program | GetExecutingAssembly->Type.GetType() | O |
            // | Static | GetExecutingAssembly->Type.GetType() | X |
            // | Program | GetCallingAssembly->Type.GetType() | X |
            // | Static | GetCallingAssembly->Type.GetType() | X |
            // | Program | GetEntryAssembly->Type.GetType() | O |
            // | Static | GetEntryAssembly->Type.GetType() | O |


            var types = Assembly.GetExecutingAssembly().GetTypes();
            var typelist = types.Where(x => x.BaseType == typeof(Form))
                .OrderBy(x =>
                {
                    var attribute = x.GetCustomAttribute<OrderAttribute>();
                    return attribute == null ? x.Name.Length : attribute.num;
                }).ToList();

            foreach (var type in typelist)
            {
                Button btn = new Button();
                var attribute = type.GetCustomAttribute<DisplayNameAttribute>();
                btn.Text = attribute == null ? type.Name : attribute.DisplayName;
                btn.Tag = Enum.Parse(typeof(FormType), type.Name);
                btn.Height = flowLayoutPanel1.Height;
                //btn.Width = (flowLayoutPanel1.Width / typelist.Count);
                btn.Margin = new Padding(0);
                btn.Click += ChangePage_Click;
                flowLayoutPanel1.Controls.Add(btn);
            }
            Console.WriteLine("建構元panel:" + flowLayoutPanel1.Width);
            Console.WriteLine("建構元navbar:" + this.Width);
        }



        private void ChangePage_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            FormType type = (FormType)button.Tag;
            Form form = SingletonFormFactory.GetForm(type);
            form.Show();
        }

        public void DisableBtn(FormType formName)
        {
            Button btn = flowLayoutPanel1.Controls.OfType<Button>().FirstOrDefault(x => (FormType)x.Tag == formName);
            if (btn != null)
            {
                btn.Enabled = false;
            }
        }


        private void Navbar_Resize(object sender, EventArgs e)
        {
            Console.WriteLine("userControl11_resize:" + this.Width);
            Console.WriteLine("panel_resize:" + flowLayoutPanel1.Width);
            List<Button> btns = flowLayoutPanel1.Controls.OfType<Button>().ToList();
            foreach (var btn in btns)
            {
                //flowLayoutPanel1.Width = this.Width;
                btn.Width = (flowLayoutPanel1.Width / btns.Count);
                btn.Height = flowLayoutPanel1.Height;
                Console.WriteLine("btn_resize:" + btn.Width);
            }
            Console.WriteLine("userControl11_resize2:" + this.Width);
            Console.WriteLine("panel_resize2:" + flowLayoutPanel1.Width);
        }


        private void flowLayoutPanel1_Resize(object sender, EventArgs e)
        {
            //List<Button> btns = flowLayoutPanel1.Controls.OfType<Button>().ToList();
            //foreach (var btn in btns)
            //{
            //    btn.Width = (flowLayoutPanel1.Width / btns.Count);
            //    btn.Height = flowLayoutPanel1.Height;
            //}
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
