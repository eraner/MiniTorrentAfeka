using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using MiniTorrent_MediationServerContract;

namespace DatabaseHelper
{
    public class DBHelper : IDBConnection
    {
        private LinqToDBDataContext linq_DB;

        public DBHelper()
        {
            linq_DB = new LinqToDBDataContext();
        }

        public List<string> GetUsernameValues()
        {
            List<string> usernameValues = new List<string>();

            List<User> users = linq_DB.Users.ToList();
            foreach(User user in users)
            {
                usernameValues.Add(user.Username);
            }
            
            return usernameValues;
        }

        public bool SignUpNewUser(string username, string password)
        {
            linq_DB.Users.InsertOnSubmit(new User
            {
                Username = username,
                Password = password
            });
            try
            {
                linq_DB.SubmitChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool ContainsUsernamePassword(string username, string password)
        {
            return linq_DB.Users.Contains(new User
            {
                Username = username,
                Password = password
            });
        }

        public bool SignInUser(string username, string ip, string port)
        {
            Signin s = new Signin
            {
                username = username,
                ip = ip,
                port = port
            };
            linq_DB.Signins.InsertOnSubmit(s);

            try
            {
                linq_DB.SubmitChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool AddFiles(List<FileDetails> files, string ip)
        {
            foreach(FileDetails file in files)
            {
                linq_DB.Files.InsertOnSubmit(new File
                {
                    name = file.Name,
                    size = file.Size,
                    userIP = ip
                });
            }
            try
            {
                linq_DB.SubmitChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public List<FileDetails> GetFilesDetailsList()
        {
            List<FileDetails> filesDetailsList = new List<FileDetails>();

            var files = (from file in linq_DB.Files
                            select file).GroupBy(key => key.name);
            foreach (File file in files)
            {
                filesDetailsList.Add(GetFileInfo(file.name));
            }
            return filesDetailsList;
        }

        public bool ContainsFile(string filename)
        {
            return (from file in linq_DB.Files
                    where file.name == filename
                    select file).Count() >= 1;
        }

        public FileDetails GetFileInfo(string filename)
        {
            FileDetails details = new FileDetails();
            details.Name = filename;
            details.Size = (float)linq_DB.Files.First(item => item.name == filename).size;
            details.Count = linq_DB.Files.Count(item => item.name == filename);

            return details;
        }

        public bool SignoutUser(string username)
        {

            var userToDelete = (from user in linq_DB.Signins
                                where user.username == username
                                select user).First();
            if (userToDelete == null)
                return false;

            linq_DB.Signins.DeleteOnSubmit(userToDelete);

            try
            {
                linq_DB.SubmitChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool ClearUserFiles(string ip)
        {
            var filesToDelete = (from files in linq_DB.Files
                                 where files.userIP == ip
                                 select files);
            foreach (var fileToDelete in filesToDelete)
                linq_DB.Files.DeleteOnSubmit(fileToDelete);

            try
            {
                linq_DB.SubmitChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public List<string> GetFileIPs(string filename)
        {
            var allIPs = (from file in linq_DB.Files
                          where file.name == filename
                          select file);

            List<string> filesIP = new List<string>();
            foreach (var ip in allIPs)
                filesIP.Add(ip.userIP);

            return filesIP;
        }
    }
}
