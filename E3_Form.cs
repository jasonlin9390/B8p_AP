using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PP791
{
    public delegate void delgatswip();
    public partial class E3_Form : Form
    {
        public E3_Form()
        {
            InitializeComponent();
            EMVTestForm.SwipCard_Process = new delgatswip(SwipCard_process);
        }
        private string filename = null;
        private Stream fileStream = null;
        //private string Open_Filter = "Cert File (*.cer)|*.cer|Cert File (*.pem)|*.pem|All files (*.*)|*.*";
        private StreamReader ReadFile(string title, string Open_Filter = "Cert File (*.cer)|*.cer|Cert File (*.pem)|*.pem|All files (*.*)|*.*")
        {
            StreamReader sr = null;
            filename = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            string currentDirectory = Environment.CurrentDirectory + @"\Config\";
            openFileDialog1.InitialDirectory = currentDirectory;
            openFileDialog1.Filter = Open_Filter;
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Title = title;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    fileStream = openFileDialog1.OpenFile();
                    filename = openFileDialog1.FileName;
                    sr = new StreamReader(fileStream);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
            return sr;
        }

        private void status_show(string str)
        {
            lbE3statustext.Text = str;
        }

        private void button1_Click(object sender, EventArgs e)//cert
        {
            status_show("");
            byte[] rcvbuff;
            string readstr = null;
            string title = "Load CA Intermediate Certification";
            string CA_Intermediate = null;
            StreamReader sr = ReadFile(title);
            //Module1.SIOOutput("E7{loading Cert. file}...");
            if (sr == null)
            {
                return;
            }
            Module1.SIOOutput("E7{loading Cert. file}...");
            status_show("Loading CA Intermediate Cert...");
            readstr = sr.ReadToEnd();
            rcvbuff = Encoding.ASCII.GetBytes(readstr);
            int rtn = PPDMain.DLL.SetCerts(1, rcvbuff);

            if (rtn == 0)
            {
                status_show("CA Intermediate Cert. loaded");
                btnLoadVendorCert.Enabled = true;
                CA_Intermediate = Module1.charStr(0x45) + Module1.charStr(0x38) + Module1.charStr(0x31);
                Module1.SIOInput(rtn, 1, Encoding.ASCII.GetBytes(CA_Intermediate));
                return;
            }
            else
            {
                goto loadfile_Fail;
            }
        loadfile_Fail:
            status_show("Cert. verification failed.");
            btnLoadVendorCert.Enabled = false;
            CA_Intermediate = Module1.charStr(0x45) + Module1.charStr(0x38) + Module1.charStr(0x30);
            Module1.SIOInput(0, 1, Encoding.ASCII.GetBytes(CA_Intermediate));
        }

        private void btnLoadVendorCert_Click(object sender, EventArgs e)//cert
        {
            status_show("");
            byte[] rcvbuff;
            string readstr = null;
            string title = "Load Vendor Certification";
            string Vendor = null;
            StreamReader sr = ReadFile(title);
            if (sr == null)
            {
                return;
            }
            Module1.SIOOutput("E7{loading Cert. file}...");
            status_show("Loading CA Intermediate Cert...");
            readstr = sr.ReadToEnd();
            sr.Close();
            rcvbuff = Encoding.ASCII.GetBytes(readstr);

            int rtn = PPDMain.DLL.SetCerts(0, rcvbuff);
            if (rtn == 0)
            {
                status_show("Vendor Cert. loaded");
                Vendor = Module1.charStr(0x45) + Module1.charStr(0x38) + Module1.charStr(0x31);
                Module1.SIOInput(rtn, 1, Encoding.ASCII.GetBytes(Vendor));
                return;
            }
            else
            {
                goto loadfile_Fail;
            }
        loadfile_Fail:
            status_show("Cert. verification failed. ");
            Vendor = Module1.charStr(0x45) + Module1.charStr(0x38) + Module1.charStr(0x30);
            Module1.SIOInput(0, 1, Encoding.ASCII.GetBytes(Vendor));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status_show("");
            string title = "Load bbParam File";
            string bbparam = null;
            ///////////Signature///////////////////

            StreamReader sr = ReadFile(title, "Signature File(*.sign)|*.sign|AllFile(*.*)|*.*");
            if (sr == null)
            {
                return;
            }
            byte[] signatureBuf = File.ReadAllBytes(filename);
            signatureBuf = Encoding.ASCII.GetBytes(Convert.ToBase64String(signatureBuf));
            ///////////DatFile///////////////////
            //Open_Filter = "Dat File(*.dat)|*.dat|AllFile(*.*)|*.*";
            sr = ReadFile(title, "Dat File(*.dat)|*.dat|AllFile(*.*)|*.*");
            if (sr == null)
            {
                return;
            }
            Module1.SIOOutput("E5{loading bbParam file}...");
            byte[] bbParamBuf = File.ReadAllBytes(filename);
            bbParamBuf = Encoding.ASCII.GetBytes(Convert.ToBase64String(bbParamBuf));
            int rtn = PPDMain.DLL.SetBbParam(signatureBuf, bbParamBuf);
            sr.Close();
            if (rtn == 0)
            {
                status_show("bbParam loaded");
                bbparam = Module1.charStr(0x45) + Module1.charStr(0x36) + Module1.charStr(0x31);
                Module1.SIOInput(rtn, 1, Encoding.ASCII.GetBytes(bbparam));
                return;
            }
            else
            {
                goto loadfile_Fail;
            }

        loadfile_Fail:
            status_show("bbParam verification failed. ");
            bbparam = Module1.charStr(0x45) + Module1.charStr(0x36) + Module1.charStr(0x30);
            Module1.SIOInput(0, 1, Encoding.ASCII.GetBytes(bbparam));
        }


        private void button4_Click(object sender, EventArgs e)//Load BIN Exclusion
        {
            status_show("");
            string BinExceptionTb = null;
            string title = "Load BIN Exclusion File";
            //Open_Filter = "Signature File(*.sign)|*.sign|AllFile(*.*)|*.*";
            StreamReader sr = ReadFile(title, "Signature File(*.sign)|*.sign|AllFile(*.*)|*.*");
            if (sr == null)
            {
                return;
            }
            byte[] load_BIN_SIGN = File.ReadAllBytes(filename);
            load_BIN_SIGN = Encoding.ASCII.GetBytes(Convert.ToBase64String(load_BIN_SIGN));
            //Open_Filter = "BIN Exclusion File(*.txt)|*.txt|AllFile(*.*)|*.*";
            sr = ReadFile(title, "BIN Exclusion File(*.txt)|*.txt|AllFile(*.*)|*.*");
            if (sr == null)
            {
                return;
            }
            Module1.SIOOutput("ED{loading BIN Exclusion Table file}...");
            byte[] load_BIN_txt = File.ReadAllBytes(filename);
            int rtn = PPDMain.DLL.SetBinExceptionTbl(load_BIN_SIGN, load_BIN_txt);
            if (rtn == 0)
            {
                status_show("BIN Exclusion Table loaded");
                BinExceptionTb = Module1.charStr(0x45) + Module1.charStr(0x45) + Module1.charStr(0x31);
                Module1.SIOInput(rtn, 1, Encoding.ASCII.GetBytes(BinExceptionTb));
                return;
            }
            else
            {
                goto loadfile_Fail;
            }
        loadfile_Fail:
            status_show("BIN Exclusion Table verification failed. ");
            BinExceptionTb = Module1.charStr(0x45) + Module1.charStr(0x45) + Module1.charStr(0x30);
            Module1.SIOInput(0, 1, Encoding.ASCII.GetBytes(BinExceptionTb));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string BinClearTb = null;
            Module1.SIOOutput("ED{clearing BIN Exclusion Table file}...");
            byte[] NULL_Byte = new byte[] { 0 };
            int rtn = PPDMain.DLL.SetBinExceptionTbl(NULL_Byte, NULL_Byte);
            if (rtn == 0)
            {
                status_show("BIN Exclusion Table Cleared");
                BinClearTb = Module1.charStr(0x45) + Module1.charStr(0x45) + Module1.charStr(0x31);
                Module1.SIOInput(rtn, 1, Encoding.ASCII.GetBytes(BinClearTb));
                return;
            }
            else
            {
                goto loadfile_Fail;

            }

        loadfile_Fail:
            status_show("BIN Table was not cleared");
            BinClearTb = Module1.charStr(0x45) + Module1.charStr(0x45) + Module1.charStr(0x30);
            Module1.SIOInput(0, 1, Encoding.ASCII.GetBytes(BinClearTb));
            return;
        }

        private void button5_Click(object sender, EventArgs e)
        {

            Module1.SIOOutput("E9." + E3_IDString.Text);
            string str_load = null;
            int rtn = PPDMain.DLL.SetIdentityStr(Encoding.ASCII.GetBytes(E3_IDString.Text));
            if (rtn == 0)
            {
                status_show("Identity String loaded");
                str_load = Module1.charStr(0x45) + Module1.charStr(0x39) + Module1.charStr(0x31);
                Module1.SIOInput(rtn, 1, Encoding.ASCII.GetBytes(str_load));

            }
            else
            {
                status_show("Identity String Loading Failed.");
                str_load = Module1.charStr(0x45) + Module1.charStr(0x39) + Module1.charStr(0x30);
                Module1.SIOInput(rtn, 1, Encoding.ASCII.GetBytes(str_load));
            }
        }
        private void SwipCard_process()
        {
            button7_Click(null, null);
        }

        private void button7_Click(object sender, EventArgs e)//btnstart
        {
            int rtn = -1;
            //btncancel.Enabled = true;
            btnstart.Enabled = false;
            PPDMain.DLL.DataEventEnabled = false;
            System.Threading.Thread.Sleep(1500);
            status_show("Swipe Your Card");
            Clear_text();
            PPDMain.DLL.DataReceived = SwipCard;
            if (Chkcontinuetest.Checked == false || Module1.port[1] == 0x5B)//KBD
            {
                Module1.SIOOutput("Q1");
                Timeout.Text = "60";
                int timeout = int.Parse(Timeout.Text);
                string time = (timeout / 100 > 0 ? "" : "0") + (timeout / 10 > 0 ? "" : "0") + timeout;
                Module1.SIOOutput("Q2");
                rtn = PPDMain.DLL.InsertCard(true, false, timeout);
                if (rtn < 0)
                {
                    status_show("Command E1 error");
                }
            }
            PPDMain.DLL.DataEventEnabled = true;
        }
        private void btnstart_EnabledChanged(object sender, System.EventArgs e)
        {
            if (btnstart.Enabled == false)
            {
                button10.Enabled = false;
                button11.Enabled = false;
                button12.Enabled = false;
                button14.Enabled = false;
            }
            else
            {
                button10.Enabled = true;
                button11.Enabled = true;
                button12.Enabled = true;
                button14.Enabled = true;
            }
        }
        private void Clear_text()
        {
            TextBox txt;
            for (int i = 8; i < 14; i++)
            {
                txt = (TextBox)this.Controls["textBox" + i.ToString()];
                txt.Text = "";
            }
        }

        private void SwipCard(UIC.ElementType type, byte[] Rcv_Buf)
        {
            UIC.TrackData MSR_Data;
            Array.Clear(Module1.receive_buf, 0, Module1.receive_buf.Length);
            Module1.receive_buf = Rcv_Buf;
            if (type != UIC.ElementType.PKT_STX)
            {
                if (type == UIC.ElementType.TRK)
                {
                    status_show("Operation Success");
                    //Module1.SIOInput(0, 1, Module1.receive_buf);
                    if (Module1.port[1] == 0x5C)//MSR Mode Auto on
                    {
                        UIC.PP791.ParseTrkData(UIC.PP791.MSRDataType.Format1, Module1.receive_buf, out MSR_Data);
                        textBox8.Text = MSR_Data.ObfuscatedTrks[0];

                        textBox9.Text = MSR_Data.ObfuscatedTrks[1];

                        textBox10.Text = MSR_Data.ObfuscatedTrks[2];

                        textBox12.Text = MSR_Data.EtbData;
                        goto show_Text;
                    }
                    else if (Module1.port[0] == 0x43)//VCOM Auto on
                    {
                        UIC.PP791.ParseTrkData(UIC.PP791.MSRDataType.Format2, Module1.receive_buf, out MSR_Data);
                        textBox8.Text = MSR_Data.ObfuscatedTrks[0];

                        textBox9.Text = MSR_Data.ObfuscatedTrks[1];

                        textBox10.Text = MSR_Data.ObfuscatedTrks[2];

                        textBox12.Text = MSR_Data.EtbData;
                        goto show_Text;
                    }
                }
                if (Module1.receive_buf[0] == 0x04)
                {
                    status_show("Operation Time Out");
                    return;
                }
                status_show("Receiving E3 / E4 error");
                return;
            }
            status_show("Operation Success");
            Module1.SIOInput(0, 1, Module1.receive_buf);
            //UIC.TrackData MSR_Data;
            UIC.PP791.ParseTrkData(UIC.PP791.MSRDataType.E3, Module1.receive_buf, out MSR_Data);
            textBox8.Text = MSR_Data.ObfuscatedTrks[0] + "\r\n" + MSR_Data.EncryptedTrks[0];

            textBox9.Text = MSR_Data.ObfuscatedTrks[1] + "\r\n" + MSR_Data.EncryptedTrks[1];

            textBox10.Text = MSR_Data.ObfuscatedTrks[2] + "\r\n" + MSR_Data.EncryptedTrks[2];

            textBox12.Text = MSR_Data.EtbData;
        show_Text:
            for (int i = 0; i < Module1.receive_buf.Length; i++)
            {
                Application.DoEvents();
                if (Module1.receive_buf[i] < 0x20)
                    if (Module1.receive_buf[i] < 0x10)
                        textBox13.Text = textBox13.Text + "<0" + Module1.receive_buf[i].ToString("X") + ">";
                    else
                        textBox13.Text = textBox13.Text + "<" + Module1.receive_buf[i].ToString("X") + ">";
                else
                    textBox13.Text = textBox13.Text + Convert.ToChar(Module1.receive_buf[i]).ToString();
            }
            if (Chkcontinuetest.Checked == true)
            {
                button7_Click(null, null);
            }
            btnstart.Enabled = true;
        }

        private void button8_Click(object sender, EventArgs e) //btncancel
        {
            PPDMain.DLL.ReadTimeout = 1000;
            Module1.SIOOutput("72");
            PPDMain.DLL.AbortTransaction();
            status_show("E3 Operation Cancelled");
            Chkcontinuetest.Checked = false;
            btnstart.Enabled = true;
            PPDMain.DLL.ReadTimeout = Module1.Timeout;
        }

        private void button14_Click(object sender, EventArgs e)//btnclear
        {
            Clear_text();
            Chkcontinuetest.Checked = false;
            status_show("E3 Ready");
            btnLoadCACert.Enabled = true;
            btnLoadVendorCert.Enabled = true;
            //E3_fileHandle = 0;
        }

        private void E3_Form_Load(object sender, EventArgs e)
        {
            Clear_text();
            Timeout.Text = "60";
            status_show("E3 Ready");
            btnLoadCACert.Enabled = true;
            btnLoadVendorCert.Enabled = true;
            Chkcontinuetest.Checked = false;

        }

        private void button13_Click(object sender, EventArgs e)
        {
            PPDMain.DLL.ReadTimeout = 1000;
            Module1.SIOOutput("72");
            PPDMain.DLL.AbortTransaction();
            Console.WriteLine("After PPDCancel");
            PPDMain.DLL.ReadTimeout = Module1.Timeout;
            this.Close();
        }

        private void button10_Click(object sender, EventArgs e)//btnCheck BBPARAM
        {
            int rtn;
            Module1.receive_buf = new byte[599];
            Array.Clear(Module1.receive_buf, 0, Module1.receive_buf.Length);
            Module1.SIOOutput("EB");
            rtn = PPDMain.DLL.GetBbParamHash(out Module1.receive_buf);
            if (rtn < 0)
            {
                status_show("Command EB error");
                return;
            }
            Module1.SIOInput(rtn, 1, Module1.receive_buf);
            string rcv_str = Encoding.ASCII.GetString(Module1.receive_buf);
            if (Module1.receive_buf[2] == 0x31)
            {
                status_show("Check digit of BBPARAM = " + rcv_str.Substring(4));
            }
            else
            {
                status_show("Cannot get check digit or BBPARAM");
            }
        }

        private void button11_Click(object sender, EventArgs e)//btnGetIDstring
        {
            int rtn;
            Module1.receive_buf = new byte[599];
            Array.Clear(Module1.receive_buf, 0, Module1.receive_buf.Length);
            Module1.SIOOutput("EA");
            rtn = PPDMain.DLL.GetIdentityStr(out Module1.receive_buf);
            if (rtn < 0)
            {
                status_show("Command EA error");
                return;
            }
            Module1.SIOInput(rtn, 1, Module1.receive_buf);
            string rcv_str = Encoding.ASCII.GetString(Module1.receive_buf);
            if (Module1.receive_buf[2] == 0x31)
            {
                status_show("ID String = " + rcv_str.Substring(4));
            }
            else
            {
                status_show("Cannot get current ID String");
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            int rtn;
            Module1.receive_buf = new byte[599];
            Array.Clear(Module1.receive_buf, 0, Module1.receive_buf.Length);
            Module1.SIOOutput("EF");
            rtn = PPDMain.DLL.GetBinExceptionTblHash(out Module1.receive_buf);
            if (rtn < 0)
            {
                status_show("Command EF error");
                return;
            }
            Module1.SIOInput(rtn, 1, Module1.receive_buf);
            string rcv_str = Encoding.ASCII.GetString(Module1.receive_buf);
            if (Module1.receive_buf[2] == 0x31)
            {
                status_show("Check digit of BIN table = " + rcv_str.Substring(4));
            }
            else
            {
                status_show("Cannot get BIN table check digit");
            }
        }

    }
}
