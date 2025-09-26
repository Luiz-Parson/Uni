
namespace ConnectorAccess
{
    partial class FrmReportValidity
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReportValidity));
            this.pnlFilters = new System.Windows.Forms.Panel();
            this.txtSKU = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.btnCsv = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.txtEpc = new System.Windows.Forms.TextBox();
            this.lblEpc = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblSKU = new System.Windows.Forms.Label();
            this.dtvResults = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtvResults)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFilters
            // 
            this.pnlFilters.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(236)))), ((int)(((byte)(240)))));
            this.pnlFilters.Controls.Add(this.label2);
            this.pnlFilters.Controls.Add(this.label1);
            this.pnlFilters.Controls.Add(this.panel2);
            this.pnlFilters.Controls.Add(this.panel1);
            this.pnlFilters.Controls.Add(this.txtSKU);
            this.pnlFilters.Controls.Add(this.txtDescription);
            this.pnlFilters.Controls.Add(this.lblDescription);
            this.pnlFilters.Controls.Add(this.btnCsv);
            this.pnlFilters.Controls.Add(this.btnClear);
            this.pnlFilters.Controls.Add(this.btnPrint);
            this.pnlFilters.Controls.Add(this.txtEpc);
            this.pnlFilters.Controls.Add(this.lblEpc);
            this.pnlFilters.Controls.Add(this.btnSearch);
            this.pnlFilters.Controls.Add(this.lblSKU);
            this.pnlFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilters.Location = new System.Drawing.Point(0, 0);
            this.pnlFilters.Margin = new System.Windows.Forms.Padding(5);
            this.pnlFilters.Name = "pnlFilters";
            this.pnlFilters.Size = new System.Drawing.Size(1165, 149);
            this.pnlFilters.TabIndex = 1;
            // 
            // txtSKU
            // 
            this.txtSKU.Location = new System.Drawing.Point(128, 105);
            this.txtSKU.Margin = new System.Windows.Forms.Padding(5);
            this.txtSKU.MaxLength = 250;
            this.txtSKU.Name = "txtSKU";
            this.txtSKU.Size = new System.Drawing.Size(91, 26);
            this.txtSKU.TabIndex = 27;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(128, 63);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(5);
            this.txtDescription.MaxLength = 250;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(367, 26);
            this.txtDescription.TabIndex = 26;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.Location = new System.Drawing.Point(17, 63);
            this.lblDescription.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(101, 20);
            this.lblDescription.TabIndex = 25;
            this.lblDescription.Text = "Descrição:";
            // 
            // btnCsv
            // 
            this.btnCsv.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnCsv.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnCsv.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.btnCsv.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.btnCsv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCsv.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCsv.Image = ((System.Drawing.Image)(resources.GetObject("btnCsv.Image")));
            this.btnCsv.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCsv.Location = new System.Drawing.Point(964, 76);
            this.btnCsv.Margin = new System.Windows.Forms.Padding(5);
            this.btnCsv.Name = "btnCsv";
            this.btnCsv.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.btnCsv.Size = new System.Drawing.Size(82, 46);
            this.btnCsv.TabIndex = 24;
            this.btnCsv.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCsv.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCsv.UseVisualStyleBackColor = false;
            this.btnCsv.Click += new System.EventHandler(this.btnCsv_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnClear.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.btnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Image = ((System.Drawing.Image)(resources.GetObject("btnClear.Image")));
            this.btnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClear.Location = new System.Drawing.Point(1069, 76);
            this.btnClear.Margin = new System.Windows.Forms.Padding(5);
            this.btnClear.Name = "btnClear";
            this.btnClear.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.btnClear.Size = new System.Drawing.Size(82, 46);
            this.btnClear.TabIndex = 21;
            this.btnClear.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnPrint.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnPrint.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.btnPrint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.Image = ((System.Drawing.Image)(resources.GetObject("btnPrint.Image")));
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrint.Location = new System.Drawing.Point(1069, 11);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(5);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.btnPrint.Size = new System.Drawing.Size(82, 46);
            this.btnPrint.TabIndex = 20;
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // txtEpc
            // 
            this.txtEpc.Location = new System.Drawing.Point(128, 19);
            this.txtEpc.Margin = new System.Windows.Forms.Padding(5);
            this.txtEpc.MaxLength = 250;
            this.txtEpc.Name = "txtEpc";
            this.txtEpc.Size = new System.Drawing.Size(367, 26);
            this.txtEpc.TabIndex = 17;
            this.txtEpc.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtEpc_KeyDown);
            // 
            // lblEpc
            // 
            this.lblEpc.AutoSize = true;
            this.lblEpc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEpc.Location = new System.Drawing.Point(66, 22);
            this.lblEpc.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblEpc.Name = "lblEpc";
            this.lblEpc.Size = new System.Drawing.Size(52, 20);
            this.lblEpc.TabIndex = 16;
            this.lblEpc.Text = "EPC:";
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnSearch.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.btnSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(964, 11);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.btnSearch.Size = new System.Drawing.Size(82, 46);
            this.btnSearch.TabIndex = 14;
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblSKU
            // 
            this.lblSKU.AutoSize = true;
            this.lblSKU.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSKU.Location = new System.Drawing.Point(66, 108);
            this.lblSKU.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblSKU.Name = "lblSKU";
            this.lblSKU.Size = new System.Drawing.Size(52, 20);
            this.lblSKU.TabIndex = 4;
            this.lblSKU.Text = "SKU:";
            // 
            // dtvResults
            // 
            this.dtvResults.AllowUserToAddRows = false;
            this.dtvResults.AllowUserToDeleteRows = false;
            this.dtvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtvResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtvResults.Location = new System.Drawing.Point(0, 149);
            this.dtvResults.Margin = new System.Windows.Forms.Padding(5);
            this.dtvResults.Name = "dtvResults";
            this.dtvResults.ReadOnly = true;
            this.dtvResults.RowHeadersWidth = 51;
            this.dtvResults.Size = new System.Drawing.Size(1165, 641);
            this.dtvResults.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Red;
            this.panel1.Location = new System.Drawing.Point(777, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(65, 26);
            this.panel1.TabIndex = 28;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Gold;
            this.panel2.Location = new System.Drawing.Point(777, 63);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(65, 26);
            this.panel2.TabIndex = 29;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(687, 22);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 20);
            this.label1.TabIndex = 30;
            this.label1.Text = "Vencido:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(550, 66);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(219, 20);
            this.label2.TabIndex = 31;
            this.label2.Text = "A vencer em até 15 dias:";
            // 
            // FrmReportValidity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1165, 790);
            this.Controls.Add(this.dtvResults);
            this.Controls.Add(this.pnlFilters);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FrmReportValidity";
            this.Text = "FrmReport";
            this.Load += new System.EventHandler(this.FrmReport_Load);
            this.pnlFilters.ResumeLayout(false);
            this.pnlFilters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtvResults)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblSKU;
        private System.Windows.Forms.DataGridView dtvResults;
        private System.Windows.Forms.Label lblEpc;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.TextBox txtEpc;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnCsv;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtSKU;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
    }
}