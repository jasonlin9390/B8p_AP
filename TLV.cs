using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PP791
{
    class TLV
    {
        public const int CLASS_MASK = 0xc0;
        public const int TYPE_MASK = 0x20;
        public const int PRIMITIVE_TYPE = 0x0;
        public const int CONSTRUCTED_TYPE = 0x20;
        public const long TLV_INVALID_TAG_VALUE = 0xfffe0002;
        public const long TLV_INVALID_LENGTH_VALUE = 0xfffe0003;
        public const long TLV_NO_DATA = 0xfffe0004;
        public const long ERR_MEMORY_ALLOC = 0xfffe0005;
        public static string[] TagList = new string[256];
        public static string[] ValueList = new string[256];
        public static int[] LengthList = new int[256];
        public struct TLV_s
        {
            public byte[] header;
            public int LengthIndex;
            public int LengthVal;
            public int tagval;
            public int ValueIndex;
            public int TLVLength;
            public bool STOP;
            public bool physical;
        }
        public static void InitializeTLV(ref TLV_s item)
        {
            item.LengthIndex = 0;
            item.ValueIndex = 0;
            item.tagval = 0;
            item.LengthVal = 0;
            item.TLVLength = 0;
            item.STOP = true;
        }
        public static void ResetTLV(ref TLV_s item)
        {
            if (item.header != null)
            {
                item.header = null;
            }
            InitializeTLV(ref item);
        }
        public static long parseTag(ref TLV_s item, ref byte[] header)
        {
            long functionReturnValue = 0;
            long taglen = 0, i = 0;

            item.tagval = header[0] & 0xff;

            if ((header[0] & 0x1f) == 0x1f)
            {
                if ((header[1] & 0x80) == 0x80)
                {
                    taglen = header[1] & 0x7F;
                    item.LengthIndex = 1;
                    do
                    {
                        item.tagval = item.tagval * (int)(Math.Pow(2, 8));
                        item.tagval = item.tagval | (header[i + 1] & 0xff);
                        item.LengthIndex++;
                        i++;
                    } while (i <= taglen);
                }
                else {
                    item.tagval = item.tagval * (int)(Math.Pow(2, 8));
                    item.tagval = item.tagval | (header[1] & 0xff);
                    item.LengthIndex = 2;
                }

            }
            else if (header[0] == 0x40)
            {
                item.tagval = item.tagval * (int)(Math.Pow(2, 24));
                item.tagval = item.tagval | ((header[1] & 0xff) * (int)(Math.Pow(2, 16))) | ((header[2] & 0xff) * (int)(Math.Pow(2, 8))) | ((header[3] & 0xff));
                item.LengthIndex = 4;

            }
            else
            {
                item.LengthIndex = 1;
            }

            functionReturnValue = 1;
            return functionReturnValue;
        }

        public static long SetTLV(ref TLV_s item, byte[] data_array, int length)
        {
            int curIndex = 0;
            long rtn = 0;
            long lengFld = 0;
            long i = 0;

            item.header = new byte[length];
            for (i = 0; i < length; i++)
            {
                Application.DoEvents();
                item.header[i] = data_array[i];
            }

            // Parse the tag
            rtn = parseTag(ref item, ref item.header);
            if (rtn != 1)
            {
                ResetTLV(ref item);
                return rtn;
            }

            curIndex = item.LengthIndex;
            if ((item.header[curIndex] & 0x80) == 0x80)
            {
                //'  The rest of the bits of the current byte represent a number of bytes
                //'  of the length field. EMV spec limit the size of the length field to
                //'  a maximum of 3 bytes (1 byte designate a size of the length field in
                //'  bytes, and the following 1 or two bytes designate the length itself
                lengFld = (item.header[curIndex] & 0x7F);
                if (lengFld > 2)
                {
                    ResetTLV(ref item);
                    return TLV_INVALID_LENGTH_VALUE;
                }

                item.LengthVal = 0;  //Initial Item->_LengthVal
                curIndex = curIndex + 1;
                for (i = curIndex; i < item.LengthIndex + lengFld + 2; i++)
                {
                    Application.DoEvents();
                    item.LengthVal = item.LengthVal * (2 ^ 8);
                    item.LengthVal = item.LengthVal | (item.header[curIndex] & 0xFF);
                }
                item.ValueIndex = curIndex;
            }
            else
            {
                item.LengthVal = (item.header[curIndex] & 0xFF);
                curIndex = curIndex + 1;
            }

            if (item.LengthVal != 0)
            {
                item.ValueIndex = curIndex;
                if ((curIndex + item.LengthVal) > length)
                {
                    ResetTLV(ref item);
                    return TLV_INVALID_LENGTH_VALUE;
                }
            }

            item.TLVLength = item.ValueIndex + item.LengthVal;

            return 1;
        }
        public static string FindTag(string tagval)
        {
            string functionReturnValue = null;
            int Index = 0;

            for (Index = 0; Index < 40; Index++)
            {
                Application.DoEvents();
                if (tagval == TagList[Index])
                {
                    functionReturnValue = ValueList[Index];
                    return functionReturnValue;
                }
            }
            functionReturnValue = "";
            return functionReturnValue;
        }
        public static bool ParseTLV(byte[] data, int data_len)
        {
            int Index = 0;
            int list_index = 0;
            int i = 0;
            TLV_s item = default(TLV_s);
            byte[] temp_str = null;
            byte[] temp_data = null;
            try
            {
                Index = 0;
                list_index = 0;
                TLV.InitializeTLV(ref item);
                while (Index < data_len)
                {
                    Application.DoEvents();
                    ResetTLV(ref item);
                    temp_data = new byte[data_len - Index];
                    for (i = 0; i < data_len - Index; i++)
                    {
                        Application.DoEvents();
                        temp_data[i] = data[Index + i];

                    }
                    if (SetTLV(ref item, temp_data, data_len - Index) == 0)
                    {
                        return false;
                    }
                    TLV.TagList[list_index] = "0x" + item.tagval.ToString("X");
                    if (temp_str != null)
                        temp_str = null;
                    temp_data = new byte[data_len - Index];
                    for (i = 0; i < item.LengthVal; i++)
                    {
                        Application.DoEvents();
                        temp_data[i] = item.header[i + item.ValueIndex];
                    }
                    temp_str = Moduel2.DumpByteArr(temp_data, item.LengthVal);
                    //''''''''''''''''''''''''' 加上能夠顯示A ~ F之功能
                    ValueList[list_index] = Encoding.ASCII.GetString(temp_str);
                    Debug.WriteLine(System.Text.Encoding.ASCII.GetString(temp_str));
                    LengthList[list_index] = item.LengthVal;
                    temp_str = null;
                    list_index = list_index + 1;
                    Index = Index + item.TLVLength;
                }
                ValueList[list_index] = "";
                TagList[list_index] = "";
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("TLV get exception");
                return true;
            }
        }
    }
}
