using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;

namespace PP791
{
    public sealed class CommPort
    {
        SerialPort _serialPort;
        Thread _readThread;
        bool _keepReading;

        static readonly CommPort instance = new CommPort();


        static CommPort()
        {
        }
        CommPort()
        {
            _serialPort = new SerialPort();
            _readThread = null;
            _keepReading = false;

        }
        public static CommPort Instance
        {
            get
            {
                return instance;
            }
        }

        public delegate void EventHandler(string param);
        public EventHandler StatusChanged;
        public EventHandler DataReceived;

        public string[] GetAvailablePorts()
        {
            string[] portNames = null;
            try
            {
                portNames = SerialPort.GetPortNames();
            }
            catch (SystemException e)
            {

            }
            return SerialPort.GetPortNames();
        }
    }
}
