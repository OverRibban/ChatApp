using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TcpListener listener;
        TcpClient client;
        int port = 12345;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            IPAddress adress = IPAddress.Parse(tBIP.Text);
            client = new TcpClient();
            client.NoDelay = true;
            client.Connect(adress, port);
            if(client.Connected)
            {
                byte[] outData = Encoding.Unicode.GetBytes("Hej");
                client.GetStream().Write(outData, 0, outData.Length);
                client.Close();
            }
        }

        private void buttonClient_Click(object sender, RoutedEventArgs e)
        {
            gridHome.Visibility = Visibility.Hidden;
            gridClient.Visibility = Visibility.Visible;
        }

        private void buttonHost_Click(object sender, RoutedEventArgs e)
        {
            gridHome.Visibility = Visibility.Hidden;
            gridHost.Visibility = Visibility.Visible;
        }

        private void btnReceive_Click(object sender, RoutedEventArgs e)
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            this.client = listener.AcceptTcpClient();

            byte[] inData = new byte[256];
            int numByte = this.client.GetStream().Read(inData, 0, inData.Length);

            tBHostMessage.Text = Encoding.Unicode.GetString(inData, 0, numByte);
            this.client.Close();
            listener.Stop();
        }
    }
}
