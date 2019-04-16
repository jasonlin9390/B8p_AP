using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;

namespace PP791
{
    public partial class BankSideForm : Form
    {
        public string ARC_Code, IAD_Code, Iss_Script1, Iss_Script2;
        string searchFor, replaceWith;
        public bool BankACK = false, BankNoACK = false;
        public int Online_Result;
        private const int TAG_DATA_TXT = 0;
        private const int TAG_DATA_BIN = 1;
        private const int TAG_DATA_TLV = 2;
        public string Active_Currency_Code;

        byte[] vip_packet_out = new byte[1024];
        int vip_out_leng;
        byte[] vip_packet_in = new byte[1024];
        int vip_in_leng;
        int vip_send_ok;
        int vip_receive_ok;
        string[] Field55_Tag;


        public BankSideForm()
        {
            InitializeComponent();
        }

        private void BankSideForm_Load(object sender, EventArgs e)
        {
            PPDE22POSEntryModeCombo.SelectedIndex = 0;
            PPDE48TCCCombo.SelectedIndex = 0;
            CheckDataElement(2, false);
            CheckDataElement(3, false);
            CheckDataElement(4, false);
            CheckDataElement(7, false);
            CheckDataElement(10, false);
            CheckDataElement(11, false);
            CheckDataElement(12, false);
            CheckDataElement(13, false);
            CheckDataElement(14, false);
            CheckDataElement(15, false);
            CheckDataElement(16, false);
            CheckDataElement(18, false);
            CheckDataElement(22, false);
            CheckDataElement(32, false);
            CheckDataElement(35, false);
            CheckDataElement(37, false);
            CheckDataElement(41, false);
            CheckDataElement(42, false);
            CheckDataElement(43, false);
            CheckDataElement(48, false);
            CheckDataElement(49, false);
            CheckDataElement(51, false);
            CheckDataElement(53, false);
            CheckDataElement(55, false);
            CheckDataElement(63, false);
            CheckDataElement(61, false);
        }

        public bool BankACK_Val()
        {
            return BankACK;
        }
        public bool BankNoACK_Val()
        {
            return BankNoACK;
        }
        //public void addDecryptionInfo(string item, string info)
        //{
        //    if (item == "EncPAN")
        //        txtEncPAN.Text = info;
        //    else if (item == "EncCN")
        //        txtEncCN.Text = info;
        //    else if (item == "EncDD1")
        //        txtEncDD1.Text = info;
        //    else if (item == "EncDD2")
        //        txtEncDD2.Text = info;
        //    else if (item == "PINBLK")
        //        txtPINBlock.Text = info;
        //    else if (item == "PAN")
        //        txtPAN.Text = info;
        //    else if (item == "CN")
        //        txtCN.Text = info;
        //    else if (item == "DD1")
        //        txtDD1.Text = info;
        //    else if (item == "DD2")
        //        txtDD2.Text = info;
        //    else if (item == "PIN")
        //        txtPIN.Text = info;
        //    else if (item == "CExp")
        //        txtCardExp.Text = info;
        //    else if (item == "FExp")
        //        txtFPEExp.Text = info;
        //}

        public void inputTextBox(string input)
        {
            Text1.Text += input;
        }

        public void CleanTextBox()
        {
            Text1.Clear();
        }

        private int strCopy(string Src, ref byte[] Dest)
        {
            //int i;

            //for (i = 1; i < Src.Length; i++)
            Dest = Encoding.ASCII.GetBytes(Src);

            //Dest[Src.Length - 1] = 0;

            return Src.Length;
        }

        private int GetTagData(byte format, string strTag, ref byte[] baData, int paddedlen = 0)
        {

            int i, compareLen;
            int outputLen = -1;
            string strTagContent;
            string strPad;
            bool serachHit;

            compareLen = strTag.Length;
            strTagContent = "";
            serachHit = false;

            for (i = 0; i < Module1.EMVL2_Tag_List.Length; i++)
            {
                if (Module1.EMVL2_Tag_List[i] != null)
                    if (strTag == Module1.EMVL2_Tag_List[i].Substring(0, compareLen))
                    {
                        serachHit = true;
                        break;
                    }
                Application.DoEvents();
            }

            if (serachHit)
            {
                switch (format)
                {
                    case TAG_DATA_TXT:
                        strTagContent = Module1.EMVL2_Tag_List[i].Substring(compareLen + 2);
                        if (paddedlen > strTagContent.Length)
                        {
                            strPad = new string('0', paddedlen);
                            strTagContent = (strPad + strTagContent).Right(paddedlen);
                        }
                        outputLen = strCopy(strTagContent, ref baData);
                        break;

                    case TAG_DATA_BIN:
                        strTagContent = Module1.EMVL2_Tag_List[i].Substring(compareLen + 2);
                        if (paddedlen > strTagContent.Length)
                        {
                            strPad = new string('0', paddedlen);
                            strTagContent = (strPad + strTagContent).Right(paddedlen);
                        }
                        outputLen = Module1.HexStrToBin(strTagContent, ref baData);
                        break;

                    case TAG_DATA_TLV:
                        strTagContent = Module1.EMVL2_Tag_List[i];
                        outputLen = Module1.HexStrToBin(strTagContent, ref baData);
                        break;
                }
            }
            else
                outputLen = -1;

            return outputLen;
        }

        private void BtnAck_Click(object sender, EventArgs e)
        {
            string KeyString, UKeyString;
            if (ARCText.Text != "")
            {
                UKeyString = ARCText.Text.ToUpper();
                KeyString = Module1.CheckStringRange(UKeyString, "0", "9", "A", "F");
                if (((KeyString.Length % 4) != 0) || (KeyString.Length != ARCText.Text.Length))
                {
                    MessageBox.Show("ARC Code Fail");
                    return;
                }
                else
                {
                    ARC_Code = KeyString;
                }
            }
            else
            {
                MessageBox.Show("ARC Data is NULL");
                return;
            }

            if (IADCheck.Checked == true)
            {
                if (IADText.Text != "")
                {
                    UKeyString = IADText.Text.ToUpper();
                    KeyString = Module1.CheckStringRange(UKeyString, "0", "9", "A", "F");
                    if (((KeyString.Length % 2) != 0) || (KeyString.Length != IADText.Text.Length))
                    {
                        MessageBox.Show("ARC Code Fail");
                        return;
                    }
                    else
                    {
                        IAD_Code = KeyString;
                    }
                }
                else
                {
                    MessageBox.Show("IAD Data is NULL");
                    return;
                }
            }

            if (IssScriptCheck1.Checked == true)
            {
                if (IssScriptText1.Text != "")
                {
                    UKeyString = IssScriptText1.Text.ToUpper();
                    KeyString = Module1.CheckStringRange(UKeyString, "0", "9", "@", "F");
                    if (KeyString.Length != IssScriptText1.Text.Length)
                    {
                        MessageBox.Show("ARC Code Fail");
                        return;
                    }
                    else
                    {
                        Iss_Script1 = KeyString.Replace("@", Module1.charStr(0x1C));

                    }
                }
                else
                {
                    MessageBox.Show("IssScript Data is NULL");
                    return;
                }
            }

            if (IssScriptCheck2.Checked == true)
            {
                if (IssScriptText2.Text != "")
                {
                    UKeyString = IssScriptText2.Text.ToUpper();
                    KeyString = Module1.CheckStringRange(UKeyString, "0", "9", "@", "F");
                    if (KeyString.Length != IssScriptText2.Text.Length)
                    {
                        MessageBox.Show("ARC Code Fail");
                        return;
                    }
                    else
                    {
                        Iss_Script2 = KeyString.Replace("@", Module1.charStr(0x1C));
                    }
                }
                else
                {
                    MessageBox.Show("IssScript Data is NULL");
                    return;
                }
            }

            BankACK = true;
            BtnAck.Enabled = false;
            BtnNoAck.Enabled = false;
            Module1.Online_Response = ARC_Code + Module1.charStr(0x1A) + IAD_Code + Module1.charStr(0x1A) +
                Iss_Script1 + Module1.charStr(0x1A) + Iss_Script2;
        }
        public void Enable_Btn()
        {
            BankACK = false;
            BankNoACK = false;
            BtnAck.Enabled = true;
            BtnNoAck.Enabled = true;
        }

        private void BtnDESelectAll_Click(object sender, EventArgs e)
        {
            foreach (Control ctrl in groupBox5.Controls)
            {
                if (ctrl.GetType().Name == "CheckBox")
                {
                    CheckBox ck = (CheckBox)ctrl;
                    ck.Checked = true;
                }
            }
        }

        private void BtnDEClearAll_Click(object sender, EventArgs e)
        {
            foreach (Control ctrl in groupBox5.Controls)
            {
                if (ctrl.GetType().Name == "CheckBox")
                {
                    CheckBox ck = (CheckBox)ctrl;
                    ck.Checked = false;
                }
            }
        }

        private void BtnNoAck_Click(object sender, EventArgs e)
        {
            BankNoACK = true;
            BtnAck.Enabled = false;
            BtnNoAck.Enabled = false;
            Module1.Online_Response = "";
        }

        private void IssScriptText1_TextChanged(object sender, EventArgs e)
        {
            IssScriptText1.Text.Replace(" ", "");
            IssScriptText1.Text.Replace("\r", "");
            IssScriptText1.Text.Replace("\n", "");
        }

        private void IssScriptText2_TextChanged(object sender, EventArgs e)
        {
            IssScriptText2.Text.Replace(" ", "");
            IssScriptText2.Text.Replace("\r", "");
            IssScriptText2.Text.Replace("\n", "");
        }
        string ReplaceMatchCase(Match m)
        {
            // Test whether the match is capitalized
            if (Char.IsUpper(m.Value[0]) == true)
            {
                // Capitalize the replacement string
                // using System.Text;
                StringBuilder sb = new StringBuilder(replaceWith);
                sb[0] = (Char.ToUpper(sb[0]));
                return sb.ToString();
            }
            else
            {
                return replaceWith;
            }
        }

        private void Text1_TextChanged(object sender, EventArgs e)
        {
            Text1.Text = Text1.Text.Replace("0x", "Tag: ");
            Text1.Text = Text1.Text.Replace(Module1.charStr(0x1C), " ");
            Text1.Text = Text1.Text.Replace(Module1.charStr(0x1A), "\r\n");
            Text1.Text = Text1.Text.Replace("\\n", "\r\n");
        }

        private void Text3_TextChanged(object sender, EventArgs e)
        {
            string s;
            s = Text1.Text;
            searchFor = "0x";
            replaceWith = "Tag: ";
            s = Regex.Replace(s, searchFor, ReplaceMatchCase, RegexOptions.IgnoreCase);
            searchFor = Module1.charStr(0x1C);
            replaceWith = " ";
            s = Regex.Replace(s, searchFor, ReplaceMatchCase, RegexOptions.IgnoreCase);
            searchFor = Module1.charStr(0x1A);
            replaceWith = "\r\n";
            s = Regex.Replace(s, searchFor, ReplaceMatchCase, RegexOptions.IgnoreCase);
            Text1.Text = s;
        }

