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

namespace ConstantHeadLiquidPermeameter2
{
  public partial class FormAuto : Form
  {
    Properties.Settings settings = Properties.Settings.Default;
    Stopwatch stopwatch = new Stopwatch();
    private string timeString;
    private string scaleReading;
    private string lastValidScaleReading = "";
    TimeSpan timeSpan;
    private double timeInMinutes = 0;
    private double timeInSeconds = -1;
    StreamWriter SR;
    int myBorderWidth = Convert.ToInt32(Properties.Settings.Default.myBorderWidth);
    string filePath;
    int timerTimeMinutes;
    int timerTimeSeconds;
    TimeSpan spanMaxTime;
    double originalWeight;
    double currentNetWeight;
    string currentNetWeightString;
    string vel;
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
    public double lastTimeInSeconds = 0;
    private bool stoppedTestWithoutReport = false;
    private string k;
    DataTable[] tableArray = new DataTable[11];
    DataTable finalWeights = new DataTable("finalWeights");
    private bool lastRound = false;
    int round = 1;
    private bool newround = true;
    string lastWeight;
    private string lastw;
    private string lastT;
    private double average;
    private bool testCancelledByUser = false;

    public FormAuto(string z)
    {
      k = z;
      InitializeComponent();
    }

    private void FormAuto_Load(object sender, EventArgs e)
    {
      finalWeights.Columns.Add("Weights");
      labelLast.Text = "";
      labelAverage.Text = "";
      for (int i = 0; i < tableArray.Length; i++)
      {
        tableArray[i] = new DataTable("DataTable " + i);
        tableArray[i].Columns.Add("Time");
        tableArray[i].Columns.Add("Weight");
      }

      testStopped = false;
      labelSavingFile.Text = "Saving data to file: " + settings.dataPath;
      label1.Text = "or when weight reaches " + settings.maxWeight.ToString() + "g";

      timerTimeMinutes = settings.timerTimeMinutes;
      timerTimeSeconds = settings.timerTimeSeconds;
      spanMaxTime = new TimeSpan(0, 0, timerTimeMinutes, timerTimeSeconds, 0);
      //timerTime = timerTimeDouble.ToString();    
      labelRunning.Text = "Test run time " + timerTimeMinutes.ToString("00") + ":" + timerTimeSeconds.ToString("00");

      if (backgroundWorkerReadScale.IsBusy != true)
      {
        backgroundWorkerReadScale.RunWorkerAsync();
      }
      chart1.Series["Series1"].BorderWidth = myBorderWidth;

      //writeHeader();

      while (lastValidScaleReading == "")
      {
        Thread.Sleep(10);
      }

      Thread.Sleep(20);//wait for bgworker to raed a value
      originalWeight = Convert.ToDouble(lastValidScaleReading);

      stopwatch.Start();
      if (backgroundWorkerElapsedTime.IsBusy != true)
      {
        //backgroundWorkerElapsedTime.RunWorkerAsync();
        timerTimeLabel.Start();
      }
    }

    public void SaveDataTableToFile()
    {
      writeHeader();
      SR = new StreamWriter(filePath, true);
      //option 1: first write the data from the last round
      /*foreach (DataRow row in tableArray[round - 2].Rows)
      {
        SR.WriteLine("{0,10}\t{1,10}", row.Field<string>(0), row.Field<string>(1));
        lastw = row.Field<string>(1);
        lastT = row.Field<string>(0);
      }*/

      //option 2: write the average data from all the rounds
      int rowcount = 0;
      try
      {
        foreach (DataRow row in tableArray[round - 2].Rows)
        {
          double sum = 0;
          double average = 0;

          for (int i = 0; i < (round - 1); i++)
          {
            sum = sum + Convert.ToDouble(tableArray[i].Rows[rowcount][1]);
          }
          average = sum / (round - 1);
          rowcount++;
          SR.WriteLine("{0,10}\t{1,10}", row.Field<string>(0), average.ToString("0"));
          lastw = average.ToString("0");
          lastT = row.Field<string>(0);
        }
      }
      catch (Exception)
      {
        Debug.WriteLine("ERROR IN LOOPONG THROUGH tableArray");
      }


      //

      SR.WriteLine("");
      SR.WriteLine("***************************");
      SR.WriteLine("");
      //
      SR.WriteLine("Total Weight = " + lastw + " g");
      SR.WriteLine("Total Time = " + lastT + " Secs");
      //SR.WriteLine("Flow Velocity = " + (weight / (Convert.ToDouble(settings.area) * time)).ToString("#0.000") + " cm/s");
      vel = (Convert.ToDouble(lastw) / (Convert.ToDouble(settings.area) * Convert.ToDouble(lastT))).ToString("#0.000");
      SR.WriteLine("Flow Velocity = " + vel + " cm/s");
      SR.WriteLine(""); 
      SR.WriteLine("***************************");
      SR.WriteLine("");
      CommonClass.addVelToLast5(vel);
      //
      if (settings.maxRepetitions > 1)
      {
        for (int i = 0; i < round - 1; i++)
        {
          SR.WriteLine("Round " + ((i + 1).ToString()));
          foreach (DataRow row in tableArray[i].Rows)
          {
            SR.WriteLine("{0,10}\t{1,10}", row.Field<string>(0), row.Field<string>(1));
          }
          SR.WriteLine("");
        }
        SR.WriteLine("***************************");
        SR.WriteLine("");
        int k = 0;
        foreach (DataRow row in finalWeights.Rows)
        {
          k++;
          SR.WriteLine("{0,10}\t{1,10}", "Weight Round " + k.ToString(), row.Field<string>(0));
        }
        SR.WriteLine("{0,10}\t{1,10}", "Average Weight ", average.ToString("0"));
      }
      SR.Close();
    }

