using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarService.Objects
{
    public partial class ViewOrdersForm : Form
    {
        bool IsOpened = false;
        DB dataBase;

        Employee employee;

        DataSet allAboutOrders;
        //DataTable ordersShort
        //0-IdOrder,1-Date, 
        //2-Brand,3-Model,4-Year
        //5-Service, 6-Price, 7-TotalPrice, 8-Comment, 9-Status
        DataTable ordersShort;
        //DataTable allInfoAboutOrders
        //0-IdOrder,1-Date, 
        //2-IdClient,3-FNameClient,4-MNameClient,5-PhoneClient,
        //6-IdCar,7-Brand,8-Model,9-Year,10-EngineID,11-EngineName,12-Value,13-Mileage,14-VIN, 
        //15-IdEmp,16-FNameEmp,17-MNameEmp,18-LNameEmp,18-PhoneEmp,
        //20-IdMaster,21-FNameMaster,22-MNameMaster,23-LNameMaster,24-PhoneMaster,
        //25-TotalCost,26-Status,27-Comment
        DataTable ordersFull;
        DataView ordersView;
        public ViewOrdersForm(int idEmployee = 2)
        {
            InitializeComponent();
            dataBase = new DB();
            employee = dataBase.getEmployeeById(idEmployee);
        }

        private void ViewOrders_Load(object sender, EventArgs e)
        {
            allAboutOrders = dataBase.LoadOrders();
            ordersShort = allAboutOrders.Tables[2];
            ordersFull = allAboutOrders.Tables[0];
            ordersView = new DataView(ordersShort);
            dataGridView1.DataSource = ordersView;
            SetHeaderTextDGV();
            SetTimeSettings();
            SetIdOrderCB();
            SetCLientNameCB();
            SetClientPhoneCB();
            SetStatusCB();
            comboBox1.SelectedIndex = comboBox2.SelectedIndex = comboBox3.SelectedIndex = comboBox4.SelectedIndex = -1;
            IsOpened = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string dateFrom = dateTimePicker1.Value.Date.ToString("yyyy-MM-dd HH:mm:ss");
            string dateTo = dateTimePicker2.Value.Date.ToString("yyyy-MM-dd HH:mm:ss");
            ordersView.RowFilter = string.Format("{0} >= #{1}# AND {2}<=#{3}#", ordersShort.Columns[1].ColumnName, dateFrom, ordersShort.Columns[1].ColumnName, dateTo);
        }

        private void SetTimeSettings()
        {
            dateTimePicker1.MaxDate =
            dateTimePicker2.MaxDate = DateTime.Now;

            dateTimePicker1.Value = DateTime.Today.AddMonths(-1);
            dateTimePicker2.Value = DateTime.Today;
        }

        private void SetHeaderTextDGV()
        {
            dataGridView1.Columns[0].HeaderText = "Номер заказа";
            dataGridView1.Columns[1].HeaderText = "Дата";
            dataGridView1.Columns[2].HeaderText = "Марка";
            dataGridView1.Columns[3].HeaderText = "Модель";
            dataGridView1.Columns[4].HeaderText = "Год";
            dataGridView1.Columns[5].HeaderText = "Услуги";
            dataGridView1.Columns[6].HeaderText = "Цена";
            dataGridView1.Columns[7].HeaderText = "Стоимость";
            dataGridView1.Columns[8].HeaderText = "Комментарий";
            dataGridView1.Columns[9].HeaderText = "Статус";
        }

        private void SetIdOrderCB()
        {
            List<string> ids = new List<string>();
            foreach (DataRow row in ordersShort.Rows)
                ids.Add(row.ItemArray[0].ToString());

            comboBox1.SelectedIndex = -1;

        }

        private void SetCLientNameCB()
        {
            List<string> names = new List<string>();
            foreach (DataRow row in ordersFull.Rows)
                names.Add(row.ItemArray[3].ToString() + ' ' + row.ItemArray[4].ToString() + ' ' + row.ItemArray[5].ToString());

            comboBox2.DataSource = names;

        }

        private void SetClientPhoneCB()
        {
            List<string> phones = new List<string>();
            foreach (DataRow row in ordersFull.Rows)
                phones.Add(row.ItemArray[5].ToString());

            comboBox3.DataSource = phones;

        }

        private void SetStatusCB()
        {
            List<string> status = new List<string>
            {
                dataBase.ConvertStatus(1),
                dataBase.ConvertStatus(2),
                dataBase.ConvertStatus(3),
            };

            comboBox4.DataSource = status;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            ordersView.RowFilter = string.Empty;
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1 && IsOpened)
            {
                string filter = string.Format("{0}={1}",
                ordersShort.Columns[0].ColumnName, comboBox1.SelectedValue.ToString()); ;
                ordersView.RowFilter = filter;
            }

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex != -1 && IsOpened)
            {
                string filter = string.Format("{0}={1}",
                    ordersShort.Columns[0].ColumnName, ordersFull.Select(ordersFull.Columns[5].ColumnName + "=" + comboBox2.SelectedValue.ToString().Split(new char[] { ' ' })[2])[0].ItemArray[0].ToString());
                ordersView.RowFilter = filter;
            }

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex != -1 && IsOpened)
            {
                string filter = string.Format("{0}={1}",
                    ordersShort.Columns[0].ColumnName, ordersFull.Select(ordersFull.Columns[5].ColumnName + "=" + comboBox3.SelectedValue.ToString()));
                ordersView.RowFilter = filter;
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.SelectedIndex != -1 && IsOpened)
            {
                string filter = string.Format("{0}='{1}'", ordersShort.Columns[9].ColumnName, comboBox4.SelectedValue.ToString());
                ordersView.RowFilter = filter;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1) return;
            //List fullInfo
            //0-IdOrder,1-Date, 
            //2-IdClient,3-FNameClient,4-MNameClient,5-PhoneClient,
            //6-IdCar,7-Brand,8-Model,9-Year,10-EngineID,11-EngineName,12-Value,13-Mileage,14-VIN, 
            //15-IdEmp,16-FNameEmp,17-MNameEmp,18-LNameEmp,18-PhoneEmp,
            //20-IdMaster,21-FNameMaster,22-MNameMaster,23-LNameMaster,24-PhoneMaster,
            //25-TotalCost,26-Status,27-Comment, 28-Services, 29-Prices,30-StatusString

            int idOrder = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            List<string> fullInfo = new List<string>();
            fullInfo.Add(idOrder.ToString());
            for (int i = 1; i < ordersFull.Columns.Count; i++)
            {
                fullInfo.Add(GetValueFromFullOrdersTable(idOrder, i));
            }
            fullInfo.Add(GetValueFromShortOrdersTable(idOrder, 5));
            fullInfo.Add(GetValueFromShortOrdersTable(idOrder, 6));
            fullInfo.Add(GetValueFromShortOrdersTable(idOrder, 9));

            OrderDetailsForm orderDetails = new OrderDetailsForm(fullInfo);
            orderDetails.ShowDialog(this);


        }

        private string GetValueFromFullOrdersTable(int idOrder, int itemNumber)
        {
            return ordersFull.Select(string.Format("{0}={1}", ordersFull.Columns[0].ColumnName, idOrder))[0].ItemArray[itemNumber].ToString();
        }
        private string GetValueFromShortOrdersTable(int idOrder, int itemNumber)
        {
            return ordersShort.Select(string.Format("{0}={1}", ordersShort.Columns[0].ColumnName, idOrder))[0].ItemArray[itemNumber].ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataTable dt = CreateReportDataTable();
            ReportForm reportForm = new ReportForm(dt, setReportParametrs(), "ordersView");
            reportForm.ShowDialog();
        }

        private List<ReportParameter> setReportParametrs()
        {
            List<ReportParameter> parametrs = new List<ReportParameter>();

            ReportParameter param1 = new ReportParameter("TotalOrdersCount", dataGridView1.Rows.Count.ToString());
            ReportParameter param2 = new ReportParameter("TotalOrdersSum", CountTotalSum());


            parametrs.Add(param1);
            parametrs.Add(param2);


            return parametrs;
        }

        private string CountTotalSum()
        {
            double sum = 0;
            foreach (DataGridViewRow viewRow in dataGridView1.Rows)
            {
                sum += Convert.ToDouble(viewRow.Cells[7].Value);
            }

            return sum.ToString();
        }

        private DataTable CreateReportDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("IdOrder", typeof(int));
            dt.Columns.Add("Date", typeof(DateTime));
            dt.Columns.Add("Brand", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Price", typeof(string));
            dt.Columns.Add("TotalCost", typeof(double));
            dt.Columns.Add("Comment", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Employee", typeof(string));

            DataRow newRow;

            foreach (DataGridViewRow viewRow in dataGridView1.Rows)
            {
                newRow = dt.NewRow();
                object[] itemArray = new object[]
                {
                    Convert.ToInt32(viewRow.Cells[0].Value),
                    Convert.ToDateTime(viewRow.Cells[1].Value),
                    viewRow.Cells[2].Value.ToString() + ' ' + viewRow.Cells[3].Value.ToString() + ' ' + viewRow.Cells[4].Value.ToString(),
                    viewRow.Cells[5].Value.ToString(),
                    viewRow.Cells[6].Value.ToString(),
                    Convert.ToDouble(viewRow.Cells[7].Value),
                    viewRow.Cells[8].Value.ToString(),
                    viewRow.Cells[9].Value.ToString(),
                    null,
                };
                newRow.ItemArray = itemArray;
                dt.Rows.Add(newRow);
            }

            return dt;
        }
    }
}
