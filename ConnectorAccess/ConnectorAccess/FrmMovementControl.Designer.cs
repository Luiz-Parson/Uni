namespace ConnectorAccess
{
    partial class FrmMovementControl
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
            this.btnFinalizar = new System.Windows.Forms.Button();
            this.btnIniciar = new System.Windows.Forms.Button();
            this.comboBoxTipo = new System.Windows.Forms.ComboBox();
            this.dataGridViewControlProducts = new System.Windows.Forms.DataGridView();
            this.EPC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descricao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SKU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTotal = new System.Windows.Forms.Label();
            this.comboBoxStatus = new System.Windows.Forms.ComboBox();
            this.dataGridViewTotalProducts = new System.Windows.Forms.DataGridView();
            this.descricaoResume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SKUResume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QuantidadeResume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewControlProducts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTotalProducts)).BeginInit();
            this.SuspendLayout();
            // 
            // btnFinalizar
            // 
            this.btnFinalizar.BackColor = System.Drawing.Color.Yellow;
            this.btnFinalizar.Location = new System.Drawing.Point(990, 725);
            this.btnFinalizar.Margin = new System.Windows.Forms.Padding(4);
            this.btnFinalizar.Name = "btnFinalizar";
            this.btnFinalizar.Size = new System.Drawing.Size(157, 50);
            this.btnFinalizar.TabIndex = 1;
            this.btnFinalizar.Text = "Finalizar";
            this.btnFinalizar.UseVisualStyleBackColor = false;
            this.btnFinalizar.Click += new System.EventHandler(this.finalizar_Click);
            // 
            // btnIniciar
            // 
            this.btnIniciar.BackColor = System.Drawing.Color.Lime;
            this.btnIniciar.Location = new System.Drawing.Point(814, 725);
            this.btnIniciar.Margin = new System.Windows.Forms.Padding(4);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new System.Drawing.Size(157, 50);
            this.btnIniciar.TabIndex = 2;
            this.btnIniciar.Text = "Iniciar";
            this.btnIniciar.UseVisualStyleBackColor = false;
            this.btnIniciar.Click += new System.EventHandler(this.iniciar_Click);
            // 
            // comboBoxTipo
            // 
            this.comboBoxTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTipo.FormattingEnabled = true;
            this.comboBoxTipo.Items.AddRange(new object[] {
            "Enviar",
            "Receber",
            "Contagem"});
            this.comboBoxTipo.Location = new System.Drawing.Point(637, 737);
            this.comboBoxTipo.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxTipo.Name = "comboBoxTipo";
            this.comboBoxTipo.Size = new System.Drawing.Size(154, 28);
            this.comboBoxTipo.TabIndex = 3;
            // 
            // dataGridViewControlProducts
            // 
            this.dataGridViewControlProducts.AllowUserToAddRows = false;
            this.dataGridViewControlProducts.AllowUserToDeleteRows = false;
            this.dataGridViewControlProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewControlProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EPC,
            this.descricao,
            this.SKU,
            this.Status});
            this.dataGridViewControlProducts.Location = new System.Drawing.Point(14, 14);
            this.dataGridViewControlProducts.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridViewControlProducts.Name = "dataGridViewControlProducts";
            this.dataGridViewControlProducts.ReadOnly = true;
            this.dataGridViewControlProducts.RowHeadersWidth = 51;
            this.dataGridViewControlProducts.RowTemplate.Height = 24;
            this.dataGridViewControlProducts.Size = new System.Drawing.Size(708, 703);
            this.dataGridViewControlProducts.TabIndex = 4;
            // 
            // EPC
            // 
            this.EPC.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.EPC.FillWeight = 108.3772F;
            this.EPC.HeaderText = "EPC";
            this.EPC.MinimumWidth = 6;
            this.EPC.Name = "EPC";
            this.EPC.ReadOnly = true;
            // 
            // descricao
            // 
            this.descricao.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.descricao.FillWeight = 108.3772F;
            this.descricao.HeaderText = "Descrição";
            this.descricao.MinimumWidth = 6;
            this.descricao.Name = "descricao";
            this.descricao.ReadOnly = true;
            // 
            // SKU
            // 
            this.SKU.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SKU.FillWeight = 108.3772F;
            this.SKU.HeaderText = "SKU";
            this.SKU.MinimumWidth = 6;
            this.SKU.Name = "SKU";
            this.SKU.ReadOnly = true;
            // 
            // Status
            // 
            this.Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Status.FillWeight = 127.8409F;
            this.Status.HeaderText = "Status";
            this.Status.MinimumWidth = 6;
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.BackColor = System.Drawing.Color.White;
            this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblTotal.Location = new System.Drawing.Point(12, 745);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(69, 20);
            this.lblTotal.TabIndex = 6;
            this.lblTotal.Text = "Total:  ";
            // 
            // comboBoxStatus
            // 
            this.comboBoxStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStatus.FormattingEnabled = true;
            this.comboBoxStatus.Items.AddRange(new object[] {
            "Ativo",
            "Reparo",
            "Calibração"});
            this.comboBoxStatus.Location = new System.Drawing.Point(454, 737);
            this.comboBoxStatus.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxStatus.Name = "comboBoxStatus";
            this.comboBoxStatus.Size = new System.Drawing.Size(154, 28);
            this.comboBoxStatus.TabIndex = 7;
            // 
            // dataGridViewTotalProducts
            // 
            this.dataGridViewTotalProducts.AllowUserToAddRows = false;
            this.dataGridViewTotalProducts.AllowUserToDeleteRows = false;
            this.dataGridViewTotalProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTotalProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.descricaoResume,
            this.SKUResume,
            this.QuantidadeResume});
            this.dataGridViewTotalProducts.Location = new System.Drawing.Point(730, 14);
            this.dataGridViewTotalProducts.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridViewTotalProducts.Name = "dataGridViewTotalProducts";
            this.dataGridViewTotalProducts.ReadOnly = true;
            this.dataGridViewTotalProducts.RowHeadersWidth = 51;
            this.dataGridViewTotalProducts.RowTemplate.Height = 24;
            this.dataGridViewTotalProducts.Size = new System.Drawing.Size(417, 703);
            this.dataGridViewTotalProducts.TabIndex = 8;
            // 
            // descricaoResume
            // 
            this.descricaoResume.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.descricaoResume.FillWeight = 100.1818F;
            this.descricaoResume.HeaderText = "Descrição";
            this.descricaoResume.MinimumWidth = 6;
            this.descricaoResume.Name = "descricaoResume";
            this.descricaoResume.ReadOnly = true;
            // 
            // SKUResume
            // 
            this.SKUResume.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SKUResume.FillWeight = 50.1818F;
            this.SKUResume.HeaderText = "SKU";
            this.SKUResume.MinimumWidth = 6;
            this.SKUResume.Name = "SKUResume";
            this.SKUResume.ReadOnly = true;
            // 
            // QuantidadeResume
            // 
            this.QuantidadeResume.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.QuantidadeResume.FillWeight = 100.4545F;
            this.QuantidadeResume.HeaderText = "Quantidade";
            this.QuantidadeResume.MinimumWidth = 6;
            this.QuantidadeResume.Name = "QuantidadeResume";
            this.QuantidadeResume.ReadOnly = true;
            // 
            // FrmMovementControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(1200, 830);
            this.Controls.Add(this.dataGridViewTotalProducts);
            this.Controls.Add(this.comboBoxStatus);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.dataGridViewControlProducts);
            this.Controls.Add(this.comboBoxTipo);
            this.Controls.Add(this.btnIniciar);
            this.Controls.Add(this.btnFinalizar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FrmMovementControl";
            this.Text = "FrmAccessControl";
            this.Activated += new System.EventHandler(this.FrmAccessMovementControl_Load);
            this.Load += new System.EventHandler(this.FrmAccessMovementControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewControlProducts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTotalProducts)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnFinalizar;
        private System.Windows.Forms.Button btnIniciar;
        private System.Windows.Forms.ComboBox comboBoxTipo;
        private System.Windows.Forms.DataGridView dataGridViewControlProducts;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.ComboBox comboBoxStatus;
        private System.Windows.Forms.DataGridView dataGridViewTotalProducts;
        private System.Windows.Forms.DataGridViewTextBoxColumn descricaoResume;
        private System.Windows.Forms.DataGridViewTextBoxColumn SKUResume;
        private System.Windows.Forms.DataGridViewTextBoxColumn QuantidadeResume;
        private System.Windows.Forms.DataGridViewTextBoxColumn EPC;
        private System.Windows.Forms.DataGridViewTextBoxColumn descricao;
        private System.Windows.Forms.DataGridViewTextBoxColumn SKU;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
    }
}