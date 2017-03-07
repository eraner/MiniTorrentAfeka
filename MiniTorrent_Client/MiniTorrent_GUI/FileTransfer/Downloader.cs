﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MiniTorrent_MediationServerContract;
using System.Threading;
using Newtonsoft.Json;

namespace MiniTorrent_GUI
{
    public class Downloader
    {
        private TcpClient socket;
        private int bytesStart;
        private int bytesEnd;
        private int receivedBytes;
        private int totalBytes;
        private FileDetails fileInfo;
        private FileDataContract dataContract;
        private ClientTask clientTask;
        private byte[] generalBuffer;

        public Downloader(TcpClient socket, int bytesStart, int bytesEnd, FileDetails fileInfo, ClientTask clientTask)
        {
            this.socket = socket;
            this.bytesStart = bytesStart;
            this.bytesEnd = bytesEnd;
            this.fileInfo = fileInfo;
            receivedBytes = 0;
            totalBytes = bytesEnd - bytesStart;
            generalBuffer = new byte[totalBytes];

            this.clientTask = clientTask;
            dataContract = new FileDataContract 
            { 
                Filename = fileInfo.Name,
                StartByte = bytesStart,
                EndByte = bytesEnd
            };

            Thread thread = new Thread(startNewDownlaod);
            thread.Start();
        }

        private void startNewDownlaod()
        {
            NetworkStream stream = socket.GetStream();

            string jsonString = JsonConvert.SerializeObject(dataContract);
            byte[] buffer = Encoding.ASCII.GetBytes(jsonString);
            stream.Write(buffer, 0, buffer.Length);

            beginReceive(stream);
        }

        private void beginReceive(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];
            int count;

            while (receivedBytes < totalBytes)
            {
                count = stream.Read(buffer, 0, buffer.Length);

                Array.Copy(buffer, 0, generalBuffer, receivedBytes, count);
                receivedBytes += count;
            }
            submitFinalBuffer();
            stream.Close();
            socket.Close();
        }

        private void submitFinalBuffer()
        {
            clientTask.UpdateFinalFileBuffer(generalBuffer, bytesStart, totalBytes);

        }
    }
}
