using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Xml;
using System.Xml.Linq;

namespace MiniTorrent_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const string CONFIG_FILE_NAME = "MyConfig.xml";
        public const string MINI_TORRENT_PARAMS = "MiniTorrentParams";
        public const string SERVER_IP_ADDRESS = "ServerIpAddress";
        public const string INCOMING_PORT = "IncomingTcpPort";
        public const string OUTGOING_PORT = "OutgoingTcpPort";
        public const string USERNAME = "Username";
        public const string PASSWORD = "Password";
        public const string SOURCE_DIR = "PublishedFilesSource";
        public const string DEST_DIR = "DownloadedFilesDestination";


        private MediationReference.MediationServerContractClient client;
        private ConnectionDetails connectionDetails;

        public MainWindow()
        {
            InitializeComponent();
            client = new MediationReference.MediationServerContractClient();

            //validateConfigFile();
        }

        private bool validateConfigFile()
        {
            if (connectionDetails == null)
            {
                connectionDetails = new ConnectionDetails();
            }
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(CONFIG_FILE_NAME);

            try
            {
                XmlNode paramsNode = xDoc.SelectSingleNode("/"+ MINI_TORRENT_PARAMS);
                connectionDetails.ServerIpAddress = paramsNode.SelectSingleNode(SERVER_IP_ADDRESS).InnerText;
                connectionDetails.IncomingTcpPort = Convert.ToInt32(paramsNode.SelectSingleNode(INCOMING_PORT).InnerText);
                connectionDetails.OutgoingTcpPort = Convert.ToInt32(paramsNode.SelectSingleNode(OUTGOING_PORT).InnerText);
                connectionDetails.Username = paramsNode.SelectSingleNode(USERNAME).InnerText;
                connectionDetails.Password = paramsNode.SelectSingleNode(PASSWORD).InnerText;
                connectionDetails.PublishedFilesSource = paramsNode.SelectSingleNode(SOURCE_DIR).InnerText;
                connectionDetails.DownloadedFilesDestination = paramsNode.SelectSingleNode(DEST_DIR).InnerText;
            }
            catch
            {
                MessageBox.Show("Please check your settings & MyConfig.xml file for valid format.\ni.e.:<MiniTorrentParams>\n" +
                        "< ServerIpAddress > 192.168.0.109 </ ServerIpAddress >\n" +
                        "< IncomingTcpPort > 8005 </ IncomingTcpPort >\n" +
                        "< OutgoingTcpPort > 8006 </ OutgoingTcpPort >\n" +
                        "< Username > Username1 </ Username >\n" +
                        "< Password > password1 </ Password >\n" +
                        "< PublishedFilesSource > D:\\UTorrent </ PublishedFilesSource >\n" +
                        "< DownloadedFilesDestination > D:\\MiniTorrent_Downloaded </ DownloadedFilesDestination >\n" +
                        "</ MiniTorrentParams >","Invalid MyConfig.xml File",MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            if (!PreDefinedConfigCheckBox.IsChecked.Value)
                updateConfigFile();

            if (validateConfigFile())
            {

            }
        }

        private void updateConfigFile()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(CONFIG_FILE_NAME);
            XmlNode paramsNode = xDoc.SelectSingleNode("/" + MINI_TORRENT_PARAMS);

            paramsNode.SelectSingleNode(SERVER_IP_ADDRESS).InnerText = ServerIpAddressTextbox.Text;
            paramsNode.SelectSingleNode(INCOMING_PORT).InnerText = IncomingTcpPortTextbox.Text;
            paramsNode.SelectSingleNode(OUTGOING_PORT).InnerText = OutgoingTcpPortTextbox.Text;
            paramsNode.SelectSingleNode(USERNAME).InnerText = UsernameTextbox.Text;
            paramsNode.SelectSingleNode(PASSWORD).InnerText = PasswordTextbox.Text;
            paramsNode.SelectSingleNode(SOURCE_DIR).InnerText = PublishedFilesSourceTextbox.Text;
            paramsNode.SelectSingleNode(DEST_DIR).InnerText = DownloadedFilesDestTextbox.Text;

            xDoc.Save(CONFIG_FILE_NAME);
        }

        private void checkBox_Changed(object sender, RoutedEventArgs e)
        {
            bool check = PreDefinedConfigCheckBox.IsChecked.Value;

            ServerIpAddressTextbox.IsEnabled = !check;
            IncomingTcpPortTextbox.IsEnabled = !check;
            OutgoingTcpPortTextbox.IsEnabled = !check;
            UsernameTextbox.IsEnabled = !check;
            PasswordTextbox.IsEnabled = !check;
            PublishedFilesSourceTextbox.IsEnabled = !check;
            DownloadedFilesDestTextbox.IsEnabled = !check;
        }
    }
}
