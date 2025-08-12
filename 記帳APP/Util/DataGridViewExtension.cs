using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 記帳APP.Models;

namespace 記帳APP.Util
{
    internal static class DataGridViewExtension
    {
        public static void CreateComboBoxColumn(this DataGridView dataGridView, PropertyInfo prop)
        {
            DataGridViewComboBoxColumn ComboBoxColumn = new DataGridViewComboBoxColumn()
            {
                HeaderText = prop.Name,
                DataPropertyName = prop.Name,
                Name = prop.Name + "Combo",
                Tag = prop.Name,
                DataSource = prop.Name == "Detail" ?
                null : typeof(DataModel).GetField(prop.Name, BindingFlags.Public | BindingFlags.Static).GetValue(null)
            };
            int index = dataGridView.Columns[prop.Name].Index;
            dataGridView.Columns.Insert(index, ComboBoxColumn);
            dataGridView.Columns[prop.Name].Visible = false;
        }

        public static void CreateImageColumn(this DataGridView dataGridView, PropertyInfo prop)
        {
            DataGridViewImageColumn imgColumn = new DataGridViewImageColumn()
            {
                HeaderText = prop.Name,
                Name = prop.Name + "ImageColumn",
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                Tag = prop.Name
            };
            int index = dataGridView.Columns[prop.Name].Index;
            dataGridView.Columns.Insert(index, imgColumn);
            dataGridView.Columns[prop.Name].Visible = false;
        }
    }
}
