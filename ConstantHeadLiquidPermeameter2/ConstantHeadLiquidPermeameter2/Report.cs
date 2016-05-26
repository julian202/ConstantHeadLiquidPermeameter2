
using MathNet.Numerics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ConstantHeadLiquidPermeameter2
{
  public partial class Report : Form
  {
    Properties.Settings settings = Properties.Settings.Default;
    //array list to store header data to, all data from each file will be stored as a CSV in single array item.
    ArrayList peep = new ArrayList();
    ArrayList distensionML = new ArrayList();
    ArrayList distensionCM = new ArrayList();
    string[] splitString = new string[2];
    bool openSaved;
    string lastPUnit = "";
    int myBorderWidth = Convert.ToInt32(Properties.Settings.Default.myBorderWidth);
    private double weight = 0;
    private double time = 0;
    private double previousWeight = 0;
    private double previousTime = 0;
    private bool endReading = false;

    public Report()
    {
      InitializeComponent();
      openSaved = false;

    }
    public Report(bool openSavedParam)
    {
      InitializeComponent();
      openSaved = openSavedParam;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      OpenDataFiles(false);
      showAll();
    }

    public void OpenDataFiles(bool knowName)
    {

      string[] selectedFiles;
      if (!knowName)
      {
        openFileDialog1.ShowDialog();
        selectedFiles = openFileDialog1.FileNames;

        //
        Properties.Settings.Default.dataPath = selectedFiles[0];
        //

        if (selectedFiles.Length > 1 || comboBox1.Items.Count > 0)
        {
          if (comboBox1.FindStringExact("Show All") == -1)
          {
            comboBox1.Items.Insert(0, "Show All");
          }
        }
      }
      else
      {
        selectedFiles = new string[1];
        selectedFiles[0] = Properties.Settings.Default.dataPath;
        //MessageBox.Show(Properties.Settings.Default.TestData);
      }

      foreach (string s in selectedFiles)
      {
        string sampleInfoCSV = null;
        string fileName;
        fileName = Path.GetFileNameWithoutExtension(s);
        if (comboBox1.FindStringExact(fileName) != -1)
        {
          //if item is already listed
          //MessageBox.Show("A file with the same name is already open. This file will be skipped.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
          continue;
        }
        StreamReader SR = new StreamReader(s);
        string RL = SR.ReadLine();
        if (RL != "PMI Liquid Permeability Test")
        {
          MessageBox.Show(s + " is not a liquid permeability data file!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
          continue;
        }
        string[] splitStuff;
        RL = SR.ReadLine(); //"date"
        splitStuff = RL.Split('=');
        string date = splitStuff[1];
        sampleInfoCSV += date + ",";
        RL = SR.ReadLine(); //"sample name"
        splitStuff = RL.Split('=');
        string sampleID = splitStuff[1];
        sampleInfoCSV += sampleID + ",";

        /*
        RL = SR.ReadLine();//Sample Thickness (cm)
        splitStuff = RL.Split('=');
        string Thickness = splitStuff[1];*/
        string Thickness = "0";
        sampleInfoCSV += Thickness + ",";

        RL = SR.ReadLine();//Liquid Height (cm)
        splitStuff = RL.Split('=');
        string Height = splitStuff[1];
        sampleInfoCSV += Height + ",";

        RL = SR.ReadLine();//Sample Diameter (cm)
        splitStuff = RL.Split('=');
        string Diameter = splitStuff[1];
        sampleInfoCSV += Diameter + ",";

        RL = SR.ReadLine();//Sample Area (cm^2)
        splitStuff = RL.Split('=');
        string Area = splitStuff[1];
        sampleInfoCSV += Area + ",";

        /*
        RL = SR.ReadLine();//Liquid Viscosity (cP)
        splitStuff = RL.Split('=');
        string Viscosity = splitStuff[1];*/
        string Viscosity = "0";
        sampleInfoCSV += Viscosity + ",";

        /*
        RL = SR.ReadLine();
        RL = SR.ReadLine();
        RL = SR.ReadLine();
        RL = SR.ReadLine();
        RL = SR.ReadLine();
        RL = SR.ReadLine(); //"column titles"    
        RL = SR.ReadLine(); //""
        */

        while (!SR.ReadLine().Contains("Time"))
        {

        }


        //setup data table for current sample
        dataSet1.Tables.Add(fileName);
        dataSet1.Tables[fileName].Columns.Add("Time");
        dataSet1.Tables[fileName].Columns.Add("Weight");

        //add sample name to combobox
        comboBox1.Items.Add(fileName);

        //while (!SR.EndOfStream)
        while (!SR.EndOfStream && !endReading)
        {
          //read/split each line and convert to double type.
          RL = SR.ReadLine();
          if (RL.Contains("Round"))
          {
            endReading = true;
          }
          if (RL.Contains('\t') && (!RL.Contains("Perm")) && (!endReading))
          {
            splitString = RL.Split('\t');
            try
            {

              time = Convert.ToDouble(splitString[0]);
              weight = Convert.ToDouble(splitString[1]);
              

              //add doubles to data table for current sample
              dataSet1.Tables[fileName].Rows.Add(time, weight);
            }
            catch (FormatException ex)
            {
              MessageBox.Show(s + " is a valid data file, however the program has encountered an error while reading the data!" + Environment.NewLine + ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
              //go to the end of file so while loop will end
              SR.ReadToEnd();
              //delete this table
              //dataSet1.Tables[sampleID].Dispose();
              dataSet1.Tables.Remove(fileName);
              //remove sample name from combobox
              comboBox1.Items.RemoveAt(comboBox1.FindStringExact(fileName));
              //peep.RemoveAt(peep.Count - 1);
              //return to while that should end; (endofstream) should be reached.
              continue;
            }
          }
          if (SR.EndOfStream || endReading)
          {
            //MessageBox.Show(RL);

            /*
            splitString = RL.Split('\t');
            string permeability = splitString[1];*/
            string permeability = "0";

            sampleInfoCSV += permeability + ",";
            labelPerm.Text = "Permeability: " + permeability + " Darcies";
            labelTotalWeight.Text = "Total Weight:  " + previousWeight + " g";
            labelTotalTime.Text = "Total Time:    " + previousTime + " Secs";
            labelSampleArea.Text = "Sample Area:  " + settings.area + " cm^2";
            string flow = string.Format("{0:0.00}", (previousWeight * 60 / previousTime));
            labelFlow.Text = "Total Flow:   " + flow + " ML/Min";  //1g of water = 1ml
            labelVelocity.Text = "Flow Velocity: " + (previousWeight / (Convert.ToDouble(settings.area) * time)).ToString("#0.000") + " cm/s";

            dataSet1.Tables[fileName].Rows.Add("permeability", permeability);
            dataSet1.Tables[fileName].Rows.Add("weight", previousWeight);
            dataSet1.Tables[fileName].Rows.Add("time", previousTime);
            dataSet1.Tables[fileName].Rows.Add("flow", flow);
            
          }
          previousWeight = weight;
          previousTime = time;
        }
        endReading = false;
        //sampleInfoCSV += "," + fileName;
        sampleInfoCSV += fileName;
        peep.Add(sampleInfoCSV);
        SR.Close();
      }

      //show the first item, what ever it is.
      if (comboBox1.Items.Count == 1)
      {
        comboBox1.SelectedIndex = 0;
      }
      else
      {
        comboBox1.SelectedIndex = comboBox1.FindStringExact("Show All");
      }
    }

    private void readData()
    {
    }

    private void Report_Load(object sender, EventArgs e)
    {
      loadPreviousGridValues();


      dataGridViewExample.Rows.Add("20", "-", "-");
      dataGridViewExample.Rows.Add("30", "-", "-");
      dataGridViewExample.Rows.Add("40", "-", "-");
      dataGridViewExample.Rows.Add("50", "-", "-");
      dataGridViewExample.Rows.Add("60", "-", "-");

      textBoxTime.Text = (settings.timeInMinutes).ToString("#0.00");
      textBoxWeight.Text = settings.netWeight;
      textBoxHeight.Text = settings.height;
      textBoxViscosity.Text = settings.viscosity;
      textBoxDiameter.Text = settings.diameter;
      textBoxThickness.Text = settings.sampleThickness;

      if (openSaved)
      {
        OpenDataFiles(true);
      }
      //open last data:
      try
      {
        OpenDataFiles(true);
      }
      catch (Exception)
      {
      }
    }

    private void loadPreviousGridValues()
    {
      dataGridViewInterpolation.Rows.Clear();
      /*
      for (int row = 0; row < settings.CollectionH.Count; row++)
      {
        dataGridViewInterpolation.Rows.Add(settings.CollectionH[row], settings.CollectionV[row]);
      }*/
      dataGridViewInterpolation.Rows.Add(settings.last5H5, settings.last5vel5);
      dataGridViewInterpolation.Rows.Add(settings.last5H4, settings.last5vel4);
      dataGridViewInterpolation.Rows.Add(settings.last5H3, settings.last5vel3);
      dataGridViewInterpolation.Rows.Add(settings.last5H2, settings.last5vel2);
      dataGridViewInterpolation.Rows.Add(settings.last5H1, settings.last5vel1);
      dataGridViewInterpolation.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      dataGridViewInterpolation.ClearSelection();

    }

    private void showAll()
    {
      //clear current series
      chart1.Series.Clear();
      //loop though each data file
      foreach (string s in comboBox1.Items)
      {
        //skip 'Show All' because.
        if (s == "Show All") continue;
        //add a series named for the sample id
        chart1.Series.Add(s);
        //set chart type
        chart1.Series[s].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
        chart1.Series[s].BorderWidth = myBorderWidth;


        //loop through the data table named for the sample id and add it to the series.
        foreach (DataRow asdf in dataSet1.Tables[s].Rows)
        {
          try
          {
            double test = Convert.ToDouble(asdf[0]);//will not go past this if it's not a number
            chart1.Series[s].Points.AddXY(Convert.ToDouble(asdf[0]), Convert.ToDouble(asdf[1]));
          }
          catch (Exception)
          {
          }
        }
      }
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      //clear legends
      chart1.Legends.Clear();
      //if they want to see all files graphed together
      if (comboBox1.Text == "Show All")
      {
        showAll();
      }
      else
      {
        //show single selected data set in chart1
        chart1.Series.Clear();
        chart1.Series.Add(comboBox1.Text);
        chart1.Series[comboBox1.Text].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
        chart1.Series[comboBox1.Text].BorderWidth = myBorderWidth;
        foreach (DataRow asdf in dataSet1.Tables[comboBox1.Text].Rows)
        {
          try
          {
            COMMS.Output(asdf[0].ToString() + ", " + asdf[1].ToString());
            if (asdf[0] is string)
            {
              if (asdf[0].ToString() == "permeability")
              {
                labelPerm.Text = "Permeability: " + asdf[1] + " Darcies";
              }
              if (asdf[0].ToString() == "weight")
              {
                labelTotalWeight.Text = "Total Weight:  " + asdf[1] + " g";
              }
              if (asdf[0].ToString() == "time")
              {
                labelTotalTime.Text = "Total Time:    " + asdf[1] + " Secs";
              }
              if (asdf[0].ToString() == "flow")
              {
                labelFlow.Text = "Total Flow:   " + asdf[1] + " ML/Min";
              }
            }
            double testForNumeric = Convert.ToDouble(asdf[0]);//will not go past this if it's not a number
            chart1.Series[comboBox1.Text].Points.AddXY(Convert.ToDouble(asdf[0]), Convert.ToDouble(asdf[1]));
          }
          catch (Exception)
          {
          }
        }

      }
      //add a legend and axis titles.
      chart1.Legends.Add("s");

      chart1.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Arial", 12F);
      chart1.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Arial", 12F);
      chart1.ChartAreas[0].AxisY.Title = "Weight (Grams)";
      chart1.ChartAreas[0].AxisX.Title = "Time (Seconds)";

    }

    private double GetBurstRatio(double grammage, double burstPressure)
    {
      //throw new NotImplementedException();
      double burstRatio = burstPressure / grammage;
      return burstRatio;
    }

    private void button2_Click(object sender, EventArgs e)
    {
      //remove data set, combo item, array item.
      if (comboBox1.Items.Count > 0)
      {
        if (comboBox1.Text != "Show All")
        {
          if (comboBox1.Items.Count == 1)
          {
            dataSet1.Tables.Clear();
            peep.Clear();
            lastPUnit = "";
            comboBox1.Items.Clear();
            chart1.Series.Clear();
            lastPUnit = null;
          }
          else
          {
            int selectedItem = comboBox1.FindStringExact(comboBox1.Text);
            peep.RemoveAt(selectedItem - 1);
            dataSet1.Tables.RemoveAt(selectedItem - 1);
            comboBox1.Items.RemoveAt(selectedItem);
            comboBox1.SelectedIndex = selectedItem - 1;
          }
        }

        if (comboBox1.FindStringExact("Show All") != -1 && comboBox1.Items.Count == 2)
        {
          comboBox1.Items.RemoveAt(comboBox1.FindStringExact("Show All"));
          comboBox1.SelectedIndex = 0;
        }
      }
    }

    private void button3_Click(object sender, EventArgs e)
    {
      //kill everything.
      dataSet1.Tables.Clear();
      peep.Clear();
      lastPUnit = "";
      comboBox1.Items.Clear();
      chart1.Series.Clear();
      lastPUnit = null;
    }

    private void button4_Click(object sender, EventArgs e)
    {
      MessageBox.Show(lastPUnit);
    }

    private void buttonExport_Click(object sender, EventArgs e)
    { }

    private void ExportExcelFile(string headerInfo, int multi, string path)
    {
      //parse header data
      string[] splitStuff = headerInfo.Split(',');

      int row = 0;
      //create an excel file object
      var excel = new OfficeOpenXml.ExcelPackage();
      //create excel worksheet for text data
      var ws = excel.Workbook.Worksheets.Add(splitStuff[1]);
      //create excel worksheet for graph
      var gs = excel.Workbook.Worksheets.Add(splitStuff[1] + " graph");
      //output header stuff

      StreamReader SR = new StreamReader(Properties.Settings.Default.dataPath);
      string RL = "";
      int i = 0;
      int timeRow = 1;
      while (!SR.EndOfStream)
      {
        i++;
        RL = SR.ReadLine();
        if (RL.Contains("="))
        {
          string[] a = RL.Split('=');
          ws.Cells[i, 1].Value = a[0];
          ws.Cells[i, 2].Value = a[1];
        }
        else if (RL.Contains("\t"))
        {
          if (RL.Contains("Time"))
          {
            //then this must be the time row
            timeRow = i + 2;
            string[] a = RL.Split('\t');
            ws.Cells[i, 1].Value = a[0];
            ws.Cells[i, 2].Value = a[1];
          }
          else
          {
            //this must be the data
            string[] a = RL.Split('\t');
            try
            {
              ws.Cells[i, 1].Value = Convert.ToDouble(a[0]);
              ws.Cells[i, 2].Value = Convert.ToDouble(a[1]);
            }
            catch (Exception)
            {
              ws.Cells[i, 1].Value = a[0];
              ws.Cells[i, 2].Value = Convert.ToDouble(a[1]);
            }
            
          }
        }
        else
        {
          ws.Cells[i, 1].Value = RL;
        }
      }
      row = i;
      string dataname = Path.GetFileNameWithoutExtension(Properties.Settings.Default.dataPath);
      SR.Close();

      /*
            ws.Cells[1, 1].Value = "PMI Liquid Permeability Test";
            ws.Cells[2, 1].Value = "Test Date =";
            ws.Cells[2, 2].Value = splitStuff[0];
            ws.Cells[3, 1].Value = "Sample =";
            ws.Cells[3, 2].Value = splitStuff[1];
            ws.Cells[4, 1].Value = "Sample Thickness (cm) =";
            ws.Cells[4, 2].Value = splitStuff[2];
            ws.Cells[5, 1].Value = "Sample Diameter(cm) =";
            ws.Cells[5, 2].Value = splitStuff[3];
            ws.Cells[6, 1].Value = "Liquid Viscosity (cP) =";
            ws.Cells[6, 2].Value = splitStuff[4];
            ws.Cells[7, 1].Value = "Liquid Height (cm) =";
            ws.Cells[7, 2].Value = splitStuff[5];
            ws.Cells[8, 1].Value = "Calculated Permeability (Darcies) =";
            ws.Cells[8, 2].Value = splitStuff[6];
            ws.Cells[9, 1].Value = " ";
            ws.Cells[10, 1].Value = "Time (Minutes)";
            ws.Cells[10, 2].Value = "Weight (Grams)";
            row = 11;

            //output test data
            string dataname = splitStuff[7];
            foreach (DataRow asdf in dataSet1.Tables[dataname].Rows)
            {
              row++;
              ws.Cells[row, 1].Value = Convert.ToDouble(asdf[0]);
              ws.Cells[row, 2].Value = Convert.ToDouble(asdf[1]);
            }*/

      //output graph
      OfficeOpenXml.ExcelRange r1, r2;
      var chart = (OfficeOpenXml.Drawing.Chart.ExcelLineChart)gs.Drawings.AddChart("some_name", OfficeOpenXml.Drawing.Chart.eChartType.Line);
      //chart.Legend.Position = OfficeOpenXml.Drawing.Chart.eLegendPosition.Right;
      //chart.Legend.Add();
      //chart.SetPosition(17, 0, 0, 0);
      //chart.SetSize(500, 300);

      try
      {
        r1 = ws.Cells["A" + timeRow.ToString() + ":A" + row.ToString()];
        r2 = ws.Cells["B" + timeRow.ToString() + ":B" + row.ToString()];
        chart.Series.Add(r2, r1);
      }
      catch (ArgumentOutOfRangeException ex)
      {
        MessageBox.Show("Argument our of range!" + Environment.NewLine + ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      chart.Title.Text = "Weight VS Time";

      //chart.YAxis.Title.Text = "Differential Pressure(" + splitStuff[3] + ")";
      chart.YAxis.Title.Text = "Weight (Grams)";
      chart.XAxis.Title.Text = "Time (Secs)";
      ws.Cells[ws.Dimension.Address.ToString()].AutoFitColumns();
      saveFileDialog1.InitialDirectory = Properties.Settings.Default.ExportPath;
      saveFileDialog1.FileName = splitStuff[0];
      if (multi == 0)
      {
        if (Properties.Settings.Default.AutoDataFile)
        {
          using (var file = File.Create(Properties.Settings.Default.ExportPath + @"\" + dataname + ".xlsx"))
            excel.SaveAs(file);
          MessageBox.Show(Properties.Settings.Default.ExportPath + @"\" + dataname + ".xlsx" + " has been saved.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
          saveFileDialog1.FileName = dataname;
          DialogResult saveFile = saveFileDialog1.ShowDialog();
          if (saveFile != DialogResult.Cancel)
          {
            try
            {
              using (var file = File.Create(saveFileDialog1.FileName))
                excel.SaveAs(file);
              MessageBox.Show(saveFileDialog1.FileName + " has been saved.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
              MessageBox.Show(ex.Message + " You probably need to close Excel");
            }
          }
        }
      }
      if (multi == 1)
      {
        try
        {
          using (var file = File.Create(path + @"\" + dataname + ".xlsx"))
            excel.SaveAs(file);
        }
        catch (Exception)
        {
          MessageBox.Show("Choose a different directory. Access to this directory is not allowed");
          return;
        }

      }

    }
    private void button5_Click(object sender, EventArgs e)
    {
      foreach (DataTable asdf in dataSet1.Tables)
      {
        chart1.Series.Add(asdf.TableName);
        chart1.Series[asdf.TableName].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
        chart1.Series[comboBox1.Text].BorderWidth = myBorderWidth;
        chart1.Series[asdf.TableName].Points.DataBindXY(asdf.Columns["Time"].ToString(), asdf.Columns["Pressure"].ToString());
      }
    }

    private void button5_Click_1(object sender, EventArgs e)
    {
      MessageBox.Show(comboBox1.FindStringExact("PIE").ToString());
    }

    private void Report_Resize(object sender, EventArgs e)
    {
      //resize and reposition things when window resizes.
      if (WindowState != FormWindowState.Minimized)
      {
        chart1.Size = new System.Drawing.Size(this.Width - 306, this.Height - 120);
        groupBox1.Location = new System.Drawing.Point(chart1.Width + 40, chart1.Top);
        groupBox2.Location = new System.Drawing.Point(chart1.Width + 40, chart1.Top + 115);
      }
    }

    private void buttonCalculate_Click(object sender, EventArgs e)
    {
      double perm;
      double timeInSecs = Convert.ToDouble(textBoxTime.Text) * 60;
      double flow = Convert.ToDouble(textBoxWeight.Text) / Convert.ToDouble(timeInSecs);
      label9.Text = "Flow (mL/sec) = " + flow.ToString("#0.00");

      double thickness = Convert.ToDouble(textBoxThickness.Text);
      double viscosity = Convert.ToDouble(textBoxViscosity.Text);
      double k1 = flow * thickness * viscosity * 14.7; //the 14.7 is a conversion factor, NOT atmospheric pressure! It lumps together the conversion from centipoise to poise for viscosity, the conversion from PSI to dynes/cm^2, and the conversion from cm^2 to Darcies.
      double area = 3.1415926 * (Convert.ToDouble(textBoxDiameter.Text) * Convert.ToDouble(textBoxDiameter.Text)) / 4;
      label17.Text = "Area = Pi * ((Diameter) ^ 2) / 4 = " + area.ToString("#0.00");
      //perm = k1  / (60 * area * Convert.ToDouble(textBoxPressure.Text)); //if time were in minutes
      double pressure = 0.014 * Convert.ToDouble(textBoxHeight.Text);
      label6.Text = "Pressure = 0.014 * Height = " + pressure + " PSI";

      perm = k1 / (area * pressure); //1m =1.4PSI => 1cm=0.014PSI
      labelPermeability.Text = "= " + perm.ToString("#0.0000000") + " Darcies";
    }

    private void linkLabelOpenFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      System.Diagnostics.Process.Start("explorer.exe", System.IO.Directory.GetParent(Properties.Settings.Default.dataPath).ToString());
    }

    private void button5_Click_2(object sender, EventArgs e)
    {
      Close();
    }

    private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      System.Diagnostics.Process.Start("Notepad.exe", (Properties.Settings.Default.dataPath).ToString());

    }

    private void button4_Click_1(object sender, EventArgs e)
    {
      
      //requires MathNet
      double[] xdata = new double[] { 10, 20, 30, 40 };
      double[] ydata = new double[] { 18, 20, 25, 45 };

      List<double> xdataList = new List<double>(xdata);
      List<double> ydataList = new List<double>(ydata);

      //xdataList.Insert(0,0);
      //ydataList.Insert(0,0);

      xdata = xdataList.ToArray();
      ydata = ydataList.ToArray();

      
      double[] p = Fit.Polynomial(xdata, ydata, 2);


      chart2.Series.Add("j");
      //set chart type
      chart2.Series["j"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
      chart2.Series["j"].BorderWidth = myBorderWidth;
      chart2.Series["j"].Color = System.Drawing.Color.Green;

      chart2.Series.Add("k");
      //set chart type
      chart2.Series["k"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
      chart2.Series["k"].BorderWidth = myBorderWidth;
      chart2.Series["k"].Color = System.Drawing.Color.Blue;

      for (int i = 0; i < xdata.Length; i++)
      {
        chart2.Series["j"].Points.AddXY(Convert.ToDouble(xdata[i]), Convert.ToDouble(ydata[i]));
        
      }
      for (int i = 0; i < 50; i++)
      {

        double newx = ((xdata[xdata.Length - 1] - xdata[0]) / 50)*i + xdata[0];


        double newy = p[0] + p[1] * newx + p[2] * Math.Pow(newx, 2);
        chart2.Series["k"].Points.AddXY(newx, newy);
      }
    }

    private void buttonAddToGrid_Click(object sender, EventArgs e)
    {
      dataGridViewInterpolation.Rows.Add(textBox1.Text, textBox2.Text);
    }

    private void button7_Click(object sender, EventArgs e)
    {
      foreach (DataGridViewCell oneCell in dataGridViewInterpolation.SelectedCells)
      {
        if (oneCell.Selected)
          dataGridViewInterpolation.Rows.RemoveAt(oneCell.RowIndex);
      }
    }

    private void button4_Click_2(object sender, EventArgs e)
    {
      chart2.Series.Clear();

      List <double> xdataList = new List<double>();
      List<double> ydataList = new List<double>();
      /*xdataList.Add(0);
      ydataList.Add(0);*/
      DataGridViewRowCollection drc = dataGridViewInterpolation.Rows;
      settings.CollectionH.Clear();
      settings.CollectionV.Clear();
      foreach (DataGridViewRow item in drc)
      {
       try
        {
          //save to settings
          /*
          settings.CollectionH.Add(item.Cells[0].Value.ToString());
          settings.CollectionV.Add(item.Cells[1].Value.ToString());
          settings.Save();*/

          //add to ArrayList
          xdataList.Add(Convert.ToDouble(item.Cells[1].Value));
          ydataList.Add(Convert.ToDouble(item.Cells[0].Value));
        }
        catch (Exception ex)
        {
          MessageBox.Show("Please introduce only numbers. " + ex.Message);
        }       
      }
      xdataList.Add(0);
      ydataList.Add(0);
      double[]  xdata = xdataList.ToArray();
      double[]  ydata = ydataList.ToArray();

      double[] p = Fit.Polynomial(xdata, ydata, 2);

      chart2.Series.Add("Data");
      chart2.Series.Add("Interpolation");
      //set chart type
      chart2.Series["Data"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
      chart2.Series["Data"].BorderWidth = myBorderWidth;
      //chart2.Series["Data"].LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
      chart2.Series["Data"].MarkerStyle = MarkerStyle.Circle;
      chart2.Series["Data"].MarkerSize = 10;
      chart2.Series["Data"].Color = System.Drawing.Color.Green;
      
      //set chart type
      chart2.Series["Interpolation"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
      chart2.Series["Interpolation"].BorderWidth = myBorderWidth;
      chart2.Series["Interpolation"].BorderDashStyle = ChartDashStyle.Dot;
      chart2.Series["Interpolation"].Color = System.Drawing.Color.Blue;

      for (int i = 0; i < xdata.Length; i++)
      {
        chart2.Series["Data"].Points.AddXY(Convert.ToDouble(xdata[i]), Convert.ToDouble(ydata[i]));
      }
      for (int i = 0; i < 50; i++)
      {
        double newx = ((xdata[xdata.Length - 1] - xdata[0]) / 50) * i + xdata[0];
        double newy = p[0] + p[1] * newx + p[2] * Math.Pow(newx, 2);
        chart2.Series["Interpolation"].Points.AddXY(newx, newy);
      }

      //calculate v at head of 50mm (solve quadratic equation at 50mm):
      double sqrtpart = p[1] * p[1] - 4 * p[2] * (p[0] - 50);
      textBoxAnswer.Text= ((-p[1] + Math.Sqrt(sqrtpart)) / (2 * p[2])).ToString("#0.000");
      textBox3.Text = ((-p[1] - Math.Sqrt(sqrtpart)) / (2 * p[2])).ToString("#0.000");

      double answer = (-p[1] + Math.Sqrt(sqrtpart)) / (2 * (p[2]));
      textBox4.Text = (p[0] + p[1] * answer + p[2] * Math.Pow(answer, 2)).ToString("#0.000");
    }

    private void button6_Click(object sender, EventArgs e)
    {

      Regex regex = new Regex(@"\d{4}[.]\d{2}");
      Match match = regex.Match(textBox5.Text);
      if (match.Success)
      {
        if (textBox5.Text.Contains("-"))
        {
          textBox6.Text = "-"+match.Value;
        }
        else
        {
          textBox6.Text = match.Value;//
        }
        
      }
      else
      {
        textBox6.Text = "no match";
      }


    }

    private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      if (comboBox1.Items.Count > 0)
      {
        export exp = new  export();
        DialogResult expResult = exp.ShowDialog();
        if (expResult == DialogResult.OK)
        {
          if (Properties.Settings.Default.ExportOption == 0)
          {
            if (comboBox1.Text == "Show All")
            {
              MessageBox.Show("To export a single data set, you must select a single data set to export.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
              return;
            }

            //Properties.Settings.Default.ExportPath =

            //get correct header
            string header = null;
            if (comboBox1.Items.Count > 1)
            {
              header = peep[comboBox1.SelectedIndex - 1].ToString();
            }
            else
            {
              header = peep[comboBox1.SelectedIndex].ToString();
            }
            //string a = Properties.Settings.Default.ExportPath;
            ExportExcelFile(header, Properties.Settings.Default.ExportOption, Properties.Settings.Default.ExportPath);

          }
          if (Properties.Settings.Default.ExportOption == 1)
          {
            if (comboBox1.Items.Count == 1)
            {
              //selected export multiple, but only has one. Thanks, rocket surgeon.
              string header = peep[comboBox1.SelectedIndex].ToString();
              ExportExcelFile(header, Properties.Settings.Default.ExportOption, Properties.Settings.Default.ExportPath);
            }
            if (comboBox1.Items.Count > 1)
            {
              //normal
              foreach (string s in comboBox1.Items)
              {
                if (s == "Show All") continue;
                string header = peep[comboBox1.FindStringExact(s) - 1].ToString();
                ExportExcelFile(header, Properties.Settings.Default.ExportOption, Properties.Settings.Default.ExportPath);
              }
            }
            MessageBox.Show("All files exported successfully!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
          }
        }
      }
    }

    private void buttonDeleteAll_Click(object sender, EventArgs e)
    {
      settings.last5H1 = "0";
      settings.last5H2 = "0";
      settings.last5H3 = "0";
      settings.last5H4 = "0";
      settings.last5H5 = "0";
      settings.last5vel1 = "0";
      settings.last5vel2 = "0";
      settings.last5vel3 = "0";
      settings.last5vel4 = "0";
      settings.last5vel5 = "0";
      settings.Save();
      loadPreviousGridValues();
    }
  }
}
