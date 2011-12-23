namespace SpotASheepWindows
{
  partial class Form1
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
      this.label1 = new System.Windows.Forms.Label();
      this.listBox1 = new System.Windows.Forms.ListBox();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.ForeColor = System.Drawing.Color.Red;
      this.label1.Location = new System.Drawing.Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(227, 42);
      this.label1.TabIndex = 0;
      this.label1.Text = "Highscores!";
      // 
      // listBox1
      // 
      this.listBox1.BackColor = System.Drawing.Color.Black;
      this.listBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.listBox1.ForeColor = System.Drawing.Color.Red;
      this.listBox1.FormattingEnabled = true;
      this.listBox1.ItemHeight = 24;
      this.listBox1.Location = new System.Drawing.Point(19, 55);
      this.listBox1.Name = "listBox1";
      this.listBox1.ScrollAlwaysVisible = true;
      this.listBox1.Size = new System.Drawing.Size(753, 484);
      this.listBox1.TabIndex = 1;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.Black;
      this.ClientSize = new System.Drawing.Size(784, 562);
      this.Controls.Add(this.listBox1);
      this.Controls.Add(this.label1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
      this.Name = "Form1";
      this.Text = "Highscores!";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ListBox listBox1;
  }
}