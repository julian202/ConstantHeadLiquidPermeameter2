using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace ConstantHeadLiquidPermeameter2
{
  public static class COMMS
  {
    public static SerialPort serialPort1 = new SerialPort("something", 115200, Parity.None, 8, StopBits.One);
    private static Properties.Settings settings = Properties.Settings.Default;
    public static bool commBusy;
    static Char newLineChar = (char)13;
    static string newLine = (newLineChar.ToString());
    public static bool connected;
    public static bool cancelling = false;

    public static void autoFindPort()
    {
      autoFindPortONCE();
      //try again:
      if (!connected)
      {
        autoFindPortONCE();
      }
      if (!connected)
      {
        MessageBox.Show("Machine not detected. Make sure the usb cable is connected and that the machine is powered.");

      }
    }

    private static void autoFindPortONCE()
    {
      serialPort1.ReadTimeout = 1000;
      serialPort1.WriteTimeout = 1000;

      foreach (string port in SerialPort.GetPortNames())
      {
        if (serialPort1.IsOpen)
        {
          serialPort1.Close();
        }
        serialPort1.PortName = port;
        try
        {
          serialPort1.Open();
          serialPort1.DiscardInBuffer();
          serialPort1.Write("W");
          Thread.Sleep(20);
          string returnValue = serialPort1.ReadExisting();
          Output(port + " returned: " + returnValue);
          if (returnValue.Contains("Testing"))
          {
            //MessageBox.Show("Port is " + port);
            settings.COMM = port;
            settings.Save();

            //labelBoardDetected.ForeColor = Color.Green;
            //toolStripStatusLabel1.Text = "Connected to machine (on " + port + ")";
            //break; //break out of foreach loop.
            connected = true;
            return; //break out of function.
          }

        }
        catch (Exception ex)
        {
          Output(ex.Message);
        }
      }
      //if it doesn't break out of the function (by hiting return):
      connected = false;
      //labelBoardDetected.Text = "Machine not detected. Please connect USB cable and restart program.";
      //labelBoardDetected.ForeColor = Color.Red;
      //labelBoardDetected.Visible = true;
      serialPort1.Close();
    }

    public static void openSerial(string a)
    {
      serialPort1.PortName = a;
      serialPort1.Open();
    }

    public static void openValve1()
    {
      MoveValve(4, "O");
    }
    public static void closeValve1()
    {
      MoveValve(4, "C");
    }
    public static void openValve2()
    {
      MoveValve(2, "C");
    }
    public static void closeValve2()
    {
      MoveValve(2, "O");
    }
    public static void openValve3()
    {
      MoveValve(3, "O");
    }
    public static void closeValve3()
    {
      MoveValve(3, "C");
    }
    public static void openValve4()
    {
      MoveValve(4, "O");
    }
    public static void closeValve4()
    {
      MoveValve(4, "C");
    }

    public static void MoveValve(int valveNum, string vPos)
    {
      //no 0 offset any longer.  Valve 1 = A and so on.
      Thread.Sleep(10);
      //convert valve number to an ascii code
      int realValve = valveNum + 64;
      //convert ascii code to a character
      Char valveName = (char)realValve;
      //append ascii code to valve position
      //ie. open valve 1 would be "OA"
      try
      {
        serialPort1.DiscardInBuffer();
        serialPort1.DiscardOutBuffer();
        serialPort1.Write(vPos + valveName.ToString());
      }
      catch (InvalidOperationException qT)
      {
        //ERROR HERE
        DialogResult stuff = MessageBox.Show("Error: " + qT.Message + Environment.NewLine + "Connect the USB Cable to the Machine.", "Reconnect USB Cable", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
        if (stuff== DialogResult.Cancel)
        {
          cancelling = true;
          Application.Exit();
        }
        //Application.Exit();
        //Close();
        //return;
      }
    }

    public static double ReadScale()
    {
      return ReadScale(1);
    }


    public static string ReadScaleAWSContinuous()
    {
      string received;
      Char newLineChar = (char)13;
      string newLine = (newLineChar.ToString());

      //string a = serialPort1.ReadExisting();
      //received = serialPort1.ReadTo(newLine);
      received = serialPort1.ReadExisting();

      return received;
      /*
      if (a.Contains("WT:"))
      {
        a.Split(new string[] { "WT:" }, StringSplitOptions.None);
        Debug.WriteLine(a[1].ToString());
        return a[1].ToString();
      }
      else
      {
        return "";
      }  */
    }

    public static double ReadScale(int scale)
    {
      string b = rsEcho("MR" + scale.ToString());
      try
      {
        double d = Convert.ToDouble(b);
        return d;
      }
      catch (FormatException ex)
      {
        Output("Error in response from scale " + ex.Message);
        return 0.0;
      }
    }



    public static string ReadScaleAWS()
    {
      //string b = rsEcho("MR");
      //string b = sendOnce("MR");
      string b = sendOnce("MQ");
      return b;
      /*
      try
      {
        double d = Convert.ToDouble(b);
        return d;
      }
      catch (FormatException ex)
      {
        MessageBox.Show("Error in response from scale " + ex.Message);
        //Output("Error in response from scale " + ex.Message);
        return 0.0;
      }*/


    }

    public static string rsEcho(string toSend)
    {
      //set up needed variables
      string received;
      Char newLineChar = (char)13;
      string newLine = (newLineChar.ToString());

      if (commBusy)
      {
        return null;
      }
      //send the initial command out
      Send(toSend);

      //if we're not connected to a machine, Send can handle it's own error, we just need
      //to clean up an error that would come from waiting for a response.

      //next response should be the value we need

      //return "321";
      try
      {

        Output("Waiting To Receive 2...");
        received = serialPort1.ReadTo(newLine); 
        Output("Received 2: " + received);
        System.Diagnostics.Debug.Write("Return Value: " + received + Environment.NewLine);
        return received;
      }
      catch (IOException ex)
      {
        System.Diagnostics.Debug.WriteLine(ex.Message);
        return null;
      }
      catch (InvalidOperationException ex)
      {
        System.Diagnostics.Debug.WriteLine(ex.Message);
        return null;
      }
      catch (Exception ex)
      {
        Output("#############################");
        Output(ex.Message);
        return null;
      }
    }

    public static void zeroScale()
    {
      zeroScale(1);
    }

    public static void zeroScale(int scale)
    {
      for (int i = 0; i < 1; i++)
      {
        Send("MZ" + scale.ToString());
        try
        {
          string received = serialPort1.ReadTo(newLine);
          break;
        }
        catch (Exception)
        {
          Output("ERROR: NO RETURN VALUE FROM ZERO SCALE");
          Output("---THIS WAS TRY NUMBER: " +i.ToString()+"-----------------------");
        }
      }    
    }


    public static string sendOnce(String toSend)
    {
      string received;
      Char newLineChar = (char)13;
      string newLine = (newLineChar.ToString());
      /*
      if (!connected || demoMode)
      {
        return;
      }*/
      //boolean to say comms are busy.  Will be used in que list.
      commBusy = true;
      try
      {
        //if the port ain't open, open it.
        //if (!(serialPort1.IsOpen))
        //{
        //    serialPort1.Open();
        //}
        //clear input and output buffer first. 

        //serialPort1.DiscardInBuffer();
        //serialPort1.DiscardOutBuffer();

        //send out string and echo it to console
        //for troubleshooting
        serialPort1.Write(toSend);
        Output("Sent command: " + toSend);
        //get the first respose, which will be the echo command
        //character
        Output("Waiting To Receive 1...");
        Thread.Sleep(200);
        //received = serialPort1.ReadTo(newLine);
        received = serialPort1.ReadExisting();

        //Thread.Sleep(20);
        Output("Received 1: " + received);
        return received;
      }
      catch (IndexOutOfRangeException kj)
      {
        DialogResult stuff = MessageBox.Show("Error: " + kj.Message + Environment.NewLine + "", Application.ProductName.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        commBusy = false;
        return "";
      }
      catch (InvalidOperationException qT)
      {
        //ERROR HERE
        DialogResult stuff = MessageBox.Show("Error: " + qT.Message + Environment.NewLine + "Connect the USB Cable to the Machine.", "Reconnect USB Cable", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
        if (stuff == DialogResult.Cancel)
        {
          cancelling = true;
          Application.Exit();
          Application.Exit();
        }
        //Application.Exit();
        commBusy = false;
        return "";
      }
      catch (IOException ex)
      {

        DialogResult stuff = MessageBox.Show("Error: " + ex.Message + Environment.NewLine + "", Application.ProductName.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        commBusy = false;
        return "";
      }
      catch (Exception ex) //catches exception that weren't catched up to now
      {
        DialogResult stuff = MessageBox.Show("Error: " + ex.Message + Environment.NewLine + "", Application.ProductName.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        commBusy = false;
        return "";
      }
      commBusy = false;
    }


    public static void Send(String toSend)
    {
      string received;
      Char newLineChar = (char)13;
      string newLine = (newLineChar.ToString());
      /*
      if (!connected || demoMode)
      {
        return;
      }*/
      //boolean to say comms are busy.  Will be used in que list.
      commBusy = true;
      try
      {
        //if the port ain't open, open it.
        //if (!(serialPort1.IsOpen))
        //{
        //    serialPort1.Open();
        //}
        //clear input and output buffer first. 
        serialPort1.DiscardInBuffer();
        serialPort1.DiscardOutBuffer();

        //send out string and echo it to console
        //for troubleshooting
        serialPort1.Write(toSend);
        Output("Sent command: " + toSend);
        //get the first respose, which will be the echo command
        //character
        Output("Waiting To Receive 1...");
        Thread.Sleep(20);
        received = serialPort1.ReadTo(newLine);
        //Thread.Sleep(20);
        Output("Received 1: " + received);

      }
      catch (IndexOutOfRangeException kj)
      {
        DialogResult stuff = MessageBox.Show("Error: " + kj.Message + Environment.NewLine + "", Application.ProductName.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);

        //return;
      }
      catch (InvalidOperationException qT)
      {
        //ERROR HERE
        DialogResult stuff = MessageBox.Show("Error: " + qT.Message + Environment.NewLine + "Connect the USB Cable to the Machine.", "Reconnect USB Cable", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
        if (stuff == DialogResult.Cancel)
        {
          cancelling = true;
          Application.Exit();
          Application.Exit();
        }
        //Application.Exit();
        //return;
      }
      catch (IOException ex)
      {

        DialogResult stuff = MessageBox.Show("Error: " + ex.Message + Environment.NewLine + "", Application.ProductName.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);

        //return;
      }
      catch (Exception ex) //catches exception that weren't catched up to now
      {
        DialogResult stuff = MessageBox.Show("Error: " + ex.Message + Environment.NewLine + "", Application.ProductName.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);

      }
      commBusy = false;
    }

    public static void Output(string s)
    {
      System.Diagnostics.Debug.WriteLine(s);
    }

  }
}
