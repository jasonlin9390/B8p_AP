using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PP791
{
    public partial class BatchShowForm : Form
    {
        public BatchShowForm()
        {
            InitializeComponent();
            textBox1.Text = null;
        }
        public void showBatch(string input)
        {
            textBox1.Text = textBox1.Text + input;
        }
        string searchFor;
        string replaceWith;
        string ReplaceMatchCase(Match m)
        {
            // Test whether the match is capitalized
            if (Char.IsUpper(m.Value[0]) == true)
            {
                // Capitalize the replacement string
                // using System.Text;
                StringBuilder sb = new StringBuilder(replaceWith);
                sb[0] = (Char.ToUpper(sb[0]));
                return sb.ToString();
            }
            else
            {
                return replaceWith;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string s;
            s = textBox1.Text;
            searchFor = "0x";
            replaceWith = "Tag: ";
            s = Regex.Replace(s, searchFor, ReplaceMatchCase, RegexOptions.IgnoreCase);
            searchFor = Module1.charStr(0x1C);
            replaceWith = " ";
            s = Regex.Replace(s, searchFor, ReplaceMatchCase, RegexOptions.IgnoreCase);
            searchFor = Module1.charStr(0x1A);
            replaceWith = "\r\n";
            s = Regex.Replace(s, searchFor, ReplaceMatchCase, RegexOptions.IgnoreCase);
            textBox1.Text = s;
            searchFor = Module1.charStr(0x0A);

        }
        public void ClearBatch()
        {
            textBox1.Text = null;
        }
    }
}
