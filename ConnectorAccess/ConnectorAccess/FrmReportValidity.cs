using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Net.Http;
using ConnectorAccess.models;
using Newtonsoft.Json;
using System.Text;

namespace ConnectorAccess
{
    public partial class FrmReportValidity : Form
    {
        protected static readonly Logger Logger = new Logger();
        DataTable dt = new DataTable();
        string apiAddress = ConfigurationManager.AppSettings["APIAddress"];
        string apiPort = ConfigurationManager.AppSettings["APIPort"];

        private static readonly HttpClient httpClient = new HttpClient();

        public FrmReportValidity()
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

            dtvResults.CellFormatting += dtvResults_CellFormatting;
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
            dtvResults.Columns["Description"].Width = 150;
            dtvResults.Columns["Description"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtvResults.Columns["Description"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dtvResults.Columns["SKU"].HeaderText = "SKU";
            dtvResults.Columns["SKU"].Width = 50;
            dtvResults.Columns["SKU"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtvResults.Columns["SKU"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dtvResults.Columns["EPC"].HeaderText = "EPC";
            dtvResults.Columns["EPC"].Width = 100;
            dtvResults.Columns["EPC"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtvResults.Columns["EPC"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dtvResults.Columns["ValidityDate"].HeaderText = "Data de validade";
            dtvResults.Columns["ValidityDate"].Width = 130;
            dtvResults.Columns["ValidityDate"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtvResults.Columns["ValidityDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtvResults.Columns["ValidityDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                dtvResults.Columns.Clear();
                dtvResults.DataSource = null;

                var result = await getValidityReport(txtDescription.Text.Trim(), txtEpc.Text.Trim(), txtSKU.Text.Trim());

                if (result == null || result.Rows.Count == 0)
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
            DateTime today = DateTime.Today;

            dtRep.Columns.Add("Description");
            dtRep.Columns.Add("SKU");
            dtRep.Columns.Add("EPC");
            dtRep.Columns.Add("ValidityDate");
            dtRep.Columns.Add("BackgroundColor", typeof(string)); // Nova coluna para cor de fundo
            dtRep.Columns.Add("ForeColor", typeof(string));       // Nova coluna para cor do texto

            foreach (DataGridViewRow item in dtvResults.Rows)
            {
                DateTime validityDate = DateTime.MinValue;
                bool hasValidDate = DateTime.TryParse(item.Cells["ValidityDate"].Value?.ToString(), out validityDate);

                string backgroundColor = "White";
                string foreColor = "Black";

                if (hasValidDate)
                {
                    if (validityDate.Date < today)
                    {
                        backgroundColor = "Red";
                        foreColor = "White";
                    }
                    else if (validityDate.Date <= today.AddDays(15))
                    {
                        backgroundColor = "Gold";
                        foreColor = "Black";
                    }
                }

                dtRep.Rows.Add(
                    item.Cells["Description"].Value.ToString(),
                    item.Cells["SKU"].Value.ToString(),
                    item.Cells["EPC"].Value.ToString(),
                    item.Cells["ValidityDate"].Value != null ? item.Cells["ValidityDate"].Value.ToString() : "",
                    backgroundColor,
                    foreColor
                );
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
            using (var frmReportPrint = new FrmReportValidityPrint(dataReport))
            {
                frmReportPrint.ShowDialog();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSKU.Text = "";
            txtEpc.Text = "";
            txtDescription.Text = "";

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
                sfd.FileName = "RelatorioValidade-" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2") + DateTime.Now.Hour.ToString("D2") + DateTime.Now.Minute.ToString("D2") + DateTime.Now.Second.ToString("D2") + ".csv";
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

        private async Task<DataTable> getValidityReport(string description, string epc, string sku)
        {
            try
            {
                string apiUrl = $"http://{apiAddress}:{apiPort}/api/report/getValidityReport";

                var validityReportDTO = new ValidityReportDTO
                {
                    Description = description,
                    EPC = epc,
                    SKU = sku
                };

                var jsonContent = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(validityReportDTO),
                    Encoding.UTF8,
                    "application/json"
                );

                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<DataTable>(jsonResponse);
                }
                else
                {
                    Console.WriteLine($"Erro na requisição: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao chamar o endpoint: {ex.Message}");
                return null;
            }
        }

        private void dtvResults_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DateTime today = DateTime.Today;
            // Confirma se é a coluna "ValidityDate"
            if (dtvResults.Columns[e.ColumnIndex].Name == "ValidityDate")
            {
                var cellValue = dtvResults.Rows[e.RowIndex].Cells["ValidityDate"].Value;

                if (cellValue != null && DateTime.TryParse(cellValue.ToString(), out DateTime validade))
                {
                    if (validade.Date < today)
                    {
                        e.CellStyle.BackColor = Color.Red;
                        e.CellStyle.ForeColor = Color.White;
                    }
                    else if (validade.Date <= today.AddDays(15))
                    {
                        e.CellStyle.BackColor = Color.Gold;
                        e.CellStyle.ForeColor = Color.Black;
                    }
                }
            }
        }
    }
}
