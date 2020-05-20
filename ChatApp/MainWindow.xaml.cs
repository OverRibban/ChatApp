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
        List<TcpClient> listOfClients = new List<TcpClient>();
        public MainWindow()
        {
            InitializeComponent();
        }



        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            StartStreaming();
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

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Creates a tcplistener with current IP and set port
                listener = new TcpListener(IPAddress.Any, port);
                //Start listener
                listener.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            btnStart.IsEnabled = false;
            StartReceiving();
        }

        private async void StartReceiving()
        {
            try
            {
                //Waits for user to connect and saves client to variable
                client = await listener.AcceptTcpClientAsync();
                listOfClients.Add(client);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            StartReading(client);
        }

        private async void StartReading(TcpClient k)
        {
            byte[] buffer = new byte[1024];
            int n = 0;
            try
            {
                n = await k.GetStream().ReadAsync(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            tBHostMessage.AppendText(Encoding.Unicode.GetString(buffer, 0, n));
            //recursive loop in order to continue taking in messages
            StartReading(k);
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            client = new TcpClient();
            client.NoDelay = true;
            if(!client.Connected)
            {
                StartConnection();
            }
        }

        private async void StartConnection()
        {
            try
            {
                IPAddress address = IPAddress.Parse(tBIP.Text);
                await client.ConnectAsync(address, port);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            btnConnect.IsEnabled = false;
        }
        private async void StartStreaming()
        {
            byte[] outData = Encoding.Unicode.GetBytes(tBMessage.Text);
            try
            {
                await client.GetStream().WriteAsync(outData, 0, outData.Length);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
    }
}
