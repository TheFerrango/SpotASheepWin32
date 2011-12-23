using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace SpotASheepWindows
{
  public partial class Form1 : Form
  {
    Game1 g;
    public Form1(Game1 ga)
    {
      InitializeComponent();
      
      g = ga;
      WebClient wc = new WebClient();
      wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
      wc.DownloadStringAsync(new Uri("http://windows98fan.xoom.it/serverPecorelle/index.php"));
    }

    void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
    {
      listBox1.Items.Clear();
      List<string[]> c = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string[]>>(e.Result);
      foreach (string[] s in c)
      {
        string SO = "";
        if (s[2].Contains("NT")) SO = "Windows PC";
        if (s[2].Contains("CE")) SO = "Windows Phone";
        listBox1.Items.Add(string.Format("{0}pts, by {1} on {2}", s[1], s[0], SO));
      }
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      g.isSubmitting = false;
      base.OnClosing(e);
    }
  }
}
