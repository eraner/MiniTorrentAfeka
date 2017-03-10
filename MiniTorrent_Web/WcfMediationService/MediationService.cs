using System;
using System.Collections.Generic;
using MiniTorrent_MediationServerContract;
using System.ServiceModel;
using System.ServiceModel.Channels;
using DatabaseHelper;
using Newtonsoft.Json;

namespace WcfMediationService
{
    public class MediationService : IMediationServerContract
    {
        private DBHelper dbHelper;
        public DBHelper DbHelper {
            get
            {
                return (dbHelper == null) ? dbHelper = new DBHelper() : dbHelper;
            }
        }

        public bool SingIn(string jsonString)
        {
            JsonItems items = JsonConvert.DeserializeObject<JsonItems>(jsonString);

            string username = items.Username;
            string password = items.Password;
            if (!Authenticate(username, password))
                return false;

            if (DbHelper.IsAlreadySignedIn(username))
                return false;

            try
            {
                /*setting params for database*/
                List<FileDetails> userFiles = items.AllFiles;
                string ip = items.Ip;
                string port = items.Port;

                /*adding user and files to the database.*/
                DbHelper.SignInUser(username, ip, port);
                DbHelper.AddFiles(userFiles, ip);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool Authenticate(string username, string password)
        {
            return DbHelper.ContainsUsernamePassword(username, password);
        }

        public string RequestAFile(string jsonString)
        {
            JsonItems items = JsonConvert.DeserializeObject<JsonItems>(jsonString);

            if (!Authenticate(items.Username, items.Password) || (items.AllFiles.Count != 1))
                return string.Empty;

            FileDetails file = items.AllFiles[0];
            if (!DbHelper.ContainsFile(file.Name))
                return string.Empty;

            FileDetails details = DbHelper.GetFileInfo(file.Name);

            return JsonConvert.SerializeObject(details);
        }

        public bool SignOut(string jsonString)
        {
            JsonItems items = JsonConvert.DeserializeObject<JsonItems>(jsonString);

            if (!Authenticate(items.Username, items.Password))
                return false;

            if (!DbHelper.SignoutUser(items.Username))
                return false;

            if (!DbHelper.ClearUserFiles(items.Ip))
                return false;

            return true;
        }

        public string GetAvailableFiles()
        {
            List<FileDetails> allFiles = DbHelper.GetFilesDetailsList();

            return JsonConvert.SerializeObject(allFiles);
        }

        public string GetIpListForAFile(string filename)
        {
            List<IpPort> allIps = DbHelper.GetFileIPs(filename);
            return JsonConvert.SerializeObject(allIps);
        }

        public bool UpdateUserFiles(string jsonString)
        { 
            JsonItems items = JsonConvert.DeserializeObject<JsonItems>(jsonString);

            return DbHelper.AddFiles(items.AllFiles, items.Ip);
        }
    }  
}
