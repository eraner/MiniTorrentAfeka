﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using MiniTorrent_MediationServerContract;
using Newtonsoft.Json;
using System.Threading;


namespace MiniTorrent_GUI
{
    /// <summary>
    /// Interaction logic for TorrentWindow.xaml
    /// </summary>
    public partial class TorrentWindow : Window
    {
        #region Constants
        public const string Failure = "Failure";
        public const string Notice = "Notice";
        public const string ServerMissingFiles = "ERROR:\nThe server couldn't find the file you are requsting.\nThe download will be canceled.";
        public const long BufferSize = FilesHelper.ONE_KB * 32;
        #endregion

        private MediationReference.MediationServerContractClient client;
        private ConnectionDetails connectionDetails;
        private string localIP;
        private List<FileDetails> availableFiles;
        private CollectionViewSource availableFileSource;
        private List<DownloadingFileItem> downloadingFileList;
        private CollectionViewSource downloadingFileSource;
        private List<FileDetails> ownedFilesList;
        private List<FileDetails> downloadedFilesList;
        private ServerTask serverTask;


        public TorrentWindow(MediationReference.MediationServerContractClient client, ConnectionDetails details, string localIP)
        {
            InitializeComponent();

            this.client = client;
            connectionDetails = details;
            this.localIP = localIP;

            
            availableFileSource = (CollectionViewSource)(FindResource("AvailableFileSource"));
            availableFileSource.Source = availableFiles;
            updateAvailableFiles();
            downloadingFileList = new List<DownloadingFileItem>();
            downloadingFileSource = (CollectionViewSource)(FindResource("DownloadingFileSource"));
            downloadingFileSource.Source = downloadingFileList;
            updateDownloadingFiles();

            serverTask = new ServerTask(connectionDetails, localIP);
        }

        private void updateDownloadingFiles()
        {
            downloadingFileSource.Source = null;
            downloadingFileSource.Source = downloadingFileList;
            
            

            //List<DownloadingFileItem> list = new List<DownloadingFileItem>();
            //list.Add(new DownloadingFileItem
            //{
            //    Percentage = 0,
            //    Size = 20
            //});
            //downloadingFileSource.Source = list;
            //for (int i = 0; i < 90; i++)
            //{
            //    list[0].Percentage++;
            //    Thread.Sleep(600);
            //}
        }

        private void updateAvailableFiles()
        {
            string serializedData = client.GetAvailableFiles();
            availableFiles = JsonConvert.DeserializeObject<List<FileDetails>>(serializedData);
            availableFileSource.Source = availableFiles;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            JsonItems json = new JsonItems
            {
                Username = connectionDetails.Username,
                Password = connectionDetails.Password,
                Ip = localIP
            };
            bool succeded = client.SignOut(JsonConvert.SerializeObject(json));
           // serverTask.CloseConnection();
            if (!succeded)
            {
                MessageBox.Show("Failed to Sing Out properly.\nPlease check your connection.","Failure to close", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Cancel = true;
            }
        }

        private void UpdateFilesButton_Click(object sender, RoutedEventArgs e)
        {
            sendMyFilesToDB();
            updateAvailableFiles();
        }

        private void sendMyFilesToDB()
        {
            List<FileDetails> filesList = FilesHelper.getFilesList(connectionDetails.PublishedFilesSource);
            JsonItems jsonItems = new JsonItems
            {
                Ip = localIP,
                AllFiles = filesList
            };
            string toSend = JsonConvert.SerializeObject(jsonItems);

            client.UpdateUserFiles(toSend);
        }

        private void requestAFile(string fileName)
        {
            FileDetails file = availableFiles.Find(f => f.Name == fileName);

            if (fileExistsOnComputer(file))
                return;

            if (!validateFileRequest(file))
                return;

            string serializedIpPortList = client.GetIpListForAFile(file.Name);
            List<IpPort> ipPortList = JsonConvert.DeserializeObject<List<IpPort>>(serializedIpPortList);

            DownloadingFileItem downloadingFile = new DownloadingFileItem
            {
                Filename = file.Name,
                Size =  file.Size,
                StartedTime = DateTime.Now,
                Percentage = 0
            };

            ClientTask download = new ClientTask(ipPortList, file, connectionDetails, downloadingFile, this);
            downloadingFileList.Add(downloadingFile);
            updateDownloadingFiles();
            RequestFileLabel.Content = file.Name+ ", " + file.Size + " MB, started downloading.";
        }

        private bool validateFileRequest(FileDetails file)
        {
            List<FileDetails> list = new List<FileDetails>();
            list.Add(file);
            JsonItems j = new JsonItems
            {
                Username = connectionDetails.Username,
                Password = connectionDetails.Password,
                AllFiles = list
            };

            string fileDetailsString = client.RequestAFile(JsonConvert.SerializeObject(j));
            if (string.IsNullOrEmpty(fileDetailsString))
            {
                showMessageBox(ServerMissingFiles, Failure);
                return false;
            }
            FileDetails requestedFile = JsonConvert.DeserializeObject<FileDetails>(fileDetailsString);
            string msg = "Please be advise you are going to download the following file:\nFile Name: " +requestedFile.Name +".\nSize: {requestedFile.Size} MB.\n" +
                "Number of Users:"+ requestedFile.Count +" .\n\nAre you sure you want to proceed?";
            if (MessageBoxResult.Cancel ==  showMessageBox(msg, Notice))
            {
                //User selected to cancel the download.
                return false;
            }
            return true;

        }

        public MessageBoxResult showMessageBox(string msg, string title)
        {
            return MessageBox.Show(msg, title, MessageBoxButton.OKCancel);
        }
    
        private bool fileExistsOnComputer(FileDetails file)
        {
            ownedFilesList = FilesHelper.getFilesList(connectionDetails.PublishedFilesSource);
            downloadedFilesList = FilesHelper.getFilesList(connectionDetails.DownloadedFilesDestination);

            if (ownedFilesList.Exists(f => f.Name == file.Name))
            {
                MessageBox.Show("The file: \"" +file.Name +"\", alreadly exists on your computer.\n"
                    + "Please check:" + connectionDetails.PublishedFilesSource + "\"","File Exist", MessageBoxButton.OK);
                return true;
            }

            if (downloadedFilesList.Exists(f => f.Name == file.Name))
            {
                MessageBox.Show("The file: \"" +file.Name+ "\", alreadly exists on your computer.\n"
                    + "Please check:" + connectionDetails.DownloadedFilesDestination +"\"");
                return true;
            }

            return false;

        }

        private void RequestAFileButton_Click(object sender, RoutedEventArgs e)
        {
            FileDetails file = (FileDetails)AvailableFileDataGrid.SelectedItem;
            requestAFile(file.Name);
        }
    }
}
