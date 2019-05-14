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
    public partial class MainConfigForm : Form
    {

        private TerAppConfigForm terAndAppForm;

        public MainConfigForm()
        {
            InitializeComponent();

            terAndAppForm = new TerAppConfigForm();
            AddFormToTab(terAndAppForm, tpConfig);

        }

        private void AddFormToTab(Form form, TabPage tabPage)
        {
            form.TopLevel = false;
            form.Visible = true;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            tabPage.Controls.Add(form);
        }

    }
}
