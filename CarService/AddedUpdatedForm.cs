using CarService.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CarService
{
    public partial class AddedUpdatedForm : Form
    {
        DB dataBase;
        DataTable dataTable;
        string action;
        string addActions = "Добавить";
        string updateActions = "Обновить";
        bool IsOpened = false;
        AdminForm adminForm;
        List<string> updateDataList;
        public AddedUpdatedForm(AdminForm ownerForm,DataTable dataTable, string action)
        {
            InitializeComponent();
            dataBase=new DB();
            this.dataTable = dataTable;
            this.action = action;
            this.adminForm = ownerForm;
        }

        public AddedUpdatedForm(AdminForm ownerForm,DataTable dataTable, List<string> list, string action)
        {
            InitializeComponent();
            dataBase = new DB();
            this.dataTable = dataTable;
            this.action = action;
            this.updateDataList = list;
            this.adminForm = ownerForm;
        }



        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(dataTable.TableName=="Cars")
                SetAddCarParametrs(checkBox1.Checked);
           
        }

        private void AddedUpdatedForm_Load(object sender, EventArgs e)
        {
            if (dataTable.TableName == "Cars" && action==addActions)
            {
                SetAddCarParametrs(checkBox1.Checked);
                this.Text = "Добавление автомобилей";

            }
            else if(dataTable.TableName == "Cars" && action == updateActions)
            {
                SetUpdateCarParametrs();
                this.Text = "Редактирование автомобилей";



            }
            else if (dataTable.TableName=="Engine" && action == addActions)
            {
                SetAddEngineParametrs();
                this.Text = "Добавление типа двигателя";
            }
            else if (dataTable.TableName == "Engine" && action == updateActions)
            {
                SetUpdateEngineParametrs();
                this.Text = "Редактирование типа двигателя";
            }
            else if (dataTable.TableName == "Clients" && action == addActions)
            {
                SetAddClientsParametrs();
                this.Text = "Добавление клиента";
            }
            else if (dataTable.TableName == "Clients" && action == updateActions)
            {
                SetAddClientsParametrs();
                setClientFullName(updateDataList[0]);
                this.Text = "Редактирование данных о клиенте клиенте";
            }
            else if (dataTable.TableName == "Categories" && action == addActions)
            {
                SetAddCategoryParametrs(); ;
                this.Text = "Добавление категории услуг";
            }
            else if (dataTable.TableName == "Categories" && action == updateActions)
            {
                SetUpdateCategoryeParametrs();
                this.Text = "Редактирование категории услуг";
            }
            else if (dataTable.TableName == "Services" && action == addActions)
            {
                SetAddServiceParametrs();
                this.Text = "Добавление услуги";
            }
            else if (dataTable.TableName == "Services" && action == updateActions)
            {
                SetUpdateServiceParametrs();
                this.Text = "Редактирование данных об услуге";
            }
            else if (dataTable.TableName == "Positions" && action == addActions)
            {
                setAddPositionParametrs();
                this.Text = "Добавление должности";
            }
            else if (dataTable.TableName == "Positions" && action == updateActions)
            {
                SetUpdatePositionParametrs();
                this.Text = "Редактирование данных об должности";
            }
            else if (dataTable.TableName == "Employees" && action == addActions)
            {
                setAddEmployeeParametrs();
                this.Text = "Добавление сотрудника";
            }
            else if (dataTable.TableName == "Employees" && action == updateActions)
            {
                SetUpdateEmployeeParametrs();
                this.Text = "Редактирование данных о сотруднике";
            }
            else
            {
                
               
            }
            button1.Text = action;
            IsOpened = true;
        }

        #region Set parametrs
        private void setAddEmployeeParametrs()
        {
            groupBox1.Visible = false;
            groupBox2.Visible = true;
            groupBox2.Dock= DockStyle.Fill;

            DataTable employeesTableDGV = new DataTable();
            employeesTableDGV.Columns.Add("FName", typeof(string));
            employeesTableDGV.Columns.Add("MNane", typeof(string));
            employeesTableDGV.Columns.Add("LNane", typeof(string));
            employeesTableDGV.Columns.Add("Phone", typeof(string));
            employeesTableDGV.Columns.Add("Position", typeof(int));
            employeesTableDGV.Columns.Add("Password",typeof(string));
            dataGridView1.DataSource = employeesTableDGV;
            dataGridView1.Columns[0].HeaderText = "Фамилия";
            dataGridView1.Columns[1].HeaderText = "Имя";
            dataGridView1.Columns[2].HeaderText = "Отчество";
            dataGridView1.Columns[3].HeaderText = "Телефон";
            dataGridView1.Columns[5].HeaderText = "Пароль";
            DataGridViewComboBoxColumn col4 = new DataGridViewComboBoxColumn();
            col4.HeaderText = "Должность";
            col4.DataSource=dataTable;
            col4.DataPropertyName = "Position";
            col4.DisplayMember = dataTable.Columns[1].ColumnName;
            col4.ValueMember = dataTable.Columns[0].ColumnName;
            col4.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
            col4.FlatStyle = FlatStyle.Flat;
            dataGridView1.Columns.Add(col4);


            button2.Text = addActions;
           

        }
        private void SetUpdateEmployeeParametrs()
        {
            //updateDataList (fname, mname, lname, phone, position, password)
            groupBox1.Visible = false;
            groupBox2.Visible = true;
            groupBox2.Dock = DockStyle.Fill;


            DataTable employeesTableDGV = new DataTable();
            employeesTableDGV.Columns.Add("FName", typeof(string));
            employeesTableDGV.Columns.Add("MNane", typeof(string));
            employeesTableDGV.Columns.Add("LNane", typeof(string));
            employeesTableDGV.Columns.Add("Phone", typeof(string));
            employeesTableDGV.Columns.Add("Position", typeof(int));
            employeesTableDGV.Columns.Add("Password", typeof(string));
            DataRow newRow = employeesTableDGV.NewRow();
            object[] itemArray = new object[]{
            updateDataList[0],updateDataList[1],updateDataList[2],updateDataList[3],updateDataList[4], updateDataList[5]
            };
            newRow.ItemArray = itemArray;
            employeesTableDGV.Rows.Add(newRow);

            dataGridView1.DataSource = employeesTableDGV;
            dataGridView1.Columns[0].HeaderText = "Фамилия";
            dataGridView1.Columns[1].HeaderText = "Имя";
            dataGridView1.Columns[2].HeaderText = "Отчество";
            dataGridView1.Columns[3].HeaderText = "Телефон";
            dataGridView1.Columns[4].HeaderText = "Должность";
            dataGridView1.Columns[5].HeaderText = "Пароль";
            DataGridViewComboBoxColumn col4 = new DataGridViewComboBoxColumn();
            col4.DataSource = dataTable;
            col4.DataPropertyName = "Position";
            col4.DisplayMember = dataTable.Columns[1].ColumnName;
            col4.ValueMember = dataTable.Columns[0].ColumnName;
            col4.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
            col4.FlatStyle = FlatStyle.Flat;
            dataGridView1.Columns.Add(col4);

            dataGridView1.AllowUserToAddRows = false;

            button2.Text = updateActions;


        }

        private void setAddPositionParametrs()
        {
            groupBox1.Visible = true;
            groupBox2.Visible = false;
            groupBox1.Dock = DockStyle.Fill;
            setPositionsInCB(dataTable);
            comboBox1.DropDownStyle = ComboBoxStyle.DropDown;
            comboBox2.Visible = false;
            checkBox1.Visible = false;
            button1.Text = addActions;
            label1.Text = "Название должности";
            label2.Visible = false;
            groupBox1.Text = "Должности";
            comboBox1.SelectedIndex = -1;
            comboBox3.Visible = label3.Visible = false;
            maskedTextBox1.Visible = label4.Visible = false;
            comboBox4.Visible = false;

            
        }
        private void SetUpdatePositionParametrs()
        {
            groupBox2.Visible = false;
            groupBox1.Dock = DockStyle.Fill;
            comboBox1.AutoCompleteMode = comboBox2.AutoCompleteMode = AutoCompleteMode.None;
            comboBox1.AutoCompleteSource = comboBox2.AutoCompleteSource = AutoCompleteSource.None;
            comboBox1.Visible = label1.Visible = true;
            comboBox2.Visible = label2.Visible = false;
            comboBox3.Visible = label3.Visible = false;
            maskedTextBox1.Visible = comboBox4.Visible = label4.Visible = false;
            checkBox1.Visible = false;

            label1.Text = "Название должности";

            comboBox1.DropDownStyle = ComboBoxStyle.DropDown;

            comboBox1.Items.Add(updateDataList[0]);
            comboBox1.SelectedIndex = 0;

        }
        private void SetAddClientsParametrs()
        {
            groupBox2.Visible = false;
            groupBox1.Dock = DockStyle.Fill;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDown;
            comboBox2.Visible = true;
            checkBox1.Visible = false;
            button1.Text = addActions;
            label1.Text = "Фамилия";
            label2.Visible = true;
            label2.Text = "Имя";
            groupBox1.Text = "Клиент";
            comboBox1.SelectedIndex = -1;
            comboBox3.Visible = label3.Visible = false;
            maskedTextBox1.Visible = label4.Visible = true;
            label4.Text = "Номер телефона";
            comboBox4.Visible = false;
        }

        private void SetAddEngineParametrs()
        {
            groupBox2.Visible = false;
            groupBox1.Dock = DockStyle.Fill;
            setEngineInCB(dataTable);
            comboBox1.DropDownStyle= ComboBoxStyle.DropDown;
            comboBox2.Visible = false;
            checkBox1.Visible = false;
            button1.Text = addActions;
            label1.Text = "Тип двигателя";
            label2.Visible = false;
            groupBox1.Text = "Двигатель";
            comboBox1.SelectedIndex = -1;
            comboBox3.Visible = label3.Visible = false;
            maskedTextBox1.Visible = label4.Visible = false;
            comboBox4.Visible = false;

        }

        private void SetUpdateEngineParametrs()
        {
            groupBox2.Visible = false;
            groupBox1.Dock = DockStyle.Fill;
            comboBox1.AutoCompleteMode = comboBox2.AutoCompleteMode = AutoCompleteMode.None;
            comboBox1.AutoCompleteSource = comboBox2.AutoCompleteSource = AutoCompleteSource.None;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDown;
            comboBox1.Items.Add(updateDataList[0]);
            comboBox1.SelectedIndex = 0;
            comboBox2.Visible = false;
            checkBox1.Visible = false;
            button1.Text = updateActions;
            label1.Text = "Тип двигателя";
            label2.Visible = false;
            groupBox1.Text = "Двигатель";
            comboBox3.Visible = label3.Visible = false;
            maskedTextBox1.Visible = label4.Visible = false;
            comboBox4.Visible = false;

        }

        private void SetAddCarParametrs(bool checkBoxValue)
        {
            groupBox2.Visible = false;
            groupBox1.Dock = DockStyle.Fill;
            checkBox1.Visible = label1.Visible= label2.Visible= true;
            comboBox1.Visible = comboBox2.Visible = true;
            comboBox3.Visible = label3.Visible = false;
            maskedTextBox1.Visible = label4.Visible = false;
            comboBox4.Visible = false;
            checkBox1.Text = "Новая марка";

            label1.Text = "Марка";
            label2.Text = "Модель";
            setCarsInCB(dataTable);
           
            if (checkBoxValue)
            {
                comboBox1.DropDownStyle = ComboBoxStyle.DropDown;
                comboBox1.SelectedIndex = comboBox2.SelectedIndex=-1;
            }

            else
            {
                comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox1.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;

            }
        }
        private void SetUpdateCarParametrs()
        {
            checkBox1.Visible = false;
            groupBox2.Visible = false;
            groupBox1.Dock = DockStyle.Fill;
            checkBox1.Visible = label1.Visible = label2.Visible = true;
            comboBox1.Visible = comboBox2.Visible = true;
            comboBox3.Visible = label3.Visible = false;
            maskedTextBox1.Visible = label4.Visible = false;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDown;
            comboBox1.Items.Add(updateDataList[0]);
            comboBox1.SelectedIndex = 0;
            comboBox4.Visible = false;
            comboBox1.AutoCompleteMode = comboBox2.AutoCompleteMode = AutoCompleteMode.None;
            comboBox1.AutoCompleteSource = comboBox2.AutoCompleteSource = AutoCompleteSource.None;
            if (updateDataList[1] == string.Empty)
                comboBox2.Enabled = false;
            else
            {
                comboBox2.Items.Add(updateDataList[1]);
                comboBox2.SelectedIndex = 0;
            }
        }

        private void SetAddServiceParametrs()
        {
            groupBox2.Visible = false;
            groupBox1.Dock = DockStyle.Fill;
            comboBox1.Visible = label1.Visible = true;
            comboBox2.Visible = label2.Visible = true;
            comboBox3.Visible=label3.Visible = false;
            maskedTextBox1.Visible = comboBox4.Visible = label4.Visible = false;
            checkBox1.Visible = false;

            label1.Text = "Услуга";
            label2.Text = "Цена";

            comboBox1.DropDownStyle = comboBox2.DropDownStyle = ComboBoxStyle.DropDown;

        }

        private void SetUpdateServiceParametrs()

        {
            groupBox2.Visible = false;
            groupBox1.Dock = DockStyle.Fill;
            comboBox1.Visible = label1.Visible = true;
            comboBox2.Visible = label2.Visible = true;
            comboBox3.Visible = label3.Visible = false;
            maskedTextBox1.Visible = comboBox4.Visible = label4.Visible = false;
            checkBox1.Visible = false;

            label1.Text = "Услуга";
            label2.Text = "Цена";

            comboBox1.DropDownStyle = comboBox2.DropDownStyle= ComboBoxStyle.DropDown;
            comboBox1.AutoCompleteMode= comboBox2.AutoCompleteMode= AutoCompleteMode.None;
            comboBox1.AutoCompleteSource=comboBox2.AutoCompleteSource=AutoCompleteSource.None;
            comboBox1.Items.Add(updateDataList[0]);
            comboBox2.Items.Add(updateDataList[1]);
            comboBox1.SelectedIndex = comboBox2.SelectedIndex = 0;
            
        }

        private void SetAddCategoryParametrs()
        {
            groupBox2.Visible = false;
            groupBox1.Dock = DockStyle.Fill;
            setCategoriesInCB(dataTable);

            comboBox1.Visible = label1.Visible = true;
            comboBox2.Visible = label2.Visible = false;
            comboBox3.Visible = label3.Visible = false;
            maskedTextBox1.Visible = comboBox4.Visible = label4.Visible = false;
            checkBox1.Visible = false;

            label1.Text = "Услуга";

            comboBox1.DropDownStyle = ComboBoxStyle.DropDown;
            comboBox1.SelectedIndex = -1;
        }

        private void SetUpdateCategoryeParametrs()
        {
            groupBox2.Visible = false;
            groupBox1.Dock = DockStyle.Fill;
            comboBox1.AutoCompleteMode = comboBox2.AutoCompleteMode = AutoCompleteMode.None;
            comboBox1.AutoCompleteSource = comboBox2.AutoCompleteSource = AutoCompleteSource.None;
            comboBox1.Visible = label1.Visible = true;
            comboBox2.Visible = label2.Visible = false;
            comboBox3.Visible = label3.Visible = false;
            maskedTextBox1.Visible = comboBox4.Visible = label4.Visible = false;
            checkBox1.Visible = false;

            label1.Text = "Категория";

            comboBox1.DropDownStyle = ComboBoxStyle.DropDown;

            comboBox1.Items.Add(updateDataList[0]);
            comboBox1.SelectedIndex = 0;

        }
        #endregion

        #region Set catalogs
        private void setCategoriesInCB(DataTable category)
        {
            comboBox1.DataSource = category;
            comboBox1.DisplayMember = category.Columns[1].ColumnName;
            comboBox1.ValueMember = category.Columns[0].ColumnName;
        }
        private void setCarsInCB(DataTable cars)
        {
            BindingSource source = new BindingSource();

            source.DataSource = cars.AsEnumerable()
            .Select(row => row[cars.Columns[1].ColumnName].ToString())
            .Distinct();

            comboBox1.DataSource = source;

            comboBox1.DisplayMember = cars.Columns[1].ColumnName;
        }
        private void setEngineInCB(DataTable engine)
        {
            comboBox1.DataSource = engine;
            comboBox1.DisplayMember = engine.Columns[1].ColumnName;
            comboBox1.ValueMember = engine.Columns[0].ColumnName;
        }
        private void setModelInCB(DataTable cars)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                comboBox2.SelectedIndex = -1;
                return;
            }
            BindingSource source = new BindingSource();
            source.DataSource = cars;
            source.Filter = cars.Columns[1].ColumnName + "='" + comboBox1.SelectedItem.ToString() + "'";
            comboBox2.Enabled = true;
            comboBox2.DataSource = source;
            comboBox2.DisplayMember = cars.Columns[2].ColumnName;
            comboBox2.ValueMember = cars.Columns[0].ColumnName;
            if (action == updateActions)
                comboBox2.SelectedIndex = -1;
        }

        private void setClientFullName(string client)
        {

            string name = client.Split(new char[] { ' ' })[0];
            string surname = client.Split(new char[] { ' ' })[1];
            string phone = client.Split(new char[] { ' ' })[2];
            comboBox1.Text = name;
            comboBox2.Text = surname;
            maskedTextBox1.Text = phone;
        }

        public void setPositionsInCB(DataTable positions)
        {
            comboBox1.DataSource = positions;
            comboBox1.DisplayMember = positions.Columns[1].ColumnName;
            comboBox1.ValueMember = positions.Columns[0].ColumnName;
            comboBox1.SelectedIndex = -1;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex!=-1&&IsOpened && action=="Cars")
                setModelInCB(dataTable);
            
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataTable.TableName == "Cars")
            {
                AddUpdateCars();
            }
            else if (dataTable.TableName == "Engine")
            {
                AddUpdateEngine();
            }
            else if (dataTable.TableName == "Clients")
            {
                AddUpdateClient();
            }
            else if (dataTable.TableName == "Categories")
            {
                AddUpdateCategory();
            }
            else if (dataTable.TableName == "Services")
            {
                AddUpdateService();
            }
            else if (dataTable.TableName == "Positions")
            {
                AddUpdatePositions();
            }
           
            

        }

        #region Add Update Cars
        private void AddUpdateCars()
        {
            string expression;
            string caption;

            Car oldCar = new Car
            {
                Brand = updateDataList==null?"-1":updateDataList[0],
                Model = updateDataList==null?"-1": updateDataList[1],
            };
            
            

            Car newCar = new Car()
            {
                Brand = comboBox1.Text,
                Model = comboBox2.Text,

            };

            if (action == addActions)
            {
                expression = string.Format("Добавить новый автомобиль ({0} {1})?", newCar.Brand, newCar.Model);
                caption = "Подтверждение добавления";
            }
            else
            {

                expression = string.Format("Изменить данные об автомобиле ({0} {1} -> {2} {3})?", oldCar.Brand, oldCar.Model, newCar.Brand, newCar.Model);
                caption = "Подтверждение редактирования";
            }

            if (action==addActions && !CheckAddCarFields())
            {
                return;
            }
            else if(action==updateActions && !CheckUpdateCarFields())
            {
                return;

            }

            DialogResult dialog = MessageBox.Show(
                           expression,
                           caption,
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
            if (dialog == DialogResult.No) return;

            string result;

            if (action == addActions)
            {
                result = dataBase.AddCar(newCar);
                if (result == "OK")
                {
                    MessageBox.Show(
                           string.Format("Автомобиль успешно добавлен!"),
                           "Результат добавления автомобиля",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
                    this.adminForm.carDataTable = dataBase.LoadCars();
                    adminForm.setCarsInCB(adminForm.carDataTable);
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                           string.Format("В результате добваления автомобиля произошла ошибка!\n", result),
                           "Результат добавления автомобиля",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error,
                           MessageBoxDefaultButton.Button1);
                }
                comboBox1.SelectedIndex = comboBox2.SelectedIndex = -1;
            }
            else
            {
                result = dataBase.UpdateCar(oldCar,newCar);
                if (result == "OK")
                {
                    MessageBox.Show(
                           string.Format("Данные об автомобиле успешно отредактировы!"),
                           "Результат редактирования данных об автомобиля",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
                    this.adminForm.carDataTable = dataBase.LoadCars();
                    adminForm.setCarsInCB(adminForm.carDataTable);
                    this.Close();


                }
                else
                {
                    MessageBox.Show(
                           string.Format("В результате редактирвоания данных об автомобиле произошла ошибка!\n", result),
                           "Результат редактирования данных об автомобиля",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error,
                           MessageBoxDefaultButton.Button1);

                }
                comboBox1.SelectedIndex = comboBox2.SelectedIndex = -1;
            }
        }

        private bool CheckAddCarFields()
        {
            bool res = true;
            if(comboBox1.Text==string.Empty)
            {
                MessageBox.Show(
                          string.Format("Не все поля были заполнены! Проверьте поле \"Марка\""),
                          "Неверные данные",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information,
                          MessageBoxDefaultButton.Button1);
                res= false;
            }
            return res;

        }

        private bool CheckUpdateCarFields()
        {
            bool res;
            if (comboBox1.Text == string.Empty && updateDataList[1] == string.Empty)
            {
                MessageBox.Show(
                          string.Format("Не все поля были заполнены! Проверьте поле \"Марка\""),
                          "Неверные данные",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information,
                          MessageBoxDefaultButton.Button1);
                res = false;
            }
            else if (comboBox1.Text == string.Empty && comboBox2.Text == string.Empty && updateDataList[1] != string.Empty)
            {
                MessageBox.Show(
                         string.Format("Не все поля были заполнены! Проверьте поля \"Марка\" и \"Модель\""),
                         "Неверные данные",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Information,
                         MessageBoxDefaultButton.Button1);
                res = false;
            }
            else
                res = true;

            return res;

        }

        #endregion

        #region Add Update Engine
        private void AddUpdateEngine()
        {
            Engine oldEngine = new Engine()
            {
                Name = updateDataList==null?"-1": updateDataList[0],
            };
            Engine newEngine = new Engine()
            {
                Name = comboBox1.Text,
            };

            string expression;
            string caption;
            if (action == addActions)
            {
                expression = string.Format("Добавить новый тип двигателя ( {0} )?", newEngine.Name);
                caption = "Подтверждение добавления";
            }
            else
            {

                expression = string.Format("Изменить данные о типе двигателя ({0} -> {1})?",oldEngine.Name, newEngine.Name);
                caption = "Подтверждение редактирования";
            }

            if (!CheckAddEngineFields())
            {
                return;
            }
      

            DialogResult dialog = MessageBox.Show(
                           expression,
                           caption,
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
            if (dialog == DialogResult.No) return;

            string result;

            if (action == addActions)
            {
                result = dataBase.AddEngine(newEngine);
                if (result == "OK")
                {
                    MessageBox.Show(
                           string.Format("Тип двигателя успешно добавлен!"),
                           "Результат добавления двигателя",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
                    this.adminForm.engineTable = dataBase.LoadEngine();
                    adminForm.setEngineInCB2(adminForm.engineTable);
                    adminForm.Engine_comboBox.SelectedIndex = -1;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                           string.Format("В результате добваления автомобиля произошла ошибка!\n", result),
                           "Результат добавления автомобиля",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error,
                           MessageBoxDefaultButton.Button1);
                }
                comboBox1.SelectedIndex = comboBox2.SelectedIndex = -1;
            }
            else
            {
                result = dataBase.UpdateEngine(oldEngine, newEngine);
                if (result == "OK")
                {
                    MessageBox.Show(
                           string.Format("Данные о типе двигателя успешно отредактировы!"),
                           "Результат редактирования данных о типе двигателя",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
                    this.adminForm.engineTable = dataBase.LoadEngine();
                    adminForm.setEngineInCB2(adminForm.engineTable);
                    adminForm.Engine_comboBox.SelectedIndex = -1;
                    this.Close();


                }
                else
                {
                    MessageBox.Show(
                           string.Format("В результате редактирвоания данных о типе двигателя произошла ошибка!\n", result),
                           "Результат редактирования данных о типе двигателя",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error,
                           MessageBoxDefaultButton.Button1);

                }
                comboBox1.SelectedIndex=-1;
            }
        }

        private bool CheckAddEngineFields()
        {
            bool res = true;
            if (comboBox1.Text == string.Empty)
            {
                MessageBox.Show(
                          string.Format("Не все поля были заполнены! Проверьте поле \"Тип двигателя\""),
                          "Неверные данные",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information,
                          MessageBoxDefaultButton.Button1);
                res = false;
            }
            return res;

        }
        #endregion

        #region Add Update Client
        private void AddUpdateClient()
        {
            Client oldClient = new Client()
            {
                FName = updateDataList == null ? "-1" : updateDataList[0].Split(new char[] { ' ' })[0],
                MName = updateDataList == null ? "-1" : updateDataList[0].Split(new char[] { ' ' })[1],
                Phone = updateDataList == null ? "-1" : updateDataList[0].Split(new char[] { ' ' })[2],
            };
            Client newClient = new Client()
            {
                FName = comboBox1.Text,
                MName = comboBox2.Text,
                Phone = ConvertPhoneNumber(maskedTextBox1.Text),
            };

            string expression;
            string caption;
            if (action == addActions)
            {
                expression = string.Format("Добавить нового клиентая ( {0} {1} {2} )?", newClient.FName,newClient.MName,newClient.Phone);
                caption = "Подтверждение добавления";
            }
            else
            {

                expression = string.Format("Изменить данные о типе двигателя ({0} {1} {2} -> {3} {4} {5})?", oldClient.FName,oldClient.MName,oldClient.Phone,newClient.FName,newClient.MName,newClient.Phone);
                caption = "Подтверждение редактирования";
            }

            if (!CheckAddClientFields())
            {
                return;
            }


            DialogResult dialog = MessageBox.Show(
                           expression,
                           caption,
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
            if (dialog == DialogResult.No) return;

            string result;

            if (action == addActions)
            {
                result = dataBase.AddClient(newClient);
                if (result == "OK")
                {
                    MessageBox.Show(
                           string.Format("Клиент успешно добавлен!"),
                           "Результат добавления клиента",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
                    this.adminForm.clientsTable = dataBase.LoadClients();
                    adminForm.setClientsInCB(adminForm.clientsTable);
                    adminForm.Clients_comboBox.SelectedIndex = -1;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                           string.Format("В результате добваления клиента произошла ошибка!\n", result),
                           "Результат добавления клиента",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error,
                           MessageBoxDefaultButton.Button1);
                }
                comboBox1.SelectedIndex = comboBox2.SelectedIndex = -1;
                maskedTextBox1.Text = string.Empty;
            }
            else
            {
                result = dataBase.UpdateClient(oldClient, newClient);
                if (result == "OK")
                {
                    MessageBox.Show(
                           string.Format("Данные о клиенте успешно отредактировы!"),
                           "Результат редактирования данных о клиенте",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
                    this.adminForm.clientsTable = dataBase.LoadClients();
                    adminForm.setClientsInCB(adminForm.clientsTable);
                    adminForm.Clients_comboBox.SelectedIndex = -1;
                    this.Close();


                }
                else
                {
                    MessageBox.Show(
                           string.Format("В результате редактирвоания данных о клиенте произошла ошибка!\n", result),
                           "Результат редактирования данных о клиенте",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error,
                           MessageBoxDefaultButton.Button1);

                }
                comboBox1.SelectedIndex = comboBox2.SelectedIndex= -1;
            }
        }

        private bool CheckAddClientFields()
        {
            bool res = true;
            if (comboBox1.Text == string.Empty || comboBox2.Text==string.Empty || maskedTextBox1.Text==string.Empty)
            {
                MessageBox.Show(
                          string.Format("Не все поля были заполнены! Проверьте поле \"Фамилия\" \"Имя\" \"Номер телефона\""),
                          "Неверные данные",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information,
                          MessageBoxDefaultButton.Button1);
                res = false;
            }
            return res;

        }

        private string ConvertPhoneNumber(string phone)
        {
            string result = "+";
            foreach (char c in phone.ToCharArray())
            {
                if (Char.IsDigit(c))
                {
                    result += c;
                }
            }
            return result;
        }
        #endregion

        #region Add Update Category
        private void AddUpdateCategory()
        {
            Category oldCategory = new Category()
            {
                Name = updateDataList==null?"-1": updateDataList[0]
            };
            Category newCategory = new Category()
            {
                Name = comboBox1.Text
            };

            

            string expression;
            string caption;
            if (action == addActions)
            {
                expression = string.Format("Добавить новую категорию ( {0} )?", newCategory.Name);
                caption = "Подтверждение добавления";
            }
            else
            {

                expression = string.Format("Изменить данные об категории услуг ({0} -> {1})?", oldCategory.Name,newCategory.Name);
                caption = "Подтверждение редактирования";
            }

            if (!CheckAddCategoryFields())
            {
                return;
            }


            DialogResult dialog = MessageBox.Show(
                           expression,
                           caption,
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
            if (dialog == DialogResult.No) return;

            string result;

            if (action == addActions)
            {
                result = dataBase.AddCategory(newCategory);

                if (result == "OK")
                {
                    MessageBox.Show(
                           string.Format("Категория успешно добавлен!"),
                           "Результат добавления категории",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
                    this.adminForm.categoryTable = dataBase.LoadCategories();
                    adminForm.setCategoriesInCB(adminForm.categoryTable);
                    adminForm.Categories_comboBox.SelectedIndex = adminForm.Service_comboBox.SelectedIndex = -1;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                           string.Format("В результате добваления категории произошла ошибка!\n", result),
                           "Результат добавления категории",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error,
                           MessageBoxDefaultButton.Button1);
                }
                comboBox1.SelectedIndex = comboBox2.SelectedIndex = -1;
                maskedTextBox1.Text = string.Empty;
            }
            else
            {
                result = dataBase.UpdateCategory(oldCategory,newCategory);
                if (result == "OK")
                {
                    MessageBox.Show(
                           string.Format("Данные о категории успешно отредактировы!"),
                           "Результат редактирования данных о категории",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
                    this.adminForm.categoryTable = dataBase.LoadCategories();
                    adminForm.setCategoriesInCB(adminForm.categoryTable);
                    adminForm.Categories_comboBox.SelectedIndex = adminForm.Service_comboBox.SelectedIndex= -1;
                    this.Close();


                }
                else
                {
                    MessageBox.Show(
                           string.Format("В результате редактирвоания данных о категории произошла ошибка!\n", result),
                           "Результат редактирования данных о категории",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error,
                           MessageBoxDefaultButton.Button1);

                }
                comboBox1.SelectedIndex = comboBox2.SelectedIndex = -1;
            }
        }

        private bool CheckAddCategoryFields()
        {
            bool res = true;
            if (comboBox1.Text == string.Empty )
            {
                MessageBox.Show(
                          string.Format("Не все поля были заполнены! Проверьте поле \"Категория\""),
                          "Неверные данные",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information,
                          MessageBoxDefaultButton.Button1);
                res = false;
            }
            return res;

        }
        #endregion

        #region Add Update Service
        private void AddUpdateService()
        {
            Service oldService = new Service()
            {
                Name = updateDataList == null || updateDataList.Count==1 ? "-1" : updateDataList[0],
                Price = updateDataList == null || updateDataList.Count == 1 ? "-1" : updateDataList[1],
                IdCategory = updateDataList == null || updateDataList.Count == 1 ? "-1" : updateDataList[2],

            };
            Service newService = new Service()
            {
                Name = comboBox1.Text,
                Price = int.Parse(comboBox2.Text).ToString(),
                IdCategory = updateDataList[0]

            };

         



            string expression;
            string caption;
            if (action == addActions)
            {
                expression = string.Format("Добавить новую услугу ( {0} {1}р)?", newService.Name,newService.Price);
                caption = "Подтверждение добавления";
            }
            else
            {

                expression = string.Format("Изменить данные об услуге ({0} {1}р -> {2} {3}р)?", oldService.Name,oldService.Price,newService.Name,newService.Price);
                caption = "Подтверждение редактирования";
            }

            if (!CheckAddServiceFields())
            {
                return;
            }


            DialogResult dialog = MessageBox.Show(
                           expression,
                           caption,
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
            if (dialog == DialogResult.No) return;

            string result;

            if (action == addActions)
            {
                result = dataBase.AddService(newService);

                if (result == "OK")
                {
                    MessageBox.Show(
                           string.Format("Услуга успешно добавлен!"),
                           "Результат добавления услуги",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
                    this.adminForm.serviceTable = dataBase.LoadServices();
                    adminForm.Service_comboBox.SelectedIndex = adminForm.Categories_comboBox.SelectedIndex= -1;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                           string.Format("В результате добваления услуги произошла ошибка!\n", result),
                           "Результат добавления услуги",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error,
                           MessageBoxDefaultButton.Button1);
                }
                comboBox1.SelectedIndex = comboBox2.SelectedIndex = -1;
                maskedTextBox1.Text = string.Empty;
            }
            else
            {
                result = dataBase.UpdateService(oldService,newService);
                if (result == "OK")
                {
                    MessageBox.Show(
                           string.Format("Данные об услуге успешно отредактировы!"),
                           "Результат редактирования данных об услуге",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
                    this.adminForm.serviceTable = dataBase.LoadServices();
                    adminForm.Service_comboBox.SelectedIndex = adminForm.Categories_comboBox.SelectedIndex = -1;
                    this.Close();


                }
                else
                {
                    MessageBox.Show(
                           string.Format("В результате редактирвоания данных об услуге произошла ошибка!\n", result),
                           "Результат редактирования данных об услуге",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error,
                           MessageBoxDefaultButton.Button1);

                }
                comboBox1.SelectedIndex = comboBox2.SelectedIndex = -1;
            }
        }

        private bool CheckAddServiceFields()
        {
            bool res = true;
            if (comboBox1.Text == string.Empty || comboBox2.Text==string.Empty)
            {
                MessageBox.Show(
                          string.Format("Не все поля были заполнены! Проверьте поле \"Услуги\" и \"Цена\""),
                          "Неверные данные",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information,
                          MessageBoxDefaultButton.Button1);
                res = false;
            }
            return res;

        }
        #endregion 

        #region Add Update Position
        private void AddUpdatePositions()
        {
            Position oldPosition = new Position()
            {
                Name = updateDataList == null ? "-1" : updateDataList[0]
            };
            Position newPosition = new Position()
            {
                Name = comboBox1.Text
            };

           
           


            string expression;
            string caption;
            if (action == addActions)
            {
                expression = string.Format("Добавить новую должность ( {0} )?", newPosition.Name);
                caption = "Подтверждение добавления";
            }
            else
            {

                expression = string.Format("Изменить данные об существующей должности ({0} -> {1})?", oldPosition.Name, newPosition.Name);
                caption = "Подтверждение редактирования";
            }

            if (!CheckAddPositionFields())
            {
                return;
            }


            DialogResult dialog = MessageBox.Show(
                           expression,
                           caption,
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
            if (dialog == DialogResult.No) return;

            string result;

            if (action == addActions)
            {
                result = dataBase.AddPosition(newPosition);

                if (result == "OK")
                {
                    MessageBox.Show(
                           string.Format("Категория успешно добавлен!"),
                           "Результат добавления категории",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
                    this.adminForm.positionsTable = dataBase.LoadPositions();
                    adminForm.setPositionsInCB(adminForm.positionsTable);
                    adminForm.Positions_comboBox.SelectedIndex = adminForm.Employees_comboBox.SelectedIndex = -1;
                    adminForm.Employees_comboBox.DataSource = null;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                           string.Format("В результате добваления категории произошла ошибка!\n", result),
                           "Результат добавления категории",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error,
                           MessageBoxDefaultButton.Button1);
                }
                comboBox1.SelectedIndex = -1;
                
            }
            else
            {
                result = dataBase.UpdatePosition(oldPosition, newPosition);
                if (result == "OK")
                {
                    MessageBox.Show(
                           string.Format("Данные о должности успешно отредактировы!"),
                           "Результат редактирования данных о должности",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
                    this.adminForm.positionsTable = dataBase.LoadPositions();
                    adminForm.setPositionsInCB(adminForm.positionsTable);
                    adminForm.Positions_comboBox.SelectedIndex = adminForm.Employees_comboBox.SelectedIndex = -1;
                    adminForm.Employees_comboBox.DataSource = null;
                    this.Close();

                }
                else
                {
                    MessageBox.Show(
                           string.Format("В результате редактирвоания данных о должности произошла ошибка!\n", result),
                           "Результат редактирования данных о должности",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error,
                           MessageBoxDefaultButton.Button1);

                }
                comboBox1.SelectedIndex=-1;
            }
        }

        private bool CheckAddPositionFields()
        {
            bool res = true;
            if (comboBox1.Text == string.Empty)
            {
                MessageBox.Show(
                          string.Format("Не все поля были заполнены! Проверьте поле \"Название должности\""),
                          "Неверные данные",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information,
                          MessageBoxDefaultButton.Button1);
                res = false;
            }
            return res;

        }
        #endregion

        #region Add Update Employee
        private void AddUpdateEmployee()
        {
            Employee oldEmployee= new Employee()
            {
                FName = updateDataList == null ? "-1" : updateDataList[0],
                MName = updateDataList == null ? "-1" : updateDataList[1],
                LName = updateDataList == null ? "-1" : updateDataList[2],
                Phone = updateDataList == null ? "-1" : updateDataList[3],
                PositionId = updateDataList == null ? "-1" : updateDataList[4],
                Password = updateDataList == null ? "-1" : updateDataList[5],

            };
            Employee newEmployee = new Employee()
            {
                FName = dataGridView1.Rows[0].Cells[0].Value.ToString(),
                MName = dataGridView1.Rows[0].Cells[1].Value.ToString(),
                LName = dataGridView1.Rows[0].Cells[2].Value.ToString(),
                Phone = dataGridView1.Rows[0].Cells[3].Value.ToString(),
                PositionId = dataGridView1.Rows[0].Cells[4].Value.ToString(),
                Password = dataGridView1.Rows[0].Cells[5].Value.ToString()
            };

            string expression;
            string caption;
            if (action == addActions)
            {
                expression = string.Format("Добавить нового сотрудника ( {0} {1} {2} )?", newEmployee.FName, newEmployee.MName, newEmployee.LName);
                caption = "Подтверждение добавления";
            }
            else
            {

                expression = string.Format("Изменить данные о сотруднике ({0} {1} {2} -> {3} {4} {5})?", oldEmployee.FName, oldEmployee.MName, oldEmployee.Phone, newEmployee.FName, newEmployee.MName, newEmployee.Phone);
                caption = "Подтверждение редактирования";
            }

            if (!CheckAddEmployeeFields())
            {
                return;
            }


            DialogResult dialog = MessageBox.Show(
                           expression,
                           caption,
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
            if (dialog == DialogResult.No) return;

            string result;

            if (action == addActions)
            {
                result = dataBase.AddEmployee(newEmployee);
                if (result == "OK")
                {
                    MessageBox.Show(
                           string.Format("Сотрудник успешно добавлен!"),
                           "Результат добавления сотрудника",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
                    this.adminForm.employeesTable = dataBase.LoadEmployees();
                    adminForm.Positions_comboBox.SelectedIndex=adminForm.Employees_comboBox.SelectedIndex = -1;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                           string.Format("В результате добваления сотрудника произошла ошибка!\n", result),
                           "Результат добавления сотрудника",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error,
                           MessageBoxDefaultButton.Button1);
                }
                DataTable dt = dataGridView1.DataSource as DataTable;
                dt.Rows.Clear();
            }
            else
            {
                result = dataBase.UpdateEmployee(oldEmployee,newEmployee);
                if (result == "OK")
                {
                    MessageBox.Show(
                           string.Format("Данные о сотруднике успешно отредактировы!"),
                           "Результат редактирования данных о клиенте",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
                    this.adminForm.employeesTable = dataBase.LoadEmployees();
                    adminForm.Positions_comboBox.SelectedIndex = adminForm.Employees_comboBox.SelectedIndex = -1;
                    this.Close();


                }
                else
                {
                    MessageBox.Show(
                           string.Format("В результате редактирвоания данных о сотруднике произошла ошибка!\n", result),
                           "Результат редактирования данных о сотруднике",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error,
                           MessageBoxDefaultButton.Button1);

                }

                DataTable dt = dataGridView1.DataSource as DataTable;
                dt.Rows.Clear();
            }
        }

        private bool CheckAddEmployeeFields()
        {
            bool res = true;
            if (dataGridView1.Rows.Count==0 || dataGridView1.Rows[0].Cells[0].Value == null || dataGridView1.Rows[0].Cells[1].Value == null || dataGridView1.Rows[0].Cells[2].Value == null || dataGridView1.Rows[0].Cells[3].Value == null || dataGridView1.Rows[0].Cells[4].Value == null || dataGridView1.Rows[0].Cells[5].Value == null)
            {
                MessageBox.Show(
                          string.Format("Не все поля были заполнены! Проверьте поле \"Фамилия\" \"Имя\" \"Отчество\" \"Номер телефона\" , \"Должность\""),
                          "Неверные данные",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information,
                          MessageBoxDefaultButton.Button1);
                res = false;
            }
            return res;

        }

        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            AddUpdateEmployee();

        }
    }
}
