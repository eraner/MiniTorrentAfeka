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
        private DBHelper helper;
        protected void Page_Load(object sender, EventArgs e)
        {
            helper = new DBHelper();
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            string username = UsernameTextBox.Text.ToString();
            string password = PasswordTextBox.Text.ToString();
            string confirmPassword = ConfirmPasswordTextBox.Text.ToString();

            //List<string> existingUsers = helper.GetUsernameValues();
            //if (existingUsers.Exists(name => name == username))
            //{
            //    //Username exists.
            //    writeErrorToLabel("Usernmae already exists.");
            //    return;
            //}

            string pattern = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,15}$";
            if (!Regex.IsMatch(password, pattern))
            {
                writeErrorToLabel("Password should contain minimum 6 characters, at least one letter and one number.");
                return;
            }
            if (!string.Equals(password, confirmPassword))
            {
                writeErrorToLabel("Passwords fields do not match.");
                return;
            }

            //if (!helper.InsertNewUser(username, password))
            //{
            //    writeErrorToLabel("Could not create new user.");
            //    return;
            //}

            Response.Redirect("RegistrationCompletedPage.aspx");
        }

        private void writeErrorToLabel(string message)
        {
            InternalErrorLabel.Text = message;
        }
    }
}