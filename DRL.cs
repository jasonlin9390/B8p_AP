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
    public partial class DRL : Form
    {
        public DRL()
        {
            InitializeComponent();
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
    }
}
