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
                string tableName = "Cars";

                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandText = "SELECT * FROM getPasswordEmployee";

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataSet, tableName);

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
       
    }
}
