<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="REApp.Forms.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Label id="txtBienvenida" CssClass="h1" runat="server" Text="Bienvenido al Sistema" ForeColor="Green"></asp:Label>
    <br />
    <asp:Label id="txtUser" CssClass="h1" runat="server" ></asp:Label>
    <asp:Label id="txtSigno" CssClass="h1" runat="server" Text="!"></asp:Label>
</asp:Content>
