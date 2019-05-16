namespace PP791
{
    partial class TerAppConfigForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gbConfigData = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.gbControls = new System.Windows.Forms.GroupBox();
            this.btnDeleteItem = new System.Windows.Forms.Button();
            this.btnAddItem = new System.Windows.Forms.Button();
            this.listBoxConfig = new System.Windows.Forms.ListBox();
            this.btnSetSelected = new System.Windows.Forms.Button();
            this.btnSetAll = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.gbConfigData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.gbControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbConfigData
            // 
            this.gbConfigData.Controls.Add(this.dataGridView1);
            this.gbConfigData.Dock = System.Windows.Forms.DockStyle.Right;
            this.gbConfigData.Location = new System.Drawing.Point(478, 0);
            this.gbConfigData.Name = "gbConfigData";
            this.gbConfigData.Size = new System.Drawing.Size(529, 604);
            this.gbConfigData.TabIndex = 0;
            this.gbConfigData.TabStop = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ActiveBorder;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 18);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(523, 583);
            this.dataGridView1.TabIndex = 0;
            // 
            // gbControls
            // 
            this.gbControls.Controls.Add(this.btnDeleteItem);
            this.gbControls.Controls.Add(this.btnAddItem);
            this.gbControls.Controls.Add(this.listBoxConfig);
            this.gbControls.Controls.Add(this.btnSetSelected);
            this.gbControls.Controls.Add(this.btnSetAll);
            this.gbControls.Controls.Add(this.btnOpen);
            this.gbControls.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbControls.Location = new System.Drawing.Point(0, 0);
            this.gbControls.Name = "gbControls";
            this.gbControls.Size = new System.Drawing.Size(472, 604);
            this.gbControls.TabIndex = 1;
            this.gbControls.TabStop = false;
            // 
            // btnDeleteItem
            // 
            this.btnDeleteItem.Location = new System.Drawing.Point(179, 80);
            this.btnDeleteItem.Name = "btnDeleteItem";
            this.btnDeleteItem.Size = new System.Drawing.Size(57, 53);
            this.btnDeleteItem.TabIndex = 5;
            this.btnDeleteItem.Text = "-";
            this.btnDeleteItem.UseVisualStyleBackColor = true;
            this.btnDeleteItem.Click += new System.EventHandler(this.btnDeleteItem_Click);
            // 
            // btnAddItem
            // 
            this.btnAddItem.Location = new System.Drawing.Point(179, 21);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(57, 53);
            this.btnAddItem.TabIndex = 4;
            this.btnAddItem.Text = "+";
            this.btnAddItem.UseVisualStyleBackColor = true;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // listBoxConfig
            // 
            this.listBoxConfig.Dock = System.Windows.Forms.DockStyle.Right;
            this.listBoxConfig.Font = new System.Drawing.Font("Arial Narrow", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.listBoxConfig.FormattingEnabled = true;
            this.listBoxConfig.ItemHeight = 21;
            this.listBoxConfig.Location = new System.Drawing.Point(242, 18);
            this.listBoxConfig.Name = "listBoxConfig";
            this.listBoxConfig.Size = new System.Drawing.Size(227, 583);
            this.listBoxConfig.TabIndex = 3;
            this.listBoxConfig.SelectedIndexChanged += new System.EventHandler(this.listBoxConfig_SelectedIndexChanged);
            // 
            // btnSetSelected
            // 
            this.btnSetSelected.Location = new System.Drawing.Point(6, 266);
            this.btnSetSelected.Name = "btnSetSelected";
            this.btnSetSelected.Size = new System.Drawing.Size(95, 97);
            this.btnSetSelected.TabIndex = 2;
            this.btnSetSelected.Text = "Set Selected";
            this.btnSetSelected.UseVisualStyleBackColor = true;
            this.btnSetSelected.Click += new System.EventHandler(this.btnSetSelected_Click);
            // 
            // btnSetAll
            // 
            this.btnSetAll.Location = new System.Drawing.Point(6, 163);
            this.btnSetAll.Name = "btnSetAll";
            this.btnSetAll.Size = new System.Drawing.Size(95, 97);
            this.btnSetAll.TabIndex = 1;
            this.btnSetAll.Text = "Set All";
            this.btnSetAll.UseVisualStyleBackColor = true;
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(6, 60);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(95, 97);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "OPEN";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // TerAppConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1007, 604);
            this.Controls.Add(this.gbControls);
            this.Controls.Add(this.gbConfigData);
            this.Name = "TerAppConfigForm";
            this.Text = "TerAppConfigForm";
            this.gbConfigData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.gbControls.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbConfigData;
        private System.Windows.Forms.GroupBox gbControls;
        private System.Windows.Forms.Button btnSetSelected;
        private System.Windows.Forms.Button btnSetAll;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.ListBox listBoxConfig;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnDeleteItem;
        private System.Windows.Forms.Button btnAddItem;
    }
}