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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            byte[] message = Encoding.Unicode.GetBytes(tBMessage.Text);
            IPAddress serverIP = IPAddress.Parse("127.0.0.1");
            IPEndPoint destination = new IPEndPoint(serverIP, 12345);
            UdpClient client = new UdpClient();
            client.Send(message, message.Length, destination);

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
    }
}
