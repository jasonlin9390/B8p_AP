namespace PP791
{
    partial class SettingForm
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
            this.gbSetConfig = new System.Windows.Forms.GroupBox();
            this.tbFilename = new System.Windows.Forms.TextBox();
            this.btnLoadFiles = new System.Windows.Forms.Button();
            this.btnSetAll = new System.Windows.Forms.Button();
            this.gbSetTime = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lbYear = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbYear = new System.Windows.Forms.TextBox();
            this.tbMonth = new System.Windows.Forms.TextBox();
            this.tbDay = new System.Windows.Forms.TextBox();
            this.lbHour = new System.Windows.Forms.Label();
            this.lbMinute = new System.Windows.Forms.Label();
            this.lbSecond = new System.Windows.Forms.Label();
            this.tbHour = new System.Windows.Forms.TextBox();
            this.tbMinute = new System.Windows.Forms.TextBox();
            this.tbSecond = new System.Windows.Forms.TextBox();
            this.btnSetTime = new System.Windows.Forms.Button();
            this.btnGetTime = new System.Windows.Forms.Button();
            this.gbSetConfig.SuspendLayout();
            this.gbSetTime.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSetConfig
            // 
            this.gbSetConfig.Controls.Add(this.btnSetAll);
            this.gbSetConfig.Controls.Add(this.btnLoadFiles);
            this.gbSetConfig.Controls.Add(this.tbFilename);
            this.gbSetConfig.Location = new System.Drawing.Point(12, 12);
            this.gbSetConfig.Name = "gbSetConfig";
            this.gbSetConfig.Size = new System.Drawing.Size(321, 121);
            this.gbSetConfig.TabIndex = 0;
            this.gbSetConfig.TabStop = false;
            this.gbSetConfig.Text = "Configuration Setting";
            // 
            // tbFilename
            // 
            this.tbFilename.Dock = System.Windows.Forms.DockStyle.Left;
            this.tbFilename.Location = new System.Drawing.Point(3, 18);
            this.tbFilename.Name = "tbFilename";
            this.tbFilename.Size = new System.Drawing.Size(217, 22);
            this.tbFilename.TabIndex = 0;
            // 
            // btnLoadFiles
            // 
            this.btnLoadFiles.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLoadFiles.Location = new System.Drawing.Point(220, 18);
            this.btnLoadFiles.Name = "btnLoadFiles";
            this.btnLoadFiles.Size = new System.Drawing.Size(98, 22);
            this.btnLoadFiles.TabIndex = 1;
            this.btnLoadFiles.Text = "Load Files";
            this.btnLoadFiles.UseVisualStyleBackColor = true;
            // 
            // btnSetAll
            // 
            this.btnSetAll.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSetAll.Location = new System.Drawing.Point(220, 40);
            this.btnSetAll.Name = "btnSetAll";
            this.btnSetAll.Size = new System.Drawing.Size(98, 78);
            this.btnSetAll.TabIndex = 2;
            this.btnSetAll.Text = "Set All";
            this.btnSetAll.UseVisualStyleBackColor = true;
            // 
            // gbSetTime
            // 
            this.gbSetTime.Controls.Add(this.btnGetTime);
            this.gbSetTime.Controls.Add(this.btnSetTime);
            this.gbSetTime.Controls.Add(this.tableLayoutPanel1);
            this.gbSetTime.Location = new System.Drawing.Point(336, 12);
            this.gbSetTime.Name = "gbSetTime";
            this.gbSetTime.Size = new System.Drawing.Size(325, 128);
            this.gbSetTime.TabIndex = 1;
            this.gbSetTime.TabStop = false;
            this.gbSetTime.Text = "Time Setting";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.3F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.3F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.4F));
            this.tableLayoutPanel1.Controls.Add(this.tbSecond, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.tbMinute, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.tbHour, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lbSecond, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.lbMinute, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lbHour, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tbDay, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbMonth, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbYear, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbYear, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 18);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(233, 107);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lbYear
            // 
            this.lbYear.AutoSize = true;
            this.lbYear.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbYear.Location = new System.Drawing.Point(3, 14);
            this.lbYear.Name = "lbYear";
            this.lbYear.Size = new System.Drawing.Size(71, 12);
            this.lbYear.TabIndex = 0;
            this.lbYear.Text = "Year";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Location = new System.Drawing.Point(80, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "Month";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label3.Location = new System.Drawing.Point(157, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "Day";
            // 
            // tbYear
            // 
            this.tbYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbYear.Location = new System.Drawing.Point(3, 29);
            this.tbYear.Name = "tbYear";
            this.tbYear.Size = new System.Drawing.Size(71, 22);
            this.tbYear.TabIndex = 3;
            // 
            // tbMonth
            // 
            this.tbMonth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbMonth.Location = new System.Drawing.Point(80, 29);
            this.tbMonth.Name = "tbMonth";
            this.tbMonth.Size = new System.Drawing.Size(71, 22);
            this.tbMonth.TabIndex = 4;
            // 
            // tbDay
            // 
            this.tbDay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbDay.Location = new System.Drawing.Point(157, 29);
            this.tbDay.Name = "tbDay";
            this.tbDay.Size = new System.Drawing.Size(73, 22);
            this.tbDay.TabIndex = 5;
            // 
            // lbHour
            // 
            this.lbHour.AutoSize = true;
            this.lbHour.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbHour.Location = new System.Drawing.Point(3, 66);
            this.lbHour.Name = "lbHour";
            this.lbHour.Size = new System.Drawing.Size(71, 12);
            this.lbHour.TabIndex = 6;
            this.lbHour.Text = "Hour";
            // 
            // lbMinute
            // 
            this.lbMinute.AutoSize = true;
            this.lbMinute.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbMinute.Location = new System.Drawing.Point(80, 66);
            this.lbMinute.Name = "lbMinute";
            this.lbMinute.Size = new System.Drawing.Size(71, 12);
            this.lbMinute.TabIndex = 7;
            this.lbMinute.Text = "Minute";
            // 
            // lbSecond
            // 
            this.lbSecond.AutoSize = true;
            this.lbSecond.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbSecond.Location = new System.Drawing.Point(157, 66);
            this.lbSecond.Name = "lbSecond";
            this.lbSecond.Size = new System.Drawing.Size(73, 12);
            this.lbSecond.TabIndex = 8;
            this.lbSecond.Text = "Second";
            // 
            // tbHour
            // 
            this.tbHour.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbHour.Location = new System.Drawing.Point(3, 81);
            this.tbHour.Name = "tbHour";
            this.tbHour.Size = new System.Drawing.Size(71, 22);
            this.tbHour.TabIndex = 9;
            // 
            // tbMinute
            // 
            this.tbMinute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbMinute.Location = new System.Drawing.Point(80, 81);
            this.tbMinute.Name = "tbMinute";
            this.tbMinute.Size = new System.Drawing.Size(71, 22);
            this.tbMinute.TabIndex = 10;
            // 
            // tbSecond
            // 
            this.tbSecond.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSecond.Location = new System.Drawing.Point(157, 81);
            this.tbSecond.Name = "tbSecond";
            this.tbSecond.Size = new System.Drawing.Size(73, 22);
            this.tbSecond.TabIndex = 11;
            // 
            // btnSetTime
            // 
            this.btnSetTime.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSetTime.Location = new System.Drawing.Point(236, 18);
            this.btnSetTime.Name = "btnSetTime";
            this.btnSetTime.Size = new System.Drawing.Size(86, 51);
            this.btnSetTime.TabIndex = 1;
            this.btnSetTime.Text = "Set Time";
            this.btnSetTime.UseVisualStyleBackColor = true;
            this.btnSetTime.Click += new System.EventHandler(this.btnSetTime_Click);
            // 
            // btnGetTime
            // 
            this.btnGetTime.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnGetTime.Location = new System.Drawing.Point(236, 74);
            this.btnGetTime.Name = "btnGetTime";
            this.btnGetTime.Size = new System.Drawing.Size(86, 51);
            this.btnGetTime.TabIndex = 2;
            this.btnGetTime.Text = "Get Time";
            this.btnGetTime.UseVisualStyleBackColor = true;
            this.btnGetTime.Click += new System.EventHandler(this.btnGetTime_Click);
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(949, 628);
            this.Controls.Add(this.gbSetTime);
            this.Controls.Add(this.gbSetConfig);
            this.Name = "SettingForm";
            this.Text = "SettingForm";
            this.gbSetConfig.ResumeLayout(false);
            this.gbSetConfig.PerformLayout();
            this.gbSetTime.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSetConfig;
        private System.Windows.Forms.Button btnSetAll;
        private System.Windows.Forms.Button btnLoadFiles;
        private System.Windows.Forms.TextBox tbFilename;
        private System.Windows.Forms.GroupBox gbSetTime;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbYear;
        private System.Windows.Forms.Button btnGetTime;
        private System.Windows.Forms.Button btnSetTime;
        private System.Windows.Forms.TextBox tbSecond;
        private System.Windows.Forms.TextBox tbMinute;
        private System.Windows.Forms.TextBox tbHour;
        private System.Windows.Forms.Label lbSecond;
        private System.Windows.Forms.Label lbMinute;
        private System.Windows.Forms.Label lbHour;
        private System.Windows.Forms.TextBox tbDay;
        private System.Windows.Forms.TextBox tbMonth;
        private System.Windows.Forms.TextBox tbYear;
    }
}