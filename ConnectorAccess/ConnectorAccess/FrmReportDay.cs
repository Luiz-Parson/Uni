using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectorAccess
{
    public partial class FrmReportDay : Form
    {
        protected static readonly Logger Logger = new Logger();
        DataTable dt = new DataTable();
        public FrmReportDay()
        {
            InitializeComponent();
        }

        private void FrmReport_Load(object sender, System.EventArgs e)
        {
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = System.Drawing.Color.White;
            this.Width = 777;
            this.Height = 526;

            System.Windows.Forms.ToolTip ttpBtnSearch = new System.Windows.Forms.ToolTip();
            ttpBtnSearch.SetToolTip(this.btnSearch, "Pesquisar");

            System.Windows.Forms.ToolTip ttpBtnPrint = new System.Windows.Forms.ToolTip();
            ttpBtnSearch.SetToolTip(this.btnPrint, "Imprimir");

            System.Windows.Forms.ToolTip ttpBtnCsv = new System.Windows.Forms.ToolTip();
            ttpBtnSearch.SetToolTip(this.btnCsv, "Exportar para CSV");

            System.Windows.Forms.ToolTip ttpBtnClear = new System.Windows.Forms.ToolTip();
            ttpBtnSearch.SetToolTip(this.btnClear, "Limpar");

            dtpAccessedOnInitial.Value = DateTime.Now.Date;
            dtpAccessedOnFinal.Value = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            dtvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void ConfigGridResultsGroup()
        {
            dtvResults.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);
            dtvResults.DefaultCellStyle.Font = new Font("Arial", 9);
            dtvResults.RowHeadersWidth = 25;
            dtvResults.DefaultCellStyle.SelectionBackColor = Color.White;
            dtvResults.DefaultCellStyle.SelectionForeColor = Color.Black;

            dtvResults.Columns["Description"].HeaderText = "Descrição";
            dtvResults.Columns["Description"].Width = 160;
            dtvResults.Columns["Description"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtvResults.Columns["Description"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dtvResults.Columns["SKU"].HeaderText = "SKU";
            dtvResults.Columns["SKU"].Width = 60;
            dtvResults.Columns["SKU"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtvResults.Columns["SKU"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dtvResults.Columns["Status"].HeaderText = "Status";
            dtvResults.Columns["Status"].Width = 80;
            dtvResults.Columns["Status"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtvResults.Columns["Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dtvResults.Columns["Quantidade"].HeaderText = "Quantidade";
            dtvResults.Columns["Quantidade"].Width = 90;
            dtvResults.Columns["Quantidade"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtvResults.Columns["Quantidade"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dtvResults.Columns["ExitDate"].HeaderText = "Enviado em";
            dtvResults.Columns["ExitDate"].Width = 130;
            dtvResults.Columns["ExitDate"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtvResults.Columns["ExitDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtvResults.Columns["ExitDate"].DefaultCellStyle.Format = "dd/MM/yyyy - HH:mm:ss";

            dtvResults.Columns["EntryDate"].HeaderText = "Recebido em";
            dtvResults.Columns["EntryDate"].Width = 130;
            dtvResults.Columns["EntryDate"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtvResults.Columns["EntryDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtvResults.Columns["EntryDate"].DefaultCellStyle.Format = "dd/MM/yyyy - HH:mm:ss";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtpAccessedOnFinal.Value.Date < dtpAccessedOnInitial.Value.Date)
                {
                    MessageBox.Show("A data final deve ser maior ou igual a data inicial.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                dtvResults.Columns.Clear();
                dtvResults.DataSource = null;

                var result = AccessControlDay.GenerateProductReport(txtDescription.Text.Trim(), txtEpc.Text.Trim(), txtSKU.Text.Trim(), dtpAccessedOnInitial.Value, dtpAccessedOnFinal.Value);

                if (result.Rows.Count == 0)
                {
                    #region header with no record
                    result = new DataTable();
                    result.Columns.Add("Resultado", typeof(string));
                    result.Rows.Add("Nenhum registro foi encontrado.");
                    dtvResults.DataSource = result;
                    dtvResults.Columns[0].Width = 200;
                    #endregion
                }
                else
                {
                    dtvResults.DataSource = result;
                    ConfigGridResultsGroup();

                    var resultAllEntrysAndExits = AccessControlDay.GetAllEntrysAndExits(txtDescription.Text.Trim(), txtEpc.Text.Trim(), txtSKU.Text.Trim(), dtpAccessedOnInitial.Value, dtpAccessedOnFinal.Value);

                    int totalEntradas = Convert.ToInt32(resultAllEntrysAndExits.Rows[0]["TotalEntradas"]);
                    int totalSaidas = Convert.ToInt32(resultAllEntrysAndExits.Rows[0]["TotalSaidas"]);

                    txtReceived.Text = totalEntradas.ToString();
                    txtSent.Text = totalSaidas.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error("Erro no BtnSearch do FrmReport.", ex);
            }
        }

        private DataTable GenerateReportData()
        {
            DataTable dtRep = new DataTable();

            dtRep.Columns.Add("Description");
            dtRep.Columns.Add("SKU");
            dtRep.Columns.Add("Status");
            dtRep.Columns.Add("Quantidade");
            dtRep.Columns.Add("ExitDate");
            dtRep.Columns.Add("EntryDate");

            foreach (DataGridViewRow item in dtvResults.Rows)
            {
                dtRep.Rows.Add(
                    item.Cells["Description"].Value.ToString(),
                    item.Cells["SKU"].Value.ToString(),
                    item.Cells["Status"].Value.ToString(),
                    item.Cells["Quantidade"].Value,
                    item.Cells["ExitDate"].Value != null ? item.Cells["ExitDate"].Value.ToString() : "",
                    item.Cells["EntryDate"].Value != null ? item.Cells["EntryDate"].Value.ToString() : "");
            }

            return dtRep;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dtvResults.Rows.Count == 0)
            {
                MessageBox.Show("Não há dados para serem impressos. Por favor, efetue uma pesquisa.", "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dtvResults.Columns.Count == 1)
            {
                MessageBox.Show("A pesquisa não retornou registros. Por favor refaça a pesquisa.", "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var dataReport = GenerateReportData();
            using (var frmReportPrint = new FrmReportDayPrint(dataReport, dtpAccessedOnInitial.Value, dtpAccessedOnFinal.Value))
            {
                frmReportPrint.ShowDialog();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSKU.Text = "";
            txtEpc.Text = "";
            txtDescription.Text = "";
            dtpAccessedOnInitial.Value = DateTime.Now.Date;
            dtpAccessedOnFinal.Value = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            txtReceived.Text = "0";
            txtSent.Text = "0";

            dtvResults.Columns.Clear();
            dtvResults.DataSource = null;
        }

        private void txtEpc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnSearch.PerformClick();
        }

        private void btnCsv_Click(object sender, EventArgs e)
        {
            if (dtvResults.Columns.Count == 1)
            {
                MessageBox.Show("A pesquisa não retornou registros. Por favor refaça a pesquisa.", "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dtvResults.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV (*.csv)|*.csv";
                sfd.FileName = "RelatorioMovimentacao-" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2") + DateTime.Now.Hour.ToString("D2") + DateTime.Now.Minute.ToString("D2") + DateTime.Now.Second.ToString("D2") + ".csv";
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("Não foi possível gravar os dados!" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Logger.Error("Não foi possível gravar os dados no disco. Exportando report para csv", ex);
                            return;
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            int columnCount = dtvResults.Columns.Count;
                            string columnNames = "";
                            string[] outputCsv = new string[dtvResults.Rows.Count + 1];
                            for (int i = 0; i < columnCount; i++)
                            {
                                columnNames += dtvResults.Columns[i].HeaderText.ToString();
                                if (i < columnCount-1)
                                    columnNames += ";";
                            }
                            outputCsv[0] += columnNames;

                            for (int i = 1; (i - 1) < dtvResults.Rows.Count; i++)
                            {
                                for (int j = 0; j < columnCount; j++)
                                {
                                    outputCsv[i] += dtvResults.Rows[i - 1].Cells[j].Value.ToString();
                                    if (j < columnCount-1)
                                        outputCsv[i] += ";";
                                }
                            }

                            File.WriteAllLines(sfd.FileName, outputCsv, System.Text.Encoding.UTF8);
                            MessageBox.Show("Dados exportados com sucesso.", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ocorreu um erro ao exportar os dados. " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Logger.Error("Ocorreu um erro ao exportar os dados. Exportando para csv report.", ex);
                            return;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Não há dados para serem impressos. Por favor, efetue uma pesquisa.", "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
