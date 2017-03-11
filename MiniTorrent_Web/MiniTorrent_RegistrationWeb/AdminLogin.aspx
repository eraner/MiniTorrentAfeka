<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminLogin.aspx.cs" Inherits="MiniTorrent_RegistrationWeb.AdminLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Admin Login:<br />
        <asp:Table runat="server" ID="Table1">
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label ID="AdminNameLabel" runat="server" Text="Username: "></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="AdminNameTextBox" runat="server"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
            <asp:Label ID="PasswordLabel" runat="server" Text="Password: "></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
        <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
        

           </asp:Table>
        <br />
        <asp:Button ID="LoginButton" runat="server" OnClick="LoginButton_Clicked" Text="Login" />
        <br />
        <br />
        <asp:Label ID="ErrorMsg" runat="server" Text="" Font-Bold="True" ForeColor="Red"></asp:Label>
        </div></form></body></html>