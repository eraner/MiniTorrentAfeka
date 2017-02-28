using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DatabaseHelper;

namespace MiniTorrent_RegistrationWeb
{
    public partial class RestrationPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            string username = UsernameTextBox.Text.ToString();
            string password = PasswordTextBox.Text.ToString();
            string confirmPassword = ConfirmPasswordTextBox.Text.ToString();
            SubmitButton.Text = "Clicked";

            //Check that the user name is not taken already.
            DBHelper helper = new DBHelper();
            List<string> existingUsers = helper.GetUsernameValues();


            string pattern = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,15}$";
            if (!Regex.IsMatch(password, pattern))
            {
                // Do something.
                return;
            }


        }
    }
}