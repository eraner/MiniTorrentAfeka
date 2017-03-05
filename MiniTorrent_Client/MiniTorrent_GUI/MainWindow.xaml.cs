﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using System.Xml;
using MiniTorrent_MediationServerContract;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;

namespace MiniTorrent_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constants
        public const string CONFIG_FILE_NAME = "MyConfig.xml";
        public const string MINI_TORRENT_PARAMS = "MiniTorrentParams";
        public const string SERVER_IP_ADDRESS = "ServerIpAddress";
        public const string INCOMING_PORT = "IncomingTcpPort";
        public const string OUTGOING_PORT = "OutgoingTcpPort";
        public const string USERNAME = "Username";
        public const string PASSWORD = "Password";
        public const string SOURCE_DIR = "PublishedFilesSource";
        public const string DEST_DIR = "DownloadedFilesDestination";
        private const long ONE_KB = 1024;
        private const long ONE_MB = ONE_KB * 1024;
        #endregion

        private MediationReference.MediationServerContractClient client;
        private ConnectionDetails connectionDetails;
        private string localIp;

        public MainWindow()
        {
            InitializeComponent();
            client = new MediationReference.MediationServerContractClient();
            localIp = GetLocalIPAddress();
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
                        "</ MiniTorrentParams >", "Invalid MyConfig.xml File",MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            if (!PreDefinedConfigCheckBox.IsChecked.Value)
                updateConfigFile();

            if (!validateConfigFile())
                return;

            try
            {
                if (!ConnectToServer())
                    return;
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                MessageBox.Show("Failed to communicate with the server,\nPlease validate that the service is up.","Failure", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            TorrentWindow torrentWin = new TorrentWindow(client, connectionDetails, localIp);
            torrentWin.Show();
            this.Close();
        }

        private bool ConnectToServer()
        {
            List<FileDetails> filesList = getFilesList(connectionDetails.PublishedFilesSource);
            JsonItems jsonItems = new JsonItems
            {
                Username = connectionDetails.Username,
                Password = connectionDetails.Password,
                Ip = localIp,
                Port = connectionDetails.IncomingTcpPort.ToString(),
                AllFiles = filesList
            };
            string toSend = JsonConvert.SerializeObject(jsonItems);

            return client.SingIn(toSend);
        }

        public static  List<FileDetails> getFilesList(string source)
        {
            List<FileDetails> filesList = new List<FileDetails>();

            if (!Directory.Exists(source))
            {
                //No files to publish returning empty list.
                return filesList;
            }

            string[] filesPaths = Directory.GetFiles(source);
            foreach (string file in filesPaths)
            {
                FileDetails curr = getFileDetailes(file);
                if (curr != null)
                    filesList.Add(curr);
            }

            string[] subDirs = Directory.GetDirectories(source);
            foreach (string dir in subDirs)
            {
                filesList.AddRange(getFilesList(dir));
            }

            return filesList;
        }

        public static FileDetails getFileDetailes(string path)
        {
            FileDetails file = new FileDetails();
            if (!File.Exists(path))
                return null;

            long sizeInBytes = new FileInfo(path).Length;
            file.Size = ((float)sizeInBytes) / ONE_MB;
            file.Name= System.IO.Path.GetFileName(path);
            file.Count = -1;

            return file;
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

        public string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
    }
}
