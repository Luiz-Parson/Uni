using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Generic;
using ConnectorAccess.models;
using System.Text;

namespace ConnectorAccess
{
    public partial class FrmExclusion : Form
    {
        protected static readonly Logger Logger = new Logger();
        private TcpReader tcpReader;
        private bool isListening;
        private HashSet<string> epcsAdicionados = new HashSet<string>();
        string epcStartFilter = ConfigurationManager.AppSettings["EpcStartFilter"];
        string apiAddress = ConfigurationManager.AppSettings["APIAddress"];
        string apiPort = ConfigurationManager.AppSettings["APIPort"];
        private bool testMode => bool.TryParse(ConfigurationManager.AppSettings["TestMode"], out bool result) && result;

        private static readonly HttpClient httpClient = new HttpClient();

        public FrmExclusion(TcpReader tcpReader)
        {
            InitializeComponent();

            this.tcpReader = tcpReader;
            this.tcpReader.OnEpcReceived += HandleEpcReceived;
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            if (isListening)
            {
                // Parar leitura
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

        private async void btnDelete_ClickAsync(object sender, EventArgs e)
        {
            btnDelete.Enabled = false;

            try
            {
                var productIds = new List<int>();
                var exclusionList = new List<ExclusionControlDTO>();
                string category = comboBoxCategory.SelectedItem?.ToString();

                if (string.IsNullOrEmpty(category))
                {
                    MessageBox.Show("Por favor, selecione uma Categoria válida.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                foreach (DataGridViewRow row in dataGridViewRegisterProducts.Rows)
                {
                    string epc = row.Cells["EPC"].Value?.ToString();

                    if (!string.IsNullOrEmpty(epc))
                    {
                        var product = await GetProductByEpc(epc);

                        if (product != null)
                        {
                            productIds.Add(product.Id);

                            exclusionList.Add(new ExclusionControlDTO
                            {
                                ProductId = product.Id,
                                Category = category,
                                ExcludedOn = DateTime.Now
                            });
                        }
                    }
                }

                bool deleted = await DeleteMultipleProductsAsync(productIds, Program.systemUserLogged.Username);
                bool registered = await AddMultipleExclusionsAsync(exclusionList);

                if (deleted && registered)
                {
                    MessageBox.Show("Exclusões realizadas com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Erro em alguma das operações de exclusão ou registro.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                dataGridViewRegisterProducts.Rows.Clear();
                epcsAdicionados.Clear();
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
                btnDelete.Enabled = true;
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
                    if (line.Contains("*"))
                    {
                        Console.WriteLine("Marcador especial encontrado.");
                        continue;
                    }

                    var fields = line.Split(',');

                    if (fields.Length >= 3)
                    {
                        string epc = fields[1].Trim();

                        // Verifica se o EPC passa no filtro epcStartFilter
                        if (!string.IsNullOrEmpty(epcStartFilter) &&
                            !epc.StartsWith(epcStartFilter, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"EPC {epc} não inicia com {epcStartFilter}. Ignorando...");
                            continue;
                        }

                        if (epcsAdicionados.Contains(epc))
                        {
                            Console.WriteLine($"EPC {epc} já adicionado. Ignorando...");
                            continue;
                        }

                        epcsAdicionados.Add(epc);

                        var product = await GetProductByEpc(epc);

                        dataGridViewRegisterProducts.Invoke((MethodInvoker)(() =>
                        {
                            if (dataGridViewRegisterProducts.Columns.Count == 0)
                            {
                                dataGridViewRegisterProducts.Columns.Add("EPC", "EPC");
                                dataGridViewRegisterProducts.Columns.Add("Description", "Descrição");
                                dataGridViewRegisterProducts.Columns.Add("SKU", "SKU");
                                dataGridViewRegisterProducts.Columns.Add("Status", "Status");
                            }

                            dataGridViewRegisterProducts.Rows.Add(
                                product?.EPC ?? epc,
                                product?.Description ?? "Desconhecido",
                                product?.SKU ?? "Desconhecido"
                            );

                            lblTotal.Text = "Total: " + dataGridViewRegisterProducts.Rows.Count.ToString();
                        }));
                    }
                    else
                    {
                        Console.WriteLine($"Linha inesperada: {line}");
                    }
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

        private async Task<bool> DeleteMultipleProductsAsync(List<int> productIds, string deletedBy)
        {
            try
            {
                var deleteMultipleDTO = new DeleteMultipleDTO
                {
                    Ids = productIds,
                    DeletedBy = deletedBy
                };

                string apiUrl = $"http://{apiAddress}:{apiPort}/api/product/delete-multiple";
                var json = JsonConvert.SerializeObject(deleteMultipleDTO);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Produtos excluídos com sucesso.");
                    return true;
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Erro ao excluir múltiplos produtos: {error}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exceção ao excluir múltiplos produtos: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> AddMultipleExclusionsAsync(List<ExclusionControlDTO> exclusions)
        {
            try
            {
                string apiUrl = $"http://{apiAddress}:{apiPort}/api/exclusioncontrol/add-multiple";
                var json = JsonConvert.SerializeObject(exclusions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Exclusões registradas com sucesso.");
                    return true;
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Erro ao registrar exclusões: {error}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exceção ao registrar exclusões: {ex.Message}");
                return false;
            }
        }

        public void UnsubscribeFromReader()
        {
            this.tcpReader.OnEpcReceived -= HandleEpcReceived;
        }
    }
}