        public void Chk_ExternHostSim_Click()
        {
            if (ExternHostSimOption0.Checked == true)
            {
                BtnAck.Enabled = true;
                BtnNoAck.Enabled = true;
                txt_extHostSimAddr.Enabled = false;
                txt_extHostSimPort.Enabled = false;
            }
            else
            {
                if (ExternHostSimOption1.Checked == true)
                    txt_extHostSimPort.Text = "5000";
                else
                    txt_extHostSimPort.Text = "6034";

                BtnAck.Enabled = false;
                BtnNoAck.Enabled = false;
                txt_extHostSimAddr.Enabled = true;
                txt_extHostSimPort.Enabled = true;
            }
        }

        private void ExternHostSimOption1_CheckedChanged(object sender, EventArgs e)
        {
            if (ExternHostSimOption1.Checked == true)
                txt_extHostSimPort.Text = "5000";
            txt_extHostSimAddr.Enabled = true;
            txt_extHostSimPort.Enabled = true;
        }

        private void ExternHostSimOption2_CheckedChanged(object sender, EventArgs e)
        {
            if (ExternHostSimOption2.Checked == true)
                txt_extHostSimPort.Text = "6034";
            txt_extHostSimAddr.Enabled = true;
            txt_extHostSimPort.Enabled = true;
        }

        public bool get_ExternHostSimOption(int option)
        {
            if (option == 1)
                return ExternHostSimOption1.Checked;
            else if (option == 2)
                return ExternHostSimOption2.Checked;
            else return false;

        }

        public void NetVipOnlineAuthotisation()
        {
            int lResult;
            int Leng;
            byte[] tag_buffer = new byte[1024];
            string strTraceNumber, strDateTime;
            int intMonth, intDay, intHour, intMin, intSec;
            byte[] srcAddr = new byte[3];
            byte[] destAddr = new byte[3];
            byte[] bitFieldIdx = new byte[129];

            string strARC, strIAD = "", strIADtmp, strIss71, strIss72, dbgVIPpacket;

            int i;

            Text1.Text += "\r\n\r\nPacking ISO8583 data...";

            Array.Clear(vip_packet_in, 0, vip_packet_in.Length);
            Array.Clear(vip_packet_out, 0, vip_packet_out.Length);

            // init bit map
            ISO8583_DLL.vip_bitmap_init();

            //According to V.I.P System BASE I Technical Spec Book 2 Chap 5.11
            //Smart card based VSDC Authorization Advice and Response should contain following fields:

            // 00: message type ("Sales" => "0100")
            Leng = strCopy("0100", ref tag_buffer);
            lResult = ISO8583_DLL.vip_add_bit_data(0, ref tag_buffer[0], Leng);

            // 02: PAN (up to 19 N, 4-bit BCD), This field is for MSR, ICC uses tag 0x57 inside field 55 to transfer Tk2 data.
            Leng = GetTagData(TAG_DATA_TXT, "57", ref tag_buffer);   //'ICC may not necessary send tag 5A, depend on tag list 4000000A's setting
            if (Leng > 0)
            {    //'Tag57 is Tk2 data, we need to trim it to get PAN
                for (i = 0; i < Leng - 1; i++)
                    if (tag_buffer[i] == Convert.ToInt32('D'))  //ASC("D")
                        Leng = i;
                lResult = ISO8583_DLL.vip_add_bit_data(2, ref tag_buffer[0], Leng);
            }

            // 03 (processing code)                6N, should be "000000" in message type 0100.
            Leng = strCopy("000000", ref tag_buffer);
            lResult = ISO8583_DLL.vip_add_bit_data(3, ref tag_buffer[0], Leng);

            // 04 (amount, transaction)            tag 9F02
            Leng = GetTagData(TAG_DATA_TXT, "9F02", ref tag_buffer);
            if (Leng > 0)
                lResult = ISO8583_DLL.vip_add_bit_data(4, ref tag_buffer[0], Leng);


            // 07 (transaction date and time)      10N, MMDDhhmmss (tag 9A + 9F21, but 9F21 is not output)
            intMonth = DateTime.Now.Month;
            intDay = DateTime.Now.Day;
            intHour = DateTime.Now.Hour;
            intMin = DateTime.Now.Minute;
            intSec = DateTime.Now.Second;
            strDateTime = Extensions.Right("00" + intMonth, 2) + Extensions.Right("00" + intDay, 2) + Extensions.Right("00" + intHour, 2) + Extensions.Right("00" + intMin, 2) + Extensions.Right("00" + intSec, 2);
            Leng = strCopy(strDateTime, ref tag_buffer);
            lResult = ISO8583_DLL.vip_add_bit_data(7, ref tag_buffer[0], Leng);

            // 11 (system trace audit number)      6N, random number
            Random random = new Random();
            strTraceNumber = random.Next(2) + 1.ToString();
            strTraceNumber = Extensions.Right("000000" + strTraceNumber, 6);
            Leng = strCopy(strTraceNumber, ref tag_buffer);
            lResult = ISO8583_DLL.vip_add_bit_data(11, ref tag_buffer[0], Leng);

            // 14  (Expiration Date)                4N, tag 5F24
            Leng = GetTagData(TAG_DATA_TXT, "5F24", ref tag_buffer);
            if (Leng > 0)
            {
                //EMV expire date is YYMMDD, but VisaNet only accepts YYMM, so we omit last two characters
                lResult = ISO8583_DLL.vip_add_bit_data(14, ref tag_buffer[0], Leng - 2);
            }

            // 18 (merchant type)                  4N, "5311"  (Means "Department Stores" according to VISA MCC)
            Leng = strCopy("5311", ref tag_buffer);
            lResult = ISO8583_DLL.vip_add_bit_data(18, ref tag_buffer[0], Leng);

            //19(acquirer country code)          3N, "158" for Taiwan
            Leng = strCopy("158", ref tag_buffer);
            lResult = ISO8583_DLL.vip_add_bit_data(19, ref tag_buffer[0], Leng);

            // 22 (POS entry mode)                 3N, tag 9F39 ("05" as defined in terminal.txt) + 3rd nibble="1"
            Leng = strCopy("051", ref tag_buffer);     // 05: Chip card entry, 1: PIN enabled
            lResult = ISO8583_DLL.vip_add_bit_data(22, ref tag_buffer[0], Leng);

            // 23 (PAN sequence number)                    3 N, 4-bit BCD (unsigned packed); 2 bytes, tag 5F34
            Leng = GetTagData(TAG_DATA_TXT, "5F34", ref tag_buffer);
            if (Leng > 0)
            {
                lResult = ISO8583_DLL.vip_add_bit_data(23, ref tag_buffer[0], Leng);
            }

            // 25 (POS condition code)             2N, should be "00" in message type 0100.
            Leng = strCopy("00", ref tag_buffer);
            lResult = ISO8583_DLL.vip_add_bit_data(25, ref tag_buffer[0], Leng);

            // 32 (Acquirer ID)                    max 11N, tag 9F1, "00000000001" as defined in A0000000031010.txt
            Leng = strCopy("00000000001", ref tag_buffer);
            lResult = ISO8583_DLL.vip_add_bit_data(32, ref tag_buffer[0], Leng);

            // 35 (Track 2 data)                    max 37N, tag 57
            Leng = GetTagData(TAG_DATA_TXT, "57", ref tag_buffer);
            if (Leng > 0)
            {
                //if (tag_buffer[Leng - 1] == 0x46) //maybe bug
                //    Leng = Leng - 1;

                lResult = ISO8583_DLL.vip_add_bit_data(35, ref tag_buffer[0], Leng);
            }

            // 37 (Retrieval referal number)       12N, MMDDhh from field7, + field 11
            Leng = strCopy(strDateTime.Substring(0, 6) + strTraceNumber, ref tag_buffer);
            lResult = ISO8583_DLL.vip_add_bit_data(37, ref tag_buffer[0], Leng);

            // 41 (Terminal identification)        8ANS, tag 9f1c,"SmartPOS" as defined in terminal.txt
            Leng = strCopy("SmartPOS", ref tag_buffer);
            lResult = ISO8583_DLL.vip_add_bit_data(41, ref tag_buffer[0], Leng);

            // 42 (Card acceptor ID)               15ANS, tag 9f16, "123456789012345" as defined in terminal.txt
            Leng = strCopy("123456789012345", ref tag_buffer);
            lResult = ISO8583_DLL.vip_add_bit_data(42, ref tag_buffer[0], Leng);

            // 43 (Card acceptor name and loc)     40ANS, "Uniform Industrial Corp.    TUCHENG   TW"
            Leng = strCopy("Uniform Industrial Corp.    TUCHENG   TW", ref tag_buffer);
            lResult = ISO8583_DLL.vip_add_bit_data(43, ref tag_buffer[0], Leng);

            // 49 (Currency code, transaction)     3N, tag 5F2A
            //leng = GetTagData(TAG_DATA_TXT, "5F2A", tag_buffer)    'query smartcard will get 4 bytes with leading 0
            Leng = strCopy(Active_Currency_Code, ref tag_buffer);   //use currency code in AP instead
            if (Leng > 0)
                lResult = ISO8583_DLL.vip_add_bit_data(49, ref tag_buffer[0], Leng);

            // 52(PIN data) if exist              8bin, tag DF71
            Leng = GetTagData(TAG_DATA_BIN, "DF71", ref tag_buffer);
            if (Leng > 0)
                lResult = ISO8583_DLL.vip_add_bit_data(52, ref tag_buffer[0], Leng);

            //55 (ICC related data)               max255bin, see tag list above
            Leng = GetField55Data(ref tag_buffer);
            if (Leng < 0)
            {
                parseErr("Field55");
                return;
            }
            else
                lResult = ISO8583_DLL.vip_add_bit_data(55, ref tag_buffer[0], Leng);

            // 60.1 (Terminal type)                "4" (Cash register)
            // 60.2 (Terminal entry capability)    "5" (according to VIP spec)
            Leng = Module1.HexStrToBin("45", ref tag_buffer);
            lResult = ISO8583_DLL.vip_add_bit_data(60, ref tag_buffer[0], Leng);

            // 63.0 (Bitmap (Field 63))            "\x80\x00\x00"  (means Network ID present, mandatory for message 0100)
            // 63.1 (Netwk ID Code)                "\x00\x00"      (means Visa determines the network and program rules)
            Leng = Module1.HexStrToBin("8000000000", ref tag_buffer);
            lResult = ISO8583_DLL.vip_add_bit_data(63, ref tag_buffer[0], Leng);

            // 136 (CRYPTOGRAM)                    16 hexadecimal digits; 8 bytes, tag 9F26
            Leng = GetTagData(TAG_DATA_TXT, "9F26",ref tag_buffer);
            if (Leng > 0)
                lResult = ISO8583_DLL.vip_add_bit_data(136, ref tag_buffer[0], Leng);

            //pack ISO8583
            Leng = Module1.HexStrToBin("711925", ref srcAddr);
            vip_out_leng = 1024;
            lResult = ISO8583_DLL.vip_data_pack(ref srcAddr[0], ref vip_packet_out[0], ref vip_out_leng);

            if (lResult != 0)
            {
                Text1.Text += "vip_data_pack( ) failed with errcode " + lResult + "\r\n";
                BtnAck.Enabled = true;
                BtnNoAck.Enabled = true;
                txt_extHostSimAddr.Enabled = false;
                txt_extHostSimPort.Enabled = false;
                return;
            }
            
            // debug output
            dbgVIPpacket = Module1.BinToHexStr(vip_packet_out, vip_out_leng);
            Text1.Text += "\r\n" + dbgVIPpacket + "\r\n\r\n";

            //clean bitmap in DLL
            ISO8583_DLL.vip_bitmap_clean();

            //initialize flags for Winsock event handlers
            vip_send_ok = 0;
            vip_receive_ok = 0;
            
            //Connect to Host
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Text1.Text += "Connecting " + txt_extHostSimAddr.Text + ":" + txt_extHostSimPort.Text + "\r\n";
            try
            {
                socket.Connect(txt_extHostSimAddr.Text, Convert.ToInt32(txt_extHostSimPort.Text)); // 1.設定 IP:Port 2.連線至伺服器
            }
            catch (Exception)
            {
                MessageBox.Show("Please make sure the setting of the Address and the Port are correct.", "Connect failed");
                Module1.Online_Response = "";
                return;
            }
            Text1.Text += "Connected.\r\n";
            Text1.Text += "Sending VIP packet...\r\n";
            socket.Send(vip_packet_out);
            Text1.Text += "Data packet sent.\r\n";
            Application.DoEvents();

            Array.Clear(vip_packet_in, 0, vip_packet_in.Length);
            vip_in_leng = socket.Receive(vip_packet_in);
            Text1.Text += "Getting response...(" + vip_in_leng + "bytes.)";
            dbgVIPpacket = Module1.BinToHexStr(vip_packet_in, vip_in_leng);
            Text1.Text += "\r\n" + dbgVIPpacket + "\r\n\r\n";
            socket.Close();

            //parse ISO8583 message into bitmap
            lResult = ISO8583_DLL.vip_data_unpack(ref srcAddr[0], ref destAddr[0], ref vip_packet_in[0], vip_in_leng, ref bitFieldIdx[0]);
            if (lResult != 0)
            {
                Text1.Text += "\r\n" + "VIP packet unpack failed (errcode=" + lResult + ")" + "\r\n";
                BtnAck.Enabled = true;
                BtnNoAck.Enabled = true;
                txt_extHostSimAddr.Enabled = false;
                txt_extHostSimPort.Enabled = false;
                return;
            }

            // debug
            Text1.Text += "Field returned: \r\n";
            for (i = 0; i < 128; i++)
                if (bitFieldIdx[i] > 0)
                    Text1.Text += i + ", ";

            Text1.Text += "\r\n\r\n";

            // get ARC, IAD or Issuer script if exitsts.
            // ARC_Code = field 39
            if (bitFieldIdx[39] > 0)
            {
                //initialize buffer and set maximum buffer length
                Array.Clear(tag_buffer, 0, tag_buffer.Length);
                Leng = 1024;
                // query bitfield data
                lResult = ISO8583_DLL.vip_get_bit_data(39, ref tag_buffer[0], ref Leng);
                strARC = Module1.BinToHexStr(tag_buffer, Leng);
                Text1.Text += "ARC = " + strARC + "\r\n";
            }
            else
            {
                Text1.Text += "ARC not exist!, exit";
                BtnAck.Enabled = true;
                BtnNoAck.Enabled = true;
                txt_extHostSimAddr.Enabled = false;
                txt_extHostSimPort.Enabled = false;
                return;
            }

            // IAD_Code = field 55, tag 91
            if (bitFieldIdx[55] > 0)
            {
                tag_buffer = new byte[1024];
                Leng = 1024;
                lResult = ISO8583_DLL.vip_get_bit_data(55, ref tag_buffer[0], ref Leng);
                dbgVIPpacket = Module1.BinToHexStr(tag_buffer, Leng);
                // search "91"
                i = dbgVIPpacket.IndexOf("91");
                if (i > 0)
                {
                    // tag 91 may contain maximum 16 bytes
                    strIADtmp = dbgVIPpacket.Substring(i);
                    // get ral length of IAD
                    i = Convert.ToInt32(strIADtmp.Substring(2, 2), 16);
                    strIAD = strIADtmp.Substring(4, i * 2);
                    Text1.Text += "IAD = " + strIAD + "\r\n";
                }
                else
                {
                    Text1.Text += "IAD not exist!, exit";
                    BtnAck.Enabled = true;
                    BtnNoAck.Enabled = true;
                    txt_extHostSimAddr.Enabled = false;
                    txt_extHostSimPort.Enabled = false;
                    return;
                }
            }

            strIss71 = "";
            strIss72 = "";

            Module1.Online_Response = strARC + Module1.charStr(0x1A) + strIAD + Module1.charStr(0x1A) + strIss71 + Module1.charStr(0x1A) + strIss72;

            // clean bitmap in DLL after parsing
            ISO8583_DLL.vip_bitmap_clean();

            // set control falg to inform EMV_TestForm that we are done.
            Text1.Text += "Host response sent to PIN Pad.\r\n";
            BankACK = true;
            return;
        }


