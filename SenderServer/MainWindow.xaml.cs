using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

namespace SenderServer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private string invalidIPAddressMessage = "Пожалуйста, введите корректный IP-адрес";

        private IPAddress currentResource;

        private List<StatusPresentation> presentations = new List<StatusPresentation>();

        private string SuccessStatus = "o";

        private string FailureStatus = "x";

        private string hostname;

        public object UpdClient { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            /*currentResource = GetIpAddress();*/
            hostname = GetHostname();
            if (hostname!=null)
            {
                StartButton.IsEnabled = false;
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += Worker_DoWork;
                worker.ProgressChanged += Worker_ProgressChanged;
                worker.RunWorkerAsync();
            }
            
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            

    
        }


        private void BroadcastToClients()
        {

            int port = 5001;
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            /*IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);*/
            IPEndPoint endPointBroadcast = new IPEndPoint(IPAddress.Broadcast, port);
            /*string ip = "192.168.1.255";*/


            string message = $"{hostname} is not working";
            byte[] data = Encoding.UTF8.GetBytes(message);

            client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);

            client.SendTo(data, endPointBroadcast);

           
            client.Close();
            
        }

       

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
   
            Ping ping = new Ping();
            bool doesNotworkForAMinute = false;
           
       
            while(true)
            {

                try
                {
                    PingReply pingResult = ping.Send(hostname);



                    presentations.Add(new StatusPresentation { Status = SuccessStatus });
                    AccessibilityList.Dispatcher.Invoke(() => AccessibilityList.Items.Add(presentations.Last()));
                    Thread.Sleep(5000);
                }
                

                catch(Exception)
                {
                    int index = 0;
                    doesNotworkForAMinute = true;
                    while(index < 1)
                    {
                        
                        try
                        {
                            ping.Send(hostname);
                            AccessibilityList.Dispatcher.Invoke(() => AccessibilityList.Items.Add(presentations.Last()));
                            doesNotworkForAMinute = false;
                            break;

                        }
                        catch(Exception)
                        {
                            presentations.Add(new StatusPresentation { Status = FailureStatus });
                            AccessibilityList.Dispatcher.Invoke(() => AccessibilityList.Items.Add(presentations.Last()));

                            index++;
                        }
                        Thread.Sleep(5000);




                    }




                }
                if(doesNotworkForAMinute)
                {
                    LogService.CreateLog(presentations.Last(), hostname);
                    BroadcastToClients();
                   

                }

                
                
            }
            
        }

        private IPAddress GetIpAddress()
        {
            try
            {
                return IPAddress.Parse(ResourceInput.Text);
            }
            catch (Exception)
            {
                MessageBox.Show(invalidIPAddressMessage);
                return null;
                
            }
        }
        private string GetHostname()
        {
            return ResourceInput.Text;
        }


       
    }
}
