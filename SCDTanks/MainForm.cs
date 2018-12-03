using Microsoft.Owin.Hosting;
using SCDTanks.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCDTanks
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Point point = new Point(1, 1);
            Point point1 = new Point(1, 1);
            Console.WriteLine((point == point1).ToString());
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string port = numericUpDown1.Value.ToString();
            try
            {
                StartOptions so = new StartOptions();
                so.Urls.Add($"http://+:{port}");
                WebApp.Start<Startup>(so);
                Write($"服务启动成功，监听端口：{port}");
            }
            catch (Exception ex)
            {
                Write(ex.Message);
            }
        }
        private void Write(string text)
        {
            richTextBox1.AppendText($"[{DateTime.Now.ToLongTimeString()}]:{text}");
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            baseclass baseclass = new a();
            baseclass.Star();
        }
        class baseclass
        {
            protected virtual void Test()
            {
                Debug.WriteLine("Base");
            }

            public void Star()
            {
                Test();
            }
        }
        class a:baseclass
        {
            protected override void Test()
            {
                Debug.WriteLine("A");
            }
        }
    }
}
