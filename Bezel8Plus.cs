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
    enum Directions
    {
        Send,
        Receive
    }

    public partial class Bezel8Plus : Form
    {
        private delegate void SafeCallDelegate(object sender, byte[] text);
        private SerialPortManager serialPort = SerialPortManager.Instance;

        private MainConfigForm configForm;

        public Bezel8Plus()
        {
            InitializeComponent();
            cbBuadRate.SelectedIndex = 0;
            cbDataBits.SelectedIndex = 0;
            cbparity.SelectedIndex = 0;
            cbTxnType.SelectedIndex = 0;
            cbHandShake.SelectedIndex = 0;
            cbStopBits.SelectedIndex = 0;

            serialPort.OnDataReceived += DataReceivingLog;
            serialPort.OnDataSent += DataSendingLog;

            configForm = new MainConfigForm();


            AddFormToTab(configForm, tpConfig);

        }


        private void AddFormToTab(Form form, TabPage tabPage)
        {
            form.TopLevel = false;
            form.Visible = true;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;

            tabPage.Controls.Add(form);
        }

        private void PrintLog(PP791.Directions dir, byte[] message)
        {
            string time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");

            string direction = String.Empty;
            if (dir == PP791.Directions.Send)
            {
                direction = "\bSent:";
            }
            else
            {
                direction = "\bReceived:";
            }

            tbCommLog.AppendText(time + direction + Environment.NewLine);
            tbCommLog.AppendText(Moduel2.ConvertLoggingMessage(message) + Environment.NewLine + Environment.NewLine);
        }


        private void DataReceivingLog(object sender, byte[] text)
        {
            
            if (tbCommLog.InvokeRequired)
            {
                var d = new SafeCallDelegate(DataReceivingLog);
                Invoke(d, new object[] { sender, text });
            }
            else
            {
                /*
                string now = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
                tbCommLog.AppendText(now + " Received:" + Environment.NewLine);
                tbCommLog.AppendText(data + Environment.NewLine + Environment.NewLine);
                */
                PrintLog(PP791.Directions.Receive, text);
            }
            
        }

        private void DataSendingLog(object sender, byte[] text)
        {

            if (tbCommLog.InvokeRequired)
            {
                var d = new SafeCallDelegate(DataSendingLog);
                Invoke(d, new object[] { sender, text });
            }
            else
            {
                /*
                string now = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
                tbCommLog.AppendText(now + " Sent:" + Environment.NewLine);
                tbCommLog.AppendText(text + Environment.NewLine + Environment.NewLine);
                */
                PrintLog(PP791.Directions.Send, text);
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
            //AsynchronousSocketListener.StartListening();
            //string send = Convert.ToChar(0x0F).ToString() + "18201905083134750" + Convert.ToChar(0x0E).ToString();
            //serialPort.SendString_test(send + Moduel2.CCalculateLRC(send));
            //serialPort.SendPacketCommand(PP791.PktType.SI, "18201904065134750");

            try
            {
                serialPort.SendAndWait_test(PP791.PktType.SI, "18", "2019051311150", false);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void btnSend1_Click(object sender, EventArgs e)
        {

        }


    }
}
