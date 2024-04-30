using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Xml.Linq;
using CarService.Objects;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace CarService
{
    internal class DB
    {
        public string connectionString = "Data Source=DESKTOP-UN6IMVL\\SQLEXPRESS;Initial Catalog=CarService;Integrated Security=True";
        private SqlConnection sqlConnection;

        public DB()
        {
            //sqlConnection = new SqlConnection(connectionString);
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

        public List<Employee> getEmployeeByPosition(int position)
        {
            List<Employee> employees = new List<Employee>();
            Employee employee;

            string sqlExpression = "getEmployeeByPosition";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                // указываем, что команда представляет хранимую процедуру
                command.CommandType = System.Data.CommandType.StoredProcedure;
                // параметр для ввода имени
                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@position",
                    Value = position
                };
                // добавляем параметр
                command.Parameters.Add(nameParam);
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        employee=new Employee();
                        employee.Id = reader.GetInt32(0);
                        employee.FName = reader.GetString(1);
                        employee.MName = reader.GetString(2);
                        employee.LName = reader.GetString(3);
                        employee.Phone = reader.GetString(4);
                        employee.PositionName = reader.GetString(5);
                        employee.Password = reader.GetString(6);
                        employees.Add(employee);

                    }
                }
                reader.Close();
                return employees;
            }
        }

        public List<Client> getClients()
        {
            List<Client> clients = new List<Client>();
            Client client;

            string sqlExpression = "SELECT * FROM Clients";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        client = new Client();
                        client.Id = reader.GetInt32(0);
                        client.FName = reader.GetString(1);
                        client.MName = reader.GetString(2);
                        client.Phone = reader.GetString(3);
                        clients.Add(client);

                    }
                }
                reader.Close();
                return clients;
            }
        }

    

        public DataTable getCategories()
        {
            DataSet dataSet = new DataSet();

            using (sqlConnection = new SqlConnection(connectionString))
            {

                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandText = "SELECT * FROM getServiceCategories";

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataSet);

                DataTable table = dataSet.Tables[0];



                return table;
            }
        }
        public DataTable LoadCars()
        {
            
            DataSet dataSet = new DataSet();

            using (sqlConnection = new SqlConnection(connectionString))
            {
                string tableName = "Cars";

                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandText = "SELECT * FROM Cars ORDER BY Brand;";

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataSet);

                DataTable table = dataSet.Tables[0];

                table.TableName = tableName;

               
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
            
            DataTable orders = new DataTable();
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
            //6-IdCar,7-Brand,8-Model,9-Year,10-Engine,11-Value,12-Mileage,13-VIN, 
            //14-IdEmp,15-FNameEmp,16-MNameEmp,17-LNameEmp,18-PhoneEmp,
            //19-IdMaster,20-FNameMaster,21-MNameMaster,22-LNameMaster,23-PhoneMaster,
            //24-TotalCost,25-Status,26-Comment
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
                newRow[orders.Columns[7].ColumnName] = Convert.ToInt32(row.ItemArray[24]);
                newRow[orders.Columns[8].ColumnName] = Convert.ToString(row.ItemArray[26]);
                newRow[orders.Columns[9].ColumnName] = Convert.ToString(ConvertStatus(Convert.ToInt32( row.ItemArray[25])));

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
    }
}
