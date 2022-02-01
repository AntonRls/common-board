using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonBoard
{
    public partial class HostBoardForm : Form
    {
        public HostBoardForm()
        {
            InitializeComponent();
        }
        private UdpClient client = new UdpClient(Form1.PORT);
        private List<IPAddress> addresses = new List<IPAddress>();
        private void HostBoardForm_Load(object sender, EventArgs e)
        {
            showConnect();
            getCurrentConnection();

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            gr = Graphics.FromImage(pictureBox1.Image);
            gr.Clear(Color.White);
        }
        private async void getCurrentConnection()
        {
            await Task.Run(() =>
            {
      
                IPEndPoint end = null;
                while (true)
                {
                    try
                    {
                        string text = Encoding.Unicode.GetString(client.Receive(ref end));
                        string[] connection = text.Split(':');
                        IPAddress ip = IPAddress.Parse(connection[1]);
                        if (connection[0] == "connect")
                        {
                            if (!addresses.Contains(ip))
                            {
                                addresses.Add(ip);
                                showConnect();
                            }
                        }
                        else if (connection[0] == "disconnect")
                        {
                            if (addresses.Contains(ip))
                            {
                                addresses.Remove(ip);
                                showConnect();
                            }
                        }
                        
                    }
                    catch
                    {

                    }
                }
            });
        }

        private void showConnect()
        {
            Text = $"Подключений: {addresses.Count}";
        }


        private bool clicked = false;
        private Graphics gr;

        Point start;
        Point end;
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            clicked = true;
            start = new Point(e.X, e.Y);
        }
        List<Point> points = new List<Point>();
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (clicked)
            {
                end = new Point(e.X, e.Y);
                points.Add(start);
                points.Add(end);
                gr.DrawLines(new Pen(Brushes.Black, 10f), points.ToArray());
                start = end;
                pictureBox1.Invalidate();
                
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            clicked = false;
            points.Clear();
        }
        ImageConverter converter = new ImageConverter();
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            
            foreach(IPAddress ip in addresses)
            {
                byte[] imageBytes = (byte[])converter.ConvertTo(pictureBox1.Image, typeof(byte[]));
                IPEndPoint endP = new IPEndPoint(ip, Form1.PORT_SEND);
                client.Send(imageBytes, imageBytes.Length, endP);
             
            }
        }

        private void HostBoardForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}
