using Newtonsoft.Json;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using System.Net.Mail;
using System.IO;
using System.Collections.Concurrent;

namespace ConnectorAccess
{
    public partial class FrmMovementControl : Form
    {
        protected static readonly Logger Logger = new Logger();
        private TcpReader tcpReader;
        private bool isListening;
        private ConcurrentDictionary<string, byte> epcsAdicionados = new ConcurrentDictionary<string, byte>();
        private ConcurrentDictionary<(string, string), byte> produtosAdicionados = new ConcurrentDictionary<(string, string), byte>();

        private static readonly HttpClient httpClient = new HttpClient();

        string epcStartFilter = ConfigurationManager.AppSettings["EpcStartFilter"];
        string apiAddress = ConfigurationManager.AppSettings["APIAddress"];
        string apiPort = ConfigurationManager.AppSettings["APIPort"];
        private bool testMode => bool.TryParse(ConfigurationManager.AppSettings["TestMode"], out bool result) && result;

        public FrmMovementControl(TcpReader tcpReader)
        {
            InitializeComponent();

            this.tcpReader = tcpReader;
            this.tcpReader.OnEpcReceived += HandleEpcReceived;
        }

        private void FrmAccessMovementControl_Load(object sender, EventArgs e)
        {
        }

        private void iniciar_Click(object sender, EventArgs e)
        {

            if (isListening)
            {
                tcpReader.StopListening();
                btnIniciar.Text = "Iniciar";
                btnIniciar.BackColor = System.Drawing.Color.Lime;
                isListening = false;
            }
            else
            {
                if (testMode)
                {
                    // Iniciar a simulação R700
                    string[] mockEpcs = {
                        "1,ABCD00001000000020250731,R700Parson\r\n" +
                        "1,ABCD00002000000020250930,R700Parson\r\n" +
                        "1,ABCD00003000000020250720,R700Parson\r\n" +
                        "1,ABCD00004000000020251231,R700Parson\r\n" +
                        "1,ABCD00005000000020260115,R700Parson\r\n" +
                        "1,ABCD00006000000020260310,R700Parson\r\n" +
                        "1,ABCD00007000000020251105,R700Parson\r\n" +
                        "1,ABCD00008000000020250818,R700Parson\r\n" +
                        "1,ABCD00009000000020251130,R700Parson\r\n" +
                        "1,ABCD00010000000020251001,R700Parson\r\n" +
                        "1,ABCD00011000000020260131,R700Parson\r\n" +
                        "1,ABCD00012000000020260228,R700Parson\r\n" +
                        "1,ABCD00013000000020260315,R700Parson\r\n" +
                        "1,ABCD00014000000020250825,R700Parson\r\n" +
                        "1,ABCD00015000000020251224,R700Parson\r\n" +
                        "1,ABCD00016000000020250909,R700Parson\r\n" +
                        "1,ABCD00017000000020251030,R700Parson\r\n" +
                        "1,ABCD00018000000020250805,R700Parson\r\n" +
                        "1,ABCD00019000000020251111,R700Parson\r\n" +
                        "1,ABCD00020000000020250714,R700Parson\r\n"
                    };

                    tcpReader.StartMockingEpcReadings(mockEpcs, 1000);
                }
                else
                {
                    // Iniciar leitura
                    tcpReader.StartListening();
                }

                btnIniciar.Text = "Parar";
                btnIniciar.BackColor = System.Drawing.Color.Red;
                isListening = true;
            }
        }

