using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using CarService.Objects;
using System.Configuration;

namespace CarService
{
    internal class DB
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["CarService2"].ConnectionString;
        private SqlConnection sqlConnection;

        public DB()
        {
        }

        public DataTable getPasswordsEmployee()
        {
            DataSet dataSet=new DataSet();
            using (sqlConnection = new SqlConnection(connectionString))
            {

                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandText = "SELECT * FROM getPasswordEmployee";

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataSet);

                DataTable table = dataSet.Tables[0];


                return table;
            }

        }

        public int getIdEmployee(string password)
        {
            string sqlExpression = "getIdByPassword";
            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, sqlConnection);
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@password",
                    Value = password
                };
                command.Parameters.Add(nameParam);

                // определяем первый выходной параметр
                SqlParameter id = new SqlParameter
                {
                    ParameterName = "@id",
                    SqlDbType = SqlDbType.Int // тип параметра
                };
                // указываем, что параметр будет выходным
                id.Direction = ParameterDirection.Output;
                command.Parameters.Add(id);

                command.ExecuteNonQuery();

                return (int)command.Parameters["@id"].Value;

            }

        }

        public Employee getEmployeeById(int id)
        {
            Employee employee = new Employee();
            string sqlExpression = "getEmployeeById";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                // указываем, что команда представляет хранимую процедуру
                command.CommandType = System.Data.CommandType.StoredProcedure;
                // параметр для ввода имени
                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@id",
                    Value = id
                };
                // добавляем параметр
                command.Parameters.Add(nameParam);
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        employee.Id = reader.GetInt32(0);
                        employee.FName = reader.GetString(1);
                        employee.MName = reader.GetString(2);
                        employee.LName = reader.GetString(3);
                        employee.Phone = reader.GetString(4);
                        employee.PositionName = reader.GetString(5);
                        employee.PositionId = reader.GetInt32(6).ToString();
                        employee.Password = reader.GetString(7);

                    }
                }
                reader.Close();
                return employee;
            }
        }

        public DataTable LoadPositions()
        {

            DataSet dataSet = new DataSet();

            using (sqlConnection = new SqlConnection(connectionString))
            {

                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandText = "SELECT * FROM Positions ORDER BY Name;";

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataSet);

                DataTable table = dataSet.Tables[0];
                table.TableName = "Positions";


                return table;
            }



        }

        public DataTable LoadEngine()
        {
            DataSet dataSet = new DataSet();

            using (sqlConnection = new SqlConnection(connectionString))
            {

                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandText = "SELECT * FROM Engine";

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataSet);

                DataTable table = dataSet.Tables[0];
                table.TableName = "Engine";


                return table;
            }
        }

        public DataTable LoadCategories()
        {
            DataSet dataSet = new DataSet();

            using (sqlConnection = new SqlConnection(connectionString))
            {

                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandText = "SELECT * FROM getServiceCategories";

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataSet);

                DataTable table = dataSet.Tables[0];
                table.TableName = "Categories";


                return table;
            }
        }

        public DataTable LoadCars()
        {
            
            DataSet dataSet = new DataSet();

            using (sqlConnection = new SqlConnection(connectionString))
            {

                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandText = "SELECT * FROM Cars ORDER BY Brand;";

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataSet);

                DataTable table = dataSet.Tables[0];

                table.TableName = "Cars";

               
                return table;
            }



        }

        public DataTable LoadServices()
        {

            DataSet dataSet = new DataSet();

            using (sqlConnection = new SqlConnection(connectionString))
            {

                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandText = "SELECT * FROM Services ORDER BY Name;";

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataSet);

                DataTable table = dataSet.Tables[0];
                table.TableName = "Services";


                return table;
            }



        }

        public DataTable LoadClients()
        {

            DataSet dataSet = new DataSet();

            using (sqlConnection = new SqlConnection(connectionString))
            {

                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandText = "SELECT * FROM Clients ORDER BY FName;";

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataSet);

                DataTable table = dataSet.Tables[0];
                table.TableName = "Clients";


                return table;
            }



        }

        public DataTable LoadEmployees()
        {

            DataSet dataSet = new DataSet();

            using (sqlConnection = new SqlConnection(connectionString))
            {

                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandText = "SELECT * FROM Employees ORDER BY FName;";

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataSet);

                DataTable table = dataSet.Tables[0];
                table.TableName = "Employees";


                return table;
            }



        }

        public int AddNewClient(Client client, SqlCommand command)
        {

            if (command.Parameters.Count != 0) command.Parameters.Clear();

            command.CommandText = "addClient";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@FName", client.FName);
            command.Parameters.AddWithValue("@MName", client.MName);
            command.Parameters.AddWithValue("@Phone", client.Phone);


            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 4));
            command.Parameters["@id"].Direction = ParameterDirection.Output;

            command.ExecuteNonQuery();

            int id = (int)command.Parameters["@id"].Value;

            return id;
        }

        public int AddCarDetails(CarDetails carDetails, SqlCommand command)
        {

            if (command.Parameters.Count != 0) command.Parameters.Clear();
            command.CommandText = "addCarDetails";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Car", carDetails.CarId);
            command.Parameters.AddWithValue("@Year", carDetails.Year);
            command.Parameters.AddWithValue("@Engine", carDetails.Engine);
            command.Parameters.AddWithValue("@Value", carDetails.Value);
            command.Parameters.AddWithValue("@VIN", carDetails.VIN);
            command.Parameters.AddWithValue("@Mileage", carDetails.Mileage);


            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 4));
            command.Parameters["@id"].Direction = ParameterDirection.Output;

            command.ExecuteNonQuery();

            int id = (int)command.Parameters["@id"].Value;

            return id;


        }

        public int AddOrder(Order order, DataTable service,SqlCommand command)
        {
            if (command.Parameters.Count != 0) command.Parameters.Clear();
            command.CommandText = "addOrder";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Date", order.DateTime);
            command.Parameters.AddWithValue("@Client", order.Client);
            command.Parameters.AddWithValue("@Car", order.Car);
            command.Parameters.AddWithValue("@Employee", order.Employee);
            command.Parameters.AddWithValue("@Master", order.Master);
            command.Parameters.AddWithValue("@TotalCost", order.TotalCost);
            command.Parameters.AddWithValue("@Status", order.Status);
            command.Parameters.AddWithValue("@Comment", order.Comment);

            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 4));
            command.Parameters["@id"].Direction = ParameterDirection.Output;

            command.ExecuteNonQuery();

            int id = (int)command.Parameters["@id"].Value;

            AddServiceDetails(id, service, command);
            return id;
        }

        public void AddServiceDetails(int id, DataTable serviceDetails, SqlCommand command)
        {

            string comandText = CreateComandToAddServiceDetails(id, serviceDetails);
            //SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = comandText;
            command.ExecuteNonQuery();

        }

        public string CreateComandToAddServiceDetails(int idOrder,DataTable service) 
        {
            string result = "INSERT INTO ServiceDetails VALUES ";
            int ind = 0;
            foreach(DataRow row in service.Rows)
            {
                result += string.Format("({0},{1})", idOrder, row.ItemArray[0]);
                ind++;
                if(ind!=service.Rows.Count)
                    result+= ",";
            }
            result += ";";
            return result;
        }

        public DataSet LoadOrders()
        {
            //DataTable allInfoAboutOrders
            //IdOrder, Date, 
            //IdClient, FNameClient, MNameClient, PhoneClient,
            //IdCar, Brand, Model, Year, Engine, Value, Mileage, VIN, 
            //IdEmp, FNameEmp,MNameEmp, LNameEmp, PhoneEmp,
            //IdMaster, FNameMaster, MNameMaster, LNameMaster, PhoneMaster,
            //TotalCost, Status, Comment
            
            DataSet dataSet = new DataSet();
            DataTable dataTableOrders = new DataTable();
            DataTable dataTableServiceDetails = new DataTable();
            DataTable shortOrdersTable = new DataTable();

            using (sqlConnection = new SqlConnection(connectionString))
            {
                
                string sqlQuery1 = "SELECT * FROM allInfoAboutOrders", sqlQuery2 = "SELECT * FROM allInfoAboutServicesInOrders";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlQuery1,sqlConnection);
                dataAdapter.Fill(dataTableOrders);
                
                dataSet.Tables.Add(dataTableOrders);
                dataAdapter.SelectCommand.CommandText = sqlQuery2;
                dataAdapter.Fill(dataTableServiceDetails);
                dataSet.Tables.Add(dataTableServiceDetails);

                dataSet.Tables[0].TableName = "OrdersFull";
                dataSet.Tables[1].TableName = "Services";
            }

            shortOrdersTable = CreateShortOrdersTable(dataSet);
            dataSet.Tables.Add(shortOrdersTable);
            dataSet.Tables[2].TableName = "OrdersShort";

            

            return dataSet;
        }

        private DataTable CreateShortOrdersTable(DataSet dataSet)
        {
            //DataTable allInfoAboutOrders
            //0-IdOrder,1-Date, 
            //2-IdClient,3-FNameClient,4-MNameClient,5-PhoneClient,
            //6-IdCar,7-Brand,8-Model,9-Year,10-EngineID,11-EngineName,12-Value,13-Mileage,14-VIN, 
            //15-IdEmp,16-FNameEmp,17-MNameEmp,18-LNameEmp,18-PhoneEmp,
            //20-IdMaster,21-FNameMaster,22-MNameMaster,23-LNameMaster,24-PhoneMaster,
            //25-TotalCost,26-Status,27-Comment
            DataTable orders = new DataTable();
            orders.Columns.Add("Id", typeof(int));
            orders.Columns.Add("Date", typeof(DateTime));
            orders.Columns.Add("Brand", typeof(string));
            orders.Columns.Add("Model", typeof(string));
            orders.Columns.Add("Year", typeof(int));
            orders.Columns.Add("Service", typeof(string));
            orders.Columns.Add("Price", typeof(string));
            orders.Columns.Add("TotalPrice", typeof(int));
            orders.Columns.Add("Comment", typeof(string));
            orders.Columns.Add("Status", typeof(string));

            DataRow newRow;

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                newRow = orders.NewRow();
                newRow[orders.Columns[0].ColumnName] = Convert.ToInt32(row.ItemArray[0]);
                newRow[orders.Columns[1].ColumnName] = Convert.ToDateTime(row.ItemArray[1]);
                newRow[orders.Columns[2].ColumnName] = Convert.ToString(row.ItemArray[7]);
                newRow[orders.Columns[3].ColumnName] = Convert.ToString(row.ItemArray[8]);
                newRow[orders.Columns[4].ColumnName] = Convert.ToInt32(row.ItemArray[9]);

                newRow[orders.Columns[5].ColumnName] = CreateServicesString(dataSet.Tables[1], Convert.ToInt32(row.ItemArray[0]));
                newRow[orders.Columns[6].ColumnName] = CreatePricesString(dataSet.Tables[1], Convert.ToInt32(row.ItemArray[0]));
                newRow[orders.Columns[7].ColumnName] = Convert.ToInt32(row.ItemArray[25]);
                newRow[orders.Columns[8].ColumnName] = Convert.ToString(row.ItemArray[27]);
                newRow[orders.Columns[9].ColumnName] = Convert.ToString(ConvertStatus(Convert.ToInt32( row.ItemArray[26])));

                orders.Rows.Add(newRow);
            }

            return orders;
        }

        public string CreateServicesString(DataTable table,int id)
        {
            string result = "";
            DataRow[] serviceRows = table.Select(string.Format("{0}={1}", table.Columns[0].ColumnName, id));
            foreach(DataRow row in serviceRows)
            {
                result += row.ItemArray[1].ToString() + Environment.NewLine;
            }
            return result;
        
        }

        public string CreatePricesString(DataTable table, int id)
        {
            string result = "";
            DataRow[] serviceRows = table.Select(string.Format("{0}={1}", table.Columns[0].ColumnName, id));
            foreach (DataRow row in serviceRows)
            {
                result += row.ItemArray[2].ToString() + Environment.NewLine;
            }
            return result;

        }

        public string ConvertStatus(int status)
        {
            string result = "";
            switch (status)
            {
                case 1:

                    result = "Оформлен";
                    break;
                case 2:
                    result = "В процессе";
                    break;
                case 3:
                    result = "Выполнен";
                    break;
            }
            return result;
        }

        public string getEngineNameByIdEngine(int idEngine)
        {
            string nameEngine= string.Empty;

            string sqlExpression = "SELECT Name FROM Engine WHERE IdEngine="+idEngine.ToString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                // указываем, что команда представляет хранимую процедуру
                command.CommandType = System.Data.CommandType.Text;
               
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nameEngine = reader.GetString(0);
                    }
                }
                reader.Close();
                return nameEngine;
            }
        }

        public void UpdateMasterInOrder(string oldId, string newId)
        {
            string sqlExpression = "updateMasterInOrder";
            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, sqlConnection);
                command.CommandType = CommandType.StoredProcedure;

             
                command.Parameters.AddWithValue("@oldId",oldId);
                command.Parameters.AddWithValue("@newId", newId);

                command.ExecuteNonQuery();


            }
        }

        public void UpdateOrderStatus(int idOrder, int newStatus)
        {
            string sqlExpression = "updateOrderStatus";
            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, sqlConnection);
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.AddWithValue("@IdOrder", idOrder);
                command.Parameters.AddWithValue("@newStatus", newStatus);

                command.ExecuteNonQuery();


            }
        }

        public string AddCar(Car car)
        {
            //OK, ex.Message - Error 
            string result;
            try
            {
                string sqlExpression = string.Format("INSERT INTO Cars VALUES ('{0}','{1}')", car.Brand, car.Model);
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, sqlConnection);
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();

                }
                result = "OK";
            }

            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
            
        }

        public string UpdateCar(Car oldCar, Car newCar)
        {
            //OK, ex.Message - Error 
            string result;
            try
            {
                string sqlExpression;
                if (oldCar.Model == newCar.Model || newCar.Model==string.Empty)
                {
                    sqlExpression = string.Format("update Cars set Brand='{0}' where Brand='{1}'", newCar.Brand, oldCar.Brand);
                }
                else
                {
                    sqlExpression= string.Format("update Cars set Brand='{0}', Model='{1}' where Brand='{2}' AND Model='{3}'", newCar.Brand, newCar.Model,oldCar.Brand,oldCar.Model);
                }
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, sqlConnection);
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();

                }
                result = "OK";
            }

            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;

        }

        public string DeleteCar(int idCar)
        {
            //OK, ex.Message - Error 
            string result;
            try
            {
                string sqlExpression = string.Format("DELETE FROM Cars WHERE IdCar={0}", idCar);
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, sqlConnection);
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();

                }
                result = "OK";
            }

            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        public string AddEngine(Engine newEngine)
        {
            //OK, ex.Message - Error 
            string result;
            try
            {
                string sqlExpression = string.Format("INSERT INTO Engine VALUES ('{0}')", newEngine.Name);
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, sqlConnection);
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();

                }
                result = "OK";
            }

            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;

        }
        public string UpdateEngine(Engine oldEngine, Engine newEngine)
        {
            //OK, ex.Message - Error 
            string result;
            try
            {
                string sqlExpression=string.Format("update Engine set Name='{0}' where Name='{1}'", newEngine.Name, oldEngine.Name);
             
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, sqlConnection);
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();

                }
                result = "OK";
            }

            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;

        }

        public string DeleteEngine(int idEngine)
        {
            //OK, ex.Message - Error 
            string result;
            try
            {
                string sqlExpression = string.Format("DELETE FROM Engine WHERE IdEngine={0}", idEngine);
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, sqlConnection);
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();

                }
                result = "OK";
            }

            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        public string AddClient (Client client)
        {
            //OK, ex.Message - Error 
            string result;
            try
            {
                string sqlExpression = string.Format("INSERT INTO Clients VALUES ('{0}','{1}','{2}')", client.FName,client.MName,client.Phone);
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, sqlConnection);
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();

                }
                result = "OK";
            }

            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;

        }

        public string UpdateClient(Client oldClient, Client newClient)
        {
            //OK, ex.Message - Error 
            string result;
            try
            {
                string sqlExpression = string.Format("update Clients set FName='{0}', MName='{1}', Phone='{2}' where Phone='{3}'", newClient.FName,newClient.MName,newClient.Phone,oldClient.Phone);

                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, sqlConnection);
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();

                }
                result = "OK";
            }

            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;

        }

        public string DeleteClient(int idClient)
        {
            //OK, ex.Message - Error 
            string result;
            try
            {
                string sqlExpression = string.Format("DELETE FROM Clients WHERE IdClient={0}", idClient);
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, sqlConnection);
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();

                }
                result = "OK";
            }

            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        public string AddCategory(Category category)
        {
            //OK, ex.Message - Error 
            string result;
            try
            {
                string sqlExpression = string.Format("INSERT INTO ServiceCategory VALUES ('{0}')", category.Name);
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, sqlConnection);
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();

                }
                result = "OK";
            }

            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;

        }

        public string UpdateCategory(Category oldCategory, Category newCategory)
        {
            //OK, ex.Message - Error 
            string result;
            try
            {
                string sqlExpression = string.Format("update ServiceCategory set Name='{0}' where Name='{1}'", newCategory.Name,oldCategory.Name);

                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, sqlConnection);
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();

                }
                result = "OK";
            }

            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;

        }

        public string DeleteService(int idService)
        {
            //OK, ex.Message - Error 
            string result;
            try
            {
                string sqlExpression = string.Format("DELETE FROM Services WHERE IdService={0}", idService);
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, sqlConnection);
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();

                }
                result = "OK";
            }

            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        public string AddService(Service service)
        {
            //OK, ex.Message - Error 
            string result;
            try
            {
                string sqlExpression = string.Format("INSERT INTO Services VALUES ('{0}',{1},{2})", service.Name,service.Price,service.IdCategory);
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, sqlConnection);
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();

                }
                result = "OK";
            }

            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;

        }

        public string UpdateService(Service oldService, Service newService)
        {
            //OK, ex.Message - Error 
            string result;
            try
            {
                string sqlExpression = string.Format("update Services set Name='{0}',Price={1} where Name='{2}'",newService.Name,newService.Price,oldService.Name);

                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, sqlConnection);
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();

                }
                result = "OK";
            }

            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;

        }

        public string AddPosition(Position newPosition)
        {
            //OK, ex.Message - Error 
            string result;
            try
            {
                string sqlExpression = string.Format("INSERT INTO Positions VALUES ('{0}')", newPosition.Name);
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, sqlConnection);
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();

                }
                result = "OK";
            }

            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;

        }

        public string UpdatePosition(Position oldPosition, Position newPosition)
        {
            //OK, ex.Message - Error 
            string result;
            try
            {
                string sqlExpression = string.Format("update Positions set Name='{0}' where Name='{1}'", newPosition.Name, oldPosition.Name);

                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, sqlConnection);
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();

                }
                result = "OK";
            }

            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;

        }

        public string DeletePosition(int idEmployee)
        {
            //OK, ex.Message - Error 
            string result;
            try
            {
                string sqlExpression = string.Format("DELETE FROM Position WHERE IdPosition={0}", idEmployee);
                string sqlExpression2 = string.Format("DELETE FROM Employees WHERE Position={0}", idEmployee);
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, sqlConnection);
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                    command.CommandText = sqlExpression2;
                    command.ExecuteNonQuery();

                }
                result = "OK";
            }

            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        public string AddEmployee(Employee employee)
        {
            //OK, ex.Message - Error 
            string result;
            try
            {
                string sqlExpression = string.Format("INSERT INTO Employees VALUES ('{0}','{1}','{2}','{3}', {4}, '{5}')", employee.FName, employee.MName, employee.LName, employee.Phone,employee.PositionId,employee.Password);
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, sqlConnection);
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();

                }
                result = "OK";
            }

            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;

        }

        public string UpdateEmployee(Employee oldEmployee, Employee newEmployee)
        {
            //OK, ex.Message - Error 
            string result;
            try
            {
                string sqlExpression = string.Format("update Employees set FName='{0}', MName='{1}',LName='{2}', Phone='{3}', Position={4}, Password='{5}' where Phone='{6}'", newEmployee.FName, newEmployee.MName, newEmployee.LName, newEmployee.Phone, newEmployee.PositionId, newEmployee.Password, oldEmployee.Phone);

                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, sqlConnection);
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();

                }
                result = "OK";
            }

            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;

        }

        public string DeleteEmployee(int idEmployee)
        {
            //OK, ex.Message - Error 
            string result;
            try
            {
                string sqlExpression = string.Format("DELETE FROM Employees WHERE IdEmployee={0}", idEmployee);
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, sqlConnection);
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();

                }
                result = "OK";
            }

            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        public string DeleteOrder(int idOrder)
        {
            //OK, ex.Message - Error 
            string result;
            try
            {
                string sqlExpression = string.Format("DELETE FROM Orders WHERE IdOrder={0}",idOrder);
                string sqlExpression2 = string.Format("DELETE FROM ServiceDetails WHERE IdOrder={0}",idOrder);

                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, sqlConnection);
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                    command.CommandText = sqlExpression2;
                    command.ExecuteNonQuery();
                }
                result = "OK";
            }

            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        #region Statistic
        public int GetMinOrderYear()
        {

            int result = -1;

            SqlCommand sqlCommand = new SqlCommand();

            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                sqlCommand.Connection = sqlConnection;

                sqlCommand.CommandText = "select DATEPART(YYYY,min(Date)) from Orders";
                result = Convert.ToInt32(sqlCommand.ExecuteScalar());

            }

            return result;
        }

        public int GetMaxOrderYear()
        {

            int result = -1;

            SqlCommand sqlCommand = new SqlCommand();

            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                sqlCommand.Connection = sqlConnection;

                sqlCommand.CommandText = "select DATEPART(YYYY,max(Date)) from Orders";
                result = Convert.ToInt32(sqlCommand.ExecuteScalar());

            }

            return result;
        }

        Dictionary<string, int> resultDictionary;
        SqlDataAdapter sqlDataAdapter;
        SqlCommand sqlCommand;
        DataSet dataSet;

        private string getMonthNameByNumber(int number)
        {
            string monthS;
            switch (number)
            {
                case 1:
                    monthS = "Январь";
                    break;
                case 2:
                    monthS = "Февраль";
                    break;
                case 3:
                    monthS = "Март";
                    break;
                case 4:
                    monthS = "Апрель";
                    break;
                case 5:
                    monthS = "Май";
                    break;
                case 6:
                    monthS = "Июнь";
                    break;
                case 7:
                    monthS = "Июль";
                    break;
                case 8:
                    monthS = "Август";
                    break;
                case 9:
                    monthS = "Сентябрь";
                    break;
                case 10:
                    monthS = "Октябрь";
                    break;
                case 11:
                    monthS = "Ноябрь";
                    break;
                case 12:
                    monthS = "Декабрь";
                    break;
                default:
                    monthS = "";
                    break;
            }
            return monthS;
        }

        public Dictionary<string, int> LoadOrdersForMonth(int month, int year)
        {
            resultDictionary = new Dictionary<string, int>();

            sqlCommand = new SqlCommand();
            dataSet = new DataSet();

            string tableName = "OrderForMonth";

            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                sqlCommand.Connection = sqlConnection;

                sqlCommand.CommandText = $"select * from OrderForMonth({month},{year})";

                sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                sqlDataAdapter.Fill(dataSet);

                dataSet.Tables[0].TableName = tableName;
            }

            string date;
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                date = ((DateTime)row.ItemArray[0]).ToString("dd.MM");
                resultDictionary.Add(date, Convert.ToInt32(row.ItemArray[1]));
            }

            return resultDictionary;
        }

        public Dictionary<string, int> LoadOrdersForYear(int year)
        {
            resultDictionary = new Dictionary<string, int>();

            sqlCommand = new SqlCommand();
            dataSet = new DataSet();

            string tableName = "OrderForYear";

            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                sqlCommand.Connection = sqlConnection;

                sqlCommand.CommandText = $"select * from OrderForYear({year})";

                sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                sqlDataAdapter.Fill(dataSet);

                dataSet.Tables[0].TableName = tableName;
            }

            int monthInt;
            string monthString;
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                monthInt = Convert.ToInt32(row.ItemArray[0]);
                monthString=getMonthNameByNumber(monthInt);

                resultDictionary.Add(monthString, Convert.ToInt32(row.ItemArray[1]));
            }

            return resultDictionary;

        }

        public Dictionary<string, int> LoadOrdersSumForMonth(int month, int year)
        {
            resultDictionary = new Dictionary<string, int>();

            sqlCommand = new SqlCommand();
            dataSet = new DataSet();

            string tableName = "OrdersSumForMonth";

            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                sqlCommand.Connection = sqlConnection;

                sqlCommand.CommandText = $"select * from OrdersSumForMonth({month},{year})";

                sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                sqlDataAdapter.Fill(dataSet);

                dataSet.Tables[0].TableName = tableName;
            }

            string date;
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                date = ((DateTime)row.ItemArray[0]).ToString("dd.MM");
                resultDictionary.Add(date, Convert.ToInt32(row.ItemArray[1]));
            }

            return resultDictionary;
        }

        public Dictionary<string, int> LoadOrdersSumForYear(int year)
        {
            resultDictionary = new Dictionary<string, int>();

            sqlCommand = new SqlCommand();
            dataSet = new DataSet();

            string tableName = "OrdersSumForYear";

            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                sqlCommand.Connection = sqlConnection;

                sqlCommand.CommandText = $"select * from OrdersSumForYear({year})";

                sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                sqlDataAdapter.Fill(dataSet);

                dataSet.Tables[0].TableName = tableName;
            }
            int monthNumber;
            string monthName;
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                monthNumber = Convert.ToInt32(row.ItemArray[0]);
                monthName=getMonthNameByNumber(monthNumber);
               

                resultDictionary.Add(monthName, Convert.ToInt32(row.ItemArray[1]));
            }

            return resultDictionary;

        }


        public DataTable LoadTops(string nameTable)
        {

            sqlCommand = new SqlCommand();
            dataSet = new DataSet();

            string[] NamesViews = new string[] { "TopCars", "ClientsOrdersCount", "TopService" };
            string table;

            if (nameTable == "cars")
                table = NamesViews[0];
            else if (nameTable == "clients")
                table = NamesViews[1];
            else 
                table = NamesViews[2];
            

            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                sqlCommand.Connection = sqlConnection;

                sqlCommand.CommandText = $"select * from {table}";

                sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                sqlDataAdapter.Fill(dataSet);

                dataSet.Tables[0].TableName = table;
            }



            return dataSet.Tables[0];
        }

        #endregion


        public DataTable getServiceByIdOrder(int id)
        {
            DataSet dataSet = new DataSet();

            using (sqlConnection = new SqlConnection(connectionString))
            {

                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandText = $"exec getServiceByIdOrder {id}";

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataSet);

                DataTable table = dataSet.Tables[0];

                return table;
            }
        }
    }
}
