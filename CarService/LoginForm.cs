using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace CarService
{
    public partial class LoginForm : Form
    {
        DB dataBase;
        DataTable employees;
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

            if(idEmployee == 1) {
                AdminForm adminForm = new AdminForm(idEmployee);
                adminForm.Show();
                this.Hide();
            }
            else if(idEmployee == 2)
            {
                MainForm mainForm = new MainForm(idEmployee);
                mainForm.Show();
                this.Hide();
            }
            else
            {
                InProgerssForm inProgerssForm = new InProgerssForm(idEmployee);
                inProgerssForm.Show();
                this.Hide();
            }

        }
    }
}
