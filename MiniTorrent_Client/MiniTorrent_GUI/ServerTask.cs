using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MiniTorrent_GUI
{
    class ServerTask
    {
        ConnectionDetails connDetails;
        private byte[] buffer = new byte[1024];
        private List<Socket> clientSockets = new List<Socket>();
        private Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public ServerTask(ConnectionDetails connDetails)
        {
            this.connDetails = connDetails;
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, connDetails.OutgoingTcpPort));
            serverSocket.Listen(4);
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        // waiting for clients callback
        private void AcceptCallback(IAsyncResult ar)
        {
            Socket socket = serverSocket.EndAccept(ar);
            clientSockets.Add(socket);
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        // recieve data from client.
        private void ReceiveCallback(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            int received = socket.EndReceive(ar);
            byte[] dataBuffer = new byte[received];
            Array.Copy(buffer, dataBuffer, received);

            /*Message from client*/
            string receivedMsg = Encoding.ASCII.GetString(dataBuffer);
            FileDataContract fileDetails = (FileDataContract)JsonConvert.DeserializeObject(receivedMsg);
            byte[] sourceFile = File.ReadAllBytes(connDetails.PublishedFilesSource + fileDetails.Filename);

            int bufSize = fileDetails.EndByte - fileDetails.StartByte;
            if(bufSize <= 0)
                return;
            byte[] data = new byte[bufSize];
            Array.Copy(sourceFile, fileDetails.StartByte, data, 0, bufSize);
            
            socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
        }

        private void SendCallback(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndSend(ar);
            socket.Close();
        }
    }

}
