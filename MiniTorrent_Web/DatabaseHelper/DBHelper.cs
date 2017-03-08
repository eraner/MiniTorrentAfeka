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
            ClearUserFiles(ip);
            foreach (FileDetails file in files)
            {
                
                File current = new File
                {
                    name = file.Name,
                    size = file.Size,
                    userIP = ip
                };
                linq_DB.Files.InsertOnSubmit(current);
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
                         select file.name).Distinct();
            foreach (string name in files)
            {
                filesDetailsList.Add(GetFileInfo(name));
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

            var query = (from user in linq_DB.Signins
                                where user.username == username
                                select user);
            
            if (query.Count() == 0)
                return false;

            var userToDelete = query.First();
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

        public List<IpPort> GetFileIPs(string filename)
        {
            var allFiles = (from file in linq_DB.Files
                          where file.name == filename
                          select file);

            List<IpPort> IpPortList = new List<IpPort>();
            foreach (var file in allFiles)
            {
                string port;
                try
                {
                    port = linq_DB.Signins.Single(s => s.ip == file.userIP).port;
                }
                catch(Exception)
                {
                    //No port was found continue to the next file. 
                    continue;
                }

                IpPortList.Add(new IpPort
                {
                    IpAddress = file.userIP,
                    Port = port
                });
            }


            return IpPortList;
        }

        public bool IsAlreadySignedIn(string username)
        {
            return (from signed in linq_DB.Signins
                           where signed.username == username
                        select signed).Count() >= 1;
        }
    }
}
