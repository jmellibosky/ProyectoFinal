<%@ Page
    Title="GestionUsuarios"
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="GestionUsuarios.aspx.cs"
    MasterPageFile="~/Site.Master"
    Inherits="REApp.Forms.GestionUsuarios" %>

<%--Head--%>
<asp:Content ID="cHead" ContentPlaceHolderID="cphHead" runat="server">
    <%-- CSS --%>
    <style>
    </style>

    <%-- JS --%>
    <script>
        //$(document).ready(function () {

        <%-- Funciones Mostrar/NoMostrar --%>
        function display(id_element) {
            document.getElementById(id_element).style.display = 'block';
        }

        function hide(id_element) {
            document.getElementById(id_element).style.display = 'none';
        }
        //});
    </script>

    <%-- Lo robé de Agustin --%>
    <link href="/Content/bootstrap.css" rel="stylesheet" />
    <link href="/Content/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/Site.css" rel="stylesheet" />
    <script src="https://kit.fontawesome.com/ae7187225e.js" crossorigin="anonymous"></script>
    <style>
		body {
		color: #566787;
		background: #f5f5f5;
		font-family: sans-serif;
		font-size: 15px;
		}
		table {
		  table-layout:fixed;
          border: 1px solid black;
		  border-collapse: separate;
		  border-spacing: 2px;
		  border-color: gray;
          
          width:50%;
		}
        thead th:nth-child(1) {
          width: 5%;
        }
        thead th:nth-child(2) {
          width: 20%;
        }
        thead th:nth-child(3) {
          width: 20%;
        }
        thead th:nth-child(4) {
          width: 20%;
        }
        thead th:nth-child(5) {
          width: 10%;
        }
        th, td {
          padding: 2px;
        }
        button{
          background-color: #343a40;
          border: none;
          color: white;
          padding: 7px 16px;
          text-align: center;
          text-decoration: none;
          display: inline-block;
          font-size: 16px;
          margin: 4px 2px;
          cursor: pointer;
        }

        #InsertUser{
            display: none;
        }
        #UpdateUser{
            display: none;
        }
       
    </style>

</asp:Content>

