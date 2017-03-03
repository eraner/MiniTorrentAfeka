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
            //string query = "SELECT Username FROM [Users]";
            List<string> usernameValues = new List<string>();

            List<User> users = linq_DB.Users.ToList();
            foreach(User user in users)
            {
                usernameValues.Add(user.Username);
            }
           
            return usernameValues;
        }

        public bool InsertNewUser(string username, string password)
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
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
