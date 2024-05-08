using CarService.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;


namespace CarService
{
    public partial class MainForm : Form
    {
        private bool IsOpened = false;
        private bool IsClientNew = false;
        public int IdEmployee;
        private Order order;
        private Employee employee;
        private DB dataBase;
        public Form parentForm;

        private DataTable selectedServices;
        private DataTable carDataTable;
        private DataTable engineTable;
        private DataTable serviceTable;
        private DataTable categoryTable;
        private DataTable clientsTable;
        private DataTable employeesTable;
        public MainForm(int Id)
        {
            InitializeComponent();
            this.IdEmployee = Id;
            dataBase = new DB();
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            setEmployeeText();

            this.carDataTable=dataBase.LoadCars();
            this.engineTable = dataBase.LoadEngine();
            this.serviceTable=dataBase.LoadServices();
            this.categoryTable=dataBase.LoadCategories();
            this.clientsTable=dataBase.LoadClients();
            this.employeesTable = dataBase.LoadEmployees();

            setCarsInCB(this.carDataTable);
            setYearInCB();
            setEngineInCB2();
            setValueInCB();
            setMastersInCB(this.employeesTable);
            setClientsInCB(this.clientsTable);
            setCategoriesInCB(this.categoryTable);
            setTableInDGV();

            ClearFields();
            IsOpened = true;
        }


       

        #region Set Catalogs

        private void setCarsInCB(DataTable cars)
        {
            BindingSource source = new BindingSource();

            source.DataSource = cars.AsEnumerable()
            .Select(row => row["Brand"].ToString())
            .Distinct();

            comboBox1.DataSource = source;

            comboBox1.DisplayMember = "Brand";
        }

