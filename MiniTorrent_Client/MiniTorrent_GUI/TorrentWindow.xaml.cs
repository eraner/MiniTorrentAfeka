﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using MiniTorrent_MediationServerContract;
using Newtonsoft.Json;


namespace MiniTorrent_GUI
{
    /// <summary>
    /// Interaction logic for TorrentWindow.xaml
    /// </summary>
    public partial class TorrentWindow : Window
    {
        #region Constants
        public const string FAILURE = "Failure";
        public const string NOTICE = "Notice";
        public const string SERVER_MISSING_FILES = "ERROR:\nThe server couldn't find the file you are requsting.\nThe download will be canceled.";
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


        public TorrentWindow(MediationReference.MediationServerContractClient client, ConnectionDetails details, string localIP)
        {
            InitializeComponent();

            this.client = client;
            connectionDetails = details;
            this.localIP = localIP;

            
            //CollectionViewSource itemCollectionViewSource;
            availableFileSource = (CollectionViewSource)(FindResource("AvailableFileSource"));
            availableFileSource.Source = availableFiles;
            updateAvailableFiles();
            downloadingFileSource = (CollectionViewSource)(FindResource("DownloadingFileSource"));
            //TODO
           // updateDownloadingFiles();

            ServerTask serverTask = new ServerTask(connectionDetails);
        }

        private void updateDownloadingFiles()
        {
            throw new NotImplementedException();
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
            if (!succeded)
            {
                MessageBox.Show("Failed to Sing Out properly.\nPlease check your connection.","Failure to close", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Cancel = true;
            }
        }

        private void UpdateFilesButton_Click(object sender, RoutedEventArgs e)
        {
            updateAvailableFiles();
        }

        private void requestAFile(string fileName)
        {
            RequestFileLabel.Content = fileName;
            FileDetails file = availableFiles.Find(f => f.Name == fileName);

            if (fileExistsOnComputer(file))
                return;

            if (!validateFileRequest(file))
                return;

            string serializedIpPortList = client.GetIpListForAFile(file.Name);
            List<IpPort> ipPortList = JsonConvert.DeserializeObject<List<IpPort>>(serializedIpPortList);

            ClientTask download = new ClientTask(ipPortList, file, connectionDetails);
            RequestFileLabel.Content = file.Name+ ", " + file.Size + " MB, request flow got to client Task";

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
                showMessageBox(SERVER_MISSING_FILES, FAILURE);
                return false;
            }
            FileDetails requestedFile = JsonConvert.DeserializeObject<FileDetails>(fileDetailsString);
            string msg = "Please be advise you are going to download the following file:\nFile Name: " +requestedFile.Name +".\nSize: {requestedFile.Size} MB.\n" +
                "Number of Users:"+ requestedFile.Count +" .\n\nAre you sure you want to proceed?";
            if (MessageBoxResult.Cancel ==  showMessageBox(msg, NOTICE))
            {
                //User selected to cancel the download.
                return false;
            }
            return true;

        }

        private MessageBoxResult showMessageBox(string msg, string title)
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