<%-- Body --%>
<asp:Content ID="cBody" ContentPlaceHolderID="cphBody" runat="server">

  
    <%-- Encabezado --%>
    <div class="row">
        <div class="col-12">
            <h2 class="page-header">
                <asp:Label ID="Label1" Text="Gestión de Usuarios" runat="server" />

                <div style="text-align: end;">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnNuevo" Text="Nuevo" CssClass="btn btn-primary" runat="server" />
                            <asp:Button ID="btnVolver" Text="Volver al Listado" Visible="false" CssClass="btn btn-info" runat="server" OnClick="btnVolver_Click"/>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </h2>
        </div>
    </div>

    <%-- ejemplo Joaquin --%>
    <asp:UpdatePanel ID="upForm" runat="server" Visible="true">
        <%-- Contenido --%>
        <ContentTemplate>
            <%-- Grilla Usuarios --%>
            <asp:Panel ID="pnlGvUsuarios" runat="server" Visible="true">
                <div class="row" visible="true">
                    <div class="col-12" visible="true">
                        <br />
                        <div class="panel-body" visible="true">
                            <br />
                            <div class="row" visible="true">
                                <asp:UpdatePanel ID="upUsuarios" Style="width: 100%;" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional" Visible="true">
                                    <ContentTemplate>
                                        <asp:GridView Visible="true" CssClass="text-center"
                                            ID="gvUsuarios"
                                            runat="server"
                                            OnRowCommand="gvUsuarios_RowCommand"
                                            AutoGenerateColumns="false"
                                            EmptyDataText="No hay datos."
                                            Width="100%">
                                            <Columns>
                                                <asp:BoundField HeaderText="IdUsuario" DataField="IdUsuario" />
                                                <asp:BoundField HeaderText="Nombre" DataField="Nombre" />
                                                <asp:BoundField HeaderText="Apellido" DataField="Apellido" />
                                                <asp:BoundField HeaderText="DNI" DataField="DNI" />
                                                <asp:BoundField HeaderText="Email" DataField="Email"/>
                                                <asp:BoundField HeaderText="Telefono" DataField="Telefono" />
                                                <asp:BoundField HeaderText="NombreRol" DataField="NombreRol" />
                                                <asp:TemplateField HeaderText="Acciones" >
                                                    <ItemTemplate>
                                                        <%-- Botón Mostrar Usuario --%>
                                                        <asp:LinkButton ID="btnDisplayUser" CommandName="DisplayUser" CommandArgument='<%# Eval("IdUsuario") %>' runat="server">
                                                            <i class="fa-solid fa-eye"></i>    
                                                        </asp:LinkButton>
                                                        <%-- Botón Modificar Usuario --%>
                                                        <asp:LinkButton ID="btnUpdateUser" CommandName="UpdateUser" CommandArgument='<%# Eval("IdUsuario") %>' runat="server">
                                                            <i class="fa-solid fa-pen"></i> 
                                                        </asp:LinkButton>
                                                        <%-- Botón Eliminar Usuario --%>
                                                        <asp:LinkButton ID="btnDeleteUser" CommandName="DeleteUser" CommandArgument='<%# Eval("IdUsuario") %>' runat="server">
                                                            <i class="fa-solid fa-trash"></i>    
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <%-- ABM Usuarios --%>
           <asp:Panel ID="pnlABM" runat="server" Visible="false">
                <div class="row">
                    <div class="col-12">
                        <br />
                        <div class="panel-body">
                            <asp:UpdatePanel ID="upModalABM" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                <ContentTemplate>
                                    <div class="col-12">
                                        <br />
                                        <div class="row" id="TituloABM">
                                            <h6>Informacion Usuario</h6>
                                        </div>
                                        <div class="row">
                                            <asp:Panel ID="pnlNombre" CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Nombre" runat="server" />
                                                <asp:TextBox runat="server" ID="txtNombre" CssClass="form-control" />
                                            </asp:Panel>

                                            <asp:Panel ID="pnlApellido" CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Apellido" runat="server" />
                                                <asp:TextBox runat="server" ID="txtApellido" CssClass="form-control" />
                                            </asp:Panel>

                                            <asp:Panel ID="pnlRol" CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Rol" runat="server" />
                                                <asp:TextBox runat="server" ID="txtRol" CssClass="form-control" />
                                            </asp:Panel>
                                        </div>
                                        <div class="row">
                                            <asp:Panel ID="pnlDNI" CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="DNI" runat="server" />
                                                <asp:TextBox runat="server" ID="txtDNI" CssClass="form-control" />
                                            </asp:Panel>

                                            <asp:Panel ID="pnlTipoDni" CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Tipo DNI" runat="server" />
                                                <asp:TextBox runat="server" ID="txtTipoDni" CssClass="form-control" />
                                            </asp:Panel>

                                            <asp:Panel ID="pnlFechaNacimiento" CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Fecha de Nacimiento" runat="server" />
                                                <asp:TextBox runat="server" ID="txtFechaNacimiento" CssClass="form-control" />
                                            </asp:Panel>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <h6>Datos de Contacto</h6>
                                        </div>
                                        <div class="row">

                                            <asp:Panel ID="pnlCorreo" CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Correo Electrónico" runat="server" />
                                                <asp:TextBox runat="server" ID="txtCorreo" CssClass="form-control" />
                                            </asp:Panel>

                                            <asp:Panel ID="pnlTelefono" CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Teléfono" runat="server" />
                                                <asp:TextBox runat="server" ID="txtTelefono" CssClass="form-control" />
                                            </asp:Panel>

                                        </div>
                                        <hr />
                                        <div class="row">
                                            <div style="justify-content:center;">
                                                <asp:Button ID="btnGuardar" Text="Guardar" CssClass="btn btn-success" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <%-- Fin ABM Usuarios --%>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
