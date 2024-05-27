using CarService.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CarService
{
    public partial class LoginForm : Form
    {
        DB dataBase;
        DataTable employees;
        ToolTip toolTip;
        Employee employee;
        string message = "Показать пароль";

        public LoginForm()
        {
            InitializeComponent();
            dataBase = new DB();
            employees=dataBase.getPasswordsEmployee();
        }

        private void button_Confirm_Click(object sender, EventArgs e)
        {
            string enterPassword = textBox1.Text;
            var secondArray = employees.Rows.Cast<DataRow>().Select(x => x.ItemArray[1]);
            List<string> passwords=new List<string>();
            for(int i = 0;i<employees.Rows.Count;i++)
            {
                passwords.Add(employees.Rows[i][0].ToString());
            }

            int idEmployee;

            if (passwords.Contains(enterPassword))
            {
                idEmployee = dataBase.getIdEmployee(enterPassword);
                employee = dataBase.getEmployeeById(idEmployee);
            }
            else
            {
                MessageBox.Show(
                           "Сотрудник с данным паролем не найден, проверьте правильность введённого пароля",
                           "Не верный пароль",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error,
                           MessageBoxDefaultButton.Button1);
                return;
            }

            if(employee.PositionId == "1") {
                AdminForm adminForm = new AdminForm(idEmployee);
                adminForm.Show();
                adminForm.Owner = this;
                this.Hide();
            }
            else if(employee.PositionId == "2")
            {
                MainForm mainForm = new MainForm(idEmployee);
                mainForm.Show();
                mainForm.Owner = this;
                this.Hide();
            }
            else
            {
                InProgerssForm inProgerssForm = new InProgerssForm(idEmployee);
                inProgerssForm.Show();
                inProgerssForm.Owner = this;
                this.Hide();
            }
            textBox1.Text = string.Empty;

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (textBox1.UseSystemPasswordChar == true)
            {
                textBox1.UseSystemPasswordChar = false;
                pictureBox1.Image = CarService.Properties.Resources.close_eye;
                message = "Скрыть пароль";
            }
            else
            {
                textBox1.UseSystemPasswordChar = true;
                pictureBox1.Image = CarService.Properties.Resources.open_eye;
                message = "Показать пароль";
            }
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            toolTip = new ToolTip();
            toolTip.SetToolTip(pictureBox1, message);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button_Confirm_Click(sender, e);
            }
        }
    }
}
