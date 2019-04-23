using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.IO;
using System.Collections;

namespace PP791

{

    static class Extensions
    {
        /// <summary>
        /// Get substring of specified number of characters on the right.
        /// </summary>
        public static string Right(this string value, int length)
        {
            if (String.IsNullOrEmpty(value)) return string.Empty;

            return value.Length <= length ? value : value.Substring(value.Length - length);
        }
    }
    public class Module1
    {
        public static delMainFormMonitor Monitor;
        //public static delMainFormBtnDisable Btndisable;
        //public static detect_newmode Detectmode;
        public const int SM_CXSCREEN = 0;
        public const int SM_CYSCREEN = 1;
        public static string path = Environment.CurrentDirectory + @"\Transaction_Record.text";


        ////' ****************Use for MAC Test***************
        public string SelectMMK;  //'Used In the Z66 test for text MAC master key ID (add in Z66 command)
        //public string strMACwk;   //'Used In the Z66 test for MAC session key
        public int iActiveMACmk;  //'Used In the Z66 test for MAC master key ID (internal)

        //////////////////////////////////////////
        //BankSideForm (for Lv2 online simulation)
        ///////////////////////////////////////////
        static public string[] EMVL2_Tag_List;
        static public int EMVL2_Tag_Num;
        public string PCD_Currency_Code;


        public static Byte[] receive_buf = new Byte[2048];
        public static string Online_Response;
        public static int Mode_flag;
        public static int Timeout;
        public static byte[] port;
        public static string terminal_msg = null;


        public static string strPack(string strInput)
        {
            string Buf1 = "";
            string Buf2;
            int i;
            for (i = 0; i < strInput.Length; i = i + 2)
            {
                Application.DoEvents();
                Buf2 = strInput.Substring(i, 2);
                //Buf2 =Encoding.ASCII.GetString(con);
                Buf1 = Buf1 + charStr(Convert.ToByte(Buf2, 16));
            }
            return Buf1;
        }

        /*-----------------------------AddDelSequence1---------------------------------*/
        private string AddDelSequence1(int SetMode, string Seq)
        {

            long temp; // ' 加減 sequencel
            string result;
            //if (Seq == null)
            //{
            //    Seq = "0";
            //}
            temp = val(Seq);

            if (SetMode == 0)
            {
                temp = temp - 1;
            }
            else
            {
                temp = temp + 1;
            }
            //MessageBox.Show((temp.Length).ToString());
            result = "0000000" + temp.ToString();

            return (result.Substring(result.Length - 7));


        }
        /*--------------------------------------------------------------------*/
        public string StrLeft(string s, int length)
        {
            return s.Substring(0, length);
        }

        public string StrRigth(string s, int length)
        {
            return s.Substring(s.Length - length);
        }

        public string StrMid(string s, int start, int length)
        {
            return s.Substring(start, length);
        }

        /*------------------------------val:字串轉數值----------------------------------------*/
        public static int val(string str)
        {
            if (str == null)
            {
                //MessageBox.Show("rcv_str=null");
                return 0;
            }
            string rcv_str;
            rcv_str = str.Replace(" ", "");
            //Console.WriteLine(str);
            string numseq = "";
            char[] input = rcv_str.ToCharArray();
            int i = 0;
            bool isnumber = int.TryParse(rcv_str, out i);
            bool IsstrEqNum = false;
            if (isnumber == false)
            {
                i = 0;

                //Console.WriteLine(i);

                while (i < input.Length)
                {
                    if (char.IsNumber(input[i]))
                    {
                        IsstrEqNum = true;
                        numseq += input[i];

                        Console.WriteLine(i);
                        //Console.WriteLine(char.IsNumber(input[]));
                        //i++;
                        if (i < input.Length - 1)
                        {
                            if ((char.IsNumber(input[i + 1]) == false))
                            {
                                break;
                            }
                        }
                    }
                    i++;
                }
                if (IsstrEqNum == false)
                {
                    numseq = "0";
                }
                numseq = numseq.Replace(" ", "");
                return int.Parse(numseq);
            }
            else { return i; }
        }

        /************************************************************************************/
        public static string BinToHexStr(byte[] baInPut, long nLen)
        {
            string Buf = BitConverter.ToString(baInPut).Replace("-", "");
            string rs_Buf = "";
            for (int i = 0; i < nLen * 2; i++)
            {
                if(Buf[i] <= 15)
                    rs_Buf += "0" + Buf[i];
                else
                    rs_Buf += Buf[i];
            }
            
            return rs_Buf;
        }


        public static int HexStrToBin(string strInput, ref byte[] baOutPut)
        {
            int i, j;
            int nStrLen;
            string Buf;

            if (strInput == null)
                return 0;

            nStrLen = strInput.Length;
            if (nStrLen % 2 == 1)
            {
                nStrLen++;
                strInput += "0";
            }

            baOutPut = new byte[nStrLen / 2];

            j = 0;
            for(i = 1; i <= nStrLen; i += 2)
            {
                Application.DoEvents();
                Buf = strInput.Substring(i - 1, 2);
                baOutPut[j++] = Convert.ToByte(Convert.ToInt32(Buf, 16));
            }
            return nStrLen / 2;
        }

