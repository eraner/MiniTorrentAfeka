using System;
using System.Collections.Generic;
using MiniTorrent_MediationServerContract;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace WcfMediationService
{
    public class MediationService : IMediationServerContract
    {
        public bool Authenticate(string username, string password)
        {
            throw new NotImplementedException();
        }

        public List<string> GetFilesNamesList()
        {
            throw new NotImplementedException();
        }

        public string GetName()
        {
            return getConnectedClientIP();
        }

        public void PostOwnedFilesNamesList(List<string> filesnames)
        {
            throw new NotImplementedException();
        }

        private string getConnectedClientIP()
        {
            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint =
                prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string ip = endpoint.Address;

            return ip;
        }
    }
}
