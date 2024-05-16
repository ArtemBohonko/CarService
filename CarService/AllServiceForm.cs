using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarService
{
    public partial class AllServiceForm : Form
    {
        DB dataBase;
        DataTable categoryesTable;
        DataTable servicesTable;
        bool IsOpened = false;
        DataView servicesView;
        public AllServiceForm()
        {
            InitializeComponent();
            dataBase= new DB();
        }

        private void AllServiceForm_Load(object sender, EventArgs e)
        {
            categoryesTable = dataBase.LoadCategories();
            servicesTable = dataBase.LoadServices();
            servicesView=new DataView(servicesTable);

            comboBox1.DataSource=categoryesTable;
            comboBox1.DisplayMember = categoryesTable.Columns[1].ColumnName;
            comboBox1.ValueMember = categoryesTable.Columns[0].ColumnName;
            comboBox1.SelectedIndex = -1;
            IsOpened = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!IsOpened || comboBox1.SelectedIndex==-1)
            {
                return;
            }
            dataGridView1.DataSource = servicesView;
            servicesView.RowFilter = string.Format("{0}={1}", servicesTable.Columns[3].ColumnName,comboBox1.SelectedValue.ToString());
            dataGridView1.Columns[0].Visible = dataGridView1.Columns[3].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Услуга";
            dataGridView1.Columns[2].HeaderText = "Цена";
        }
    }
}
