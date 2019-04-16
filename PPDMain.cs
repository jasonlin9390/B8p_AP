#define USE_COLLECTION
using System;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.IO.Ports;
using System.Threading;


namespace PP791
{
    public delegate void delMainFormMonitor(string source, string content);
    //public delegate void delMainFormBtnDisable(bool swich);
    public delegate void detect_newmode();

    public partial class PPDMain : Form
    {
        public static UIC.PP791 DLL = new UIC.PP791();
        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int screensize);
        public static PP791.messages msgCollection = new PP791.messages();
        string Mode = "";

        public PPDMain()
        {
            InitializeComponent();
            Module1.Monitor = new delMainFormMonitor(monitor);
        }
        Thread t1;

        private void PPDMain_Load(object sender, EventArgs e)
        {
            int X, Y;
            X = GetSystemMetrics(Module1.SM_CXSCREEN);
            Y = GetSystemMetrics(Module1.SM_CYSCREEN);
            this.Left = X / 10;
            this.Top = Y / 10;
            Module1.port = null;
            TimeOutDefault();
            //comboBox_Mode.Items.AddRange(new string[] {"VCOM"});
            comboBox_Mode.Items.Add("VCOM");
            comboBox_Mode.Enabled = true;
            COMDefault();

            //t1.Start();
            BtnMainOpenCOM.Enabled = true;
            CueText.Text = "";
            LbMonitor.Text = "";
        }

        public void Decet_Mode()
        {
            byte[] new_mode = null;
            //if (PPDMain.DLL != null && PPDMain.DLL.IsOpen == true)
            //{
            //    PPDMain.DLL.Close();
            //    Thread.Sleep(2000);
            //}
            while (true)
            {
                if (PPDMain.DLL != null && PPDMain.DLL.IsOpen == false)
                {
                    int rtn = UIC.PP791.GetReaderPID(out new_mode);
                    if (rtn == 0)
                    {
                        if (Module1.port != null && new_mode != null && Encoding.ASCII.GetString(new_mode) != Encoding.ASCII.GetString(Module1.port))
                        {
                            comboBox_Mode.InvokeIfRequired(COMDefault);
                            BtnMainOpenCOM.InvokeIfRequired(enableACT);
                            new_mode = null;
                        }
                        else if (Module1.port == null)
                        {
                            comboBox_Mode.InvokeIfRequired(COMDefault);
                            BtnMainOpenCOM.InvokeIfRequired(enableACT);
                        }
                    }
                }
                if ((PPDMain.DLL != null) && (Module1.Mode_flag == -1 || PPDMain.DLL.IsOpen == true))
                {
                    t1.Abort();
                }
            }
        }
        private void BtnMainCloseCOM_EnabledChanged(object sender, EventArgs e)
        {
            if (BtnMainCloseCOM.Enabled == false)//status close
            {
                t1 = new Thread(Decet_Mode);
                t1.Start();
            }
        }

        private void enableACT()
        {
            BtnMainOpenCOM.Enabled = true;
        }

