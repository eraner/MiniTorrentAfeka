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
        DBHelper dbHelper;

        public bool signin(string jsonString)
        {
            JsonItems items = JsonConvert.DeserializeObject<JsonItems>(jsonString);
            dbHelper = new DBHelper();

            string username = items.Username;
            string password = items.Password;
            if (!Authenticate(username, password))
                return false;

            try
            {
                /*setting params for database*/
                List<FileInfo> userFiles = items.AllFiles;
                string ip = items.Ip;
                string port = items.Port;

                /*adding user and files to the database.*/
                dbHelper.SignInUser(username, ip, port);
                foreach (FileInfo file in userFiles)
                {
                    dbHelper.AddFiles(file.name, file.size, ip);
                }
            }
            catch
            {
                return false;
            }

            //NOTIFY OTHER CLIENTS FOR A CHANGE!!
            
            return true;
        }

        public bool Authenticate(string username, string password)
        {
            if (dbHelper == null)
                new DBHelper();

            return dbHelper.ContainsUsernamePassword(username, password);
        }


        public string RequestAFile(string jsonString)
        {
            JsonItems items = JsonConvert.DeserializeObject<JsonItems>(jsonString);
            if (dbHelper == null)
                new DBHelper();

            if (!Authenticate(items.Username, items.Password))
                return string.Empty;

            if (items.AllFiles.Count != 1)
                return string.Empty;

            FileInfo file = items.AllFiles[0];
            if (!dbHelper.ContainsFile(file.name))
                return string.Empty;



            return "";
        }
    }

    
}
