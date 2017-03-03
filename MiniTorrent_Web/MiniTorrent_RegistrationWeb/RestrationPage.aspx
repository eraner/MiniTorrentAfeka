<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RestrationPage.aspx.cs" Inherits="MiniTorrent_RegistrationWeb.RestrationPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Label ID="UsernameLabel" runat="server" Text="Username: "></asp:Label>
        <asp:TextBox ID="UsernameTextBox" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Username is required." ControlToValidate="UsernameTextBox"></asp:RequiredFieldValidator>
        <br />
        <asp:Label ID="PasswordLabel" runat="server" Text="Password: "></asp:Label>
        <asp:TextBox ID="PasswordTextBox" runat="server"></asp:TextBox>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="PasswordTextBox" ErrorMessage="Password should contain minimum 6 characters, at least one letter and one number " ValidationExpression="^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,15}$"></asp:RegularExpressionValidator>
        <br />
        <asp:Label ID="ConfirmPasswordLabel" runat="server" Text="Confirm Password: "></asp:Label>
        <asp:TextBox ID="ConfirmPasswordTextBox" runat="server" />
        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Passwords fields do not match." Font-Italic="False" ControlToCompare="PasswordTextBox" ControlToValidate="ConfirmPasswordTextBox"></asp:CompareValidator>
        <br />
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
        <br />
        <asp:Button ID="SubmitButton" runat="server" Text="Submit" OnClick="SubmitButton_Click"/>

    
        <br />
        <br />
        <asp:Label ID="InternalErrorLabel" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>

    
        <br />
        
    
    </div>
    </form>
</body>
</html>