        void frm2_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
            Frame2.Enabled = true;
            Frame3.Enabled = true;
            DriverFrame.Enabled = true;
        }

        static public void ErrorMessage(int val)
        {
            switch (val)
            {
                case -1:
                    // MsgBox " LRC ERROR", , "Error Message"
                    _Status.message = "LRC Error";
                    break;

                case -2:
                    // MsgBox " TIME-OUT OR NOT RECEIVE <EOT>", , "Error Massage"
                    _Status.message = "Time-Out <EOT>";
                    break;

                case -3:
                    // MsgBox " NOT RECEIVE <ACK>", , "Error Massage"
                    _Status.message = "No Response <ACK>";
                    break;

                case -4:
                    // MsgBox " TIME-OUT OR NOT RECEIVE <SI> OR <STX>", , "Error Massage"
                    _Status.message = "Time-Out <SI> Or <STX>";
                    break;

                case -5:
                    //   MsgBox " RESPONSE DATA LENGTH ERROR", , "Error Massage"
                    _Status.message = "Response Data Length Error";
                    break;

                case -6:
                    //   MsgBox " INPUT DATA LENGTH ERROR", , "Error Massage"
                    _Status.message = "Input Data Length Error";
                    break;

                case -7:
                    //  MsgBox " INPUT DATA RANGE ERROR", , "Error Massage"
                    _Status.message = "Input Data Range Error";
                    break;

                case -9:
                    // MsgBox " HAVE FUNCTION IS EXECUTION", , "Error Massage"
                    _Status.message = "Have Function Is Executing";
                    break;

                case -10:
                    //  MsgBox " Mode Select Error", , "Error Massage"
                    _Status.message = "Select Mode Error";
                    break;

                case -11:
                    //  MsgBox " Response Error", , "Error Massage"
                    _Status.message = "Response Error";
                    break;

                case -100:
                    //  MsgBox " RESPONSE IS TIME-OUT OR ERROR", , "Error Massage"
                    _Status.message = "Time-Out";
                    break;
                default:
                    _Status.message = "Unknow Error";
                    break;
            }
        }

        public void showErrorStatus()
        {
            StatusLabel5.Text = _Status.message;
        }

        private void showStatus()
        {
            if (_Status.status == "Connected")
            {
                StatusLabel2.Text = ComPortCombo.Text + "," + BaudRateCombo.Text + ",None,8 " + "Connected";
            }
            else
            {
                StatusLabel2.Text = ComPortCombo.Text + "," + BaudRateCombo.Text + ",None,8 " + "Disconnected";
            }

        }

        public void DisplayTextList(object sender, EventArgs e)
        {
            int total = 0, i;
            foreach (message output in msgCollection)
            {
                LbMonitor.AppendText(output.output());
                i = msgCollection.FindIndex(output);
            }
            total = msgCollection.Total();
            if (total == 0)
                return;
            total--;
            do
            {
                msgCollection.Remove(total);
                total--;
            } while (total >= 0);
        }

        public void monitor(string source, string content)
        {
            if (source == "PC")
            {
                msgCollection.Add(new messageIn("PC  ---> PIN PAD\r\n"));
            }
            else if (source == "PINPAD")
            {
                msgCollection.Add(new messageIn("PC <---  PIN PAD\r\n"));
            }
            msgCollection.Add(new messageIn(content + "\r\n"));
            Invoke(new EventHandler(DisplayTextList));
        }

        public void COMDefault()
        {
            int rtn;
            int found = 0;
            string[] portList = null;
            BtnMainEnable(false);
            Module1.Mode_flag = 0;
            rtn = UIC.PP791.GetReaderPID(out Module1.port);
            //Mode

            comboBox_Mode.SelectedItem = 0;
            comboBox_Mode.Text = "VCOM";
            ComPortCombo.Enabled = true;
            BaudRateCombo.Enabled = true;
            StatusLabel2.Text = "Recommend Connect VCOM Mode ";

            portList = SerialPort.GetPortNames();
            if (portList == null || portList.Length == 0)
            {
                portList = new string[] { "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "COM10", "COM11", "COM12", "COM13", "COM14", "COM15", "COM16" };
            }

            ComPortCombo.Items.Clear();
            for (int i = 0; i < portList.Length; ++i)
            {
                string name = portList[i];
                ComPortCombo.Items.Add(name);
                if (name == "COM1")
                    found = i;
            }
            if (portList.Length > 0)
                ComPortCombo.SelectedIndex = found;

            //BaudRate
            string[] baudRates = {
                "1200","2400","4800","9600","19200",
                "38400","57600","115200","0"
            };
            found = 0;
            BaudRateCombo.Items.Clear();
            for (int i = 0; baudRates[i] != "0"; ++i)
            {
                BaudRateCombo.Items.Add(baudRates[i].ToString());
                if (baudRates[i] == "9600")
                    found = i;
            }
            BaudRateCombo.SelectedIndex = 3;

            //'ParityCombo
            foreach (string s in Enum.GetNames(typeof(Parity)))
            {
                ParityCombo.Items.Add(s);
            }
            ParityCombo.SelectedIndex = (int)Settings.Port.Parity;

            //'DataBitsCombo
            //DataBitsCombo.Items.Add("5");
            //DataBitsCombo.Items.Add("6");
            DataBitsCombo.Items.Add("7");
            DataBitsCombo.Items.Add("8");
            DataBitsCombo.SelectedIndex = 1;
            //Module1.Leng1 = 8;
            //Module1.Parity1 = 0; //Loadfile 設定無意義?          
        }

        private void BtnMainOpenCOM_Click(object sender, EventArgs e)
        {
            int mode_number = -1;
            PPDMain.DLL = null;
            GC.Collect();

            PPDMain.DLL = new UIC.PP791();
            PPDMain.DLL.CommMode = UIC.PP791.UsbMode.ACM;
            PPDMain.DLL.PortName = ComPortCombo.SelectedItem.ToString();
            PPDMain.DLL.BaudRate = int.Parse(BaudRateCombo.SelectedItem.ToString());
            mode_number = 0;

            PPDMain.DLL.Open();
            if (PPDMain.DLL.IsOpen == true && PPDMain.DLL.AliveTest())
            {
                string[] time_out = TimeOutComb.SelectedItem.ToString().Split('s');
                PPDMain.DLL.ReadTimeout = int.Parse(time_out[0] + "000");
                Mode = comboBox_Mode.SelectedItem.ToString();
                Module1.Mode_flag = 1;
                _Status.status = "Connected";
                StatusLabel5.Text = "Connected";
                StatusLabel5.Text = "";
                BtnMainEnable(true);

                switch (mode_number)
                {
                    case 0:
                        showStatus();
                        break;
                    case 1:
                        StatusLabel2.Text = "KBD Mode Connected";
                        break;
                    case 2:
                        StatusLabel2.Text = "MSR Mode Connected";
                        break;
                }
                return;
            }
            else
            {
                MessageBox.Show("       " + ComPortCombo.Text + " open error", "COM Port Error Message");
                goto Error;
            }
        Error:
            Module1.Mode_flag = 0;
            Mode = "";
            BtnMainCloseCOM_Click(null, null);
            _Status.status = "DisConnected";
            //_Status.message = "DisConnected";
            StatusLabel5.Text = "Open  error";
            COMDefault();
        }
        private void Btnopen_color_changed(object sender, EventArgs e)//btnopen 
        {
            if (BtnMainOpenCOM.BackColor == _Status.c_green)
            {
                BtnMainCloseCOM.BackColor = _Status.c_Transparent;
                BtnMainCloseCOM.ForeColor = _Status.c_dark_dark;
            }
            else
            {
                BtnMainCloseCOM.BackColor = _Status.c_green;
                BtnMainCloseCOM.ForeColor = _Status.c_black;
            }
        }


        private void BtnMainEnable(bool swich)
        {
            var contrl = this.Frame1.Controls;
            for (int i = 0; i < this.Frame1.Controls.Count; i++)
            {
                if (contrl[i] is GroupBox)
                {
                    foreach (var isbutton in contrl[i].Controls)
                    {
                        if (isbutton is Button)
                        {
                            ((Button)isbutton).Enabled = swich;
                        }
                    }
                }
                else if (contrl[i] is Button) { ((Button)contrl[i]).Enabled = swich; }
            }
            BtnClear.Enabled = true;
            BtnExit.Enabled = true;

            if (swich == true)//open enable=false
            {
                BtnMainOpenCOM.BackColor = _Status.c_Transparent;
                BtnMainOpenCOM.ForeColor = _Status.c_dark_dark;
                BtnMainOpenCOM.Enabled = false;
            }
            else
            {
                BtnMainOpenCOM.BackColor = _Status.c_green;
                BtnMainOpenCOM.ForeColor = _Status.c_black;
                BtnMainOpenCOM.Enabled = true;
            }
        }

        private void BtnMainCloseCOM_Click(object sender, EventArgs e)
        {
            Module1.CloseCOM();
            _Status.status = "Disconnected";
            StatusLabel5.Text = "";
            BtnMainEnable(false);
            StatusLabel2.Text = "";
        }

        private void BtnClockSetup_Click(object sender, EventArgs e)
        {
            int[] RcvData = new int[20];
            DateTime date1 = DateTime.Now;
            if (_Status.status != "Connected")
            {
                MessageBox.Show("No pin PAD response");
                return;
            }
            byte[] GetTime;
            int GetTime_rcv = DLL.GetReaderTime(out GetTime);
            string ascii_str = Encoding.ASCII.GetString(GetTime);
            string[] stime = ascii_str.Split(new char[] { '-', '.', ':' });
            ascii_str = String.Join("", stime);
            ascii_str = ascii_str.Substring(2);
            monitor("PC", "18" + ascii_str + "\n");
            int SetTime_rcv = DLL.SetReaderTime(date1);
            BtnMainCloseCOM_Click(null, null);
            if (SetTime_rcv < 0)
            {
                ErrorMessage(SetTime_rcv);
                showErrorStatus();
                monitor("PINPAD", "18F\n");
            }
            if (SetTime_rcv == 0) monitor("PINPAD", "18" + SetTime_rcv.ToString() + "\n");
            MessageBox.Show("Please Open Mode again.", "Message");
        }

        private void TimeOutComb_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] time_out = TimeOutComb.SelectedItem.ToString().Split('s');
            Module1.Timeout = int.Parse(time_out[0] + "000");
            PPDMain.DLL.ReadTimeout = Module1.Timeout;
        }

        public void TimeOutDefault()
        {
            TimeOutComb.Items.Add("30s TimeOut");
            TimeOutComb.Items.Add("60s TimeOut");
            TimeOutComb.Items.Add("90s TimeOut");
            TimeOutComb.Items.Add("120s TimeOut");
            TimeOutComb.Items.Add("150s TimeOut");
            TimeOutComb.Items.Add("180s TimeOut");
            TimeOutComb.Items.Add("210s TimeOut");
            TimeOutComb.Items.Add("240s TimeOut");
            TimeOutComb.Items.Add("270s TimeOut");
            TimeOutComb.SelectedIndex = 0;
        }
         /*
        private void BtnFunTest1_Click(object sender, EventArgs e)
        {
            if (_Status.status == "Connected")
            {
                Frame_Func_T1 FramT1 = new Frame_Func_T1();
                FramT1.FormClosed += new FormClosedEventHandler(frm2_FormClosed);
                FramT1.Show();
                Frame2.Enabled = false;
                Frame3.Enabled = false;
                DriverFrame.Enabled = false;
            }
            else
                MessageBox.Show("No pin PAD response");
        }
        */
        private void BtnEMVDataSetup_Click(object sender, EventArgs e)
        {
            if (_Status.status == "Connected")
            {
                EMV_Data_LoadForm FramEMV = new EMV_Data_LoadForm();
                FramEMV.FormClosed += new FormClosedEventHandler(frm2_FormClosed);
                FramEMV.Show();
                Frame2.Enabled = false;
                Frame3.Enabled = false;
                DriverFrame.Enabled = false;
            }
            else
                MessageBox.Show("No pin PAD response");
        }
       /*
        private void BtnEMVTranTest_Click(object sender, EventArgs e)
        {
            if (_Status.status == "Connected")
            {
                EMVTestForm FramEMVTest = new EMVTestForm();
                FramEMVTest.FormClosed += new FormClosedEventHandler(frm2_FormClosed);
                FramEMVTest.Show();
                Frame2.Enabled = false;
                Frame3.Enabled = false;
                DriverFrame.Enabled = false;
            }
            else
                MessageBox.Show("No pin PAD response");
        }
        */
        private void BtnClear_Click(object sender, EventArgs e)
        {
            LbMonitor.Text = null;
            CueText.Text = null;
        }
