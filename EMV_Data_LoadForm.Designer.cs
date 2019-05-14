namespace PP791
{
    partial class EMV_Data_LoadForm
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnDataFormat = new System.Windows.Forms.Button();
            this.btnPubKey = new System.Windows.Forms.Button();
            this.btnApplData = new System.Windows.Forms.Button();
            this.btnTermData = new System.Windows.Forms.Button();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripMessageLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnRfidPubKey = new System.Windows.Forms.Button();
            this.btnRfidApplData = new System.Windows.Forms.Button();
            this.btnRfidTermData = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(12, 203);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(457, 252);
            this.textBox1.TabIndex = 19;
            // 
            // btnDataFormat
            // 
            this.btnDataFormat.BackColor = System.Drawing.Color.LightSeaGreen;
            this.btnDataFormat.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDataFormat.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnDataFormat.Location = new System.Drawing.Point(27, 138);
            this.btnDataFormat.Name = "btnDataFormat";
            this.btnDataFormat.Size = new System.Drawing.Size(168, 33);
            this.btnDataFormat.TabIndex = 18;
            this.btnDataFormat.Text = "Data Format Setup";
            this.btnDataFormat.UseVisualStyleBackColor = false;
            this.btnDataFormat.Click += new System.EventHandler(this.btnDataFormat_Click);
            // 
            // btnPubKey
            // 
            this.btnPubKey.BackColor = System.Drawing.Color.LightSeaGreen;
            this.btnPubKey.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPubKey.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnPubKey.Location = new System.Drawing.Point(25, 101);
            this.btnPubKey.Name = "btnPubKey";
            this.btnPubKey.Size = new System.Drawing.Size(170, 30);
            this.btnPubKey.TabIndex = 17;
            this.btnPubKey.Text = "Public Key Setup";
            this.btnPubKey.UseVisualStyleBackColor = false;
            this.btnPubKey.Click += new System.EventHandler(this.btnPubKey_Click);
            // 
            // btnApplData
            // 
            this.btnApplData.BackColor = System.Drawing.Color.LightSeaGreen;
            this.btnApplData.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnApplData.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnApplData.Location = new System.Drawing.Point(25, 62);
            this.btnApplData.Name = "btnApplData";
            this.btnApplData.Size = new System.Drawing.Size(170, 33);
            this.btnApplData.TabIndex = 16;
            this.btnApplData.Text = "Application Data Setup";
            this.btnApplData.UseVisualStyleBackColor = false;
            this.btnApplData.Click += new System.EventHandler(this.btnApplData_Click);
            // 
            // btnTermData
            // 
            this.btnTermData.BackColor = System.Drawing.Color.LightSeaGreen;
            this.btnTermData.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnTermData.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnTermData.Location = new System.Drawing.Point(25, 25);
            this.btnTermData.Name = "btnTermData";
            this.btnTermData.Size = new System.Drawing.Size(170, 31);
            this.btnTermData.TabIndex = 15;
            this.btnTermData.Text = "Terminal Data Setup";
            this.btnTermData.UseVisualStyleBackColor = false;
            this.btnTermData.Click += new System.EventHandler(this.btnTermData_Click);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.toolStripStatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(117, 20);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "STATUS";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(117, 20);
            this.toolStripStatusLabel.Spring = true;
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.AutoSize = false;
            this.toolStripStatusLabel3.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel3.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.toolStripStatusLabel3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(117, 20);
            this.toolStripStatusLabel3.Spring = true;
            this.toolStripStatusLabel3.Text = "MESSAGE";
            // 
            // toolStripMessageLabel
            // 
            this.toolStripMessageLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripMessageLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.toolStripMessageLabel.Name = "toolStripMessageLabel";
            this.toolStripMessageLabel.Size = new System.Drawing.Size(117, 20);
            this.toolStripMessageLabel.Spring = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel,
            this.toolStripStatusLabel3,
            this.toolStripMessageLabel,
            this.toolStripStatusLabel5});
            this.statusStrip1.Location = new System.Drawing.Point(0, 468);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(485, 25);
            this.statusStrip1.TabIndex = 20;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(0, 20);
            // 
            // openFileDialog
            // 
            this.openFileDialog1.FileName = "openFileDialog";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnDataFormat);
            this.groupBox1.Controls.Add(this.btnPubKey);
            this.groupBox1.Controls.Add(this.btnApplData);
            this.groupBox1.Controls.Add(this.btnTermData);
            this.groupBox1.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(221, 185);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ICC";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnRfidPubKey);
            this.groupBox2.Controls.Add(this.btnRfidApplData);
            this.groupBox2.Controls.Add(this.btnRfidTermData);
            this.groupBox2.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox2.Location = new System.Drawing.Point(248, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(221, 185);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "RFID";
            // 
            // btnRfidPubKey
            // 
            this.btnRfidPubKey.BackColor = System.Drawing.Color.LightSeaGreen;
            this.btnRfidPubKey.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRfidPubKey.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnRfidPubKey.Location = new System.Drawing.Point(25, 101);
            this.btnRfidPubKey.Name = "btnRfidPubKey";
            this.btnRfidPubKey.Size = new System.Drawing.Size(170, 30);
            this.btnRfidPubKey.TabIndex = 17;
            this.btnRfidPubKey.Text = "Public Key Setup";
            this.btnRfidPubKey.UseVisualStyleBackColor = false;
            this.btnRfidPubKey.Click += new System.EventHandler(this.btnRfidPubKey_Click);
            // 
            // btnRfidApplData
            // 
            this.btnRfidApplData.BackColor = System.Drawing.Color.LightSeaGreen;
            this.btnRfidApplData.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRfidApplData.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnRfidApplData.Location = new System.Drawing.Point(25, 62);
            this.btnRfidApplData.Name = "btnRfidApplData";
            this.btnRfidApplData.Size = new System.Drawing.Size(170, 33);
            this.btnRfidApplData.TabIndex = 16;
            this.btnRfidApplData.Text = "Application Data Setup";
            this.btnRfidApplData.UseVisualStyleBackColor = false;
            this.btnRfidApplData.Click += new System.EventHandler(this.btnRfidApplData_Click);
            // 
            // btnRfidTermData
            // 
            this.btnRfidTermData.BackColor = System.Drawing.Color.LightSeaGreen;
            this.btnRfidTermData.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRfidTermData.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnRfidTermData.Location = new System.Drawing.Point(25, 25);
            this.btnRfidTermData.Name = "btnRfidTermData";
            this.btnRfidTermData.Size = new System.Drawing.Size(170, 31);
            this.btnRfidTermData.TabIndex = 15;
            this.btnRfidTermData.Text = "Terminal Data Setup";
            this.btnRfidTermData.UseVisualStyleBackColor = false;
            this.btnRfidTermData.Click += new System.EventHandler(this.btnRfidTermData_Click);
            // 
            // EMV_Data_LoadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 493);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "EMV_Data_LoadForm";
            this.Text = "EMV_Data_LoadForm";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnDataFormat;
        private System.Windows.Forms.Button btnPubKey;
        private System.Windows.Forms.Button btnApplData;
        private System.Windows.Forms.Button btnTermData;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripMessageLabel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnRfidPubKey;
        private System.Windows.Forms.Button btnRfidApplData;
        private System.Windows.Forms.Button btnRfidTermData;
    }
}