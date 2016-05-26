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

namespace ConstantHeadLiquidPermeameter2
{
  public partial class SettingsForm : Form
  {
    Properties.Settings settings = Properties.Settings.Default;

    public SettingsForm()
    {
      InitializeComponent();
    }

    private void SettingsForm_Load(object sender, EventArgs e)
    {
      loadPortList();
      textBoxWeight.Text= settings.maxWeight.ToString();
      textBoxRepetitions.Text = settings.maxRepetitions.ToString();
      textBoxWithin.Text = settings.stabilizeWithin.ToString();
    }

    private void loadPortList()
    {
      //add a selection for demo mode
      comboBox1.Items.Add("Demo");
      foreach (string port in SerialPort.GetPortNames())
      {
        //iterate through available ports and add them for selection.
        comboBox1.Items.Add(port);
        //if we have found our last selected port, set it as the selected item.
        if (port == Properties.Settings.Default.COMM)
        {
          comboBox1.SelectedIndex = comboBox1.FindStringExact(port);
        }
      }

    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      //MessageBox.Show(comboBox1.SelectedItem.ToString());
      settings.COMM = comboBox1.SelectedItem.ToString();
      settings.Save();
    }

    private void button2_Click(object sender, EventArgs e)
    {
      try
      {

        if (Convert.ToInt32(textBoxRepetitions.Text) > 10)
        {
          MessageBox.Show("The maximum number of repetitions is 10"); //change tablerarray to more if needed.
          return;
        }
        settings.maxWeight = Convert.ToInt32(textBoxWeight.Text);
        settings.maxRepetitions = Convert.ToInt32(textBoxRepetitions.Text);
        settings.stabilizeWithin = Convert.ToDouble(textBoxWithin.Text);
        settings.Save();
      }
      catch (Exception)
      {
        MessageBox.Show("Value must be an integer");
        return;
      }            
      Close();
    }

    private void textBoxWeight_TextChanged(object sender, EventArgs e)
    {

    }

    private void textBoxRepetitions_TextChanged(object sender, EventArgs e)
    {

    }
  }
}
