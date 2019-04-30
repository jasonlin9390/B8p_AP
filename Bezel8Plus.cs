using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace PP791
{
    public partial class Bezel8Plus : Form
    {
        private delegate void SafeCallDelegate(object sender, string text);
        private SerialPortManager serialPort = SerialPortManager.Instance;

        public Bezel8Plus()
        {
            InitializeComponent();
            cbBuadRate.SelectedIndex = 0;
            cbDataBits.SelectedIndex = 0;
            cbparity.SelectedIndex = 0;
            cbTxnType.SelectedIndex = 0;

            serialPort.OnDataReceived += DataReceivingLog;
            serialPort.OnDataSent += DataSendingLog;
        }

        private void DataReceivingLog(object sender, string data)
        {

            if (tbCommLog.InvokeRequired)
            {
                var d = new SafeCallDelegate(DataReceivingLog);
                Invoke(d, new object[] { sender, data });
            }
            else
            {
                string now = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
                tbCommLog.AppendText(now + " Received:" + Environment.NewLine);
                tbCommLog.AppendText(data + Environment.NewLine + Environment.NewLine);
            }
        }

        private void DataSendingLog(object sender, string data)
        {

            if (tbCommLog.InvokeRequired)
            {
                var d = new SafeCallDelegate(DataSendingLog);
                Invoke(d, new object[] { sender, data });
            }
            else
            {
                string now = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
                tbCommLog.AppendText(now + " Sent:" + Environment.NewLine);
                tbCommLog.AppendText(data + Environment.NewLine + Environment.NewLine);
            }
        }


        private void btnOpenCom_Click(object sender, EventArgs e)
        {

            if (cbCOM.SelectedIndex == -1)
            {
                return;
            }
            serialPort.Open(
            cbCOM.SelectedItem.ToString(),
            9600,
            Parity.None,
            8,
            StopBits.One,
            Handshake.None);

            if (serialPort.IsOpen)
            {
                btnOpenCom.Enabled = false;
                btnCloseCom.Enabled = true;
                gbComSetting.Enabled = false;
            }
            
        }

        private void cbCOM_Click(object sender, EventArgs e)
        {
            cbCOM.Items.Clear();
            foreach (string port in SerialPort.GetPortNames())
            {
                cbCOM.Items.Add(port);
            }
        }

        private void btnCloseCom_Click(object sender, EventArgs e)
        {
            serialPort.Close();

            if (!serialPort.IsOpen)
            {
                btnOpenCom.Enabled = true;
                btnCloseCom.Enabled = false;
                gbComSetting.Enabled = true;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            //serialPort.SendString(Module1.charStr(0x0F) + tbMessage.Text + Module1.charStr(0x0E));
            serialPort.SendString("18201904065134750");
        }

        private void btnSend1_Click(object sender, EventArgs e)
        {
            //serialPort.SendString(Module1.charStr(0x02) + "T6C" + Module1.charStr(0x03) + "\"");
            /*
            byte[] bb = new byte[] { 0x02, 0x54, 0x36, 0x43, 0x03 };
            byte test = Moduel2.LRCCalculator(bb, 5);
            Console.WriteLine(test);
            */

            //char cc = Moduel2.CCalculateLRC(Module1.charStr(0x02) + "T6C" + Module1.charStr(0x03));
            //Console.WriteLine(cc);

            //serialPort.SendPacketCommand(PP791.PktType.STX, tbMessage1.Text);

            serialPort.SendPacketCommand(PP791.PktType.SI, "18201904065134750");


        }
    }
}