        public void NetMasterOnlineAuthotisation()
        {
            int lResult, Leng;
            byte[] tag_buffer = new byte[1024];
            string strTraceNumber, strDateTime, strTime;
            int intMonth, intDay, intHour, intMin, intSec;
            byte[] srcAddr = new byte[3];
            byte[] destAddr = new byte[3];
            byte[] bitFieldIdx = new byte[129];

            string strARC, strIAD = "", strIADtmp, strIss71, strIss72, dataISS71, dataISS72, dbgVIPpacket;

            int i;

            Text1.Text += "\r\n\r\nPacking ISO8583 data...";

            Array.Clear(vip_packet_in, 0, vip_packet_in.Length);
            Array.Clear(vip_packet_out, 0, vip_packet_out.Length);

            // init bit map
            ISO8583_DLL.bank_bitmap_init();

            // According to V.I.P System BASE I Technical Spec Book 2 Chap 5.11
            // Smart card based VSDC Authorization Advice and Response should contain following fields:

            // 00: message type ("Sales" => "0100")
            Leng = strCopy("0100", ref tag_buffer);
            lResult = ISO8583_DLL.bank_add_bit_data(0, ref tag_buffer[0], Leng);

            // 01: Primary Bit Map ("Sales" => "6000000000000000") 16N
            Leng = strCopy("767F460128E1AA0A", ref tag_buffer);
            lResult = ISO8583_DLL.bank_add_bit_data(1, ref tag_buffer[0], Leng);

            // 02: PAN (up to 19 N, 4-bit BCD), This field is for MSR, ICC uses tag 0x57 inside field 55 to transfer Tk2 data.
            Leng = GetTagData(TAG_DATA_TXT, "57", ref tag_buffer);   //'ICC may not necessary send tag 5A, depend on tag list 4000000A's setting
            if (Leng > 0)
            {    //'Tag57 is Tk2 data, we need to trim it to get PAN
                for (i = 0; i < Leng - 1; i++)
                    if (tag_buffer[i] == Convert.ToInt32('D'))  //ASC("D")
                        Leng = i;
                lResult = ISO8583_DLL.bank_add_bit_data(2, ref tag_buffer[0], Leng);
            }

            // 03 (processing code)                6N, should be "000000" in message type 0100.
            //Leng = strCopy("093030", ref tag_buffer);   //for cashback
            Leng = strCopy("000000", ref tag_buffer);
            lResult = ISO8583_DLL.bank_add_bit_data(3, ref tag_buffer[0], Leng);

            // 04 (amount, transaction)            tag 9F02
            Leng = GetTagData(TAG_DATA_TXT, "9F02", ref tag_buffer);
            if (Leng > 0)
            {
                lResult = ISO8583_DLL.bank_add_bit_data(4, ref tag_buffer[0], Leng);
            }

            // 06 (Amount, Cardholder Billing) 12AN.
            Leng = strCopy("000000030000", ref tag_buffer);
            lResult = ISO8583_DLL.bank_add_bit_data(6, ref tag_buffer[0], Leng);

            // 07 (transaction date and time)      10N, MMDDhhmmss (tag 9A + 9F21, but 9F21 is not output)
            intMonth = DateTime.Now.Month;
            intDay = DateTime.Now.Day;
            intHour = DateTime.Now.Hour;
            intMin = DateTime.Now.Minute;
            intSec = DateTime.Now.Second;
            strDateTime = Extensions.Right("00" + intMonth, 2) + Extensions.Right("00" + intDay, 2) + Extensions.Right("00" + intHour, 2) + Extensions.Right("00" + intMin, 2) + Extensions.Right("00" + intSec, 2);
            Leng = strCopy(strDateTime, ref tag_buffer);
            lResult = ISO8583_DLL.bank_add_bit_data(7, ref tag_buffer[0], Leng);

            // 10 (Conversion Rate, Cardholder Billing) 8AN.
            Leng = strCopy("61000000", ref tag_buffer);
            lResult = ISO8583_DLL.bank_add_bit_data(10, ref tag_buffer[0], Leng);

            // 11 (system trace audit number)      6N, random number
            Random random = new Random();
            strTraceNumber = random.Next(2) + 1.ToString();
            strTraceNumber = Extensions.Right("000000" + strTraceNumber, 6);
            Leng = strCopy(strTraceNumber, ref tag_buffer);
            lResult = ISO8583_DLL.bank_add_bit_data(11, ref tag_buffer[0], Leng);

            // 12 (Time, Local Txn) 6AN.
            strDateTime = Extensions.Right("00" + intHour, 2) + Extensions.Right("00" + intMin, 2) + Extensions.Right("00" + intSec, 2);
            Leng = strCopy(strDateTime, ref tag_buffer);
            lResult = ISO8583_DLL.bank_add_bit_data(12, ref tag_buffer[0], Leng);

            // 13 (Date, Local Txn) 4AN.
            strDateTime = Extensions.Right("00" + intMonth, 2) + Extensions.Right("00" + intDay, 2);
            Leng = strCopy(strDateTime, ref tag_buffer);
            lResult = ISO8583_DLL.bank_add_bit_data(13, ref tag_buffer[0], Leng);

            // 14 (Expiration Date) 4N, tag 5F24
            Leng = GetTagData(TAG_DATA_TXT, "5F24", ref tag_buffer);
            if (Leng > 0)
            {
                //EMV expire date is YYMMDD, but VisaNet only accepts YYMM, so we omit last two characters
                lResult = ISO8583_DLL.bank_add_bit_data(14, ref tag_buffer[0], Leng - 2);
            }

            // 15 (Date, Settlement) 4AN.
            strDateTime = Extensions.Right("00" + intMonth, 2) + Extensions.Right("00" + intDay, 2);
            Leng = strCopy(strDateTime, ref tag_buffer);
            lResult = ISO8583_DLL.bank_add_bit_data(15, ref tag_buffer[0], Leng);

            // 16 (Date, Conversion) 4AN.
            strTime = Extensions.Right("00" + intMonth, 2) + Extensions.Right("00" + intDay, 2);
            Leng = strCopy(strTime, ref tag_buffer);
            lResult = ISO8583_DLL.bank_add_bit_data(16, ref tag_buffer[0], Leng);

            // 18 (merchant type) 4N, "5311"  (Means "Department Stores" according to VISA MCC)
            Leng = strCopy("5311", ref tag_buffer);
            lResult = ISO8583_DLL.bank_add_bit_data(18, ref tag_buffer[0], Leng);

            // 22 (POS entry mode) 3N, tag 9F39 ("05" as defined in terminal.txt) + 3rd nibble="1"
            Leng = strCopy("051", ref tag_buffer);     // 05: Chip card entry, 1: PIN enabled
            lResult = ISO8583_DLL.bank_add_bit_data(22, ref tag_buffer[0], Leng);

            // 23 (PAN sequence number) 3 N, 4-bit BCD (unsigned packed); 2 bytes, tag 5F34
            Leng = GetTagData(TAG_DATA_TXT, "5F34", ref tag_buffer);
            byte[] tmp = new byte[3];
            tmp[2] = tag_buffer[1];
            tmp[1] = tag_buffer[0];
            tmp[0] = 48;
            if (Leng > 0)
            {
                lResult = ISO8583_DLL.bank_add_bit_data(23, ref tmp[0], Leng + 1);
            }

            // 32 (Acquirer ID) max 11N, tag 9F1, "00000000001" as defined in A0000000031010.txt
            Leng = strCopy("123456", ref tag_buffer);
            lResult = ISO8583_DLL.bank_add_bit_data(32, ref tag_buffer[0], Leng);

            // 35 (Track 2 data) max 37N, tag 57
            Leng = GetTagData(TAG_DATA_TXT, "57", ref tag_buffer);
            if (Leng > 0)
            {
                if (tag_buffer[Leng - 1] == 0x46) //maybebug
                    Leng = Leng - 1;

                lResult = ISO8583_DLL.bank_add_bit_data(35, ref tag_buffer[0], Leng);
            }

            // 37 (Retrieval referal number) 12N, MMDDhh from field7, + field 11
            strDateTime = Extensions.Right("00" + intMonth, 2) + Extensions.Right("00" + intDay, 2) + Extensions.Right("00" + intHour, 2);
            Leng = strCopy(strDateTime.Substring(0, 6) + strTraceNumber, ref tag_buffer);
            lResult = ISO8583_DLL.bank_add_bit_data(37, ref tag_buffer[0], Leng);

            // 41 (Terminal identification) 8ANS, tag 9f1c,"SmartPOS" as defined in terminal.txt
            Leng = strCopy("SmartPOS", ref tag_buffer);
            lResult = ISO8583_DLL.bank_add_bit_data(41, ref tag_buffer[0], Leng);

            // 42 (Card acceptor ID) 15ANS, tag 9f16, "123456789012345" as defined in terminal.txt
            Leng = strCopy("123456789012345", ref tag_buffer);
            lResult = ISO8583_DLL.bank_add_bit_data(42, ref tag_buffer[0], Leng);

            // 43 (Card acceptor name and loc) 40ANS, "Uniform Industrial Corp.    TUCHENG   TW"
            Leng = strCopy("Uniform Industrial Corp.    TUCHENG   TW", ref tag_buffer);
            lResult = ISO8583_DLL.bank_add_bit_data(43, ref tag_buffer[0], Leng);

            // 48 (Additional Data - Private) 14N.
            Leng = strCopy("A9F8F0F0F2E3E5", ref tag_buffer);
            lResult = ISO8583_DLL.bank_add_bit_data(48, ref tag_buffer[0], Leng);

            // 49 (Currency code, transaction) 3N, tag 5F2A
            //leng = GetTagData(TAG_DATA_TXT, "5F2A", tag_buffer)    'query smartcard will get 4 bytes with leading 0
            Leng = strCopy("978", ref tag_buffer);   //use currency code in AP instead
            if (Leng > 0)
            {
                lResult = ISO8583_DLL.bank_add_bit_data(49, ref tag_buffer[0], Leng);
            }

            // 51 (Currency Code, Cardholder Billing) 3AN.
            Leng = strCopy("978", ref tag_buffer);
            lResult = ISO8583_DLL.bank_add_bit_data(51, ref tag_buffer[0], Leng);

            // 52 (PIN data) if exist 8bin, tag DF71
            Leng = GetTagData(TAG_DATA_BIN, "DF71", ref tag_buffer);
            if (Leng > 0)
            {
                lResult = ISO8583_DLL.bank_add_bit_data(52, ref tag_buffer[0], Leng);
            }

            // 53 (Security Related Control Information) 16AN.
            Leng = strCopy("9701100001000000", ref tag_buffer);
            lResult = ISO8583_DLL.bank_add_bit_data(53, ref tag_buffer[0], Leng);


            // 54 (Additional Amounts)
            // DE 54 SF1(N2)+SF2(N2)+SF3(N3)+SF4(AN13)
            //if ((EMV_TestForm.Combo1.Text, "CashBack") != 0 )
            //{
            //    Leng = strCopy(Text4.Text, tag_buffer);
            //    lResult = ISO8583_DLL.bank_add_bit_data(54, tag_buffer[0], Leng);
            //}

            // 55 (ICC related data) max255bin, see tag list above
            Leng = GetField55Data(ref tag_buffer);
            if (Leng < 0)
            {
                parseErr("Field55");
                return;
            }
            else
            {
                lResult = ISO8583_DLL.bank_add_bit_data(55, ref tag_buffer[0], Leng);
            }

            // 61 (Point-of-Service(POS)Data) 52N.
            Leng = strCopy("00000100033008860000000237", ref tag_buffer);
            lResult = ISO8583_DLL.bank_add_bit_data(61, ref tag_buffer[0], Leng);

            // 63 (Network Data) 24N.
            Leng = strCopy("MCC123456789", ref tag_buffer);
            lResult = ISO8583_DLL.bank_add_bit_data(63, ref tag_buffer[0], Leng);

            vip_out_leng = 1024;
            lResult = ISO8583_DLL.bank_data_pack(ref srcAddr[0], ref vip_packet_out[0], ref vip_out_leng);

            if (lResult != 0)
            {
                Text1.Text += "vip_data_pack( ) failed with errcode " + lResult + "\r\n";
                BtnAck.Enabled = true;
                BtnNoAck.Enabled = true;
                txt_extHostSimAddr.Enabled = false;
                txt_extHostSimPort.Enabled = false;
                return;
            }

            // debug output
            dbgVIPpacket = Module1.BinToHexStr(vip_packet_out, vip_out_leng);
            Text1.Text += "\r\n" + dbgVIPpacket + "\r\n\r\n";

            //clean bitmap in DLL
            ISO8583_DLL.bank_bitmap_clean();

            //initialize flags for Winsock event handlers
            vip_send_ok = 0;
            vip_receive_ok = 0;

            //Connect to Host
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Text1.Text += "Connecting " + txt_extHostSimAddr.Text + ":" + txt_extHostSimPort.Text + "\r\n";
            try
            {
                socket.Connect(txt_extHostSimAddr.Text, Convert.ToInt32(txt_extHostSimPort.Text)); // 1.設定 IP:Port 2.連線至伺服器
            }
            catch (Exception)
            {
                MessageBox.Show("Please make sure the setting of the Address and the Port are correct.", "Connect failed");
                Module1.Online_Response = "";
                return;
            }
            Text1.Text += "Connected.\r\n";
            Text1.Text += "Sending VIP packet...\r\n";
            socket.Send(vip_packet_out);
            Text1.Text += "Data packet sent.\r\n";
            Application.DoEvents();

            Array.Clear(vip_packet_in, 0, vip_packet_in.Length);
            vip_in_leng = socket.Receive(vip_packet_in);
            Text1.Text += "Getting response...(" + vip_in_leng + "bytes.)";
            dbgVIPpacket = Module1.BinToHexStr(vip_packet_in, vip_in_leng);
            Text1.Text += "\r\n" + dbgVIPpacket + "\r\n\r\n";
            socket.Close();

            //parse ISO8583 message into bitmap
            lResult = ISO8583_DLL.bank_data_unpack(ref srcAddr[0], ref destAddr[0], ref vip_packet_in[0], vip_in_leng, ref bitFieldIdx[0]);
            if (lResult != 0)
            {
                Text1.Text += "\r\n" + "VIP packet unpack failed (errcode=" + lResult + ")" + "\r\n";
                BtnAck.Enabled = true;
                BtnNoAck.Enabled = true;
                txt_extHostSimAddr.Enabled = false;
                txt_extHostSimPort.Enabled = false;
                return;
            }

            // debug
            Text1.Text += "Field returned: \r\n";
            for (i = 0; i < 128; i++)
                if (bitFieldIdx[i] > 0)
                    Text1.Text += i + ", ";

            Text1.Text += "\r\n\r\n";

            // get ARC, IAD or Issuer script if exitsts.
            // ARC_Code = field 39
            if (bitFieldIdx[39] > 0)
            {
                //initialize buffer and set maximum buffer length
                Array.Clear(tag_buffer, 0, tag_buffer.Length);
                Leng = 1024;
                // query bitfield data
                lResult = ISO8583_DLL.bank_get_bit_data(39, ref tag_buffer[0], ref Leng);
                strARC = Module1.BinToHexStr(tag_buffer, Leng);
                Text1.Text += "ARC = " + strARC + "\r\n";
            }
            else
            {
                Text1.Text += "ARC not exist!, exit";
                BtnAck.Enabled = true;
                BtnNoAck.Enabled = true;
                txt_extHostSimAddr.Enabled = false;
                txt_extHostSimPort.Enabled = false;
                return;
            }

            // IAD_Code = field 55, tag 91
            if (bitFieldIdx[55] > 0)
            {
                tag_buffer = new byte[1024];
                Leng = 1024;
                lResult = ISO8583_DLL.bank_get_bit_data(55, ref tag_buffer[0], ref Leng);
                dbgVIPpacket = Module1.BinToHexStr(tag_buffer, Leng);
                //dbgVIPpacket = Encoding.ASCII.GetString(tag_buffer);
                // search "91"
                i = dbgVIPpacket.IndexOf("91");
                if (i >= 0)
                {
                    // tag 91 may contain maximum 16 bytes
                    strIADtmp = dbgVIPpacket.Substring(i, dbgVIPpacket.Length);
                    // get ral length of IAD
                    i = Convert.ToInt32(strIADtmp.Substring(2, 2), 16);
                    strIAD = strIADtmp.Substring(4, i * 2);
                    Text1.Text += "IAD = " + strIAD + "\r\n";
                }
                else
                {
                    Text1.Text += "IAD not exist!, exit";
                    BtnAck.Enabled = true;
                    BtnNoAck.Enabled = true;
                    txt_extHostSimAddr.Enabled = false;
                    txt_extHostSimPort.Enabled = false;
                    return;
                }
            }

            strIss71 = "";
            strIss72 = "";

            Module1.Online_Response = strARC + Module1.charStr(0x1A) + strIAD + Module1.charStr(0x1A) + strIss71 + Module1.charStr(0x1A) + strIss72;

            // clean bitmap in DLL after parsing
            ISO8583_DLL.bank_bitmap_clean();

            // set control falg to inform EMV_TestForm that we are done.
            Text1.Text += "Host response sent to PIN Pad.\r\n";
            BankACK = true;
            return;
        }

