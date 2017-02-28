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
        const string CREATE_USERS_TABLE = "IF NOT EXISTS (SELECT * FROM sysobjects WHERE NAME='Users' and xtype='U') " +
                                    "CREATE TABLE Users(Username NVARCHAR(50) NOT NULL PRIMARY KEY, Password NVARCHAR(16) NOT NULL)";

        private string connectionString;
        private SqlConnection conn;
        private SqlCommand command;

        public DBHelper()
        {
            //connectionString = ConfigurationManager.ConnectionStrings["DatabaseHelper.Properties.Settings.MiniTorrentDBConnectionString"].ConnectionString;
            connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\MiniTorrentDatabase.mdf;Integrated Security=True";

            using (conn = new SqlConnection(connectionString))
            using (command = new SqlCommand(CREATE_USERS_TABLE, conn))
            {
                conn.Open();

                command.ExecuteNonQuery();
            }
        }

        public List<string> GetUsernameValues()
        {
            string query = "SELECT Username FROM [Users]";
            List<string> usernameValues = new List<string>();

            using (conn = new SqlConnection(connectionString))
            using (command = new SqlCommand(query, conn))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    usernameValues.Add(reader["Username"].ToString());
                }
            }
            return usernameValues;
        }
    }
}
