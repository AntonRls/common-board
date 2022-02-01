using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace CommonBoard
{
    public partial class GetBoardForm : Form
    {
        public GetBoardForm(IPEndPoint endPoint)
        {
            InitializeComponent();
            point = endPoint;
        }
        private IPEndPoint point;
        private UdpClient client;
        public IPAddress getHost()
        {
            return Dns.GetHostAddresses(Dns.GetHostName())[0];
        }
        private void GetBoardForm_Load(object sender, EventArgs e)
        {
            client = new UdpClient();
         
            byte[] text = Encoding.Unicode.GetBytes($"connect:{getHost().ToString()}");
            client.Send(text,text.Length, point);

        
            getBoardStat();
        }
        private async void getBoardStat()
        {
            await Task.Run(() =>
            {

                UdpClient receiver = new UdpClient(Form1.PORT_SEND);
          
                while (true)
                {
             
                    byte[] stat = receiver.Receive(ref point);
           
                    using (var ms = new MemoryStream(stat))
                    {
                        pictureBox1.Image = Image.FromStream(ms);
                    }
                }

            });
        }
        private void GetBoardForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            byte[] text = Encoding.Unicode.GetBytes($"disconnect:{getHost().ToString()}");
            client.Send(text, text.Length, point);
 
            client.Close();
            client.Dispose();

            Process.GetCurrentProcess().Kill();
        }
    }
}
