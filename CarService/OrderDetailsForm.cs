using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace CarService.Objects
{
    public partial class OrderDetailsForm : Form
    {
        //List fullInfo
        //0-IdOrder,1-Date, 
        //2-IdClient,3-FNameClient,4-MNameClient,5-PhoneClient,
        //6-IdCar,7-Brand,8-Model,9-Year,10-EngineID,11-EngineName,12-Value,13-Mileage,14-VIN, 
        //15-IdEmp,16-FNameEmp,17-MNameEmp,18-LNameEmp,18-PhoneEmp,
        //20-IdMaster,21-FNameMaster,22-MNameMaster,23-LNameMaster,24-PhoneMaster,
        //25-TotalCost,26-Status,27-Comment, 28-Services, 29-Prices, 30-StatusString
        List<string> fullInfo;
        DataTable services;
        public OrderDetailsForm(List<string> fullInfo)
        {
            InitializeComponent();
            this.fullInfo = fullInfo;
        }

        private void OrderDetails_Load(object sender, EventArgs e)
        {
            
            SetInfoInFields();

        }

        private void SetInfoInFields()
        {
            this.Text = "Заказ " + fullInfo[0];
            OrderNumber_textBox1.Text = fullInfo[0];
            Date_textBox1.Text = fullInfo[1];
            FNameCl_textBox1.Text = fullInfo[3];
            MNameCl_textBox2.Text = fullInfo[4];
            PhoneCl_maskedTextBox1.Text = fullInfo[5];
            Brand_textBox3.Text = fullInfo[7];
            Model_textBox4.Text = fullInfo[8];
            Year_textBox5.Text = fullInfo[9];
            Engine_textBox8.Text = fullInfo[11];
            Value_textBox9.Text = fullInfo[12];
            Mileage_textBox6.Text = fullInfo[13] == "0" ? "-" : fullInfo[13];
            VIN_textBox7.Text = fullInfo[14];
            FNameEmp_textBox12.Text = fullInfo[16];
            MNameEmp_textBox11.Text = fullInfo[17];
            LNameEmp_textBox15.Text = fullInfo[18];
            PhoneEmp_maskedTextBox2.Text = fullInfo[19];
            FNameMast_textBox14.Text = fullInfo[21];
            MNameMast_textBox13.Text = fullInfo[22];
            LNameMast_textBox16.Text = fullInfo[23];
            PhoneMast_maskedTextBox3.Text = fullInfo[24];
            Cost_textBox10.Text = fullInfo[25];
            Status_textBox2.Text = fullInfo[30];
            comboBox1.Items.Add(fullInfo[27]);
            services = CreateServiceTable(fullInfo[28], fullInfo[29]);
            dataGridView1.DataSource = services;
            dataGridView1.Columns[0].HeaderText = "Услуга";
            dataGridView1.Columns[1].HeaderText = "Цена";
        }

        private DataTable CreateServiceTable(string services, string prices)
        {
            DataTable servicesTable = new DataTable();
            servicesTable.Columns.Add("Service", typeof(string));
            servicesTable.Columns.Add("Price", typeof(string));

            string[] serviceArray= services.Split('\n');
            string[] priceArray = prices.Split('\n');

            DataRow row;
            for(int i = 0;i<serviceArray.Length; i++)
            {
                row = servicesTable.NewRow();
                row[servicesTable.Columns[0].ColumnName]= serviceArray[i];
                row[servicesTable.Columns[1].ColumnName] = priceArray[i];
                servicesTable.Rows.Add(row);

            }
            servicesTable.Rows.RemoveAt(servicesTable.Rows.Count-1);
            return servicesTable;
        }

        
    }
}
