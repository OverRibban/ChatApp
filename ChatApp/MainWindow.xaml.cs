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

        #region GUI BUTTONS

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
                listener = new TcpListener(IPAddress.Any, port); //Creates a tcplistener with current IP and set port
                listener.Start(); //Start listener
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            btnStart.IsEnabled = false;
            StartReceiving();
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            client = new TcpClient();
            client.NoDelay = true;
            if (!client.Connected)
            {
                StartConnection();
            }
        }

        #endregion GUI BUTTONS

        #region FUNCTIONS

        /// <summary>
        /// Starts the server and waits for client
        /// </summary>
        private async void StartReceiving()
        {
            try
            {
                client = await listener.AcceptTcpClientAsync(); //Waits for user to connect and saves client to variable
                listOfClients.Add(client);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            StartReading(client); 
        }
        /// <summary>
        /// Waits for client to message to host
        /// </summary>
        /// <param name="k"></param>
        private async void StartReading(TcpClient k)
        {
            byte[] buffer = new byte[1024]; //send using bytes in favor for faster network speed
            int n = 0;
            try
            {
                n = await k.GetStream().ReadAsync(buffer, 0, buffer.Length); // await sent message from client
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            tBHostMessage.AppendText(Encoding.Unicode.GetString(buffer, 0, n)); //decode bytes into unicode to display onto gui
            //recursive loop in order to continue taking in messages
            StartReading(k); //Recursive loop that continues recieving sent messages
        }

        /// <summary>
        /// Connects to set ip and port
        /// </summary>
        private async void StartConnection()
        {
            try
            {
                IPAddress address = IPAddress.Parse(tBIP.Text); //parse IP to begin connection
                await client.ConnectAsync(address, port);  //connect using said ip and port
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            btnConnect.IsEnabled = false;
        }
        /// <summary>
        /// Sends message in bytes to host
        /// </summary>
        private async void StartStreaming()
        {
            byte[] outData = Encoding.Unicode.GetBytes(tBMessage.Text); //send using bytes in favor for faster network speed
            try
            {
                await client.GetStream().WriteAsync(outData, 0, outData.Length); //send bytes to host
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        #endregion FUNCTIONS
    }
}