/*
        private void BtnMstTest_Click(object sender, EventArgs e)
        {
            if (_Status.status == "Connected")
            {
                E3_Form E3Form = new E3_Form();
                E3Form.FormClosed += new FormClosedEventHandler(frm2_FormClosed);
                E3Form.Show();
                Frame2.Enabled = false;
                Frame3.Enabled = false;
                DriverFrame.Enabled = false;
            }
            else
                MessageBox.Show("No pin PAD response");
        }
        */
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Module1.ppCancel();
            StatusLabel5.Text = "";
        }
        private void PPDMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Module1.Mode_flag = -1;
            //Module1 M1 = new Module1();
            if (File.Exists(Module1.path))
            {
                File.Delete(Module1.path);
            }

            Module1.ppCancel();
            BtnMainCloseCOM_Click(null, null);
            GC.Collect();
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox_Mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Module1.Mode_flag == 0)
            {
                if (comboBox_Mode.SelectedIndex == 0)
                {
                    ComPortCombo.Enabled = true;
                    BaudRateCombo.Enabled = true;
                }
                else
                {
                    ComPortCombo.Enabled = false;
                    BaudRateCombo.Enabled = false;
                }
            }
            else if (Module1.Mode_flag == 1 && comboBox_Mode.SelectedItem.ToString() != Mode)
            {
                int rtn = -1;
                DialogResult msg_rst;
                msg_rst = MessageBox.Show("Do you want change Mode?", "", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
                if (msg_rst == DialogResult.Yes)
                {
                    if (PPDMain.DLL.AliveTest() == false)
                    {
                        PPDMain.DLL.Open();
                    }
                    switch (comboBox_Mode.SelectedIndex)
                    {
                        case 0:
                            Module1.SIOOutput("1S0");
                            rtn = PPDMain.DLL.SetUsbMode(UIC.PP791.UsbMode.ACM);

                            break;
                        case 1:
                            Module1.SIOOutput("1S1");
                            rtn = PPDMain.DLL.SetUsbMode(UIC.PP791.UsbMode.KBD);
                            break;
                        case 2:
                            Module1.SIOOutput("1S2");
                            rtn = PPDMain.DLL.SetUsbMode(UIC.PP791.UsbMode.MSR);
                            break;
                    }
                    if (rtn == 0)
                    {
                        MessageBox.Show("Please reconnect the divice.");
                        BtnMainCloseCOM_Click(null, null);

                        monitor("PINPAD", "1S0");
                        StatusLabel2.Text = "Reconnect the device";
                    }
                    else
                    {
                        monitor("PINPAD", "1SF");
                        StatusLabel2.Text = "Error";
                    }
                }
                else return;
            }
        }

        private void BtnEMVTranTest_Click(object sender, EventArgs e)
        {
            if (_Status.status == "Connected")
            {
                EMVTestForm FramEMVTest = new EMVTestForm();
                FramEMVTest.FormClosed += new FormClosedEventHandler(frm2_FormClosed);
                FramEMVTest.Show();
                Frame2.Enabled = false;
                Frame3.Enabled = false;
                DriverFrame.Enabled = false;
            }
            else
                MessageBox.Show("No pin PAD response");
        }

        private void BtnMstTest_Click(object sender, EventArgs e)
        {
            if (_Status.status == "Connected")
            {
                E3_Form E3Form = new E3_Form();
                E3Form.FormClosed += new FormClosedEventHandler(frm2_FormClosed);
                E3Form.Show();
                Frame2.Enabled = false;
                Frame3.Enabled = false;
                DriverFrame.Enabled = false;
            }
            else
                MessageBox.Show("No pin PAD response");
        }


        private void BtnPCDTranTest_Click(object sender, EventArgs e)
        {
            if (_Status.status == "Connected")
            {
                PCD_TestForm FramEMVTest = new PCD_TestForm();
                
                FramEMVTest.FormClosed += new FormClosedEventHandler(frm2_FormClosed);
                FramEMVTest.Show();
                Frame2.Enabled = false;
                Frame3.Enabled = false;
                DriverFrame.Enabled = false;
            }
            else
                MessageBox.Show("No pin PAD response");
        }

        private void TextMark_TextChanged(object sender, EventArgs e)
        {

        }

    }

}