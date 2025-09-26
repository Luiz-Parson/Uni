using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ConnectorAccess
{
    public partial class FrmSystemUsers : Form
    {
        protected static readonly Logger Logger = new Logger();
        DataTable dt = new DataTable();
        bool newRegister;
        int? rowIndexGdvSelected = null;

        public FrmSystemUsers()
        {
            InitializeComponent();
        }

        private void FrmSystemUsers_Load(object sender, EventArgs e)
        {
            ClearFieldsForm();
            GetSystemUsers(); //populate gridview
        }

        private void ClearFieldsForm()
        {
            pnlForm.Visible = false;
            tsbNew.Enabled = true;
            tsbSave.Enabled = false;
            tsbCancel.Enabled = false;
            tsbDelete.Enabled = false;
            tsTxtSearch.Enabled = true;
            tsbSearch.Enabled = true;
            txtId.Text = "";
            txtUsername.Text = "";
            chkIsAdmin.Checked = false;
            txtPassword.Text = "";
            txtConfirmPwd.Text = "";
            tsTxtSearch.Text = "";
            rowIndexGdvSelected = null;
        }

        private void GetSystemUsers(bool hasfilter = false)
        {
            try
            {
                dtvResults.Columns.Clear();

                if (hasfilter)
                    dt = SystemUser.GetSearchByFilter(tsTxtSearch.Text.Trim());
                else
                    dt = SystemUser.GetAll();

                if (dt.Rows.Count == 0)
                {
                    #region header with no record
                    dt = new DataTable();
                    dt.Columns.Add("Resultado", typeof(string));
                    dt.Rows.Add("Nenhum item foi encontrado.");
                    dtvResults.DataSource = dt;
                    dtvResults.Columns[0].Width = 200;
                    #endregion
                }
                else
                {
                    dtvResults.DataSource = dt;
                    ConfigGridResults();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error("Erro GetSystemUsers", ex);
            }
        }

        private void ConfigGridResults()
        {
            dtvResults.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);
            dtvResults.DefaultCellStyle.Font = new Font("Arial", 9);
            dtvResults.RowHeadersWidth = 25;
            dtvResults.DefaultCellStyle.SelectionBackColor = Color.White;
            dtvResults.DefaultCellStyle.SelectionForeColor = Color.Black;

            dtvResults.Columns["Id"].HeaderText = "Id";
            dtvResults.Columns["Id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtvResults.Columns["Id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dtvResults.Columns["Username"].HeaderText = "Usuário";
            dtvResults.Columns["Username"].Width = 390;
            dtvResults.Columns["Username"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dtvResults.Columns["IsAdmin"].HeaderText = "Administrador";
            dtvResults.Columns["IsAdmin"].Width = 200;
            dtvResults.Columns["IsAdmin"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

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

        private void tsbNew_Click(object sender, EventArgs e)
        {
            pnlForm.Visible = true;
            tsbNew.Enabled = false;
            tsbSave.Enabled = true;
            tsbCancel.Enabled = true;
            tsbDelete.Enabled = false;
            tsTxtSearch.Enabled = false;
            tsbSearch.Enabled = false;
            txtUsername.Focus();
            txtUsername.Enabled = true;
            newRegister = true;
        }

        private void tsbCancel_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ClearFieldsForm();
            GetSystemUsers();
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            #region Validar obrigatoriedade dos campos do form
            if (String.IsNullOrEmpty(txtUsername.Text.Trim()))
            {
                MessageBox.Show("Por favor, preencha o campo Usuário.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (String.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                MessageBox.Show("Por favor, preencha o campo Senha.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtPassword.Text.Trim().Length < 4)
            {
                MessageBox.Show("O campo Senha deve ter ao menos 4 caracteres.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtPassword.Text.Trim() != txtConfirmPwd.Text.Trim())
            {
                MessageBox.Show("O campo Confirmar senha está diferente do campo Senha, por favor verifique.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            #endregion

            try
            {
                var systemUser = new SystemUser();
                systemUser.Id = (txtId.Text == "" ? 0 : Convert.ToInt32(txtId.Text));
                systemUser.Username = txtUsername.Text.Trim();
                systemUser.IsAdmin = chkIsAdmin.Checked;
                systemUser.Password = txtPassword.Text.Trim();
                if (newRegister)
                {
                    var isSuccess = SystemUser.Insert(systemUser);
                    if (isSuccess)
                        MessageBox.Show("Cadastro realizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Ocorreu um erro ao inserir este item.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    var isSuccess = SystemUser.Update(systemUser);
                    if (isSuccess)
                        MessageBox.Show("Cadastro atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                        MessageBox.Show("Ocorreu um erro ao atualizar este item.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("unique key"))
                {
                    MessageBox.Show("Usuário já utilizado em outro registro. Por favor, verifique e tente novamente.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Logger.Error("Erro ao clicar no BtnSave FrmSystemUsers", ex);
            }

            ClearFieldsForm();
            GetSystemUsers();
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DialogResult confirmResult = MessageBox.Show("Tem certeza que deseja excluir?", "Confirme a exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    var isSuccess = SystemUser.DeleteById(Convert.ToInt32(txtId.Text));
                    if (isSuccess)
                    {
                        MessageBox.Show("Registro excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (rowIndexGdvSelected != null)
                            dtvResults.Rows.RemoveAt(rowIndexGdvSelected.Value);
                    }
                    else
                        MessageBox.Show("Ocorreu um erro ao excluir este item.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger.Error("Erro ao Delete SystemUser id: " + txtId.Text, ex);
                }
            }

            ClearFieldsForm();
        }

        private void tsbSearch_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (String.IsNullOrEmpty(tsTxtSearch.Text.Trim()))
                GetSystemUsers();
            else
                GetSystemUsers(true);
        }

        private void dtvResults_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                var colName = dtvResults.Columns[e.ColumnIndex].Name;
                if (colName == "Edit")
                {
                    rowIndexGdvSelected = e.RowIndex;
                    var idSystemUser = dtvResults["Id", e.RowIndex].Value;
                    var result = SystemUser.GetById(Convert.ToInt32(idSystemUser));

                    if (result != null)
                    {
                        pnlForm.Visible = true;
                        tsbNew.Enabled = false;
                        tsbSave.Enabled = true;
                        tsbCancel.Enabled = true;
                        tsbDelete.Enabled = true;
                        tsTxtSearch.Enabled = false;
                        tsbSearch.Enabled = false;
                        txtUsername.Enabled = false;
                        txtPassword.Focus();
                        txtId.Text = result.Id.ToString();
                        txtUsername.Text = result.Username;
                        chkIsAdmin.Checked = result.IsAdmin;
                        txtPassword.Text = result.Password;
                        txtConfirmPwd.Text = result.Password;
                        newRegister = false;
                    }
                    else
                    {
                        MessageBox.Show("Item não encontrado na base de dados. (Id: " + idSystemUser + ").", "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                }
                if (colName == "Delete")
                {
                    DialogResult confirmResult = MessageBox.Show("Tem certeza que deseja excluir?", "Confirme a exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                    if (confirmResult == DialogResult.Yes)
                    {
                        var idSystemUser = dtvResults["Id", e.RowIndex].Value;
                        var isSuccess = SystemUser.DeleteById(Convert.ToInt32(idSystemUser));
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
                Logger.Error("Erro ao clicar no gridview FrmSystemUsers", ex);
            }
        }

        private void tsTxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                tsbSearch.PerformClick();
        }
    }
}
