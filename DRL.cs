using System;
using System.IO;
using System.Security;
using System.Diagnostics;
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

    public partial class DRL : Form
    {

        private OpenFileDialog openFileDialog1;

        private CheckBox[] cbDRLs;
        private TextBox[] tbProgramIDs;
        private CheckBox[] cbStatusChecks;
        private CheckBox[] cbZeroChecks;
        private ComboBox[] cbbZeroChecks;
        private CheckBox[] cbRCTLs;
        private TextBox[] tbRCTLs;
        private CheckBox[] cbCVMs;
        private TextBox[] tbCVMs;
        private CheckBox[] cbRCFLs;
        private TextBox[] tbRCFLs;

        public DRL()
        {
            InitializeComponent();
            SetComponentArray();
            InitializeUI();
        }

        private void SetComponentArray()
        {
            cbDRLs = new CheckBox[] { cbDRL1, cbDRL2, cbDRL3, cbDRL4 };
            tbProgramIDs = new TextBox[] { tbProgramID1, tbProgramID2, tbProgramID3, tbProgramID4 };
            cbStatusChecks = new CheckBox[] { cbStatusCheck1 , cbStatusCheck2 , cbStatusCheck3 , cbStatusCheck4 };
            cbZeroChecks = new CheckBox[] { cbZeroCheck1 , cbZeroCheck2 , cbZeroCheck3 , cbZeroCheck4 };
            cbbZeroChecks = new ComboBox[] { cbbOption1 , cbbOption2 , cbbOption3 , cbbOption4 };
            cbRCTLs = new CheckBox[] { cbRCTL1, cbRCTL2, cbRCTL3, cbRCTL4 };
            tbRCTLs = new TextBox[] { tbRCTL1 , tbRCTL2 , tbRCTL3 , tbRCTL4 };
            cbCVMs = new CheckBox[] { cbCVMCheck1 , cbCVMCheck2 , cbCVMCheck3 , cbCVMCheck4 };
            tbCVMs = new TextBox[] { tbCVML1 , tbCVML2 , tbCVML3 , tbCVML4 };
            cbRCFLs = new CheckBox[] { cbRCFLCheck1 , cbRCFLCheck2 , cbRCFLCheck3 , cbRCFLCheck4 };
            tbRCFLs = new TextBox[] { tbRCFL1 , tbRCFL2 , tbRCFL3 , tbRCFL4 };
        }

        private void InitializeUI()
        {
            for (int i = 0; i < 4; i++)
            {
                tbProgramIDs[i].Text = "";
                cbStatusChecks[i].Checked = false;
                cbZeroChecks[i].Checked = false;
                cbbZeroChecks[i].Enabled = false;
                cbRCTLs[i].Checked = false;
                tbRCTLs[i].Text = "";
                cbCVMs[i].Checked = false;
                tbCVMs[i].Text = "";
                cbRCFLs[i].Checked = false;
                tbRCFLs[i].Text = "";
                cbDRLs[i].Checked = false;
            }
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            int rtn = -1;
            byte[] receive_buf;
            string sending_msg = "";
            List<string> strlist = new List<string>();

            btnDeleteAll_Click(sender, e);

            for (int i = 0; i < 4; i++)
            {
                if (cbDRLs[i].Checked)
                {
                    if (tbProgramIDs[i].Text == "")
                    {
                        MessageBox.Show("Program ID can not be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }

                    // Reset string fro each DRL
                    strlist.Clear();
                    sending_msg = "";

                    // Program ID
                    sending_msg += Module1.charStr(0x1a) + tbProgramIDs[i].Text;


                    // Status Check: FFFF8007<1C>2<1C>XX
                    strlist.Add("FFFF8007");
                    strlist.Add("2");
                    if (cbStatusChecks[i].Checked)
                    {
                        strlist.Add("01");
                    }
                    else
                    {
                        strlist.Add("00");
                    }
                    sending_msg += Module1.charStr(0x1a) + string.Join(Module1.charStr(0x1c), strlist);
                    strlist.Clear();

                    // Zero Amount Check: FFFF8005<1C>2<1C>XX
                    // Zero Amount Option: FFFF8008<1C>2<1C>XX
                    strlist.Add("FFFF8005");
                    strlist.Add("2");
                    if (cbZeroChecks[i].Checked)
                    {
                        strlist.Add("01");
                        sending_msg += Module1.charStr(0x1a) + string.Join(Module1.charStr(0x1c), strlist);
                        strlist.Clear();

                        // Zero Amount Option
                        strlist.Add("FFFF8008");
                        strlist.Add("2");
                        if (cbbZeroChecks[i].SelectedIndex == 0)
                        {
                            // Option 1
                            strlist.Add("01");

                        }
                        else
                        {
                            strlist.Add("02");
                        }
                        
                    }
                    else
                    {
                        strlist.Add("00");
                        sending_msg += Module1.charStr(0x1a) + string.Join(Module1.charStr(0x1c), strlist);
                        strlist.Clear();

                        strlist.Add("FFFF8008");
                        strlist.Add("2");
                        strlist.Add("00");

                    }
                    sending_msg += Module1.charStr(0x1a) + string.Join(Module1.charStr(0x1c), strlist);
                    strlist.Clear();

                    // Reader CL Transaction Limit Check: FFFF8004<1C>2<1C>XX                    
                    strlist.Add("FFFF8004");
                    strlist.Add("2");
                    if (cbRCTLs[i].Checked)
                    {
                        strlist.Add("01");
                    }
                    else
                    {
                        strlist.Add("00");
                    }
                    sending_msg += Module1.charStr(0x1a) + string.Join(Module1.charStr(0x1c), strlist);
                    strlist.Clear();

                    // Reader CL Transaction Limit: DF8124<1C>2<1C>XXXXXXXXXXXX
                    if (cbRCTLs[i].Checked)
                    {
                        strlist.Add("DF8124");
                        strlist.Add("2");
                        if (tbRCTLs[i].Text.Length != 12)
                        {
                            MessageBox.Show("Error in " + cbDRLs[i].Text + ":" +
                                "\n\nThe length of Reader CL Transaction Limit must be 12",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                        strlist.Add(tbRCTLs[i].Text);
                        sending_msg += Module1.charStr(0x1a) + string.Join(Module1.charStr(0x1c), strlist);
                        strlist.Clear();
                    }
                        
                    // CVM Limit Check: FFFF8009<1C>2<1C>XX
                    strlist.Add("FFFF8009");
                    strlist.Add("2");
                    if (cbCVMs[i].Checked)
                    {
                        strlist.Add("01");
                    }
                    else
                    {
                        strlist.Add("00");
                    }
                    sending_msg += Module1.charStr(0x1a) + string.Join(Module1.charStr(0x1c), strlist);
                    strlist.Clear();

                    // CVM Limit: DF8126<1C>2<1C>XXXXXXXXXXXX
                    if (cbCVMs[i].Checked)
                    {
                        strlist.Add("DF8126");
                        strlist.Add("2");
                        if (tbCVMs[i].Text.Length != 12)
                        {
                            MessageBox.Show("Error in " + cbDRLs[i].Text + ":" +
                                "\n\nThe length of CVM Limit must be 12",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                        strlist.Add(tbCVMs[i].Text);
                        sending_msg += Module1.charStr(0x1a) + string.Join(Module1.charStr(0x1c), strlist);
                        strlist.Clear();
                    }                   

                    // Reader CL Floor Limit Check: FFFF800A<1C>2<1C>XX
                    strlist.Add("FFFF800A");
                    strlist.Add("2");
                    if (cbRCFLs[i].Checked)
                    {
                        if (tbRCFLs[i].Text == "")
                        {
                            // Reader CL Floor Limit not present
                            strlist.Add("10");

                        }
                        else
                        {
                            strlist.Add("01");
                        }
                        
                    }
                    else
                    {
                        strlist.Add("00");
                    }
                    sending_msg += Module1.charStr(0x1a) + string.Join(Module1.charStr(0x1c), strlist);
                    strlist.Clear();

                    // Reader CL Floor Limit: DF8123<1C>2<1C>XXXXXXXXXXXX
                    if (cbRCFLs[i].Checked && tbRCFLs[i].Text != "")
                    {
                        strlist.Add("DF8123");
                        strlist.Add("2");
                        if (tbRCFLs[i].Text!= "" && tbRCFLs[i].Text.Length != 12)
                        {
                            MessageBox.Show("Error in " + cbDRLs[i].Text + ":" +
                                "\n\nThe length of Reader CL Floor Limit must be 12",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                        strlist.Add(tbRCFLs[i].Text);
                        sending_msg += Module1.charStr(0x1a) + string.Join(Module1.charStr(0x1c), strlist);
                        strlist.Clear();
                    }

                    // Sending T5F message to device
                    //Console.WriteLine(sending_msg);
                    Module1.SIOOutput("T5F" + sending_msg);
                    rtn = PPDMain.DLL.Getter(UIC.PktType.STX, "T5F", sending_msg, out receive_buf);
                    Module1.SIOInput(rtn, 1, receive_buf);
                    if (rtn != 0)
                    {
                        MessageBox.Show("Setting " + cbDRLs[i].Text + " failed." ,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                    if (!Module1.ByteArrayToString(receive_buf).Equals("T5G0"))
                    {
                        MessageBox.Show("Setting " + cbDRLs[i].Text + " failed:\n\n" + Module1.ByteArrayToString(receive_buf)
                            , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                }
            }

            //Debug.WriteLine(sending_msg);

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog();
            string sl = "";
            int drl_index = -1;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                InitializeUI();
                try
                {
                    var sr = new StreamReader(openFileDialog1.FileName);
                    while (!sr.EndOfStream)
                    {
                        sl = sr.ReadLine();
                        //Debug.WriteLine(sl);
                        switch (sl)
                        {
                            case "#1":
                                drl_index = 0;
                                cbDRL1.Checked = true;
                                break;
                            case "#2":
                                drl_index = 1;
                                cbDRL2.Checked = true;
                                break;
                            case "#3":
                                drl_index = 2;
                                cbDRL3.Checked = true;
                                break;
                            case "#4":
                                drl_index = 3;
                                cbDRL4.Checked = true;
                                break;
                            default:
                                break;
                        }

                        if (drl_index >= 0 && drl_index <= 4)
                        {
                            if (sl.StartsWith("ProgramID"))
                            {
                                tbProgramIDs[drl_index].Text = sl.Substring(sl.IndexOf("=") + 1).Trim();
                            }
                            if (sl.StartsWith("StatusCheck"))
                            {
                                if (Int32.Parse(sl.Substring(sl.IndexOf("=") + 1)) == 1)
                                {
                                    cbStatusChecks[drl_index].Checked = true;
                                }         
                            }
                            if (sl.StartsWith("AmountZeroCheck"))
                            {
                                if (Int32.Parse(sl.Substring(sl.IndexOf("=") + 1)) == 1)
                                {
                                    cbZeroChecks[drl_index].Checked = true;
                                }
                            }
                            if (sl.StartsWith("AmountOption"))
                            {
                                int option = Int32.Parse(sl.Substring(sl.IndexOf("=") + 1));
                                if (option == 1)
                                {
                                    cbbZeroChecks[drl_index].SelectedIndex = 0;
                                }
                                else if (option == 2)
                                {
                                    cbbZeroChecks[drl_index].SelectedIndex = 1;
                                }
                                else
                                {
                                    cbbZeroChecks[drl_index].SelectedIndex = -1;
                                }
                            }
                            if (sl.StartsWith("RCTLChcek"))
                            {
                                if (Int32.Parse(sl.Substring(sl.IndexOf("=") + 1)) == 1)
                                {
                                    cbRCTLs[drl_index].Checked = true;
                                }
                            }
                            if (sl.StartsWith("RCTL=") || sl.StartsWith("RCTL ="))
                            {
                                string limitValue = sl.Substring(sl.IndexOf("=") + 1).Trim();
                                if (limitValue.Equals("FFFFFFFFFFFF"))
                                {
                                    tbRCTLs[drl_index].Text = "";
                                } else
                                {
                                    tbRCTLs[drl_index].Text = limitValue;
                                }
                            }
                            if (sl.StartsWith("CVMLCheck"))
                            {
                                if (Int32.Parse(sl.Substring(sl.IndexOf("=") + 1)) == 1)
                                {
                                    cbCVMs[drl_index].Checked = true;
                                }
                            }
                            if (sl.StartsWith("CVML=") || sl.StartsWith("CVML ="))
                            {
                                string limitValue = sl.Substring(sl.IndexOf("=") + 1).Trim();
                                if (limitValue.Equals("FFFFFFFFFFFF"))
                                {
                                    tbCVMs[drl_index].Text = "";
                                }
                                else
                                {
                                    tbCVMs[drl_index].Text = limitValue;
                                }
                            }
                            if (sl.StartsWith("RCFLCheck"))
                            {
                                if (Int32.Parse(sl.Substring(sl.IndexOf("=") + 1)) == 1)
                                {
                                    cbRCFLs[drl_index].Checked = true;
                                }
                            }
                            if (sl.StartsWith("RCFL=") || sl.StartsWith("RCFL ="))
                            {
                                string limitValue = sl.Substring(sl.IndexOf("=") + 1).Trim();
                                if (limitValue.Equals("FFFFFFFFFFFF"))
                                {
                                    tbRCFLs[drl_index].Text = "";
                                }
                                else
                                {
                                    tbRCFLs[drl_index].Text = limitValue;
                                }
                            }
                        }
                    }
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }


        private void cbDRL1_CheckStateChanged(object sender, EventArgs e)
        {
            if (cbDRL1.Checked)
            {
                gbDRL1.Enabled = true;
            }
            else
            {
                gbDRL1.Enabled = false;
            }

        }

        private void cbDRL2_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDRL2.Checked)
            {
                gbDRL2.Enabled = true;
            }
            else
            {
                gbDRL2.Enabled = false;
            }
        }

        private void cbDRL3_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDRL3.Checked)
            {
                gbDRL3.Enabled = true;
            }
            else
            {
                gbDRL3.Enabled = false;
            }
        }

        private void cbDRL4_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDRL4.Checked)
            {
                gbDRL4.Enabled = true;
            }
            else
            {
                gbDRL4.Enabled = false;
            }
        }

        private void cbZeroCheck1_CheckedChanged(object sender, EventArgs e)
        {
            if (cbZeroCheck1.Checked)
            {
                cbbOption1.Enabled = true;
                cbbOption1.SelectedIndex = 0;         
            }
            else
            {
                cbbOption1.SelectedIndex = -1;
                cbbOption1.Enabled = false;
            }
        }

        private void cbZeroCheck2_CheckedChanged(object sender, EventArgs e)
        {
            if (cbZeroCheck2.Checked)
            {
                cbbOption2.Enabled = true;
                cbbOption2.SelectedIndex = 0;
            }
            else
            {
                cbbOption2.SelectedIndex = -1;
                cbbOption2.Enabled = false;
            }
        }

        private void cbZeroCheck3_CheckedChanged(object sender, EventArgs e)
        {
            if (cbZeroCheck3.Checked)
            {
                cbbOption3.Enabled = true;
                cbbOption3.SelectedIndex = 0;
            }
            else
            {
                cbbOption3.SelectedIndex = -1;
                cbbOption3.Enabled = false;
            }
        }

        private void cbZeroCheck4_CheckedChanged(object sender, EventArgs e)
        {
            if (cbZeroCheck4.Checked)
            {
                cbbOption4.Enabled = true;
                cbbOption4.SelectedIndex = 0;
            }
            else
            {
                cbbOption4.SelectedIndex = -1;
                cbbOption4.Enabled = false;
            }
        }
        
        private void cbRCTL1_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRCTL1.Checked)
            {
                tbRCTL1.Enabled = true;
            }
            else
            {
                tbRCTL1.Enabled = false;
            }
        }

        private void cbRCTL2_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRCTL2.Checked)
            {
                tbRCTL2.Enabled = true;
            }
            else
            {
                tbRCTL2.Enabled = false;
            }
        }

        private void cbRCTL3_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRCTL3.Checked)
            {
                tbRCTL3.Enabled = true;
            }
            else
            {
                tbRCTL3.Enabled = false;
            }
        }

        private void cbRCTL4_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRCTL4.Checked)
            {
                tbRCTL4.Enabled = true;
            }
            else
            {
                tbRCTL4.Enabled = false;
            }
        }

        private void cbCVMCheck1_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCVMCheck1.Checked)
            {
                tbCVML1.Enabled = true;
            }
            else
            {
                tbCVML1.Enabled = false;
            }
        }

        private void cbCVMCheck2_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCVMCheck2.Checked)
            {
                tbCVML2.Enabled = true;
            }
            else
            {
                tbCVML2.Enabled = false;
            }
        }

        private void cbCVMCheck3_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCVMCheck3.Checked)
            {
                tbCVML3.Enabled = true;
            }
            else
            {
                tbCVML3.Enabled = false;
            }
        }

        private void cbCVMCheck4_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCVMCheck4.Checked)
            {
                tbCVML4.Enabled = true;
            }
            else
            {
                tbCVML4.Enabled = false;
            }
        }

        private void cbRCFLCheck1_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRCFLCheck1.Checked)
            {
                tbRCFL1.Enabled = true;
            }
            else
            {
                tbRCFL1.Enabled = false;
            }
        }

        private void cbRCFLCheck2_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRCFLCheck2.Checked)
            {
                tbRCFL2.Enabled = true;
            }
            else
            {
                tbRCFL2.Enabled = false;
            }
        }

        private void cbRCFLCheck3_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRCFLCheck3.Checked)
            {
                tbRCFL3.Enabled = true;
            }
            else
            {
                tbRCFL3.Enabled = false;
            }
        }

        private void cbRCFLCheck4_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRCFLCheck4.Checked)
            {
                tbRCFL4.Enabled = true;
            }
            else
            {
                tbRCFL4.Enabled = false;
            }
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            int rtn;
            byte[] receive_buf;

            Module1.SIOOutput("T5H");
            rtn = PPDMain.DLL.Getter(UIC.PktType.STX, "T5H", "", out receive_buf);
            Module1.SIOInput(rtn, 1, receive_buf);
            if (rtn != 0)
            {
                MessageBox.Show("Communicating Error.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!Module1.ByteArrayToString(receive_buf).Equals("T5I0"))
            {
                MessageBox.Show("Deleting DRL failed.\n\nError Message: " + Module1.ByteArrayToString(receive_buf), 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (sender.Equals(btnDeleteAll))
                {
                    MessageBox.Show("Deleting DRL Success.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            return;
        }
    }
}
