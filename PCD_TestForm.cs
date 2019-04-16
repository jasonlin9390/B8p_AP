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
    public partial class PCD_TestForm : Form
    {
        private Button[] BtnNum;

        private int point_pos;
        bool force_sign;

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

        //jack
        public PCD_TestForm()
        {
            InitializeComponent();
            BtnNum = new Button[] {
                                button0, button1, button2,
                                button3, button4, button5,
                                button6, button7, button8,
                                button9, button10, button11};
        }

        public void AutoarmInput71(UIC.ElementType type, byte[] RawTrackData)
        {
            string returned_message = null;
            Array.Clear(Module1.receive_buf, 0, Module1.receive_buf.Length);
            Module1.receive_buf = RawTrackData;
            Module1.SIOInput(0, 1, Module1.receive_buf);
            returned_message = Encoding.ASCII.GetString(Module1.receive_buf);
            if (Module1.receive_buf[3] == 0x30)
            {
                if (Module1.receive_buf[4] == 0x59)
                    Check_rsp_2(Module1.receive_buf);
                else
                    Check_rsp_3(Module1.receive_buf);
            }
            else
            {
                statusTextBox.Text = "PIN PAD FATAL ERROR";
                
            }
        }

        //jack
        private void pinPAD_No_Response()
        {
            statusTextBox.Text = "No pin PAD response";
        }
        
        //jack
        private void ResetPCD_TransactionFrame()
        {
            LabelPurchaseAmount.Text = "0";
            LabelCashbackAmount.Text = "0";
            LabelTransAmount.Text = "0";
            Combo1.SelectedIndex = 0;
            //Combo2.SelectedIndex = 0;
            Combo3.SelectedIndex = 0;
            Combo1_SelectedIndexChanged(null, EventArgs.Empty);
            Combo2_SelectedIndexChanged(null, EventArgs.Empty);
            Combo3_SelectedIndexChanged(null, EventArgs.Empty);
            PurchaseAmountLabel.Text = "0.0";
            CashBackAmountLabel.Text = "0.0";
            TotalAmountLabel.Text = "0.0";
            Combo2.Text = "CAD";
        }

        //jack
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
            filename = Environment.CurrentDirectory + @"\Config\PCD\TransactionInfo.txt";
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

        //jack
        private void Combo2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //point_pos = Convert.ToInt32(Currency_Code[Combo2.SelectedIndex].Substring(0, 1));
            //ClearAmount.PerformClick();
            //LabelPurchaseAmount.Text = "0";
            //LabelCashbackAmount.Text = "0";
            //LabelTransAmount.Text = "0";
            //PurchaseAmountLabel.Text = "0.0";
            //CashBackAmountLabel.Text = "0.0";
            //TotalAmountLabel.Text = "0.0";
            ///ResetCalculatorFrame();
            ///CalculateEnabled(true);
        }

        //jack
        private void Combo3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //jack
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

        //jack
        private void ResetCalculatorFrame()
        {
            textBox3.Text = "0";
            CalTitleLabel.Text = "Purchase Amount";
        }

        private void EMV_TransactionEnabled(bool Switch)
        {
            Combo1.Enabled = Switch;
            Combo2.Enabled = Switch;
            TransactionStart.Enabled = Switch;
            ClearAmount.Enabled = Switch;
        }

        //jack
        private void TransactionTerminate()
        {
            int i = 0;
            statusTextBox.Text = "";
            ResetPCD_TransactionFrame();
            ResetCalculatorFrame();
            EMV_TransactionEnabled(false);
            CalculateEnabled(true);
            for (i = 0; i < 40; i++)
            {
                Application.DoEvents();
                TLV.TagList[i] = "";
                TLV.ValueList[i] = "";
            }
        }

        //jack
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
            LabelEntryMode.Text = "";
            LabelSign.Visible = false;
        }

        //jack
        private void PCD_TestForm_Load(object sender, EventArgs e)
        {
            bankSideForm.Show();
            bankSideForm.BringToFront();
            Moduel2.init_str();
            Load_Trans_Curr_Account();
            TransactionTerminate();
        }

        //jack
        private void button1_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(textBox3.Text);
            if (textBox3.Text == "0") { textBox3.Text = ""; }
            textBox3.Text += ((Button)sender).Text;
        }

        //jack
        private void button10_Click(object sender, EventArgs e)
        {
            if (textBox3.Text.IndexOf(".") < 0) textBox3.Text += ".";
        }

        //jack
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

        //jack
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

        //jack
        private void ClearAmount_Click(object sender, EventArgs e)
        {
            //LabelPurchaseAmount.Text = "0";
            //LabelCashbackAmount.Text = "0";
            //LabelTransAmount.Text = "0";
            //Combo1.SelectedIndex = 0;
            //Combo2.SelectedIndex = 0;
            //Combo3.SelectedIndex = 0;
            PurchaseAmountLabel.Text = "0.0";
            CashBackAmountLabel.Text = "0.0";
            TotalAmountLabel.Text = "0.0";
            ResetCalculatorFrame();
            CalculateEnabled(true);
            CashBackAmount = false;
            //Combo2.Text = "EUR";
        }

        //jack
        private void Enter_Click(object sender, EventArgs e)
        {
            int point_index = 0, lack;

            if (!textBox3.Text.Contains('.'))
            {
                textBox3.Text = textBox3.Text + ".00";
            }
            string[] split_by_dot = textBox3.Text.Split('.');
            
            if (split_by_dot[0].Length > 10)
            {
                textBox3.Text = "0";
                return;
            }
            if (split_by_dot[1].Length < 2)
            {
                split_by_dot[1] = split_by_dot[1].PadRight(2, '0');
            }
            else if (split_by_dot[1].Length > 2)
            {
                split_by_dot[1] = split_by_dot[1].Substring(0, 2);
            }
            Debug.WriteLine(split_by_dot[0]);
            Debug.WriteLine(split_by_dot[1]);
            textBox3.Text = split_by_dot[0] + "." + split_by_dot[1];

            /*point_index = textBox3.Text.IndexOf(".");
            if (point_index == -1)
                point_index = 0;
            if (point_index == 0 && point_pos != 0)
            {
                textBox3.Text = textBox3.Text + ".";
            }

            point_index = textBox3.Text.IndexOf(".");
            lack = point_pos - (textBox3.Text.Length - (point_index + 1));
            if (lack > 0)
                textBox3.Text = textBox3.Text + "0".PadLeft(lack, '0');*/
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
            EMV_TransactionEnabled(true);
            
            textBox3.Text = "0";
        }

        //jack
        private void EMVTestForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            Module1.ppCancel();
        }

        private string TransRecordToTLV(string record)
        {
            string record_string, tmp;
            string[] TLV_format;
            byte[] TLV_format_inByte;
            byte[] temp;
            int hex_len, temp_len, i;
            if (record == null)
                return null;
            record_string = record;
            TLV_format = record_string.Split(new string[] { Module1.charStr(0x1A) }, StringSplitOptions.None);
            record_string = String.Join("", TLV_format);
            temp = Encoding.ASCII.GetBytes(record_string);
            temp_len = record_string.Length;
            hex_len = temp_len / 2 + (temp_len % 2);
            TLV_format_inByte = new byte[hex_len];
            Moduel2.AsciiStr2HexByte(temp, temp_len, ref TLV_format_inByte, hex_len);
            if (false == TLV.ParseTLV(TLV_format_inByte, hex_len))
            {
                return null;
            }
            record_string = "";
            for (i = 0; i < 40; i++)
            {
                Application.DoEvents();
                if (TLV.TagList[i] == "")
                    break;
                record_string = record_string + Module1.charStr(0x1A);
                record_string = record_string + TLV.TagList[i];
                record_string = record_string + Module1.charStr(0x1C);
                tmp = TLV.LengthList[i].ToString("X");
                if (tmp.Length < 2)
                    tmp = "0" + tmp;
                record_string = record_string + tmp.Substring(0, 2);
                record_string = record_string + Module1.charStr(0x1C);
                tmp = TLV.ValueList[i];
                record_string = record_string + tmp.Substring(0, tmp.Length - 1);
            }

            if (record_string == "")
            {
                MessageBox.Show("No record Data");
                return null;
            }
            return record_string;
        }

        private string GetTransactionResultData(string tag_list)  // send T21 request
        {
            int rtn;
            if (tag_list == null)
                return null;
            Array.Clear(Module1.receive_buf, 0, Module1.receive_buf.Length);
            Module1.SIOOutput("T63" + tag_list);
            //rtn = PPDMain.DLL.EmvGetTagValues(Encoding.ASCII.GetBytes(tag_list), out Module1.receive_buf);
            //rtn = PPDMain.DLL.Getter(PktType.STX, "T21", Encoding.ASCII.GetString(datain), out buffer);
            rtn = PPDMain.DLL.Getter(UIC.PktType.STX, "T63", "", out Module1.receive_buf);
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

            Online_Data = GetPCDRecordFromReader(0);
            if (Online_Data == "")
            {
                TransactionTerminate();
                MessageBox.Show("No Online Data");
                return;
            }

            bankSideForm.inputTextBox(Online_Data); // output Online data to bank simulator

            Online_Data = GetPCDRecordFromReader(1);
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
                bankSideForm.PCDNetVipOnlineAuthotisation();
            else if (bankSideForm.get_ExternHostSimOption(2) == true)
                bankSideForm.PCDNetMasterOnlineAuthotisation();
            else 
                // nothing
            

            while (bankSideForm.BankACK_Val() == false && bankSideForm.BankNoACK_Val() == false)
            {
                Application.DoEvents();
            }

            if (Module1.Online_Response == "")
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
            if (Module1.Online_Response == null)
            {
                Module1.Online_Response = "";
                return;
            }

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
            Module1.SIOOutput("T71" + Command_str);
            PPDMain.DLL.DataReceived = AutoarmInput71;
            rtn = PPDMain.DLL.Getter(UIC.PktType.STX, "T71", Command_str, out Module1.receive_buf);
            Module1.SIOInput(rtn, 1, Module1.receive_buf);
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
                    Module1.SIOOutput("T73" + script_string[loop_index]);
                    rtn = PPDMain.DLL.Getter(UIC.PktType.STX, "T73", script_string[loop_index], out Module1.receive_buf);
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
                    Module1.SIOOutput("T73" + script_string[loop_index]);
                    rtn = PPDMain.DLL.Getter(UIC.PktType.STX, "T73", script_string[loop_index], out Module1.receive_buf);
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
            Module1.SIOOutput("T710");
            PPDMain.DLL.DataReceived = AutoarmInput71;
            rtn = PPDMain.DLL.Getter(UIC.PktType.STX, "T71", "0", out Module1.receive_buf);
            //rtn = PPDMain.DLL.EmvOnlineProc(Encoding.ASCII.GetBytes("0"));
            if (rtn < 0)
            {
                TransactionTerminate();
                pinPAD_No_Response();
                return;
            }
            statusTextBox.Text = "Unable go online, Offline processing...";
        }

        //jack
        private void pinPAD_Error_handling(string err_message)
        {
            statusTextBox.Text = "Error, returned message : " + err_message;
        }

        private void Check_rsp_2(byte[] input)
        {
            switch (input[5])
            {
                case 0x31:
                    statusTextBox.Text = "Offline Approved";
                    break;
                case 0x32:
                    statusTextBox.Text = "Card Referral Approve";
                    break;
                case 0x33:
                    statusTextBox.Text = "Unable Online, Offline Approved";
                    break;
                case 0x34:
                    statusTextBox.Text = "Online Approved";
                    break;
            }
        }

        private void Check_rsp_3(byte[] input)
        {
            switch (input[5])
            {
                case 0x31:
                    statusTextBox.Text = "Offline Declined";
                    break;
                case 0x32:
                    statusTextBox.Text = "Card Referral Declined";
                    break;
                case 0x33:
                    statusTextBox.Text = "Unable Online, Offline Declined";
                    break;
                case 0x34:
                    statusTextBox.Text = "Online Declined";
                    break;
            }
        }

        // Online Authorize Requesting...
        private void rsp_0A1(ref string[] Online_Response_Data)
        {
            bankSideForm.CleanTextBox();
            Unable_Online_unchecked(ref Online_Response_Data);
        }
        
        private void get_trans_start_rsp(ref string Transaction_response)
        {
            string Command_str = null;
            string[] TextAmount = null;
            string[] TextAmountOther = null;
            string TransactionAmount = null;
            string AmountOther = null;
            int rtn = 0;
            string EMV_OnlinePIN_Key = null;

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

            //bankSideForm.PCD_Currency_Code = Currency_Code[Combo2.SelectedIndex].Substring(0, 2);
            bankSideForm.Active_Currency_Code = Currency_Code[Combo2.SelectedIndex].Substring(0, 3);

            Command_str = Module1.charStr(0x1A) + TransactionAmount + Module1.charStr(0x1A);

            if (CkbAmountOther.Checked == true)
                Command_str += AmountOther;

            Command_str += Module1.charStr(0x1A) +
                Currency_Code[Combo2.SelectedIndex] + Module1.charStr(0x1A) + Transaction_Type[Combo1.SelectedIndex] + Module1.charStr(0x1A)
                + Transaction_Information[Combo1.SelectedIndex] + Module1.charStr(0x1A) + Account_Type[Combo3.SelectedIndex];

            statusTextBox.Text = "Transaction Start, processing...";

            // Forced Online?
            if (ForceOnline.Checked == true)
                Command_str = Command_str + Module1.charStr(0x1A) + "1";
            else
                Command_str = Command_str + Module1.charStr(0x1A) + "0";

            Command_str += Module1.charStr(0x1A);

            Array.Clear(Module1.receive_buf, 0, Module1.receive_buf.Length);
            PPDMain.DLL.ReadTimeout = 50000;
            rtn = PPDMain.DLL.Getter(UIC.PktType.STX, "T61", Command_str, out Module1.receive_buf);
            Module1.SIOOutput("T61" + Command_str);
            Module1.SIOInput(rtn, 1, Module1.receive_buf);
            if (rtn < 0)
            {
                TransactionTerminate();
                pinPAD_No_Response();
                Transaction_Cancel();
                return;
            }
            PPDMain.DLL.ReadTimeout = Module1.Timeout;
            Transaction_response = Encoding.ASCII.GetString(Module1.receive_buf);
        }

        //jack
        private void TextBox3_TextChanged(object sender, System.EventArgs e)
        {
            string[] textlenth = null;
            textlenth = (textBox3.Text).Split(new string[] { "." }, StringSplitOptions.None);
            if (textlenth.Length == 2)
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
        TransationResponse:
            
            if (Transaction_response.IndexOf("T621") == 0)
            {
                TransactionTerminate();
                pinPAD_Error_handling(Transaction_response);
            }
            else if (Transaction_response.IndexOf("0Y1") == 3)
            {
                statusTextBox.Text = "Offline Approved";
            }
            else if (Transaction_response.IndexOf("0Z1") == 3)
            {
                statusTextBox.Text = "Offline Declined";
            }
            else if (Transaction_response.IndexOf("0Y3") == 3)
            {
                statusTextBox.Text = "Unable Online, Offline Approved";
            }
            else if (Transaction_response.IndexOf("0Z3") == 3)
            {
                statusTextBox.Text = "Unable Online, Offline Declined";
            }
            else if (Transaction_response.IndexOf("0Y4") == 3)
            {
                statusTextBox.Text = "Online Approved";
            }
            else if (Transaction_response.IndexOf("0Z4") == 3)
            {
                statusTextBox.Text = "Online Declined";
            }
            else if (Transaction_response.IndexOf("0Y7") == 3)
            {
                statusTextBox.Text = "Offline Approved with Signature";
                force_sign = true;
            }
            else if (Transaction_response.IndexOf("0Y8") == 3)
            {
                statusTextBox.Text = "Unable go online, Offline Approved with Signature";
                force_sign = true;
            }
            else if (Transaction_response.IndexOf("0Y9") == 3)
            {
                statusTextBox.Text = "Online Approved with Signature";
                force_sign = true;
            }
            else if (Transaction_response.IndexOf("0A1") == 3)
            {
                statusTextBox.Text = "Online Authorize Requesting...";
                rsp_0A1(ref Online_Response_Data);
                Transaction_response = Encoding.ASCII.GetString(Module1.receive_buf);
                goto TransationResponse;
            }
            else if (Transaction_response.IndexOf("0A7") == 3)
            {
                statusTextBox.Text = "Online Pin Requesting...";
                goto TransationResponse;
            }
            else if (Transaction_response.IndexOf("0B0") == 3)
            {
                statusTextBox.Text = "Try another interface...";
            }
            else
            {
                TransactionTerminate();
                pinPAD_Error_handling(Transaction_response);
            }
        }

        string GetPCDRecordFromReader(int record_type)
        {
            string Send_Tag_Type = null;
            string Command = null;
            string record_string = null, tmp;
            int rtn, temp_len, hex_len, i;
            string[] TLV_format;
            byte[] TLV_format_inByte;
            byte[] temp;
            int record_len = 0;
            byte pkt_no, total_pkt;
            string pkt_string, TLV_string;

            switch (record_type)
            {
                case 0:
                    Send_Tag_Type = "T65";
                    Command = "0";
                    break;
                case 1:
                    Send_Tag_Type = "T65";
                    Command = "1";
                    break;
            }

            Module1.SIOOutput(Send_Tag_Type + Command);

        Retry:
            Array.Clear(Module1.receive_buf, 0, Module1.receive_buf.Length);
            PPDMain.DLL.ReadTimeout = Module1.Timeout;
            rtn = PPDMain.DLL.Getter(UIC.PktType.STX, Send_Tag_Type, Command, out Module1.receive_buf);
            Module1.SIOInput(rtn, 1, Module1.receive_buf);
            if (rtn < 0)
                return null;

            if (Module1.receive_buf[3] == 'F')  // Query failed or data empty
                return null;

            pkt_no = Module1.receive_buf[3];
            total_pkt = Module1.receive_buf[4];

            pkt_string = Encoding.ASCII.GetString(Module1.receive_buf);
            pkt_string = pkt_string.Substring(6);

            //bankSideForm.inputTextBox(pkt_string);
            temp = Encoding.UTF8.GetBytes(pkt_string);

            if(total_pkt != pkt_no)
            {
                Module1.SIOOutput(Send_Tag_Type + Command + "0");
                if (PPDMain.DLL.Getter(UIC.PktType.STX, Send_Tag_Type + Command + "0" , "", out Module1.receive_buf) < 0)
                {
                    //record_string = "";
                    return null;
                }
                goto Retry;
            }

            temp_len = Encoding.ASCII.GetString(temp).Length;

            hex_len = temp_len / 2;

            //TLV_format = record_string.Split(new string[] { Module1.charStr(0x1A) }, StringSplitOptions.None);
            //Module1.EMVL2_Tag_List = TLV_format;
            //record_string = String.Join("", TLV_format);
            //temp = Encoding.ASCII.GetBytes(record_string);
            //temp_len = record_string.Length;
            //hex_len = temp_len / 2;

            TLV_format_inByte = new byte[hex_len];

            Moduel2.AsciiStr2HexByte(temp, temp_len, ref TLV_format_inByte, hex_len);

            TLV.ParseTLV(TLV_format_inByte, hex_len);

            if (Module1.EMVL2_Tag_Num == 0)
                Module1.EMVL2_Tag_List = new string[40];

            record_string = "";
            for (i = 0; i < 40; i++)
            {
                TLV_string = "";
                Application.DoEvents();
                if (TLV.TagList[i] == "")
                    break;

                record_string = record_string + Module1.charStr(0x1A);
                record_string = record_string + TLV.TagList[i];
                //For L3
                TLV_string += TLV.TagList[i].Substring(2);
                record_string = record_string + Module1.charStr(0x1C);
                //tmp = "0" + TLV.LengthList[i].ToString("X");
                //record_string = record_string + tmp.Substring(0, 2);
                record_string = record_string + Extensions.Right("0" + TLV.LengthList[i].ToString("X"), 2);
                //For L3
                TLV_string += Extensions.Right("0" + TLV.LengthList[i].ToString("X"), 2);
                record_string += Module1.charStr(0x1C);
                record_string += TLV.ValueList[i].Substring(0, TLV.ValueList[i].Length - 1);
                //For L3
                TLV_string += TLV.ValueList[i].Substring(0, TLV.ValueList[i].Length - 1);
                //record_string = record_string + tmp.Substring(0, tmp.Length - 1);
                //tmp = TLV.ValueList[i];

                //if (tmp.Length == 0)
                //    continue;
                Module1.EMVL2_Tag_List[Module1.EMVL2_Tag_Num] = TLV_string.Replace(" ", "");
                textBox1.Text += Module1.EMVL2_Tag_Num + ", " + Module1.EMVL2_Tag_List[Module1.EMVL2_Tag_Num] + "\r\n";
                Module1.EMVL2_Tag_Num++;
            }

            if (record_string == "")
            {
                //'Call TransactionTerminate
                MessageBox.Show("No record Data");
                return null;
            }

            switch (record_type)
            {
                case 0:
                    record_string = "Data record:" + record_string;
                    break;
                case 1:
                    record_string = Module1.charStr(0x1A) + "Discretionary data:" + record_string;
                    break;
            }
            
            return record_string;
        }

        //===========================================================
        private void TransactionStart_Click(object sender, EventArgs e)
        {
            string Transaction_response = null;
            byte[] rec = new byte[20];
            string[] Online_Response_Data = null;
            string Online_Data = null, cvm_res = null;
            string[] TLV_format = null;
            byte[] TLV_format_inByte, temp = null;
            int rtn, temp_len, hex_len, a = 0;
            Module1.receive_buf = new Byte[2048];
            Module1.EMVL2_Tag_Num = 0;

            TransactionStart.Enabled = false;
            ClearAmount.Enabled = false;
            EMV_TransactionEnabled(false);
            CalculateEnabled(false);
            ClearReceipt();
            receiptGroupBox.Visible = false;
            get_trans_start_rsp(ref Transaction_response);

            if (Transaction_response == null)
                return;

            parse_trans_rsp(Transaction_response, ref Online_Response_Data);

            //while ((Module1.receive_buf[0] != 0x54) || (Module1.receive_buf[1] != 0x31) || (Module1.receive_buf[2] != 0x36))
            //{
            //    Application.DoEvents();
            //}
            //MessageBox.Show(Encoding.ASCII.GetString(Module1.receive_buf));
            //if (Module1.receive_buf.Length > 6)
            //{
            //    if (module1.receive_buf[6] == 0x31)
            //    {
            //        online_data = getpcdrecordfromreader(2);
            //        if (online_data == "")
            //        {
            //            messagebox.show("reversal data");
            //            return;
            //        }
            //    }
            //}
            string Command_str = "95" + Module1.charStr(0x1A) + "5A" + Module1.charStr(0x1A) + "9F34" + Module1.charStr(0x1A) + "9B" + Module1.charStr(0x1A) + "84"
                + Module1.charStr(0x1A) + "9F39" + Module1.charStr(0x1A) + "40000057" + Module1.charStr(0x1A) + "57";
            Module1.SIOOutput("T63" + Command_str);
            Array.Clear(Module1.receive_buf, 0, Module1.receive_buf.Length);
            //rtn = PPDMain.DLL.EmvGetTagValues(Encoding.ASCII.GetBytes("95" + Module1.charStr(0x1A) + "5A" + Module1.charStr(0x1A) + "9F34" + Module1.charStr(0x1A) + "9B" + Module1.charStr(0x1A) + "84" + Module1.charStr(0x1A) + "57"), out Module1.receive_buf);
            rtn = PPDMain.DLL.Getter(UIC.PktType.STX, "T63" + Command_str, "", out Module1.receive_buf);
            //PPDMain.DLL.ReadTimeout = 5000;
            Module1.SIOInput(rtn, 1, Module1.receive_buf);
            if (rtn < 0)
            {
                TransactionTerminate();
                pinPAD_No_Response();
                return;
            }
            PPDMain.DLL.ReadTimeout = Module1.Timeout;
            Transaction_response = Encoding.ASCII.GetString(Module1.receive_buf);
            TLV_format = Transaction_response.Split(new string[] { Module1.charStr(0x1A) }, StringSplitOptions.None);
            TLV_format[0] = String.Join("", TLV_format);
            TLV_format = TLV_format[0].Split(new string[] { Module1.charStr(0x1C) }, StringSplitOptions.None);
            TLV_format[0] = String.Join("", TLV_format);
            TLV_format = TLV_format[0].Split(new string[] { Module1.charStr(0x0) }, StringSplitOptions.None);
            TLV_format[0] = String.Join("", TLV_format);
            if (TLV_format[0].Length > 5)
                TLV_format[0] = TLV_format[0].Substring(5, TLV_format[0].Length - 5);
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
            LabelRes.Text = statusTextBox.Text;
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

            if (force_sign == false)
            {
                if (a <= Moduel2.CVM_Result_byte.Count())
                    LabelCVMRes.Text = Moduel2.CVM_Result[a];
            }
            else
                LabelCVMRes.Text = Moduel2.CVM_Result_byte[6];  //Signature

            LabelTVR.Text = TLV.FindTag("0x95");
            LabelEntryMode.Text = TLV.FindTag("0x9F39");
            LabelPAN.Text = TLV.FindTag("0x5A");
            if(LabelPAN.Text.Length == 0)
            {
                Transaction_response = TLV.FindTag("0x57");
                if(Transaction_response.Length != 0)
                {
                    TLV_format = Transaction_response.Split(new string[] { Module1.charStr(0x44) }, StringSplitOptions.None);
                    LabelPAN.Text = TLV_format[0];
                    Array.Clear(TLV_format, 0, TLV_format.Length);
                }
            }

            LabelAID.Text = TLV.FindTag("0x84");
            LabelDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            LabelTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
            if (LabelCVMRes.Text == "Signature")
                LabelSign.Visible = true;
            else
                LabelSign.Visible = false;

            // control transaction record
            string receipt_req = TLV.FindTag("0x40000057");
            if (receipt_req != "" && Int32.Parse(receipt_req) == 1)
            {
                receiptGroupBox.Visible = true;
            } else
            {
                receiptGroupBox.Visible = false;
            }

            //Call LogEnabled(True)  // not yet implemented.
            BtnPrintReceipt.Enabled = true;
            CalculateEnabled(true);
            EMV_TransactionEnabled(true);

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

        //jack
        private void BtnPrintReceipt_Click(object sender, EventArgs e)
        {
            /*PrintReceipt.PrintPage += PrintReceipt_PrintPage1;
            Print_Dialog.Document = PrintReceipt;
            if (Print_Dialog.ShowDialog() == DialogResult.OK)
            {
                PrintReceipt.Print();
            }*/
            if (receiptGroupBox.Visible == true)
            {
                receiptGroupBox.Visible = false;
                BtnPrintReceipt.Text = "Display Transaction Record";
            } else
            {
                receiptGroupBox.Visible = true;
                BtnPrintReceipt.Text = "Close Transaction Record";
            }
        }

        //jack
        private void PrintReceipt_PrintPage1(object sender, PrintPageEventArgs e)
        {
            Graphics pg = e.Graphics;
            Font pf = new Font(receiptGroupBox.Font.Name, receiptGroupBox.Font.Size, receiptGroupBox.Font.Style);
            SolidBrush pb = new SolidBrush(Color.Black);
            Single x = PrintReceipt.DefaultPageSettings.Margins.Left;
            Single y = PrintReceipt.DefaultPageSettings.Margins.Top;
            foreach (Label txt in receiptGroupBox.Controls)
            {
                pg.DrawString(txt.Text, Font, pb, txt.Location.X, txt.Location.Y, new StringFormat());
            }
        }

        //jack
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

        //jack
        private void record_txt()
        {
            string Batch_data = null;
            string seperate = "\r\n===========================================================\r\n";
            Batch_data = GetPCDRecordFromReader(1);
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

        //jack
        private void BatchDisplay_Click(object sender, EventArgs e)
        {
            /*string record_trasaction_data = "";
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
            bsf.showBatch(record_trasaction_data);*/

            string Command_str ="";
            int rtn = 0;

            Array.Clear(Module1.receive_buf, 0, Module1.receive_buf.Length);
            PPDMain.DLL.ReadTimeout = 50000;
            rtn = PPDMain.DLL.Getter(UIC.PktType.STX, "Q1", Command_str, out Module1.receive_buf);
            Module1.SIOOutput("Q1" + Command_str);
            Module1.SIOInput(rtn, 1, Module1.receive_buf);
            if (rtn < 0)
            {
                TransactionTerminate();
                pinPAD_No_Response();
                return;
            }
            PPDMain.DLL.ReadTimeout = Module1.Timeout;
            //rtn = PPDMain.DLL.Getter(UIC.PktType.STX, "Q2", Command_str, out Module1.receive_buf);
            //Module1.SIOOutput("Q2" + Command_str);
            //Transaction_response = Encoding.ASCII.GetString(Module1.receive_buf);

        }

        private void Transaction_Cancel()
        {
            PPDMain.DLL.ReadTimeout = 100;
            int rtn = PPDMain.DLL.Getter(UIC.PktType.STX, "T6C", "", out Module1.receive_buf);
            Module1.SIOOutput("T6C");
            Module1.SIOInput(rtn, 1, Module1.receive_buf);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void CkbAmountOther_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void resultTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
