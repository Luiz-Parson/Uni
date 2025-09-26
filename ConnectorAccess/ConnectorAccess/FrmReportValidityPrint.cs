using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace ConnectorAccess
{
    public partial class FrmReportValidityPrint : Form
    {
        DataTable dt = new DataTable();

        public FrmReportValidityPrint(DataTable dt)
        {
            InitializeComponent();
            this.dt = dt;
        }

        private void FrmReportPrint_Load(object sender, EventArgs e)
        {
            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "ConnectorAccess.reports.ReportValidity.rdlc";

            Microsoft.Reporting.WinForms.ReportParameter[] para = new Microsoft.Reporting.WinForms.ReportParameter[]
            {
                new Microsoft.Reporting.WinForms.ReportParameter("pDate", DateTime.Now.ToString())
            };

            this.reportViewer1.LocalReport.SetParameters(para);
            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dt));
            this.reportViewer1.RefreshReport();
        }
    }
}
