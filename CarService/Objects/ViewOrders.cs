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
    public partial class ViewOrders : Form
    {
        bool IsOpened = false;
        DB dataBase;
        DataSet allAboutOrders;
        //DataTable ordersFull
        //0-IdOrder,1-Date, 
        //2-Brand,3-Model,4-Year
        //5-Service, 6-Price, 7-TotalPrice, 8-Comment, 9-Status
        DataTable ordersShort;
        //DataTable allInfoAboutOrders
        //0-IdOrder,1-Date, 
        //2-IdClient,3-FNameClient,4-MNameClient,5-PhoneClient,
        //6-IdCar,7-Brand,8-Model,9-Year,10-Engine,11-Value,12-Mileage,13-VIN, 
        //14-IdEmp,15-FNameEmp,16-MNameEmp,17-LNameEmp,18-PhoneEmp,
        //19-IdMaster,20-FNameMaster,21-MNameMaster,22-LNameMaster,23-PhoneMaster,
        //24-TotalCost,25-Status,26-Comment
        DataTable ordersFull;
        DataView ordersView;
        public ViewOrders()
        {
            InitializeComponent();
            dataBase = new DB();
        }

        private void ViewOrders_Load(object sender, EventArgs e)
        {
            allAboutOrders= dataBase.LoadOrders();
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
            comboBox1.SelectedIndex = comboBox2.SelectedIndex = comboBox3.SelectedIndex=comboBox4.SelectedIndex= -1;
            IsOpened=true;
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
            dateTimePicker2.MaxDate =DateTime.Now;

            dateTimePicker1.Value = DateTime.Today.AddMonths(-1);
            dateTimePicker2.Value = DateTime.Today;
        }

        private void SetHeaderTextDGV()
        {
            dataGridView1.Columns[0].HeaderText="Номер заказа";
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
                names.Add(row.ItemArray[3].ToString()+' '+ row.ItemArray[4].ToString()+' '+ row.ItemArray[5].ToString());

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
                "Выполнен",
                "Оформлен",
                "В процессе"
            };

            comboBox4.DataSource = status;

        }

        private void button2_Click(object sender, EventArgs e)
        {

            //string condition1 = string.Format("{0}={1}",
            //    ordersShort.Columns[0].ColumnName, comboBox1.SelectedValue.ToString());
            //string condition2= string.Format("{0}={1}",
            //    ordersShort.Columns[0].ColumnName, ordersFull.Select(ordersFull.Columns[5].ColumnName+"="+comboBox2.SelectedValue.ToString().Split(new char[] {' ' })[2])[0].ItemArray[0].ToString());
            ////if(comboBox1.SelectedValue==null)
            //string filter = condition1+" OR "+condition2;
            //ordersView.RowFilter = filter;

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
            if(comboBox1.SelectedIndex != -1 && IsOpened) {
                string filter = string.Format("{0}={1}",
                ordersShort.Columns[0].ColumnName, comboBox1.SelectedValue.ToString()); ;
                ordersView.RowFilter = filter;
            }
         
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox2.SelectedIndex!= -1 && IsOpened)
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
            //6-IdCar,7-Brand,8-Model,9-Year,10-Engine,11-Value,12-Mileage,13-VIN, 
            //14-IdEmp,15-FNameEmp,16-MNameEmp,17-LNameEmp,18-PhoneEmp,
            //19-IdMaster,20-FNameMaster,21-MNameMaster,22-LNameMaster,23-PhoneMaster,
            //24-TotalCost,25-StatusInt,26-Comment, 27-Services, 28-Prices,29-StatusString

            int idOrder = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            List<string> fullInfo = new List<string>();
            fullInfo.Add(idOrder.ToString());
            for(int i = 1; i < ordersFull.Columns.Count; i++)
            {
                fullInfo.Add(GetValueFromFullOrdersTable(idOrder, i));
            }
            fullInfo.Add(GetValueFromShortOrdersTable(idOrder, 5));
            fullInfo.Add(GetValueFromShortOrdersTable(idOrder, 6));
            fullInfo.Add(GetValueFromShortOrdersTable(idOrder,9));

            OrderDetails orderDetails = new OrderDetails(fullInfo);
            orderDetails.ShowDialog(this);

            //string date = GetValueFromFullOrdersTable(idOrder,1);
            //string idCln = GetValueFromFullOrdersTable(idOrder, 2);
            //string FNameCLn = GetValueFromFullOrdersTable(idOrder, 3);
            //string MNameCln = GetValueFromFullOrdersTable(idOrder, 4);
            //string PhoneCln = GetValueFromFullOrdersTable(idOrder, 5);
            //string IdCar = GetValueFromFullOrdersTable(idOrder, 6);
            //string Brand = GetValueFromFullOrdersTable(idOrder, 7);
            //string Model = GetValueFromFullOrdersTable(idOrder, 8);
            //string Year = GetValueFromFullOrdersTable(idOrder, 9);
            //string Engine = GetValueFromFullOrdersTable(idOrder, 10);
            //string Value = GetValueFromFullOrdersTable(idOrder, 11);
            //string Mileage = GetValueFromFullOrdersTable(idOrder, 12);
            //string VIN = GetValueFromFullOrdersTable(idOrder, 13);
            //string IdEmp = GetValueFromFullOrdersTable(idOrder, 14);
            //string FNameEmp = GetValueFromFullOrdersTable(idOrder, 15);
            //string MNameEmp = GetValueFromFullOrdersTable(idOrder, 16);
            //string LNameEmp = GetValueFromFullOrdersTable(idOrder, 17);
            //string PhoneEmp = GetValueFromFullOrdersTable(idOrder, 18);
            //string IdMast = GetValueFromFullOrdersTable(idOrder, 19);
            //string FNameMast = GetValueFromFullOrdersTable(idOrder, 20);
            //string MNameMast = GetValueFromFullOrdersTable(idOrder, 21);
            //string LNameMast = GetValueFromFullOrdersTable(idOrder, 22);
            //string PhoneMast = GetValueFromFullOrdersTable(idOrder, 23);
            //string Cost = GetValueFromFullOrdersTable(idOrder, 24);
            //string Status = GetValueFromFullOrdersTable(idOrder, 25);
            //string Comment = GetValueFromFullOrdersTable(idOrder, 26);



        }

        private string GetValueFromFullOrdersTable(int idOrder,int itemNumber)
        {
            return ordersFull.Select(string.Format("{0}={1}", ordersFull.Columns[0].ColumnName, idOrder))[0].ItemArray[itemNumber].ToString();
        }
        private string GetValueFromShortOrdersTable(int idOrder, int itemNumber)
        {
            return ordersShort.Select(string.Format("{0}={1}", ordersShort.Columns[0].ColumnName, idOrder))[0].ItemArray[itemNumber].ToString();
        }
    }
}
