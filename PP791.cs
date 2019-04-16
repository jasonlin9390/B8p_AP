using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace UIC
{
    public enum ElementType //for DataTypeInQueue need to be public
    {
        EMPTY = 0,
        PKT_SI = 1,
        PKT_STX = 2,
        TRK = 3,
        PROC = 4,
    }

    public enum PktType
    {
        SI = 0, STX = 1
    }

    //public enum ErrorType
    //{
    //    NoError = 0,
    //    //GotNACK = 1,
    //    LRCFail = -1,
    //    NoRecvEOTInSITransmission = -2,
    //}

    public struct TrackData
    {
        public byte[] TrkDecodeResult;
        public byte[] TrkLuhnResult;
        public byte[] TrkDataLayout;
        public string[] ObfuscatedTrks;
        public string[] EncryptedTrks;
        public string[] EncryptedPans;
        public string EtbData;
    }

    public delegate void ByteArrayDelegate(ElementType type, byte[] buffer);

    #region Virtual COM Only

    public enum BaudRate
    { _1200 = 1, _2400, _4800, _9600, _19200, _38400, _57600, _115200 }

    public enum UartAttr { N81 = 1, E71, O71 }
    #endregion

    public class PP791
    {
        public enum UsbMode { ACM = 0, KBD = 1, MSR = 2 }

        public PP791()
        {
            InstanceofVC = new VirtualCom();
            InstanceofHID = new HID(0x6352);
        }

        #region Constants
        private readonly char STX = (char)0x02;
        private readonly char ETX = (char)0x03;
        private readonly char EOT = (char)0x04;
        private readonly char ACK = (char)0x06;
        private readonly char SO = (char)0x0e;
        private readonly char SI = (char)0x0f;
        private readonly char NAK = (char)0x15;
        private readonly char SUB = (char)0x1a;
        private readonly char FS = (char)0x1c;
        private readonly char NUL = (char)0x00;

        private readonly int ExecFail = -1;

        private readonly int NotRequestedTypeInBuffer = -3;
        private readonly int PacketNotAccepted = -4;
        private readonly int GotNACK = -15;

        private readonly int ReadEOTTimeOut = -100;
        private readonly int ReadACKTimeOut = -100;
        private readonly int ReadPacketTimeOut = -100;
        private readonly int ReadByteTimeOut = -100;
        #endregion

        private HID InstanceofHID;
        private VirtualCom InstanceofVC;

        private UsbMode _CommMode;
        public UsbMode CommMode
        {
            get
            {
                return _CommMode;
            }
            set
            {
                if (_IsOpen == false) _CommMode = value;
                else throw new System.Data.ReadOnlyException("CommMode");
            }
        }

        private bool _IsOpen = false;
        public bool IsOpen
        {
            get
            {
                return _IsOpen;
            }
        }

        public int ReadBufferSize { get; } = 2048;

        private int _ReadTimeout = 3000;
        public int ReadTimeout
        {
            get
            {
                return _ReadTimeout;
            }
            set
            {
                InstanceofHID.ReadTimeout = value;
                InstanceofVC.ReadTimeout = value;
                _ReadTimeout = value;
            }
        }

        public int WriteBufferSize { get; } = 2048;

        private int _WriteTimeout = 3000;
        public int WriteTimeout
        {
            get
            {
                return _WriteTimeout;
            }
            set
            {
                InstanceofHID.WriteTimeout = value;
                InstanceofVC.WriteTimeout = value;
                _WriteTimeout = value;
            }
        }

        static public readonly string version = "S21LGX1D";

        public int BytesToRead
        {
            get
            {
                return _CommMode != UsbMode.ACM ? InstanceofHID.BytesToRead : InstanceofVC.BytesToRead;
            }
        }

        public ElementType DataTypeInQueue
        {
            get
            {
                return _CommMode != UsbMode.ACM ? InstanceofHID.DataTypeInQueue : InstanceofVC.DataTypeInQueue;
            }
        }

        public ByteArrayDelegate DataReceived
        {
            set
            {
                //_DataReceived = value;
                if (_CommMode != UsbMode.ACM)
                    InstanceofHID.DataReceived = value;
                else
                    InstanceofVC.DataReceived = value;
            }
        }

        public int LastError
        {
            get
            {
                return _CommMode != UsbMode.ACM ? InstanceofHID.LastError : InstanceofVC.LastError;
            }
        }

        private bool _DataEventEnabled = true;
        public bool DataEventEnabled
        {
            get
            {
                return _DataEventEnabled;
            }
            set
            {
                if (value == true)
                {
                    if (_CommMode != UsbMode.ACM)
                        InstanceofHID.DataEventEnabled.Set();
                    else
                        InstanceofVC.DataEventEnabled.Set();
                }
                else
                {
                    if (_CommMode != UsbMode.ACM)
                        InstanceofHID.DataEventEnabled.Reset();
                    else
                        InstanceofVC.DataEventEnabled.Reset();
                }
                _DataEventEnabled = value;
            }
        }

        #region Virtual COM Only

        public string PortName
        {
            get
            {
                return InstanceofVC.SerialPort.PortName;
            }
            set
            {
                if (IsOpen == true) throw new System.Data.ReadOnlyException("Already opened, PortName set failed");
                if (string.Compare(value.Substring(0, 3), "COM") == 0 && int.Parse(value.Substring(3)) >= 0)
                    InstanceofVC.SerialPort.PortName = value;
                else throw new FormatException("PortName");
            }
        }

        public int BaudRate
        {
            get
            {
                return InstanceofVC.SerialPort.BaudRate;
            }
            set
            {
                if (IsOpen == true) throw new System.Data.ReadOnlyException("Already opened, BaudRate set failed");
                if (value > 115200 || value < 0) throw new FormatException("BaudRate");
                else InstanceofVC.SerialPort.BaudRate = value;
            }
        }

        public int Parity
        {
            get
            {
                return Convert.ToInt32(InstanceofVC.SerialPort.Parity);
            }
            set
            {
                if (IsOpen == true) throw new System.Data.ReadOnlyException("Already opened, Parity set failed");
                if (value >= Convert.ToInt32(System.IO.Ports.Parity.None) && value <= Convert.ToInt32(System.IO.Ports.Parity.Space)) InstanceofVC.SerialPort.Parity = (Parity)value;
                else throw new FormatException("Parity");
            }
        }

        public int DataBits
        {
            get
            {
                return InstanceofVC.SerialPort.DataBits;
            }
            set
            {
                if (IsOpen == true) throw new System.Data.ReadOnlyException("Already opened, DataBits set failed");
                InstanceofVC.SerialPort.DataBits = value;
            }
        }

        public int StopBits
        {
            get
            {
                return Convert.ToInt32(InstanceofVC.SerialPort.StopBits);
            }
            set
            {
                if (IsOpen == true) throw new System.Data.ReadOnlyException("Already opened, StopBits set failed");
                if (value > Convert.ToInt32(System.IO.Ports.StopBits.None) && value <= Convert.ToInt32(System.IO.Ports.StopBits.Two)) InstanceofVC.SerialPort.StopBits = (StopBits)value;
                else throw new FormatException("StopBits");
            }
        }

        public int SetCOMparam(BaudRate baudrate, UartAttr uartpara)
        {
            if (_CommMode == UsbMode.ACM && _IsOpen != true)
            {
                int ret = WritePacket(true, PktType.SI, Encoding.ASCII.GetBytes("13"), Encoding.ASCII.GetBytes("" + (int)baudrate + (int)uartpara));
                if (ret != 0) return ret;
                ret = ReadByte();
                if (ret == EOT) return PacketNotAccepted;//Packet was accepted by device, go on
                byte[] Recv = null;
                ret = ReadPacket(out Recv, PktType.SI);
                if (CheckEOT(2000)) return ret > 0 ? 0 : ret;
                else return ReadEOTTimeOut;
            }
            else
                return ExecFail;
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            if (_CommMode == UsbMode.ACM && _IsOpen == true)
                InstanceofVC.SerialPort.Write(buffer, offset, count);
        }
        #endregion

        #region Low-Level Communication


        public void Open(BaudRate baudrate, UartAttr uartattr)
        {
            if (_CommMode == UsbMode.ACM && _IsOpen == false)
            {
                InstanceofVC.SerialPort.BaudRate = 1200 * (int)Math.Pow(2, (int)baudrate - 1);
                switch (uartattr)
                {
                    case UartAttr.N81:
                        InstanceofVC.SerialPort.Parity = System.IO.Ports.Parity.None;
                        InstanceofVC.SerialPort.DataBits = 8;
                        break;
                    case UartAttr.E71:
                        InstanceofVC.SerialPort.Parity = System.IO.Ports.Parity.Even;
                        InstanceofVC.SerialPort.DataBits = 7;
                        break;
                    case UartAttr.O71:
                        InstanceofVC.SerialPort.Parity = System.IO.Ports.Parity.Odd;
                        InstanceofVC.SerialPort.DataBits = 7;
                        break;
                }
                InstanceofVC.SerialPort.StopBits = System.IO.Ports.StopBits.One;

                InstanceofVC.Open();//5, 115200, 0, 8, 0);
                _IsOpen = InstanceofVC.SerialPort.IsOpen;
                //return 1;
            }
            else throw new IOException("Already Opened or Wrong CommMode");
            //else
            //return ExecFail;
        }

        public void Open()
        {
            if (_CommMode != UsbMode.ACM && _IsOpen == false)
            {
                //InstanceofHID = new HID(0x6352); //, new StringDelegate(CommunicationInfo));
                InstanceofHID.SearchForHidDevice(0, HID.FILE_SHARE_READ | HID.FILE_SHARE_WRITE, HID.OpenHIDMode.Search);
                if (InstanceofHID.hasHidFeature)
                {
                    if (InstanceofHID.hasHidInput)
                        InstanceofHID.SearchForHidDevice(HID.GENERIC_READ | HID.GENERIC_WRITE, HID.FILE_SHARE_READ | HID.FILE_SHARE_WRITE, HID.OpenHIDMode.Read);
                    else InstanceofHID.SearchForHidDevice(HID.GENERIC_WRITE, HID.FILE_SHARE_READ | HID.FILE_SHARE_WRITE, HID.OpenHIDMode.Read);
                    _IsOpen = true;
                }
                else if (InstanceofHID.hasHidInput)
                {
                    InstanceofHID.SearchForHidDevice(HID.GENERIC_READ, HID.FILE_SHARE_READ | HID.FILE_SHARE_WRITE, HID.OpenHIDMode.Read);
                    _IsOpen = true;
                }
                else throw new IOException("Can't find appropriate device");
                //return 1;
            }
            else if (_CommMode == UsbMode.ACM && _IsOpen == false)
            {
                //InstanceofVC = new VirtualCom();
                InstanceofVC.Open();//5, 115200, 0, 8, 0);
                _IsOpen = InstanceofVC.SerialPort.IsOpen;
                //return 1;
            }
            else throw new IOException("Already Opened or Wrong CommMode");
            //else
            //return ExecFail;
        }

        public void Close()
        {
            if (_CommMode != UsbMode.ACM && _IsOpen == true) InstanceofHID.HIDDeviceClose();
            if (_CommMode == UsbMode.ACM && _IsOpen == true) InstanceofVC.Close();
            _IsOpen = false;
        }

        public int WritePacket(bool IsSync, PktType type, byte[] DataIn)
        {
            if (_CommMode != UsbMode.ACM && _IsOpen == true) return InstanceofHID.WritePacket(IsSync, type, DataIn);
            if (_CommMode == UsbMode.ACM && _IsOpen == true) return InstanceofVC.WritePacket(IsSync, type, DataIn);
            else return -1;
        }

        private int WritePacket(bool IsSync, PktType type, byte[] MsgId, byte[] data)
        {
            if (_CommMode != UsbMode.ACM && _IsOpen == true) return InstanceofHID.WritePacket(IsSync, type, MsgId, data);
            if (_CommMode == UsbMode.ACM && _IsOpen == true) return InstanceofVC.WritePacket(IsSync, type, MsgId, data);
            else return -1;
        }

        public int ReadByte()
        {
            if (_CommMode != UsbMode.ACM && _IsOpen == true) return InstanceofHID.ReadByte();
            if (_CommMode == UsbMode.ACM && _IsOpen == true) return InstanceofVC.ReadByte();
            else return -1;
        }

        private int ReadPacket(out byte[] data, PktType Type)
        {
            data = null;
            if (_CommMode != UsbMode.ACM && _IsOpen == true) return InstanceofHID.ReadPacket(out data, Type);
            if (_CommMode == UsbMode.ACM && _IsOpen == true) return InstanceofVC.ReadPacket(out data, Type);
            else return -1;
        }

        public int ReadPacket(out byte[] data)
        {
            data = null;
            if (_CommMode != UsbMode.ACM && _IsOpen == true) return InstanceofHID.ReadPacket(out data);
            if (_CommMode == UsbMode.ACM && _IsOpen == true) return InstanceofVC.ReadPacket(out data);
            else return -1;
        }

        private bool CheckEOT(int Timeout)
        {
            if (_CommMode != UsbMode.ACM && _IsOpen == true) return InstanceofHID.CheckEOT(Timeout);
            if (_CommMode == UsbMode.ACM && _IsOpen == true) return InstanceofVC.CheckEOT(Timeout);
            return false;
        }

        static public int GetReaderPID(out byte[] PID)
        {
            PID = null;
            //if(_IsOpen == false) return -1;
            //if (_CommMode != UsbMode.ACM)
            //{
            //    PID = InstanceofHID.GetReaderPID();
            //    Array.Reverse(PID);
            //    return 0;
            //}
            //else if (_CommMode == UsbMode.ACM)
            //{
            //    PID = InstanceofVC.GetReaderPID();
            //    if (PID == null) return -1;
            //    return 0;
            //}
            //else return -1;
            PID = HID.GetReaderPID();
            if (PID != null)
            {
                Array.Reverse(PID);
                return 0;
            }
            PID = VirtualCom.GetReaderPID();
            if (PID != null) return 0;
            return -1;
        }

        public void DiscardInBuffer()
        {
            if (_CommMode != UsbMode.ACM && _IsOpen == true) InstanceofHID.DiscardInBuffer();
            if (_CommMode == UsbMode.ACM && _IsOpen == true) InstanceofVC.DiscardInBuffer();
            //else return null;
        }

        public void DiscardOutBuffer()
        {
            if (_CommMode != UsbMode.ACM && _IsOpen == true) InstanceofHID.DiscardOutBuffer();
            if (_CommMode == UsbMode.ACM && _IsOpen == true) InstanceofVC.DiscardOutBuffer();
        }
        #endregion

        #region General Command

        public int SetReaderSN(byte[] SN)
        {
            if (SN == null || SN.Length > 16)
            {
                throw new ArgumentException(String.Format("SetReaderSN: SN length > 16 or SN is null"), "SN");
                //return ExecFail;
            }
            int ret = WritePacket(true, PktType.SI, Encoding.ASCII.GetBytes("05"), SN);
            if (ret == 0)
            {
                ret = ReadByte();
                if (ret != EOT) //Packet was accepted by device, go on
                {
                    byte[] Recv = null;
                    ret = ReadPacket(out Recv, PktType.SI);
                    if (CheckEOT(2000)) return ret > 0 ? 0 : ret;
                    else return ReadEOTTimeOut;
                }
                else return PacketNotAccepted;
            }
            else
                return ret;
        }

        public int GetReaderSN(out byte[] SN)
        {
            return Getter(PktType.SI, "06", "", out SN);
        }

        public bool PktCommTest()
        {
            int ret = WritePacket(true, PktType.SI, Encoding.ASCII.GetBytes("09"), Encoding.ASCII.GetBytes(""));
            if (ret != 0) return false;
            ret = ReadByte();
            if (ret == EOT) return false;//Packet was accepted by device, go on
            byte[] Recv = null;
            ret = ReadPacket(out Recv, PktType.SI);
            if (ret < 0) return false;
            Thread.Sleep(1000);
            Recv = new List<byte>(Recv).GetRange(2, Recv.Length - 2).ToArray();
            ret = WritePacket(true, PktType.SI, Encoding.ASCII.GetBytes("09"), Recv); //Encoding.ASCII.GetBytes('\x1A'+"PROCESSING")
            if (ret != 0) return false;
            ret = ReadByte();
            if (ret == EOT) return false;//Packet was accepted by device, go on
            Recv = null;
            ret = ReadPacket(out Recv, PktType.SI);
            if (ret < 0) return false;
            if (!CheckEOT(2000)) return false;
            return Recv[2] == 0x30;
        }

        public bool AliveTest()
        {
            int ret = WritePacket(true, PktType.SI, Encoding.ASCII.GetBytes("11"), Encoding.ASCII.GetBytes(""));
            if (ret != 0) return false;
            else return true;
        }

        public int SetReaderTime(DateTime time)
        {
            string stemp = time.ToString("yyyyMMdd" + ((int)DateTime.Now.DayOfWeek % 7) + "HHmmss");
            int ret = WritePacket(true, PktType.SI, Encoding.ASCII.GetBytes("18"), Encoding.ASCII.GetBytes(stemp));
            if (ret != 0) return ret;
            ret = ReadByte();
            if (ret == EOT) return PacketNotAccepted;//Packet was accepted by device, go on
            byte[] Recv = null;
            ret = ReadPacket(out Recv, PktType.SI);
            if (CheckEOT(2000)) return ret > 0 ? 0 : ret;
            else return ReadEOTTimeOut;
        }

        public int Getter(PktType type, string cmd, string parameters, out byte[] dataout)
        {
            dataout = null;
            int ret = WritePacket(true, type, Encoding.ASCII.GetBytes(cmd), Encoding.ASCII.GetBytes(parameters));
            if (ret != 0) return ret;
            ret = ReadByte();
            if (ret == EOT) return PacketNotAccepted;//Packet was accepted by device, go on
            byte[] Recv = null;
            ret = ReadPacket(out Recv, type);
            if (type == PktType.SI && !CheckEOT(2000)) return ReadEOTTimeOut;
            dataout = Recv;
            //new List<byte>(time).GetRange(3, time.Length).ToArray();
            return ret > 0 ? 0 : ret;
        }

        public int GetReaderTime(out byte[] time)
        {
            //string stemp = time.ToString("yyyyMMdd" + ((int)DateTime.Now.DayOfWeek % 7) + "HHmmss");
            return Getter(PktType.SI, "18", "", out time);
        }

        public int GetFWVersion(out byte[] version)
        {
            return Getter(PktType.SI, "19", "", out version);
        }

        private int SetParameters(PktType type, string cmd, string parameters)
        {
            int ret = WritePacket(true, type, Encoding.ASCII.GetBytes(cmd), Encoding.ASCII.GetBytes(parameters));
            if (ret != 0) return ret;
            ret = ReadByte();
            if (ret == EOT) return PacketNotAccepted;//Packet was not accepted by device
            byte[] Recv = null;
            ret = ReadPacket(out Recv, type);
            if (ret < 0) return ret;
            if (type == PktType.SI && !CheckEOT(2000)) return ReadEOTTimeOut;
            return Recv[2];
            //if (Recv[3] == 0x30) return Recv[3];// == '0'
            //else return ExecFail;
        }

        public int SendBeeperCmd(byte[] cmdbuffer)
        {
            int ret = SetParameters(PktType.SI, "1P", Encoding.ASCII.GetString(cmdbuffer));
            if (ret < 0) return ret;
            return ret == 0x30 ? 0 : ExecFail;
        }

        public int SendLedCmd(byte[] cmdbuffer)
        {
            int ret = SetParameters(PktType.SI, "1R", Encoding.ASCII.GetString(cmdbuffer));
            if (ret < 0) return ret;
            return ret == 0x30 ? 0 : ExecFail;
        }

        private int SetMode(string cmd, int mode)
        {
            int ret = WritePacket(true, PktType.SI, Encoding.ASCII.GetBytes(cmd), Encoding.ASCII.GetBytes("" + mode));
            if (ret != 0) return ret;
            ret = ReadByte();
            if (ret == EOT) return PacketNotAccepted;//Packet was accepted by device, go on
            byte[] Recv = null;
            ret = ReadPacket(out Recv, PktType.SI);
            if (ret < 0) return ret;
            if (!CheckEOT(2000)) return ReadEOTTimeOut;
            Recv = new List<byte>(Recv).GetRange(0, 3).ToArray();
            if (Recv.SequenceEqual(Encoding.ASCII.GetBytes(cmd + mode))) return ret > 0 ? 0 : ret;
            else return ExecFail;
        }

        public int SetBeeperBehavior(bool EnableDefaultUIBehavior)
        {
            return SetMode("1M", EnableDefaultUIBehavior ? 1 : 0);
        }

        public int SetLedBehavior(bool EnableDefaultUIBehavior)
        {
            int timeout;
            if (_CommMode == UsbMode.ACM)
            {
                timeout = InstanceofVC.ReadTimeout;
                InstanceofVC.ReadTimeout = 10000;
            }
            else {
                timeout = InstanceofHID.ReadTimeout;
                InstanceofHID.ReadTimeout = 10000;
            }
            int temp = SetMode("1Q", EnableDefaultUIBehavior ? 1 : 0);
            if (_CommMode == UsbMode.ACM)
                InstanceofVC.ReadTimeout = timeout;
            else
                InstanceofHID.ReadTimeout = timeout;
            return temp;
        }

        public int SetUsbMode(UsbMode mode)
        {
            //return SetMode("1S", (int)mode);
            int ret = WritePacket(true, PktType.SI, Encoding.ASCII.GetBytes("1S"), Encoding.ASCII.GetBytes("" + (int)mode));
            if (ret != 0) return ret;
            ret = ReadByte();
            if (ret == EOT) return PacketNotAccepted;//Packet was accepted by device, go on
            byte[] Recv = null;
            ret = ReadPacket(out Recv, PktType.SI);
            if (ret < 0) return ret;
            if (!CheckEOT(2000)) return ReadEOTTimeOut;
            Recv = new List<byte>(Recv).GetRange(0, 3).ToArray();
            if (Recv == null || Recv.Length != 3) return ExecFail;
            if (Recv[2] == '0') return ret > 0 ? 0 : ret;
            else return ExecFail;
            //if (Recv.SequenceEqual(Encoding.ASCII.GetBytes(cmd + mode))) return ret > 0 ? 0 : ret;
            //else return ExecFail;
        }

        public int GetTamperEvidence(out byte[] buffer)
        {
            return Getter(PktType.SI, "20", "", out buffer);
        }

        public int SetResetTime(byte[] cmdbuffer)
        {
            int ret = SetParameters(PktType.SI, "27", Encoding.ASCII.GetString(cmdbuffer));
            if (ret < 0) return ret;
            return ret == 0x30 ? 0 : ExecFail;
        }
        #endregion

        #region MSR Command

        public int InsertCard(bool EnableMsr, bool EnableIcc, int TimeOut)
        {
            if (TimeOut < 0 || TimeOut > 255) return ExecFail;
            string time = "." + (TimeOut / 100 > 0 ? "" : "0") + (TimeOut / 10 > 0 ? "" : "0") + TimeOut;
            int ret = WritePacket(true, PktType.STX, Encoding.ASCII.GetBytes("E1"), Encoding.ASCII.GetBytes("&" + Convert.ToInt32(EnableMsr) + Convert.ToInt32(EnableIcc)));
            if (ret != 0) return ret;
            ret = WritePacket(false, PktType.STX, Encoding.ASCII.GetBytes("E2"), Encoding.ASCII.GetBytes(time));
            //if (ret != 0) return ret;
            //else return 1;
            return ret;
        }

        public int SetBbParam(byte[] signatureBuf, byte[] bbParamBuf)
        {
            string parameters = "." + Encoding.ASCII.GetString(signatureBuf) + '\x1C' + Encoding.ASCII.GetString(bbParamBuf);
            int ret = SetParameters(PktType.STX, "E5", parameters);
            if (ret < 0) return ret;
            return ret == 0x31 ? 0 : ExecFail;
        }

        public int SetCerts(int CertType, byte[] x509CertBuf)
        {
            string parameters = "." + CertType + '\x1C' + Encoding.ASCII.GetString(x509CertBuf);
            int ret = SetParameters(PktType.STX, "E7", parameters);
            if (ret < 0) return ret;
            return ret == 0x31 ? 0 : ExecFail;
        }

        public int SetIdentityStr(byte[] identityBuf)
        {
            if (identityBuf.Length > 255) return ExecFail;
            string parameters = "." + Encoding.ASCII.GetString(identityBuf);
            int ret = SetParameters(PktType.STX, "E9", parameters);
            if (ret < 0) return ret;
            return ret == 0x31 ? 0 : ExecFail;
        }

        public int GetIdentityStr(out byte[] buffer)
        {
            return Getter(PktType.STX, "EA", "", out buffer);
        }

        public int GetBbParamHash(out byte[] buffer)
        {
            return Getter(PktType.STX, "EB", "", out buffer);
        }

        public int GetTepStatus(out byte[] buffer)
        {
            return Getter(PktType.STX, "EC", "", out buffer);
        }

        public int SetBinExceptionTbl(byte[] signatureBuf, byte[] tableBuf)
        {
            string parameters = "." + Encoding.ASCII.GetString(signatureBuf) + '\x1C' + Encoding.ASCII.GetString(tableBuf);
            int ret = SetParameters(PktType.STX, "ED", parameters);
            if (ret < 0) return ret;
            return ret == 0x31 ? 0 : ExecFail;
        }

        public int GetBinExceptionTblHash(out byte[] buffer)
        {
            return Getter(PktType.STX, "EF", "", out buffer);
        }

        public bool AbortTransaction()
        {
            if (WritePacket(true, PktType.STX, Encoding.ASCII.GetBytes("72"), Encoding.ASCII.GetBytes("")) != 0) return false;
            if (ReadByte() == EOT) return true;
            else return false;
        }
        #endregion

        #region EMV Level 2 transaction

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

        public int SetEmvTermConf(string path, out byte[] buffer)
        {
            buffer = null;
            string msg = "11";
            using (StreamReader sr = new StreamReader(path))
            {
                if (sr == null) throw new FileNotFoundException();
                string tmp;
                string[] temp;
                int val;
                while (!sr.EndOfStream)
                {
                    tmp = sr.ReadLine();
                    if (tmp.Trim() == "") continue;
                    val = Convert.ToChar(tmp.Substring(0, 1));
                    if (val < 0x30 || val > 0x39) goto Next;
                    temp = tmp.Split(new Char[] { ' ' });
                    if (temp[0].Length % 2 != 0 || temp[0].Length > 8)
                        return ExecFail;
                    tmp = Check_val(temp[1], out temp[1]);
                    if (tmp == "Format_Err") return ExecFail;
                    msg = msg + '\x1a' + string.Join("" + '\x1c', temp);
                }
            }
        Next:
            return Getter(PktType.STX, "T01", msg, out buffer);
        }

        public int SetEmvCapk(string path, out byte[] buffer)
        {
            buffer = null;
            string msg = "1";
            string nextstr = "";
            string[] temp;
            using (StreamReader sr = new StreamReader(path))
            {
                if (sr == null) throw new FileNotFoundException();
                string tmp;
                tmp = sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    nextstr = sr.ReadLine();
                    if (tmp.Trim() == "") continue;
                    temp = tmp.Split(new Char[] { ' ' });
                    if (temp.Length != 2) return ExecFail;
                    if (string.Compare(temp[0], "PKExp") == 0)
                    {
                        if (string.Compare(temp[1], "03") == 0) msg = msg + "1";
                        else if (string.Compare(temp[1], "010001") == 0) msg = msg + "2";
                        else return ExecFail;
                    }
                    else
                        msg = msg + temp[1];
                    tmp = nextstr;
                }
            }
            int ret = Getter(PktType.STX, "T03", msg, out buffer);
            if (ret < 0) return ret;
            if (buffer[4] != 0x30) return ExecFail;
            temp = nextstr.Split(new Char[] { ' ' });
            if (temp.Length != 2) return ExecFail;
            msg = "2" + temp[1];
            return Getter(PktType.STX, "T03", msg, out buffer);
        }

        public int SetEmvAppConf(string path, out byte[] buffer)
        {
            buffer = null;
            string msg = "11";
            using (StreamReader sr = new StreamReader(path))
            {
                if (sr == null) throw new FileNotFoundException();
                string tmp;
                string[] temp;
                int val;
                msg = msg + Convert.ToChar(0x1a).ToString() + sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    tmp = sr.ReadLine();
                    if (tmp.Trim() == "") continue;
                    val = Convert.ToChar(tmp.Substring(0, 1));
                    if (val < 0x30 || val > 0x39) goto Next;
                    temp = tmp.Split(new Char[] { ' ' });
                    if (temp[0].Length % 2 != 0 || temp[0].Length > 8)
                        return ExecFail;
                    tmp = Check_val(temp[1], out temp[1]);
                    if (tmp == "Format_Err") return ExecFail;
                    msg = msg + Convert.ToChar(0x1a).ToString() + string.Join(Convert.ToChar(0x1c).ToString(), temp);
                }
            }
        Next:
            return Getter(PktType.STX, "T05", msg, out buffer);
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

        public int SetEmvDataFmtTbl(string path, out byte[] buffer)
        {
            buffer = null;
            string msg = "1";
            using (StreamReader sr = new StreamReader(path))
            {
                if (sr == null) throw new FileNotFoundException();
                string tmp;
                string[] tempStringArray;
                while (!sr.EndOfStream)
                {
                    tmp = sr.ReadLine();
                    if (tmp.Trim() == "")
                        continue;
                    tempStringArray = tmp.Split(new Char[] { ' ' });
                    if (tempStringArray.Length != 5) return ExecFail;
                    if (check_format_len(tempStringArray) != 0) return ExecFail;
                    change_format(tempStringArray[1], out tempStringArray[1]);
                    tmp = string.Join(Convert.ToChar(0x1c).ToString(), tempStringArray);
                    msg = msg + Convert.ToChar(0x1a).ToString() + tempStringArray[0] + Convert.ToChar(0x1c).ToString() + tempStringArray[1] +
                        tempStringArray[2] + tempStringArray[3] + tempStringArray[4];
                }
            }
            return Getter(PktType.STX, "T07", msg, out buffer);
        }

        public int GetEmvConfList(int config, out byte[] buffer)
        {
            buffer = null;
            if (config != 1 && config != 2 && config != 3 && config != 4) return ExecFail;
            return Getter(PktType.STX, "T09", "" + config, out buffer);
        }

        public int DelEmvConfData(int config, byte[] datain, out byte[] buffer)
        {
            buffer = null;
            if (config != 1 && config != 2 && config != 3 && config != 4) return ExecFail;
            return Getter(PktType.STX, "T0B", "" + config + '\x1A' + Encoding.ASCII.GetString(datain), out buffer);
        }

        public int EmvSndRuntimeData(byte[] datain, out byte[] buffer)
        {
            buffer = null;
            return Getter(PktType.STX, "T1D", Encoding.ASCII.GetString(datain), out buffer);
        }

        public int EmvGetTagValues(byte[] datain, out byte[] buffer)
        {
            buffer = null;
            return Getter(PktType.STX, "T21", Encoding.ASCII.GetString(datain), out buffer);
        }

        public int EmvGetTransactionData(byte[] datain, out byte[] buffer)
        {
            buffer = null;
            return Getter(PktType.STX, "T2H", Encoding.ASCII.GetString(datain), out buffer);
        }

        public int EmvAbortTransaction()
        {
            int ret = WritePacket(true, PktType.STX, Encoding.ASCII.GetBytes("T1C"), Encoding.ASCII.GetBytes(""));
            int timeout;
            if (_CommMode == UsbMode.ACM)
            {
                timeout = InstanceofVC.ReadTimeout;
                InstanceofVC.ReadTimeout = 500;
            }
            else
            {
                timeout = InstanceofHID.ReadTimeout;
                InstanceofHID.ReadTimeout = 500;
            }
            ReadByte();
            if (_CommMode == UsbMode.ACM)
                InstanceofVC.ReadTimeout = timeout;
            else
                InstanceofHID.ReadTimeout = timeout;
            return ret;
        }

        public int EmvTransactionInit()
        {
            int ret = WritePacket(false, PktType.STX, Encoding.ASCII.GetBytes("T11"), Encoding.ASCII.GetBytes(""));
            //if (ret != 0) return ret;
            //return 1;
            return ret;
        }

        public int EmvSelectNextApp()
        {
            int ret = WritePacket(false, PktType.STX, Encoding.ASCII.GetBytes("T13"), Encoding.ASCII.GetBytes(""));
            //if (ret != 0) return ret;
            //return 1;
            return ret;
        }

        public int EmvTransStart(byte[] datain)
        {
            int ret = WritePacket(false, PktType.STX, Encoding.ASCII.GetBytes("T15"), datain);
            //if (ret != 0) return ret;
            //return 1;
            return ret;
        }

        public int EmvOnlineProc(byte[] datain)
        {
            int ret = WritePacket(false, PktType.STX, Encoding.ASCII.GetBytes("T17"), datain);
            //if (ret != 0) return ret;
            //return 1;
            return ret;
        }

        public int EmvSndIssuerScript(byte[] datain)
        {
            int ret = WritePacket(false, PktType.STX, Encoding.ASCII.GetBytes("T19"), datain);
            //if (ret != 0) return ret;
            //return 1;
            return ret;
        }
        #endregion

        public enum MSRDataType
        {
            Format1 = 0,
            Format2,
            E3
        }

        static public void ParseTrkData(MSRDataType type, byte[] data, out TrackData a)
        {
            a = new TrackData();
            a.TrkDataLayout = new byte[2];
            a.TrkDecodeResult = new byte[3];
            a.TrkLuhnResult = new byte[3];
            a.ObfuscatedTrks = new string[3];
            a.EncryptedTrks = new string[3];
            a.EncryptedPans = new string[3];
            string[] strarray;
            switch (type)
            {
                case MSRDataType.Format1:
                    if (data == null || data.Length < 1714)
                    {
                        throw new ArgumentException(String.Format("ParseTrkData: data length < 1715 or SN is null"), "SN");
                        //return ExecFail;
                    }
                    if (data.Length == 1714)
                    {
                        byte[] temp = new byte[1715];
                        Array.Copy(data, 0, temp, 1, 1714);
                        data = temp;
                    }
                    int idx = 1;
                    a.TrkDecodeResult[0] = data[idx++];//Convert.ToBoolean(data[idx++]);
                    a.TrkDecodeResult[1] = data[idx++];
                    a.TrkDecodeResult[2] = data[idx++];
                    idx++; idx++; idx++; //Encrypted Track n data length
                    byte CardEncodeType = data[idx++];
                    a.TrkLuhnResult[0] = data[idx++];//Convert.ToBoolean(data[idx++]);
                    a.TrkLuhnResult[1] = data[idx++];
                    a.TrkLuhnResult[2] = data[idx++];
                    a.TrkDataLayout[0] = data[idx++];
                    a.TrkDataLayout[1] = data[idx++];
                    while (data[idx] != '\x00' && idx < 173)
                        a.EncryptedTrks[0] += Convert.ToChar(data[idx++]);
                    idx = 173;
                    while (data[idx] != '\x00' && idx < 333)
                        a.EncryptedTrks[1] += Convert.ToChar(data[idx++]);
                    idx = 333;
                    while (data[idx] != '\x00' && idx < 493)
                        a.EncryptedTrks[2] += Convert.ToChar(data[idx++]);
                    idx = 493;
                    idx++; idx++; idx++; //Obfuscated Track n data length
                    while (data[idx] != '\x00' && idx < 656)
                        a.ObfuscatedTrks[0] += Convert.ToChar(data[idx++]);
                    idx = 656;
                    while (data[idx] != '\x00' && idx < 816)
                        a.ObfuscatedTrks[1] += Convert.ToChar(data[idx++]);
                    idx = 816;
                    while (data[idx] != '\x00' && idx < 976)
                        a.ObfuscatedTrks[2] += Convert.ToChar(data[idx++]);
                    idx = 976;
                    idx++; idx++; //Key transmission block length
                    while (data[idx] != '\x00' && idx < 1328)
                        a.EtbData += Convert.ToChar(data[idx++]);
                    idx = 1328;
                    idx++; idx++; idx++; //Encrypted Track n PAN data length
                    while (data[idx] != '\x00' && idx < 1459)
                        a.EncryptedPans[0] += Convert.ToChar(data[idx++]);
                    idx = 1459;
                    while (data[idx] != '\x00' && idx < 1587)
                        a.EncryptedPans[1] += Convert.ToChar(data[idx++]);
                    idx = 1587;
                    while (data[idx] != '\x00' && idx < 1715)
                        a.EncryptedPans[2] += Convert.ToChar(data[idx++]);
                    break;
                case MSRDataType.Format2:
                    strarray = Encoding.ASCII.GetString(data).Split('|');
                    if (strarray.Length == 11 && strarray[0][0] == '<' && strarray[10][0] == '>')
                    {
                        if (strarray[0].Length != 0)
                        {
                            a.TrkDataLayout[0] = Convert.ToByte(strarray[0][1]);
                            a.TrkDataLayout[1] = Convert.ToByte(strarray[0][2]);
                            a.TrkDecodeResult[0] = Convert.ToByte(strarray[0][7]);
                            a.TrkLuhnResult[0] = Convert.ToByte(strarray[0][8]);
                            a.ObfuscatedTrks[0] = strarray[0].Substring(9);
                        }
                        a.EncryptedTrks[0] = strarray[1];
                        a.EncryptedPans[0] = strarray[2];
                        if (strarray[3].Length != 0)
                        {
                            a.TrkDecodeResult[1] = Convert.ToByte(strarray[3][0]);
                            a.TrkLuhnResult[1] = Convert.ToByte(strarray[3][1]);
                            a.ObfuscatedTrks[1] = strarray[3].Substring(2);
                        }
                        a.EncryptedTrks[1] = strarray[4];
                        a.EncryptedPans[1] = strarray[5];
                        if (strarray[6].Length != 0)
                        {
                            a.TrkDecodeResult[2] = Convert.ToByte(strarray[6][0]);
                            a.TrkLuhnResult[2] = Convert.ToByte(strarray[6][1]);
                            a.ObfuscatedTrks[2] = strarray[6].Substring(2);
                        }
                        a.EncryptedTrks[2] = strarray[7];
                        a.EncryptedPans[2] = strarray[8];
                        a.EtbData = strarray[9];
                    }
                    break;
                case MSRDataType.E3:
                    strarray = Encoding.ASCII.GetString(data).Split('.');
                    if (strarray.Length < 2)
                    {
                        throw new ArgumentException(String.Format("ParseTrkData: not E3 Format"), "SN");
                    }
                    strarray = strarray[1].Split('\x1C');
                    for (int i = 0; i < 3; i++)
                    {
                        string[] tempstrarr = strarray[i].Split('|');
                        a.TrkDecodeResult[i] = Convert.ToByte(tempstrarr[0][0]);
                        a.TrkLuhnResult[i] = Convert.ToByte(tempstrarr[0][1]);
                        a.ObfuscatedTrks[i] = tempstrarr[0].Substring(2);
                        a.EncryptedTrks[i] = tempstrarr[1];
                        a.EncryptedPans[i] = tempstrarr[2];
                    }
                    a.EtbData = strarray[3];
                    break;
            }

        }
        //public delegate void HIDTrackDataDelegate(TrackData trackdata);
    }
}

