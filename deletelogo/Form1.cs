using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Point Point { get; set; }
        public string Video { get; set; }
        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "选择视频";
            open.Filter = "mp4|*.mp4|flv|*.flv";
            if (open.ShowDialog() == DialogResult.OK)
            {
                var file = open.FileName;
                this.Video = open.FileName;
                if (System.IO.File.Exists("1.jpg"))
                {
                    System.IO.File.Delete("1.jpg");
                }                
                FFMpegHelper.Screenshot(file, "1.jpg");

                Form2 form2 = new Form2(this, "1.jpg");
                progressBar1.Value = progressBar1.Minimum;
                form2.Show();
            }
        }

    }
    public class Point
    {
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

    }
}
