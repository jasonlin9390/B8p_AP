using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace PP791
{
    class ISO8583_DLL
    {
        //VisaNet/ISO8583 bitmap manulating functions
        [DllImport("vip_api.dll", EntryPoint = "vip_bitmap_init")]
        public static extern int vip_bitmap_init();
        [DllImport("vip_api.dll", EntryPoint = "vip_bitmap_clean")]
        public static extern int vip_bitmap_clean();
        [DllImport("vip_api.dll", EntryPoint = "vip_add_bit_data")]
        public static extern int vip_add_bit_data(int bit_id, ref byte data, int leng);
        [DllImport("vip_api.dll", EntryPoint = "vip_get_bit_data")]
        public static extern int vip_get_bit_data(int bit_id, ref byte data, ref int leng);
        [DllImport("vip_api.dll", EntryPoint = "vip_data_pack")]
        public static extern int vip_data_pack(ref byte src_addr, ref byte packBuf, ref int packedSize);
        [DllImport("vip_api.dll", EntryPoint = "vip_data_unpack")]
        public static extern int vip_data_unpack(ref byte src_addr, ref byte dest_addr, ref byte packBuf, int packedSize, ref byte bitFieldIdx);

        //BankNet/ISO8583 bitmap manulating functions
        [DllImport("bank_api.dll", EntryPoint = "bank_bitmap_init")]
        public static extern int bank_bitmap_init();
        [DllImport("bank_api.dll", EntryPoint = "bank_bitmap_clean")]
        public static extern int bank_bitmap_clean();
        [DllImport("bank_api.dll", EntryPoint = "bank_add_bit_data")]
        public static extern int bank_add_bit_data(int bit_id, ref byte data, int leng);
        [DllImport("bank_api.dll", EntryPoint = "bank_get_bit_data")]
        public static extern int bank_get_bit_data(int bit_id, ref byte data, ref int leng);
        [DllImport("bank_api.dll", EntryPoint = "bank_data_pack")]
        public static extern int bank_data_pack(ref byte src_addr, ref byte packBuf, ref int packedSize);
        [DllImport("bank_api.dll", EntryPoint = "bank_data_unpack")]
        public static extern int bank_data_unpack(ref byte src_addr, ref byte dest_addr, ref byte packBuf, int packedSize, ref byte bitFieldIdx);

    }
}
