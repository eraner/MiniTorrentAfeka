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
        /// checks if the file exists and returns json string containing delails of the file.
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns>
        ///     returns empty string if the process fails.
        /// </returns>
        [OperationContract]
        string RequestAFile(string jsonString);

        /// <summary>
        /// gets json string, with username and password, and mark this user as unavailable including his files.
        /// </summary>
        /// <param name="jasonString"></param>
        /// <returns></returns>
        [OperationContract]
        bool SignOut(string jsonString);
        
    }

    public class JsonItems
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Ip { get; set; }
        public string Port { get; set; }
        public List<FileDetails> AllFiles { get; set; }
    }

    public class FileDetails
    {
        public string Name { get; set; }
        public float Size { get; set; }
        public int Count { get; set; }
    }
}
