using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConstantHeadLiquidPermeameter2
{
  public partial class export : Form
  {
    public export()
    {
      InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      if (textBox1.Text.Length == 0)
      {
        MessageBox.Show("You must set a directory to export your data to!", "Burst Tester", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      if (radioButton1.Checked) Properties.Settings.Default.ExportOption = 0;
      if (radioButton2.Checked) Properties.Settings.Default.ExportOption = 1;
      if (radioButton3.Checked) Properties.Settings.Default.ExportOption = 2;
      Properties.Settings.Default.ExportPath = textBox1.Text;
      Properties.Settings.Default.Save();
    }

    private void button3_Click(object sender, EventArgs e)
    {
      folderBrowserDialog1.ShowDialog();
      textBox1.Text = folderBrowserDialog1.SelectedPath;
    }

    private void export_Load(object sender, EventArgs e)
    {
      //textBox1.Text = Properties.Settings.Default.ExportPath;
      textBox1.Text = System.IO.Directory.GetParent(Properties.Settings.Default.dataPath).ToString();
      if (Properties.Settings.Default.AutoDataFile)
      {
        textBox1.Enabled = true;
        button3.Enabled = true;
      }
    }

    private void radioButton2_CheckedChanged(object sender, EventArgs e)
    {
      if (radioButton2.Checked && !Properties.Settings.Default.AutoDataFile)
      {
        folderBrowserDialog1.ShowDialog();
        textBox1.Text = folderBrowserDialog1.SelectedPath;
      }
      if (Properties.Settings.Default.AutoDataFile)
      {
        textBox1.Text = Properties.Settings.Default.ExportPath;
      }
    }
  }
}