    public void Output(string s)
    {
      System.Diagnostics.Debug.WriteLine(s);
    }

    private void timerReadScale_Tick(object sender, EventArgs e)
    {
    }

    private void addDataPoint()
    {
      if (newround)
      {
        chart1.Series["Series1"].Points.Clear();
        newround = false;
        originalWeight = Convert.ToDouble(lastValidScaleReading);
        netWeight();
      }
      chart1.Series["Series1"].Points.AddXY(String.Format("{0:#0}", timeInSeconds), currentNetWeightString);
      /*try
      {
        SR = new StreamWriter(filePath, true);
        //SR.WriteLine(String.Format("{0:#0.00}", timeInSeconds) + "\t" + currentNetWeightString);
        SR.WriteLine("{0,10}\t{1,10}", timeInSeconds.ToString("#0.00"), currentNetWeightString);
        lastTimeInSeconds = timeInSeconds;
        SR.Close();
      }
      catch (Exception)
      {
      }*/
      lastTimeInSeconds = timeInSeconds;
      tableArray[round - 1].Rows.Add(timeInSeconds.ToString("#0"), currentNetWeightString);
      lastWeight = currentNetWeightString;
    }

    private void netWeight()
    {
      currentNetWeight = 1000 * (Convert.ToDouble(lastValidScaleReading) - originalWeight);  //use this to remove original weight
      currentNetWeightString = currentNetWeight.ToString("0");
      labelScale.Text = "Weight = " + currentNetWeightString + " g";
    }

    private void buttonStopTimer_Click(object sender, EventArgs e)
    {
      testCancelledByUser = true;
      stopTest();
    }

    private void stopTest()
    {
      stopProc();

      //if (settings.checkBoxSaveData) //don't show report if data hasn't been saved
      //{
      Thread.Sleep(300);//if I don't sleep here the report might not catch the last data point.
      Enabled = false;

      if (!testCancelledByUser) //we didn't save data properly if it was cancelled so we can't make a report
      {
        Report setScrn = new Report();
        setScrn.ShowDialog();
      }

      //}
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
      testStopped = true; //this needed because bgworker doesnt end
      if (!stoppedTestWithoutReport)
      {
        if (!testCancelledByUser) //we didn't save data properly if it was cancelled so we can't make a report
        {
          SaveDataTableToFile();
          calcPerm();
        }

      }
      timerReadScale.Stop();
      timerTimeLabel.Stop();
      stopwatch.Stop();
      backgroundWorkerElapsedTime.CancelAsync();
      backgroundWorkerReadScale.CancelAsync();
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

        SR.Close();
      }
      catch (Exception)
      {
      }
    }

