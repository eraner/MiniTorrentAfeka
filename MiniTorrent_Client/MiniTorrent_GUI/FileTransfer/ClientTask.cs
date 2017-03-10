using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MiniTorrent_MediationServerContract;

namespace MiniTorrent_GUI
{
    public class ClientTask
    {
        private Object locker = new Object();
        private byte[] fileBuffer;
        private int numOfPeers;
        private int totalSizeInBytes;
        private int bytesReceived;
        private FileDetails fileInfo;
        private ConnectionDetails connDetails;
        private DownloadingFileItem downloadingFileItem;
        private TorrentWindow torrentWindow;

        public ClientTask(List<IpPort> ipList, FileDetails fileInfo, ConnectionDetails connDetails, DownloadingFileItem downloadingFileItem, TorrentWindow torrentWindow)
        {
            this.torrentWindow = torrentWindow;

            bytesReceived = 0;
            numOfPeers = ipList.Count;
            totalSizeInBytes = (int)(fileInfo.Size * FilesHelper.ONE_MB);

            fileBuffer = new byte[totalSizeInBytes];

            this.fileInfo = fileInfo;
            this.connDetails = connDetails;
            this.downloadingFileItem = downloadingFileItem;

            requestFromPeers(ipList);
        }

        private async void requestFromPeers(List<IpPort> ipList)
        {
            int bytesPerPeer = (int)(totalSizeInBytes / numOfPeers);
            int counterBytes = 0;

            for(int i=0 ; i < numOfPeers ; i++)
            {
                TcpClient socket = new TcpClient();
                await socket.ConnectAsync(IPAddress.Parse(ipList[i].IpAddress), 
                                            Convert.ToInt32(ipList[i].Port));
                int bytesStart = counterBytes;
                int bytesEnd = i != (numOfPeers-1)? bytesPerPeer : totalSizeInBytes;

                Downloader downloader = new Downloader(socket, bytesStart, bytesEnd, fileInfo, this, downloadingFileItem);
            }
        }

        public void UpdateFinalFileBuffer (byte[] buffer, int startIndex, int size)
        {
            lock (locker) 
            {
                Array.Copy(buffer, 0, fileBuffer, startIndex, size);
                bytesReceived += size;
            }
            if (bytesReceived == totalSizeInBytes)
            {
                writeFinalFile();
            }
        }

        private void writeFinalFile()
        {
            string path = connDetails.DownloadedFilesDestination + "\\" + fileInfo.Name;
            File.WriteAllBytes(path, fileBuffer);

            downloadingFileItem.EndedTime = DateTime.Now;
            TimeSpan totalTime = downloadingFileItem.EndedTime - downloadingFileItem.StartedTime;
            float byteRate = (float)(totalSizeInBytes / totalTime.TotalSeconds);
            torrentWindow.showMessageBox("Congrats! you've downloaded succesfully the file: \nFile Name: "+fileInfo.Name + " \nSize: " + fileInfo.Size
                + "MB\nTotal Time: " + totalTime.TotalSeconds + " sec, \nDownload speed: " + byteRate + " byte/sec", "Downloading Succesful!!");
        }
    }
}
