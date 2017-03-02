using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace DatabaseHelper
{
    public class DBHelper
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
    }
}
