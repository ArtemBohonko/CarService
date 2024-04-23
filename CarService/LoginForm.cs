using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                
            if (passwords.Contains(enterPassword))
            {
                MainForm mainForm = new MainForm(dataBase.getIdEmployee(enterPassword));
                mainForm.Show();
                //mainForm.IdImployee = dataBase.getIdEmployee(enterPassword);
                mainForm.parentForm = this;
                this.Hide();
                
               
            }
        }
    }
}
