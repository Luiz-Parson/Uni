using System;
using System.Configuration;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

//referencia para o side menu: https://www.youtube.com/watch?v=8Mxaky5YazQ

namespace ConnectorAccess
{
    public partial class FrmMain : Form
    {
        private Form frmActive;
        private TcpReader tcpReader;
        string readerAddress = ConfigurationManager.AppSettings["ReaderAddress"];
        int readerPort = Convert.ToInt32(ConfigurationManager.AppSettings["ReaderPort"]);
        private bool testMode => bool.TryParse(ConfigurationManager.AppSettings["TestMode"], out bool result) && result;

        public FrmMain()
        {
            InitializeComponent();
            tcpReader = new TcpReader(readerAddress, readerPort);
        }

        private async void FrmMain_Load(object sender, EventArgs e)
        {
            this.Enabled = false;

            try
            {
                if (!testMode)
                {
                    await tcpReader.ConnectAsync();
                }
            }
            catch (Exception)
            {
                MessageBox.Show($"Sem conexão com o leitor. Por favor, saia do Connector e providencie a reinicialização do leitor. Depois de 1 minuto entre novamente.",
                    "Erro na Conexão",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }
            finally
            {
                this.Enabled = true;
            }

            if (!Program.systemUserLogged.IsAdmin)
                btnSystemUsers.Visible = false;

            ActiveButton(btnMovementControl);
            FormShow(new FrmMovementControl(tcpReader));
        }

        private void FormShow(Form frm)
        {
            ActiveFormClose();
            frmActive = frm;
            frm.TopLevel = false;
            pnlForm.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void ActiveFormClose()
        {
            if (frmActive != null)
            {
                if (tcpReader.IsListening())
                {
                    tcpReader.StopListening();
                }

                if (frmActive is FrmGeneralAssociations frmGeneralAssociations) frmGeneralAssociations.UnsubscribeFromReader();
                else if (frmActive is FrmMovementControl frmMovementControl) frmMovementControl.UnsubscribeFromReader();
                else if (frmActive is FrmExclusionAndUpdate frmExclusionAndUpdate) frmExclusionAndUpdate.UnsubscribeFromReader();
                else if (frmActive is FrmAssociationsBySKU frmAssociationsBySKU) frmAssociationsBySKU.UnsubscribeFromReader();
                else if (frmActive is FrmExclusion frmExclusion) frmExclusion.UnsubscribeFromReader();

                frmActive.Dispose();
                frmActive.Close();
            }
        }

        private void ActiveButton(Button frmActive)
        {
            foreach (Control ctrl in pnlMain.Controls)
            {
                if (ctrl.GetType().Name != "Panel")
                    ctrl.BackColor = Color.Gainsboro;
            }
            foreach (Control ctrl in pnlSubMenuReport.Controls)
            {
                if (ctrl.GetType().Name != "Panel")
                    ctrl.BackColor = Color.Gainsboro;
            }
            foreach (Control ctrl in pnlSubMenuAssociation.Controls)
            {
                if (ctrl.GetType().Name != "Panel")
                    ctrl.BackColor = Color.Gainsboro;
            }
            foreach (Control ctrl in pnlSubMenuExclusion.Controls)
            {
                if (ctrl.GetType().Name != "Panel")
                    ctrl.BackColor = Color.Gainsboro;
            }
            frmActive.BackColor = Color.DarkGray;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Program.systemUserLogged = null;
            this.Close();

            frmLogin loginForm = new frmLogin();
            loginForm.Show();

            tcpReader.Disconnect();
        }

        private void btnSystemUsers_Click(object sender, EventArgs e)
        {
            ActiveButton(btnSystemUsers);
            FormShow(new FrmSystemUsers());
        }

        private void btnMovementControl_Click(object sender, EventArgs e)
        {
            ActiveButton(btnMovementControl);

            if (frmActive is FrmMovementControl MovementControlForm)
            {
                MovementControlForm.StopListening();
            }

            FormShow(new FrmMovementControl(tcpReader));
        }

        private void btnReportDay_Click(object sender, EventArgs e)
        {
            ActiveButton(btnReportDay);
            FormShow(new FrmReportDay());
        }

        private void btnReportExclusion_Click(object sender, EventArgs e)
        {
            ActiveButton(btnReportExclusion);
            FormShow(new FrmReportExclusion());
        }

        private void btnReportStock_Click(object sender, EventArgs e)
        {
            ActiveButton(btnReportStock);
            FormShow(new FrmReportStock());
        }

        private void btnReportValidity_Click(object sender, EventArgs e)
        {
            ActiveButton(btnReportValidity);
            FormShow(new FrmReportValidity());
        }

        private void btnPrinters_Click(object sender, EventArgs e)
        {
            ActiveButton(btnItens);
            FormShow(new FrmProducts());
        }

        private void btnGeneralAssociations_Click(object sender, EventArgs e)
        {
            ActiveButton(btnGeneralAssociations);
            FormShow(new FrmGeneralAssociations(tcpReader));
        }

        private void btnExclusionAndUpdate_Click(object sender, EventArgs e)
        {
            ActiveButton(btnGeneralExclusion);
            FormShow(new FrmExclusionAndUpdate(tcpReader));
        }

        private void btnReportTitle_Click(object sender, EventArgs e)
        {
            pnlSubMenuReport.Visible = !pnlSubMenuReport.Visible;

            if (pnlSubMenuReport.Visible)
                btnReportTitle.Text = "Relatórios ▼";
            else
                btnReportTitle.Text = "Relatórios ▶";
        }

        private void btnAssociationType_Click(object sender, EventArgs e)
        {
            pnlSubMenuAssociation.Visible = !pnlSubMenuAssociation.Visible;

            if (pnlSubMenuAssociation.Visible)
                btnAssociationType.Text = "Cadastramento de itens ▼";
            else
                btnAssociationType.Text = "Cadastramento de itens ▶";
        }

        private void btnExclusionType_Click(object sender, EventArgs e)
        {
            pnlSubMenuExclusion.Visible = !pnlSubMenuExclusion.Visible;

            if (pnlSubMenuExclusion.Visible)
                btnExclusionType.Text = "Baixa de itens ▼";
            else
                btnExclusionType.Text = "Baixa de itens ▶";
        }

        private void btnAssociationsBySKU_Click(object sender, EventArgs e)
        {
            ActiveButton(btnAssociationsBySKU);
            FormShow(new FrmAssociationsBySKU(tcpReader));
        }

        private void btnExclusion_Click(object sender, EventArgs e)
        {
            ActiveButton(btnExclusion);
            FormShow(new FrmExclusion(tcpReader));
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            tcpReader.Dispose();
        }

    }
}
