using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MiniTorrent_GUI
{
    class ServerTask
    {
        private Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public ServerTask()
        {
            //serverSocket.Bind(new IPEndPoint(IPAddress.Any, ))
        }
    }

}
