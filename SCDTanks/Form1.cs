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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
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
