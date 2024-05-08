using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace CarService
{
    public partial class ReportForm : Form
    {
        List<ReportParameter> parameters;
        string mode;
        DataTable dt;
        string reportPath;
        public ReportForm(DataTable dt, List<ReportParameter> parametrs, string name)
        {
            this.dt= dt;
            this.parameters= parametrs;
            this.mode = name;
            InitializeComponent();
        }

        private void ReportForm_Load(object sender, EventArgs e)
        {

            if (mode == "order")
            {
                this.Text = "Отчёт по заказу";
                reportPath= Path.GetFullPath(Directory.GetCurrentDirectory() + "\\..\\..\\Reports\\") + "OrderReport.rdlc";
            }
            else if (mode == "ordersView")
            {
                this.Text = "Отчёт по заказам";
                reportPath = Path.GetFullPath(Directory.GetCurrentDirectory() + "\\..\\..\\Reports\\") + "OrdersViewReport.rdlc";
            }

            SetReportParametrs();

            this.reportViewer1.RefreshReport();
        }

        private void SetReportParametrs()
        {

            reportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource dataSource = new ReportDataSource("DataSet1", dt);
            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Add(dataSource);
            reportViewer1.LocalReport.SetParameters(parameters);
        }
    }
}
