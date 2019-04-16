using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PP791
{
    class Moduel2
    {
        public static Byte[] rec = new Byte[520];
        public static string file_name;
        static public string[] CVM_Result = new string[9];
        static public string[] CVM_Result_byte = new string[8];
        public static void init_str()
        {
            CVM_Result[0] = "Fail CVM processing";
            CVM_Result[1] = "Plaintext PIN Offline";
            CVM_Result[2] = "Enciphered PIN Online";
            CVM_Result[3] = "Plaintext PIN Offline and Signature";
            CVM_Result[4] = "Enciphered PIN Offline";
            CVM_Result[5] = "Encipherd PIN Offline and Signature";
            CVM_Result[6] = "Signature";
            CVM_Result[7] = "No CVM Required";
            CVM_Result[8] = "Unrecognized CVM";

            CVM_Result_byte[0] = "0";
            CVM_Result_byte[1] = "1";
            CVM_Result_byte[2] = "2";
            CVM_Result_byte[3] = "3";
            CVM_Result_byte[4] = "4";
            CVM_Result_byte[5] = "5";
            CVM_Result_byte[6] = "E";
            CVM_Result_byte[7] = "F";
        }
        static public bool getHexDigit(byte ch_hex, ref byte bt_hex)
        {
            if ((ch_hex >= 0x30) & (ch_hex <= 0x39))
            {
                bt_hex = (byte)(ch_hex - 0x30);
            }
            else if ((ch_hex >= 0x41) & (ch_hex <= 0x46))
            {
                bt_hex = (byte)(ch_hex - 0x41 + 10);
            }
            else if ((ch_hex >= 0x61) & (ch_hex <= 0x66))
            {
                bt_hex = (byte)(ch_hex - 0x61 + 10);
            }
            else
            {
                return false;
            }
            return true;
        }
        static public byte getHexChar(byte bt)
        {
            bt = (byte)(bt & 0x0f);
            if (bt >= 0 && bt <= 9)
                return (byte)(bt + 0x30);
            else
                return (byte)(0x41 + bt - 10);
        }
        static public byte[] DumpByteArr(byte[] arr, int size)
        {
            byte[] buff = null;
            int i, j;

            try
            {
                j = 0;
                if (size == 0)
                    return null;
                buff = new byte[(size * 3)];
                for (i = 0; i < size; i++)
                {
                    Application.DoEvents();
                    buff[j] = getHexChar((byte)(arr[i] / (16)));
                    j++;
                    buff[j] = getHexChar((byte)(arr[i] & 0xF));
                    j++;
                    buff[j] = 0x20;
                    j++;
                }

                buff[j - 1] = 0;
                return buff;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        static public int AsciiStr2HexByte(byte[] ascii_str, int ascii_len, ref byte[] hex_val, int hex_len)
        {
            byte hex_digit = 0;
            int i = 0;
            int j = 0;
            int k = 0;

            if (ascii_len > 0)
            {
                if ((ascii_len / 2 + (ascii_len % 2)) > hex_len)
                {
                    return 0;
                }

                if (ascii_len % 2 != 0)
                {
                    if (!getHexDigit(ascii_str[i], ref hex_digit))
                    {
                        return -1;
                    }
                    else
                    {
                        hex_val[j] = (byte)(0xf & hex_digit);
                    }
                    i++;
                    j++;
                }
            }

            for (k = i; k < ascii_len; k++)
            {
                Application.DoEvents();
                // Get most significant hex digit
                if (!getHexDigit(ascii_str[k], ref hex_digit))
                {
                    return -1;
                }
                else
                {
                    hex_digit = (byte)(hex_digit * ((byte)Math.Pow(2, 4)));
                    hex_val[j] = hex_digit;
                }
                k = k + 1;
                // This should never occur
                if (k >= ascii_len)
                    return -1;
                // Get least significant digit
                if (!getHexDigit(ascii_str[k], ref hex_digit))
                    return -1;
                else
                {
                    hex_val[j] = (byte)(hex_val[j] | hex_digit);
                    j = j + 1;
                }
            }
            hex_len = j;
            return 1;
        }
    }
}
