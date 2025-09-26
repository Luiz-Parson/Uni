using ConnectorAccess.models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectorAccess
{
    public partial class FrmProducts : Form
    {
        protected static readonly Logger Logger = new Logger();
        DataTable dt = new DataTable();
        int idProduct = 0;
        bool newRegister;
        int? rowIndexGdvSelected = null;
        private string expandedSku = null;
        private string expandedDescription = null;
        private List<Product> allProducts = new List<Product>();
        private Dictionary<string, string> skuToDescription = new Dictionary<string, string>();
        private Dictionary<string, string> descriptionToSku = new Dictionary<string, string>();

        string apiAddress = ConfigurationManager.AppSettings["APIAddress"];
        string apiPort = ConfigurationManager.AppSettings["APIPort"];

        private static readonly HttpClient httpClient = new HttpClient();

        public FrmProducts()
        {
            InitializeComponent();
        }

        private async void FrmPrinters_LoadAsync(object sender, EventArgs e)
        {
            ClearFieldsForm();
            await GetProducts();
            await LoadSKUsAndDescriptionsAsync();

            comboBoxSKU.SelectedIndexChanged += comboBoxSKU_SelectedIndexChanged;
            comboBoxDescription.SelectedIndexChanged += comboBoxDescription_SelectedIndexChanged;

            comboBoxSKU.TextChanged += comboBoxSKU_TextChanged;
            comboBoxDescription.TextChanged += comboBoxDescription_TextChanged;
        }

        private void ClearFieldsForm()
        {
            pnlForm.Visible = false;
            tsbNew.Enabled = true;
            tsbSave.Enabled = false;
            tsbCancel.Enabled = false;
            comboBoxDescription.Text = "";
            comboBoxSKU.Text = "";
            txtEpc.Text = "";
            rowIndexGdvSelected = null;
        }

        private DataTable ConvertToDataTable(List<Product> products)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("SKU", typeof(string));
            dt.Columns.Add("EPC", typeof(string));
            dt.Columns.Add("Quantidade", typeof(int));
            dt.Columns.Add("UpdatedBy", typeof(string));
            dt.Columns.Add("UpdatedOn", typeof(DateTime));

            var groupedProducts = products.GroupBy(p => new { p.SKU, p.Description })
                                          .Select(g => new
                                          {
                                              Id = g.First().Id,
                                              Description = g.Key.Description,
                                              SKU = g.Key.SKU,
                                              EPCs = string.Join(", ", g.Select(p => p.EPC)),
                                              Quantidade = g.Count(),
                                              UpdatedBy = g.First().UpdatedBy,
                                              UpdatedOn = g.First().UpdatedOn
                                          }).ToList();

            foreach (var product in groupedProducts)
            {
                dt.Rows.Add(product.Id, product.Description, product.SKU, product.EPCs, product.Quantidade, product.UpdatedBy, product.UpdatedOn);
            }

            return dt;
        }

        private DataTable GenerateReportData()
        {
            DataTable dtRep = new DataTable();

            dtRep.Columns.Add("Description");
            dtRep.Columns.Add("SKU");
            dtRep.Columns.Add("Quantidade");

            foreach (DataGridViewRow item in dtvResults.Rows)
            {
                dtRep.Rows.Add(
                    item.Cells["Description"].Value.ToString(),
                    item.Cells["SKU"].Value.ToString(),
                    item.Cells["Quantidade"].Value);
            }

            return dtRep;
        }

        private void ConfigGridResults(bool isExpanded = false)
        {
            dtvResults.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);
            dtvResults.DefaultCellStyle.Font = new Font("Arial", 9);
            dtvResults.RowHeadersWidth = 25;
            dtvResults.DefaultCellStyle.SelectionBackColor = Color.White;
            dtvResults.DefaultCellStyle.SelectionForeColor = Color.Black;

            dtvResults.Columns["Id"].HeaderText = "Id";
            dtvResults.Columns["Id"].Width = 30;
            dtvResults.Columns["Id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtvResults.Columns["Id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dtvResults.Columns["Description"].HeaderText = "Descrição";
            if (isExpanded)
            {
                dtvResults.Columns["Description"].Width = 170;
            } else
            {
                dtvResults.Columns["Description"].Width = 250;
            }
            dtvResults.Columns["Description"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtvResults.Columns["Description"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dtvResults.Columns["SKU"].HeaderText = "SKU";
            if (isExpanded)
            {
                dtvResults.Columns["SKU"].Width = 70;
            } else
            {
                dtvResults.Columns["SKU"].Width = 120;
            }
            dtvResults.Columns["SKU"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtvResults.Columns["SKU"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            if (isExpanded && dtvResults.Columns["EPC"] != null)
            {
                dtvResults.Columns["EPC"].HeaderText = "EPC";
                dtvResults.Columns["EPC"].Width = 200;
                dtvResults.Columns["EPC"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtvResults.Columns["EPC"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtvResults.Columns["EPC"].Visible = true;
            }
            else if (!isExpanded && dtvResults.Columns["EPC"] != null)
            {
                dtvResults.Columns["EPC"].Visible = false;
            }

            if (!isExpanded && dtvResults.Columns["Quantidade"] != null)
            {
                dtvResults.Columns["Quantidade"].HeaderText = "Quantidade";
                dtvResults.Columns["Quantidade"].Width = 80;
                dtvResults.Columns["Quantidade"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtvResults.Columns["Quantidade"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtvResults.Columns["Quantidade"].Visible = true;
            }
            else if (isExpanded && dtvResults.Columns["Quantidade"] != null)
            {
                dtvResults.Columns["Quantidade"].Visible = false;
            }

            dtvResults.Columns["UpdatedBy"].HeaderText = "Modificado";
            dtvResults.Columns["UpdatedBy"].Width = 80;
            dtvResults.Columns["UpdatedBy"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtvResults.Columns["UpdatedBy"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dtvResults.Columns["UpdatedOn"].HeaderText = "Data da modificação";
            dtvResults.Columns["UpdatedOn"].Width = 120;
            dtvResults.Columns["UpdatedOn"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtvResults.Columns["UpdatedOn"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            if (!isExpanded && dtvResults.Columns["Quantidade"] != null)
            {
                dtvResults.Columns["Quantidade"].HeaderText = "Quantidade";
                dtvResults.Columns["Quantidade"].Width = 80;
                dtvResults.Columns["Quantidade"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtvResults.Columns["Quantidade"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtvResults.Columns["Quantidade"].Visible = true;
            }
            else if (isExpanded && dtvResults.Columns["Quantidade"] != null)
            {
                dtvResults.Columns["Quantidade"].Visible = false;
            }

            if (!isExpanded && dtvResults.Columns["Expand"] == null)
            {
                DataGridViewButtonColumn btnExpand = new DataGridViewButtonColumn();
                btnExpand.HeaderText = "";
                btnExpand.Text = "Visualizar";
                btnExpand.Name = "Expand";
                btnExpand.UseColumnTextForButtonValue = true;
                btnExpand.Width = 70;
                dtvResults.Columns.Add(btnExpand);
            }
            else if (isExpanded && dtvResults.Columns["Expand"] != null)
            {
                dtvResults.Columns["Expand"].Visible = false;
            }

            if (isExpanded && dtvResults.Columns["Edit"] == null && dtvResults.Columns["Delete"] == null)
            {
                DataGridViewImageColumn imageColumnEdit = new DataGridViewImageColumn();
                imageColumnEdit.HeaderText = "";
                imageColumnEdit.ToolTipText = "Editar";
                imageColumnEdit.Image = Properties.Resources.edit_icon_grid_16;
                imageColumnEdit.Name = "Edit";
                imageColumnEdit.Width = 30;
                dtvResults.Columns.Add(imageColumnEdit);

                DataGridViewImageColumn imageColumnDelete = new DataGridViewImageColumn();
                imageColumnDelete.HeaderText = "";
                imageColumnDelete.ToolTipText = "Excluir";
                imageColumnDelete.Image = Properties.Resources.trash_icon;
                imageColumnDelete.Name = "Delete";
                imageColumnDelete.Width = 30;
                dtvResults.Columns.Add(imageColumnDelete);
            }
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            pnlForm.Visible = true;
            tsbNew.Enabled = false;
            tsbSave.Enabled = true;
            tsbCancel.Enabled = true;
            newRegister = true;
        }

        private async void tsbCancel_ClickAsync(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ClearFieldsForm();
            await GetProducts();
        }

        private async void tsbSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string selectedSKU = comboBoxSKU?.Text.Trim();
            string selectedDescription = comboBoxDescription?.Text.Trim();

            #region Validar obrigatoriedade dos campos do form
            if (String.IsNullOrEmpty(selectedDescription))
            {
                MessageBox.Show("Por favor, preencha o campo Descrição.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (String.IsNullOrEmpty(txtEpc.Text.Trim()))
            {
                MessageBox.Show("Por favor, preencha o campo EPC.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtEpc.Text.Trim().Length != 24)
            {
                MessageBox.Show("O EPC deve ter exatamente 24 caracteres.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (String.IsNullOrEmpty(selectedSKU))
            {
                MessageBox.Show("Por favor, preencha o campo SKU.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            #endregion

            try
            {
                var existingProduct = await GetProductByEpc(txtEpc.Text.Trim());

                if (newRegister)
                {
                    if (existingProduct != null)
                    {
                        MessageBox.Show("Já existe um produto registrado com este EPC.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var product = await AddProduct(selectedDescription, selectedSKU, txtEpc.Text.Trim(), Program.systemUserLogged.Username);

                    if (product != null)
                    {
                        MessageBox.Show("Cadastro realizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Ocorreu um erro ao inserir este item!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    var product = await UpdateProductById(idProduct, selectedDescription, selectedSKU, txtEpc.Text.Trim(), Program.systemUserLogged.Username);

                    if (product != null)
                    {
                        MessageBox.Show("Atualização realizada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Ocorreu um erro ao atualizar este item!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error("Erro BtnSave Frm Products", ex);
            }

            ClearFieldsForm();
            await LoadSKUsAndDescriptionsAsync();
            await GetProducts();
        }

        private async void dtvResults_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                var colName = dtvResults.Columns[e.ColumnIndex].Name;

                if (colName == "Expand")
                {
                    string sku = dtvResults.Rows[e.RowIndex].Cells["SKU"].Value.ToString();
                    string description = dtvResults.Rows[e.RowIndex].Cells["Description"].Value.ToString();
                    string buttonText = dtvResults.Rows[e.RowIndex].Cells["Expand"].Value?.ToString() ?? "Visualizar";

                    if (buttonText == "Visualizar")
                    {
                        expandedSku = sku;
                        expandedDescription = description;
                        ExpandDetails(sku, description);
                        tsbCancel.Enabled = true;
                    }
                    else
                    {
                        expandedSku = null;
                        expandedDescription = null;
                        CollapseDetails();
                    }

                    RestoreExpandCollapseButtons();
                }

                if (colName == "Edit")
                {
                    rowIndexGdvSelected = e.RowIndex;
                    var epc = dtvResults["EPC", e.RowIndex].Value;
                    var result = await GetProductByEpc(epc.ToString());

                    if (result != null)
                    {
                        pnlForm.Visible = true;
                        tsbNew.Enabled = false;
                        tsbSave.Enabled = true;
                        tsbCancel.Enabled = true;

                        if (!comboBoxDescription.Items.Contains(result.Description))
                        {
                            comboBoxDescription.Items.Add(result.Description);
                        }
                        comboBoxDescription.Text = result.Description;

                        txtEpc.Text = result.EPC;

                        if (!comboBoxSKU.Items.Contains(result.SKU))
                        {
                            comboBoxSKU.Items.Add(result.SKU);
                        }
                        comboBoxSKU.Text = result.SKU;

                        comboBoxSKU.Text = result.SKU;
                        idProduct = result.Id;
                        newRegister = false;
                    }
                    else
                    {
                        MessageBox.Show("Item não encontrado na base de dados. (Id: " + idProduct + ").", "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                }
                if (colName == "Delete")
                {
                    DialogResult confirmResult = MessageBox.Show("Tem certeza que deseja excluir?", "Confirme a exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                    if (confirmResult == DialogResult.Yes)
                    {
                        var idProduct = dtvResults["Id", e.RowIndex].Value;
                        var isSuccess = await DeleteById(Convert.ToInt32(idProduct), Program.systemUserLogged.Username);

                        if (isSuccess)
                        {
                            MessageBox.Show("Registro excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFieldsForm();
                            dtvResults.Rows.RemoveAt(e.RowIndex);
                        }
                        else
                            MessageBox.Show("Ocorreu um erro ao excluir este item.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error("Erro ao clicar no gridview FrmPrinters", ex);
            }
        }

        private void ExpandDetails(string sku, string description)
        {
            var filteredProducts = allProducts.Where(p => p.SKU == sku && p.Description == description).ToList();

            if (filteredProducts.Count > 0)
            {
                DataTable dtExpanded = new DataTable();
                dtExpanded.Columns.Add("Id", typeof(int));
                dtExpanded.Columns.Add("Description", typeof(string));
                dtExpanded.Columns.Add("SKU", typeof(string));
                dtExpanded.Columns.Add("EPC", typeof(string));
                dtExpanded.Columns.Add("UpdatedBy", typeof(string));
                dtExpanded.Columns.Add("UpdatedOn", typeof(DateTime));

                foreach (var product in filteredProducts)
                {
                    dtExpanded.Rows.Add(product.Id, product.Description, product.SKU, product.EPC, product.UpdatedBy, product.UpdatedOn);
                }

                dtvResults.DataSource = dtExpanded;
                ConfigGridResults(true);
                RestoreExpandCollapseButtons();
            }
        }

        private void RestoreExpandCollapseButtons()
        {
            if (dtvResults.Columns["Expand"] == null)
            {
                DataGridViewButtonColumn btnExpand = new DataGridViewButtonColumn();
                btnExpand.HeaderText = "";
                btnExpand.Text = "Visualizar";
                btnExpand.Name = "Expand";
                btnExpand.UseColumnTextForButtonValue = true;
                dtvResults.Columns.Add(btnExpand);
            }

            foreach (DataGridViewRow row in dtvResults.Rows)
            {
                if (row.Cells["SKU"].Value != null && row.Cells["Description"].Value != null)
                {
                    row.Cells["Expand"].Value = "Visualizar";
                }
            }
        }

        private void CollapseDetails()
        {
            dtvResults.DataSource = ConvertToDataTable(allProducts);
            ConfigGridResults(false);
            RestoreExpandCollapseButtons();
        }

        private async Task GetProducts(bool hasfilter = false)
        {
            try
            {
                dtvResults.Columns.Clear();
                string apiUrl = $"http://{apiAddress}:{apiPort}/api/product/getAll";

                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    allProducts = JsonConvert.DeserializeObject<List<Product>>(jsonResponse) ?? new List<Product>();

                    if (allProducts.Count == 0)
                    {
                        dt = new DataTable();
                        dt.Columns.Add("Resultado", typeof(string));
                        dt.Rows.Add("Nenhum item foi encontrado.");
                        dtvResults.DataSource = dt;
                        dtvResults.Columns[0].Width = 200;
                    }
                    else
                    {
                        dt = ConvertToDataTable(allProducts);
                        dtvResults.DataSource = dt;
                        ConfigGridResults();
                    }
                }
                else
                {
                    Console.WriteLine($"Erro na requisição: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error("Erro GetProducts FrmProducts", ex);
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

        private async Task<Product> UpdateProductById(int id, string description, string sku, string epc, string updatedBy)
        {
            try
            {
                string apiUrl = $"http://{apiAddress}:{apiPort}/api/product/update/{id}";

                var productUpdateDTO = new
                {
                    Description = description,
                    SKU = sku,
                    EPC = epc,
                    UpdatedBy = updatedBy
                };

                var jsonContent = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(productUpdateDTO),
                    Encoding.UTF8,
                    "application/json"
                );

                HttpResponseMessage response = await httpClient.PutAsync(apiUrl, jsonContent);

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

        private async Task<bool> DeleteById(int id, string deletedBy)
        {
            try
            {
                string apiUrl = $"http://{apiAddress}:{apiPort}/api/product/delete/{id}?deletedBy={deletedBy}";

                HttpResponseMessage response = await httpClient.DeleteAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Produto {id} excluído com sucesso.");
                    return true;
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Erro ao excluir o produto {id}: {errorMessage}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exceção ao tentar excluir o produto {id}: {ex.Message}");
                return false;
            }
        }

        private async Task LoadSKUsAndDescriptionsAsync()
        {
            try
            {
                string apiUrl = $"http://{apiAddress}:{apiPort}/api/product/getAllSKUsAndDescriptions";

                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var skusAndDescriptions = JsonConvert.DeserializeObject<List<SKUAndDescriptionDTO>>(jsonResponse);

                    comboBoxSKU.Items.Clear();
                    comboBoxDescription.Items.Clear();
                    skuToDescription.Clear();
                    descriptionToSku.Clear();

                    foreach (var item in skusAndDescriptions)
                    {
                        skuToDescription[item.SKU] = item.Description;
                        descriptionToSku[item.Description] = item.SKU;

                        comboBoxSKU.Items.Add(item.SKU);
                        comboBoxDescription.Items.Add(item.Description);
                    }
                }
                else
                {
                    Console.WriteLine($"Erro na requisição: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao chamar o endpoint: {ex.Message}");
            }
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
            using (var frmReportPrint = new FrmProductsPrint(dataReport))
            {
                frmReportPrint.ShowDialog();
            }
        }

        private void comboBoxSKU_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedSKU = comboBoxSKU.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedSKU) && skuToDescription.ContainsKey(selectedSKU))
            {
                string description = skuToDescription[selectedSKU];
                comboBoxDescription.SelectedItem = description;
            }
        }

        private void comboBoxSKU_TextChanged(object sender, EventArgs e)
        {
            string typedSKU = comboBoxSKU.Text;

            if (!string.IsNullOrEmpty(typedSKU) && skuToDescription.ContainsKey(typedSKU))
            {
                comboBoxDescription.Text = skuToDescription[typedSKU];
            }
        }

        private void comboBoxDescription_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedDescription = comboBoxDescription.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedDescription) && descriptionToSku.ContainsKey(selectedDescription))
            {
                string sku = descriptionToSku[selectedDescription];
                comboBoxSKU.SelectedItem = sku;
            }
        }
        private void comboBoxDescription_TextChanged(object sender, EventArgs e)
        {
            string typedDescription = comboBoxDescription.Text;

            if (!string.IsNullOrEmpty(typedDescription) && descriptionToSku.ContainsKey(typedDescription))
            {
                comboBoxSKU.Text = descriptionToSku[typedDescription];
            }
        }
    }
}
