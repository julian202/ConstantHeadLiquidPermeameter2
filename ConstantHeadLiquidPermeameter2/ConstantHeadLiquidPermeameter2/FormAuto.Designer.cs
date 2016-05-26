namespace ConstantHeadLiquidPermeameter2
{
  partial class FormAuto
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
      System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
      this.labelBoardDetected = new System.Windows.Forms.Label();
      this.labelScale = new System.Windows.Forms.Label();
      this.timerReadScale = new System.Windows.Forms.Timer(this.components);
      this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
      this.buttonStopTimer = new System.Windows.Forms.Button();
      this.labelTime = new System.Windows.Forms.Label();
      this.backgroundWorkerElapsedTime = new System.ComponentModel.BackgroundWorker();
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
      this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
      this.labelRunning = new System.Windows.Forms.Label();
      this.labelSavingFile = new System.Windows.Forms.Label();
      this.backgroundWorkerReadScale = new System.ComponentModel.BackgroundWorker();
      this.label1 = new System.Windows.Forms.Label();
      this.timerTimeLabel = new System.Windows.Forms.Timer(this.components);
      this.labelRound = new System.Windows.Forms.Label();
      this.labelLast = new System.Windows.Forms.Label();
      this.labelAverage = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
      this.statusStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // labelBoardDetected
      // 
      this.labelBoardDetected.AutoSize = true;
      this.labelBoardDetected.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
      this.labelBoardDetected.Location = new System.Drawing.Point(-1, 585);
      this.labelBoardDetected.Name = "labelBoardDetected";
      this.labelBoardDetected.Size = new System.Drawing.Size(217, 29);
      this.labelBoardDetected.TabIndex = 18;
      this.labelBoardDetected.Text = "Board detected on ";
      this.labelBoardDetected.Visible = false;
      // 
      // labelScale
      // 
      this.labelScale.AutoSize = true;
      this.labelScale.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelScale.ForeColor = System.Drawing.Color.Red;
      this.labelScale.Location = new System.Drawing.Point(192, 50);
      this.labelScale.Name = "labelScale";
      this.labelScale.Size = new System.Drawing.Size(199, 37);
      this.labelScale.TabIndex = 20;
      this.labelScale.Text = "Weight = 0 g";
      this.labelScale.Click += new System.EventHandler(this.labelScale_Click);
      // 
      // timerReadScale
      // 
      this.timerReadScale.Interval = 200;
      this.timerReadScale.Tick += new System.EventHandler(this.timerReadScale_Tick);
      // 
      // chart1
      // 
      chartArea2.AxisX.Title = "Time (Seconds)";
      chartArea2.AxisX.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 11F);
      chartArea2.AxisY.Title = "Weight (Grams)";
      chartArea2.AxisY.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 11F);
      chartArea2.Name = "ChartArea1";
      this.chart1.ChartAreas.Add(chartArea2);
      this.chart1.Location = new System.Drawing.Point(80, 104);
      this.chart1.Name = "chart1";
      series2.ChartArea = "ChartArea1";
      series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
      series2.Name = "Series1";
      this.chart1.Series.Add(series2);
      this.chart1.Size = new System.Drawing.Size(769, 400);
      this.chart1.TabIndex = 24;
      this.chart1.Text = "chart1";
      // 
      // buttonStopTimer
      // 
      this.buttonStopTimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.buttonStopTimer.Location = new System.Drawing.Point(572, 527);
      this.buttonStopTimer.Name = "buttonStopTimer";
      this.buttonStopTimer.Size = new System.Drawing.Size(219, 72);
      this.buttonStopTimer.TabIndex = 28;
      this.buttonStopTimer.Text = "Stop Test Now";
      this.buttonStopTimer.UseVisualStyleBackColor = true;
      this.buttonStopTimer.Click += new System.EventHandler(this.buttonStopTimer_Click);
      // 
      // labelTime
      // 
      this.labelTime.AutoSize = true;
      this.labelTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
      this.labelTime.Location = new System.Drawing.Point(462, 50);
      this.labelTime.MaximumSize = new System.Drawing.Size(450, 50);
      this.labelTime.Name = "labelTime";
      this.labelTime.Size = new System.Drawing.Size(187, 37);
      this.labelTime.TabIndex = 109;
      this.labelTime.Text = "Time: 00:00";
      this.labelTime.Click += new System.EventHandler(this.labelTime_Click);
      // 
      // backgroundWorkerElapsedTime
      // 
      this.backgroundWorkerElapsedTime.WorkerReportsProgress = true;
      this.backgroundWorkerElapsedTime.WorkerSupportsCancellation = true;
      this.backgroundWorkerElapsedTime.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerElapsedTime_DoWork);
      this.backgroundWorkerElapsedTime.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerElapsedTime_ProgressChanged);
      // 
      // statusStrip1
      // 
      this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
      this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
      this.statusStrip1.Location = new System.Drawing.Point(0, 620);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new System.Drawing.Size(958, 22);
      this.statusStrip1.SizingGrip = false;
      this.statusStrip1.TabIndex = 111;
      this.statusStrip1.Text = "statusStrip1";
      // 
      // toolStripStatusLabel1
      // 
      this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
      this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
      // 
      // saveFileDialog1
      // 
      this.saveFileDialog1.DefaultExt = "pmi";
      this.saveFileDialog1.Filter = "PMI Data File|*.pmi";
      // 
      // labelRunning
      // 
      this.labelRunning.AutoSize = true;
      this.labelRunning.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
      this.labelRunning.Location = new System.Drawing.Point(129, 527);
      this.labelRunning.Name = "labelRunning";
      this.labelRunning.Size = new System.Drawing.Size(163, 30);
      this.labelRunning.TabIndex = 112;
      this.labelRunning.Text = "Test run time";
      // 
      // labelSavingFile
      // 
      this.labelSavingFile.AutoSize = true;
      this.labelSavingFile.Location = new System.Drawing.Point(228, 13);
      this.labelSavingFile.Name = "labelSavingFile";
      this.labelSavingFile.Size = new System.Drawing.Size(143, 20);
      this.labelSavingFile.TabIndex = 113;
      this.labelSavingFile.Text = "Saving data to file: ";
      // 
      // backgroundWorkerReadScale
      // 
      this.backgroundWorkerReadScale.WorkerReportsProgress = true;
      this.backgroundWorkerReadScale.WorkerSupportsCancellation = true;
      this.backgroundWorkerReadScale.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerReadScale_DoWork);
      this.backgroundWorkerReadScale.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerReadScale_ProgressChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
      this.label1.Location = new System.Drawing.Point(129, 557);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(283, 30);
      this.label1.TabIndex = 114;
      this.label1.Text = "or when weight reaches";
      this.label1.Visible = false;
      // 
      // timerTimeLabel
      // 
      this.timerTimeLabel.Interval = 1000;
      this.timerTimeLabel.Tick += new System.EventHandler(this.timerTimeLabel_Tick);
      // 
      // labelRound
      // 
      this.labelRound.AutoSize = true;
      this.labelRound.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
      this.labelRound.Location = new System.Drawing.Point(29, 57);
      this.labelRound.Name = "labelRound";
      this.labelRound.Size = new System.Drawing.Size(82, 26);
      this.labelRound.TabIndex = 115;
      this.labelRound.Text = "Round:";
      // 
      // labelLast
      // 
      this.labelLast.AutoSize = true;
      this.labelLast.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
      this.labelLast.Location = new System.Drawing.Point(725, 36);
      this.labelLast.Name = "labelLast";
      this.labelLast.Size = new System.Drawing.Size(133, 26);
      this.labelLast.TabIndex = 116;
      this.labelLast.Text = "Last Weight:";
      // 
      // labelAverage
      // 
      this.labelAverage.AutoSize = true;
      this.labelAverage.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
      this.labelAverage.Location = new System.Drawing.Point(725, 63);
      this.labelAverage.Name = "labelAverage";
      this.labelAverage.Size = new System.Drawing.Size(173, 26);
      this.labelAverage.TabIndex = 117;
      this.labelAverage.Text = "Average Weight:";
      // 
      // FormAuto
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.ClientSize = new System.Drawing.Size(958, 642);
      this.Controls.Add(this.labelAverage);
      this.Controls.Add(this.labelLast);
      this.Controls.Add(this.labelRound);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.labelSavingFile);
      this.Controls.Add(this.labelRunning);
      this.Controls.Add(this.statusStrip1);
      this.Controls.Add(this.labelTime);
      this.Controls.Add(this.buttonStopTimer);
      this.Controls.Add(this.chart1);
      this.Controls.Add(this.labelScale);
      this.Controls.Add(this.labelBoardDetected);
      this.MaximizeBox = false;
      this.Name = "FormAuto";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Constant Head Liquid Permeameter - Auto Test";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAuto_FormClosing);
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormAuto_FormClosed);
      this.Load += new System.EventHandler(this.FormAuto_Load);
      ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.Label labelBoardDetected;
    private System.Windows.Forms.Label labelScale;
    private System.Windows.Forms.Timer timerReadScale;
    private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
    private System.Windows.Forms.Button buttonStopTimer;
    private System.Windows.Forms.Label labelTime;
    private System.ComponentModel.BackgroundWorker backgroundWorkerElapsedTime;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    private System.Windows.Forms.Label labelRunning;
    private System.Windows.Forms.Label labelSavingFile;
    private System.ComponentModel.BackgroundWorker backgroundWorkerReadScale;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Timer timerTimeLabel;
    private System.Windows.Forms.Label labelRound;
    private System.Windows.Forms.Label labelLast;
    private System.Windows.Forms.Label labelAverage;
  }
}

