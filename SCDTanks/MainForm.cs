using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        private void button1_Click(object sender, EventArgs e)
        {
            string port = numericUpDown1.Value.ToString();
            try
            {
                StartOptions so = new StartOptions();
                so.Urls.Add($"http://+:{port}");
                WebApp.Start<Startup>(so);
                Write($"服务启动成功，监听端口：{port}");
            }
            catch(Exception ex)
            {
                Write(ex.Message);
            }
        }
        private void Write(string text)
        {
            richTextBox1.AppendText($"[{DateTime.Now.ToLongTimeString()}]:{text}");
        }
    }
}
