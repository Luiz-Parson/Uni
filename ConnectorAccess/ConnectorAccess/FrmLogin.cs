using System;
using System.Windows.Forms;

namespace ConnectorAccess
{
    public partial class frmLogin : Form
    {
        protected static readonly Logger Logger = new Logger();
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (String.IsNullOrEmpty(txtUsername.Text.Trim()) || String.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                MessageBox.Show("Por favor, preencha os campos Usuário e Senha!", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                SystemUser resultLogin = SystemUser.Login(txtUsername.Text.Trim(), txtPassword.Text.Trim());
                if (resultLogin != null)
                {
                    Program.systemUserLogged = new SystemUser();
                    Program.systemUserLogged.Id = resultLogin.Id;
                    Program.systemUserLogged.Username = resultLogin.Username;
                    Program.systemUserLogged.IsAdmin = resultLogin.IsAdmin;
                    this.Hide();
                    using (FrmMain mainForm = new FrmMain())
                    {
                        mainForm.ShowDialog();
                    }
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Usuário ou Senha inválidos.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error("Erro Login", ex);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
        }

        private void frmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnLogin.PerformClick();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnLogin.PerformClick();
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnLogin.PerformClick();
        }
    }
}
