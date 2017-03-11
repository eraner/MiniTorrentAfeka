using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DatabaseHelper;

namespace MiniTorrent_RegistrationWeb
{
    public partial class AdminPage : System.Web.UI.Page
    {
        DBHelper db;
        List<string> usernameList;
        protected void Page_Load(object sender, EventArgs e)
        {
            db = new DBHelper();

            usernameList = db.GetUsernameValues();
            UserTable.DataSource = usernameList;

          //  UserTable.Columns[1].HeaderText = "Users";
            UserTable.DataBind();

        }

        protected void RemoveButton_Click(object sender, EventArgs e)
        {
            if (usernameList.Count == 0)
                return;
            string usernameToDelete = UserTable.SelectedRow.Cells[1].Text;
            db.RemoveUser(usernameToDelete);
            usernameList.Remove(usernameToDelete);
            UserTable.DataBind();
            UserTable.SelectRow(-1);
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