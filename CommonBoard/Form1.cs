using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonBoard
{
    public partial class Form1 : Form
    {

        public const int PORT = 1033;
        public const int PORT_SEND = 1001;

        public Form1()
        {
            InitializeComponent();
         
        }

        private IPEndPoint endPoint;
        public IPAddress getHost()
        {
            return Dns.GetHostAddresses(Dns.GetHostName())[0];
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox_currentIp.ReadOnly = true;
            textBox_currentIp.Text = getHost().ToString();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            endPoint = new IPEndPoint(IPAddress.Parse(textBox_otherIp.Text), PORT);
            GetBoardForm client = new GetBoardForm(endPoint);
            client.Show();
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HostBoardForm host = new HostBoardForm();
            host.Show();
            Hide();
        }
    }
}