        private void BtnDESetDefault_Click(object sender, EventArgs e)
        {
            CheckDataElement(2, false);
            CheckDataElement(3, false);
            CheckDataElement(4, false);
            CheckDataElement(7, false);
            CheckDataElement(10, false);
            CheckDataElement(11, false);
            CheckDataElement(12, false);
            CheckDataElement(13, false);
            CheckDataElement(14, false);
            CheckDataElement(15, false);
            CheckDataElement(16, false);
            CheckDataElement(18, false);
            CheckDataElement(22, false);
            CheckDataElement(32, false);
            CheckDataElement(35, false);
            CheckDataElement(37, false);
            CheckDataElement(41, false);
            CheckDataElement(42, false);
            CheckDataElement(43, false);
            CheckDataElement(48, false);
            CheckDataElement(49, false);
            CheckDataElement(51, false);
            CheckDataElement(53, false);
            CheckDataElement(55, false);
            CheckDataElement(63, false);
            CheckDataElement(61, false);
        }

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            int index, temp_value;
            int i, j, k;

            temp_value = 0;
            LabDataField.Text = "";

            for (k = 1; k <= 8; k++)
                for (j = 1; j <= 2; j++)
                {
                    for (i = 1; i <= 4; i++)
                    {
                        index = ((k - 1) * 8) + (((j - 1) * 4) + i);
                        foreach (Control ctrl in groupBox5.Controls)
                        {
                            if (ctrl.Name == "checkBox" + index)
                            {
                                CheckBox ck = (CheckBox)ctrl;
                                if (ck.Checked == true)
                                    switch (i)
                                    {
                                        case 1:
                                            temp_value += 8;
                                            break;
                                        case 2:
                                            temp_value += 4;
                                            break;
                                        case 3:
                                            temp_value += 2;
                                            break;
                                        case 4:
                                            temp_value += 1;
                                            break;
                                    }
                            }
                        }
                    }

                    LabDataField.Text += temp_value.ToString("X");
                    temp_value = 0;
                }
        }

