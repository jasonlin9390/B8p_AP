namespace PP791
{
    partial class PPDMain
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.Frame1 = new System.Windows.Forms.GroupBox();
            this.Frame2 = new System.Windows.Forms.GroupBox();
            this.BtnPCDTranTest = new System.Windows.Forms.Button();
            this.BtnClockSetup = new System.Windows.Forms.Button();
            this.BtnEMVTranTest = new System.Windows.Forms.Button();
            this.BtnEMVDataSetup = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.LbMonitor = new System.Windows.Forms.RichTextBox();
            this.CueTxet = new System.Windows.Forms.GroupBox();
            this.CueText = new System.Windows.Forms.RichTextBox();
            this.BtnExit = new System.Windows.Forms.Button();
            this.BtnClear = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.DriverFrame = new System.Windows.Forms.GroupBox();
            this.comboBox_Mode = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.BtnMainCloseCOM = new System.Windows.Forms.Button();
            this.BtnMainOpenCOM = new System.Windows.Forms.Button();
            this.ParityCombo = new System.Windows.Forms.ComboBox();
            this.BaudRateCombo = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.DataBitsCombo = new System.Windows.Forms.ComboBox();
            this.ComPortCombo = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TimeOutComb = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TextMark = new System.Windows.Forms.TextBox();
            this.Frame3 = new System.Windows.Forms.GroupBox();
            this.BtnMstTest = new System.Windows.Forms.Button();
            this.BtnFunTest1 = new System.Windows.Forms.Button();
            this.statusbar = new System.Windows.Forms.StatusStrip();
            this.StatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.Frame1.SuspendLayout();
            this.Frame2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.CueTxet.SuspendLayout();
            this.DriverFrame.SuspendLayout();
            this.Frame3.SuspendLayout();
            this.statusbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // Frame1
            // 
            this.Frame1.BackColor = System.Drawing.SystemColors.Control;
            this.Frame1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Frame1.Controls.Add(this.Frame2);
            this.Frame1.Controls.Add(this.groupBox1);
            this.Frame1.Controls.Add(this.CueTxet);
            this.Frame1.Controls.Add(this.BtnExit);
            this.Frame1.Controls.Add(this.BtnClear);
            this.Frame1.Controls.Add(this.BtnCancel);
            this.Frame1.Controls.Add(this.DriverFrame);
            this.Frame1.Controls.Add(this.TimeOutComb);
            this.Frame1.Controls.Add(this.label1);
            this.Frame1.Controls.Add(this.TextMark);
            this.Frame1.Controls.Add(this.Frame3);
            this.Frame1.Controls.Add(this.statusbar);
            this.Frame1.Cursor = System.Windows.Forms.Cursors.Default;
            this.Frame1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Frame1.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Frame1.Location = new System.Drawing.Point(6, 2);
            this.Frame1.Margin = new System.Windows.Forms.Padding(0);
            this.Frame1.Name = "Frame1";
            this.Frame1.Size = new System.Drawing.Size(746, 406);
            this.Frame1.TabIndex = 103;
            this.Frame1.TabStop = false;
            // 
            // Frame2
            // 
            this.Frame2.Controls.Add(this.BtnPCDTranTest);
            this.Frame2.Controls.Add(this.BtnClockSetup);
            this.Frame2.Controls.Add(this.BtnEMVTranTest);
            this.Frame2.Controls.Add(this.BtnEMVDataSetup);
            this.Frame2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Frame2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Frame2.Location = new System.Drawing.Point(548, 44);
            this.Frame2.Name = "Frame2";
            this.Frame2.Size = new System.Drawing.Size(198, 124);
            this.Frame2.TabIndex = 110;
            this.Frame2.TabStop = false;
            this.Frame2.Text = "EMV Transaction";
            // 
            // BtnPCDTranTest
            // 
            this.BtnPCDTranTest.BackColor = System.Drawing.Color.HotPink;
            this.BtnPCDTranTest.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.BtnPCDTranTest.FlatAppearance.BorderSize = 0;
            this.BtnPCDTranTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnPCDTranTest.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(166)));
            this.BtnPCDTranTest.Location = new System.Drawing.Point(96, 70);
            this.BtnPCDTranTest.Name = "BtnPCDTranTest";
            this.BtnPCDTranTest.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.BtnPCDTranTest.Size = new System.Drawing.Size(81, 41);
            this.BtnPCDTranTest.TabIndex = 48;
            this.BtnPCDTranTest.Text = "PCD Transaction";
            this.BtnPCDTranTest.UseVisualStyleBackColor = false;
            this.BtnPCDTranTest.Click += new System.EventHandler(this.BtnPCDTranTest_Click);
            // 
            // BtnClockSetup
            // 
            this.BtnClockSetup.BackColor = System.Drawing.Color.HotPink;
            this.BtnClockSetup.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.BtnClockSetup.FlatAppearance.BorderSize = 0;
            this.BtnClockSetup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnClockSetup.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(166)), true);
            this.BtnClockSetup.Location = new System.Drawing.Point(8, 70);
            this.BtnClockSetup.Name = "BtnClockSetup";
            this.BtnClockSetup.Size = new System.Drawing.Size(81, 41);
            this.BtnClockSetup.TabIndex = 47;
            this.BtnClockSetup.Text = "Time Clock Setting";
            this.BtnClockSetup.UseVisualStyleBackColor = false;
            this.BtnClockSetup.Click += new System.EventHandler(this.BtnClockSetup_Click);
            // 
            // BtnEMVTranTest
            // 
            this.BtnEMVTranTest.BackColor = System.Drawing.Color.HotPink;
            this.BtnEMVTranTest.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.BtnEMVTranTest.FlatAppearance.BorderSize = 0;
            this.BtnEMVTranTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnEMVTranTest.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(166)));
            this.BtnEMVTranTest.Location = new System.Drawing.Point(96, 22);
            this.BtnEMVTranTest.Name = "BtnEMVTranTest";
            this.BtnEMVTranTest.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.BtnEMVTranTest.Size = new System.Drawing.Size(81, 41);
            this.BtnEMVTranTest.TabIndex = 45;
            this.BtnEMVTranTest.Text = "ICC Transaction";
            this.BtnEMVTranTest.UseVisualStyleBackColor = false;
            this.BtnEMVTranTest.Click += new System.EventHandler(this.BtnEMVTranTest_Click);
            // 
            // BtnEMVDataSetup
            // 
            this.BtnEMVDataSetup.BackColor = System.Drawing.Color.HotPink;
            this.BtnEMVDataSetup.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.BtnEMVDataSetup.FlatAppearance.BorderSize = 0;
            this.BtnEMVDataSetup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnEMVDataSetup.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(166)), true);
            this.BtnEMVDataSetup.Location = new System.Drawing.Point(8, 22);
            this.BtnEMVDataSetup.Name = "BtnEMVDataSetup";
            this.BtnEMVDataSetup.Size = new System.Drawing.Size(81, 41);
            this.BtnEMVDataSetup.TabIndex = 46;
            this.BtnEMVDataSetup.Text = "EMV Data Setup";
            this.BtnEMVDataSetup.UseVisualStyleBackColor = false;
            this.BtnEMVDataSetup.Click += new System.EventHandler(this.BtnEMVDataSetup_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.groupBox1.Controls.Add(this.LbMonitor);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(2, 39);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(208, 331);
            this.groupBox1.TabIndex = 115;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Monitor";
            // 
            // LbMonitor
            // 
            this.LbMonitor.BackColor = System.Drawing.Color.White;
            this.LbMonitor.Font = new System.Drawing.Font("新細明體", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LbMonitor.Location = new System.Drawing.Point(6, 16);
            this.LbMonitor.Name = "LbMonitor";
            this.LbMonitor.ReadOnly = true;
            this.LbMonitor.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LbMonitor.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.LbMonitor.Size = new System.Drawing.Size(196, 305);
            this.LbMonitor.TabIndex = 1;
            this.LbMonitor.Text = "";
            // 
            // CueTxet
            // 
            this.CueTxet.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.CueTxet.Controls.Add(this.CueText);
            this.CueTxet.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.CueTxet.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CueTxet.Location = new System.Drawing.Point(2, 39);
            this.CueTxet.Name = "CueTxet";
            this.CueTxet.Size = new System.Drawing.Size(208, 129);
            this.CueTxet.TabIndex = 114;
            this.CueTxet.TabStop = false;
            this.CueTxet.Text = "CueTxet";
            this.CueTxet.Visible = false;
            // 
            // CueText
            // 
            this.CueText.BackColor = System.Drawing.Color.White;
            this.CueText.Location = new System.Drawing.Point(6, 16);
            this.CueText.Name = "CueText";
            this.CueText.ReadOnly = true;
            this.CueText.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CueText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.CueText.Size = new System.Drawing.Size(196, 107);
            this.CueText.TabIndex = 1;
            this.CueText.Text = "";
            // 
            // BtnExit
            // 
            this.BtnExit.BackColor = System.Drawing.Color.Red;
            this.BtnExit.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.BtnExit.FlatAppearance.BorderSize = 0;
            this.BtnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnExit.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)), true);
            this.BtnExit.Location = new System.Drawing.Point(657, 327);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(81, 33);
            this.BtnExit.TabIndex = 113;
            this.BtnExit.Text = "Exit";
            this.BtnExit.UseVisualStyleBackColor = false;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // BtnClear
            // 
            this.BtnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.BtnClear.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.BtnClear.FlatAppearance.BorderSize = 0;
            this.BtnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnClear.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)), true);
            this.BtnClear.Location = new System.Drawing.Point(570, 327);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(83, 33);
            this.BtnClear.TabIndex = 105;
            this.BtnClear.Text = "Clear";
            this.BtnClear.UseVisualStyleBackColor = false;
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.BtnCancel.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.BtnCancel.FlatAppearance.BorderSize = 0;
            this.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCancel.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)), true);
            this.BtnCancel.Location = new System.Drawing.Point(482, 327);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(83, 33);
            this.BtnCancel.TabIndex = 107;
            this.BtnCancel.Text = "Reset Device";
            this.BtnCancel.UseVisualStyleBackColor = false;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // DriverFrame
            // 
            this.DriverFrame.Controls.Add(this.comboBox_Mode);
            this.DriverFrame.Controls.Add(this.label2);
            this.DriverFrame.Controls.Add(this.BtnMainCloseCOM);
            this.DriverFrame.Controls.Add(this.BtnMainOpenCOM);
            this.DriverFrame.Controls.Add(this.ParityCombo);
            this.DriverFrame.Controls.Add(this.BaudRateCombo);
            this.DriverFrame.Controls.Add(this.label6);
            this.DriverFrame.Controls.Add(this.label5);
            this.DriverFrame.Controls.Add(this.DataBitsCombo);
            this.DriverFrame.Controls.Add(this.ComPortCombo);
            this.DriverFrame.Controls.Add(this.label4);
            this.DriverFrame.Controls.Add(this.label3);
            this.DriverFrame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DriverFrame.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DriverFrame.Location = new System.Drawing.Point(217, 50);
            this.DriverFrame.Name = "DriverFrame";
            this.DriverFrame.Size = new System.Drawing.Size(327, 112);
            this.DriverFrame.TabIndex = 108;
            this.DriverFrame.TabStop = false;
            this.DriverFrame.Text = "Mode Setting";
            // 
            // comboBox_Mode
            // 
            this.comboBox_Mode.Enabled = false;
            this.comboBox_Mode.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_Mode.FormattingEnabled = true;
            this.comboBox_Mode.IntegralHeight = false;
            this.comboBox_Mode.ItemHeight = 14;
            this.comboBox_Mode.Location = new System.Drawing.Point(13, 37);
            this.comboBox_Mode.Name = "comboBox_Mode";
            this.comboBox_Mode.Size = new System.Drawing.Size(73, 22);
            this.comboBox_Mode.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 17);
            this.label2.TabIndex = 16;
            this.label2.Text = "Mode";
            // 
            // BtnMainCloseCOM
            // 
            this.BtnMainCloseCOM.BackColor = System.Drawing.Color.Green;
            this.BtnMainCloseCOM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnMainCloseCOM.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(166)));
            this.BtnMainCloseCOM.Location = new System.Drawing.Point(251, 71);
            this.BtnMainCloseCOM.Name = "BtnMainCloseCOM";
            this.BtnMainCloseCOM.Size = new System.Drawing.Size(68, 25);
            this.BtnMainCloseCOM.TabIndex = 10;
            this.BtnMainCloseCOM.Text = "Close ";
            this.BtnMainCloseCOM.UseVisualStyleBackColor = false;
            this.BtnMainCloseCOM.Click += new System.EventHandler(this.BtnMainCloseCOM_Click);
            // 
            // BtnMainOpenCOM
            // 
            this.BtnMainOpenCOM.BackColor = System.Drawing.Color.Green;
            this.BtnMainOpenCOM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnMainOpenCOM.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(166)));
            this.BtnMainOpenCOM.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BtnMainOpenCOM.Location = new System.Drawing.Point(251, 32);
            this.BtnMainOpenCOM.Name = "BtnMainOpenCOM";
            this.BtnMainOpenCOM.Size = new System.Drawing.Size(70, 25);
            this.BtnMainOpenCOM.TabIndex = 9;
            this.BtnMainOpenCOM.Text = "Open ";
            this.BtnMainOpenCOM.UseVisualStyleBackColor = false;
            this.BtnMainOpenCOM.Click += new System.EventHandler(this.BtnMainOpenCOM_Click);
            // 
            // ParityCombo
            // 
            this.ParityCombo.Enabled = false;
            this.ParityCombo.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ParityCombo.FormattingEnabled = true;
            this.ParityCombo.Location = new System.Drawing.Point(92, 80);
            this.ParityCombo.Name = "ParityCombo";
            this.ParityCombo.Size = new System.Drawing.Size(73, 22);
            this.ParityCombo.TabIndex = 8;
            // 
            // BaudRateCombo
            // 
            this.BaudRateCombo.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BaudRateCombo.FormattingEnabled = true;
            this.BaudRateCombo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BaudRateCombo.Location = new System.Drawing.Point(13, 80);
            this.BaudRateCombo.Name = "BaudRateCombo";
            this.BaudRateCombo.Size = new System.Drawing.Size(73, 22);
            this.BaudRateCombo.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(92, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 17);
            this.label6.TabIndex = 14;
            this.label6.Text = "Parity";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(13, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 17);
            this.label5.TabIndex = 13;
            this.label5.Text = "Baudrate";
            // 
            // DataBitsCombo
            // 
            this.DataBitsCombo.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DataBitsCombo.FormattingEnabled = true;
            this.DataBitsCombo.Location = new System.Drawing.Point(171, 37);
            this.DataBitsCombo.Name = "DataBitsCombo";
            this.DataBitsCombo.Size = new System.Drawing.Size(73, 22);
            this.DataBitsCombo.TabIndex = 13;
            // 
            // ComPortCombo
            // 
            this.ComPortCombo.Enabled = false;
            this.ComPortCombo.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComPortCombo.FormattingEnabled = true;
            this.ComPortCombo.IntegralHeight = false;
            this.ComPortCombo.ItemHeight = 14;
            this.ComPortCombo.Location = new System.Drawing.Point(92, 36);
            this.ComPortCombo.Name = "ComPortCombo";
            this.ComPortCombo.Size = new System.Drawing.Size(73, 22);
            this.ComPortCombo.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(171, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 17);
            this.label4.TabIndex = 12;
            this.label4.Text = "Data Length";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(92, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 17);
            this.label3.TabIndex = 11;
            this.label3.Text = "COM Port";
            // 
            // TimeOutComb
            // 
            this.TimeOutComb.Cursor = System.Windows.Forms.Cursors.Default;
            this.TimeOutComb.FormattingEnabled = true;
            this.TimeOutComb.Location = new System.Drawing.Point(105, 15);
            this.TimeOutComb.Name = "TimeOutComb";
            this.TimeOutComb.Size = new System.Drawing.Size(105, 22);
            this.TimeOutComb.TabIndex = 109;
            this.TimeOutComb.SelectedIndexChanged += new System.EventHandler(this.TimeOutComb_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(-3, 16);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(96, 14);
            this.label1.TabIndex = 106;
            this.label1.Text = "TimeOut Setting";
            // 
            // TextMark
            // 
            this.TextMark.BackColor = System.Drawing.Color.Black;
            this.TextMark.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextMark.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.TextMark.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextMark.ForeColor = System.Drawing.Color.Aqua;
            this.TextMark.Location = new System.Drawing.Point(217, 15);
            this.TextMark.Name = "TextMark";
            this.TextMark.Size = new System.Drawing.Size(521, 29);
            this.TextMark.TabIndex = 104;
            this.TextMark.TabStop = false;
            this.TextMark.Text = "Bz8  Demo Application";
            this.TextMark.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextMark.TextChanged += new System.EventHandler(this.TextMark_TextChanged);
            // 
            // Frame3
            // 
            this.Frame3.Controls.Add(this.BtnMstTest);
            this.Frame3.Controls.Add(this.BtnFunTest1);
            this.Frame3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Frame3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Frame3.Location = new System.Drawing.Point(548, 174);
            this.Frame3.Name = "Frame3";
            this.Frame3.Size = new System.Drawing.Size(198, 79);
            this.Frame3.TabIndex = 112;
            this.Frame3.TabStop = false;
            this.Frame3.Text = "Hardware Setting and Tests";
            // 
            // BtnMstTest
            // 
            this.BtnMstTest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.BtnMstTest.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.BtnMstTest.FlatAppearance.BorderSize = 0;
            this.BtnMstTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnMstTest.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(166)), true);
            this.BtnMstTest.Location = new System.Drawing.Point(96, 24);
            this.BtnMstTest.Name = "BtnMstTest";
            this.BtnMstTest.Size = new System.Drawing.Size(81, 41);
            this.BtnMstTest.TabIndex = 49;
            this.BtnMstTest.Text = "E3 Secure Card Test";
            this.BtnMstTest.UseVisualStyleBackColor = false;
            this.BtnMstTest.Click += new System.EventHandler(this.BtnMstTest_Click);
            // 
            // BtnFunTest1
            // 
            this.BtnFunTest1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.BtnFunTest1.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.BtnFunTest1.FlatAppearance.BorderSize = 0;
            this.BtnFunTest1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnFunTest1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(166)), true);
            this.BtnFunTest1.Location = new System.Drawing.Point(8, 24);
            this.BtnFunTest1.Name = "BtnFunTest1";
            this.BtnFunTest1.Size = new System.Drawing.Size(81, 41);
            this.BtnFunTest1.TabIndex = 70;
            this.BtnFunTest1.Text = "Function Test";
            this.BtnFunTest1.UseVisualStyleBackColor = false;
            // 
            // statusbar
            // 
            this.statusbar.AutoSize = false;
            this.statusbar.BackColor = System.Drawing.SystemColors.Control;
            this.statusbar.Font = new System.Drawing.Font("Arial", 8F);
            this.statusbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.statusbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel1,
            this.StatusLabel2,
            this.StatusLabel4,
            this.StatusLabel5});
            this.statusbar.Location = new System.Drawing.Point(3, 373);
            this.statusbar.Name = "statusbar";
            this.statusbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusbar.Size = new System.Drawing.Size(740, 30);
            this.statusbar.TabIndex = 103;
            this.statusbar.Text = "statusStrip1";
            // 
            // StatusLabel1
            // 
            this.StatusLabel1.AutoSize = false;
            this.StatusLabel1.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.StatusLabel1.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.StatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.StatusLabel1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.StatusLabel1.Name = "StatusLabel1";
            this.StatusLabel1.Size = new System.Drawing.Size(99, 25);
            this.StatusLabel1.Text = "STATUS";
            // 
            // StatusLabel2
            // 
            this.StatusLabel2.AutoSize = false;
            this.StatusLabel2.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.StatusLabel2.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.StatusLabel2.Name = "StatusLabel2";
            this.StatusLabel2.Size = new System.Drawing.Size(239, 25);
            this.StatusLabel2.Text = "   ";
            // 
            // StatusLabel4
            // 
            this.StatusLabel4.AutoSize = false;
            this.StatusLabel4.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.StatusLabel4.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.StatusLabel4.Name = "StatusLabel4";
            this.StatusLabel4.Size = new System.Drawing.Size(152, 25);
            this.StatusLabel4.Text = "MESSAGE";
            // 
            // StatusLabel5
            // 
            this.StatusLabel5.AutoSize = false;
            this.StatusLabel5.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.StatusLabel5.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.StatusLabel5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.StatusLabel5.Name = "StatusLabel5";
            this.StatusLabel5.Size = new System.Drawing.Size(246, 25);
            // 
            // PPDMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 413);
            this.Controls.Add(this.Frame1);
            this.Name = "PPDMain";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.PPDMain_Load);
            this.Frame1.ResumeLayout(false);
            this.Frame1.PerformLayout();
            this.Frame2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.CueTxet.ResumeLayout(false);
            this.DriverFrame.ResumeLayout(false);
            this.Frame3.ResumeLayout(false);
            this.statusbar.ResumeLayout(false);
            this.statusbar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox Frame1;
        private System.Windows.Forms.GroupBox Frame2;
        private System.Windows.Forms.Button BtnClockSetup;
        private System.Windows.Forms.Button BtnEMVTranTest;
        private System.Windows.Forms.Button BtnEMVDataSetup;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox LbMonitor;
        private System.Windows.Forms.GroupBox CueTxet;
        private System.Windows.Forms.RichTextBox CueText;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.GroupBox DriverFrame;
        private System.Windows.Forms.ComboBox comboBox_Mode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BtnMainCloseCOM;
        private System.Windows.Forms.Button BtnMainOpenCOM;
        private System.Windows.Forms.ComboBox ParityCombo;
        private System.Windows.Forms.ComboBox BaudRateCombo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox DataBitsCombo;
        private System.Windows.Forms.ComboBox ComPortCombo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox TimeOutComb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TextMark;
        private System.Windows.Forms.GroupBox Frame3;
        private System.Windows.Forms.Button BtnMstTest;
        private System.Windows.Forms.Button BtnFunTest1;
        private System.Windows.Forms.StatusStrip statusbar;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel5;
        private System.Windows.Forms.Button BtnPCDTranTest;
    }
}

