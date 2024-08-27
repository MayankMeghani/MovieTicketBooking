<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="MovieTicketBooking.LoginPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lblHeading" runat="server" Text="Login Page"></asp:Label>
            <br />
            <asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label>
&nbsp;<asp:TextBox ID="tbEmail" runat="server" TextMode="Email"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="tbEmail" ErrorMessage="Please Enter Email"></asp:RequiredFieldValidator>
&nbsp;<br />
            <asp:Label ID="lblPassword" runat="server" Text="Password"></asp:Label>
&nbsp;<asp:TextBox ID="tbPassword" runat="server" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbPassword" ErrorMessage="Please Enter Password"></asp:RequiredFieldValidator>
            <br />
            <asp:Button ID="btnSignup" runat="server" Text="Login" OnClick="btn_clk_sbm" />
            <br />
            <asp:HyperLink ID="hlLogin" runat="server" NavigateUrl="SignUpPage.aspx">New User? want to register</asp:HyperLink>
            <br />
            <asp:Label ID="lblMessage" runat="server" Text="Label"></asp:Label>
        </div>
    </form>
</body>
</html>
