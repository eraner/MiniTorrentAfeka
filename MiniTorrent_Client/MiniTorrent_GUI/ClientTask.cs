using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MiniTorrent_GUI
{
    class ClientTask
    {
        /*Connection params*/
        private Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private List<Socket> serverSocket = new List<Socket>();
        private List<IPAddress> allIPs = new List<IPAddress>();
        
        /*fileDetails params*/
        private FileDataContract fileDetails;

        /*process params*/
        private int numOfPeers;
        private int bytesReceived;
        private int requestedBytes;
        private int bytesPerRequest;

        public ClientTask(List<string> ipList, FileDataContract fileInfo, ConnectionDetails connDetails)
        {
            bytesReceived = 0;
            requestedBytes = 0;
            clientSocket.Bind(new IPEndPoint(IPAddress.Any, connDetails.IncomingTcpPort));
            fileDetails = fileInfo;
            numOfPeers = ipList.Count;
            //what to do when its not devided well.
            bytesPerRequest = fileDetails.TotalFileSizeInBytes / numOfPeers; 
            foreach (string ip in ipList)
            {
                IPAddress ipAddress = IPAddress.Parse(ip);
                allIPs.Add(ipAddress);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    //SOLVE PORT ISSUE!!
                    socket.BeginConnect(ipAddress, 1000, new AsyncCallback(ConnectCallback), null);
                }
                catch(SocketException)
                {

                }
                serverSocket.Add(socket);

            }

        }

        private void ConnectCallback(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndSend(ar);

            int[] range = getSendRange();
            FileDataContract toSend = new FileDataContract();
            toSend.Filename = fileDetails.Filename;
            toSend.StartByte = range[0];
            toSend.EndByte = range[1];

            string jsonString = JsonConvert.SerializeObject(toSend);
            byte[] data = Encoding.ASCII.GetBytes(jsonString);
            socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
        }

        private void SendCallback(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndSend(ar);
        }

        private static bool isFree = true;
        private int[] getSendRange()
        {
            while (!isFree) ; //wait to a avoid collusions
            isFree = false;

            int[] range = new int[2];
            range[0] = requestedBytes;
            range[1] = requestedBytes + bytesPerRequest;

            if (range[1] >= fileDetails.TotalFileSizeInBytes)
                range[1] = fileDetails.TotalFileSizeInBytes - 1;

            requestedBytes += bytesPerRequest;
            isFree = true;
            return range;
        }



    }
}
