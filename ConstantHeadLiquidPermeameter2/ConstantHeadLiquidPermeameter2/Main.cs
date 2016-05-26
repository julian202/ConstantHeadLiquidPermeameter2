using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using System.Deployment.Application;

namespace ConstantHeadLiquidPermeameter2
{
  public partial class Main : Form
  {
    public Main()
    {
      InitializeComponent();
    }

    private void buttonTest_Click(object sender, EventArgs e)
    {
      this.Hide();

      SetupForm setup = new SetupForm();
      DialogResult setupResult = setup.ShowDialog();

      if (setupResult != DialogResult.Cancel)
      {
        FormAuto setScrn = new FormAuto("1");
        setScrn.ShowDialog();
      }
  
      if (!this.IsDisposed)
      {
        this.Show();
      }
    }

    private void buttonSettings_Click(object sender, EventArgs e)
    {
      this.Hide();
      FormManual setScrn = new FormManual();
      setScrn.ShowDialog();
      if (!this.IsDisposed)
      {
        this.Show();
      }
    }

    private void buttonReport_Click(object sender, EventArgs e)
    {
      this.Hide();
      Report setScrn = new Report();
      setScrn.ShowDialog();
      this.Show();
    }

    private void Main_Load(object sender, EventArgs e)
    {
      
      Assembly assembly = Assembly.GetExecutingAssembly();
      FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
      string version = fileVersionInfo.ProductVersion;
      //MessageBox.Show(ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString());
      /*if (ApplicationDeployment.IsNetworkDeployed)
      {
        MessageBox.Show(ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString());
        MessageBox.Show("Version " + Application.ProductVersion);
      }*/
    }
  }
}
