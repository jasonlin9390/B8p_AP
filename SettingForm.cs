using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PP791
{
    public partial class SettingForm : Form
    {

        private SerialPortManager serialPort = SerialPortManager.Instance;

        public SettingForm()
        {
            InitializeComponent();
        }


        private void btnGetTime_Click(object sender, EventArgs e)
        {
            tbYear.Text = DateTime.Now.Year.ToString("0000");
            tbMonth.Text = DateTime.Now.Month.ToString("00");
            tbDay.Text = DateTime.Now.Day.ToString("00");
            tbHour.Text = DateTime.Now.Hour.ToString("00");
            tbMinute.Text = DateTime.Now.Minute.ToString("00");
            tbSecond.Text = DateTime.Now.Second.ToString("00");
        }

        private void btnSetTime_Click(object sender, EventArgs e)
        {
            DateTime date;
            string formattedDate = tbYear.Text + "/" + tbMonth.Text + "/" + tbDay.Text + " " +
                tbHour.Text + ":" + tbMinute.Text + ":" + tbSecond.Text;

            if (DateTime.TryParse(formattedDate, out date))
            {
                string datetime = date.ToString("yyyyMMdd") + ((int)date.DayOfWeek).ToString() + date.ToString("HHmmss");
                Console.WriteLine("Valid Date: " + datetime);

                //serialPort.SendPacketCommand(PktType.SI, datetime);
            }
            else
            {
                Console.WriteLine("Invalid Date: "+ formattedDate);
            }
        }
    }
}
