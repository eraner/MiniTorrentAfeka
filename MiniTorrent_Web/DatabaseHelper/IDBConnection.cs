using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniTorrent_MediationServerContract;

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
        bool AddFiles(List<FileDetails> files, string ip);

        /// <summary>
        /// returns a list of strings for files.
        /// </summary>
        /// <returns></returns>
        List<FileDetails> GetFilesDetailsList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>true - when successful
        ///          false - when fails</returns>
        bool ContainsFile(string filename);

        /// <summary>
        /// returns FileDetails object with file details.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        FileDetails GetFileInfo(string filename);

        /// <summary>
        /// signing out a user from the signin table with username+password.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool SignoutUser(string username);

        /// <summary>
        /// clearing the user's files from DB.
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        bool ClearUserFiles(string ip);

        /// <summary>
        /// returns a list of strings containing the ips of the users which are holding this file.
        /// </summary>
        /// <returns></returns>
        List<IpPort> GetFileIPs(string filename);

        /// <summary>
        /// Checking whether the Username is already singed in.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        bool IsAlreadySignedIn(string username);

        /// <summary>
        /// Checking the credentials of the admin details by adminname and password.
        /// </summary>
        /// <param name="adminname"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool ValidateAdmin(string adminname, string password);

        /// <summary>
        /// Removes a User from the Users table, by it's username. return true when succeeded, 
        /// false when there aren't such user or more than one or failed to delete it.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        bool RemoveUser(string username);

        /// <summary>
        /// gets number of users signed in.
        /// </summary>
        /// <returns></returns>
        int GetSignedInNumber();
    }
}
