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
    public partial class EMV_Data_LoadForm : Form
    {
        bool ICC = false, RFID = true;

        public EMV_Data_LoadForm()
        {
            InitializeComponent();
        }
        private Stream fileStream = null;
        private string filename = null;
        string[] Filenames = null;
        private StreamReader ReadFile(string title, bool mutiselect = false)
        {
            string currentDirectory = Environment.CurrentDirectory;
            string path = currentDirectory + @"\Config\";
            StreamReader sr = null;
            filename = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            
            if (title.IndexOf("Public Key Setup") != -1)
                path += @"CAPK\";
            else
                if (title.IndexOf("PCD") != -1)
                    path += @"PCD\";
                else if (title.IndexOf("ICC") != -1)
                    path += @"ICC\";

            openFileDialog1.Multiselect = mutiselect;
            openFileDialog1.InitialDirectory = path;
            openFileDialog1.Filter = "KeyFile (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Title = title;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (mutiselect == true)
                    {
                        Filenames = openFileDialog1.FileNames;
                        return sr;
                    }
                    fileStream = openFileDialog1.OpenFile();
                    filename = openFileDialog1.FileName;
                    textBox1.Text = textBox1.Text + openFileDialog1.FileName + "\r\n";
                    sr = new StreamReader(fileStream);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
            return sr;
        }
        private string Check_val(string temp, out string output)
        {
            if (temp == "ans")
                output = "4";
            else if (temp == "an")
                output = "3";
            else if (temp == "a")
                output = "1";
            else if (temp == "b")
                output = "2";
            else if (temp == "cn")
                output = "5";
            else if (temp == "n")
                output = "6";
            else if (temp == "d")
                output = "5";
            else
                goto Format_Err;

            return null;

        Format_Err:
            output = temp;
            return "Format_Err";
        }
        private void Check_rsp(byte[] input)
        {
            string msg = Encoding.ASCII.GetString(input);
            if (input[3] == 0x31)
            {
                switch (input[4])
                {
                    case 0x31:
                        MessageBox.Show("PIN PAD Fatal Error");
                        break;
                    case 0x32:
                        MessageBox.Show("Message Format Error");
                        break;
                    case 0x33:
                        MessageBox.Show("TLV Format Error" + msg.Substring(msg.Length - 6, 6));
                        break;
                    case 0x34:
                        MessageBox.Show("TLV not valid" + msg.Substring(msg.Length - 6, 6));
                        break;
                }
            }
        }
        private int check_format_len(string[] tempStringArray)
        {
            int value_len = 0;
            // Check each value
            value_len = tempStringArray[0].Length;
            // Tag
            if (value_len > 8 || (value_len % 2) != 0)
            {
                goto Format_Err;
            }

            // Min Len
            value_len = tempStringArray[2].Length;
            if (value_len != 2)
                goto Format_Err;

            // Max Len
            value_len = tempStringArray[3].Length;
            if (value_len != 2)
                goto Format_Err;

            // or Flag
            value_len = tempStringArray[4].Length;
            if (value_len != 1)
                goto Format_Err;

            return 0;
        Format_Err:
            return -1;
        }
        private void change_format(string input, out string output)
        {
            switch (input)
            {
                case "A":
                    output = "1";
                    break;
                case "B":
                    output = "2";
                    break;
                case "AN":
                    output = "3";
                    break;
                case "ANS":
                    output = "4";
                    break;
                case "CN":
                    output = "5";
                    break;
                case "N":
                    output = "6";
                    break;
                default:
                    output = null;
                    break;
            }
        }

        private void btnTermData_Click(object sender, EventArgs e)
        {
            status_Default();
            string tmp;
            int rtn = 0;
            int val;
            string msg = "11";
            string[] temp = null;
            byte[] receive_buf = new byte[60];
            byte[] sha1value = new byte[20];
            string title = "ICC Terminal Data Load";
            StreamReader sr = ReadFile(title);
            if (sr == null)
            {
                return;
            }
            toolStripStatusLabel.Text = "Terminal Data";
            toolStripMessageLabel.Text = "Loading";
            while (!sr.EndOfStream)
            {
                tmp = sr.ReadLine();
                if (tmp.Trim() == "")
                    continue;
                val = Convert.ToChar(tmp.Substring(0, 1));
                if (val < 0x30 || val > 0x39)
                    goto OUT;
                textBox1.Text = textBox1.Text + tmp + "\r\n";
                temp = tmp.Split(new Char[] { ' ' });
                if (temp[0].Length % 2 != 0 || temp[0].Length > 8)
                {
                    goto Format_Err;
                }
                tmp = Check_val(temp[1], out temp[1]);
                if (tmp == "Format_Err")
                    goto Format_Err;
                msg = msg + Convert.ToChar(0x1a).ToString() + string.Join(Convert.ToChar(0x1c).ToString(), temp);

            }

        OUT:
            Module1.terminal_msg = msg;
            Module1.SIOOutput("T01" + msg);
            rtn = PPDMain.DLL.SetEmvTermConf(ICC, @filename, out receive_buf);
            if (rtn < 0)
                goto PINPAD_No_Resp;
            Module1.SIOInput(rtn, 1, receive_buf);

            if (receive_buf[3] != 0x30)
            {
                toolStripMessageLabel.Text = "Fail";
                MessageBox.Show("Load Fail");
                goto END;
            }

            toolStripMessageLabel.Text = "Success";
            goto END;
        PINPAD_No_Resp:
            toolStripMessageLabel.Text = "Fail";
            MessageBox.Show("PIN Pad No Response");
            goto END;
        Format_Err:
            toolStripMessageLabel.Text = "Fail";
            MessageBox.Show("File Format Error");
        END:
            sr.Close();
        }

        private void btnDataFormat_Click(object sender, EventArgs e)
        {
            status_Default();
            string tmp;
            int rec = -1;//, modulus_len = 0, value_len = 0;
            //int Index = 0, limit = 0;
            string msg = null;
            string[] tempStringArray = null;
            byte[] receive_buf = new byte[60];
            byte[] sha1value = new byte[20];
            string title = "ICC Data Format Setup";
            StreamReader sr = ReadFile(title);
            if (sr == null)
            {
                return;
            }
            toolStripStatusLabel.Text = "Data Format";
            toolStripMessageLabel.Text = "Loading";
            while (!sr.EndOfStream)
            {
                tmp = sr.ReadLine();

                if (tmp.Trim() == "")
                    continue;
                textBox1.Text = textBox1.Text + msg + "\r\n";
                tempStringArray = tmp.Split(new Char[] { ' ' });
                if (tempStringArray.Length != 5)
                    goto Format_Err;


                if (check_format_len(tempStringArray) != 0)
                    goto Format_Err;

                change_format(tempStringArray[1], out tempStringArray[1]);
                //tmp = string.Join(Convert.ToChar(0x1c).ToString(), tempStringArray);
                msg = msg + Module1.charStr(0x1A) + tempStringArray[0] + Module1.charStr(0x1C) + tempStringArray[1] +
                    tempStringArray[2] + tempStringArray[3] + tempStringArray[4];
            }

            msg = "1" + msg;
            rec = PPDMain.DLL.SetEmvDataFmtTbl(@filename, out receive_buf);
            Module1.SIOOutput("T07" + msg);
            if (rec < 0)
                goto PINPAD_No_Resp;

            Module1.SIOInput(rec, 1, receive_buf);

            if (receive_buf[3] == 0x31)
            {
                toolStripMessageLabel.Text = "Fail";
                MessageBox.Show("Load Fail");
            }
            else
                toolStripMessageLabel.Text = "Success";
            goto END;

        PINPAD_No_Resp:
            toolStripMessageLabel.Text = "Fail";
            MessageBox.Show("PIN Pad No Response");
            goto END;
        Format_Err:
            toolStripMessageLabel.Text = "Fail";
            MessageBox.Show("File Format Error");
        END:
            sr.Close();
        }

        private void btnApplData_Click(object sender, EventArgs e)
        {
            status_Default();
            string tmp, AID = null;
            int rtn = 0, val = 0, first_issue = 1, pkt_index = 1;
            int temp_msg_len = 0, msg_len = 0, len_limit = 488, total_pkt = 0;
            //string msg = "11";
            string msg = "";
            string[] temp = null;
            string ObjectString = null;
            byte[] receive_buf = new byte[60];
            byte[] sha1value = new byte[20];
            string title = "ICC Application Data Load";
            StreamReader sr = ReadFile(title);
            if (sr == null)
            {
                return;
            }
            //j- module2.file_name = title;
            toolStripStatusLabel.Text = "Appl. Data";
            toolStripMessageLabel.Text = "Loading";
            AID = sr.ReadLine();

            while (!sr.EndOfStream)
            {
                tmp = sr.ReadLine();
                if (tmp.Trim() == "")
                    continue;

                val = Convert.ToChar(tmp.Substring(0, 1));
                if (val < 0x30 || val > 0x39)
                    goto OUT;

                textBox1.Text = textBox1.Text + tmp + "\r\n";
                temp = tmp.Split(new Char[] { ' ' });
                if (temp[0].Length % 2 != 0 || temp[0].Length > 8)
                {
                    goto Format_Err;
                }
                tmp = Check_val(temp[1], out temp[1]);
                if (tmp == "Format_Err")
                    goto Format_Err;

                ObjectString = string.Join(Convert.ToChar(0x1c).ToString(), temp);
                temp_msg_len = msg_len + 1 + ObjectString.Length;
                if (temp_msg_len > len_limit)
                {
                    msg_len = 0;
                    total_pkt++;
                }
                msg_len = msg_len + 1 + ObjectString.Length;
                textBox1.Text = textBox1.Text + tmp + "\r\n";
            }
        OUT:
            if (msg_len % 488 != 0)
                total_pkt++;

            msg_len = 0;
            sr.DiscardBufferedData();
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            AID = sr.ReadLine();

            while (!sr.EndOfStream)
            {
                tmp = sr.ReadLine();
                if (tmp.Trim() == "")
                    continue;

                val = Convert.ToChar(tmp.Substring(0, 1));
                if (val < 0x30 || val > 0x39)
                    goto OUT2;

                textBox1.Text = textBox1.Text + tmp + "\r\n";
                temp = tmp.Split(new Char[] { ' ' });
                if (temp[0].Length % 2 != 0 || temp[0].Length > 8)
                {
                    goto Format_Err;
                }
                tmp = Check_val(temp[1], out temp[1]);
                if (tmp == "Format_Err")
                    goto Format_Err;

                ObjectString = string.Join(Convert.ToChar(0x1c).ToString(), temp);
                temp_msg_len = msg_len + 1 + ObjectString.Length;
                if (temp_msg_len > len_limit)
                {
                    if (first_issue != 1)
                    {
                        msg = pkt_index.ToString() + total_pkt.ToString() + msg;
                    }
                    else
                    {
                        first_issue = 0;
                        //'msg = "11" & msg   '取檔名當AID
                        msg = pkt_index.ToString() + total_pkt.ToString() + Module1.charStr(0x1a) + AID + msg;
                    }
                    Module1.SIOOutput("T05" + msg);
                    rtn = PPDMain.DLL.SetEmvAppConf(ICC, @filename, out receive_buf, "");
                    if (rtn < 0)
                    {
                        goto PINPAD_No_Resp;
                    }
                    Module1.SIOInput(rtn, 1, receive_buf);
                    Check_rsp(receive_buf);
                    msg_len = 0;
                    pkt_index = pkt_index + 1;
                    msg = null;
                }
                msg_len = msg_len + 1 + ObjectString.Length;
                msg = msg + Module1.charStr(0x1A) + ObjectString;
            }
        OUT2:
            if (msg.Length > 520)
                goto Format_Err;

            if (first_issue != 1)
            {
                msg = pkt_index.ToString() + total_pkt.ToString() + msg;
            }
            else
            {
                first_issue = 0;
                //'msg = "11" & msg   '取檔名當AID
                msg = pkt_index.ToString() + total_pkt.ToString() + Module1.charStr(0x1A) + AID + msg;
            }
            rtn = PPDMain.DLL.SetEmvAppConf(ICC, @filename, out receive_buf, "");
            Module1.SIOOutput("T05" + msg);
            if (rtn < 0)
            {
                goto PINPAD_No_Resp;
            }
            Module1.SIOInput(rtn, 1, receive_buf);
            Check_rsp(receive_buf);
            toolStripMessageLabel.Text = "Success";
            goto END;
        PINPAD_No_Resp:
            toolStripMessageLabel.Text = "Fail";
            MessageBox.Show("PIN Pad No Response");
            goto END;
        Format_Err:
            toolStripMessageLabel.Text = "Fail";
            MessageBox.Show("File Format Error");
        END:
            sr.Close();
        }

        private void btnPubKey_Click(object sender, EventArgs e)
        {
            status_Default();
            string tmp, msg1, msg2;
            string[] temp;
            string title = "ICC Public Key Setup";
            string[] file_arr = new string[] { "RID", "PKI", "HashAlgorithm", "HashValue",
                "PKAlgorithm", "PKLen","PKExp","PKModulus" };
            byte[] receive_buf;
            StreamReader sr = null;
            Filenames = null;
            ReadFile(title, true);
            if (Filenames == null) return;
            foreach (string filepath in Filenames)
            {
                msg1 = "";
                msg2 = "";
                receive_buf = null;
                sr = new StreamReader(@filepath, System.Text.Encoding.Default);
                textBox1.Text = textBox1.Text + filepath + "\r\n";
                if (sr == null)
                {
                    return;
                }
                toolStripMessageLabel.Text = filepath;
                int i = 0;
                while (!sr.EndOfStream)
                {
                    tmp = sr.ReadLine();
                    if (tmp.Trim() == "") continue;
                    temp = tmp.Split(new Char[] { ' ' });
                    if (temp.Length != 2) goto FormatError;
                    if (string.Compare(temp[0], file_arr[i]) == 0)
                    {
                        if (i == 7)
                        {
                            msg2 += temp[1];
                            sr.Close();
                            break;
                        }
                        msg1 += temp[1];
                        i++;
                    }
                    else { goto FormatError; }
                }
                textBox1.Text += msg1 + msg2 + "\r\n";
                //T031
                Module1.SIOOutput("T031" + msg1);
                int rtn = PPDMain.DLL.Getter(UIC.PktType.STX, "T03", "1" + msg1, out receive_buf);
                Module1.SIOInput(rtn, 1, receive_buf);
                if (rtn != 0 || receive_buf[4] != 0x30)
                {
                    toolStripMessageLabel.Text = "T031 Error response.";
                    return;
                }
                //T032
                Module1.SIOOutput("T032" + msg2);
                receive_buf = null;
                rtn = PPDMain.DLL.Getter(UIC.PktType.STX, "T03", "2" + msg2, out receive_buf);
                Module1.SIOInput(rtn, 1, receive_buf);
                if (rtn != 0 || receive_buf[4] != 0x30)
                {
                    toolStripMessageLabel.Text = "T032 Error response.";
                    return;
                }
                else
                {
                    toolStripMessageLabel.Text = "Success";
                }
            }
            toolStripStatusLabel.Text = "ALL Keys Loaded.";
            sr.Close();
            return;
        FormatError:
            toolStripMessageLabel.Text = "Fail";
            MessageBox.Show("File Format Error", "Error Message");
            sr.Close();
        }
        private void status_Default()
        {
            toolStripStatusLabel.Text = "";
            toolStripMessageLabel.Text = "";
            textBox1.Clear();

        }

        private void btnRfidTermData_Click(object sender, EventArgs e)
        {
            status_Default();
            string tmp;
            int rtn = 0;
            int val;
            string msg = "11";
            string[] temp = null;
            byte[] receive_buf = new byte[60];
            byte[] sha1value = new byte[20];
            string title = "PCD Terminal Data Load";
            StreamReader sr = ReadFile(title);
            if (sr == null)
            {
                return;
            }
            toolStripStatusLabel.Text = "Terminal Data";
            toolStripMessageLabel.Text = "Loading";
            while (!sr.EndOfStream)
            {
                tmp = sr.ReadLine();
                if (tmp.Trim() == "")
                    continue;
                val = Convert.ToChar(tmp.Substring(0, 1));
                if (val < 0x30 || val > 0x39)
                    goto OUT;
                textBox1.Text = textBox1.Text + tmp + "\r\n";
                temp = tmp.Split(new Char[] { ' ' });
                if (temp[0].Length % 2 != 0 || temp[0].Length > 8)
                {
                    goto Format_Err;
                }
                tmp = Check_val(temp[1], out temp[1]);
                if (tmp == "Format_Err")
                    goto Format_Err;
                msg = msg + Convert.ToChar(0x1a).ToString() + string.Join(Convert.ToChar(0x1c).ToString(), temp);

            }

        OUT:
            Module1.terminal_msg = msg;
            Module1.SIOOutput("T51" + msg);
            rtn = PPDMain.DLL.SetEmvTermConf(RFID, @filename, out receive_buf);
            if (rtn < 0)
                goto PINPAD_No_Resp;
            Module1.SIOInput(rtn, 1, receive_buf);

            if (receive_buf[3] != 0x30)
            {
                toolStripMessageLabel.Text = "Fail";
                MessageBox.Show("Load Fail");
                goto END;
            }

            toolStripMessageLabel.Text = "Success";
            goto END;
        PINPAD_No_Resp:
            toolStripMessageLabel.Text = "Fail";
            MessageBox.Show("PIN Pad No Response");
            goto END;
        Format_Err:
            toolStripMessageLabel.Text = "Fail";
            MessageBox.Show("File Format Error");
        END:
            sr.Close();
        }

        private void btnRfidApplData_Click(object sender, EventArgs e)
        {
            status_Default();
            string tmp, AID = null, KID = null, Txn_type = null;
            int rtn = 0, val = 0, first_issue = 1, pkt_index = 1;
            int temp_msg_len = 0, msg_len = 0, len_limit = 248, total_pkt = 0;
            string msg = "";
            string[] temp = null;
            string ObjectString = null;
            byte[] receive_buf = new byte[60];
            byte[] sha1value = new byte[20];
            string title = "PCD Application Data Load";
            StreamReader sr = ReadFile(title);
            if (sr == null)
            {
                return;
            }

            toolStripStatusLabel.Text = "Appl. Data";
            toolStripMessageLabel.Text = "Loading";

            while (!sr.EndOfStream)
            {
                tmp = sr.ReadLine();
                if (tmp.Trim() == "")
                    continue;

                if ((String.Compare(Convert.ToString(tmp.Substring(0, 4)), "NOTE") == 0))
                    goto OUT;

                textBox1.Text = textBox1.Text + tmp + "\r\n";
                temp = tmp.Split(new Char[] { ' ' });
                if (temp[0].Length % 2 != 0 || temp[0].Length > 8)
                {
                    goto Format_Err;
                }
                tmp = Check_val(temp[1], out temp[1]);
                if (tmp == "Format_Err")
                    goto Format_Err;

                if (String.Compare(temp[0], "9C") == 0)
                    Txn_type = temp[2];
                else if (String.Compare(temp[0], "40000040") == 0)
                    KID = temp[2];
                else if (String.Compare(temp[0], "9F06") == 0)
                    AID = temp[2];

                ObjectString = string.Join(Convert.ToChar(0x1c).ToString(), temp);
                temp_msg_len = msg_len + 1 + ObjectString.Length;
                if (temp_msg_len > len_limit)
                {
                    msg_len = 0;
                    total_pkt++;
                }
                msg_len = msg_len + 1 + ObjectString.Length;
            }
        OUT:
            if (msg_len % 488 != 0)
                total_pkt++;

            msg_len = 0;
            sr.DiscardBufferedData();
            sr.BaseStream.Seek(0, SeekOrigin.Begin);

            while (!sr.EndOfStream)
            {
                tmp = sr.ReadLine();
                if (tmp.Trim() == "")
                    continue;

                if ((String.Compare(Convert.ToString(tmp.Substring(0, 4)), "NOTE") == 0))
                    goto OUT2;

                temp = tmp.Split(new Char[] { ' ' });
                if (temp[0].Length % 2 != 0 || temp[0].Length > 8)
                {
                    goto Format_Err;
                }
                tmp = Check_val(temp[1], out temp[1]);
                if (tmp == "Format_Err")
                    goto Format_Err;

                if (String.Compare(temp[0], "9C") == 0)
                    Txn_type = temp[2];
                else if (String.Compare(temp[0], "40000040") == 0)
                    KID = temp[2];
                else if (String.Compare(temp[0], "9F06") == 0)
                    AID = temp[2];

                ObjectString = string.Join(Convert.ToChar(0x1c).ToString(), temp);
                temp_msg_len = msg_len + 1 + ObjectString.Length;
                if (temp_msg_len > len_limit)
                {
                    if (first_issue != 1)
                    {
                        msg = pkt_index.ToString() + total_pkt.ToString() + msg;
                    }
                    else
                    {
                        first_issue = 0;
                        msg = pkt_index.ToString() + total_pkt.ToString() + Module1.charStr(0x1a) + Txn_type + Module1.charStr(0x1a) + KID + Module1.charStr(0x1a) + AID + msg;
                    }
                    Module1.SIOOutput("T55" + msg);
                    rtn = PPDMain.DLL.SetEmvAppConf(RFID, @filename, out receive_buf, msg);
                    if (rtn < 0)
                    {
                        goto PINPAD_No_Resp;
                    }
                    Module1.SIOInput(rtn, 1, receive_buf);
                    Check_rsp(receive_buf);
                    msg_len = 0;
                    pkt_index = pkt_index + 1;
                    msg = null;
                }
                msg_len = msg_len + 1 + ObjectString.Length;
                msg = msg + Module1.charStr(0x1A) + ObjectString;
            }
        OUT2:
            if (msg.Length > 520)
                goto Format_Err;

            if (first_issue != 1)
            {
                msg = pkt_index.ToString() + total_pkt.ToString() + msg;
            }
            else
            {
                first_issue = 0;
                msg = pkt_index.ToString() + total_pkt.ToString() + Module1.charStr(0x1a) + Txn_type + Module1.charStr(0x1a) + KID + Module1.charStr(0x1a) + AID + msg;
            }
            Module1.SIOOutput("T55" + msg);
            rtn = PPDMain.DLL.SetEmvAppConf(RFID, @filename, out receive_buf, msg);
            if (rtn < 0)
            {
                goto PINPAD_No_Resp;
            }
            Module1.SIOInput(rtn, 1, receive_buf);
            Check_rsp(receive_buf);
            toolStripMessageLabel.Text = "Success";
            goto END;
        PINPAD_No_Resp:
            toolStripMessageLabel.Text = "Fail";
            MessageBox.Show("PIN Pad No Response");
            goto END;
        Format_Err:
            toolStripMessageLabel.Text = "Fail";
            MessageBox.Show("File Format Error");
        END:
            sr.Close();
        }

        private void btnRfidPubKey_Click(object sender, EventArgs e)
        {
            status_Default();
            string tmp, msg1, msg2;
            string[] temp;
            string title = "PCD Public Key Setup";
            string[] file_arr = new string[] { "RID", "PKI", "HashAlgorithm", "HashValue",
                "PKAlgorithm", "PKLen","PKExp","PKModulus" };
            byte[] receive_buf;
            StreamReader sr = null;
            Filenames = null;
            ReadFile(title, true);
            if (Filenames == null) return;
            foreach (string filepath in Filenames)
            {
                msg1 = "";
                msg2 = "";
                receive_buf = null;
                sr = new StreamReader(@filepath, System.Text.Encoding.Default);
                textBox1.Text = textBox1.Text + filepath + "\r\n";
                if (sr == null)
                {
                    return;
                }
                toolStripMessageLabel.Text = filepath;
                int i = 0;
                while (!sr.EndOfStream)
                {
                    tmp = sr.ReadLine();
                    if (tmp.Trim() == "") continue;
                    temp = tmp.Split(new Char[] { ' ' });
                    if (temp.Length != 2) goto FormatError;
                    if (string.Compare(temp[0], file_arr[i]) == 0)
                    {
                        if (i == 7)
                        {
                            msg2 += temp[1];
                            sr.Close();
                            break;
                        }
                        msg1 += temp[1];
                        i++;
                    }
                    else { goto FormatError; }
                }
                textBox1.Text += msg1 + msg2 + "\r\n";
                //T531
                Module1.SIOOutput("T531" + msg1);
                int rtn = PPDMain.DLL.Getter(UIC.PktType.STX, "T53", "1" + msg1, out receive_buf);
                Module1.SIOInput(rtn, 1, receive_buf);
                if (rtn != 0 || receive_buf[4] != 0x30)
                {
                    toolStripMessageLabel.Text = "T531 Error response.";
                    return;
                }
                //T532
                Module1.SIOOutput("T532" + msg2);
                receive_buf = null;
                rtn = PPDMain.DLL.Getter(UIC.PktType.STX, "T53", "2" + msg2, out receive_buf);
                Module1.SIOInput(rtn, 1, receive_buf);
                if (rtn != 0 || receive_buf[4] != 0x30)
                {
                    toolStripMessageLabel.Text = "T532 Error response.";
                    return;
                }
                else
                {
                    toolStripMessageLabel.Text = "Success";
                }
            }
            toolStripStatusLabel.Text = "ALL Keys Loaded.";
            sr.Close();
            return;
        FormatError:
            toolStripMessageLabel.Text = "Fail";
            MessageBox.Show("File Format Error", "Error Message");
            sr.Close();
        }
    }
}
