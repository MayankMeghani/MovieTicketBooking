<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TicketBooking.aspx.cs" Inherits="MovieTicketBooking.MovieTicketBooking" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            
            <asp:Label ID="lblHeading" runat="server" Text="Ticket Booking Form"></asp:Label>
            <br />
            <asp:Label ID="lbSeats" runat="server" Text="No of Seats"></asp:Label>
&nbsp;<asp:TextBox ID="tbSeats" runat="server"></asp:TextBox>
            <asp:RangeValidator ID="rvSeats" runat="server" ErrorMessage="Seats not available" MinimumValue="1" ControlToValidate="tbSeats" Type="Integer"></asp:RangeValidator>
            <br />
            <asp:Button ID="btnBook" runat="server" Text="Book" OnClick="btn_sbm_book" />
            <br />
            <asp:Label ID="lblMessage" runat="server" Text="Label" Visible="False"></asp:Label>
            
        </div>
    </form>
</body>
</html>
