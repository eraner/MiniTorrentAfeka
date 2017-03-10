using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MiniTorrent_GUI
{
    public class Uploader
    {
        private TcpClient clientSocket;
        private ConnectionDetails connDetails;

        public Uploader(TcpClient client, ConnectionDetails conn)
        {
            clientSocket = client;
            connDetails = conn;
            Thread thread = new Thread(startNewClient);
            thread.Start();
        }

        private void startNewClient()
        {
            byte[] buffer = new byte[TorrentWindow.BufferSize];
            NetworkStream stream = clientSocket.GetStream();

            //Reading file details from client.
            stream.Read(buffer, 0, buffer.Length);
            
            string jsonString = Encoding.ASCII.GetString(buffer);
            //int pos = jsonString.IndexOf('}');
            //jsonString = jsonString.Substring(0, pos+1);
            FileDataContract fileDetails = JsonConvert.DeserializeObject<FileDataContract>(jsonString);
            uploadFile(stream, fileDetails);
        }

        private async void uploadFile(NetworkStream stream, FileDataContract fileDetails)
        {
            byte[] fileBuffer = new byte[TorrentWindow.BufferSize];
            int startByte = fileDetails.StartByte;
            int endByte = fileDetails.EndByte;

            FileInfo fileToSend = new FileInfo(connDetails.PublishedFilesSource + "\\" + fileDetails.Filename);

            FileStream fileStream = new FileStream(fileToSend.FullName, FileMode.Open, FileAccess.Read);
            fileStream.Seek(startByte, 0);

            int byteCount = 0;

            while (startByte < endByte)
            {
                if (endByte - startByte >= fileBuffer.Length)
                {
                    fileStream.Read(fileBuffer, 0, fileBuffer.Length);
                    byteCount = fileBuffer.Length;
                }
                else
                {
                    fileStream.Read(fileBuffer, 0, endByte - startByte);
                    byteCount = endByte - startByte;
                }

                await stream.WriteAsync(fileBuffer, 0, byteCount);
                startByte += byteCount;
            }

            stream.Close();
            fileStream.Close();
            clientSocket.Close();
        }
    }
}
