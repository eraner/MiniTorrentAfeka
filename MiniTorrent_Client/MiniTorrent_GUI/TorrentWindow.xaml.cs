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
using System.Windows.Shapes;
using MiniTorrent_MediationServerContract;
using Newtonsoft.Json;

namespace MiniTorrent_GUI
{
    /// <summary>
    /// Interaction logic for TorrentWindow.xaml
    /// </summary>
    public partial class TorrentWindow : Window
    {
        private MediationReference.MediationServerContractClient client;
        private ConnectionDetails connectionDetails;
        private string localIP;
        private List<FileDetails> availableFiles;

        public TorrentWindow(MediationReference.MediationServerContractClient client, ConnectionDetails details, string localIP)
        {
            InitializeComponent();

            this.client = client;
            connectionDetails = details;
            this.localIP = localIP;

            updateAvailableFiles();
            CollectionViewSource itemCollectionViewSource;
            itemCollectionViewSource = (CollectionViewSource)(FindResource("ItemCollectionViewSource"));
            itemCollectionViewSource.Source = availableFiles;
        }

        private void updateAvailableFiles()
        {
            string serializedData = client.GetAvailableFiles();
            availableFiles = JsonConvert.DeserializeObject<List<FileDetails>>(serializedData);
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

        }

        private void RequestAFileButton_Click(object sender, RoutedEventArgs e)
        {
            FileDetails file = (FileDetails)AvailableFileDataGrid.SelectedItem;
            requestAFile(file.Name);
        }
    }
}