        public void PCDNetVipOnlineAuthotisation()
        {
            int lResult;
            int Leng;
            byte[] tag_buffer = new byte[1024];
            string strTraceNumber, strDateTime;
            int intMonth, intDay, intHour, intMin, intSec;
            byte[] srcAddr = new byte[3];
            byte[] destAddr = new byte[3];
            byte[] bitFieldIdx = new byte[129];

            string strARC, strIAD = "", strIADtmp, strIss71, strIss72, dbgVIPpacket;

            int i;

            Text1.Text += "\r\n\r\nPacking ISO8583 data...";

            Array.Clear(vip_packet_in, 0, vip_packet_in.Length);
            Array.Clear(vip_packet_out, 0, vip_packet_out.Length);

            // init bit map
            ISO8583_DLL.vip_bitmap_init();

            //According to V.I.P System BASE I Technical Spec Book 2 Chap 5.11
            //Smart card based VSDC Authorization Advice and Response should contain following fields:

            // 00: message type ("Sales" => "0100")
            Leng = strCopy("0100", ref tag_buffer);
            lResult = ISO8583_DLL.vip_add_bit_data(0, ref tag_buffer[0], Leng);
            textBox3.Text += "00, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            // 02: PAN (up to 19 N, 4-bit BCD), This field is for MSR, ICC uses tag 0x57 inside field 55 to transfer Tk2 data.
            Leng = GetTagData(TAG_DATA_TXT, "57", ref tag_buffer);   //'ICC may not necessary send tag 5A, depend on tag list 4000000A's setting
            if (Leng > 0)
            {    //'Tag57 is Tk2 data, we need to trim it to get PAN
                for (i = 0; i < Leng - 1; i++)
                    if (tag_buffer[i] == Convert.ToInt32('D'))  //ASC("D")
                        Leng = i;
                lResult = ISO8583_DLL.vip_add_bit_data(2, ref tag_buffer[0], Leng);
                textBox3.Text += "02, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";

            }

            // 03 (processing code)                6N, should be "000000" in message type 0100.
            Leng = strCopy("000000", ref tag_buffer);
            lResult = ISO8583_DLL.vip_add_bit_data(3, ref tag_buffer[0], Leng);
            textBox3.Text += "03, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";

            // 04 (amount, transaction)            tag 9F02
            Leng = GetTagData(TAG_DATA_TXT, "9F02", ref tag_buffer);
            if (Leng > 0)
            {
                lResult = ISO8583_DLL.vip_add_bit_data(4, ref tag_buffer[0], Leng);
                textBox3.Text += "04, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            }

            // 07 (transaction date and time)      10N, MMDDhhmmss (tag 9A + 9F21, but 9F21 is not output)
            intMonth = DateTime.Now.Month;
            intDay = DateTime.Now.Day;
            intHour = DateTime.Now.Hour;
            intMin = DateTime.Now.Minute;
            intSec = DateTime.Now.Second;
            strDateTime = Extensions.Right("00" + intMonth, 2) + Extensions.Right("00" + intDay, 2) + Extensions.Right("00" + intHour, 2) + Extensions.Right("00" + intMin, 2) + Extensions.Right("00" + intSec, 2);
            Leng = strCopy(strDateTime, ref tag_buffer);
            lResult = ISO8583_DLL.vip_add_bit_data(7, ref tag_buffer[0], Leng);
            textBox3.Text += "07, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";


            // 11 (system trace audit number)      6N, random number
            Random random = new Random();
            strTraceNumber = random.Next(2) + 1.ToString();
            strTraceNumber = Extensions.Right("000000" + strTraceNumber, 6);
            Leng = strCopy(strTraceNumber, ref tag_buffer);
            lResult = ISO8583_DLL.vip_add_bit_data(11, ref tag_buffer[0], Leng);
            textBox3.Text += "11, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";

            // 14  (Expiration Date)                4N, tag 5F24
            Leng = GetTagData(TAG_DATA_TXT, "5F24", ref tag_buffer);
            if (Leng > 0)
            {
                //EMV expire date is YYMMDD, but VisaNet only accepts YYMM, so we omit last two characters
                lResult = ISO8583_DLL.vip_add_bit_data(14, ref tag_buffer[0], Leng - 2);
                textBox3.Text += "14, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            }

            // 18 (merchant type) 4N, "5311"  (Means "Department Stores" according to VISA MCC)
            Leng = strCopy("5311", ref tag_buffer);
            lResult = ISO8583_DLL.vip_add_bit_data(18, ref tag_buffer[0], Leng);
            textBox3.Text += "18, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";

            //19(acquirer country code) 3N, "158" for Taiwan
            Leng = strCopy("158", ref tag_buffer);
            lResult = ISO8583_DLL.vip_add_bit_data(19, ref tag_buffer[0], Leng);
            textBox3.Text += "19, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";

            // 22 (POS entry mode) 3N, tag 9F39 ("05" as defined in terminal.txt) + 3rd nibble="1"
            Leng = strCopy("051", ref tag_buffer);     // 05: Chip card entry, 1: PIN enabled
            lResult = ISO8583_DLL.vip_add_bit_data(22, ref tag_buffer[0], Leng);
            textBox3.Text += "22, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";

            // 23 (PAN sequence number) 3 N, 4-bit BCD (unsigned packed); 2 bytes, tag 5F34
            Leng = GetTagData(TAG_DATA_TXT, "5F34", ref tag_buffer);
            if (Leng > 0)
            {
                lResult = ISO8583_DLL.vip_add_bit_data(23, ref tag_buffer[0], Leng);
                textBox3.Text += "23, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";

            }

            // 25 (POS condition code) 2N, should be "00" in message type 0100.
            Leng = strCopy("00", ref tag_buffer);
            lResult = ISO8583_DLL.vip_add_bit_data(25, ref tag_buffer[0], Leng);
            textBox3.Text += "25, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";

            // 32 (Acquirer ID) max 11N, tag 9F1, "00000000001" as defined in A0000000031010.txt
            Leng = strCopy("00000000001", ref tag_buffer);
            lResult = ISO8583_DLL.vip_add_bit_data(32, ref tag_buffer[0], Leng);
            textBox3.Text += "32, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";

            // 35 (Track 2 data) max 37N, tag 57
            Leng = GetTagData(TAG_DATA_TXT, "57", ref tag_buffer);
            if (Leng > 0)
            {
                //if (tag_buffer[Leng - 1] == 0x46) //maybe bug
                //    Leng = Leng - 1;

                lResult = ISO8583_DLL.vip_add_bit_data(35, ref tag_buffer[0], Leng);
                textBox3.Text += "35, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";

            }

            // 37 (Retrieval referal number)       12N, MMDDhh from field7, + field 11
            Leng = strCopy(strDateTime.Substring(0, 6) + strTraceNumber, ref tag_buffer);
            lResult = ISO8583_DLL.vip_add_bit_data(37, ref tag_buffer[0], Leng);
            textBox3.Text += "37, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";

            // 41 (Terminal identification)        8ANS, tag 9f1c,"SmartPOS" as defined in terminal.txt
            Leng = strCopy("SmartPOS", ref tag_buffer);
            lResult = ISO8583_DLL.vip_add_bit_data(41, ref tag_buffer[0], Leng);
            textBox3.Text += "41, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";

            // 42 (Card acceptor ID)               15ANS, tag 9f16, "123456789012345" as defined in terminal.txt
            Leng = strCopy("123456789012345", ref tag_buffer);
            lResult = ISO8583_DLL.vip_add_bit_data(42, ref tag_buffer[0], Leng);
            textBox3.Text += "42, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";

            // 43 (Card acceptor name and loc)     40ANS, "Uniform Industrial Corp.    TUCHENG   TW"
            Leng = strCopy("Uniform Industrial Corp.    TUCHENG   TW", ref tag_buffer);
            lResult = ISO8583_DLL.vip_add_bit_data(43, ref tag_buffer[0], Leng);
            textBox3.Text += "43, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";

            // 49 (Currency code, transaction)     3N, tag 5F2A
            //leng = GetTagData(TAG_DATA_TXT, "5F2A", tag_buffer)    'query smartcard will get 4 bytes with leading 0
            Leng = strCopy(Active_Currency_Code, ref tag_buffer);   //use currency code in AP instead
            if (Leng > 0)
            {
                lResult = ISO8583_DLL.vip_add_bit_data(49, ref tag_buffer[0], Leng);
                textBox3.Text += "49, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            }

            // 52 (PIN data) if exist 8bin, tag DF71
            //Leng = GetTagData(TAG_DATA_BIN, "DF71", ref tag_buffer);
            Leng = GetTagData(TAG_DATA_BIN, "DF01", ref tag_buffer);
            if (Leng > 0)
            {
                lResult = ISO8583_DLL.vip_add_bit_data(52, ref tag_buffer[0], Leng);
                textBox3.Text += "52, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";

            }

            //55 (ICC related data) max255bin, see tag list above
            Leng = GetField55Data(ref tag_buffer);
            if (Leng < 0)
            {
                parseErr("Field55");
                return;
            }
            else
            {
                lResult = ISO8583_DLL.vip_add_bit_data(55, ref tag_buffer[0], Leng);
                textBox3.Text += "55, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            }

            // 60.1 (Terminal type)                "4" (Cash register)
            // 60.2 (Terminal entry capability)    "5" (according to VIP spec)
            Leng = Module1.HexStrToBin("45", ref tag_buffer);
            lResult = ISO8583_DLL.vip_add_bit_data(60, ref tag_buffer[0], Leng);
            textBox3.Text += "60, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";

            // 63.0 (Bitmap (Field 63))            "\x80\x00\x00"  (means Network ID present, mandatory for message 0100)
            // 63.1 (Netwk ID Code)                "\x00\x00"      (means Visa determines the network and program rules)
            Leng = Module1.HexStrToBin("8000000000", ref tag_buffer);
            lResult = ISO8583_DLL.vip_add_bit_data(63, ref tag_buffer[0], Leng);
            textBox3.Text += "63, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";

            // 136 (CRYPTOGRAM)                    16 hexadecimal digits; 8 bytes, tag 9F26
            Leng = GetTagData(TAG_DATA_TXT, "9F26", ref tag_buffer);
            if (Leng > 0)
            {
                lResult = ISO8583_DLL.vip_add_bit_data(136, ref tag_buffer[0], Leng);
                textBox3.Text += "136, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            }

            //pack ISO8583
            Leng = Module1.HexStrToBin("711925", ref srcAddr);
            vip_out_leng = 1024;
            lResult = ISO8583_DLL.vip_data_pack(ref srcAddr[0], ref vip_packet_out[0], ref vip_out_leng);

            if (lResult != 0)
            {
                Text1.Text += "vip_data_pack( ) failed with errcode " + lResult + "\r\n";
                BtnAck.Enabled = true;
                BtnNoAck.Enabled = true;
                txt_extHostSimAddr.Enabled = false;
                txt_extHostSimPort.Enabled = false;
                return;
            }

            // debug output
            dbgVIPpacket = Module1.BinToHexStr(vip_packet_out, vip_out_leng);
            Text1.Text += "\r\n" + dbgVIPpacket + "\r\n\r\n";
            Module1.Online_Response = "";
            
            //clean bitmap in DLL
            ISO8583_DLL.vip_bitmap_clean();

            //initialize flags for Winsock event handlers
            vip_send_ok = 0;
            vip_receive_ok = 0;

            //Connect to Host
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Text1.Text += "Connecting " + txt_extHostSimAddr.Text + ":" + txt_extHostSimPort.Text + "\r\n";
            try
            {
                socket.Connect(txt_extHostSimAddr.Text, Convert.ToInt32(txt_extHostSimPort.Text)); // 1.設定 IP:Port 2.連線至伺服器
            }
            catch (Exception)
            {
                MessageBox.Show("Please make sure the setting of the Address and the Port are correct.", "Connect failed");
                Module1.Online_Response = "";
                return;
            }
            Text1.Text += "Connected.\r\n";
            Text1.Text += "Sending VIP packet...\r\n";
            socket.Send(vip_packet_out);
            Text1.Text += "Data packet sent.\r\n";
            Application.DoEvents();

            Array.Clear(vip_packet_in, 0, vip_packet_in.Length);
            vip_in_leng = socket.Receive(vip_packet_in);
            Text1.Text += "Getting response...(" + vip_in_leng + "bytes.)";
            dbgVIPpacket = Module1.BinToHexStr(vip_packet_in, vip_in_leng);
            Text1.Text += "\r\n" + dbgVIPpacket + "\r\n\r\n";
            socket.Close();

            //parse ISO8583 message into bitmap
            lResult = ISO8583_DLL.vip_data_unpack(ref srcAddr[0], ref destAddr[0], ref vip_packet_in[0], vip_in_leng, ref bitFieldIdx[0]);
            if (lResult != 0)
            {
                Text1.Text += "\r\n" + "VIP packet unpack failed (errcode=" + lResult + ")" + "\r\n";
                BtnAck.Enabled = true;
                BtnNoAck.Enabled = true;
                txt_extHostSimAddr.Enabled = false;
                txt_extHostSimPort.Enabled = false;
                return;
            }

            // debug
            Text1.Text += "Field returned: \r\n";
            for (i = 0; i < 128; i++)
                if (bitFieldIdx[i] > 0)
                    Text1.Text += i + ", ";

            Text1.Text += "\r\n\r\n";

            // get ARC, IAD or Issuer script if exitsts.
            // ARC_Code = field 39
            if (bitFieldIdx[39] > 0)
            {
                //initialize buffer and set maximum buffer length
                Array.Clear(tag_buffer, 0, tag_buffer.Length);
                Leng = 1024;
                // query bitfield data
                lResult = ISO8583_DLL.vip_get_bit_data(39, ref tag_buffer[0], ref Leng);
                strARC = Module1.BinToHexStr(tag_buffer, Leng);
                Text1.Text += "ARC = " + strARC + "\r\n";
            }
            else
            {
                Text1.Text += "ARC not exist!, exit";
                BtnAck.Enabled = true;
                BtnNoAck.Enabled = true;
                txt_extHostSimAddr.Enabled = false;
                txt_extHostSimPort.Enabled = false;
                return;
            }

            // IAD_Code = field 55, tag 91
            if (bitFieldIdx[55] > 0)
            {
                tag_buffer = new byte[1024];
                Leng = 1024;
                lResult = ISO8583_DLL.vip_get_bit_data(55, ref tag_buffer[0], ref Leng);
                dbgVIPpacket = Module1.BinToHexStr(tag_buffer, Leng);
                // search "91"
                i = dbgVIPpacket.IndexOf("91");
                if (i > 0)
                {
                    // tag 91 may contain maximum 16 bytes
                    strIADtmp = dbgVIPpacket.Substring(i);
                    // get ral length of IAD
                    i = Convert.ToInt32(strIADtmp.Substring(2, 2), 16);
                    strIAD = strIADtmp.Substring(4, i * 2);
                    Text1.Text += "IAD = " + strIAD + "\r\n";
                }
                else
                {
                    Text1.Text += "IAD not exist!, exit";
                    BtnAck.Enabled = true;
                    BtnNoAck.Enabled = true;
                    txt_extHostSimAddr.Enabled = false;
                    txt_extHostSimPort.Enabled = false;
                }
            }

            strIss71 = "";
            strIss72 = "";

            Module1.Online_Response = strARC + Module1.charStr(0x1A) + strIAD + Module1.charStr(0x1A) + strIss71 + Module1.charStr(0x1A) + strIss72;

            // clean bitmap in DLL after parsing
            ISO8583_DLL.vip_bitmap_clean();

            // set control falg to inform EMV_TestForm that we are done.
            Text1.Text += "Host response sent to PIN Pad.\r\n";
            BankACK = true;
            return;
        }

        public void PCDNetMasterOnlineAuthotisation()
        {
            int lResult;
            int Leng;
            byte[] tag_buffer = new byte[1024];
            string strTraceNumber, strDateTime;
            int intMonth, intDay, intHour, intMin, intSec;
            string Command_str;
            byte[] srcAddr = new byte[3];
            byte[] destAddr = new byte[3];
            byte[] bitFieldIdx = new byte[129];

            string strARC, strIAD = "", strIADtmp, strIss71, strIss72, dbgVIPpacket;
            string strAdditionalData, strDE22POSEntryMode;

            int i;
            intMonth = intDay = intHour = intMin = intSec = 0;
            strTraceNumber = strDateTime = "";
            Text1.Text += "\r\n\r\nPacking ISO8583 data...";

            Array.Clear(vip_packet_in, 0, vip_packet_in.Length);
            Array.Clear(vip_packet_out, 0, vip_packet_out.Length);

            ISO8583_DLL.bank_bitmap_init();

            // 00: message type ("Sales" => "0100")
            Leng = strCopy("0100", ref tag_buffer);
            lResult = ISO8583_DLL.bank_add_bit_data(0, ref tag_buffer[0], Leng);
            textBox3.Text += "00, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";

            // 01: Primary Bit Map ("Sales" => "6000000000000000") 16N
            Leng = strCopy(LabDataField.Text, ref tag_buffer);
            lResult = ISO8583_DLL.bank_add_bit_data(1, ref tag_buffer[0], Leng);
            textBox3.Text += "01, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";

            // 02
            if (CheckDataElement(2, true))
            {
                // 02: PAN (up to 19 N, 4-bit BCD), This field is for MSR, ICC uses tag 0x57 inside field 55 to transfer Tk2 data.
                Leng = GetTagData(TAG_DATA_TXT, "57", ref tag_buffer);  //ICC may not necessary send tag 5A, depend on tag list 4000000A's setting
                if (Leng > 0)
                {
                    // Tag57 is Tk2 data, we need to trim it to get PAN
                    for (i = 0; i < Leng - 1; i++)
                        if (tag_buffer[i] == Convert.ToInt32('D'))  //ASC("D")
                            Leng = i;
                    lResult = ISO8583_DLL.bank_add_bit_data(2, ref tag_buffer[0], Leng);
                }
                else
                {
                    Command_str = "57";
                    Module1.SIOOutput("T63" + Command_str);
                    Array.Clear(tag_buffer, 0, tag_buffer.Length);
                    int rtn;
                    rtn = PPDMain.DLL.Getter(UIC.PktType.STX, "T6357", "", out Module1.receive_buf);
                    Module1.SIOInput(rtn, 1, Module1.receive_buf);
                    if (rtn < 0)
                    {
                        //'Call pinPAD_No_Response      
                        return;
                    }
                    
                    if (rtn > 6)
                    {
                        for (i = 0; i <= rtn - 12; i++)
                            if (tag_buffer[12 + i] == Convert.ToInt32('D'))
                            {
                                Leng = i;
                                break;
                            }
                        lResult = ISO8583_DLL.bank_add_bit_data(2, ref tag_buffer[12], Leng);
                    }
                }
                textBox3.Text += "02, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            }

            if (CheckDataElement(3, true))
            {   // 03 (processing code)                6N, should be "000000" in message type 0100.
                Array.Clear(tag_buffer, 0, tag_buffer.Length);
                Leng = construct_PP_DE3_data(tag_buffer);
                lResult = ISO8583_DLL.bank_add_bit_data(3, ref tag_buffer[0], Leng);
                textBox3.Text += "03, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            }

            if (CheckDataElement(4, true))
            {   // 04 (amount, transaction)            tag 9F02
                Leng = GetTagData(TAG_DATA_TXT, "9F02", ref tag_buffer);
                if (Leng > 0)
                    lResult = ISO8583_DLL.bank_add_bit_data(4, ref tag_buffer[0], Leng);
                else
                {
                    Command_str = "9F02";
                    Array.Clear(tag_buffer, 0, tag_buffer.Length);
                    int rtn = PPDMain.DLL.Getter(UIC.PktType.STX, "T63", Command_str, out tag_buffer);
                    if (rtn == 0)
                        return;
                    if (rtn > 6)
                        lResult = ISO8583_DLL.bank_add_bit_data(4, ref tag_buffer[13], 12);
                }
                textBox3.Text += "04, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            }

            if (CheckDataElement(7, true))
            {   // 07 (transaction date and time)      10N, MMDDhhmmss (tag 9A + 9F21, but 9F21 is not output)
                intMonth = DateTime.Now.Month;
                intDay = DateTime.Now.Day;
                intHour = DateTime.Now.Hour;
                intMin = DateTime.Now.Minute;
                intSec = DateTime.Now.Second;
                strDateTime = Extensions.Right("00" + intMonth, 2) + Extensions.Right("00" + intDay, 2) + Extensions.Right("00" + intHour, 2) + Extensions.Right("00" + intMin, 2) + Extensions.Right("00" + intSec, 2);
                Leng = strCopy(strDateTime, ref tag_buffer);
                lResult = ISO8583_DLL.bank_add_bit_data(7, ref tag_buffer[0], Leng);
                textBox3.Text += "07, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            }

            if (CheckDataElement(10, true))
            {  // 10 (Conversion Rate, Cardholder Billing) 8AN.
                Leng = strCopy("61000000", ref tag_buffer);
                lResult = ISO8583_DLL.bank_add_bit_data(10, ref tag_buffer[0], Leng);
                textBox3.Text += "10, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            }

            if (CheckDataElement(11, true))
            {  // 11 (system trace audit number) 6N, random number
                Random random = new Random();
                strTraceNumber = random.Next(2) + 1.ToString();
                strTraceNumber = Extensions.Right("000000" + strTraceNumber, 6);
                Leng = strCopy(strTraceNumber, ref tag_buffer);
                lResult = ISO8583_DLL.bank_add_bit_data(11, ref tag_buffer[0], Leng);
                textBox3.Text += "11, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            }

            if (CheckDataElement(12, true))
            {   // 12 (Time, Local Txn) 6AN.
                strDateTime = Extensions.Right("00" + intHour, 2) + Extensions.Right("00" + intMin, 2) + Extensions.Right("00" + intSec, 2);
                Leng = strCopy(strDateTime, ref tag_buffer);
                lResult = ISO8583_DLL.bank_add_bit_data(12, ref tag_buffer[0], Leng);
                textBox3.Text += "12, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            }

            if (CheckDataElement(13, true))
            {   // 13 (Date, Local Txn) 4AN.
                strDateTime = Extensions.Right("00" + intMonth, 2) + Extensions.Right("00" + intDay, 2);
                Leng = strCopy(strDateTime, ref tag_buffer);
                lResult = ISO8583_DLL.bank_add_bit_data(13, ref tag_buffer[0], Leng);
                textBox3.Text += "13, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            }

            if (CheckDataElement(14, true))
            {    // 14 (Expiration Date) 4N, tag 5F24
                Leng = GetTagData(TAG_DATA_TXT, "5F24", ref tag_buffer);
                if (Leng > 0)
                {
                    //EMV expire date is YYMMDD, but VisaNet only accepts YYMM, so we omit last two characters
                    lResult = ISO8583_DLL.bank_add_bit_data(14, ref tag_buffer[0], Leng - 2);
                    textBox3.Text += "14, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
                }
            }

            if (CheckDataElement(15, true))
            {
                // 15 (Date, Settlement) 4AN.
                strDateTime = Extensions.Right("00" + intMonth, 2) + Extensions.Right("00" + intDay, 2);
                Leng = strCopy(strDateTime, ref tag_buffer);
                lResult = ISO8583_DLL.bank_add_bit_data(15, ref tag_buffer[0], Leng);
                textBox3.Text += "15, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            }

            if (CheckDataElement(16, true))
            {   // 16 (Date, Conversion) 4AN.
                strDateTime = Extensions.Right("00" + intMonth, 2) + Extensions.Right("00" + intDay, 2);
                Leng = strCopy(strDateTime, ref tag_buffer);
                lResult = ISO8583_DLL.bank_add_bit_data(16, ref tag_buffer[0], Leng);
                textBox3.Text += "16, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            }

            if (CheckDataElement(18, true))
            {   // 18 (merchant type) 4N, "5311"  (Means "Department Stores" according to VISA MCC)
                Leng = strCopy("5499", ref tag_buffer);
                lResult = ISO8583_DLL.bank_add_bit_data(18, ref tag_buffer[0], Leng);
                textBox3.Text += "18, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            }

            if (CheckDataElement(22, true))
            {
                // 22 (POS entry mode) 3N, tag 9F39 ("05" as defined in terminal.txt) + 3rd nibble="1"
                Leng = strCopy(PPDE22POSEntryModeCombo.SelectedItem + "1", ref tag_buffer);     // 05: Chip card entry, 1: PIN enabled
                lResult = ISO8583_DLL.bank_add_bit_data(22, ref tag_buffer[0], Leng);
                textBox3.Text += "22, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            }

            if (CheckDataElement(23, true))
            {   // 23 (PAN sequence number) 3 N, 4-bit BCD (unsigned packed); 2 bytes, tag 5F34
                Leng = GetTagData(TAG_DATA_TXT, "5F34", ref tag_buffer);
                if (Leng > 0)
                {
                    byte[] tmp = new byte[3];
                    tmp[2] = tag_buffer[1];
                    tmp[1] = tag_buffer[0];
                    tmp[0] = 0x30;
                    lResult = ISO8583_DLL.bank_add_bit_data(23, ref tmp[0], Leng + 1);
                }
                else
                {   // hardcode for test
                    Leng = strCopy(TextDE23.Text, ref tag_buffer);
                    lResult = ISO8583_DLL.bank_add_bit_data(23, ref tag_buffer[0], Leng);
                }
                textBox3.Text += "23, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            }

            if (CheckDataElement(32, true))
            {   // 32 (Acquirer ID) max 11N, tag 9F1, "00000000001" as defined in A0000000031010.txt
                Leng = strCopy("123456", ref tag_buffer);
                lResult = ISO8583_DLL.bank_add_bit_data(32, ref tag_buffer[0], Leng);
                textBox3.Text += "32, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            }

            if (CheckDataElement(35, true))
            {   // 35 (Track 2 data) max 37N, tag 57
                Leng = GetTagData(TAG_DATA_TXT, "57", ref tag_buffer);
                if (Leng > 0)
                {
                    if (tag_buffer[Leng - 1] == 0x46) //maybebug
                        Leng = Leng - 1;
                    lResult = ISO8583_DLL.bank_add_bit_data(35, ref tag_buffer[0], Leng);
                }
                else
                {
                    Command_str = "9F6B";
                    Array.Clear(tag_buffer, 0, tag_buffer.Length);
                    int rtn = PPDMain.DLL.Getter(UIC.PktType.STX, "T63", Command_str, out tag_buffer);
                    if (rtn == 0)
                        return;
                    if (rtn > 6)
                    {
                        if (tag_buffer[rtn - 1] == Convert.ToInt32('F'))
                            Leng = rtn - 15;
                        else
                            Leng = rtn - 14;
                        
                        lResult = ISO8583_DLL.bank_add_bit_data(35, ref tag_buffer[14], Leng);
                    }
                }
                textBox3.Text += "35, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            }

            if (CheckDataElement(37, true))
            {   // 37 (Retrieval referal number) 12N, MMDDhh from field7, + field 11
                strDateTime = Extensions.Right("00" + intMonth, 2) + Extensions.Right("00" + intDay, 2) + Extensions.Right("00" + intHour, 2);
                Leng = strCopy(strDateTime.Substring(0, 6) + strTraceNumber, ref tag_buffer);
                lResult = ISO8583_DLL.bank_add_bit_data(37, ref tag_buffer[0], Leng);
                textBox3.Text += "37, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            }

            if (CheckDataElement(41, true))
            {   // 41 (Terminal identification) 8ANS, tag 9f1c,"SmartPOS" as defined in terminal.txt
                Leng = strCopy("SmartPOS", ref tag_buffer);
                lResult = ISO8583_DLL.bank_add_bit_data(41, ref tag_buffer[0], Leng);
                textBox3.Text += "41, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            }

            if (CheckDataElement(42, true))
            {   // 42 (Card acceptor ID) 15ANS, tag 9f16, "123456789012345" as defined in terminal.txt
                Leng = strCopy("123456789012345", ref tag_buffer);
                lResult = ISO8583_DLL.bank_add_bit_data(42, ref tag_buffer[0], Leng);
                textBox3.Text += "42, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            }

            if (CheckDataElement(43, true))
            {   // 43 (Card acceptor name and loc) 40ANS, "Uniform Industrial Corp.    TUCHENG   TW"
                Leng = strCopy("Uniform Industrial Corp.    TUCHENG   TW", ref tag_buffer);
                lResult = ISO8583_DLL.bank_add_bit_data(43, ref tag_buffer[0], Leng);
                textBox3.Text += "43, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            }

            if (CheckDataElement(48, true))
            {   // 48 (Additional Data - Private) 14N.
                if (OptDE48List.Checked == true)
                    strAdditionalData = PPDE48TCCCombo.SelectedItem + "710204";
                else
                    strAdditionalData = TextDE48.Text;
                Leng = strCopy(strAdditionalData, ref tag_buffer);
                lResult = ISO8583_DLL.bank_add_bit_data(48, ref tag_buffer[0], Leng);
                textBox3.Text += "48, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            }

            if (CheckDataElement(49, true))
            {   // 49 (Currency code, transaction) 3N, tag 5F2A
                 //leng = GetTagData(TAG_DATA_TXT, "5F2A", tag_buffer)    'query smartcard will get 4 bytes with leading 0
                Leng = strCopy(Active_Currency_Code, ref tag_buffer);   //use currency code in AP instead
                if (Leng > 0)
                {
                    lResult = ISO8583_DLL.bank_add_bit_data(49, ref tag_buffer[0], Leng);
                    textBox3.Text += "49, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
                }
            }

            if (CheckDataElement(51, true))
            {   // 51 (Currency Code, Cardholder Billing) 3AN.
                Leng = strCopy("978", ref tag_buffer);
                lResult = ISO8583_DLL.bank_add_bit_data(51, ref tag_buffer[0], Leng);
                textBox3.Text += "51, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            }

            if (CheckDataElement(53, true))
            {   // 53 (Security Related Control Information) 16AN.
                Leng = strCopy("9701100001000000", ref tag_buffer);
                lResult = ISO8583_DLL.bank_add_bit_data(53, ref tag_buffer[0], Leng);
                textBox3.Text += "53, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            }

            if (CheckDataElement(54, true))
            {   // 54 (Additional Amounts)
                // DE 54 SF1(N2)+SF2(N2)+SF3(N3)+SF4(AN13)
                Leng = strCopy("0040840D000000002000", ref tag_buffer);
                //leng = strCopy("0040840FDF0F0F0F0F0F0F0F0F2F0F0F0", tag_buffer)
                //Leng = strCopy("F0F0F4F0F9F7F844F0F0F0F0F0F0F0F0F2F0F0F0", tag_buffer)
                lResult = ISO8583_DLL.bank_add_bit_data(54, ref tag_buffer[0], Leng);
                textBox3.Text += "54, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            }

            if (CheckDataElement(55, true))
            {
                Leng = GetField55Data_BankNet(ref tag_buffer);
                if (Leng < 0)
                {
                    parseErr("Field55");
                    return;
                }
                else
                {
                    lResult = ISO8583_DLL.bank_add_bit_data(55, ref tag_buffer[0], Leng);
                    textBox3.Text += "55, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
                }
            }

            if (CheckDataElement(61, true))
            {   // 61 (Point-of-Service(POS)Data) 52N.
                Leng = strCopy("10000100033008860000000237", ref tag_buffer);
                lResult = ISO8583_DLL.bank_add_bit_data(61, ref tag_buffer[0], Leng);
                textBox3.Text += "61, " + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            }

            if (CheckDataElement(63, true))
            {   // 63 (Network Data) 24N.
                Leng = strCopy("MCC123456789", ref tag_buffer);
                lResult = ISO8583_DLL.bank_add_bit_data(63, ref tag_buffer[0], Leng);
                textBox3.Text += "63." + Encoding.Default.GetString(tag_buffer) + ", " + Leng + "\r\n";
            }

            //pack ISO8583
            //Leng = Module1.HexStrToBin("711925", ref srcAddr);
            Leng = Module1.HexStrToBin("", ref srcAddr);
            vip_out_leng = 1024;
            lResult = ISO8583_DLL.bank_data_pack(ref srcAddr[0], ref vip_packet_out[0], ref vip_out_leng);

            if (lResult != 0)
            {
                Text1.Text += "vip_data_pack( ) failed with errcode " + lResult + "\r\n";
                BtnAck.Enabled = true;
                BtnNoAck.Enabled = true;
                txt_extHostSimAddr.Enabled = false;
                txt_extHostSimPort.Enabled = false;
                return;
            }

            // debug output
            dbgVIPpacket = Module1.BinToHexStr(vip_packet_out, vip_out_leng);
            Text1.Text += "\r\n" + dbgVIPpacket + "\r\n\r\n";
            return;
            //clean bitmap in DLL
            ISO8583_DLL.bank_bitmap_clean();

            //initialize flags for Winsock event handlers
            vip_send_ok = 0;
            vip_receive_ok = 0;

            //Connect to Host
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Text1.Text += "Connecting " + txt_extHostSimAddr.Text + ":" + txt_extHostSimPort.Text + "\r\n";
            try
            {
                socket.Connect(txt_extHostSimAddr.Text, Convert.ToInt32(txt_extHostSimPort.Text)); // 1.設定 IP:Port 2.連線至伺服器
            }
            catch (Exception)
            {
                MessageBox.Show("Please make sure the setting of the Address and the Port are correct.", "Connect failed");
                Module1.Online_Response = "";
                return;
            }
            Text1.Text += "Connected.\r\n";
            Text1.Text += "Sending VIP packet...\r\n";
            socket.Send(vip_packet_out);
            Text1.Text += "Data packet sent.\r\n";
            Application.DoEvents();

            Array.Clear(vip_packet_in, 0, vip_packet_in.Length);
            vip_in_leng = socket.Receive(vip_packet_in);
            Text1.Text += "Getting response...(" + vip_in_leng + "bytes.)";
            dbgVIPpacket = Module1.BinToHexStr(vip_packet_in, vip_in_leng);
            Text1.Text += "\r\n" + dbgVIPpacket + "\r\n\r\n";
            socket.Close();

            //parse ISO8583 message into bitmap
            lResult = ISO8583_DLL.bank_data_unpack(ref srcAddr[0], ref destAddr[0], ref vip_packet_in[0], vip_in_leng, ref bitFieldIdx[0]);
            if (lResult != 0)
            {
                Text1.Text += "\r\n" + "VIP packet unpack failed (errcode=" + lResult + ")" + "\r\n";
                BtnAck.Enabled = true;
                BtnNoAck.Enabled = true;
                txt_extHostSimAddr.Enabled = false;
                txt_extHostSimPort.Enabled = false;
                return;
            }

            // debug
            Text1.Text += "Field returned: \r\n";
            for (i = 0; i < 128; i++)
                if (bitFieldIdx[i] > 0)
                    Text1.Text += i + ", ";

            Text1.Text += "\r\n\r\n";

            // get ARC, IAD or Issuer script if exitsts.
            // ARC_Code = field 39
            if (bitFieldIdx[39] > 0)
            {
                //initialize buffer and set maximum buffer length
                Array.Clear(tag_buffer, 0, tag_buffer.Length);
                Leng = 1024;
                // query bitfield data
                lResult = ISO8583_DLL.bank_get_bit_data(39, ref tag_buffer[0], ref Leng);
                strARC = Module1.BinToHexStr(tag_buffer, Leng);
                Text1.Text += "ARC = " + strARC + "\r\n";
            }
            else
            {
                Text1.Text += "ARC not exist!, exit";
                BtnAck.Enabled = true;
                BtnNoAck.Enabled = true;
                txt_extHostSimAddr.Enabled = false;
                txt_extHostSimPort.Enabled = false;
                return;
            }

            // IAD_Code = field 55, tag 91
            if (bitFieldIdx[55] > 0)
            {
                tag_buffer = new byte[1024];
                Leng = 1024;
                lResult = ISO8583_DLL.bank_get_bit_data(55, ref tag_buffer[0], ref Leng);
                dbgVIPpacket = Module1.BinToHexStr(tag_buffer, Leng);
                // search "91"
                i = dbgVIPpacket.IndexOf("91");
                if (i > 0)
                {
                    // tag 91 may contain maximum 16 bytes
                    strIADtmp = dbgVIPpacket.Substring(i);
                    // get ral length of IAD
                    i = Convert.ToInt32(strIADtmp.Substring(2, 2), 16);
                    strIAD = strIADtmp.Substring(4, i * 2);
                    Text1.Text += "IAD = " + strIAD + "\r\n";
                }
                else
                {
                    Text1.Text += "IAD not exist!, exit";
                    BtnAck.Enabled = true;
                    BtnNoAck.Enabled = true;
                    txt_extHostSimAddr.Enabled = false;
                    txt_extHostSimPort.Enabled = false;
                }
            }

            strIss71 = "";
            strIss72 = "";

            Module1.Online_Response = strARC + Module1.charStr(0x1A) + strIAD + Module1.charStr(0x1A) + strIss71 + Module1.charStr(0x1A) + strIss72;

            // clean bitmap in DLL after parsing
            ISO8583_DLL.bank_bitmap_clean();

            // set control falg to inform EMV_TestForm that we are done.
            Text1.Text += "Host response sent to PIN Pad.\r\n";
            BankACK = true;
            return;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TextDE48.Text = PPDE22POSEntryModeCombo.SelectedItem + "1";
        }

        private int construct_PP_DE3_data(byte[] Dest)
        {
            string strProcessingCode = "", Command_str;
            int Leng, rtn;
            byte[] temp_buff = new byte[1024]; ;

            Leng = GetTagData(TAG_DATA_TXT, "9C", ref temp_buff);
            if (Leng > 0)
            {
                strProcessingCode = Encoding.Default.GetString(temp_buff);
                strProcessingCode = strProcessingCode.Substring(0, 2);
            }
            else
            {
                Command_str = "9C";
                Array.Clear(Module1.receive_buf, 0, Module1.receive_buf.Length);
                rtn = PPDMain.DLL.Getter(UIC.PktType.STX, "T63", Command_str, out Module1.receive_buf);
                if (rtn <= 0)
                    return 0;

                if (rtn > 6)
                {
                    strProcessingCode = Encoding.Default.GetString(Module1.receive_buf);
                    strProcessingCode = strProcessingCode.Substring(13, 2);
                }
            }

            strProcessingCode += "0000";

            Leng = strCopy(strProcessingCode, ref Dest);
            return Leng;
        }

        private void ExternHostSimOption0_CheckedChanged(object sender, EventArgs e)
        {
            txt_extHostSimAddr.Enabled = false;
            txt_extHostSimPort.Enabled = false;
        }

        private void parseErr(string code)
        {
            Text1.Text += "Cannot get tag " + code + ", end.\n"; 
        }

        private int GetField55Data(ref byte[] outBuffer)
        {
            int i, j, k, tagLen;
            byte[] tagBuf = new byte[256];
            outBuffer = new byte[256];

            j = 0;

            if (Module1.EMVL2_Tag_List == null)
                return j;
            else
                for (i = 0; i < Module1.EMVL2_Tag_List.Length; i++)
                {
                    tagLen = Module1.HexStrToBin(Module1.EMVL2_Tag_List[i], ref tagBuf);
                    for (k = 0; k < tagLen; k++)
                    {
                        outBuffer[j] = tagBuf[k];
                        j++;
                    }
                    Application.DoEvents();
                }
            return j;
        }

        private void Init_Field55_Tag()
        {
            Field55_Tag = new string[]  {
                "9F26", "9F27", "9F10", "9F37", "9F36",
                "95", "9A", "9C", "9F02", "5F2A", "82", "9F1A",
                "9F34", "9F33", "9F35", "9F1E", "9F53", "84",
                "9F09", "9F41", "9F03"
            };
        }

        private int GetField55Data_BankNet(ref byte[] outBuffer)
        {
            int i, rtn;
            byte[] RcvBuf = new byte[256];
            string Command_str, result_str, out_str;
            string[] split_1c_str;
            int total_len, data_len;

            Init_Field55_Tag();

            result_str = "";
            out_str = "";
            total_len = 0;

            for (i = 0; i < Field55_Tag.Length; i++)
            {
                Command_str = Field55_Tag[i];
                Module1.SIOOutput("T63" + Command_str);
                Array.Clear(RcvBuf, 0, RcvBuf.Length);
                rtn = PPDMain.DLL.Getter(UIC.PktType.STX, "T63", Command_str, out RcvBuf);
                if (rtn <= 0)
                    return 0;

                if (RcvBuf[2] != Convert.ToInt32('F') && rtn != 3)
                {
                    result_str = Encoding.Unicode.GetString(RcvBuf);
                    split_1c_str = result_str.Split(new string[] { Module1.charStr(0x1C) }, StringSplitOptions.None);
                    data_len = rtn - split_1c_str[0].Length + split_1c_str[1].Length - 2;
                    split_1c_str[2] = split_1c_str[2].Substring(0, data_len);
                    split_1c_str[0] = split_1c_str[0].Substring(7, split_1c_str[0].Length - 5);
                    out_str += split_1c_str[0] + split_1c_str[1] + split_1c_str[2];
                    total_len += split_1c_str[1].Length + split_1c_str[2].Length + data_len;
                }
            }
            total_len = Module1.HexStrToBin(out_str, ref outBuffer);
            return total_len;
        }

        private bool CheckDataElement(int index, bool Get_Status)
        {   
            foreach (Control ctrl in groupBox5.Controls)
            {
                if (ctrl.Name == "checkBox" + index)
                {
                    CheckBox ck = (CheckBox)ctrl;
                    if (Get_Status)
                        return ck.Checked;
                    else
                        ck.Checked = true;
                }
            }
            return false;
        }
    }
}