using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

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
            User u = new User
            {
                Username = username,
                Password = password
            };

            linq_DB.Users.InsertOnSubmit(u);
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

        public bool AddFiles(string name, float size, string ip)
        {
            File f = new File
            {
                name = name,
                size = size,
                userIP = ip,
            };
            linq_DB.Files.InsertOnSubmit(f);

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

        public List<string> GetFilesNameList()
        {
            List<string> filesNamesList = new List<string>();

            List<File> files = linq_DB.Files.ToList();
            foreach (File file in files)
            {
                filesNamesList.Add(file.name);
            }

            return filesNamesList;
        }

        public bool ContainsFile(string filename)
        {
            return linq_DB.Files.Any(item => item.name == filename);
        }



        /*NEED TO IMPLEMENT*/
        public string GetFileInfo(string filename)
        {
            string details = filename;
            linq_DB.Files.Select(item => item.name == filename);
            linq_DB.Files.Count(item => item.name == filename);

            return "";
        }
    }
}
