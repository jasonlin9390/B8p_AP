using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security;
using System.Diagnostics;
using System.IO;

namespace PP791
{
    public partial class TerAppConfigForm : Form
    {

        private OpenFileDialog openFileDialog;

        private DataSet confDataSet;

        public TerAppConfigForm()
        {
            InitializeComponent();

            confDataSet = new DataSet();
        }

        private void DemonstrateDataView()
        {
            DataTable table = new DataTable("table");
            DataColumn colDescrip = new DataColumn("Description", Type.GetType("System.String"));
            DataColumn colTag = new DataColumn("Tag", Type.GetType("System.String"));
            DataColumn colFormat = new DataColumn("Format", Type.GetType("System.String"));
            DataColumn colValue = new DataColumn("Value", Type.GetType("System.String"));

            table.Columns.Add(colDescrip);
            table.Columns.Add(colTag);
            table.Columns.Add(colFormat);
            table.Columns.Add(colValue);

        }

        private DataTable InitializeDataTable(string tableName)
        {

            DataTable table = new DataTable(tableName);

            DataColumn colDescrip = new DataColumn("Description", Type.GetType("System.String"));



            DataColumn colTag = new DataColumn("Tag", Type.GetType("System.String"));
            DataColumn colFormat = new DataColumn("Format", Type.GetType("System.String"));
            DataColumn colValue = new DataColumn("Value", Type.GetType("System.String"));

            table.Columns.Add(colDescrip);
            table.Columns.Add(colTag);
            table.Columns.Add(colFormat);
            table.Columns.Add(colValue);

            return table;
        }

        private int LoadConfigFileToDataSetView(string filename, string path)
        {
            try
            {
                var sr = new StreamReader(path + filename);
                DataTable table = InitializeDataTable(Path.GetFileNameWithoutExtension(filename));

                while (!sr.EndOfStream)
                {
                    string sl = sr.ReadLine();

                    if (!string.IsNullOrEmpty(sl))
                    {
                        // Description string handling
                        string description = String.Empty;
                        if (sl.Contains("//"))
                        {
                            int descripePosition = sl.IndexOf("//");
                            description = sl.Substring(descripePosition + 2).Trim();
                            //Console.WriteLine(sl);
                            //Console.WriteLine(description);

                            sl = sl.Substring(0, descripePosition);
                            //Console.WriteLine(sl);
                        }

                        // Tag Format Value string handling
                        string[] tagFormatValue = sl.Trim().Split(' ');
                        //Console.WriteLine("tagFormatValue size = {0}", tagFormatValue.Length);
                        if (tagFormatValue.Length == 3)
                        {
                            DataRow dataRow = table.NewRow();
                            dataRow["Tag"] = tagFormatValue[0];
                            dataRow["Format"] = tagFormatValue[1];
                            dataRow["Value"] = tagFormatValue[2];

                            if (!string.IsNullOrEmpty(description))
                            {
                                dataRow["Description"] = description;
                            }
                            table.Rows.Add(dataRow);
                        }
                    }
                }
                confDataSet.Tables.Add(table);
            }
            catch (FileNotFoundException)
            {
                throw new System.IO.FileNotFoundException("File: {0} Not Found.", filename);
            }
            

            return 0;
        }

        private void BuildConfigDataSet(string fileName)
        {

            try
            {
                var sr = new StreamReader(fileName);
                DataTable table = InitializeDataTable(Path.GetFileNameWithoutExtension(fileName));

                while (!sr.EndOfStream)
                {
                    string sl = sr.ReadLine();

                    if (!string.IsNullOrEmpty(sl))
                    {
                        // Description string handling
                        string description = String.Empty;
                        if (sl.Contains("//"))
                        {
                            int descripePosition = sl.IndexOf("//");
                            description = sl.Substring(descripePosition + 2).Trim();
                            sl = sl.Substring(0, descripePosition);
                        }

                        // Tag Format Value string handling
                        string[] tagFormatValue = sl.Trim().Split(' ');
                        if (tagFormatValue.Length == 3)
                        {
                            DataRow dataRow = table.NewRow();
                            dataRow["Tag"] = tagFormatValue[0];
                            dataRow["Format"] = tagFormatValue[1];
                            dataRow["Value"] = tagFormatValue[2];

                            if (!string.IsNullOrEmpty(description))
                            {
                                dataRow["Description"] = description;
                            }
                            table.Rows.Add(dataRow);
                        }
                    }

                }
                confDataSet.Tables.Add(table);
                listBoxConfig.Items.Add(table.TableName);
            }
            catch (FileNotFoundException)
            {
                throw new System.IO.FileNotFoundException("File: {0} Not Found.", fileName);
            }

        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            openFileDialog = new OpenFileDialog();
            string path = String.Empty;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                listBoxConfig.Items.Clear();
                confDataSet.Tables.Clear();
                path = Path.GetDirectoryName(openFileDialog.FileName) + @"\";
                //Console.WriteLine(path);
                try
                {
                    var sr = new StreamReader(openFileDialog.FileName);
                    while (!sr.EndOfStream)
                    {
                        string configFileName = sr.ReadLine().Trim();
                        if (!string.IsNullOrEmpty(configFileName) &&
                            File.Exists(path + configFileName))
                        {
                            BuildConfigDataSet(path + configFileName);
                        }
                    }
                    if (listBoxConfig.Items.Count > 0)
                    {
                        listBoxConfig.SelectedIndex = 0;
                    }
                }
                catch (FileNotFoundException)
                {

                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        private void listBoxConfig_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxConfig.SelectedItem == null) return;

            string configName = listBoxConfig.SelectedItem.ToString();
            if (confDataSet.Tables.Contains(configName))
            {
                int tableIndex = confDataSet.Tables.IndexOf(configName);
                dataGridView1.DataSource = confDataSet.Tables[tableIndex];
                dataGridView1.AutoResizeColumns();
            }
            //dataGridView1.DataSource = confDataSet.Tables[listBoxConfig.SelectedIndex];
            //dataGridView1.AutoResizeColumns();
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            openFileDialog = new OpenFileDialog();
            //openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    BuildConfigDataSet(openFileDialog.FileName);
                }
                catch (DuplicateNameException)
                {
                    MessageBox.Show($"{openFileDialog.FileName} \n\n\talready exists");
                }
            }
        }

        private void btnDeleteItem_Click(object sender, EventArgs e)
        {

            if (listBoxConfig.SelectedItem == null) return;

            string configName = listBoxConfig.SelectedItem.ToString();

            if (confDataSet.Tables.Contains(configName))
            {
                confDataSet.Tables.Remove(configName);
            }

            listBoxConfig.Items.Remove(listBoxConfig.SelectedItem);
        }
    }
}
