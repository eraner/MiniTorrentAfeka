using System.ServiceModel;
using System;
using System.Collections.Generic;

namespace MiniTorrent_MediationServerContract
{
    [ServiceContract]
    public interface IMediationServerContract
    {
        [OperationContract]
        string GetName();

        [OperationContract]
        bool Authenticate(string username, string password);

        [OperationContract]
        List<string> GetFilesNamesList();

        [OperationContract]
        void PostOwnedFilesNamesList(List<string> filesnames);

    }
}
