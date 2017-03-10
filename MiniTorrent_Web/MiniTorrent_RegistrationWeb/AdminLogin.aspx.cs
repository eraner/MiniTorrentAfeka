using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DatabaseHelper;

namespace MiniTorrent_RegistrationWeb
{
    public partial class AdminLogin : System.Web.UI.Page
    {
        private DBHelper helper;
        protected void Page_Load(object sender, EventArgs e)
        {
            helper = new DBHelper();
        }

        protected void LoginButton_Clicked(object sender, EventArgs e)
        {
            string adminName = AdminNameTextBox.Text;
            string password = PasswordTextBox.Text;

           // if (!helper.ValidateAdmin(adminName, password))
            {
                ErrorMsg.Text = "Wrong username or password!";
                return;
            }

        }
    }
}