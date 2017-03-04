using System.ServiceModel;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MiniTorrent_MediationServerContract
{
    [ServiceContract]
    public interface IMediationServerContract
    {
        /// <summary>
        /// signin a user and adding his available files to the database.
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        [OperationContract]
        bool signin(string jsonString);

        /// <summary>
        /// checking for existing username+password at the database.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [OperationContract]
        bool Authenticate(string username, string password);

        [OperationContract]
        string RequestAFile(string jsonString);
            
    }
}
