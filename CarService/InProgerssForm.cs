using CarService.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace CarService
{
    public partial class InProgerssForm : Form
    {
        //Данные о сотруднике, который открыл форму
        Employee Employee;
        //Id сотрудник
        public int EmployeeId;
        //Id должности сотрудника
        public int PositionId;

        DB dataBase;

        DataSet allAboutOrders;
        //DataTable 1 from DataSet
        //0-IdOrder,1-Date, 
        //2-IdClient,3-FNameClient,4-MNameClient,5-PhoneClient,
        //6-IdCar,7-Brand,8-Model,9-Year,10-EngineID,11-EngineName,12-Value,13-Mileage,14-VIN, 
        //15-IdEmp,16-FNameEmp,17-MNameEmp,18-LNameEmp,18-PhoneEmp,
        //20-IdMaster,21-FNameMaster,22-MNameMaster,23-LNameMaster,24-PhoneMaster,
        //25-TotalCost,26-Status,27-Comment
        
        //Таблица сотрудников
        DataTable employeesTable;
        //Таблица, в которой описано какие мастера назначена на каждый заказ
        DataTable ordersMaster;
        //Таблица, в которой описаны задачи конкретного мастера (номер заказа, дата, авто, услуги, статус)
        DataTable serviceMasterFull;
        //Таблица с заказами со статусов оформлен и в процессе
        DataTable serviceMasterInProgress;
        //Таблица для выполненных заказов
        DataTable serviceMasterDone;
        
        public InProgerssForm(int employeeId)
        {
            InitializeComponent();
            dataBase = new DB();
            //Получаем Id сотрудника, открывшего форму
            this.EmployeeId = employeeId;
          
        }

        private void InProgerssForm_Load(object sender, EventArgs e)
        {
            allAboutOrders = dataBase.LoadOrders();
            employeesTable = dataBase.LoadEmployees();
            Employee = dataBase.getEmployeeById(this.EmployeeId);
            PositionId = Convert.ToInt32(this.Employee.PositionId);

            if (this.PositionId == 1 || this.PositionId == 2)
            {
                toolStrip1.Visible=false;
                groupBox1.Visible = true;
                groupBox2.Visible = true;
                InitTableOrdersMaster();
                SetValueInOrdersMaster(allAboutOrders.Tables[0]);
                SetDataInDGV(ordersMaster);
                SetMastersInCB(employeesTable);
                SetTextInUpdateStatusButons();

            }
            else
            {
                this.Text = "Мастер " + Employee.FName + ' ' + Employee.MName + ' ' + Employee.LName;
                groupBox1.Visible = false;
                groupBox2.Visible = true;
                button9.Visible = false;
                InitServicesByMaster();
                SetValueInServiceByMaster(allAboutOrders.Tables[0]);
                SetValueInServiceByMasterInProgress(serviceMasterFull);
                SetValueInServiceByMasterDone(serviceMasterFull);
                SetDataInDGV(serviceMasterInProgress);

                SetTextInUpdateStatusButons();
            }
        }

        #region Администратор и приёмщик

        private void InitTableOrdersMaster()
        {
            ordersMaster = new DataTable();
            ordersMaster.Columns.Add("IdOrder",typeof(int));
            ordersMaster.Columns.Add("Date", typeof(string));
            ordersMaster.Columns.Add("Car", typeof(string));
            ordersMaster.Columns.Add("Service", typeof(string));
            ordersMaster.Columns.Add("Master",typeof (string));
            ordersMaster.Columns.Add("Status", typeof(string));

        }

        private void SetValueInOrdersMaster(DataTable fullInfo)
        {
            DataRow row;

            foreach(DataRow r in fullInfo.Rows)
            {
                row = ordersMaster.NewRow();
                row[ordersMaster.Columns[0].ColumnName] = r.ItemArray[0];
                row[ordersMaster.Columns[1].ColumnName] = r.ItemArray[1];
                row[ordersMaster.Columns[2].ColumnName] = Convert.ToString( r.ItemArray[7]+" " + r.ItemArray[8]+" (" + r.ItemArray[9]+")");
                row[ordersMaster.Columns[3].ColumnName] = dataBase.CreateServicesString(allAboutOrders.Tables[1], Convert.ToInt32(r.ItemArray[0]));
                row[ordersMaster.Columns[4].ColumnName] = Convert.ToString(r.ItemArray[21]+" " + r.ItemArray[22] + " " + r.ItemArray[23]);
                row[ordersMaster.Columns[5].ColumnName] = dataBase.ConvertStatus(Convert.ToInt32(r.ItemArray[26]));
                ordersMaster.Rows.Add(row);
            }
        }

       
        private void SetMastersInCB(DataTable employees)
        {
            DataTable dataTable=CreateMasterTable(employees);
            comboBox1.DataSource = dataTable;
            comboBox1.ValueMember = dataTable.Columns[0].ColumnName;
            comboBox1.DisplayMember = dataTable.Columns[1].ColumnName;
        }

        private DataTable CreateMasterTable(DataTable employees)
        {
            DataTable mastersIDName = new DataTable();
            mastersIDName.Columns.Add("IdMaster", typeof(int));
            mastersIDName.Columns.Add("Name", typeof(string));
            DataRow newRow;
            foreach (DataRow row in employees.Select(string.Format("{0}={1}", employees.Columns[5].ColumnName,3)))
            {
                newRow=mastersIDName.NewRow();
                newRow[0] = row.ItemArray[0];
                newRow[1]= row.ItemArray[1] +" "+ row.ItemArray[2] + " " + row.ItemArray[3];
                mastersIDName.Rows.Add(newRow);
            }
            return mastersIDName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Visible = comboBox1.Visible = textBox1.Visible = button2.Visible = button3.Visible = true;
            button1.Visible = false;
            string idOrder = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            string columnName = allAboutOrders.Tables[0].Columns[0].ColumnName;
            int IdSelectedMaster = Convert.ToInt32(allAboutOrders.Tables[0].Select(string.Format("{0}={1}", columnName, idOrder))[0].ItemArray[20]);
            comboBox1.SelectedValue = IdSelectedMaster;
            textBox1.Text = idOrder;


        }

        private void button3_Click(object sender, EventArgs e)
        {
            label1.Visible = comboBox1.Visible = textBox1.Visible = button2.Visible = button3.Visible = false;
            button1.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UpdateMaster();
        }

        //Смена мастера на выбранном заказе
        private void UpdateMaster()
        {
            string idOrder = textBox1.Text;
            string oldMaster = ordersMaster.Select(string.Format("{0}={1}", ordersMaster.Columns[0].ColumnName, idOrder))[0].ItemArray[4].ToString();
            string idOldMaster = allAboutOrders.Tables[0].Select(string.Format("{0}={1}", allAboutOrders.Tables[0].Columns[0].ColumnName, idOrder))[0].ItemArray[20].ToString();
            string idNewMaster = comboBox1.SelectedValue.ToString();

            string phoneNewMaster = employeesTable.Select(string.Format("{0}={1}", employeesTable.Columns[0], idNewMaster))[0].ItemArray[4].ToString();
            DataRowView newRow = (DataRowView)comboBox1.SelectedItem;
            string newMaster = newRow.Row.ItemArray[1].ToString();
            DialogResult result = MessageBox.Show(
                            string.Format("Номер заказа: {0} \n  сменить мастера\nс: {1}\nна: {2}", idOrder, oldMaster, newMaster),
                            "Подтверждение смены мастера",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                try
                {
                    dataBase.UpdateMasterInOrder(idOldMaster, idNewMaster);
                    MessageBox.Show(
                            string.Format("Мастер успешно изменён", idOrder, oldMaster, newMaster),
                            "Результат смены мастера",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1);

                    DataRow oldRow1 = allAboutOrders.Tables[0].Select(string.Format("{0}={1}", allAboutOrders.Tables[0].Columns[0].ColumnName, idOrder))[0];
                    object[] newItemArray1 = new object[] {
                        oldRow1.ItemArray[0], oldRow1.ItemArray[1], oldRow1.ItemArray[2], oldRow1.ItemArray[3],
                        oldRow1.ItemArray[4], oldRow1.ItemArray[5], oldRow1.ItemArray[6],oldRow1.ItemArray[7],
                        oldRow1.ItemArray[8], oldRow1.ItemArray[9], oldRow1.ItemArray[10],oldRow1.ItemArray[11],oldRow1.ItemArray[12],
                        oldRow1.ItemArray[13], oldRow1.ItemArray[14], oldRow1.ItemArray[15],oldRow1.ItemArray[16],
                        oldRow1.ItemArray[17], oldRow1.ItemArray[18], oldRow1.ItemArray[19],idNewMaster,
                        newMaster.Split(' ')[0], newMaster.Split(' ')[1], newMaster.Split(' ')[2],phoneNewMaster,
                        oldRow1.ItemArray[25], oldRow1.ItemArray[26], oldRow1.ItemArray[27]
                    };

                    DataRow oldRow2 = ordersMaster.Select(string.Format("{0}={1}", ordersMaster.Columns[0].ColumnName, idOrder))[0];
                    object[] newItemArray2 = new object[]
                    {
                         oldRow2.ItemArray[0], oldRow2.ItemArray[1], oldRow2.ItemArray[2], oldRow2.ItemArray[3],newMaster
                    };

                    allAboutOrders.Tables[0].Select(string.Format("{0}={1}", allAboutOrders.Tables[0].Columns[0].ColumnName, idOrder))[0].ItemArray = newItemArray1;
                    ordersMaster.Select(string.Format("{0}={1}", ordersMaster.Columns[0].ColumnName, idOrder))[0].ItemArray = newItemArray2;
                    //dataGridView1.Refresh();
                    label1.Visible = comboBox1.Visible = textBox1.Visible = button2.Visible = button3.Visible = false;
                    button1.Visible = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                            "При смене мастера произошла ошибка!\n" + ex.Message,
                            "Результат смены мастера",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
                }

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenDetailsForm();
        }

        //Открывает форму с подробностями заказа
        private void OpenDetailsForm()
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
            for (int i = 1; i < allAboutOrders.Tables[0].Columns.Count; i++)
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
            return allAboutOrders.Tables[0].Select(string.Format("{0}={1}", allAboutOrders.Tables[0].Columns[0].ColumnName, idOrder))[0].ItemArray[itemNumber].ToString();
        }
        private string GetValueFromShortOrdersTable(int idOrder, int itemNumber)
        {
            return allAboutOrders.Tables[2].Select(string.Format("{0}={1}", allAboutOrders.Tables[2].Columns[0].ColumnName, idOrder))[0].ItemArray[itemNumber].ToString();
        }
        #endregion

        #region Мастер
        private void InitServicesByMaster()
        {
            serviceMasterFull = new DataTable();
            serviceMasterFull.Columns.Add("IdOrder", typeof(int));
            serviceMasterFull.Columns.Add("Date", typeof(string));
            serviceMasterFull.Columns.Add("Car", typeof(string));
            serviceMasterFull.Columns.Add("Service", typeof(string));
            serviceMasterFull.Columns.Add("Status", typeof(string));

            serviceMasterInProgress = new DataTable();
            serviceMasterInProgress.Columns.Add("IdOrder", typeof(int));
            serviceMasterInProgress.Columns.Add("Date", typeof(string));
            serviceMasterInProgress.Columns.Add("Car", typeof(string));
            serviceMasterInProgress.Columns.Add("Service", typeof(string));
            serviceMasterInProgress.Columns.Add("Status", typeof(string));

            serviceMasterDone = new DataTable();
            serviceMasterDone.Columns.Add("IdOrder", typeof(int));
            serviceMasterDone.Columns.Add("Date", typeof(string));
            serviceMasterDone.Columns.Add("Car", typeof(string));
            serviceMasterDone.Columns.Add("Service", typeof(string));
            serviceMasterDone.Columns.Add("Status", typeof(string));
        }

        //Заполняет общую таблицу с заказами для мастера (оформлены, в процессе, выполнены)
        private void SetValueInServiceByMaster(DataTable fullInfo)
        {
            DataRow row;

            foreach (DataRow r in fullInfo.Select(string.Format("{0}={1}", fullInfo.Columns[20].ColumnName, EmployeeId)))
            {
                row = serviceMasterFull.NewRow();
                row[serviceMasterFull.Columns[0].ColumnName] = r.ItemArray[0];
                row[serviceMasterFull.Columns[1].ColumnName] = r.ItemArray[1];
                row[serviceMasterFull.Columns[2].ColumnName] = Convert.ToString(r.ItemArray[7] + " " + r.ItemArray[8] + " (" + r.ItemArray[9] + ")");
                row[serviceMasterFull.Columns[3].ColumnName] = dataBase.CreateServicesString(allAboutOrders.Tables[1], Convert.ToInt32(r.ItemArray[0]));
                row[serviceMasterFull.Columns[4].ColumnName] = dataBase.ConvertStatus(Convert.ToInt32(r.ItemArray[26]));

                serviceMasterFull.Rows.Add(row);
            }
        }

        //Заполняет таблицу с оформленными заказами и заказами в процесее для мастера
        private void SetValueInServiceByMasterInProgress(DataTable serviceMasterFull)
        {
            //string.Format("{0}={1} AND ({2}={3} OR {4}={5})", fullInfo.Columns[19].ColumnName, EmployeeId, fullInfo.Columns[25].ColumnName, 1, fullInfo.Columns[25].ColumnName, 2)
            DataRow row;

            foreach (DataRow r in serviceMasterFull.Select(string.Format("{0}='{1}' OR {2}='{3}'", serviceMasterFull.Columns[4].ColumnName,dataBase.ConvertStatus(1), serviceMasterFull.Columns[4].ColumnName, dataBase.ConvertStatus(2))))
            {
                row = serviceMasterInProgress.NewRow();
                row.ItemArray = r.ItemArray;
                serviceMasterInProgress.Rows.Add(row);
            }
        }

        //Заполняет таблицу с выполненными заказами для мастера
        private void SetValueInServiceByMasterDone(DataTable serviceMasterFull)
        {
            DataRow row;

            foreach (DataRow r in serviceMasterFull.Select(string.Format("{0}='{1}'", serviceMasterFull.Columns[4].ColumnName, dataBase.ConvertStatus(3))))
            {
                row = serviceMasterDone.NewRow();
                row.ItemArray = r.ItemArray;
                serviceMasterDone.Rows.Add(row);
            }
        }

        //Ставит текст в кнопки смены статуса
        private void SetTextInUpdateStatusButons()
        {
            button6.Text = dataBase.ConvertStatus(1);
            button7.Text = dataBase.ConvertStatus(2);
            button8.Text = dataBase.ConvertStatus(3);
            button6.Visible = button7.Visible = button8.Visible = false;
        }

        

        private void button5_Click(object sender, EventArgs e)
        {
            button7.Visible = button8.Visible = button6.Visible = true;
            button5.Enabled = false;
            string currentStatus = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            SetEnabledFalseButtonForCurrentStatus(currentStatus);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int idOrder = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            string oldStatus = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            int newStatus = 1;

            UpdateStatus(idOrder, newStatus);
            
            if (oldStatus == dataBase.ConvertStatus(3) && EmployeeId==3)
                MoveOrderInOtherTable(idOrder, "Done");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int idOrder = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            string oldStatus = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            int newStatus = 2;

            UpdateStatus(idOrder, newStatus);
            if (oldStatus == dataBase.ConvertStatus(3) && EmployeeId == 3)
                MoveOrderInOtherTable(idOrder,"Done");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            int idOrder = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            int newStatus = 3;

            UpdateStatus(idOrder, newStatus);
            if(EmployeeId == 3)
                MoveOrderInOtherTable(idOrder, "InProgress");
        }

        //Меняет статус заказа
        private void UpdateStatus(int idOrder, int newStatus)
        {
            try
            {
                dataBase.UpdateOrderStatus(idOrder, newStatus);
                DataTable table=dataGridView1.DataSource as DataTable;
                DataRow oldRow = table.Select(string.Format("{0}={1}", table.Columns[0].ColumnName, idOrder))[0];
                if (EmployeeId == 3)
                {
                    object[] newItemArray = new object[]
              {
                         oldRow.ItemArray[0], oldRow.ItemArray[1], oldRow.ItemArray[2], oldRow.ItemArray[3],dataBase.ConvertStatus(newStatus)
              };
                    table.Select(string.Format("{0}={1}", table.Columns[0].ColumnName, idOrder))[0].ItemArray = newItemArray;

                    DataRow row = serviceMasterFull.Select(string.Format("{0}={1}", table.Columns[0].ColumnName, idOrder))[0];
                    row.ItemArray = newItemArray;
                }
                else
                {
                    object[] newItemArray = new object[]
              {
                         oldRow.ItemArray[0], oldRow.ItemArray[1], oldRow.ItemArray[2], oldRow.ItemArray[3],oldRow.ItemArray[4],dataBase.ConvertStatus(newStatus)
              };
                    table.Select(string.Format("{0}={1}", table.Columns[0].ColumnName, idOrder))[0].ItemArray = newItemArray;

                    DataRow row = ordersMaster.Select(string.Format("{0}={1}", table.Columns[0].ColumnName, idOrder))[0];
                    row.ItemArray = newItemArray;
                }
                
              
                //DataRow newRowInServiceMasterFull = serviceMasterFull.NewRow();
                //newRowInServiceMasterFull.ItemArray = newItemArray;

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                           "При смене статуса произошла ошибка!\n" + ex.Message,
                           "Результат смены статуса",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error,
                           MessageBoxDefaultButton.Button1);
            }

            button7.Visible = button8.Visible = button6.Visible = false;
            button5.Enabled = true;
        }

        //Делает активными кнопки смены статуса в зависимости от текущего статуса
        private void SetEnabledFalseButtonForCurrentStatus(string currentStatus)
        {
            if (currentStatus == dataBase.ConvertStatus(1))
            {
                button6.Enabled = false;
                button7.Enabled = true;
                button8.Enabled = true;

            }
            else if (currentStatus == dataBase.ConvertStatus(2))
            {
                button6.Enabled = true;
                button7.Enabled = false;
                button8.Enabled = true;
            }
            else
            {
                button6.Enabled = false;
                button7.Enabled = true;
                button8.Enabled = false;
            }
        }

        //Если статус меняется с оформлен/в процессе на выполнен то заказ переносится в таблицу Done, если наоборот то в таблицу InProgress
        private void MoveOrderInOtherTable(int idOrder, string oldTableName /*inProgress or Done*/)
        {
            string currentStatus = serviceMasterFull.Select(string.Format("{0}={1}", serviceMasterFull.Columns[0].ColumnName, idOrder))[0].ItemArray[4].ToString();
            if (oldTableName == "InProgress")
            {
                DataRow row = serviceMasterInProgress.Select(string.Format("{0}={1}", serviceMasterInProgress.Columns[0].ColumnName, idOrder))[0];
                DataRow newRow = serviceMasterDone.NewRow();
                newRow.ItemArray = row.ItemArray;
                serviceMasterDone.Rows.Add(newRow);
                serviceMasterInProgress.Rows.Remove(row);
            }
            else if (oldTableName == "Done")
            {
                DataRow row = serviceMasterDone.Select(string.Format("{0}={1}", serviceMasterDone.Columns[0].ColumnName, idOrder))[0];
                DataRow newRow = serviceMasterInProgress.NewRow();
                newRow.ItemArray = row.ItemArray;
                serviceMasterInProgress.Rows.Add(newRow);
                serviceMasterDone.Rows.Remove(row);
            }
        }


        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            SetDataInDGV(serviceMasterInProgress);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            SetDataInDGV(serviceMasterDone);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            SetDataInDGV(serviceMasterFull);
        }
        #endregion

        //Заполняет dataGridView 
        private void SetDataInDGV(DataTable dataTable)
        {

            dataGridView1.DataSource = dataTable;

            if (dataGridView1.Columns.Count > dataTable.Columns.Count) return;

            if (PositionId == 1 || PositionId == 2)
            {
                dataGridView1.Columns[0].HeaderText = "Номер заказа";
                dataGridView1.Columns[1].HeaderText = "Дата оформления";
                dataGridView1.Columns[2].HeaderText = "Автомобиль";
                dataGridView1.Columns[3].HeaderText = "Услуги";
                dataGridView1.Columns[4].HeaderText = "Мастер";
                dataGridView1.Columns[5].HeaderText = "Статус";

            }
            else
            {
                dataGridView1.Columns[0].HeaderText = "Номер заказа";
                dataGridView1.Columns[1].HeaderText = "Дата оформления";
                dataGridView1.Columns[2].HeaderText = "Автомобиль";
                dataGridView1.Columns[3].HeaderText = "Услуги";
                dataGridView1.Columns[4].HeaderText = "Статус";
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            DeleteOrder();
        }

        private bool CheckOrderSelected()
        {
            bool res = true;
            if (dataGridView1.SelectedRows.Count==0)
                res = false;
            return res;
        }
        private void DeleteOrder()
        {
            if (!CheckOrderSelected())
            {
                MessageBox.Show(
                         string.Format("Для удаления заказа его необходимо выбрать"),
                         "Неверные данные",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Information,
                         MessageBoxDefaultButton.Button1);
                return;
            }

            DialogResult dialog = MessageBox.Show(
                          $"Вы действительно хотите удалить заказ {dataGridView1.SelectedRows[0].Cells[0].Value.ToString()}?",
                          "Подтверждение удаления",
                          MessageBoxButtons.YesNo,
                          MessageBoxIcon.Information,
                          MessageBoxDefaultButton.Button1);
            if (dialog == DialogResult.No) return;

            string result;
            int idOrder = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

            result = dataBase.DeleteOrder(idOrder);

            if (result == "OK")
            {
                MessageBox.Show(
                       string.Format("Заказ успешно удален!"),
                       "Результат удаления",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Information,
                       MessageBoxDefaultButton.Button1);
                DataRow deletedRow = ordersMaster.Select(string.Format("{0}={1}", ordersMaster.Columns[0].ColumnName, idOrder))[0];
                ordersMaster.Rows.Remove(deletedRow);

            }
            else
            {
                MessageBox.Show(
                       string.Format("В результате удаления заказа произошла ошибка!\n", result),
                       "Результат удаления",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Error,
                       MessageBoxDefaultButton.Button1);
            }
            
        }
    }
}
