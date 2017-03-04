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
        bool SignUpNewUser(string username, string password);

        /// <summary>
        /// checks if username+password exists in DB
        /// </summary>
        /// <param name="username"></param>
        /// <returns>
        ///     true - if contains
        ///     false - if not contains
        /// </returns>
        bool ContainsUsernamePassword(string username, string password);

        /// <summary>
        /// adding user which is authenticated to the database.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="ip"></param>
        /// <returns>true - when successful
        ///          false - when fails</returns>
        bool SignInUser(string username, string ip, string port);

        /// <summary>
        /// add files to table.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        bool AddFiles(string name, float size, string ip);

        /// <summary>
        /// returns a list of strings for files.
        /// </summary>
        /// <returns></returns>
        List<string> GetFilesNameList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>true - when successful
        ///          false - when fails</returns>
        bool ContainsFile(string filename);

        /// <summary>
        /// returns string with "<filename> <size> <count>"
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        string GetFileInfo(string filename);
    }
}