        private async void finalizar_Click(object sender, EventArgs e)
        {
            btnFinalizar.Enabled = false;

            try
            {
                string tipoDeEnvio = comboBoxTipo.SelectedItem?.ToString();
                string tipoDeStatus = comboBoxStatus.SelectedItem?.ToString();

                if (tipoDeEnvio == null)
                {
                    MessageBox.Show("Insira uma opção de envio!",
                    "Atenção",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                    return;
                }

                if (tipoDeStatus == null && tipoDeEnvio?.ToUpper() != "CONTAGEM")
                {
                    MessageBox.Show("Insira uma opção de status!",
                    "Atenção",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                    return;
                }

                if (tipoDeEnvio != null && tipoDeEnvio.ToUpper() != "CONTAGEM")
                {
                    DateTime accessedOn = DateTime.Now;

                    foreach (DataGridViewRow row in dataGridViewControlProducts.Rows)
                    {
                        string epc = row.Cells["EPC"].Value?.ToString();
                        string status = row.Cells["Status"].Value?.ToString();

                        if (!string.IsNullOrEmpty(epc))
                        {
                            var product = await GetProductByEpc(epc);

                            if (product == null)
                            {
                                product = await AddProduct("Desconhecido", "Desconhecido", epc, Program.systemUserLogged.Username);
                            }

                            if (tipoDeEnvio.ToUpper() == "RECEBER")
                            {
                                await AddAccessControl("IN", product.Id, accessedOn, tipoDeStatus);
                            }
                            else if (tipoDeEnvio.ToUpper() == "ENVIAR")
                            {
                                await AddAccessControl("OUT", product.Id, accessedOn, tipoDeStatus);
                            }
                        }
                    }
                }

                if (tipoDeEnvio?.ToUpper() == "CONTAGEM")
                {
                    var productCounts = new Dictionary<string, int>();

                    foreach (DataGridViewRow row in dataGridViewControlProducts.Rows)
                    {
                        string sku = row.Cells["SKU"].Value?.ToString() ?? "Desconhecido";
                        string description = row.Cells["descricao"].Value?.ToString() ?? "Desconhecido";
                        string key = $"{description} ({sku})";

                        if (productCounts.ContainsKey(key))
                        {
                            productCounts[key]++;
                        }
                        else
                        {
                            productCounts[key] = 1;
                        }
                    }

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(lblTotal.Text);

                    foreach (var entry in productCounts)
                    {
                        sb.AppendLine($"{entry.Key}: {entry.Value}");
                    }

                    MessageBox.Show(sb.ToString(), "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Ação de {tipoDeEnvio} finalizada!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    int seconds = int.Parse(ConfigurationManager.AppSettings["IntervalSeconds"]);
                    DateTime dataInicial = DateTime.Now.AddSeconds(-seconds);
                    DateTime dataFinal = DateTime.Now;
                    
                    var result = AccessControlDay.GenerateProductReport("", "", "", dataInicial, dataFinal);

                    Console.WriteLine($"Total de linhas: {result.Rows.Count}");

                    using (var frmReportPrint = new FrmReportDayPrint(result, dataInicial, dataFinal))
                    {
                        frmReportPrint.InitializeReport();

                        byte[] pdfBytes = frmReportPrint.ExportReportToPdf();

                        try
                        {
                            EnviarEmailComAnexo(pdfBytes);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erro ao enviar email: " + ex.Message);
                        }
                    }

                    MessageBox.Show("Relatório enviado com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                dataGridViewControlProducts.Rows.Clear();
                dataGridViewTotalProducts.Rows.Clear();
                epcsAdicionados.Clear();
                produtosAdicionados.Clear();
                lblTotal.Text = "Total: 0";

                StopListening();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar os EPCs: {ex.Message}");
                MessageBox.Show("Ocorreu um erro durante a busca.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnFinalizar.Enabled = true;
            }
        }

        private void EnviarEmailComAnexo(byte[] pdfContent)
        {
            string emailListRaw = ConfigurationManager.AppSettings["EmailList"];
            if (string.IsNullOrEmpty(emailListRaw))
            {
                MessageBox.Show("Nenhum email configurado no arquivo de configurações");
                return;
            }

            string[] emailArray = emailListRaw.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            List<string> destinatarios = new List<string>();
            foreach (var email in emailArray)
            {
                destinatarios.Add(email.Trim());
            }

            string smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
            int smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
            string remetente = ConfigurationManager.AppSettings["SenderEmail"];
            string senha = ConfigurationManager.AppSettings["SenderPassword"];

            using (var mail = new MailMessage())
            {
                mail.From = new MailAddress(remetente);

                foreach (var dest in destinatarios)
                {
                    mail.To.Add(dest.Trim());
                }

                mail.Subject = "Relatório";
                mail.Body = "Segue em anexo o relatório.";

                using (var pdfStream = new MemoryStream(pdfContent))
                {
                    string dataAtual = DateTime.Now.ToString();
                    string archiveName = $"Relatorio-{dataAtual}.pdf";
                    var attachment = new Attachment(pdfStream, archiveName, "application/pdf");
                    mail.Attachments.Add(attachment);

                    using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
                    {
                        smtpClient.Credentials = new System.Net.NetworkCredential(remetente, senha);
                        smtpClient.EnableSsl = true;
                        smtpClient.Send(mail);
                    }
                }
            }
        }

        public void StopListening()
        {
            if (isListening)
            {
                tcpReader.StopListening();
                btnIniciar.Text = "Iniciar";
                btnIniciar.BackColor = System.Drawing.Color.Lime;
                isListening = false;
            }
        }

        private async void HandleEpcReceived(string message)
        {
            try
            {
                var lines = message.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var line in lines)
                {
                    if (line.Contains("*")) continue;

                    var fields = line.Split(',');
                    
                    if (fields.Length < 3) continue;

                    string epc = fields[1].Trim();

                    if (!string.IsNullOrEmpty(epcStartFilter) &&
                        !epc.StartsWith(epcStartFilter, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if (!epcsAdicionados.TryAdd(epc, 0))
                    {
                        continue;
                    }

                    var product = await GetProductByEpc(epc);

                    string status = product != null
                        ? await GetLastAccessControlByProductId(product.Id)
                        : ("Desconhecido");

                    dataGridViewControlProducts.Invoke((MethodInvoker)(() =>
                    {
                        if (dataGridViewControlProducts.Columns.Count == 0)
                        {
                            dataGridViewControlProducts.Columns.Add("EPC", "EPC");
                            dataGridViewControlProducts.Columns.Add("Description", "Descrição");
                            dataGridViewControlProducts.Columns.Add("SKU", "SKU");
                            dataGridViewControlProducts.Columns.Add("Status", "Status");
                        }

                        dataGridViewControlProducts.Rows.Add(
                            product?.EPC ?? epc,
                            product?.Description ?? "Desconhecido",
                            product?.SKU ?? "Desconhecido",
                            status
                        );

                        lblTotal.Text = "Total: " + dataGridViewControlProducts.Rows.Count;

                        var produtoKey = (
                            (product?.Description ?? "Desconhecido").ToLowerInvariant(),
                            (product?.SKU ?? "Desconhecido").ToLowerInvariant()
                        );

                        bool added = produtosAdicionados.TryAdd(produtoKey, 0);

                        if (!added)
                        {
                            foreach (DataGridViewRow row in dataGridViewTotalProducts.Rows)
                            {
                                if (string.Equals(row.Cells[0].Value?.ToString(), produtoKey.Item1, StringComparison.OrdinalIgnoreCase) &&
                                    string.Equals(row.Cells[1].Value?.ToString(), produtoKey.Item2, StringComparison.OrdinalIgnoreCase))
                                {
                                    row.Cells[2].Value = (Convert.ToInt32(row.Cells[2].Value) + 1).ToString();
                                    break;
                                }
                            }
                        }
                        else
                        {
                            dataGridViewTotalProducts.Rows.Add(
                                product?.Description ?? "Desconhecido",
                                product?.SKU ?? "Desconhecido",
                                1
                            );
                        }
                    }));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar a mensagem: {ex.Message}");
            }
        }

        private async Task<Product> GetProductByEpc(string epc)
        {
            try
            {
                string apiUrl = $"http://{apiAddress}:{apiPort}/api/product/getByEpc/{epc}";

                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Product>(jsonResponse);
                }
                else
                {
                    Console.WriteLine($"Erro {response.StatusCode} ao buscar EPC: {epc}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exceção ao buscar EPC: {epc} | Erro: {ex.Message}");
            }

            return null;
        }

        private async Task<string> GetLastAccessControlByProductId(int productId)
        {
            try
            {
                string apiUrl = $"http://{apiAddress}:{apiPort}/api/accessControl/getLastAccess/{productId}";

                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    string direction = JsonConvert.DeserializeObject<AccessControl>(jsonResponse).Direction;

                    if (direction.Trim().ToUpper() == "OUT")
                    {
                        return "Enviado";
                    }
                    else if (direction.Trim().ToUpper() == "IN")
                    {
                        return "Recebido";
                    } else
                    {
                        return "Desconhecido";
                    }
                }
                else
                {
                    Console.WriteLine($"Erro na requisição: {response.StatusCode}");
                    return "Desconhecido";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao chamar o endpoint: {ex.Message}");
                return "Desconhecido";
            }
        }

        private async Task<Product> AddProduct(string description, string sku, string epc, string createdBy)
        {
            try
            {
                string apiUrl = $"http://{apiAddress}:{apiPort}/api/product/add";

                var productDTO = new
                {
                    Description = description,
                    SKU = sku,
                    EPC = epc,
                    CreatedBy = createdBy
                };

                var jsonContent = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(productDTO),
                    Encoding.UTF8,
                    "application/json"
                );

                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Product>(jsonResponse);
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

        private async Task<AccessControl> AddAccessControl(string direction, int productId, DateTime accessedOn, string status)
        {
            try
            {
                string apiUrl = $"http://{apiAddress}:{apiPort}/api/accessControl/add";

                var accessControlDTO = new
                {
                    Direction = direction,
                    ProductId = productId,
                    AccessedOn = accessedOn,
                    Status = status
                };

                var jsonContent = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(accessControlDTO),
                    Encoding.UTF8,
                    "application/json"
                );

                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<AccessControl>(jsonResponse);
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

        public void UnsubscribeFromReader()
        {
            this.tcpReader.OnEpcReceived -= HandleEpcReceived;
        }
    }
}
