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
        /// gets json string which is JsonItems object signin a user and adding his available files to the database.
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        [OperationContract]
        bool SingIn(string jsonString);

        /// <summary>
        /// checking for existing username+password at the database.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [OperationContract]
        bool Authenticate(string username, string password);
        
        /// <summary>
        /// gets json string which is JsonItems object contains username, password and one file inside the list. 
        /// checks if the file exists and returns json string containing Filedetails object of the file - name, size, count.
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns>
        ///     returns empty string if the process fails.
        /// </returns>
        [OperationContract]
        string RequestAFile(string jsonString);

        /// <summary>
        /// gets json string which is JsonItems object, with username password and ip, and mark this user as unavailable including his files.
        /// </summary>
        /// <param name="jasonString"></param>
        /// <returns></returns>
        [OperationContract]
        bool SignOut(string jsonString);

        /// <summary>
        /// Returns available files in json string with list of FileDetails object.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        string GetAvailableFiles();

        /// <summary>
        /// returns a json string which is a list of IpPort objects for the requested filename.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        [OperationContract]
        string GetIpListForAFile(string filename);
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

    public class IpPort
    {
        public string IpAddress { get; set; }
        public string Port { get; set; }
    }
}
