namespace ConstantHeadLiquidPermeameter2
{
  partial class Main
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
      this.buttonSettings = new System.Windows.Forms.Button();
      this.buttonReport = new System.Windows.Forms.Button();
      this.buttonTest = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // buttonSettings
      // 
      this.buttonSettings.AutoSize = true;
      this.buttonSettings.Image = global::ConstantHeadLiquidPermeameter2.Properties.Resources.settings_48;
      this.buttonSettings.Location = new System.Drawing.Point(191, 24);
      this.buttonSettings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.buttonSettings.Name = "buttonSettings";
      this.buttonSettings.Size = new System.Drawing.Size(157, 122);
      this.buttonSettings.TabIndex = 12;
      this.buttonSettings.Text = "Manual Mode";
      this.buttonSettings.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.buttonSettings.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
      this.buttonSettings.UseVisualStyleBackColor = true;
      this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
      // 
      // buttonReport
      // 
      this.buttonReport.AutoSize = true;
      this.buttonReport.Image = ((System.Drawing.Image)(resources.GetObject("buttonReport.Image")));
      this.buttonReport.Location = new System.Drawing.Point(361, 24);
      this.buttonReport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.buttonReport.Name = "buttonReport";
      this.buttonReport.Size = new System.Drawing.Size(157, 122);
      this.buttonReport.TabIndex = 11;
      this.buttonReport.Text = "Reporting";
      this.buttonReport.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.buttonReport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
      this.buttonReport.UseVisualStyleBackColor = true;
      this.buttonReport.Click += new System.EventHandler(this.buttonReport_Click);
      // 
      // buttonTest
      // 
      this.buttonTest.AutoSize = true;
      this.buttonTest.Image = ((System.Drawing.Image)(resources.GetObject("buttonTest.Image")));
      this.buttonTest.Location = new System.Drawing.Point(20, 24);
      this.buttonTest.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.buttonTest.Name = "buttonTest";
      this.buttonTest.Size = new System.Drawing.Size(157, 122);
      this.buttonTest.TabIndex = 9;
      this.buttonTest.Text = "Auto Mode";
      this.buttonTest.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.buttonTest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
      this.buttonTest.UseVisualStyleBackColor = true;
      this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
      // 
      // Main
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(539, 170);
      this.Controls.Add(this.buttonSettings);
      this.Controls.Add(this.buttonReport);
      this.Controls.Add(this.buttonTest);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Margin = new System.Windows.Forms.Padding(4);
      this.Name = "Main";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "PMI Constant Head Permeameter";
      this.Load += new System.EventHandler(this.Main_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonReport;
    private System.Windows.Forms.Button buttonTest;
    private System.Windows.Forms.Button buttonSettings;
  }
}