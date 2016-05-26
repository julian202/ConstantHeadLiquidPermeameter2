using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConstantHeadLiquidPermeameter2
{
  public partial class SetupForm : Form
  {
    Properties.Settings settings = Properties.Settings.Default;

    public SetupForm()
    {
      InitializeComponent();
    }

    private void SetupForm_Load(object sender, EventArgs e)
    {
      //autoFindBoardPort();
      if (settings.maxRepetitions>1)
      {
        label17.Text = "The program will run up to " + settings.maxRepetitions + " rounds until the value is within " + settings.stabilizeWithin.ToString() + "g of the mean.";
      }
      else
      {
        label17.Text = "";
        label18.Text = "";
      }

      COMMS.autoFindPort();
      if (COMMS.connected == false)
      {
        Close();
        return;
      }
      //string testThatCOMworks = COMMS.ReadScale().ToString(); //reads the first weight value which will be first data point.

      try
      {
        textBox1.Text = settings.timerTimeMinutes.ToString();
      }
      catch (Exception)
      {
      }
      try
      {
        textBoxSeconds.Text = settings.timerTimeSeconds.ToString();
      }
      catch (Exception)
      {
      }

      try
      {
        textBox2.Text = settings.dataAcquisitionInterval.ToString();
      }
      catch (Exception)
      {
      }

      if (settings.upperSelected)
      {
        radioButton1.Checked = true;
        textBoxHeight.Text = settings.upperHeight;
      }
      else if (settings.middleSelected)
      {
        radioButton2.Checked = true;
        textBoxHeight.Text = settings.middleHeight;
      }
      else if (settings.lowerSelected)
      {
        radioButton3.Checked = true;
        textBoxHeight.Text = settings.lowerHeight;
      }

      textBoxThickness.Text = settings.sampleThickness;
      textBoxDiameter.Text = settings.diameter;
      textBoxViscosity.Text = settings.viscosity;

      //textBoxHeight.Text = settings.height;

      loadPathString();


       /* //new atira
      //COMMS.openValve2();// we open valve 2 here so that the user has time to fill up the reservoir before starting the test
      COMMS.closeValve2();
      Thread.Sleep(800);
      COMMS.openValve1();
      Thread.Sleep(800);
      COMMS.closeValve3();   */

    }

    public void loadPathString()
    {
      if (Properties.Settings.Default.dataPath == "")
      {
        string path = System.IO.Path.Combine(Environment.GetFolderPath(
    Environment.SpecialFolder.MyDoc‌​uments), "PMI", "data.pmi");
        //MessageBox.Show(path);
        textBoxPath.Text = path;
      }
      else
      {
        textBoxPath.Text = Properties.Settings.Default.dataPath;
      }
      if (File.Exists(textBoxPath.Text))
      {
        string previousString = textBoxPath.Text;
        int num;
        string previousStringCropped = previousString.Substring(0, previousString.Length - 8);
        string croppedPart = previousString.Substring(previousString.Length - 7, 3);
        string dash = previousString.Substring(previousString.Length - 8, 1);
        if (IsNumeric(croppedPart) && (dash == "-"))
        {
          num = Convert.ToInt32(croppedPart) + 1;
          textBoxPath.Text = previousStringCropped + "-" + num.ToString("000") + ".pmi";
        }
        else
        {
          textBoxPath.Text = previousString.Substring(0, previousString.Length - 4) + "-001" + ".pmi";
        }
      }
    }

    public bool IsNumeric(string input)
    {
      double test;
      return double.TryParse(input, out test);
    }

    private void button1_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void buttonContinue_Click(object sender, EventArgs e)
    {

      if ((textBoxHeight.Text == "") || (textBoxViscosity.Text == "") || (textBoxDiameter.Text == "") || (textBoxThickness.Text == ""))
      {
        MessageBox.Show("You must enter a value for all fields");
        return;
      }

      try
      {
       settings.area = (3.1415926 * (Convert.ToDouble(textBoxDiameter.Text) * Convert.ToDouble(textBoxDiameter.Text)) / 4).ToString("#0.000");
        settings.Save();
      }
      catch (Exception)
      {
        MessageBox.Show("You must enter a valid value for diameter");
      }   

      settings.sampleThickness = textBoxThickness.Text;
      settings.diameter = textBoxDiameter.Text;
      settings.viscosity = textBoxViscosity.Text;
      if (radioButton1.Checked)
      {
        settings.upperSelected = true;
        settings.middleSelected = false;
        settings.lowerSelected = false;
        settings.upperHeight = textBoxHeight.Text;
      }
      else if (radioButton2.Checked)
      {
        settings.upperSelected = false;
        settings.middleSelected = true;
        settings.lowerSelected = false;
        settings.middleHeight = textBoxHeight.Text;
      }
      else if (radioButton3.Checked)
      {
        settings.upperSelected = false;
        settings.middleSelected = false;
        settings.lowerSelected = true;
        settings.lowerHeight = textBoxHeight.Text;
      }
      settings.height = textBoxHeight.Text;
      settings.Save();

      if (File.Exists(textBoxPath.Text))
      {
        DialogResult result = MessageBox.Show("This data file already exists. Do you want to overwrite it?", "Overwrite data file?", MessageBoxButtons.YesNo);
        if (result == System.Windows.Forms.DialogResult.No)
        {
          return;
        }
      }

      //create directory
      try
      {
        Directory.CreateDirectory((Directory.GetParent(textBoxPath.Text)).ToString());
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }     

      settings.dataPath = textBoxPath.Text;
      settings.Save();

      this.DialogResult = DialogResult.OK;
      this.Close();
    }


    

    private void buttonFileDialog_Click(object sender, EventArgs e)
    {
      saveFileDialog1.InitialDirectory = System.IO.Path.Combine(Environment.GetFolderPath(
    Environment.SpecialFolder.MyDoc‌​uments), "PMI");
      saveFileDialog1.FileName = textBoxPath.Text;
      saveFileDialog1.ShowDialog();
      if (saveFileDialog1.FileName != "")
      {
        textBoxPath.Text = saveFileDialog1.FileName;
      }
    }

    private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      string path = System.IO.Path.Combine(Environment.GetFolderPath(
   Environment.SpecialFolder.MyDoc‌​uments), "PMI", "data-001.pmi");
      //MessageBox.Show(path);
      textBoxPath.Text = path;
    }

    private void textBox2_TextChanged(object sender, EventArgs e)
    {
      try
      {
        settings.dataAcquisitionInterval = Convert.ToDouble(textBox2.Text);
        settings.Save();
        //timerReadScale.Interval = 1000 * settings.dataAcquisitionInterval;
      }
      catch (Exception)
      {
        MessageBox.Show("Please enter an integer number");
      }
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
      try
      {
        settings.timerTimeMinutes = Convert.ToInt32(textBox1.Text);
        settings.Save();
        //timerReadScale.Interval = 1000 * settings.dataAcquisitionInterval;
      }
      catch (Exception)
      {
        MessageBox.Show("Please enter an integer number");
      }
    }

    private void textBoxPath_TextChanged(object sender, EventArgs e)
    {

    }

    private void textBoxSeconds_TextChanged(object sender, EventArgs e)
    {
      try
      {
        settings.timerTimeSeconds = Convert.ToInt32(textBoxSeconds.Text);
        settings.Save();
        //timerReadScale.Interval = 1000 * settings.dataAcquisitionInterval;
      }
      catch (Exception)
      {
        MessageBox.Show("Please enter an integer number");
      }
    }

    private void radioButton1_CheckedChanged(object sender, EventArgs e)
    {
      if (radioButton1.Checked)
      {
        settings.upperSelected = true;
        settings.middleSelected = false;
        settings.lowerSelected = false;
        settings.Save();
        textBoxHeight.Text = settings.upper;
      }
    }

    private void radioButton2_CheckedChanged(object sender, EventArgs e)
    {
      if (radioButton2.Checked)
      {
        settings.upperSelected = false;
        settings.middleSelected = true;
        settings.lowerSelected = false;
        settings.Save();
        textBoxHeight.Text = settings.middle;
      }
    }

    private void radioButton3_CheckedChanged(object sender, EventArgs e)
    {
      if (radioButton3.Checked)
      {
        settings.upperSelected = false;
        settings.middleSelected = false;
        settings.lowerSelected = true;
        settings.Save();
        textBoxHeight.Text = settings.lower;
      }
    }

    private void textBoxThickness_TextChanged(object sender, EventArgs e)
    {

    }

    private void textBoxHeight_TextChanged(object sender, EventArgs e)
    {

    }

    private void textBoxDiameter_TextChanged(object sender, EventArgs e)
    {

    }
  }
}
