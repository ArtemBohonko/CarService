using CarService.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CarService
{
    public partial class AdminForm : Form
    {
        bool IsOpened = false;
        private DB dataBase;

        private Employee employee;

        public DataTable carDataTable;
        public DataTable engineTable;
        public DataTable serviceTable;
        public DataTable categoryTable;
        public DataTable clientsTable;
        public DataTable employeesTable;
        public DataTable positionsTable;

        string addActions = "Добавить";
        string updateActions = "Обновить";

        public AdminForm(int idEmployee)
        {
            InitializeComponent();
            dataBase = new DB();
            employee = dataBase.getEmployeeById(idEmployee);
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            this.carDataTable = dataBase.LoadCars();
            
            this.serviceTable = dataBase.LoadServices();
            this.engineTable = dataBase.LoadEngine();
            this.categoryTable = dataBase.LoadCategories();
            this.clientsTable = dataBase.LoadClients();
            this.employeesTable = dataBase.LoadEmployees();
            this.positionsTable=dataBase.LoadPositions();

            setCarsInCB(this.carDataTable);
            setEngineInCB2(this.engineTable);
            setClientsInCB(this.clientsTable);
            setCategoriesInCB(this.categoryTable);
            setPositionsInCB(this.positionsTable);
            //setEmployeesInCB(this.employeesTable);

            Brand_comboBox.SelectedIndex = -1;
            Engine_comboBox.SelectedIndex = -1;
            Clients_comboBox.SelectedIndex = -1;
            Categories_comboBox.SelectedIndex = -1;
            Positions_comboBox.SelectedIndex = -1;
            Employees_comboBox.SelectedIndex = -1;
            IsOpened = true;
        }


        public void setCarsInCB(DataTable cars)
        {
            BindingSource source = new BindingSource();

            source.DataSource = cars.AsEnumerable()
            .Select(row => row["Brand"].ToString())
            .Distinct();

            Brand_comboBox.DataSource = source;

            Brand_comboBox.DisplayMember = "Brand";

            Brand_comboBox.SelectedIndex = -1;
        }

        private void setModelInCB(DataTable cars)
        {
            if (Brand_comboBox.SelectedIndex == -1)
            {
                Model_comboBox.SelectedIndex = -1;
                Model_comboBox.Enabled = false;
                return;
            }
            BindingSource source = new BindingSource();
            source.DataSource = cars;
            source.Filter = cars.Columns[1].ColumnName + "='" + Brand_comboBox.SelectedItem.ToString() + "'";
            Model_comboBox.Enabled = true;
            Model_comboBox.DataSource = source;
            Model_comboBox.DisplayMember = cars.Columns[2].ColumnName;
            Model_comboBox.ValueMember = cars.Columns[0].ColumnName;
            Model_comboBox.SelectedIndex = -1;
        }

        public void setEngineInCB2(DataTable engine)
        {
            Engine_comboBox.DataSource = engine;
            Engine_comboBox.DisplayMember = engine.Columns[1].ColumnName;
            Engine_comboBox.ValueMember = engine.Columns[0].ColumnName;
            Model_comboBox.SelectedIndex = -1;
        }

        public void setPositionsInCB(DataTable positions)
        {
            Positions_comboBox.DataSource = positions;
            Positions_comboBox.DisplayMember = positions.Columns[1].ColumnName;
            Positions_comboBox.ValueMember = positions.Columns[0].ColumnName;
            Positions_comboBox.SelectedIndex = -1;
        }

        public void setClientsInCB(DataTable clients)
        {
            List<string> clientsFullName = new List<string>();
            string fullName;

            foreach (DataRow row in clients.Rows)
            {
                fullName = row.ItemArray[1].ToString() + ' ' + row.ItemArray[2].ToString() + ' ' + row.ItemArray[3].ToString();
                clientsFullName.Add(fullName);
            }

            Clients_comboBox.DataSource = clientsFullName;
        }


        public void setCategoriesInCB(DataTable category)
        {
            Categories_comboBox.DataSource = category;
            Categories_comboBox.DisplayMember = category.Columns[1].ColumnName;
            Categories_comboBox.ValueMember = category.Columns[0].ColumnName;
        }
        private void setServiceInCB(DataTable services)
        {
            if (Categories_comboBox.SelectedIndex == -1)
            {
                Service_comboBox.SelectedIndex = -1;
                Service_comboBox.Enabled = false;
                return;
            }
            DataTable serviceWithPrice = new DataTable();
            serviceWithPrice.Columns.Add("Id", typeof(int));
            serviceWithPrice.Columns.Add("Service", typeof(string));
            serviceWithPrice.Columns.Add("CategoriId", typeof(string));
            DataRow newRow;
            object[] newItemAray;
            foreach (DataRow row in services.Rows)
            {
                newRow = serviceWithPrice.NewRow();
                newItemAray = new object[]
                {
                    Convert.ToInt32(row[0]),
                    row[1]+ "-"+ row[2].ToString(),
                    row[3].ToString(),
                };
                newRow.ItemArray = newItemAray;
                serviceWithPrice.Rows.Add(newRow);
                
            }
            BindingSource source = new BindingSource();
            source.DataSource = serviceWithPrice;
            source.Filter = serviceWithPrice.Columns[2].ColumnName + "='" + Categories_comboBox.SelectedValue.ToString() + "'";
            Service_comboBox.Enabled = true;
            Service_comboBox.DataSource = source;
            Service_comboBox.DisplayMember = serviceWithPrice.Columns[1].ColumnName;
            Service_comboBox.ValueMember = serviceWithPrice.Columns[0].ColumnName;
        }

        private void setEmployeesInCB(DataTable employees)
        {
            DataTable emp = new DataTable();
            emp.Columns.Add("Id", typeof(string));
            emp.Columns.Add("Name",typeof(string));


            DataRow newRow;
            string id,fullName;

            foreach (DataRow row in employees.Select(string.Format("{0}={1}", employees.Columns[5].ColumnName, Positions_comboBox.SelectedValue)))
            {
                newRow = emp.NewRow();
                id= row[0].ToString();
                fullName = row.ItemArray[1].ToString() + ' ' + row.ItemArray[2].ToString() + ' ' + row.ItemArray[3].ToString();
                newRow.ItemArray=new object[] {id,fullName};
                emp.Rows.Add(newRow);
            }

            Employees_comboBox.DataSource = emp;
            Employees_comboBox.DisplayMember = emp.Columns[1].ColumnName;
            Employees_comboBox.ValueMember = emp.Columns[0].ColumnName;
        }

        private void AddOrder_button_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm(employee.Id);
            mainForm.ShowDialog();
        }

        private void OpenOrders_button_Click(object sender, EventArgs e)
        {
            ViewOrdersForm viewOrdersForm = new ViewOrdersForm(employee.Id);
            viewOrdersForm.ShowDialog();
        }

        private void AddCar_button_Click(object sender, EventArgs e)
        {
            AddedUpdatedForm addedUpdatedForm = new AddedUpdatedForm(this, carDataTable, addActions);
            addedUpdatedForm.ShowDialog(this);
        }

        private void UpdateCar_button_Click(object sender, EventArgs e)
        {
            if (!CheckCarsField())
            {
                MessageBox.Show(
                           string.Format("Не все поля были заполнены! Проверьте поле \"Марка\""),
                           "Ошибка редактирвоания",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
                return;
            }
            List<string> car = new List<string>() { Brand_comboBox.Text, Model_comboBox.Text };
            AddedUpdatedForm addedUpdatedForm = new AddedUpdatedForm(this, carDataTable, car, updateActions);
            addedUpdatedForm.ShowDialog(this);
        }

        private bool CheckCarsField()
        {
            bool res = true;
            if (Brand_comboBox.Text == string.Empty)
                res = false;
            return res;
        }
        private bool CheckEngineField()
        {
            bool res = true;
            if (Engine_comboBox.Text == string.Empty)
                res = false;
            return res;
        }
        private bool CheckClientField()
        {
            bool res = true;
            if (Clients_comboBox.Text == string.Empty)
                res = false;
            return res;
        }
        private bool CheckCategoryField()
        {
            bool res = true;
            if (Categories_comboBox.Text == string.Empty)
                res = false;
            return res;
        }
        private bool CheckServiceyField()
        {
            bool res = true;
            if (Service_comboBox.Text == string.Empty)
                res = false;
            return res;
        }
        private void Brand_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Brand_comboBox.SelectedIndex != -1 && IsOpened)
            {
                setModelInCB(carDataTable);
            }
        }
        private bool CheckPositionField()
        {
            bool res = true;
            if (Positions_comboBox.Text == string.Empty)
                res = false;
            return res;
        }

        private bool CheckEmployeeField()
        {
            bool res = true;
            if (Employees_comboBox.Text == string.Empty)
                res = false;
            return res;
        }

        private void AddEngine_button_Click(object sender, EventArgs e)
        {
            AddedUpdatedForm addedUpdatedForm = new AddedUpdatedForm(this, engineTable, addActions);
            addedUpdatedForm.ShowDialog(this);
        }

        private void UpdateEngine_button_Click(object sender, EventArgs e)
        {
            if (!CheckEngineField())
            {
                MessageBox.Show(
                           string.Format("Не все поля были заполнены! Проверьте поле \"Тип двигателя\""),
                           "Ошибка редактирвоания",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
                return;
            }
            List<string> engine = new List<string>() { Engine_comboBox.Text};
            AddedUpdatedForm addedUpdatedForm = new AddedUpdatedForm(this, engineTable,engine, updateActions);
            addedUpdatedForm.ShowDialog(this);
        }

        private void AddClient_button_Click(object sender, EventArgs e)
        {
            AddedUpdatedForm addedUpdatedForm = new AddedUpdatedForm(this, clientsTable, addActions);
            addedUpdatedForm.ShowDialog(this);
        }

        private void UpdateClient_button_Click(object sender, EventArgs e)
        {
            if (!CheckClientField())
            {
                MessageBox.Show(
                           string.Format("Не все поля были заполнены! Проверьте поле \"Клиент\""),
                           "Ошибка редактирвоания",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
                return;
            }
            List<string> client = new List<string>() { Clients_comboBox.Text };
            AddedUpdatedForm addedUpdatedForm = new AddedUpdatedForm(this, clientsTable, client, updateActions);
            addedUpdatedForm.ShowDialog(this);
        }

        private void Categories_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Categories_comboBox.SelectedIndex != -1 && IsOpened)
            {
                setServiceInCB(serviceTable);
                Service_comboBox.SelectedIndex = -1;
            }
            
        }

        private void AddService_button_Click(object sender, EventArgs e)
        {
            if (!CheckCategoryField())
            {
                MessageBox.Show(
                           string.Format("Не все поля были заполнены! Проверьте поле \"Категория\""),
                           "Ошибка редактирвоания",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
                return;
            }
            List<string> service = new List<string>() { Categories_comboBox.SelectedValue.ToString() };

            AddedUpdatedForm addedUpdatedForm = new AddedUpdatedForm(this, serviceTable,service, addActions);
            addedUpdatedForm.ShowDialog(this);
        }
        private void UpdateService_button_Click(object sender, EventArgs e)
        {
            if (!CheckServiceyField())
            {
                MessageBox.Show(
                           string.Format("Не все поля были заполнены! Проверьте поле \"Услуга\""),
                           "Ошибка редактирвоания",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
                return;
            }
            string expression = string.Format("{0}={1}", serviceTable.Columns[0].ColumnName, Service_comboBox.SelectedValue);
            string serviceName= serviceTable.Select(expression)[0].ItemArray[1].ToString();
            string price = serviceTable.Select(expression)[0].ItemArray[2].ToString();
            string category = serviceTable.Select(expression)[0].ItemArray[3].ToString();
            List<string> service = new List<string>() { serviceName,price,category };
            AddedUpdatedForm addedUpdatedForm = new AddedUpdatedForm(this, serviceTable, service, updateActions);
            addedUpdatedForm.ShowDialog(this);
        }
        private void AddCategory_button_Click(object sender, EventArgs e)
        {
            AddedUpdatedForm addedUpdatedForm = new AddedUpdatedForm(this, categoryTable, addActions);
            addedUpdatedForm.ShowDialog(this);
        }

        private void UpdateCategory_button_Click(object sender, EventArgs e)
        {
            if (!CheckCategoryField())
            {
                MessageBox.Show(
                           string.Format("Не все поля были заполнены! Проверьте поле \"Категория\""),
                           "Ошибка редактирвоания",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
                return;
            }
            List<string> category = new List<string>() { Categories_comboBox.Text };
            AddedUpdatedForm addedUpdatedForm = new AddedUpdatedForm(this, categoryTable, category, updateActions);
            addedUpdatedForm.ShowDialog(this);
        }

        private void AddPosition_button_Click(object sender, EventArgs e)
        {
            AddedUpdatedForm addedUpdatedForm = new AddedUpdatedForm(this, positionsTable, addActions);
            addedUpdatedForm.ShowDialog(this);
        }
        private void UpdatePosition_button_Click(object sender, EventArgs e)
        {
            if (!CheckPositionField())
            {
                MessageBox.Show(
                           string.Format("Не все поля были заполнены! Проверьте поле \"Название должности\""),
                           "Ошибка редактирования",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
                return;
            }
            List<string> position = new List<string>() { Positions_comboBox.Text };
            AddedUpdatedForm addedUpdatedForm = new AddedUpdatedForm(this, positionsTable,position, updateActions);
            addedUpdatedForm.ShowDialog(this);
        }

        private void AddEmployee_button_Click(object sender, EventArgs e)
        {
            if (!CheckPositionField())
            {
                MessageBox.Show(
                           string.Format("Не все поля были заполнены! Проверьте поле \"Должность\""),
                           "Ошибка добавления",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
                return;
            }
            positionsTable.TableName = "Employees";
            AddedUpdatedForm addedUpdatedForm = new AddedUpdatedForm(this, positionsTable, addActions);
            addedUpdatedForm.ShowDialog(this);
            positionsTable.TableName = "Positions";
        }

        private void UpdateEmployee_button_Click(object sender, EventArgs e)
        {
            if (!CheckEmployeeField())
            {
                MessageBox.Show(
                           string.Format("Не все поля были заполнены! Проверьте поле \"Сотрудник\""),
                           "Ошибка редактирования",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
                return;
            }

            string expression = string.Format("{0}={1}", employeesTable.Columns[0].ColumnName, Employees_comboBox.SelectedValue.ToString());
            DataRow row = employeesTable.Select(expression)[0];

            string fname = row.ItemArray[1].ToString();
            string mname = row.ItemArray[2].ToString();
            string lname = row.ItemArray[3].ToString();
            string phone = row.ItemArray[4].ToString();
            string positionId = row.ItemArray[5].ToString();
            string password = row.ItemArray[6].ToString();

            List<string> emp = new List<string>() { fname,mname,lname,phone,positionId,password };
            positionsTable.TableName = "Employees";
            AddedUpdatedForm addedUpdatedForm = new AddedUpdatedForm(this, positionsTable, emp, updateActions);
            addedUpdatedForm.ShowDialog(this);
            positionsTable.TableName = "Positions";

        }


        private void Positions_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsOpened || Positions_comboBox.SelectedIndex==-1) return;
            setEmployeesInCB(employeesTable);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            StatisticForm statisticForm = new StatisticForm();
            statisticForm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InProgerssForm inProgerssForm = new InProgerssForm(employee.Id);
            inProgerssForm.ShowDialog();
        }
    }
}
