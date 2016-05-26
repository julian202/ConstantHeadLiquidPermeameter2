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
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ConstantHeadLiquidPermeameter2
{
  public partial class FormManual : Form
  {
    Properties.Settings settings = Properties.Settings.Default;
    Stopwatch stopwatch = new Stopwatch();
    private string timeString;
    private string scaleReading;
    private string lastValidScaleReading;
    TimeSpan timeSpan;
    private double timeInMinutes = 0;
    private double timeInSeconds = 0;
    public double lastTimeInSeconds = 0;
    StreamWriter SR;
    int myBorderWidth = Convert.ToInt32(Properties.Settings.Default.myBorderWidth);
    double originalWeight;
    double currentNetWeight;
    string currentNetWeightString;
    string filePath;
    private bool testRunning;
    bool closeValve1;
    bool closeValve2;
    bool closeValve3;
    bool openValve1;
    bool openValve2;
    bool openValve3;
    private bool testStopped = false;
    string[] splitReceived;
    string[] splitReceivedB;
    private string filtered;
    private bool stoppedTestWithoutReport = false;
    private string vel;

    public FormManual()
    {
      InitializeComponent();

    }

    private void FormManaul_Load(object sender, EventArgs e)
    {
      testStopped = false;
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


      if (settings.checkBoxZeroScale)
      {
        checkBoxZeroScale.Checked = true;
      }
      else
      {
        checkBoxZeroScale.Checked = false;
      }


      COMMS.autoFindPort();
      //COMMS.openSerial("COM3");    
      //MessageBox.Show(serialPort1.IsOpen.ToString());

      if (COMMS.connected == false)
      {
        Close();
      }
      //scaleReading = COMMS.ReadScaleAWSContinuous().ToString(); //reads the first weight value which will be first data point.

      /*
      try
      {
        originalWeight = Convert.ToDouble(scaleReading);
      }
      catch (Exception)
      {
        Debug.WriteLine("ERROR CONVERTING ORIGINAL scaleReading to DOUBLE");
        originalWeight = 0;
      }*/


      currentNetWeight = 0;
      loadPathString();
      textBox1.Text = settings.dataAcquisitionInterval.ToString();
      checkBoxSaveData.Checked = settings.checkBoxSaveData;

      //enable this next line when ready!!!!!!!!!!!!!!!!
      //timerReadScale.Start();
      if (backgroundWorkerReadScale.IsBusy != true)
      {
        backgroundWorkerReadScale.RunWorkerAsync();
      }

      //chart1.Series["Series1"].Points.AddXY(String.Format("{0:#0.00}", 0), 0);
      chart1.Series["Series1"].BorderWidth = myBorderWidth;
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

    private void button2_Click(object sender, EventArgs e)
    {
      double d = COMMS.ReadScale();
      labelScale.Text = "Weight = " + d.ToString() + " g";
    }
    private void button8_Click(object sender, EventArgs e)
    {
      string d = COMMS.ReadScaleAWS();
      labelScale.Text = "Weight = " + d + " g";
    }
    public void Output(string s)
    {
      System.Diagnostics.Debug.WriteLine(s);
    }

    private void button3_Click(object sender, EventArgs e)
    {
      SettingsForm sf = new SettingsForm();
      sf.ShowDialog();
      //update com port in case it has been changed in the settings form:
      serialPort1.Close();
      serialPort1.PortName = settings.COMM;
      try
      {
        serialPort1.Open();
      }
      catch (Exception)
      {
        //MessageBox.Show("The selected port can't be opened. Please select a different port.");
      }
    }

    private void timerReadScale_Tick(object sender, EventArgs e)
    {
      readWeight();
    }

    private void readWeight()
    {/*

      //scaleReading = COMMS.ReadScale().ToString();
      //scaleReading = COMMS.ReadScaleAWSContinuous();
      scaleReading = COMMS.ReadScaleAWS();
      if (scaleReading != "" && !scaleReading.Contains("M") && scaleReading.Length > 2)
      {
        scaleReading = scaleReading.Substring(2, scaleReading.Length - 3);
        netWeight();
        if (testRunning)
        {
          addDataPoint();
        }
        if (currentNetWeight > settings.maxWeight)
        {
          stopTest();
          MessageBox.Show("The test was stopped because the maximum weight was reached");
        }
      }

      */
    }

    private void netWeight()
    {
      Debug.WriteLine("lastValidScaleReading is");
      Debug.WriteLine(lastValidScaleReading);

      if (checkBoxZeroScale.Checked)
      {
        currentNetWeight = 1000 * (Convert.ToDouble(lastValidScaleReading) - originalWeight);  //use this to remove original weight
        //currentNetWeight = Convert.ToDouble(lastValidScaleReading);
      }
      else
      {
        currentNetWeight = 1000 * Convert.ToDouble(lastValidScaleReading);
      }

      currentNetWeightString = currentNetWeight.ToString("0");
      labelScale.Text = "Weight = " + currentNetWeightString + " g";
    }

    private void button1_Click(object sender, EventArgs e)
    {
      openValve1 = true;
      //COMMS.openValve1();
    }

    private void buttonCloseValve_Click(object sender, EventArgs e)
    {
      closeValve1 = true;
      //COMMS.closeValve1();
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
      try
      {
        settings.dataAcquisitionInterval = Convert.ToDouble(textBox1.Text);
        settings.Save();
        //timerReadScale.Interval = 1000 * settings.dataAcquisitionInterval;
      }
      catch (Exception)
      {
        MessageBox.Show("Please enter an integer number");
      }
    }

    private void buttonStopTimer_Click(object sender, EventArgs e)
    {
      stopTest();
    }

    private void stopTest()
    {
      stopProc();

      if (settings.checkBoxSaveData) //don't show report if data hasn't been saved
      {
        Report setScrn = new Report();
        setScrn.ShowDialog();
      }
      //this.Show();
      Close();
    }

    private void stopTestWithoutReport()
    {
      stoppedTestWithoutReport = true;
      stopProc();
      stoppedTestWithoutReport = false;
    }

    private void stopProc()
    {
      /*
         //close valve
         COMMS.closeValve2();
         Thread.Sleep(800);
         COMMS.openValve3();*/

      /*// new atira
      closeValve2 = true;
      openValve3 = true;
      */

      testRunning = false;
      testStopped = true; //this needed because bgworker doesnt end

      if (!stoppedTestWithoutReport)
      {
        calcPerm();
      }    

      timerReadScale.Stop();
      //stopwatch.Reset();
      stopwatch.Stop();
      backgroundWorkerElapsedTime.CancelAsync();
      backgroundWorkerReadScale.CancelAsync();
      //this.Hide();

      /*// new atira
      Thread.Sleep(500);
      COMMS.closeValve2();
      Thread.Sleep(200);
      COMMS.openValve3();
      */
    }


    private void calcPerm()
    {
      double perm;
      settings.timeInMinutes = timeInMinutes;
      settings.timeInSeconds = timeInSeconds;
      settings.netWeight = currentNetWeightString;
      settings.Save();
      double flow = Convert.ToDouble(currentNetWeightString) / Convert.ToDouble(timeInSeconds);
      double thickness = Convert.ToDouble(settings.sampleThickness);
      double viscosity = Convert.ToDouble(settings.viscosity);
      double k1 = flow * thickness * viscosity * 14.7; //the 14.7 is a conversion factor, NOT atmospheric pressure! It lumps together the conversion from centipoise to poise for viscosity, the conversion from PSI to dynes/cm^2, and the conversion from cm^2 to Darcies.
      double area = 3.1415926 * (Convert.ToDouble(settings.diameter) * Convert.ToDouble(settings.diameter)) / 4;

      //perm = k1  / (60 * area * Convert.ToDouble(textBoxPressure.Text)); //if time were in minutes
      perm = k1 / (area * (0.014 * Convert.ToDouble(settings.height))); //1m =1.4PSI => 1cm=0.014PSI

      //write to last line of file
      try
      {
        filePath = settings.dataPath;
        SR = new StreamWriter(filePath, true);
        /*
        SR.WriteLine("");
        SR.WriteLine("Calculated Permeability (Darcies) =\t" + perm.ToString("#.00000"));*/
        SR.WriteLine("");
        SR.WriteLine("Total Weight = " + currentNetWeightString + " g");
        SR.WriteLine("Total Time = " + lastTimeInSeconds.ToString("#0.00") + " Secs");
        //SR.WriteLine("Flow Velocity = " + (weight / (Convert.ToDouble(settings.area) * time)).ToString("#0.000") + " cm/s");

        vel = (Convert.ToDouble(currentNetWeightString) / (Convert.ToDouble(settings.area) * Convert.ToDouble(lastTimeInSeconds.ToString("#0.00")))).ToString("#0.000");
        SR.WriteLine("Flow Velocity = " + vel + " cm/s");
        CommonClass.addVelToLast5(vel);
        SR.Close();
      }
      catch (Exception)
      {
      }

    }

    private void buttonStartTimer_Click(object sender, EventArgs e)
    {/*
      DialogResult dialogResult = MessageBox.Show("Before starting always check that the weight scale container is not full of liquid", "", MessageBoxButtons.OKCancel);
      if (dialogResult == DialogResult.Cancel)
      {
        return;
      }*/

      try
      {
        Convert.ToDouble(textBoxHeight.Text);
      }
      catch (Exception)
      {
        MessageBox.Show("The value for Liquid Column (Head Difference) must be a valid number!");
        return;
      }

      buttonStartTimer.Enabled = false;
      if (!checkBoxSaveData.Checked)
      {
        DialogResult dialogResult = MessageBox.Show("Are you sure you don't want to save the data? (No report will be created)", "You didn't check the Save Data checkbox", MessageBoxButtons.OKCancel);
        if (dialogResult == DialogResult.Cancel)
        {
          buttonStartTimer.Enabled = true;
          return;
        }
      }



      if (checkBoxSaveData.Checked)
      {
        if ((textBoxHeight.Text == "") || (textBoxViscosity.Text == "") || (textBoxDiameter.Text == "") || (textBoxThickness.Text == ""))
        {
          MessageBox.Show("You must enter a value for all fields");
          return;
        }

        if (File.Exists(textBoxPath.Text))
        {
          DialogResult result = MessageBox.Show("This data file already exists. Do you want to overwrite it?", "Overwrite data file?", MessageBoxButtons.YesNo);
          if (result == System.Windows.Forms.DialogResult.No)
          {
            buttonStartTimer.Enabled = true;
            return;
          }
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

        settings.dataPath = textBoxPath.Text;
        settings.Save();

        //create directory
        try
        {
          Directory.CreateDirectory((Directory.GetParent(textBoxPath.Text)).ToString());
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message);
        }
        CommonClass.writeHeader();
      }

      /*
      //close valve 2
     
      COMMS.openValve2(); //sample valve
      Thread.Sleep(800);
      COMMS.closeValve3(); //weight valve
      Thread.Sleep(800);
      COMMS.openValve1(); //top reservoir valve
      */

      //open valve 2:
      if (checkBoxV2OnStart.Checked)
      {
        //COMMS.openValve2();

        /*// new atira
        openValve2 = true;
        */
      }

      stopwatch.Reset();
      stopwatch.Start();

      /*
      scaleReading = COMMS.ReadScale().ToString(); //reads the first weight value which will be first data point.
      originalWeight = Convert.ToDouble(scaleReading);
      netWeight();*/

        /*
        while (true) //get a first reading 
        {
          scaleReading = COMMS.ReadScaleAWS();
          if (scaleReading != "" && !scaleReading.Contains("M") && scaleReading.Length > 2)
          {
            scaleReading = scaleReading.Substring(2, scaleReading.Length - 3);
            originalWeight = Convert.ToDouble(scaleReading);
            netWeight();
            break;
          }
        }*/
      originalWeight = Convert.ToDouble(lastValidScaleReading);


      if (backgroundWorkerElapsedTime.IsBusy != true)
      {
        backgroundWorkerElapsedTime.RunWorkerAsync();
      }

      testRunning = true;
    }

    /*
    private void writeHeader()
    {
      filePath = settings.dataPath;
      SR = new StreamWriter(filePath);
      SR.WriteLine("PMI Liquid Permeability Test");
      SR.WriteLine("Test Date = " + DateTime.Now);
      SR.WriteLine("Sample Name = " + Path.GetFileNameWithoutExtension(filePath));
      SR.WriteLine("Liquid Height = " + settings.height + " mm");
      //SR.WriteLine("Sample Thickness (cm) = " + settings.sampleThickness);
      SR.WriteLine("Sample Diameter = " + settings.diameter + " cm");
      // SR.WriteLine("Liquid Viscosity (cP) = " + settings.viscosity);
      SR.WriteLine("Sample Area = " + settings.area + " cm^2");
      SR.WriteLine("");
      SR.WriteLine("Time (Seconds)" + "\t" + "Weight (Grams)");
      SR.WriteLine("");
      SR.Close();
    }*/

    private void buttonPause_Click(object sender, EventArgs e)
    {
      timerReadScale.Stop();
      backgroundWorkerElapsedTime.CancelAsync();
      stopwatch.Stop();
    }

    private void backgroundWorkerElapsedTime_DoWork(object sender, DoWorkEventArgs e)
    {
      while (true)
      {
        if ((backgroundWorkerElapsedTime.CancellationPending == true))
        {
          e.Cancel = true;
          break;
        }
        timeSpan = stopwatch.Elapsed;
        timeString = String.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
        //timeInMinutes = Convert.ToDouble(timeSpan.Minutes) + Convert.ToDouble(timeSpan.Seconds) / 60 + Convert.ToDouble(timeSpan.Milliseconds) / 60000;
        timeInMinutes = timeSpan.TotalMinutes;
        timeInSeconds = timeSpan.TotalSeconds;
        backgroundWorkerElapsedTime.ReportProgress(0);
        Thread.Sleep(50);
      }
    }

    private void backgroundWorkerElapsedTime_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      labelTime.Text = "Time = " + timeString;
    }

    private void addDataPoint()
    {
      chart1.Series["Series1"].Points.AddXY(String.Format("{0:#0.00}", timeInSeconds), currentNetWeightString);
      if (checkBoxSaveData.Checked)
      {
        try
        {
          SR = new StreamWriter(textBoxPath.Text, true);
          //SR.WriteLine(String.Format("{0:#0.00}", timeInSeconds) + "\t" + currentNetWeightString);
          SR.WriteLine("{0,10}\t{1,10}", timeInSeconds.ToString("#0.00"), currentNetWeightString);
          lastTimeInSeconds = timeInSeconds;
          //SR.WriteLine("{0,10}\t{1,10}\t{2,10}\t{3,10}\t{4,10}", "Time", "Flow", "Temperature", "Pressure", "Permeability");
          SR.Close();
        }
        catch (Exception)
        {
        }
      }
    }

    private void checkBoxSaveData_CheckedChanged(object sender, EventArgs e)
    {
      if (checkBoxSaveData.Checked)
      {
        settings.checkBoxSaveData = true;
        settings.Save();
      }
      else
      {
        settings.checkBoxSaveData = false;
        settings.Save();
      }
    }

    private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      string path = System.IO.Path.Combine(Environment.GetFolderPath(
   Environment.SpecialFolder.MyDoc‌​uments), "PMI", "data-001.pmi");
      //MessageBox.Show(path);
      textBoxPath.Text = path;
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

    private void FormManaul_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!COMMS.cancelling)
      {
        stopTestWithoutReport();
      }

    }

    private void FormManaul_FormClosed(object sender, FormClosedEventArgs e)
    {
      timerReadScale.Stop();
      /*while (commBusy)
      {
      }*/
      serialPort1.Close();
    }

    private void textBoxPath_TextChanged(object sender, EventArgs e)
    {
    }

    private void button5_Click(object sender, EventArgs e)
    {
      openValve2 = true;
      //COMMS.openValve2();
    }

    private void button4_Click(object sender, EventArgs e)
    {
      closeValve2 = true;
      //COMMS.closeValve2();
    }

    private void button7_Click(object sender, EventArgs e)
    {
      openValve3 = true;
      //COMMS.openValve3();
    }

    private void button6_Click(object sender, EventArgs e)
    {
      closeValve3 = true;
      //COMMS.closeValve3();
    }

    private void buttonZeroScale_Click(object sender, EventArgs e)
    {
      COMMS.zeroScale();
    }

    private void checkBoxZeroScale_CheckedChanged(object sender, EventArgs e)
    {
      if (settings.checkBoxZeroScale)
      {
        settings.checkBoxZeroScale = true;
      }
      else
      {
        settings.checkBoxZeroScale = false;
      }
      settings.Save();
    }

    private void radioButton1_CheckedChanged(object sender, EventArgs e)
    {
      doRadioButtonCheckedChange();
    }

    private void radioButton2_CheckedChanged(object sender, EventArgs e)
    {
      doRadioButtonCheckedChange();
    }

    private void radioButton3_CheckedChanged(object sender, EventArgs e)
    {
      doRadioButtonCheckedChange();
    }

    private void doRadioButtonCheckedChange()
    {
      if (radioButton1.Checked)
      {
        settings.upperSelected = true;
        settings.middleSelected = false;
        settings.lowerSelected = false;
        settings.Save();
        textBoxHeight.Text = settings.upper;
      }

      if (radioButton2.Checked)
      {
        settings.upperSelected = false;
        settings.middleSelected = true;
        settings.lowerSelected = false;
        settings.Save();
        textBoxHeight.Text = settings.middle;
      }

      if (radioButton3.Checked)
      {
        settings.upperSelected = false;
        settings.middleSelected = false;
        settings.lowerSelected = true;
        settings.Save();
        textBoxHeight.Text = settings.lower;
      }
    }

    private void backgroundWorkerReadScale_DoWork(object sender, DoWorkEventArgs e)
    {
      while (true)
      {
        if (testStopped)
        {
          Debug.WriteLine("RETURN; hit");
          Debug.WriteLine("RETURN; hit");
          Debug.WriteLine("RETURN; hit");
          return;
        }
        if (openValve1)
        {
          COMMS.openValve4();
          openValve1 = false;
        }
        if (closeValve1)
        {
          COMMS.closeValve4();
          closeValve1 = false;
        }
        if (openValve2)
        {
          COMMS.openValve2();
          openValve2 = false;
        }
        if (openValve3)
        {
          COMMS.openValve3();
          openValve3 = false;
        }

        if (closeValve2)
        {
          COMMS.closeValve2();
          //MessageBox.Show("Test");
          closeValve2 = false;
        }
        if (closeValve3)
        {
          COMMS.closeValve3();
          closeValve3 = false;
        }
        
        scaleReading = COMMS.ReadScaleAWS();
        if (scaleReading.Length > 4)
        {
          //lastValidScaleReading = filterReading(scaleReading);
          lastValidScaleReading = CommonClass.filterRegex(scaleReading);     
          backgroundWorkerReadScale.ReportProgress(0);
        }
          
        /*
        scaleReading = COMMS.ReadScaleAWS();
        //if (scaleReading != "" && !scaleReading.Contains("M") && scaleReading.Length > 2)
        if (scaleReading != "" && scaleReading.Length > 4)
        {
          splitReceived = scaleReading.Split(new string[] { "g" }, StringSplitOptions.None);
          splitReceived = splitReceived[0].Split(new string[] { ":" }, StringSplitOptions.None);
          scaleReading = splitReceived[1];
          //scaleReading = scaleReading.Substring(2, scaleReading.Length - 3);
          if (scaleReading != "")
          {
            lastValidScaleReading = scaleReading;
          }
          

          backgroundWorkerReadScale.ReportProgress(0);
          if ((backgroundWorkerReadScale.CancellationPending == true))
          {
            Debug.WriteLine("e.Cancel ; hit");
            Debug.WriteLine("e.Cancel ; hit");
            Debug.WriteLine("e.Cancel ; hit");
            e.Cancel = true;
            break;
          }        
        }
        */

        Thread.Sleep(150);
      }
    }

    private void backgroundWorkerReadScale_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      netWeight();
      if (testRunning)
      {
        addDataPoint();
        if (currentNetWeight > settings.maxWeight)
        {
          //stopTest();
          stopTestWithoutReport();
          MessageBox.Show("The test was stopped because the maximum weight was reached");
        }
      }
    }

    private string filterReading(string received)
    {
      splitReceived = received.Split(new string[] { "US" }, StringSplitOptions.None);
      if (splitReceived.Length == 1)
      {
        splitReceived = received.Split(new string[] { "ST" }, StringSplitOptions.None);
      }

      if (splitReceived[1].Contains("kg"))
      {
        splitReceivedB = splitReceived[1].Split(new string[] { "kg" }, StringSplitOptions.None);
        if (splitReceivedB[0].Length != 8)
        {
          {
            splitReceivedB = splitReceived[2].Split(new string[] { "kg" }, StringSplitOptions.None);
          }
        }
      }
      else
      {
        try
        {
          splitReceivedB = splitReceived[2].Split(new string[] { "kg" }, StringSplitOptions.None);
        }
        catch (Exception)
        {
          //use the old one:
          splitReceivedB[0] = CommonClass.previouslyFiltered;
        }
      }

      filtered = splitReceivedB[0];
      //keep a copy in case the next one fails:
      CommonClass.previouslyFiltered = filtered;
      return filtered;
    }

    private void button9_Click(object sender, EventArgs e)
    {
      originalWeight = Convert.ToDouble(lastValidScaleReading);
    }

    private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Process.Start("explorer.exe", System.IO.Directory.GetParent(textBoxPath.Text).ToString());
    }

    private void groupBox6_Enter(object sender, EventArgs e)
    {

    }

    private void textBoxHeight_TextChanged(object sender, EventArgs e)
    {

    }
  }
}
