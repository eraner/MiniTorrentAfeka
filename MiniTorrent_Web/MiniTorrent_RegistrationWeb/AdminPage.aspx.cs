using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DatabaseHelper;
using MiniTorrent_MediationServerContract;

namespace MiniTorrent_RegistrationWeb
{
    public partial class AdminPage : System.Web.UI.Page
    {
        private DBHelper db;
        private List<string> usernameList;
        private List<FileDetails> filesList;
        protected void Page_Load(object sender, EventArgs e)
        {
            db = new DBHelper();
            
            //user init
            usernameList = db.GetUsernameValues();
            UserTable.DataSource = usernameList;
            UserTable.DataBind();
            //files init
            filesList = db.GetFilesDetailsList();
            FilesGridView.DataSource = filesList;
            FilesGridView.DataBind();

            //signed in init
            SetSignedInUsers();
        }

        private void SetSignedInUsers()
        {
            int num = db.GetSignedInNumber();
            NumberSignedInLabel.Text = num.ToString();
        }

        protected void RemoveButton_Click(object sender, EventArgs e)
        {
            if (usernameList.Count == 0 || UserTable.SelectedIndex == -1)
                return;
            string usernameToDelete = UserTable.SelectedRow.Cells[1].Text;
            db.RemoveUser(usernameToDelete);
            usernameList.Remove(usernameToDelete);
            UserTable.DataBind();
            UserTable.SelectRow(-1);
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            filesList = db.GetFilesDetailsList();
            string searchStr = SearchTextBox.Text;
            filesList = filesList.FindAll(f => f.Name.Contains(searchStr));
            FilesGridView.DataSource = filesList;
            FilesGridView.DataBind();
        }

        protected void UserTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Text = "Users";
            }
        }
    }
}