        private void setModelInCB(DataTable cars)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                comboBox2.SelectedIndex = -1;
                comboBox2.Enabled = false;
                return;
            }
            BindingSource source = new BindingSource();
            source.DataSource = cars;
            source.Filter = cars.Columns[1].ColumnName+"='" + comboBox1.SelectedItem.ToString() + "'";
            comboBox2.Enabled = true;
            comboBox2.DataSource = source;
            comboBox2.DisplayMember = cars.Columns[2].ColumnName;
            comboBox2.ValueMember = cars.Columns[0].ColumnName;

        }

        private void setYearInCB()
        {
            List<string> years = new List<string>();

            for(int i = DateTime.Now.Year; i > 1950; i--)
            {
                years.Add(i.ToString());
            }
            comboBox3.DataSource = years;
        }

        private void setEngineInCB2()
        {
            comboBox4.DataSource = engineTable;
            comboBox4.DisplayMember = engineTable.Columns[1].ColumnName;
            comboBox4.ValueMember = engineTable.Columns[0].ColumnName;
        } 

        private void setValueInCB()
        {
            
            List<string> value = new List<string>();
            for (int i=10; i < 80; i ++)
            {
                value.Add(((double)(i/10.0)).ToString());
            }
          
            comboBox5.DataSource = value;
        }

        private void setMastersInCB(DataTable employees)
        {
            List<string> namesEmployee= new List<string>(); 
            string fullName;

            foreach (DataRow row in employees.Select(string.Format("{0}={1}", employees.Columns[5].ColumnName,3)))
            {
                fullName = row.ItemArray[1].ToString() + ' ' + row.ItemArray[2].ToString() + ' ' + row.ItemArray[3].ToString();
                namesEmployee.Add(fullName);
            }

            comboBox8.DataSource = namesEmployee;
        }

        private void setClientsInCB(DataTable clients)
        {
            List<string> clientsFullName = new List<string>();
            string fullName;

            foreach (DataRow row in clients.Rows)
            {
                fullName = row.ItemArray[1].ToString() +' ' + row.ItemArray[2].ToString() + ' '+ row.ItemArray[3].ToString();
                clientsFullName.Add(fullName);
            }

            comboBox7.DataSource = clientsFullName;
        }

        private void setClientFullName()
        {
            if (comboBox7.SelectedIndex == -1) return;
            label10.Visible = label11.Visible = maskedTextBox1.Visible = textBox3.Visible = textBox5.Visible = true;
            comboBox7.Visible = false;
            string name = comboBox7.SelectedItem.ToString().Split(new char[] { ' ' })[0];
            string surname = comboBox7.SelectedItem.ToString().Split(new char[] { ' ' })[1];
            string phone = comboBox7.SelectedItem.ToString().Split(new char[] { ' ' })[2];
            textBox5.Text = name;
            textBox3.Text = surname;
            maskedTextBox1.Text = phone;
        }

        private void setCategoriesInCB(DataTable category)
        {
            comboBox9.DataSource = category;
            comboBox9.DisplayMember= category.Columns[1].ColumnName;
            comboBox9.ValueMember = category.Columns[0].ColumnName;
        }

        private void setServiceInCB(DataTable services)
        {
            if (comboBox9.SelectedIndex == -1)
            {
                comboBox6.SelectedIndex = -1;
                comboBox6.Enabled = false;
                return;
            }
            BindingSource source = new BindingSource();
            source.DataSource = services;
            source.Filter = services.Columns[3].ColumnName + "='" + comboBox9.SelectedValue.ToString() + "'";
            comboBox6.Enabled = true;
            comboBox6.DataSource = source;
            comboBox6.DisplayMember = services.Columns[1].ColumnName;
            comboBox6.ValueMember = services.Columns[0].ColumnName;
        }


        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsOpened) return;
            setServiceInCB(this.serviceTable);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsOpened) return;
            setModelInCB(this.carDataTable);

        }

        private void setEmployeeText()
        {
            this.employee = dataBase.getEmployeeById(this.IdEmployee);
            this.Text = "Сотрудник: " + this.employee.FName + ' ' + this.employee.MName + ' ' + this.employee.LName;

        }

        #endregion

        #region Work with client

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text.ToUpper();
            textBox1.Select(textBox1.Text.Length, 0);
        }

       

        //Новый клиент
        private void label9_Click(object sender, EventArgs e)
        {
            label10.Visible = label11.Visible = maskedTextBox1.Visible = textBox3.Visible=textBox5.Visible = true;
            label8.Text = "Фамилия";
            textBox5.Clear();
            textBox3.Clear();
            maskedTextBox1.Clear();
            comboBox7.Visible=false;
            IsClientNew = true;
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsOpened) return;
            setClientFullName();

        }

        #endregion

        #region Clear fields
        //сбросить поля авто
        private void button2_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex =comboBox2.SelectedIndex=comboBox3.SelectedIndex=comboBox4.SelectedIndex=comboBox5.SelectedIndex= -1;
            textBox1.Clear();
            textBox4.Clear();

        }

        //сбросить поля клиента
        private void button1_Click(object sender, EventArgs e)
        {
            comboBox7.SelectedIndex = -1;
            comboBox7.Visible = true;
            label10.Visible = label11.Visible = maskedTextBox1.Visible = textBox3.Visible = textBox5.Visible = false;
            label8.Text = "Поиск";
            maskedTextBox1.Text = textBox3.Text = textBox5.Text = "";
            IsClientNew = false;
        }

        //сбросить выбранные услуги
        private void button3_Click(object sender, EventArgs e)
        {
            comboBox6.SelectedIndex = comboBox9.SelectedIndex = -1;
            selectedServices.Rows.Clear();
            textBox6.Clear();

        }

        #endregion

       
        #region Working with service dgv

        //Заполнение dgv с услугами
        private void setTableInDGV()
        {
            //if (!IsOpened) return;
            selectedServices = new DataTable();
            selectedServices.Columns.Add(serviceTable.Columns[0].ColumnName, typeof(int));
            selectedServices.Columns.Add(serviceTable.Columns[1].ColumnName, typeof(string));
            selectedServices.Columns.Add(serviceTable.Columns[2].ColumnName, typeof(string));
            selectedServices.Columns.Add(serviceTable.Columns[3].ColumnName, typeof(string));


            dataGridView1.DataSource = selectedServices;

            dataGridView1.Columns[1].HeaderText = "Услуга";
            dataGridView1.Columns[2].HeaderText = "Цена";
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[3].Visible = false;

        }

        //Добавление услуг в dgv
        private void button6_Click(object sender, EventArgs e)
        {

            AddLineInDGV();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DeleteLineFromDGV();
        }

        private void AddLineInDGV()
        {
            try
            {

                DataRow row = selectedServices.NewRow();

                int idService = (int)comboBox6.SelectedValue;
                string name = comboBox6.Text;
                string price = serviceTable.Select(string.Format("IdService={0}", idService))[0].ItemArray[2].ToString();
                string idCategory = comboBox9.SelectedValue.ToString();

                row.ItemArray = new object[] { idService, name, price, idCategory };

                selectedServices.Rows.Add(row);
                textBox6.Text = CalculateTotalSum().ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении\n" + ex.Message);
            }
        }
       

        //Удаление 1 строки из dgv
        private void DeleteLineFromDGV()
        {
            if (dataGridView1.SelectedRows.Count == 0) return;
            selectedServices.Rows[dataGridView1.SelectedRows[0].Cells[0].RowIndex].Delete();
            textBox6.Text = CalculateTotalSum().ToString();

        }


        //Рассчёт общей стоимости услуг
        private double CalculateTotalSum()
        {
            double sum = 0;
            foreach (DataGridViewRow dGVr in dataGridView1.Rows)
            {
                sum += Convert.ToDouble(dGVr.Cells[2].Value);
            }
            return sum;
        }

        #endregion


        #region Create Order

        //Оформление заказа
        private void button5_Click(object sender, EventArgs e)
        {

            CreateOrder();

        }

        private void CreateOrder()
        {
            if (!CheckNullField())
            {
                MessageBox.Show("Некорректные данные! Пожалуйста проверьте правильность заполнения полей. Возможно не все поля были заполнены\n",
                     "Оформление услуги",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Warning);
                return;
            }

            SqlConnection sqlConnection = new SqlConnection(dataBase.connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            SqlTransaction transaction = sqlConnection.BeginTransaction();
            sqlCommand.Transaction = transaction;



            try
            {
                this.order = new Order();
                order.DateTime = DateTime.Now;
                order.Client = getIdClient(sqlCommand);
                order.Car = getIdDetail(sqlCommand);
                order.Employee = this.IdEmployee;
                order.Master = getIdMaster();
                order.TotalCost = float.Parse(textBox6.Text);
                order.Status = 1;
                order.Comment = textBox2.Text;

                dataBase.AddOrder(order, selectedServices, sqlCommand);

                transaction.Commit();
                if (this.IsClientNew)
                    this.clientsTable = dataBase.LoadClients();
                IsClientNew = false;
                MessageBox.Show("Заказ успешно оформлен!\n",
                     "Оформление услуг",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при оформлении заказа!\n" + ex.Message.ToString(),
                     "Оформление услуг",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
                transaction.Rollback();
            }

        }

        private int getIdClient(SqlCommand command)
        {
            int id;
            
            if (IsClientNew)
            {
                Client client = new Client();
                client.FName = textBox5.Text;
                client.MName = textBox3.Text;
                client.Phone = ConvertPhoneNumber(maskedTextBox1.Text);

                id = dataBase.AddNewClient(client,command);
            }
            else
            {

                id = (int)clientsTable.Select(string.Format("Phone={0}", ConvertPhoneNumber(maskedTextBox1.Text)))[0].ItemArray[0];
            }

            return id;
        }

        private int getIdDetail(SqlCommand command)
        {
            int id=-1;
            try
            {
                CarDetails carDetails = new CarDetails();
                carDetails.CarId = Convert.ToInt32(comboBox2.SelectedValue);
                carDetails.Year = Convert.ToInt32(comboBox3.SelectedValue);
                carDetails.Engine = Convert.ToInt32(comboBox4.SelectedValue);
                carDetails.Value = comboBox5.SelectedValue==null?null:comboBox5.SelectedValue.ToString();
                carDetails.VIN = textBox1.Text;
                carDetails.Mileage = textBox4.Text==string.Empty? 0:Convert.ToInt32(textBox4.Text);


                id = dataBase.AddCarDetails(carDetails,command);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при добавлении заказа!\n" + ex.Message.ToString(),
                   "Добавление заказа",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
            }
           

            return id;
        }

        private int getIdMaster()
        {
            string fname = comboBox8.SelectedItem.ToString().Split(new char[] { ' ' })[0];
            string mname = comboBox8.SelectedItem.ToString().Split(new char[] { ' ' })[1];
            string lname = comboBox8.SelectedItem.ToString().Split(new char[] { ' ' })[2];
            int id = (int)employeesTable.Select(string.Format("FName='{0}' AND MName='{1}' AND LName='{2}'",fname,mname,lname))[0].ItemArray[0];
            return id;
        }

        private string ConvertPhoneNumber(string phone)
        {
            string result="+"; 
            foreach (char c in phone.ToCharArray())
            {
                if (Char.IsDigit(c))
                {
                    result += c;
                }
            }
            return result;
        }


        private bool CheckNullField()
        {
            if (comboBox1.SelectedIndex == -1 ||
                comboBox2.SelectedIndex == -1 ||
                comboBox3.SelectedIndex == -1 ||
                textBox3.Text == string.Empty ||
                textBox5.Text == string.Empty ||
                maskedTextBox1.Text==string.Empty ||
                dataGridView1.Rows.Count==0)
                return false;
            else
                return true;
        }

        //Очистка всех полей
        private void ClearFields()
        {
            comboBox1.SelectedIndex = comboBox2.SelectedIndex =
            comboBox3.SelectedIndex = comboBox4.SelectedIndex =
            comboBox5.SelectedIndex = comboBox6.SelectedIndex =
            comboBox7.SelectedIndex = comboBox8.SelectedIndex =
            comboBox9.SelectedIndex = -1;
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            selectedServices.Rows.Clear();


            comboBox2.Enabled = false;
            comboBox6.Enabled = false;

            comboBox7.SelectedIndex = -1;
            comboBox7.Visible = true;
            label10.Visible = label11.Visible =
            maskedTextBox1.Visible = textBox3.Visible =
            textBox5.Visible = false;
            label8.Text = "Поиск";
            maskedTextBox1.Text = textBox3.Text = textBox5.Text = "";
            IsClientNew = false;
        }
        #endregion


        #region Stop input letter
        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8) //Если символ, введенный с клавы - не цифра (IsDigit),
            {
                e.Handled = true;// то событие не обрабатывается. ch!=8 (8 - это Backspace)
            }
        }

        private void comboBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8) //Если символ, введенный с клавы - не цифра (IsDigit),
            {
                e.Handled = true;// то событие не обрабатывается. ch!=8 (8 - это Backspace)
            }
        }

        private void comboBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8) //Если символ, введенный с клавы - не цифра (IsDigit),
            {
                e.Handled = true;// то событие не обрабатывается. ch!=8 (8 - это Backspace)
            }
        }

        private void comboBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsLetter(ch) && ch != 8) //Если символ, введенный с клавы - не цифра (IsDigit),
            {
                e.Handled = true;// то событие не обрабатывается. ch!=8 (8 - это Backspace)
            }
        }
        #endregion

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ViewOrdersForm viewOrders = new ViewOrdersForm();
            viewOrders.ShowDialog(this);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            InProgerssForm inProgerss = new InProgerssForm(5);   
            inProgerss.ShowDialog(this);
        }
    }
}
