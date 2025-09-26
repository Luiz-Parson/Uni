
namespace ConnectorAccess
{
    partial class FrmExclusionAndUpdate
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
            this.btnDeleteAndUpdate = new System.Windows.Forms.Button();
            this.btnIniciar = new System.Windows.Forms.Button();
            this.dataGridViewRegisterProducts = new System.Windows.Forms.DataGridView();
            this.EPC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descricao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SKU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lbldescription = new System.Windows.Forms.Label();
            this.lblSKU = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.comboBoxSKU = new System.Windows.Forms.ComboBox();
            this.comboBoxDescription = new System.Windows.Forms.ComboBox();
            this.comboBoxCategory = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRegisterProducts)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDeleteAndUpdate
            // 
            this.btnDeleteAndUpdate.BackColor = System.Drawing.Color.Crimson;
            this.btnDeleteAndUpdate.Location = new System.Drawing.Point(990, 725);
            this.btnDeleteAndUpdate.Margin = new System.Windows.Forms.Padding(4);
            this.btnDeleteAndUpdate.Name = "btnDeleteAndUpdate";
            this.btnDeleteAndUpdate.Size = new System.Drawing.Size(157, 50);
            this.btnDeleteAndUpdate.TabIndex = 1;
            this.btnDeleteAndUpdate.Text = "Excluir e Alterar";
            this.btnDeleteAndUpdate.UseVisualStyleBackColor = false;
            this.btnDeleteAndUpdate.Click += new System.EventHandler(this.btnCadastrar_ClickAsync);
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
            this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click);
            // 
            // dataGridViewRegisterProducts
            // 
            this.dataGridViewRegisterProducts.AllowUserToAddRows = false;
            this.dataGridViewRegisterProducts.AllowUserToDeleteRows = false;
            this.dataGridViewRegisterProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRegisterProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EPC,
            this.descricao,
            this.SKU});
            this.dataGridViewRegisterProducts.Location = new System.Drawing.Point(14, 119);
            this.dataGridViewRegisterProducts.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridViewRegisterProducts.Name = "dataGridViewRegisterProducts";
            this.dataGridViewRegisterProducts.RowHeadersWidth = 51;
            this.dataGridViewRegisterProducts.RowTemplate.Height = 24;
            this.dataGridViewRegisterProducts.Size = new System.Drawing.Size(1133, 598);
            this.dataGridViewRegisterProducts.TabIndex = 4;
            // 
            // EPC
            // 
            this.EPC.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.EPC.HeaderText = "EPC";
            this.EPC.MinimumWidth = 6;
            this.EPC.Name = "EPC";
            this.EPC.ReadOnly = true;
            // 
            // descricao
            // 
            this.descricao.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.descricao.HeaderText = "Descrição";
            this.descricao.MinimumWidth = 6;
            this.descricao.Name = "descricao";
            // 
            // SKU
            // 
            this.SKU.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SKU.HeaderText = "SKU";
            this.SKU.MinimumWidth = 6;
            this.SKU.Name = "SKU";
            // 
            // lbldescription
            // 
            this.lbldescription.AutoSize = true;
            this.lbldescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lbldescription.Location = new System.Drawing.Point(12, 27);
            this.lbldescription.Name = "lbldescription";
            this.lbldescription.Size = new System.Drawing.Size(101, 20);
            this.lbldescription.TabIndex = 5;
            this.lbldescription.Text = "Descrição:";
            // 
            // lblSKU
            // 
            this.lblSKU.AutoSize = true;
            this.lblSKU.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblSKU.Location = new System.Drawing.Point(61, 70);
            this.lblSKU.Name = "lblSKU";
            this.lblSKU.Size = new System.Drawing.Size(52, 20);
            this.lblSKU.TabIndex = 7;
            this.lblSKU.Text = "SKU:";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.BackColor = System.Drawing.Color.White;
            this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblTotal.Location = new System.Drawing.Point(12, 745);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(69, 20);
            this.lblTotal.TabIndex = 9;
            this.lblTotal.Text = "Total:  ";
            // 
            // comboBoxSKU
            // 
            this.comboBoxSKU.FormattingEnabled = true;
            this.comboBoxSKU.Location = new System.Drawing.Point(119, 67);
            this.comboBoxSKU.Name = "comboBoxSKU";
            this.comboBoxSKU.Size = new System.Drawing.Size(234, 28);
            this.comboBoxSKU.TabIndex = 10;
            // 
            // comboBoxDescription
            // 
            this.comboBoxDescription.FormattingEnabled = true;
            this.comboBoxDescription.Location = new System.Drawing.Point(119, 24);
            this.comboBoxDescription.Name = "comboBoxDescription";
            this.comboBoxDescription.Size = new System.Drawing.Size(573, 28);
            this.comboBoxDescription.TabIndex = 11;
            // 
            // comboBoxCategory
            // 
            this.comboBoxCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCategory.FormattingEnabled = true;
            this.comboBoxCategory.Items.AddRange(new object[] {
            "Desuso",
            "Danificado"});
            this.comboBoxCategory.Location = new System.Drawing.Point(641, 737);
            this.comboBoxCategory.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxCategory.Name = "comboBoxCategory";
            this.comboBoxCategory.Size = new System.Drawing.Size(154, 28);
            this.comboBoxCategory.TabIndex = 12;
            // 
            // FrmExclusionAndUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(1200, 830);
            this.Controls.Add(this.comboBoxCategory);
            this.Controls.Add(this.comboBoxDescription);
            this.Controls.Add(this.comboBoxSKU);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.lblSKU);
            this.Controls.Add(this.lbldescription);
            this.Controls.Add(this.dataGridViewRegisterProducts);
            this.Controls.Add(this.btnIniciar);
            this.Controls.Add(this.btnDeleteAndUpdate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FrmExclusionAndUpdate";
            this.Text = "FrmAccessControl";
            this.Activated += new System.EventHandler(this.FrmExclusionAndUpdate_LoadAsync);
            this.Load += new System.EventHandler(this.FrmExclusionAndUpdate_LoadAsync);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRegisterProducts)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnDeleteAndUpdate;
        private System.Windows.Forms.Button btnIniciar;
        private System.Windows.Forms.DataGridView dataGridViewRegisterProducts;
        private System.Windows.Forms.DataGridViewTextBoxColumn EPC;
        private System.Windows.Forms.DataGridViewTextBoxColumn descricao;
        private System.Windows.Forms.DataGridViewTextBoxColumn SKU;
        private System.Windows.Forms.Label lbldescription;
        private System.Windows.Forms.Label lblSKU;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.ComboBox comboBoxSKU;
        private System.Windows.Forms.ComboBox comboBoxDescription;
        private System.Windows.Forms.ComboBox comboBoxCategory;
    }
}