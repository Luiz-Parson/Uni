
namespace ConnectorAccess
{
    partial class FrmProducts
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmProducts));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.tsbNew = new System.Windows.Forms.ToolStripButton();
            this.tsbCancel = new System.Windows.Forms.ToolStripButton();
            this.tsbPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.pnlForm = new System.Windows.Forms.Panel();
            this.txtEpc = new System.Windows.Forms.TextBox();
            this.lblEpc = new System.Windows.Forms.Label();
            this.lblSKU = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.dtvResults = new System.Windows.Forms.DataGridView();
            this.comboBoxDescription = new System.Windows.Forms.ComboBox();
            this.comboBoxSKU = new System.Windows.Forms.ComboBox();
            this.toolStrip1.SuspendLayout();
            this.pnlForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtvResults)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbSave,
            this.tsbNew,
            this.tsbCancel,
            this.tsbPrint,
            this.toolStripSeparator1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1165, 29);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbSave
            // 
            this.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSave.Image = ((System.Drawing.Image)(resources.GetObject("tsbSave.Image")));
            this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(34, 24);
            this.tsbSave.Text = "Salvar";
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // tsbNew
            // 
            this.tsbNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNew.Image = ((System.Drawing.Image)(resources.GetObject("tsbNew.Image")));
            this.tsbNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNew.Name = "tsbNew";
            this.tsbNew.Size = new System.Drawing.Size(34, 24);
            this.tsbNew.Text = "Novo";
            this.tsbNew.Click += new System.EventHandler(this.tsbNew_Click);
            // 
            // tsbCancel
            // 
            this.tsbCancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCancel.Image = ((System.Drawing.Image)(resources.GetObject("tsbCancel.Image")));
            this.tsbCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCancel.Name = "tsbCancel";
            this.tsbCancel.Size = new System.Drawing.Size(34, 24);
            this.tsbCancel.Text = "Voltar";
            this.tsbCancel.Click += new System.EventHandler(this.tsbCancel_ClickAsync);
            //
            // tsbPrint
            //
            this.tsbPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPrint.Image = ((System.Drawing.Image)(resources.GetObject("tsbPrint.Image")));
            this.tsbPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPrint.Name = "tsbPrint";
            this.tsbPrint.Size = new System.Drawing.Size(34, 24);
            this.tsbPrint.Text = "Imprimir";
            this.tsbPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 29);
            // 
            // pnlForm
            // 
            this.pnlForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(236)))), ((int)(((byte)(240)))));
            this.pnlForm.Controls.Add(this.comboBoxSKU);
            this.pnlForm.Controls.Add(this.comboBoxDescription);
            this.pnlForm.Controls.Add(this.txtEpc);
            this.pnlForm.Controls.Add(this.lblEpc);
            this.pnlForm.Controls.Add(this.lblSKU);
            this.pnlForm.Controls.Add(this.lblDescription);
            this.pnlForm.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlForm.Location = new System.Drawing.Point(0, 29);
            this.pnlForm.Margin = new System.Windows.Forms.Padding(5);
            this.pnlForm.Name = "pnlForm";
            this.pnlForm.Size = new System.Drawing.Size(1165, 186);
            this.pnlForm.TabIndex = 3;
            // 
            // txtEpc
            // 
            this.txtEpc.Location = new System.Drawing.Point(119, 120);
            this.txtEpc.Margin = new System.Windows.Forms.Padding(5);
            this.txtEpc.MaxLength = 250;
            this.txtEpc.Name = "txtEpc";
            this.txtEpc.Size = new System.Drawing.Size(334, 26);
            this.txtEpc.TabIndex = 10;
            // 
            // lblEpc
            // 
            this.lblEpc.AutoSize = true;
            this.lblEpc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEpc.Location = new System.Drawing.Point(57, 120);
            this.lblEpc.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblEpc.Name = "lblEpc";
            this.lblEpc.Size = new System.Drawing.Size(52, 20);
            this.lblEpc.TabIndex = 9;
            this.lblEpc.Text = "EPC:";
            // 
            // lblSKU
            // 
            this.lblSKU.AutoSize = true;
            this.lblSKU.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSKU.Location = new System.Drawing.Point(57, 77);
            this.lblSKU.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblSKU.Name = "lblSKU";
            this.lblSKU.Size = new System.Drawing.Size(52, 20);
            this.lblSKU.TabIndex = 6;
            this.lblSKU.Text = "SKU:";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.Location = new System.Drawing.Point(8, 33);
            this.lblDescription.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(101, 20);
            this.lblDescription.TabIndex = 4;
            this.lblDescription.Text = "Descrição:";
            // 
            // dtvResults
            // 
            this.dtvResults.AllowUserToAddRows = false;
            this.dtvResults.AllowUserToDeleteRows = false;
            this.dtvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtvResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtvResults.Location = new System.Drawing.Point(0, 215);
            this.dtvResults.Margin = new System.Windows.Forms.Padding(5);
            this.dtvResults.Name = "dtvResults";
            this.dtvResults.ReadOnly = true;
            this.dtvResults.RowHeadersWidth = 51;
            this.dtvResults.Size = new System.Drawing.Size(1165, 575);
            this.dtvResults.TabIndex = 4;
            this.dtvResults.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtvResults_CellContentClick);
            // 
            // comboBoxDescription
            // 
            this.comboBoxDescription.FormattingEnabled = true;
            this.comboBoxDescription.Location = new System.Drawing.Point(119, 30);
            this.comboBoxDescription.Name = "comboBoxDescription";
            this.comboBoxDescription.Size = new System.Drawing.Size(573, 28);
            this.comboBoxDescription.TabIndex = 12;
            // 
            // comboBoxSKU
            // 
            this.comboBoxSKU.FormattingEnabled = true;
            this.comboBoxSKU.Location = new System.Drawing.Point(119, 74);
            this.comboBoxSKU.Name = "comboBoxSKU";
            this.comboBoxSKU.Size = new System.Drawing.Size(334, 28);
            this.comboBoxSKU.TabIndex = 13;
            // 
            // FrmProducts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1165, 790);
            this.Controls.Add(this.dtvResults);
            this.Controls.Add(this.pnlForm);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FrmProducts";
            this.Text = "FrmUsers";
            this.Load += new System.EventHandler(this.FrmPrinters_LoadAsync);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.pnlForm.ResumeLayout(false);
            this.pnlForm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtvResults)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbNew;
        private System.Windows.Forms.ToolStripButton tsbSave;
        private System.Windows.Forms.ToolStripButton tsbCancel;
        private System.Windows.Forms.ToolStripButton tsbPrint;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Panel pnlForm;
        private System.Windows.Forms.DataGridView dtvResults;
        private System.Windows.Forms.Label lblSKU;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtEpc;
        private System.Windows.Forms.Label lblEpc;
        private System.Windows.Forms.ComboBox comboBoxDescription;
        private System.Windows.Forms.ComboBox comboBoxSKU;
    }
}