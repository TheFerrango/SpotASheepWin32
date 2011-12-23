using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using Microsoft.Xna.Framework;

namespace SpotASheepWindows
{
  public partial class ScoreSubmit : Form
  {
    int _punkte;
    Game1 g;
    public ScoreSubmit(int punti, Game1 ga)
    {
      InitializeComponent();
      _punkte = punti;
      g = ga;
      label3.Text = _punkte.ToString();
      
    }

    private void button1_Click(object sender, EventArgs e)
    {
      WebClient wc = new WebClient();

      wc.Headers["Content-Type"] = "application/x-www-form-urlencoded";
      string os = Environment.OSVersion.Platform.ToString();
      string toPost = string.Format("Usr={0}&Punkts={1}&OSVersion={2}", textBox1.Text, _punkte, os);
      wc.UploadString(new Uri("http://windows98fan.xoom.it/serverPecorelle/input.php"), toPost);
      g.isSubmitting = false;
      Close();
    }

    private void button2_Click(object sender, EventArgs e)
    {
      g.isSubmitting = false;
      Close();
    }
  }
}
