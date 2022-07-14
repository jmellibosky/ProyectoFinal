<%@ Page
    Title="__Ejemplos"
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="__Ejemplos.aspx.cs" 
    MasterPageFile="~/Site.Master"
    Inherits="REApp.Forms.__Ejemplos" 
%>

<%--Head--%>
<asp:Content ID="cHead" ContentPlaceHolderID="cphHead" runat="server">
    <%-- CSS --%>
    <style>

    </style>

    <%-- JS --%>
    <script>
        //$(document).ready(function () {

        //});
    </script>
</asp:Content>

<%-- Body --%>
<asp:Content ID="cBody" ContentPlaceHolderID="cphBody" runat="server">

    <%-- Encabezado --%>
    <div class="row">
        <div class="col-12">
            <h2 class="page-header">
                
                <%-- Título --%>
                <asp:Label ID="lblTitulo" Text="Ejemplos" runat="server" />

                <%-- Botón Nuevo --%>
                <asp:Button ID="btnNuevo" Text="Nuevo" CssClass="btn btn-primary pull-right" runat="server" />
            </h2>
        </div>
    </div>

    <%-- Contenido --%>
    <div class="row">
        <div class="col-12">
            <div class="panel-body">
                <div class="row">
                    <h3>TextBoxes</h3>
                    <%-- Texto Plano --%>
                    <asp:Panel CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-6 col-xs-12" runat="server" ID="pnlTextoPlano">
                        <asp:Label Text="Texto Plano" runat="server" />
                        <asp:TextBox runat="server" ID="txtTextoEjemplo" Class="form-control text-plain" />
                    </asp:Panel>
                    <%-- Sólo Números --%>
                    <asp:Panel CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-6 col-xs-12" runat="server" ID="pnlSoloNumeros">
                        <asp:Label Text="Sólo Números" runat="server" />
                        <asp:TextBox runat="server" ID="txtSoloNumeros" Class="form-control numeric-integer" />
                    </asp:Panel>
                    <%-- Sólo Números Positivos --%>
                    <asp:Panel CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-6 col-xs-12" runat="server" ID="pnlSoloNumerosPositivos">
                        <asp:Label Text="Sólo Números Positivos" runat="server" />
                        <asp:TextBox runat="server" ID="txtSoloNumerosPositivos" Class="form-control numeric-integer-positive" />
                    </asp:Panel>
                    <%-- Combo Ejemplo --%>
                    <asp:Panel CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-6 col-xs-12" runat="server" ID="pnlComboEjemplo">
                        <asp:Label Text="Combo de Valores Predeterminados" runat="server" />
                        <asp:DropDownList runat="server" ID="ddlComboEjemplo" CssClass="form-control select-single">
                            <asp:ListItem Value="1" Text="Opción 1" />
                            <asp:ListItem Value="2" Text="Opción 2" />
                        </asp:DropDownList>
                    </asp:Panel>
                    <%-- Combo Ejemplo --%>
                    <asp:Panel CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-6 col-xs-12" runat="server" ID="Panel1">
                        <asp:Label Text="Combo de BD" runat="server" />
                        <asp:DropDownList runat="server" ID="ddlComboBD" CssClass="form-control select-single" />
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>

</asp:Content>