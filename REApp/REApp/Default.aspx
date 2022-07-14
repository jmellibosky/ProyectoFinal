<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="REApp._Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="Content/bootstrap.min.css" rel="stylesheet">
    <link href="Content/Site.css" rel="stylesheet" />
    <script src="https://kit.fontawesome.com/8e4807e881.js" crossorigin="anonymous"></script>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <title></title>
    <style>
        #lateral-menu {
            background: #f0f0ff;
        }

        body {
            overflow-y: hidden;
            background-color: white;
            padding-top: 0px;
        }

        #navbar {
            margin-bottom: 0;
            border-bottom-color: #e7e7e7;
            border-bottom-width: 1px;
            border-bottom-style: solid;
        }

        nav > div.navbar-header {
            width: 100%;
        }

            nav > div.navbar-header > ul {
                float: right;
            }

        .navbar-brand {
            padding: 5px !important;
        }

        .navbar-header {
            background: #7777ff;
        }

        #side-menu {
            border-right-width: 1px;
            border-right-style: solid;
            border-right-color: #e7e7e7;
            height: calc(100vh - 51px) !important;
            overflow-y: auto;
        }

        #page-wrapper {
            padding: 0px;
            border: none;
        }

        #content-page {
            padding: 0px;
            padding-left:220px;
        }

        #iframepage {
            height: calc(100vh - 56px);
            //background: url("Content/images/logo-watermark.png") center center no-repeat;
        }

        .dropbtn {
            background-color: #000099;
            color: white;
            padding: 16px;
            font-size: 16px;
            border: none;
            width: 50px;
            height: 50px;
        }

        .dropdown {
            position: relative;
            display: inline-block;
        }

        .dropdown-content {
            display: none;
            position: absolute;
            background-color: #f1f1f1;
            min-width: 160px;
            box-shadow: 0px 8px 16px 0px rgba(0,0,0,0.2);
            z-index: 1;
        }

            .dropdown-content a {
                color: black;
                padding: 12px 16px;
                text-decoration: none;
                display: block;
            }

                .dropdown-content a:hover {
                    background-color: #ddd;
                }

        .dropdown:hover .dropdown-content {
            display: block;
        }

        .dropdown:hover .dropbtn {
            background-color: #000066;
        }

        .form-control {
            display: inline;
        }

        .navbar-right {
            margin-right: 0px;
        }
    </style>

    <%-- jQuery --%>
    <script src="Scripts/jquery-3.4.1.min.js"></script>
    <%-- Bootstrap --%>
    <script src="Scripts/bootstrap.min.js"></script>
    <%-- CollapsibleMenu --%>
    <script src="Scripts/collapsiblemenu.min.js"></script>

    <script>
        $(document).ready(function () {

        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="wrapper">
            <nav id="navbar" class="navbar navbar-default navbar-static-top" role="navigation">
                <div class="navbar-header">
                    <div class="dropdown nav navbar-top-links navbar-right">
                        <button id="btnLogout" class="dropbtn" style="margin-right: 15px;" runat="server">
                            <i class="fas fa-power-off"></i>
                        </button>
                    </div>
                    <div class="dropdown nav navbar-top-links navbar-right">
                        <button id="btnPerfilUsuario" class="dropbtn" runat="server">
                            <i class="fas fa-user"></i>
                        </button>
                    </div>
                </div>

                <%-- Menu lateral --%>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div id="lateral-menu" class="navbar-default sidebar" style="width: 200px;" role="navigation">
                            <div class="sidebar-nav navbar-collapse">
                                <ul id="side-menu" class="nav">
                                    <%-- Administrador --%>
                                    <li id="liAdministrador" runat="server">
                                        <a href="#">
                                            <strong>
                                                <asp:Label Text="Administrador" runat="server" />
                                            </strong>
                                        </a>
                                        <ul class="nav nav-second-level">
                                            <li>
                                                <a href="Forms/__Ejemplos.aspx" class="item-menu" target="iframePage" runat="server">
                                                    <i class="fa fa-home" aria-hidden="true"></i>
                                                    <asp:Label Text="__Ejemplos" runat="server" />
                                                </a>
                                            </li>
                                            <li>
                                                <a href="Forms/__Ejemplos2.aspx" class="item-menu" target="iframePage" runat="server">
                                                    <i class="fa fa-home" aria-hidden="true"></i>
                                                    <asp:Label Text="__Ejemplos2" runat="server" />
                                                </a>
                                            </li>
                                            <li>
                                                <a runat="server" id="lnkHomeAdministrador">
                                                    <i class="fa fa-home" aria-hidden="true"></i>
                                                    <asp:Label Text="Home" runat="server" />
                                                </a>
                                            </li>
                                            <li>
                                                <a runat="server" id="lnkGestionSolicitudes">
                                                    <i class="fas fa-list" aria-hidden="true"></i>
                                                    <asp:Label Text="Gestión de Solicitudes" runat="server" />
                                                </a>
                                            </li>
                                            <li>
                                                <a runat="server" id="lnkGestionUsuarios">
                                                    <i class="fas fa-user" aria-hidden="true"></i>
                                                    <asp:Label Text="Gestión de Usuarios" runat="server" />
                                                </a>
                                            </li>
                                            <li>
                                                <a runat="server" id="lnkGestionVehiculos">
                                                    <i class="fas fa-plane" aria-hidden="true"></i>
                                                    <asp:Label Text="Gestión de Vehículos Aéreos" runat="server" />
                                                </a>
                                            </li>
                                            <li>
                                                <a runat="server" id="lnkGestionAerodromos">
                                                    <i class="fas fa-plane-departure" aria-hidden="true"></i>
                                                    <asp:Label Text="Gestión de Aeródromos" runat="server" />
                                                </a>
                                            </li>
                                            <li>
                                                <a runat="server" id="lnkGestionZonasEspeciales">
                                                    <i class="fas fa-map-marker-alt" aria-hidden="true"></i>
                                                    <asp:Label Text="Gestión de Zonas Especiales" runat="server" />
                                                </a>
                                            </li>
                                            <li>
                                                <a runat="server" id="lnkParametrizacion">
                                                    <i class="fa fa-cog" aria-hidden="true"></i>
                                                    <asp:Label Text="Parametrización" runat="server" />
                                                </a>
                                            </li>
                                        </ul>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </nav>
            <div id="content-page" class="container-fluid">
                <%-- Paginas --%>
                <iframe id="iframePage" style="height:1000px; background-color:white;" name="iframePage" src="Forms\__Ejemplos.aspx" frameborder="0" width="100%" runat="server"></iframe>
            </div>
        </div>
    </form>
</body>
</html>

