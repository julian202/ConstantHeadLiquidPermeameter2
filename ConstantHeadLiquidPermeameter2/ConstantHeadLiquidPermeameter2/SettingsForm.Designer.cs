namespace ConstantHeadLiquidPermeameter2
{
  partial class SettingsForm
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
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.comboBox1 = new System.Windows.Forms.ComboBox();
      this.button1 = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.textBoxWeight = new System.Windows.Forms.TextBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.label2 = new System.Windows.Forms.Label();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.label3 = new System.Windows.Forms.Label();
      this.textBoxRepetitions = new System.Windows.Forms.TextBox();
      this.groupBox4 = new System.Windows.Forms.GroupBox();
      this.label4 = new System.Windows.Forms.Label();
      this.textBoxWithin = new System.Windows.Forms.TextBox();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.groupBox4.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.comboBox1);
      this.groupBox1.Controls.Add(this.button1);
      this.groupBox1.Location = new System.Drawing.Point(64, 22);
      this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.groupBox1.Size = new System.Drawing.Size(236, 92);
      this.groupBox1.TabIndex = 16;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "COM Port:";
      // 
      // comboBox1
      // 
      this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBox1.FormattingEnabled = true;
      this.comboBox1.Location = new System.Drawing.Point(16, 32);
      this.comboBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.comboBox1.Name = "comboBox1";
      this.comboBox1.Size = new System.Drawing.Size(140, 28);
      this.comboBox1.TabIndex = 1;
      this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
      // 
      // button1
      // 
      this.button1.BackgroundImage = global::ConstantHeadLiquidPermeameter2.Properties.Resources.Refresh;
      this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.button1.Font = new System.Drawing.Font("Arial Narrow", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.button1.Location = new System.Drawing.Point(173, 25);
      this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(42, 42);
      this.button1.TabIndex = 0;
      this.button1.UseVisualStyleBackColor = true;
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(514, 432);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(110, 55);
      this.button2.TabIndex = 17;
      this.button2.Text = "Okay";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.button2_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(323, 34);
      this.label1.MaximumSize = new System.Drawing.Size(200, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(196, 80);
      this.label1.TabIndex = 18;
      this.label1.Text = "Note: The COM Port is set automatically at startup. \r\nOnly change it here if the " +
    "automatic method fails ";
      // 
      // textBoxWeight
      // 
      this.textBoxWeight.Location = new System.Drawing.Point(30, 37);
      this.textBoxWeight.Name = "textBoxWeight";
      this.textBoxWeight.Size = new System.Drawing.Size(100, 26);
      this.textBoxWeight.TabIndex = 19;
      this.textBoxWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.textBoxWeight.TextChanged += new System.EventHandler(this.textBoxWeight_TextChanged);
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.label2);
      this.groupBox2.Controls.Add(this.textBoxWeight);
      this.groupBox2.Location = new System.Drawing.Point(64, 134);
      this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.groupBox2.Size = new System.Drawing.Size(303, 92);
      this.groupBox2.TabIndex = 17;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Maximum scale weight:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(146, 40);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(57, 20);
      this.label2.TabIndex = 19;
      this.label2.Text = "Grams";
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.label3);
      this.groupBox3.Controls.Add(this.textBoxRepetitions);
      this.groupBox3.Location = new System.Drawing.Point(64, 246);
      this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.groupBox3.Size = new System.Drawing.Size(303, 92);
      this.groupBox3.TabIndex = 20;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Maximum test repetitions in Auto Test:";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(146, 40);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(123, 20);
      this.label3.TabIndex = 19;
      this.label3.Text = "Max Repetitions";
      // 
      // textBoxRepetitions
      // 
      this.textBoxRepetitions.Location = new System.Drawing.Point(30, 37);
      this.textBoxRepetitions.Name = "textBoxRepetitions";
      this.textBoxRepetitions.Size = new System.Drawing.Size(100, 26);
      this.textBoxRepetitions.TabIndex = 19;
      this.textBoxRepetitions.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.textBoxRepetitions.TextChanged += new System.EventHandler(this.textBoxRepetitions_TextChanged);
      // 
      // groupBox4
      // 
      this.groupBox4.Controls.Add(this.label4);
      this.groupBox4.Controls.Add(this.textBoxWithin);
      this.groupBox4.Location = new System.Drawing.Point(64, 358);
      this.groupBox4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.groupBox4.Size = new System.Drawing.Size(396, 92);
      this.groupBox4.TabIndex = 21;
      this.groupBox4.TabStop = false;
      this.groupBox4.Text = "Stop Auto Test repetitions when value is within:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(146, 40);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(154, 20);
      this.label4.TabIndex = 19;
      this.label4.Text = "Grams from average";
      // 
      // textBoxWithin
      // 
      this.textBoxWithin.Location = new System.Drawing.Point(30, 37);
      this.textBoxWithin.Name = "textBoxWithin";
      this.textBoxWithin.Size = new System.Drawing.Size(100, 26);
      this.textBoxWithin.TabIndex = 19;
      this.textBoxWithin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // SettingsForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(666, 513);
      this.Controls.Add(this.groupBox4);
      this.Controls.Add(this.groupBox3);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.groupBox1);
      this.Name = "SettingsForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "SettingsForm";
      this.Load += new System.EventHandler(this.SettingsForm_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.groupBox4.ResumeLayout(false);
      this.groupBox4.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.ComboBox comboBox1;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox textBoxWeight;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox textBoxRepetitions;
    private System.Windows.Forms.GroupBox groupBox4;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox textBoxWithin;
  }
}