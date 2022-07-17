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

                <%-- Título --%>
                <asp:Label ID="lblTitulo" Text="Gestión de Usuarios" runat="server" class="text-center"/>

            </h2>
        </div>
        </div>

    <%-- Contenido --%>
    <div class="container" style="margin-top:10px;" id="griviewUsers" >

        <div class="row">
            <h3>Listado de Usuarios</h3>
        </div>
                <table class="table table-striped table-hover">
                  <thead>
                        <tr>
                            <th class="text-center" width="50px">ID</th>
                            <th class="text-center">Nombre</th>
                            <th class="text-center">Tipo</th>
                            <th class="text-center">Estado</th>
                            <th class="text-center">DNI</th>
                            <th class="text-center text-nowrap">Acciones</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td class="text-center">1</td>
                            <td class="text-center">Franco</td>
                            <td class="text-center">Administrador</td>
                            <td class="text-center">Activo</td>
                            <td class="text-center">41294371</td>
                            <td class="text-center">
                                <button class="button" type="button"  title="Eliminar">
                                    <i class="fa-solid fa-eye"></i>
                     		    </button>
                                <button class="button" type="button"  title="Modificar" onclick="display('UpdateUser');hide('griviewUsers')">
                                    <i class="fa-solid fa-pen"></i>
                                </button>
							    <button class="button" type="button"  title="Eliminar">
                                    <i class="fa-solid fa-trash"></i>
                     		    </button>
                            </td>
                        </tr>
                    </tbody>
                </table>
        <%-- Botón Nuevo Usuario --%>
        <button class="button" type="button" title="Agregar" onclick="display('InsertUser');hide('griviewUsers')"> <i class="fa-solid fa-square-plus"></i> &ensp; Agregar</button>
      </div>
    
    <%-- Crear nuevo usuario --%>
    <div class="container" style="margin-top:10px;" id="InsertUser">
        <div class="row">
            <h3>Registrar Nuevo Usuario</h3>
        </div>
        <div class="row">
            <%-- ID Usuario --%>
            <asp:Panel CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-6 col-xs-12" runat="server" ID="Panel2" Enabled="false">
                <asp:Label Text="ID Usuario" runat="server" />
                <asp:TextBox runat="server" ID="TextBox1" Class="form-control text-plain" />
            </asp:Panel>
            <%-- Nombre --%>
            <asp:Panel CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-6 col-xs-12" runat="server" ID="Panel3">
                <asp:Label Text="Nombre" runat="server" />
                <asp:TextBox runat="server" ID="TextBox2" Class="form-control numeric-integer" />
            </asp:Panel>
            <%-- Email --%>
            <asp:Panel CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-6 col-xs-12" runat="server" ID="Panel4">
                <asp:Label Text="Email" runat="server" />
                <asp:TextBox runat="server" ID="TextBox3" Class="form-control numeric-integer-positive" />
            </asp:Panel>
        </div>
        <br />
        <div class="row">
            <%-- Tipo Usuario --%>
            <asp:Panel CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-6 col-xs-12" runat="server" ID="Panel5">
                <asp:Label Text="Tipo Usuario" runat="server" />
                <asp:DropDownList runat="server" ID="DropDownList2" CssClass="form-control select-single">
                    <asp:ListItem Value="1" Text="Administrador" />
                    <asp:ListItem Value="2" Text="Operador" />
                    <asp:ListItem Value="3" Text="Afectado" />
                    <asp:ListItem Value="4" Text="Solicitante" />
                </asp:DropDownList>
            </asp:Panel>
            <%-- Tipo DNI --%>
            <asp:Panel CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-6 col-xs-12" runat="server" ID="Panel6">
                <asp:Label Text="Tipo DNI" runat="server" />
                <asp:DropDownList runat="server" ID="DropDownList1" CssClass="form-control select-single">
                    <asp:ListItem Value="1" Text="A" />
                    <asp:ListItem Value="2" Text="B" />
                </asp:DropDownList>
            </asp:Panel>
            <%-- DNI --%>
            <asp:Panel CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-6 col-xs-12" runat="server" ID="Panel7">
                <asp:Label Text="DNI" runat="server" />
                <asp:TextBox runat="server" ID="TextBox6" Class="form-control numeric-integer-positive" />
            </asp:Panel>
         </div>
        <br />
        <%-- Botón Registrar --%>
        <button class="button" type="button" title="Registrar" onclick="hide('InsertUser');display('griviewUsers')"> <i class="fa-solid fa-square-plus"></i> &ensp; Registrar</button>
        <%-- Botón Cancelar --%>
        <button class="button" type="button" title="Cancelar" onclick="hide('InsertUser');display('griviewUsers')"> <i class="fa-solid fa-xmark"></i> &ensp; Cancelar</button>
    </div>

    <%-- Modificar usuario--%>
    <div class="container" style="margin-top:10px;" id="UpdateUser">
        <div class="row">
            <h3>Modificar Usuario</h3>
        </div>
        <div class="row">
            <%-- ID Usuario --%>
            <asp:Panel CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-6 col-xs-12" runat="server" ID="Panel8" Enabled="false">
                <asp:Label Text="ID Usuario" runat="server" />
                <asp:TextBox runat="server" ID="TextBox4" Class="form-control text-plain" />
            </asp:Panel>
            <%-- Nombre --%>
            <asp:Panel CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-6 col-xs-12" runat="server" ID="Panel9">
                <asp:Label Text="Nombre" runat="server" />
                <asp:TextBox runat="server" ID="TextBox5" Class="form-control numeric-integer" />
            </asp:Panel>
            <%-- Email --%>
            <asp:Panel CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-6 col-xs-12" runat="server" ID="Panel10">
                <asp:Label Text="Email" runat="server" />
                <asp:TextBox runat="server" ID="TextBox7" Class="form-control numeric-integer-positive" />
            </asp:Panel>
        </div>
        <br />
        <div class="row">
            <%-- Tipo Usuario --%>
            <asp:Panel CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-6 col-xs-12" runat="server" ID="Panel11">
                <asp:Label Text="Tipo Usuario" runat="server" />
                <asp:DropDownList runat="server" ID="DropDownList3" CssClass="form-control select-single">
                    <asp:ListItem Value="1" Text="Administrador" />
                    <asp:ListItem Value="2" Text="Operador" />
                    <asp:ListItem Value="3" Text="Afectado" />
                    <asp:ListItem Value="4" Text="Solicitante" />
                </asp:DropDownList>
            </asp:Panel>
            <%-- Tipo DNI --%>
            <asp:Panel CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-6 col-xs-12" runat="server" ID="Panel12">
                <asp:Label Text="Tipo DNI" runat="server" />
                <asp:DropDownList runat="server" ID="DropDownList4" CssClass="form-control select-single">
                    <asp:ListItem Value="1" Text="A" />
                    <asp:ListItem Value="2" Text="B" />
                </asp:DropDownList>
            </asp:Panel>
            <%-- DNI --%>
            <asp:Panel CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-6 col-xs-12" runat="server" ID="Panel13">
                <asp:Label Text="DNI" runat="server" />
                <asp:TextBox runat="server" ID="TextBox8" Class="form-control numeric-integer-positive" />
            </asp:Panel>
         </div>
        <br />
        <%-- Botón Guardar --%>
        <button class="button" type="button" title="Guardar" onclick="hide('UpdateUser');display('griviewUsers')"> <i class="fa-solid fa-floppy-disk"></i> &ensp; Guardar</button>
        <%-- Botón No Guardar --%>
        <button class="button" type="button" title="NoGuardar" onclick="hide('UpdateUser');display('griviewUsers')"> <i class="fa-solid fa-xmark"></i> &ensp; Cancelar</button>
    </div>

</asp:Content>
