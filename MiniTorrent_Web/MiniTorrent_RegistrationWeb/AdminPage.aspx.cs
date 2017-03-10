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
        protected void Page_Load(object sender, EventArgs e)
        {
            //DBHelper db = new DBHelper();

            //List<string> usernameList = db.GetUsernameValues();
            //DataSet ds = new DataSet();
            //UserTable.DataSource = usernameList;
            UserTable.DataSource = new int[]{1,2,3};
            UserTable.DataBind();
        }
    }
}