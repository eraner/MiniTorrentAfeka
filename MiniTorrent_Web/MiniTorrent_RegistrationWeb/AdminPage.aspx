<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminPage.aspx.cs" Inherits="MiniTorrent_RegistrationWeb.AdminPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="UserTable" runat="server" OnRowDataBound="UserTable_RowDataBound">
            <Columns>
                <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
            </Columns>
            <SelectedRowStyle BackColor="#0099FF"/>
        </asp:GridView>
        <br />
        <br />
        <asp:Button ID="RemoveButton" runat="server" Text="Remove Selected User" OnClick="RemoveButton_Click"/>
    </div>
    </form>
</body>
</html>
