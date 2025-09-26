using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Linq;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace ConnectorAccess
{
    public partial class FrmReportExclusion : Form
    {
        protected static readonly Logger Logger = new Logger();
        DataTable dt = new DataTable();
        string apiAddress = ConfigurationManager.AppSettings["APIAddress"];
        string apiPort = ConfigurationManager.AppSettings["APIPort"];

        private static readonly HttpClient httpClient = new HttpClient();

        public FrmReportExclusion()
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
            dtvResults.Columns["Description"].Width = 118;
            dtvResults.Columns["Description"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtvResults.Columns["Description"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dtvResults.Columns["SKU"].HeaderText = "SKU";
            dtvResults.Columns["SKU"].Width = 50;
            dtvResults.Columns["SKU"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtvResults.Columns["SKU"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dtvResults.Columns["EPC"].HeaderText = "EPC";
            dtvResults.Columns["EPC"].Width = 190;
            dtvResults.Columns["EPC"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtvResults.Columns["EPC"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dtvResults.Columns["Category"].HeaderText = "Categoria";
            dtvResults.Columns["Category"].Width = 130;
            dtvResults.Columns["Category"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtvResults.Columns["Category"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dtvResults.Columns["QuantityOfDeletes"].HeaderText = "Quantidade de vezes utilizado";
            dtvResults.Columns["QuantityOfDeletes"].Width = 115;
            dtvResults.Columns["QuantityOfDeletes"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtvResults.Columns["QuantityOfDeletes"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dtvResults.Columns["ExcludedOn"].HeaderText = "Excluído em";
            dtvResults.Columns["ExcludedOn"].Width = 130;
            dtvResults.Columns["ExcludedOn"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtvResults.Columns["ExcludedOn"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtvResults.Columns["ExcludedOn"].DefaultCellStyle.Format = "dd/MM/yyyy - HH:mm:ss";
        }

        private async void btnSearch_Click(object sender, EventArgs e)
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

                var result = await getReport(txtDescription.Text.Trim(), txtEpc.Text.Trim(), txtSKU.Text.Trim(), dtpAccessedOnInitial.Value, dtpAccessedOnFinal.Value);

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

            dtRep.Columns.Add("Description");
            dtRep.Columns.Add("SKU");
            dtRep.Columns.Add("EPC");
            dtRep.Columns.Add("Category");
            dtRep.Columns.Add("QuantityOfDeletes");
            dtRep.Columns.Add("ExcludedOn");

            foreach (DataGridViewRow item in dtvResults.Rows)
            {
                dtRep.Rows.Add(
                    item.Cells["Description"].Value.ToString(),
                    item.Cells["SKU"].Value.ToString(),
                    item.Cells["EPC"].Value.ToString(),
                    item.Cells["Category"].Value.ToString(),
                    item.Cells["QuantityOfDeletes"].Value,
                    item.Cells["ExcludedOn"].Value != null ? item.Cells["ExcludedOn"].Value.ToString() : "");
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
            using (var frmReportPrint = new FrmReportExclusionPrint(dataReport, dtpAccessedOnInitial.Value, dtpAccessedOnFinal.Value))
            {
                frmReportPrint.ShowDialog();
            }
        }

        private async Task<DataTable> getReport(string description, string epc, string sku, DateTime initialDate, DateTime endDate)
        {
            try
            {
                string apiUrl = $"http://{apiAddress}:{apiPort}/api/product/getAllExclusions";

                var exclusionReportRequestDTO = new ExclusionReportRequestDTO
                {
                    Description = description,
                    EPC = epc,
                    SKU = sku,
                    InitialDate = initialDate,
                    EndDate = endDate
                };

                var jsonContent = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(exclusionReportRequestDTO),
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSKU.Text = "";
            txtEpc.Text = "";
            txtDescription.Text = "";
            dtpAccessedOnInitial.Value = DateTime.Now.Date;
            dtpAccessedOnFinal.Value = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

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
                sfd.FileName = "RelatorioBaixas-" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2") + DateTime.Now.Hour.ToString("D2") + DateTime.Now.Minute.ToString("D2") + DateTime.Now.Second.ToString("D2") + ".csv";
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
