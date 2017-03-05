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
        List<string> GetFileIPs(string filename);

        bool IsAlreadySignedIn(string username);
    }
}