    private void writeHeader()
    {
      filePath = settings.dataPath;
      SR = new StreamWriter(filePath);
      SR.WriteLine("PMI Liquid Permeability Test");
      SR.WriteLine("Test Date = " + DateTime.Now);
      SR.WriteLine("Sample Name = " + Path.GetFileNameWithoutExtension(filePath));
      SR.WriteLine("Liquid Column Head Difference = " + settings.height + " mm");
      //SR.WriteLine("Sample Thickness (cm) = " + settings.sampleThickness);
      SR.WriteLine("Sample Diameter = " + settings.diameter + " cm");
      // SR.WriteLine("Liquid Viscosity (cP) = " + settings.viscosity);
      SR.WriteLine("Sample Area = " + settings.area + " cm^2");
      SR.WriteLine("");
      SR.WriteLine("***************************");

      if (settings.maxRepetitions>1)
      {
        SR.WriteLine("Averaged data from " + (round - 1) + " rounds.");
        SR.WriteLine("");
        SR.WriteLine("Averaged Data:");
      }

      SR.WriteLine("");
      SR.WriteLine("Time (Seconds)" + "\t" + "Weight (Grams)");
      SR.WriteLine("");
      SR.Close();
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
        Thread.Sleep(50);
      }
    }

    private void backgroundWorkerElapsedTime_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      timerTimeLabel.Stop();
      stopTest();
    }


    private void labelScale_Click(object sender, EventArgs e)
    {

    }

    private void labelTime_Click(object sender, EventArgs e)
    {

    }

    private void FormAuto_FormClosed(object sender, FormClosedEventArgs e)
    {
      timerReadScale.Stop();
      /*while (commBusy)
      {
      }*/
      COMMS.serialPort1.Close();
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


        scaleReading = COMMS.ReadScaleAWS();
        if (scaleReading.Length > 4)
        {
          //lastValidScaleReading = filterReading(scaleReading);
          lastValidScaleReading = CommonClass.filterRegex(scaleReading);
          backgroundWorkerReadScale.ReportProgress(0);
        }

        if (testStopped)
        {
          Debug.WriteLine("testStopped hit");
          Debug.WriteLine("testStopped hit");
          Debug.WriteLine("testStopped hit");
          return;
        }


        if ((backgroundWorkerReadScale.CancellationPending == true))
        {
          Debug.WriteLine("e.Cancel ; hit");
          Debug.WriteLine("e.Cancel ; hit");
          Debug.WriteLine("e.Cancel ; hit");
          e.Cancel = true;
          //break;
          return;
        }


        Thread.Sleep(150);
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
          if (splitReceivedB[0].Contains("-"))//sets it to zero if it is negative
          {
            splitReceivedB[0] = "0.00";
          }
          else
          {
            splitReceivedB = splitReceived[2].Split(new string[] { "kg" }, StringSplitOptions.None);
          }
        }
      }
      else
      {
        splitReceivedB = splitReceived[2].Split(new string[] { "kg" }, StringSplitOptions.None);
      }

      filtered = splitReceivedB[0];
      return filtered;
    }

    private void backgroundWorkerReadScale_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      //now moved over to timer
      /*labelRound.Text = round.ToString();
      netWeight();
      if (!testStopped)
      {
        addDataPoint();
      }
      if (currentNetWeight > settings.maxWeight)
      {
        if (!testStopped)
        {
          testStopped = true;
          stopTest();
          MessageBox.Show("The test was stopped because the maximum weight was reached");
        }
      }*/
    }

    private void FormAuto_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!COMMS.cancelling)
      {
        stopTestWithoutReport();
      }
    }

    private void timerTimeLabel_Tick(object sender, EventArgs e)
    {
      timeInSeconds++;
      timeSpan = TimeSpan.FromSeconds(timeInSeconds);
      timeString = String.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
      labelTime.Text = "Time = " + timeString;

      labelRound.Text = "Round " + round.ToString();
      netWeight();
      if (!testStopped)
      {
        addDataPoint();
      }
      if (currentNetWeight > settings.maxWeight)
      {
        if (!testStopped)
        {
          //testStopped = true;
          //stopTest();
          MessageBox.Show("The maximum weight has been reached!! Drain the containers. (You can change this number in the settings page)");
        }
      }

      //checkIfWeShouldStopTheTest();

      if (round > settings.maxRepetitions)
      {
        stopTest();
      }

      if (timeSpan >= spanMaxTime)
      {

        if (lastRound) //if lastRound then stop test and show report, else restart new round
        {
          testStopped = true;
          backgroundWorkerElapsedTime.ReportProgress(0);
          return;
        }
        else
        {
          round++;
          labelLast.Text = "Last Weight: " + lastWeight;

          finalWeights.Rows.Add(lastWeight);
          timeInSeconds = -1;
          //stopwatch.Restart();        
          newround = true;
        }
        checkIfWeShouldStopTheTest();

      }
    }

    private void checkIfWeShouldStopTheTest()
    {
      if (round > 2)
      {
        string lastRow = finalWeights.Rows[finalWeights.Rows.Count - 1].Field<string>(0);
        string previousToLastRow = finalWeights.Rows[finalWeights.Rows.Count - 2].Field<string>(0);
        double sum = 0;
        //
        foreach (DataRow row in finalWeights.Rows)
        {
          sum = sum + Convert.ToDouble(row.Field<string>(0));
        }
        average = sum / (round - 1);
        labelAverage.Text = "Average Weight: " + average.ToString("0");


        //
        /*
        if ((lastRow == previousToLastRow))
        {
          stopTest();
        }*/

        if ((Convert.ToDouble(lastRow) < (average + settings.stabilizeWithin)) && (Convert.ToDouble(lastRow) > (average - settings.stabilizeWithin)))
        {
          stopTest();
        }
      }
    }
  }
}