        /************************************************************************************/
        public static void SIOInput(int RtnData, int SelectF, byte[] InputBuff)
        {
            int i;
            string temp;

            //MainForm.io(MainForm.msgCollection, "\r\n" + "PC <-- PIN Pad(API)" + "\r\n");
            //MonitorIO("\r\n" + "PC <-- PIN Pad(API)" + "\r\n");
            if (SelectF == 0)        // 只回1個字   1 ->ok  , 小於 0 -> fail)
            {
                if (RtnData < 0)
                    //MainForm.io(MainForm.msgCollection, RtnData.ToString() + "(Error)");
                    //MonitorIO(RtnData.ToString() + "(Error)");
                    Monitor("PINPAD", RtnData.ToString() + "(Error)");
                else
                    //MainForm.io(MainForm.msgCollection, RtnData.ToString() + "(Completed)");
                    //MonitorIO(RtnData.ToString() + "(Completed)");
                    Monitor("PINPAD", RtnData.ToString() + "(Completed)");
            }
            else
            {
                if (RtnData < 0)
                    //MainForm.io(MainForm.msgCollection, RtnData.ToString());
                    //MonitorIO(RtnData.ToString());
                    Monitor("PINPAD", RtnData.ToString());
                else
                {

                    temp = null;

                    for (i = 0; i < InputBuff.Length; i++)
                    {
                        Application.DoEvents();
                        if (InputBuff[i] < 0x20)
                            if (InputBuff[i] < 0x10)
                                temp = temp + "<0" + InputBuff[i].ToString("X") + ">";
                            else
                                temp = temp + "<" + InputBuff[i].ToString("X") + ">";
                        else
                            temp = temp + Convert.ToChar(InputBuff[i]).ToString();
                    }
                    //MainForm.io(MainForm.msgCollection, temp + "\r\n");
                    //MonitorIO(temp + "\r\n");
                    Monitor("PINPAD", temp);
                }
            }
        }

        public static string ByteArrayToString(byte[] InputBuff)
        {
            int i;
            string rtnString = null;

            for (i=0; i < InputBuff.Length; i++)
            {
                if (InputBuff[i] < 0x20)
                    if (InputBuff[i] < 0x10)
                        rtnString = rtnString + "<0" + InputBuff[i].ToString("X") + ">";
                    else
                        rtnString = rtnString + "<" + InputBuff[i].ToString("X") + ">";
                else
                    rtnString = rtnString + Convert.ToChar(InputBuff[i]).ToString();
            }
            return rtnString;

        }
        /*****************************************************************/
        public static string charStr(byte input)
        {
            return Convert.ToChar(input).ToString();
        }

        public static string CheckStringRange(string StrData, string StrMin, string StrMax, string StrMin1, string StrMax1)
        {
            string functionReturnValue = null;
            string KeyData = null;
            string KeyString = null;
            // 碓認 strdata 是否在 strmin ~ strmax 內 or strmin1 ~strmax1 內
            int Len1 = 0;
            int i = 0;
            functionReturnValue = "";
            if ((!string.IsNullOrEmpty(StrData)))
            {
                KeyString = "";
                Len1 = StrData.Length;

                for (i = 0; i < Len1; i = i + 1)
                {
                    Application.DoEvents();
                    KeyData = StrData.Substring(i, 1);
                    byte[] Key_byte = Encoding.ASCII.GetBytes(KeyData);
                    if ((Key_byte[0] <= 0x39 && Key_byte[0] >= 0x30) || (Key_byte[0] <= 0x46 && Key_byte[0] >= 0x41))
                    {
                        KeyString = KeyString + KeyData;
                    }
                }
                functionReturnValue = KeyString;
            }
            return functionReturnValue;
        }
        public static void SIOOutput(string OutputData)
        {
            int i, Length1, tmp;
            string temp = null;
            byte[] data = Encoding.ASCII.GetBytes(OutputData);

            if (OutputData != null)
            {
                Length1 = OutputData.Length;
                for (i = 0; i < Length1; i++)
                {
                    tmp = data[i];
                    if (tmp < 0x20)
                    {
                        if (tmp < 0x10)
                            temp = temp + "<0" + tmp.ToString("X") + ">";  //小於 "F" 前面補"0"
                        else
                            temp = temp + "<" + tmp.ToString("X") + ">";
                    }
                    else
                        temp = temp + Convert.ToChar(tmp).ToString();
                }
                Monitor("PC", temp);
            }
        }
        public static void ppCancel()
        {
            PPDMain.DLL.ReadTimeout = 1000;
            SIOOutput("72");
            PPDMain.DLL.AbortTransaction();
            SIOOutput("T1C");
            PPDMain.DLL.EmvAbortTransaction();
            PPDMain.DLL.ReadTimeout = Module1.Timeout;
        }


        /*-------------------------------CloseCOM-----------------------------------------*/
        public static void CloseCOM()
        {
            PPDMain.DLL.Close();
        }
    }
}
