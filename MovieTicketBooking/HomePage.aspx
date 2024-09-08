<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="MovieTicketBooking.HomePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="height: 299px">
    <form id="form1" runat="server">
        <div accesskey="rblMovie">
            <asp:Label ID="lblWelcome" runat="server" Text="Label"></asp:Label>
            <br />
            <br />
            <asp:Label ID="lblHeading" runat="server" Text="Movies"></asp:Label>
            <br />
            <asp:RadioButtonList ID="rblMovies" runat="server">
            </asp:RadioButtonList>
            <asp:Literal ID="ltlMovieList" runat="server"></asp:Literal>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="rblMovies" ErrorMessage="Select movie">Please select movie</asp:RequiredFieldValidator>
            <br />
            <asp:Button ID="btnView" runat="server" OnClick="btnView_Click" Text="Explore" />
            <br />
            <br />
            <asp:Panel ID="pnlAdmin" runat="server" Visible="False">
                <asp:HyperLink ID="hlAddMovie" runat="server" NavigateUrl="AddMovieForm.aspx">Add Movie</asp:HyperLink>
                &nbsp;<asp:HyperLink ID="hlAddMovie0" runat="server" NavigateUrl="UpdateMovieForm.aspx">Update Movie</asp:HyperLink>
            </asp:Panel>
            <br />
            <asp:HyperLink ID="hlSchedule" runat="server" NavigateUrl="Schedule.aspx">View Schedule</asp:HyperLink>
&nbsp;<br />
            <br />
            <asp:HyperLink ID="hlBookings" runat="server" NavigateUrl="Bookings.aspx">Booking Records</asp:HyperLink>
            <br />
            <br />
            <asp:Button ID="btnLogOut" runat="server" CausesValidation="False" OnClick="btn_clk_LogOut" Text="Logout" />
        </div>
    </form>
</body>
</html>
