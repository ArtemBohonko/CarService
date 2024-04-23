using CarService.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CarService
{
    public partial class MainForm : Form
    {
        public int IdImployee;
        private Employee employee;
        private DB dataBase;
        public Form parentForm;

        private DataTable carDataTable;
        private List<Employee> employeeList;
        public MainForm(int Id)
        {
            InitializeComponent();
            this.IdImployee = Id;
            dataBase = new DB();
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            setEmployee();

            this.carDataTable=dataBase.LoadCars();
            this.employeeList = dataBase.getEmployeeByPosition(3);

            setCarsInCB(this.carDataTable);
            setYearInCB();
            setEngineInCB();
            setValueInCB();
            setEmployeesInCB();
        }


        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //this.parentForm.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            setModelInCB(this.carDataTable);

        }

        private void setEmployee()
        {
            this.employee = dataBase.getEmployeeById(this.IdImployee);
            this.Text = "Сотрудник: " + this.employee.FName + ' ' + this.employee.MName + ' ' + this.employee.LName;

        }

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
            BindingSource source = new BindingSource();
            source.DataSource = cars;
            source.Filter = "Brand='" + comboBox1.SelectedItem.ToString() + "'";
            comboBox2.Enabled = true;
            comboBox2.DataSource = source;
            comboBox2.DisplayMember = "Model";

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

        private void setEngineInCB()
        {
            List<string> engine = new List<string>();
            string[] s = new string[7] { "Бензин", "Бензин(пропан-бутан)", "Бензин(метан)", "Бензин(гибрид)", "Дизель", "Дизель(гибрид)", "Электро" };
            engine.AddRange(s);
            comboBox4.DataSource = engine;
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

        private void setEmployeesInCB()
        {
            List<string> names= new List<string>(); 
            string fullName;

            foreach(Employee emp in employeeList)
            {
                fullName = emp.FName + ' ' + emp.MName + ' ' + emp.LName;
                names.Add(fullName);
            }

            comboBox8.DataSource = names;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text.ToUpper();
            textBox1.Select(textBox1.Text.Length, 0);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8) //Если символ, введенный с клавы - не цифра (IsDigit),
            {
                e.Handled = true;// то событие не обрабатывается. ch!=8 (8 - это Backspace)
            }
        }
    }
}
