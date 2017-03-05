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
        
        /// <summary>
        /// checks if the file exists and returns delails of the file.
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns>
        ///     returns empty string if the process fails.
        /// </returns>
        [OperationContract]
        string RequestAFile(string jsonString);


        
    }

    public class JsonItems
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Ip { get; set; }
        public string Port { get; set; }
        public List<FileInfo> AllFiles { get; set; }
    }

    public class FileInfo
    {
        public string name { get; set; }
        public float size { get; set; }
    }
}
