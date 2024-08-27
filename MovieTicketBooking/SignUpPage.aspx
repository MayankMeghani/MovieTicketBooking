<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUpPage.aspx.cs" Inherits="MovieTicketBooking.SignUp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lblHeading" runat="server" Text="SignUp Page"></asp:Label>
            <br />
            <asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label>
&nbsp;<asp:TextBox ID="tbEmail" runat="server" TextMode="Email"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="tbEmail" ErrorMessage="Please Enter Email"></asp:RequiredFieldValidator>
            <br />
            <asp:Label ID="lblName" runat="server" Text="Name"></asp:Label>
&nbsp;<asp:TextBox ID="tbName" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="tbName" ErrorMessage="Please Enter Name"></asp:RequiredFieldValidator>
            <br />
            <asp:Label ID="lblPassword" runat="server" Text="Password"></asp:Label>
&nbsp;<asp:TextBox ID="tbPassword" runat="server" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbPassword" ErrorMessage="Please Enter Password"></asp:RequiredFieldValidator>
            <br />
            <asp:Label ID="lblRole" runat="server" Text="Role"></asp:Label>
&nbsp;<asp:RadioButton ID="rbAdmin" runat="server" GroupName="Role" Text="Admin" />
            <asp:RadioButton ID="rbUser" runat="server" Checked="True" GroupName="Role" Text="User" />
            <br />
            <asp:Button ID="btnSignup" runat="server" Text="Register" OnClick="btn_clk_sbm" />
            <br />
            <asp:HyperLink ID="hlLogin" runat="server" NavigateUrl="LoginPage.aspx">Already registered? want to Login</asp:HyperLink>
            <br />
            <asp:Label ID="lblMessage" runat="server" Text="Label"></asp:Label>
        </div>
    </form>
</body>
</html>
