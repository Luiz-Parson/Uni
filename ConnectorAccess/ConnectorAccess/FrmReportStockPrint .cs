using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace ConnectorAccess
{
    public partial class FrmReportStockPrint : Form
    {
        DataTable dt = new DataTable();
        DateTime initialDate;
        DateTime finalDate;

        public FrmReportStockPrint(DataTable dt, DateTime initialDate, DateTime finalDate)
        {
            InitializeComponent();
            this.dt = dt;
            this.initialDate = initialDate;
            this.finalDate = finalDate;
        }

        private void FrmReportPrint_Load(object sender, EventArgs e)
        {
            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "ConnectorAccess.reports.ReportAccessControlStockGroup.rdlc";

            Microsoft.Reporting.WinForms.ReportParameter[] para = new Microsoft.Reporting.WinForms.ReportParameter[]
            {
                new Microsoft.Reporting.WinForms.ReportParameter("pDate", DateTime.Now.ToString()),
                new Microsoft.Reporting.WinForms.ReportParameter("pDateInitial", initialDate.ToString()),
                new Microsoft.Reporting.WinForms.ReportParameter("pDateFinal", finalDate.ToString())
            };

            this.reportViewer1.LocalReport.SetParameters(para);
            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dt));
            this.reportViewer1.RefreshReport();
        }
    }
}
