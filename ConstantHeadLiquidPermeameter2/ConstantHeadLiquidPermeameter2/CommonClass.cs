using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConstantHeadLiquidPermeameter2
{
  public static class CommonClass
  {
    static Properties.Settings settings = Properties.Settings.Default;
    static StreamWriter SR;
    static string filePath;
    static Regex regex;
    static Match match;
    public static string previouslyFiltered = "0";

    public static void addVelToLast5(string v)
    {
      settings.last5vel5 = settings.last5vel4;
      settings.last5vel4 = settings.last5vel3;
      settings.last5vel3 = settings.last5vel2;
      settings.last5vel2 = settings.last5vel1;
      settings.last5vel1 = v;

      settings.last5H5 = settings.last5H4;
      settings.last5H4 = settings.last5H3;
      settings.last5H3 = settings.last5H2;
      settings.last5H2 = settings.last5H1;
      settings.last5H1 = settings.height;

      settings.Save();
    }

    public static void writeHeader()
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
      SR.WriteLine("");
      SR.WriteLine("Time (Seconds)" + "\t" + "Weight (Grams)");
      SR.WriteLine("");
      SR.Close();

      //SR.WriteLine("Flow Velocity = " + (weight / (Convert.ToDouble(settings.area) * time)).ToString("#0.000") + " cm/s";);
      //SR.WriteLine("");
    }

    public static string filterRegex(string received)
    {
      regex = new Regex(@"\d{4}[.]\d{2}");
      match = regex.Match(received);
      if (match.Success)
      {
        if (received.Contains("-"))
        {
          previouslyFiltered = "-" + match.Value;
          return "-" + match.Value;
        }
        else
        {
          previouslyFiltered = match.Value;
          return match.Value;
        }
      }
      else
      {
        return previouslyFiltered;
      }
    }

  }
}
