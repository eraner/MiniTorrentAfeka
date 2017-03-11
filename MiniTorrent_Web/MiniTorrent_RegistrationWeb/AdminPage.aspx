<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminPage.aspx.cs" Inherits="MiniTorrent_RegistrationWeb.AdminPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        Users List:<asp:GridView ID="UserTable" runat="server" OnRowDataBound="UserTable_RowDataBound">
            <Columns>
                <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
            </Columns>
            <SelectedRowStyle BackColor="#0099FF"/>
        </asp:GridView>
        <br />
        <asp:Button ID="RemoveButton" runat="server" Text="Remove Selected User" OnClick="RemoveButton_Click"/>
        <br />
        <br />
        Files List:<asp:GridView ID="FilesGridView" runat="server">
        </asp:GridView>
        <br />
        <asp:TextBox ID="SearchTextBox" runat="server"></asp:TextBox>
        <asp:Button ID="SearchButton" runat="server" Text="Search" OnClick="SearchButton_Click"/>
        <br />
        <br />
        <asp:Label ID="SignedInLabel" runat="server" Text="Singed In Users Number: "></asp:Label>
        <asp:Label ID="NumberSignedInLabel" runat="server" Text=""></asp:Label>
    </div>
    </form>
</body>
</html>
