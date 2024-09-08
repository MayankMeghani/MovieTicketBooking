<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddMovieForm.aspx.cs" Inherits="MovieTicketBooking.AddMovie" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lblHeading" runat="server" Text="Add Movie"></asp:Label>
        </div>
        <asp:Label ID="lblTitle" runat="server" Text="Title"></asp:Label>
&nbsp;<asp:TextBox ID="tbTitle" runat="server"></asp:TextBox>
        <br />
        <asp:Label ID="lblGenre" runat="server" Text="Genre"></asp:Label>
&nbsp;<asp:TextBox ID="tbGenre" runat="server"></asp:TextBox>
        <br />
        <asp:Label ID="lblDuration" runat="server" Text="Duration"></asp:Label>
&nbsp;<asp:TextBox ID="tbDuration" runat="server"></asp:TextBox>
        <br />
        <asp:Label ID="lblDate" runat="server" Text="Release Date"></asp:Label>
&nbsp;<asp:TextBox ID="tbDate" runat="server" TextMode="Date"></asp:TextBox>
        <br />
        Poster
        <asp:FileUpload ID="FileUpload" runat="server" OnDataBinding="btn_sbt_add" />
        <br />
        <br />
        <asp:Button ID="btnAdd" runat="server" Text="Add Movie" OnClick="btn_sbt_add" />
        <br />
        <asp:Label ID="lblMessage" runat="server" Text="Label"></asp:Label>
    </form>
</body>
</html>
