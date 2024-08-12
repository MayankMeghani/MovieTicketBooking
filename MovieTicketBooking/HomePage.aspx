<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="MovieTicketBooking.HomePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="height: 299px">
    <form id="form1" runat="server">
        <div accesskey="rblMovie">
            <asp:RadioButtonList ID="rblMovies" runat="server" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
            </asp:RadioButtonList>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="rblMovies" ErrorMessage="Select movie">Please select movie</asp:RequiredFieldValidator>
            <br />
            <asp:Button ID="btnView" runat="server" OnClick="btnView_Click" Text="View" />
        </div>
    </form>
</body>
</html>
