using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CarService
{
    public partial class StatisticForm : Form
    {
        bool IsOpened = false;

        DataTable topCars, topParts, topBrands, topServices;


        Dictionary<string, int> statisticForMonth, statisticForYear, statisticForMonthSum, statisticForYearSum;

        DB dataBase;
        int minYearOrder, maxYearOrder;

        public StatisticForm()
        {
            InitializeComponent();
            dataBase = new DB();
        }

        private void StatisticForm_Load(object sender, EventArgs e)
        {
            minYearOrder = dataBase.GetMinOrderYear();
            maxYearOrder = dataBase.GetMaxOrderYear();
            LoadMonthsInComboBox();
            LoadYearInComboBox();
            IsOpened = true;
        }

        private void LoadMonthsInComboBox()
        {
            DataTable months = new DataTable();
            months.Columns.Add("MonthNumber",typeof(int));
            months.Columns.Add("MonthName", typeof(string));

            months.Rows.Add(new object[] { 1, "Январь" });
            months.Rows.Add(new object[] { 2, "Февраль" });
            months.Rows.Add(new object[] { 3, "Март" });
            months.Rows.Add(new object[] { 4, "Апрель" });
            months.Rows.Add(new object[] { 5, "Май" });
            months.Rows.Add(new object[] { 6, "Июнь" });
            months.Rows.Add(new object[] { 7, "Июль" });
            months.Rows.Add(new object[] { 8, "Август" });
            months.Rows.Add(new object[] { 9, "Сентябрь" });
            months.Rows.Add(new object[] { 10, "Октябрь" });
            months.Rows.Add(new object[] { 11, "Ноябрь" });
            months.Rows.Add(new object[] { 12, "Декабрь" });


            comboBox1.DataSource = months;
            comboBox1.ValueMember = months.Columns[0].ColumnName;
            comboBox1.DisplayMember = months.Columns[1].ColumnName;

            comboBox1.SelectedIndex = 0;
        }


        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SetStatistic();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
            {
                MessageBox.Show(
                          string.Format("Для отображения статистики необходимо выбрать наименование в списке слева"),
                          "Не верные данные",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information,
                          MessageBoxDefaultButton.Button1);
                return;
            }

          
        }


        private void SetStatistic()
        {
            if (treeView1.SelectedNode.Tag!=null && treeView1.SelectedNode.Tag.ToString() == "N1")
            {
                
                SetOrdersForMonth();
            }
            else if (treeView1.SelectedNode.Tag != null && treeView1.SelectedNode.Tag.ToString() == "N2")
            {
                SetOrdersForYear();
            }
            else if (treeView1.SelectedNode.Tag != null && treeView1.SelectedNode.Tag.ToString() == "N3")
            {
                SetOrdersSumForMonth();
            }
            else if (treeView1.SelectedNode.Tag != null && treeView1.SelectedNode.Tag.ToString() == "N4")
            {
                SetOrdersSumForYear();
            }
            else if (treeView1.SelectedNode.Tag != null && treeView1.SelectedNode.Tag.ToString() == "N5")
            {
                SetTopClients();
            }
            else if (treeView1.SelectedNode.Tag != null && treeView1.SelectedNode.Tag.ToString() == "N6")
            {
                SetTopService();
            }
            else if(treeView1.SelectedNode.Tag != null && treeView1.SelectedNode.Tag.ToString() == "N7")
            {
                SetTopCars();
            }

            this.Text = treeView1.SelectedNode != null ? treeView1.SelectedNode.Text : "Статистика";
            chart1.Series[0].Name = this.Text;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(IsOpened)
                SetStatistic();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsOpened)
                SetStatistic();
        }

        private void LoadYearInComboBox()
        {
            if (comboBox2.Items.Count != 0)
                comboBox2.Items.Clear();

                for (int i = minYearOrder; i < maxYearOrder + 1; i++)
                {
                    comboBox2.Items.Add(i);
                }

           
            if (comboBox2.Items.Count != 0)
                comboBox2.SelectedIndex = 0;
        }

        private void SetOrdersForMonth()
        {
            groupBox1.Visible = true;
            comboBox1.Visible = label3.Visible = true;
            dataGridView1.Visible = false;
            chart1.Visible = true;
            statisticForMonth = dataBase.LoadOrdersForMonth(Convert.ToInt32(comboBox1.SelectedValue),Convert.ToInt32(comboBox2.Text));
            ShowStatisticInChart(statisticForMonth, chart1);

        }
        private void SetOrdersForYear()
        {
            groupBox1.Visible = true;
            comboBox1.Visible = label3.Visible = false;
            dataGridView1.Visible = false;
            chart1.Visible = true;
            statisticForYear = dataBase.LoadOrdersForYear(Convert.ToInt32(comboBox2.Text));
            ShowStatisticInChart(statisticForYear, chart1);
        }

        private void SetOrdersSumForYear()
        {
            groupBox1.Visible = true;
            comboBox1.Visible = label3.Visible = false;
            dataGridView1.Visible = false;
            chart1.Visible = true;
            statisticForYearSum = dataBase.LoadOrdersSumForYear(Convert.ToInt32(comboBox2.Text));
            ShowStatisticInChart(statisticForYearSum, chart1);
        }

        private void SetOrdersSumForMonth()
        {
            groupBox1.Visible = true;
            comboBox1.Visible = label3.Visible = true;
            dataGridView1.Visible = false;
            chart1.Visible = true;
            statisticForMonthSum = dataBase.LoadOrdersSumForMonth(Convert.ToInt32(comboBox1.SelectedValue), Convert.ToInt32(comboBox2.Text));
            ShowStatisticInChart(statisticForMonthSum,chart1);
            
        }

        private void SetTopCars()
        {
            groupBox1.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.BringToFront();
            chart1.Visible = false;
            topCars = dataBase.LoadTops("cars");
            dataGridView1.DataSource = topCars;
            dataGridView1.Columns[0].HeaderText = "Автомобиль";
            dataGridView1.Columns[1].HeaderText = "Количество обслуживаний";
        }

        private void SetTopService()
        {
            groupBox1.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.BringToFront();
            chart1.Visible = false;
            topServices = dataBase.LoadTops("service");
            dataGridView1.DataSource = topServices;
            dataGridView1.Columns[0].HeaderText = "Услуга";
            dataGridView1.Columns[1].HeaderText = "Количество исполнений";
        }


        private void SetTopClients()
        {
            groupBox1.Visible = false;
            dataGridView1.Visible = true;
            dataGridView1.BringToFront();
            chart1.Visible = false;
            topServices = dataBase.LoadTops("clients");
            dataGridView1.DataSource = topServices;
            dataGridView1.Columns[0].HeaderText = "Клиент";
            dataGridView1.Columns[1].HeaderText = "Количество обращений";
        }

        private void ShowStatisticInChart(Dictionary<string, int> statistic, System.Windows.Forms.DataVisualization.Charting.Chart chart)
        {
            if (chart.Series[0].Points.Count != 0)
                chart.Series[0].Points.Clear();

            int ind = 0;
            foreach (var i in statistic)
            {
                chart.Series[0].Points.Add(i.Value);
                chart.Series[0].Points[ind].AxisLabel = i.Key;
                chart.Series[0].Points[ind].Label = i.Value.ToString();

                ind++;
            }

        }
    }
}
