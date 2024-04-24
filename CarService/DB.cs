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

namespace CarService
{
    internal class DB
    {
        private const string connectionString = "Data Source=DESKTOP-UN6IMVL\\SQLEXPRESS;Initial Catalog=CarService;Integrated Security=True";
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
                        employee.Position = reader.GetString(5);
                        employee.Password = reader.GetString(6);

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
                        employee.Position = reader.GetString(5);
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

        public List<Category> getCategories() {
            List<Category> categories = new List<Category>();
            Category category;

            string sqlExpression = "SELECT * FROM getServiceCategories";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        category = new Category();
                        category.Id = reader.GetInt32(0);
                        category.Name = reader.GetString(1);
                        categories.Add(category);

                    }
                }
                reader.Close();
                return categories;
            }
        }

        public DataTable getCategories1()
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

        public int AddNewClient(Client client)
        {
            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandText = "addClient";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FName", client.FName);
                command.Parameters.AddWithValue("@MName", client.MName);
                command.Parameters.AddWithValue("@Phone", client.Phone);


                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 4));
                command.Parameters["@id"].Direction = ParameterDirection.Output;

                command.ExecuteNonQuery();

                int id = (int)command.Parameters["@id"].Value;

                sqlConnection.Close();
                return id;
            }
        }

        public int AddCarDetails(CarDetails carDetails)
        {


            sqlConnection = new SqlConnection(connectionString);


            sqlConnection.Open();

            SqlCommand command = sqlConnection.CreateCommand();
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

            sqlConnection.Close();
            return id;


        }

        public int AddOrder(Order order, DataTable service)
        {
            sqlConnection = new SqlConnection(connectionString);


            sqlConnection.Open();

            SqlCommand command = sqlConnection.CreateCommand();
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

            AddServiceDetails(id, service, sqlConnection);

            sqlConnection.Close();
            return id;
        }

        public void AddServiceDetails(int id, DataTable serviceDetails, SqlConnection connection)
        {
            
                string comandText = CreateComandToAddServiceDetails(id, serviceDetails);
                SqlCommand command = connection.CreateCommand();
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

    }
}
