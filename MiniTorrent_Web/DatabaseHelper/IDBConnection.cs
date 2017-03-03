using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseHelper
{
    interface IDBConnection
    {
        /// <summary>
        /// gets all the usernames exists in the DB.
        /// </summary>
        /// <returns> List contains string of names.</returns>
        List<string> GetUsernameValues();

        /// <summary>
        /// insert new user to the DB by it's username and password.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>true - when successful
        ///          false - when fails</returns>
        bool InsertNewUser(string username, string password);
    }
}
