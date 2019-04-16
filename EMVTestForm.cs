using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PP791
{
    public partial class EMVTestForm : Form
    {
        private Button[] BtnNum;

        private int point_pos;

        string TagPIN;

        string TagVDSPTk2;

        string TagVdspPAN;

        string TagVdspCN;

        string TagVdspDD1;

        string TagVdspDD2;
        //string ResKSN;
        //string ResPAN;
        //string Track1Data;
        //string Track2Data;
        //string Track3Data;
        bool CashBackAmount;
        bool Select_nextAP;

        //bool MSR_Mode;
        string[] Transaction_Type = new string[20];
        string[] Transaction_Information = new string[20];
        string[] Account_Type = new string[20];
        string[] Currency_Code = new string[500];
        string TagKeySN;

        BankSideForm bankSideForm = new BankSideForm();

        PrintDocument PrintReceipt = new PrintDocument(); 

        PrintDialog Print_Dialog = new PrintDialog();

        public static delgatswip SwipCard_Process;

        public static FileStream fs = null;
        public EMVTestForm()
        {
            InitializeComponent();
            BtnNum = new Button[] {
                                button0, button1, button2,
                                button3, button4, button5,
                                button6, button7, button8,
                                button9, button10, button11};
        }

        public void AutoarmInput11(UIC.ElementType type, byte[] RawTrackData)
        {
            ClearAmount_Click(null, null);
            string returned_message = null;
            Array.Clear(Module1.receive_buf, 0, Module1.receive_buf.Length);
            Module1.receive_buf = RawTrackData;
            returned_message = System.Text.Encoding.ASCII.GetString(Module1.receive_buf);
            if (returned_message.Substring(0, 1) == "T")
            {
                Module1.SIOInput(0, 1, Module1.receive_buf);
            }
            promptAppSelect(returned_message);
            //ClearAmount_Click(null, null);
        }

        public void AutoarmInput17(UIC.ElementType type, byte[] RawTrackData)
        {
            string returned_message = null;
            Array.Clear(Module1.receive_buf, 0, Module1.receive_buf.Length);
            Module1.receive_buf = RawTrackData;
            Module1.SIOInput(0, 1, Module1.receive_buf);
            returned_message = System.Text.Encoding.ASCII.GetString(Module1.receive_buf);
            if (Module1.receive_buf[3] == 0x30)
            {
                if (Module1.receive_buf[4] == 0x59)
                    Check_rsp_2(Module1.receive_buf);
                else
                    Check_rsp_3(Module1.receive_buf);
            }
            else
            {
                textBox2.Text = "PIN PAD FATAL ERROR";
            }
        }

        private void ResetEMV_App_SelFrame()
        {
            Select_nextAP = false;
        }

        private void pinPAD_No_Response()
        {
            textBox2.Text = "No pin PAD response";
        }

        private void Load_Custom_Tag_In_Term_Conf()
        {
            string fname;
            string tmp = null;
            string[] temp;
            fname = System.Environment.CurrentDirectory + @"\Config\ICC\ICC_Terminal.txt";
            FileStream aFile = new FileStream(fname, FileMode.Open);
            StreamReader sr = new StreamReader(aFile);
            TagKeySN = null;
            TagPIN = null;
            TagVDSPTk2 = null;
            TagVdspPAN = null;
            TagVdspCN = null;
            TagVdspDD1 = null;
            TagVdspDD2 = null;
            while (!sr.EndOfStream && (TagKeySN == null || TagVdspPAN == null || TagVdspCN == null || TagVdspDD1 == null || TagVdspDD2 == null))
            {
                tmp = sr.ReadLine();
                temp = tmp.Split(new Char[] { ' ' });
                if (temp[0] == "50000022")          // Custom tag 50000022: DUKPT Key S/N
                {
                    if (TagKeySN == null)
                        TagKeySN = temp[2].Trim();
                }
                else if (temp[0] == "50000004")     // Custom tag 50000004: Get PIN
                {
                    if (TagPIN == null)
                        TagPIN = temp[2].Trim();
                }
                else if (temp[0] == "50000024")    // Custom tag 50000024: VDSP Encrypted TK2
                {
                    if (TagVDSPTk2 == null)
                        TagVDSPTk2 = temp[2].Trim();
                }
                else if (temp[0] == "50000027")      // Custom tag 50000027: VDSP Encrypted PAN
                {
                    if (TagVdspPAN == null)
                        TagVdspPAN = temp[2].Trim();
                }
                else if (temp[0] == "50000028")     // Custom tag 50000028: VDSP Encrypted Cardholder Name
                {
                    if (TagVdspCN == null)
                        TagVdspCN = temp[2].Trim();
                }
                else if (temp[0] == "50000029")      // Custom tag 50000029: VDSP Encrypted DD1
                {
                    if (TagVdspDD1 == null)
                        TagVdspDD1 = temp[2].Trim();
                }
                else if (temp[0] == "5000002A")     // Custom tag 5000002A: VDSP Encrypted DD2
                {
                    if (TagVdspDD2 == null)
                        TagVdspDD2 = temp[2].Trim();
                }
            }
            sr.Close();
        }

        private void ResetEMV_TransactionFrame()
        {
            LabelPurchaseAmount.Text = "0";
            LabelCashbackAmount.Text = "0";
            LabelTransAmount.Text = "0";
            Combo1.SelectedIndex = 0;
            Combo2.SelectedIndex = 0;
            Combo3.SelectedIndex = 0;
            Combo1_SelectedIndexChanged(null, EventArgs.Empty);
            Combo2_SelectedIndexChanged(null, EventArgs.Empty);
            Combo3_SelectedIndexChanged(null, EventArgs.Empty);
            PurchaseAmountLabel.Text = "0.0";
            CashBackAmountLabel.Text = "0.0";
            TotalAmountLabel.Text = "0.0";
            Combo2.Text = "EUR";
        }

        private void Load_Trans_Curr_Account()
        {
            string tmp = null;
            string[] temp;
            int index = 0;
            string filename = Environment.CurrentDirectory + @"\Config\Currency Code.txt";
            StreamReader sr = new StreamReader(filename);
            while (!sr.EndOfStream)
            {
                tmp = sr.ReadLine();
                temp = tmp.Split(new Char[] { ' ' });
                Currency_Code[index] = temp[1];
                Combo2.Items.Add(temp[0]);
                index++;
            }
            sr.Close();
            index = 0;
            filename = Environment.CurrentDirectory + @"\Config\TransactionInfo.txt";
            sr = new StreamReader(filename);
            while (!sr.EndOfStream)
            {
                tmp = sr.ReadLine();
                temp = tmp.Split(new Char[] { ' ' });
                Combo1.Items.Add(temp[2]);
                Transaction_Type[index] = temp[0];
                Transaction_Information[index] = temp[4];
                index++;
            }
            sr.Close();
            index = 0;
            filename = System.Environment.CurrentDirectory + @"\Config\AccountType.txt";
            sr = new StreamReader(filename);
            while (!sr.EndOfStream)
            {
                tmp = sr.ReadLine();
                temp = tmp.Split(new Char[] { ' ' });
                Combo3.Items.Add(temp[0]);
                Account_Type[index] = temp[1];
                index++;
            }
            sr.Close();
        }

        private void Combo2_SelectedIndexChanged(object sender, EventArgs e)
        {
            point_pos = Convert.ToInt32(Currency_Code[Combo2.SelectedIndex].Substring(0, 1));
            //ClearAmount.PerformClick();
            LabelPurchaseAmount.Text = "0";
            LabelCashbackAmount.Text = "0";
            LabelTransAmount.Text = "0";
            PurchaseAmountLabel.Text = "0.0";
            CashBackAmountLabel.Text = "0.0";
            TotalAmountLabel.Text = "0.0";
            ResetCalculatorFrame();
            CalculateEnabled(true);
        }

        private void Combo3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void CalculateEnabled(bool Switch)
        {
            int Index;
            for (Index = 0; Index < 12; Index++)
            {
                Application.DoEvents();
                BtnNum[Index].Enabled = Switch;
            }
            Enter.Enabled = Switch;
        }

        private void ResetCalculatorFrame()
        {
            textBox3.Text = "0";
            CalTitleLabel.Text = "Purchase Amount";
        }

        private void EMV_App_SelEnabled(bool Switch)
        {
            //OptionPSE.Enabled = Switch;
            //OptionAID.Enabled = Switch;
            ApplicationSelection.Enabled = Switch;
        }

        private void EMV_TransactionEnabled(bool Switch)
        {
            Combo1.Enabled = Switch;
            Combo2.Enabled = Switch;
            TransactionStart.Enabled = Switch;
            ClearAmount.Enabled = Switch;
        }

        private void promptAppSelect(string returned_message)
        {
            string tmp = null;
            if (Encoding.ASCII.GetBytes(returned_message)[0] != 0x04)
            {
                tmp = returned_message.Substring(0, 4);
                switch (tmp)
                {
                    case "T121":
                        tmp = returned_message.Substring(4, 1);
                        switch (tmp)
                        {
                            case "1":
                                textBox2.Text = "PIN PAD FATAL ERROR :" + returned_message.Substring(5, 8); //Kathy
                                break;
                            case "2":
                                textBox2.Text = "Command Format Error";
                                break;
                            case "3":
                                textBox2.Text = "Transaction Canceled";
                                break;
                            case "4":
                                textBox2.Text = "ICC error, fallback to MSR processing.";
                                Swipcard();
                                break;
                        }
                        break;
                    case "T120":
                        textBox2.Text = returned_message.Substring(4);
                        if (textBox2.Text == "")
                        {
                            TransactionTerminate();
                            textBox2.Text = "Candidate application list is empty";
                        }
                        else
                        {
                            textBox2.Text = "Selected application id : " + textBox2.Text;
                            EMV_App_SelEnabled(false);
                            textBox2.Text = textBox2.Text + "\r\n" + "ENTER AMOUNT";
                            EMV_TransactionEnabled(true);
                            CalculateEnabled(true);
                        }
                        break;
                    default:
                        TransactionTerminate();
                        break;
                }
            }
            else textBox2.Text = "No response";
        }

        private void Swipcard()
        {
            this.Close();
            E3_Form E3 = new E3_Form();
            E3.Show();
            SwipCard_Process();
            bankSideForm.Close();

        }

        private void TransactionTerminate()
        {
            int i = 0;
            textBox2.Text = "";
            ResetEMV_App_SelFrame();
            ResetEMV_TransactionFrame();
            ResetCalculatorFrame();
            EMV_App_SelEnabled(true);
            EMV_TransactionEnabled(false);
            CalculateEnabled(false);
            for (i = 0; i < 40; i++)
            {
                Application.DoEvents();
                TLV.TagList[i] = "";
                TLV.ValueList[i] = "";
            }
        }

        private void ClearReceipt()
        {
            LabelPurchaseAmount.Text = "";
            LabelCashbackAmount.Text = "";
            LabelTransAmount.Text = "";
            LabelRes.Text = "";
            LabelCVMRes.Text = "";
            LabelTVR.Text = "";
            LabelPAN.Text = "";
            LabelAID.Text = "";
            LabelDate.Text = "";
            LabelTime.Text = "";
            LabelTSI.Text = "";
            LabelSign.Visible = false;
        }

        private void ApplicationSelection_Click(object sender, EventArgs e)
        {
            int rtn = 0;
            BtnPrintReceipt.Enabled = false;
            ClearReceipt();
            if (Select_nextAP == false)
            {
                PPDMain.DLL.DataReceived = AutoarmInput11;
                TransactionTerminate();
                Module1.SIOOutput("T11");
                rtn = PPDMain.DLL.EmvTransactionInit();
                if (rtn < 0)
                {
                    TransactionTerminate();
                    pinPAD_No_Response();
                }
            }
            else
            {
                PPDMain.DLL.DataReceived = AutoarmInput11;
                Module1.SIOOutput("T13");
                rtn = PPDMain.DLL.EmvSelectNextApp();
                Select_nextAP = false;
                ApplicationSelection.Text = "Application Select";
                if (rtn < 0)
                {
                    TransactionTerminate();
                    pinPAD_No_Response();
                }
            }
        }

        private void EMVTestForm_Load(object sender, EventArgs e)
        {
            bankSideForm.Show();
            bankSideForm.BringToFront();
            Moduel2.init_str();
            Load_Trans_Curr_Account();
            Load_Custom_Tag_In_Term_Conf();
            TransactionTerminate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "0") { textBox3.Text = ""; }
            textBox3.Text += ((Button)sender).Text;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (textBox3.Text.IndexOf(".") < 0) textBox3.Text += ".";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string txt = textBox3.Text;
            if (txt.Length > 1)
            {
                textBox3.Text = txt.Substring(0, txt.Length - 1);

            }
            else
            {
                textBox3.Text = "0";
            }
            textBox3.Text = double.Parse(textBox3.Text).ToString();
        }

        private void BtnNum_Click(int Index)
        {
            if (Index != 11)
            {
                if (Index == 10)
                {
                    if (textBox3.Text.IndexOf(".") < 0)
                        textBox3.Text = textBox3.Text + ".";
                }
                else
                {
                    if (textBox3.Text == "0")
                        textBox3.Text = BtnNum[Index].Text;
                    else
                        textBox3.Text = textBox3.Text + BtnNum[Index].Text;
                }
            }
            else
            { // 處理Back鍵...長度為1則設內容為"0", 否則減少一位數

                if (textBox3.Text.Length == 1)
                    textBox3.Text = "0";
                else
                    textBox3.Text = textBox3.Text.Substring(0, textBox3.Text.Length - 1);
            }
            TransactionStart.Enabled = true;
        }

        private void Unable_Online_checked()
        {
            int rtn;
            PPDMain.DLL.DataReceived = AutoarmInput17;
            Module1.SIOOutput("T170");
            rtn = PPDMain.DLL.EmvOnlineProc(Encoding.ASCII.GetBytes("0"));
            if (rtn < 0)//20161018
            {
                TransactionTerminate();
                pinPAD_No_Response();
                return;
            }
            textBox2.Text = "Unable go online, Offline processing...";
            Array.Clear(Module1.receive_buf, 0, Module1.receive_buf.Length);
        }

        private void ClearAmount_Click(object sender, EventArgs e)
        {
            LabelPurchaseAmount.Text = "0";
            LabelCashbackAmount.Text = "0";
            LabelTransAmount.Text = "0";
            Combo1.SelectedIndex = 0;
            Combo2.SelectedIndex = 0;
            Combo3.SelectedIndex = 0;
            PurchaseAmountLabel.Text = "0.0";
            CashBackAmountLabel.Text = "0.0";
            TotalAmountLabel.Text = "0.0";
            ResetCalculatorFrame();
            CalculateEnabled(true);
            CashBackAmount = false;
            Combo2.Text = "EUR";
        }

        private void Enter_Click(object sender, EventArgs e)
        {
            int point_index = 0, lack;

            point_index = textBox3.Text.IndexOf(".");
            if (point_index == -1)
                point_index = 0;
            if (point_index == 0 && point_pos != 0)
            {
                textBox3.Text = textBox3.Text + ".";
            }

            point_index = textBox3.Text.IndexOf(".");
            lack = point_pos - (textBox3.Text.Length - (point_index + 1));
            if (lack > 0)
                textBox3.Text = textBox3.Text + "0".PadLeft(lack, '0');
            if (CalTitleLabel.Text.IndexOf("Purchase") == 0)
            {
                PurchaseAmountLabel.Text = textBox3.Text;
                TotalAmountLabel.Text = textBox3.Text;
                CalTitleLabel.Text = "CashBack Amount";
                if (CashBackAmount == false)
                    CalculateEnabled(false);
            }
            else
            {
                string total;
                CashBackAmountLabel.Text = textBox3.Text;
                total = (Convert.ToSingle(CashBackAmountLabel.Text) + Convert.ToSingle(PurchaseAmountLabel.Text)).ToString();
                point_index = total.IndexOf(".");
                if (point_index == -1)
                    point_index = 0;
                if (point_index == 0 && point_pos != 0)
                    total = total + ".";
                point_index = total.IndexOf(".");
                lack = point_pos - (total.Length - (point_index + 1));
                if (lack > 0)
                    total = total + "0".PadLeft(lack, '0');
                TotalAmountLabel.Text = total.ToString();
                CalculateEnabled(false);
            }
            textBox3.Text = "0";
        }

        private void TransactionCancel_Click(object sender, EventArgs e)
        {
            PPDMain.DLL.ReadTimeout = 2000;
            Module1.SIOOutput("T1C");
            textBox2.Text = "Transaction cancel..";
            PPDMain.DLL.EmvAbortTransaction();
            PPDMain.DLL.ReadTimeout = Module1.Timeout;
            TransactionTerminate();
        }
        private void EMVTestForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            Module1.ppCancel();
        }

        private string GetTransactionResultData(string tag_list)  // send T21 request
        {
            int rtn;
            if (tag_list == null)
                return null;
            Array.Clear(Module1.receive_buf, 0, Module1.receive_buf.Length);
            Module1.SIOOutput("T21" + tag_list);
            rtn = PPDMain.DLL.EmvGetTagValues(Encoding.ASCII.GetBytes(tag_list), out Module1.receive_buf);
            Module1.SIOInput(rtn, 1, Module1.receive_buf);
            if (rtn < 0)
                return null;
            return System.Text.Encoding.ASCII.GetString(Module1.receive_buf).Substring(3);    // remove "T22"
        }

        private void GetTransactionResultTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            string record_string, tag;
            string[] TLV_format;
            if (!sender.Equals(textBox1) || e.KeyCode != Keys.Enter)
                return;
            if (textBox1.Text == null)
                return;
            tag = textBox1.Text.Trim();
            record_string = GetTransactionResultData(tag);
            if (record_string == null)
            {
                textBox1.Text = null;
                return;
            }
            TLV_format = record_string.Split('\x1C');

            if (TLV_format[0] == tag && TLV_format[1] != null && TLV_format[2] != null)
            {
                textBox1.Text = TLV_format[0] + " " + TLV_format[1] + " " + TLV_format[2];
            }
            else
                textBox1.Text = null;
        }

        private string GetDUKPTKeySN(string tag)
        {
            string record_string = null;
            string[] TLV_format;

            record_string = GetTransactionResultData(tag);
            if (record_string == null)
            {
                Debug.WriteLine("No KSN result");
                return null;
            }

            TLV_format = record_string.Split('\x1C');

            if (TLV_format[0] == tag && TLV_format[1] != null && TLV_format[2] != null)
            {
                if (Convert.ToInt32(TLV_format[1], 16) == 10 && TLV_format[2].Length == 20)
                    return TLV_format[2];
            }
            Debug.WriteLine("Get KSN failed");
            return null;
        }

        private string Get_TLV_Tag_Value(string Tag)
        {
            string record;
            string strRtn = null;
            string[] TLV_format;

            record = GetTransactionResultData(Tag);
            if (record == null)
                return null;

            TLV_format = record.Split('\x1C');
            if (TLV_format[0] == Tag && TLV_format[1] != null && TLV_format[2] != null)
            {
                strRtn = TLV_format[2];
            }
            return strRtn;
        }

        private void Unable_Online_unchecked(ref string[] Online_Response_Data)
        {
            string Online_Data = null;
            int rtn = -1;
            Online_Data = GetRecordFromPINpad(1); //j+
            if (Online_Data == "")
            {
                TransactionTerminate();
                MessageBox.Show("No Online Data");
                return;
            }

            bankSideForm.inputTextBox(Online_Data); // output Online data to bank simulator

            bankSideForm.Chk_ExternHostSim_Click();
            bankSideForm.Enable_Btn();
            bankSideForm.BringToFront();

            
            if (bankSideForm.get_ExternHostSimOption(1) == true)
                bankSideForm.NetVipOnlineAuthotisation();
            else if (bankSideForm.get_ExternHostSimOption(2) == true)
                bankSideForm.NetMasterOnlineAuthotisation();
            else 
                // nothing
            

            while (bankSideForm.BankACK_Val() == false && bankSideForm.BankNoACK_Val() == false)
            {
                Application.DoEvents();
            }

            if (Module1.Online_Response == "" || Module1.Online_Response == null)
            {
                noOnlineResponse();
            }
            else
            {
                OnlineResponse(ref Online_Response_Data);
            }
        }

        private void OnlineResponse(ref string[] Online_Response_Data)
        {
            string Command_str;
            int rtn;
            Online_Response_Data = Module1.Online_Response.Split(new string[] { Module1.charStr(0x1A) }, StringSplitOptions.None);
            bankSideForm.ARC_Code = Online_Response_Data[0];
            bankSideForm.IAD_Code = Online_Response_Data[1];
            //bankSideForm.ARC_Code = TDES_DES.strPack(bankSideForm.ARC_Code);
            bankSideForm.ARC_Code = Module1.strPack(bankSideForm.ARC_Code);
            // Script71
            Script_71(ref Online_Response_Data);
            //// Script72
            Script_72(ref Online_Response_Data);
            Online_Response_Data = null;
            Command_str = "1" + Module1.charStr(0x1A) + bankSideForm.ARC_Code + Module1.charStr(0x1A) + bankSideForm.IAD_Code;
            Module1.SIOOutput("T17" + Command_str);
            PPDMain.DLL.DataReceived = AutoarmInput17;
            //rtn = PPDMain.DLL.Getter(UIC.PktType.STX, "T17", Command_str, out Module1.receive_buf);
            rtn = PPDMain.DLL.EmvOnlineProc(Encoding.ASCII.GetBytes(Command_str));
            if (rtn < 0)
            {
                TransactionTerminate();
                pinPAD_No_Response();
                return;
            }
        }

        private void Script_72(ref string[] Online_Response_Data)
        {
            string[] script_string;
            int loop_index, loop_max, rtn;

            if (!string.IsNullOrEmpty(Online_Response_Data[3]))
            {
                Online_Response_Data[3] = Online_Response_Data[3].Replace(" ", "");
                Online_Response_Data[3] = Online_Response_Data[3].Replace("\r", "");
                Online_Response_Data[3] = Online_Response_Data[3].Replace("\n", "");
                script_string = Online_Response_Data[3].Split(new string[] { Module1.charStr(0x1c) }, StringSplitOptions.None);
                //loop_max = Convert.ToInt32(script_string.GetUpperBound(0));
                loop_max = Convert.ToInt32(script_string.Length);
                for (loop_index = 0; loop_index < loop_max; loop_index++)
                {
                    Application.DoEvents();
                    Module1.SIOOutput("T19" + script_string[loop_index]);
                    rtn = PPDMain.DLL.Getter(UIC.PktType.STX, "T19", script_string[loop_index], out Module1.receive_buf);
                    Module1.SIOInput(rtn, 1, Module1.receive_buf);
                    if (rtn < 0)
                    {
                        TransactionTerminate();
                        pinPAD_No_Response();
                        script_string = null;
                        Online_Response_Data = null;
                        return;
                    }
                }
            }
            script_string = null;
        }

        private void Script_71(ref string[] Online_Response_Data)
        {
            string[] script_string;
            int loop_index, loop_max, rtn;

            if (!string.IsNullOrEmpty(Online_Response_Data[2]))
            {
                Online_Response_Data[2] = Online_Response_Data[2].Replace(" ", "");
                Online_Response_Data[2] = Online_Response_Data[2].Replace("\r", "");
                Online_Response_Data[2] = Online_Response_Data[2].Replace("\n", "");
                script_string = Online_Response_Data[2].Split(new string[] { Module1.charStr(0x1c) }, StringSplitOptions.None);
                loop_max = Convert.ToInt32(script_string.Length);
                //loop_max = Convert.ToInt32(script_string.GetUpperBound(0));
                for (loop_index = 0; loop_index < loop_max; loop_index++)
                {
                    Application.DoEvents();
                    Module1.SIOOutput("T19" + script_string[loop_index]);
                    rtn = PPDMain.DLL.Getter(UIC.PktType.STX, "T19", script_string[loop_index], out Module1.receive_buf);
                    Module1.SIOInput(rtn, 1, Module1.receive_buf);
                    if (rtn < 0)
                    {
                        TransactionTerminate();
                        pinPAD_No_Response();
                        script_string = null;
                        Online_Response_Data = null;
                        return;
                    }
                }
                script_string = null;
            }
        }

        private void noOnlineResponse()
        {
            int rtn;
            Module1.SIOOutput("T170");
            PPDMain.DLL.DataReceived = AutoarmInput17;
            rtn = PPDMain.DLL.EmvOnlineProc(Encoding.ASCII.GetBytes("0"));
            if (rtn < 0)
            {
                TransactionTerminate();
                pinPAD_No_Response();
                return;
            }
            textBox2.Text = "Unable go online, Offline processing...";
        }

        private void pinPAD_Error_handling(string err_message)
        {
            textBox2.Text = "Error, returned message : " + err_message;
        }

        private void Check_rsp_2(byte[] input)
        {
            switch (input[5])
            {
                case 0x31:
                    textBox2.Text = "Offline Approved";
                    break;
                case 0x32:
                    textBox2.Text = "Card Referral Approve";
                    break;
                case 0x33:
                    textBox2.Text = "Unable Online, Offline Approved";
                    break;
                case 0x34:
                    textBox2.Text = "Online Approved";
                    break;
            }
        }

        private void Check_rsp_3(byte[] input)
        {
            switch (input[5])
            {
                case 0x31:
                    textBox2.Text = "Offline Declined";
                    break;
                case 0x32:
                    textBox2.Text = "Card Referral Declined";
                    break;
                case 0x33:
                    textBox2.Text = "Unable Online, Offline Declined";
                    break;
                case 0x34:
                    textBox2.Text = "Online Declined";
                    break;
            }
        }

        private void rsp_0A1(ref string[] Online_Response_Data)
        {
            textBox2.Text = "Online Authorize Requesting...";
            bankSideForm.CleanTextBox();
            if (UnableOnline.Checked == true)
            {
                Unable_Online_checked();
            }
            else
            {
                Unable_Online_unchecked(ref Online_Response_Data);
            }
        }

        private void get_trans_start_rsp(ref string Transaction_response)
        {
            string Command_str = null;
            string[] TextAmount = null;
            string[] TextAmountOther = null;
            string TransactionAmount = null;
            string AmountOther = null;
            int rtn = 0;
            //string EMV_OnlinePIN_Key = null;
            TransactionAmount = "";
            AmountOther = "";
            //PIN = null;
            //PANB = null;
            // Split and Join TransactionAmount
            TextAmount = TotalAmountLabel.Text.Split(new string[] { "." }, StringSplitOptions.None);
            TransactionAmount = String.Join("", TextAmount);
            //    TransactionAmount = TextAmount(0)
            TextAmount = null;
            // Split and Join CashbackAmount
            TextAmountOther = LabelCashbackAmount.Text.Split(new string[] { "." }, StringSplitOptions.None);
            AmountOther = String.Join("", TextAmountOther);
            //    AmountOther = TextAmountOther(0)
            TextAmountOther = null;
            TransactionAmount = TransactionAmount.PadLeft(12, '0');
            AmountOther = AmountOther.PadLeft(12, '0');

            bankSideForm.Active_Currency_Code = Currency_Code[Combo2.SelectedIndex].Substring(0, 2);

            Command_str = Module1.charStr(0x1A) + TransactionAmount + Module1.charStr(0x1A) + AmountOther + Module1.charStr(0x1A) +
                Currency_Code[Combo2.SelectedIndex] + Module1.charStr(0x1A) + Transaction_Type[Combo1.SelectedIndex] + Module1.charStr(0x1A)
                + Transaction_Information[Combo1.SelectedIndex] + Module1.charStr(0x1A) + Account_Type[Combo3.SelectedIndex];
            textBox2.Text = "Transaction Start, processing...";
            // Forced Online?
            if (ForceOnline.Checked == true)
                Command_str = Command_str + Module1.charStr(0x1A) + "1";
            else
                Command_str = Command_str + Module1.charStr(0x1A) + "0";

            Command_str += Module1.charStr(0x1A) + "E0C093E6917AE3476A6448A8358CB007";

            Array.Clear(Module1.receive_buf, 0, Module1.receive_buf.Length);
            rtn = PPDMain.DLL.Getter(UIC.PktType.STX, "T15", Command_str, out Module1.receive_buf);
            Module1.SIOOutput("T15" + Command_str);
            Module1.SIOInput(rtn, 1, Module1.receive_buf);
            if (rtn < 0)
            {
                TransactionTerminate();
                pinPAD_No_Response();
            }
            Transaction_response = System.Text.Encoding.ASCII.GetString(Module1.receive_buf);
        }
        private void TextBox3_TextChanged(object sender, System.EventArgs e)
        {
            string[] textlenth = null;
            textlenth = (textBox3.Text).Split(new string[] { "." }, StringSplitOptions.None);
            if (textlenth.Length == 2)//
            {
                if (textBox3.Text.Length > 13 || textlenth[1].Length > 2)
                {
                    textBox3.Text = textBox3.Text.Remove(textBox3.Text.Length - 1, 1);
                }
            }
            else
            {
                if (textBox3.Text.Length > 10)
                {
                    textBox3.Text = textBox3.Text.Remove(textBox3.Text.Length - 1, 1);
                }
            }
        }

        private void parse_trans_rsp(string Transaction_response, ref string[] Online_Response_Data)
        {
            if (Transaction_response.IndexOf("T1F") == 0)
            {
                TransactionTerminate();
                pinPAD_Error_handling(Transaction_response);
            }
            else if (Transaction_response.IndexOf("0Y1") == 3)
            {
                textBox2.Text = "Offline Approved";
            }
            else if (Transaction_response.IndexOf("0Z1") == 3)
            {
                textBox2.Text = "Offline Declined";
            }
            else if (Transaction_response.IndexOf("0Y3") == 3)
            {
                textBox2.Text = "Unable Online, Offline Approved";
            }
            else if (Transaction_response.IndexOf("0A4") == 3)
            {
                ApplicationSelection.Text = "Select Next";
                Select_nextAP = true;
                TransactionStart.Enabled = false;
                ApplicationSelection.Enabled = true;
                textBox2.Text = "Application is blocked, Reselect";
            }
            else if (Transaction_response.IndexOf("0Z3") == 3)
            {
                textBox2.Text = "Unable Online, Offline Declined";
            }
            else if (Transaction_response.IndexOf("0A1") == 3)
            {
                rsp_0A1(ref Online_Response_Data);
            }
            else if (Transaction_response.IndexOf("0A2") == 3)
            {
                textBox2.Text = "Request referral...";
            }
            else if (Transaction_response.IndexOf("0A4") == 3)
            {
                textBox2.Text = "Application is blocked, select again";
                ApplicationSelection.Enabled = true;
                return;
            }
            else
            {
                TransactionTerminate();
                pinPAD_Error_handling(Transaction_response);
            }
        }

        //private void ExternalPinProcessing(ref string[] online_resp_data)
        //{
        //    ExtPINPadForm ExtPINPad;
        //    byte[] RcvData = new byte[64];
        //    string msg, strAccount, strKey, strAmount;
        //    int rtn;

        //    Module1.SIOOutput("7C0");
        //    rtn = uicppd.ppCryptogramForExtPINEntry(util.port, 0, RcvData);
        //    if (rtn < 0)
        //        goto err_pin_processing;
        //    module1.SIOInput(rtn, 1, RcvData);

        //    msg = System.Text.Encoding.UTF8.GetString(RcvData);
        //    strAccount = null;
        //    strKey = null;
        //    ToEPP70Msg = null;
        //    if (msg.Substring(0, 3) == "7D.")
        //    {
        //        int i = 3;
        //        char msg_c;
        //        while (i < msg.Length)
        //        {
        //            msg_c = msg[i++];
        //            if (msg_c == '\x1C')
        //                break;
        //            strAccount = strAccount + msg_c;
        //        }
        //        if (strAccount != null && (i + 32) < msg.Length)
        //        {
        //            string[] arrAmount;
        //            string strPINB;
        //            strKey = msg.Substring(i, 32);
        //            arrAmount = TotalAmountLabel.Text.Split(new string[] { "." }, StringSplitOptions.None);
        //            strAmount = String.Join("", arrAmount);
        //            ToEPP70Msg = "70." + strAccount + '\x1C' + strKey + strAmount + '\x1C' + "0";
        //            FromEPP71Msg = null;
        //            ExtPINPad = new ExtPINPadForm(ToEPP70Msg);
        //            ExtPINPad.pinPadMasterKey = new delExtPINPadSyncMasterKey(syncMasterKey);
        //            ExtPINPad.extPINPadResult = new delExtPINPadFormResult(getExtPINPadResult);
        //            ExtPINPad.ShowDialog();
        //            ExtPINPad.BringToFront();

        //            if (FromEPP71Msg == null || FromEPP71Msg.Length < 24 || FromEPP71Msg.Substring(0, 4) != "71.0" || FromEPP71Msg.Substring(6, 2) != "01")
        //            {
        //                goto err_pin_processing;
        //            }
        //            else
        //            {
        //                strPINB = FromEPP71Msg.Substring(8, 16);

        //                //string strPAN = GetTransactionResultData("5A");

        //                module1.SIOOutput("T1F0" + strPINB);
        //                if (module2.SendStxData("T1F", "0" + module1.charStr(0x1A) + strPINB) <= 0)
        //                {
        //                    TransactionTerminate();
        //                    pinPAD_No_Response();
        //                    return;
        //                }
        //                Array.Clear(module1.receive_buf, 0, module1.receive_buf.Length);
        //                rtn = module2.ReceiveData(100000, module1.receive_buf);
        //                if (rtn == 0)
        //                {
        //                    TransactionTerminate();
        //                    pinPAD_No_Response();
        //                    return;
        //                }
        //                module1.SIOInput(rtn, 1, module1.receive_buf);
        //                msg = System.Text.Encoding.UTF8.GetString(module1.receive_buf);
        //                if (msg.IndexOf("0Y1") == 3)
        //                    textBox2.Text = "Offline Approved";
        //                else if (msg.IndexOf("0Z1") == 3)
        //                    textBox2.Text = "Offline Declined";
        //                else if (msg.IndexOf("0A1") == 3)
        //                    rsp_0A1(ref online_resp_data);
        //                else
        //                {
        //                    TransactionTerminate();
        //                    pinPAD_Error_handling(msg);
        //                }
        //                return;
        //            }
        //        }   // End Of - if (strAccount != null && (i + 32) < msg.Length)
        //    }   // End of - if (msg.Substring(0, 3) == "7D.")
        //    err_pin_processing:
        //    // Send T1F + "2"
        //    module1.SIOOutput("T1F2");
        //    module2.SendStxData("T1F", "2");
        //    TransactionTerminate();
        //    pinPAD_No_Response();
        //    return;
        //}

        string GetRecordFromPINpad(int record_type)
        {
            string Send_Tag_Type = null;
            string record_string, tmp;
            int rtn, temp_len, hex_len, i;
            string[] TLV_format;
            byte[] TLV_format_inByte;
            byte[] temp;
            int record_len = 0;

            switch (record_type)
            {
                case 1:
                    Send_Tag_Type = "T27";
                    record_len = 4;
                    break;
                case 2:
                    Send_Tag_Type = "T29";
                    record_len = 4;
                    break;

                case 3:
                    Send_Tag_Type = "T25";
                    record_len = 5;
                    break;
            }
            Module1.SIOOutput(Send_Tag_Type);
            PPDMain.DLL.ReadTimeout = 5000;
            rtn = PPDMain.DLL.Getter(UIC.PktType.STX, Send_Tag_Type, "", out Module1.receive_buf);
            PPDMain.DLL.ReadTimeout = Module1.Timeout;
            Module1.SIOInput(rtn, 1, Module1.receive_buf);
            if (rtn < 0)
            {
                //'Call pinPAD_No_Response      
                return null;
            }

            record_string = System.Text.Encoding.ASCII.GetString(Module1.receive_buf);
            record_string = record_string.Substring(record_len - 1, record_string.Length - record_len + 1);

            TLV_format = record_string.Split(new string[] { Module1.charStr(0x1A) }, StringSplitOptions.None);
            Module1.EMVL2_Tag_List = TLV_format;
            record_string = String.Join("", TLV_format);
            temp = Encoding.ASCII.GetBytes(record_string);
            temp_len = record_string.Length;
            hex_len = temp_len / 2;
            TLV_format_inByte = new byte[hex_len];

            Moduel2.AsciiStr2HexByte(temp, temp_len, ref TLV_format_inByte, hex_len);
            TLV.ParseTLV(TLV_format_inByte, hex_len);
            record_string = "";
            for (i = 0; i < 40; i++)
            {
                Application.DoEvents();
                if (TLV.TagList[i] == "")
                    break;

                record_string = record_string + Module1.charStr(0x1A);
                record_string = record_string + TLV.TagList[i];
                record_string = record_string + Module1.charStr(0x1C);
                tmp = "0" + TLV.LengthList[i].ToString("X");
                record_string = record_string + tmp.Substring(0, 2);
                record_string = record_string + Module1.charStr(0x1C);
                tmp = TLV.ValueList[i];
                record_string = record_string + tmp.Substring(0, tmp.Length - 1);
            }

            if (record_string == "")
            {
                //'Call TransactionTerminate
                MessageBox.Show("No record Data");
                return null;
            }
            return record_string;
        }


        private void TransactionStart_Click(object sender, EventArgs e)
        {
            string Transaction_response = null;
            byte[] rec = new byte[20];
            string[] Online_Response_Data = null;
            string Online_Data = null, cvm_res = null;
            string[] TLV_format = null;
            byte[] TLV_format_inByte, temp = null;
            int rtn, temp_len, hex_len, a;

            Module1.EMVL2_Tag_Num = 0;
            TransactionStart.Enabled = false;
            ClearAmount.Enabled = false;
            ResetEMV_App_SelFrame();
            EMV_TransactionEnabled(false);
            CalculateEnabled(false);
            get_trans_start_rsp(ref Transaction_response);
            parse_trans_rsp(Transaction_response, ref Online_Response_Data);
            while ((Module1.receive_buf[0] != 0x54) || (Module1.receive_buf[1] != 0x31) || (Module1.receive_buf[2] != 0x36))
            {
                Application.DoEvents();
            }
            if (Module1.receive_buf.Length > 6)
            {
                if (Module1.receive_buf[6] == 0x31)
                {
                    Online_Data = GetRecordFromPINpad(2);
                    if (Online_Data == "")
                    {
                        MessageBox.Show("Reversal Data");
                        return;
                    }
                }
            }
            Module1.SIOOutput("T21" + "95" + Module1.charStr(0x1A) + "5A" + Module1.charStr(0x1A) + "9F34");
            Array.Clear(Module1.receive_buf, 0, Module1.receive_buf.Length);
            PPDMain.DLL.ReadTimeout = 5000;
            rtn = PPDMain.DLL.EmvGetTagValues(Encoding.ASCII.GetBytes("95" + Module1.charStr(0x1A) + "5A" + Module1.charStr(0x1A) + "9F34" + Module1.charStr(0x1A) + "9B" + Module1.charStr(0x1A) + "4F"), out Module1.receive_buf);
            Module1.SIOInput(rtn, 1, Module1.receive_buf);
            if (rtn < 0)
            {
                TransactionTerminate();
                pinPAD_No_Response();
                return;
            }
            //PPDMain.DLL.ReadTimeout = Module1.Timeout;
            Transaction_response = Encoding.ASCII.GetString(Module1.receive_buf);
            TLV_format = Transaction_response.Split(new string[] { Module1.charStr(0x1A) }, StringSplitOptions.None);
            TLV_format[0] = String.Join("", TLV_format);
            TLV_format = TLV_format[0].Split(new string[] { Module1.charStr(0x1C) }, StringSplitOptions.None);
            TLV_format[0] = String.Join("", TLV_format);
            TLV_format = TLV_format[0].Split(new string[] { Module1.charStr(0x0) }, StringSplitOptions.None);
            TLV_format[0] = String.Join("", TLV_format);
            TLV_format[0] = TLV_format[0].Substring(3, TLV_format[0].Length - 3);
            temp_len = TLV_format[0].Length;
            temp = new byte[temp_len];
            temp = Encoding.ASCII.GetBytes(TLV_format[0]);
            hex_len = temp_len / 2 + (temp_len % 2);
            TLV_format_inByte = new byte[hex_len];
            TLV_format = null;
            Moduel2.AsciiStr2HexByte(temp, temp_len, ref TLV_format_inByte, hex_len);
            TLV.ParseTLV(TLV_format_inByte, hex_len);
            LabelPurchaseAmount.Text = PurchaseAmountLabel.Text;
            LabelCashbackAmount.Text = LabelCashbackAmount.Text;
            LabelTransAmount.Text = TotalAmountLabel.Text;
            LabelRes.Text = textBox2.Text;
            // CVM result
            cvm_res = TLV.FindTag("0x9F34");
            if (cvm_res != "")
            {
                cvm_res = cvm_res.Substring(1, 1);
                for (a = 0; a < 7; a++)
                {
                    Application.DoEvents();
                    if (cvm_res == Moduel2.CVM_Result_byte[a])
                    {
                        break;
                    }
                }
                LabelCVMRes.Text = Moduel2.CVM_Result[a];
            }
            LabelTVR.Text = TLV.FindTag("0x95");
            LabelPAN.Text = TLV.FindTag("0x5A");
            LabelAID.Text = TLV.FindTag("0x4F");
            LabelDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            LabelTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
            LabelTSI.Text = TLV.FindTag("0x9B");
            if (LabelCVMRes.Text == "Signature")
                LabelSign.Visible = true;
            else
                LabelSign.Visible = false;
            ApplicationSelection.Enabled = true;
            //Call LogEnabled(True)  // not yet implemented.
            BtnPrintReceipt.Enabled = true;
            EMV_TransactionEnabled(false);
            //record_txt();
        }

        private void OnlineSetting_CheckedChanged(object sender, EventArgs e)
        {
            ForceOnline.Enabled = true;
            UnableOnline.Enabled = true;
            if (sender.Equals(ForceOnline))
            {
                if (ForceOnline.Checked)
                    UnableOnline.Enabled = false;
                UnableOnline.Checked = false;
            }
            else if (sender.Equals(UnableOnline))
            {
                if (UnableOnline.Checked)
                    ForceOnline.Enabled = false;
                ForceOnline.Checked = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox4.Text = DateTime.Now.ToString();
        }

        private void BtnPrintReceipt_Click(object sender, EventArgs e)
        {
            PrintReceipt.PrintPage += PrintReceipt_PrintPage1;
            Print_Dialog.Document = PrintReceipt;
            if (Print_Dialog.ShowDialog() == DialogResult.OK)
            {
                PrintReceipt.Print();
            }
        }

        private void PrintReceipt_PrintPage1(object sender, PrintPageEventArgs e)
        {
            Graphics pg = e.Graphics;
            Font pf = new Font(groupBox1.Font.Name, groupBox1.Font.Size, groupBox1.Font.Style);
            SolidBrush pb = new SolidBrush(Color.Black);
            Single x = PrintReceipt.DefaultPageSettings.Margins.Left;
            Single y = PrintReceipt.DefaultPageSettings.Margins.Top;
            foreach (Label txt in groupBox1.Controls)
            {
                pg.DrawString(txt.Text, Font, pb, txt.Location.X, txt.Location.Y, new StringFormat());
            }
        }

        private void Combo1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LabelPurchaseAmount.Text = "0";
            LabelCashbackAmount.Text = "0";
            LabelTransAmount.Text = "0";
            PurchaseAmountLabel.Text = "0.0";
            CashBackAmountLabel.Text = "0.0";
            TotalAmountLabel.Text = "0.0";
            ResetCalculatorFrame();
            CalculateEnabled(true);

            if (Combo1.Text == "CashBack")
                CashBackAmount = true;
            else
                CashBackAmount = false;
        }

        private void record_txt()
        {
            string Batch_data = null;
            string seperate = "\r\n===========================================================\r\n";
            Batch_data = GetRecordFromPINpad(3);
            if (Batch_data == null)
            {
                return;
            }
            try
            {
                if (File.Exists(Module1.path))
                {
                    fs = File.Open(Module1.path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    Byte[] info = new UTF8Encoding(true).GetBytes(seperate + Batch_data);
                    fs.Write(info, 0, info.Length);
                    fs.Close();
                }
                else
                {
                    using (fs = File.Create(Module1.path))
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes(seperate + Batch_data);
                        fs.Write(info, 0, info.Length);
                        fs.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void BatchDisplay_Click(object sender, EventArgs e)
        {
            string record_trasaction_data = "";
            BatchShowForm bsf = new BatchShowForm();
            bsf.Show();
            bsf.ClearBatch();
            if (File.Exists(Module1.path) != true)
            {
                MessageBox.Show("No More Batch Data");
                bsf.Close();
                return;
            }
            StreamReader sr = new StreamReader(Module1.path);
            record_trasaction_data = sr.ReadToEnd();
            sr.Close();
            bsf.showBatch(record_trasaction_data);

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
