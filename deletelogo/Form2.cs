using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {    
        private Point Point { get; set; }
        public Form1 form1 { get; set; }
       
        public Form2(Form1 form1, string img) {
            this.form1 = form1;
          
            InitializeComponent();
            var image = Image.FromFile(img);
            this.Width = image.Width;
            this.Height = image.Height+40;
            pictureBox1.Image = image;
           
            g = pictureBox1.CreateGraphics();
        }
        public bool drawkine = false;
        Graphics g;  int startx, starty;
        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            drawkine = false;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                startx = e.X;
                starty = e.Y;
                drawkine = true;
            }
            else if (e.Button == MouseButtons.Right) {
                pictureBox1.Refresh();
                this.Point = null;
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            g.Dispose();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog() {
                Filter = "mp4|*.mp4"
            };
            var j = new DataReceivedEventHandler((a, b) => {
                if (b.Data == null)
                {
                    return;
                }
                if (b.Data.Trim().StartsWith("Duration:"))
                {
                    Regex rr = new Regex("(?<=Duration: ).+(?=, start)");
                    var mm = rr.Match(b.Data);
                    if (mm.Success)
                    {
                        SetPM((DateTime.Parse("0001-1-1 " + mm.Value) - new DateTime()).TotalSeconds);
                    }
                }
                else if (b.Data.Trim().StartsWith("frame="))
                {
                    Regex rr = new Regex("(?<=time=).+(?= bitrate=)");
                    var mm = rr.Match(b.Data);
                    if (mm.Success)
                    {
                        SetPV((DateTime.Parse("0001-1-1 " + mm.Value) - new DateTime()).TotalSeconds);
                    }
                }
                else if(b.Data.Trim().StartsWith("[aac @") && !this.form1.button1.Enabled && b.Data.Contains("Qavg:"))
                {
                    SetPV(this.form1.progressBar1.Maximum);
                    BtnEnabled();
                }
            })   ;
            if (saveFile.ShowDialog() == DialogResult.OK && this.Point != null) {

                this.form1.button1.Enabled = false;
                FFMpegHelper.DeleteLogo(this.form1.Video, saveFile.FileName, this.Point.StartX, this.Point.StartY, this.Point.Width, this.Point.Height
                    , j
                    , j);
                
            }
            
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawkine)
            {
                pictureBox1.Refresh();
                Pen p = new Pen(Color.Black, 1);
                g.DrawRectangle(p, new Rectangle(startx, starty, e.X-startx, e.Y-starty));
                this.Text = $"选择logo区域后关闭窗口(x:{startx},y:{starty}:x:{e.X},y:{e.Y})";
                this.Point = new Point() {
                StartX = startx,
                StartY = starty,
                Width = e.X-startx,
                Height = e.Y - starty
                };
                
            }
        }


        public void SetPM(double max)
        {
            if (form1. progressBar1.InvokeRequired)
            {
                form1.progressBar1.Invoke(new Action(() => { form1.progressBar1.Maximum = (int)max; }));
            }
        }
        public void SetPV(double v)
        {
            if (form1.progressBar1.InvokeRequired)
            {
                form1.progressBar1.Invoke(new Action(() => { form1.progressBar1.Value = (int)v; }));
            }
        }
        public void BtnEnabled() {
            if (form1.button1.InvokeRequired)
            {
                form1.button1.Invoke(new Action(() => { form1.button1.Enabled = true; }));
            }
        }
    }
}
