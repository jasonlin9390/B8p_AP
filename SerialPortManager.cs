﻿using System;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace PP791
{

    public enum PktType
    {
        SI = 0,
        STX = 1
    }

    public sealed class SerialPortManager
    {
        private static readonly Lazy<SerialPortManager> lazy = new Lazy<SerialPortManager>(() => new SerialPortManager());
        public static SerialPortManager Instance { get { return lazy.Value; } }

        private SerialPort _serialPort;
        private Thread _readThread;
        private volatile bool _keepReading;
        private string _readMessage;

        private SerialPortManager()
        {
            _serialPort = new SerialPort();
            _readThread = null;
            _keepReading = false;
            _readMessage = "";
        }

        /// <summary>
        /// Update the serial port status to the event subscriber
        /// </summary>
        public event EventHandler<string> OnStatusChanged;

        /// <summary>
        /// Update received data from the serial port to the event subscriber
        /// </summary>
        public event EventHandler<string> OnDataSent;

        /// <summary>
        /// Update received data from the serial port to the event subscriber
        /// </summary>
        public event EventHandler<string> OnDataReceived;

        /// <summary>
        /// Update TRUE/FALSE for the serial port connection to the event subscriber
        /// </summary>
        public event EventHandler<bool> OnSerialPortOpened;

        /// <summary>
        /// Return TRUE if the serial port is currently connected
        /// </summary>
        public bool IsOpen { get { return _serialPort.IsOpen; } }

        /// <summary>
        /// Open the serial port connection using basic serial port settings
        /// </summary>
        /// <param name="portname">COM1 / COM3 / COM4 / etc.</param>
        /// <param name="baudrate">0 / 100 / 300 / 600 / 1200 / 2400 / 4800 / 9600 / 14400 / 19200 / 38400 / 56000 / 57600 / 115200 / 128000 / 256000</param>
        /// <param name="parity">None / Odd / Even / Mark / Space</param>
        /// <param name="databits">5 / 6 / 7 / 8</param>
        /// <param name="stopbits">None / One / Two / OnePointFive</param>
        /// <param name="handshake">None / XOnXOff / RequestToSend / RequestToSendXOnXOff</param>
        public void Open(
            string portname = "COM1",
            int baudrate = 9600,
            Parity parity = Parity.None,
            int databits = 8,
            StopBits stopbits = StopBits.One,
            Handshake handshake = Handshake.None)
        {
            Close();

            try
            {
                _serialPort.PortName = portname;
                _serialPort.BaudRate = baudrate;
                _serialPort.Parity = parity;
                _serialPort.DataBits = databits;
                _serialPort.StopBits = stopbits;
                _serialPort.Handshake = handshake;

                _serialPort.ReadTimeout = 1000;
                _serialPort.WriteTimeout = 1000;

                _serialPort.Open();
                StartReading();
            }
            catch (IOException)
            {
                if (OnStatusChanged != null)
                    OnStatusChanged(this, string.Format("{0} does not exist.", portname));
            }
            catch (UnauthorizedAccessException)
            {
                if (OnStatusChanged != null)
                    OnStatusChanged(this, string.Format("{0} already in use.", portname));
            }
            catch (Exception ex)
            {
                if (OnStatusChanged != null)
                    OnStatusChanged(this, "Error: " + ex.Message);
            }

            if (_serialPort.IsOpen)
            {
                string sb = StopBits.None.ToString().Substring(0, 1);
                switch (_serialPort.StopBits)
                {
                    case StopBits.One:
                        sb = "1"; break;
                    case StopBits.OnePointFive:
                        sb = "1.5"; break;
                    case StopBits.Two:
                        sb = "2"; break;
                    default:
                        break;
                }

                string p = _serialPort.Parity.ToString().Substring(0, 1);
                string hs = _serialPort.Handshake == Handshake.None ? "No Handshake" : _serialPort.Handshake.ToString();

                if (OnStatusChanged != null)
                    OnStatusChanged(this, string.Format(
                    "Connected to {0}: {1} bps, {2}{3}{4}, {5}.",
                    _serialPort.PortName,
                    _serialPort.BaudRate,
                    _serialPort.DataBits,
                    p,
                    sb,
                    hs));

                if (OnSerialPortOpened != null)
                    OnSerialPortOpened(this, true);
            }
            else
            {
                if (OnStatusChanged != null)
                    OnStatusChanged(this, string.Format(
                    "{0} already in use.",
                    portname));

                if (OnSerialPortOpened != null)
                    OnSerialPortOpened(this, false);
            }
        }

        /// <summary>
        /// Close the serial port connection
        /// </summary>
        public void Close()
        {
            StopReading();
            _serialPort.Close();

            if (OnStatusChanged != null)
                OnStatusChanged(this, "Connection closed.");

            if (OnSerialPortOpened != null)
                OnSerialPortOpened(this, false);
        }

        public void SendPacketCommand(PP791.PktType type, string message)
        {
            if (_serialPort.IsOpen)
            {
                string pktCommand = "";
                if (type == PP791.PktType.SI)
                {
                    pktCommand = Convert.ToChar(0x0F).ToString() +
                        message + Convert.ToChar(0x0E).ToString();
                }
                else if (type == PP791.PktType.STX)
                {
                    pktCommand = Convert.ToChar(0x02).ToString() +
                        message + Convert.ToChar(0x03).ToString();
                }
                else
                {
                    // Unknown type
                }

                try
                {          
                    _serialPort.Write(pktCommand + Moduel2.CCalculateLRC(pktCommand));

                    if (OnDataSent != null)
                    {
                        OnDataSent(this, pktCommand + Moduel2.CCalculateLRC(pktCommand));
                    }

                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// Send/write string to the serial port
        /// </summary>
        /// <param name="message"></param>
        public void SendString(string message)
        {
            if (_serialPort.IsOpen)
            {
                try
                {
                    _serialPort.Write(message);

                    if (OnStatusChanged != null)
                        OnStatusChanged(this, string.Format(
                        "Message sent: {0}",
                        message));
                }
                catch (Exception ex)
                {
                    if (OnStatusChanged != null)
                        OnStatusChanged(this, string.Format(
                            "Failed to send string: {0}",
                            ex.Message));
                }
            }
        }

        private void StartReading()
        {
            if (!_keepReading)
            {
                _keepReading = true;
                _readThread = new Thread(ReadPort);
                _readThread.Start();
            }
        }

        private void StopReading()
        {
            if (_keepReading)
            {
                _keepReading = false;
                _readThread.Join();
                _readThread = null;
            }
        }

        private void ReadPort()
        {
            while (_keepReading)
            {
                if (_serialPort.IsOpen)
                {
                    byte[] readBuffer = new byte[_serialPort.ReadBufferSize + 1];
                    try
                    {
                        
                        _serialPort.Read(readBuffer, 0, _serialPort.ReadBufferSize);
                        string temp = Encoding.ASCII.GetString(readBuffer);

                        if (temp.Length == 1 && temp == Convert.ToChar(0x06).ToString())
                        {
                            Console.WriteLine("ACK");
                        }
                        else
                        {
                            _readMessage += temp;
                        }
                        Console.WriteLine(_readMessage);

                        if (_readMessage.Contains(Convert.ToChar(0x0F).ToString()) &&
                            _readMessage.Contains(Convert.ToChar(0x0E).ToString()))
                        {
                            Console.WriteLine(_readMessage);
                        }
                        else if (_readMessage.Contains(Convert.ToChar(0x02).ToString()) &&
                            _readMessage.Contains(Convert.ToChar(0x03).ToString()))
                        {
                            Console.WriteLine(_readMessage);
                        }

                        /*
                        string data = Encoding.ASCII.GetString(readBuffer);
                        string receivedData = null;

                        if (count > 0)
                        {
                            for (int i = 0; i < count; i++)
                            {
                                if (readBuffer[i] < 0x20)
                                {
                                    receivedData += "<" + readBuffer[i].ToString("X2") + ">";
                                }
                                else
                                {
                                    receivedData +=  Convert.ToChar(readBuffer[i]).ToString();
                                }
                            }
                            Console.WriteLine(receivedData);

                            SendString(Convert.ToChar(0x06).ToString());
                        }
                        

                        if (OnDataReceived != null)
                            OnDataReceived(this, receivedData);

                        */
                    }
                    catch (TimeoutException) { }
                }
                else
                {
                    TimeSpan waitTime = new TimeSpan(0, 0, 0, 0, 50);
                    Thread.Sleep(waitTime);
                }
            }
        }
    }
}