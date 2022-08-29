﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomeDash.aspx.cs" Inherits="REApp.Forms.HomeDash.HomeDash" %>

<!DOCTYPE html>
<html lang="en">
    <head>
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
        <meta name="description" content="" />
        <meta name="author" content="" />
        <title>Home Dashboard</title>
        <!-- Favicon y FontAwesome-->
        <link rel="icon" type="image/x-icon" href="assets/favicon.ico" />
        <script src="https://kit.fontawesome.com/8e4807e881.js" crossorigin="anonymous"></script>
        <!-- Bootstrap core JS-->
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
        <!-- Core theme JS-->
        <script src="js/scripts.js"></script>
        <!-- Core theme CSS (includes Bootstrap)-->
        <link href="css/styles.css" rel="stylesheet" />
    </head>
    <body>
        <form id="HomeDashForm" runat="server">
            <%-- Ver bien porque se usa esto --%>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <div class="d-flex" id="wrapper">
            <!-- Sidebar-->
            <div class="border-end" style="background-color:#484c64" id="sidebar-wrapper">
                <%-- Aca es donde depende si es Admin o no lo que se muestra --%>
                <image src="/Assets/EANA3.png" alt="LogoEANA" class="sidebar-heading border-bottom" style="width:300px;height:fit-content;"></image>
                <%--<div runat="server" id="lblRol" class="sidebar-heading border-bottom bg-light">Rol Sistema</div>--%>
                <div class="list-group list-group-flush">
                    <ul id="side-menu" class="nav">
                         <%-- Administrador --%>
                        <li id="liAdministrador" style="" runat="server">
                            <a class="list-group-item list-group-item-action p-3" runat="server" target="iframePage" href="/Forms/Home.aspx">Home
                                <i class="fa fa-home"></i>
                            </a>
                            <a class="list-group-item list-group-item-action p-3" runat="server" target="iframePage" href="/Forms/GestionSolicitudes">Gestión de Solicitudes
                                <i class="fas fa-plane-departure"></i>
                            </a>
                            <a class="list-group-item list-group-item-action p-3" runat="server" target="iframePage" href="/Forms/GestionTripulantes.aspx">Gestión de Tripulación
                                <i class="fas fa-users"></i>
                            </a>
                            <a class="list-group-item list-group-item-action p-3" runat="server" target="iframePage" href="/Forms/GestionUsuarios.aspx">Gestión de Usuarios
                                <i class="fas fa-user"></i>
                            </a>
                            <a class="list-group-item list-group-item-action p-3" runat="server" target="iframePage" href="/Forms/GestionVants.aspx">Gestión de Vants
                                <i class="fas fa-plane"></i>
                            </a>
                            <a class="list-group-item list-group-item-action p-3" runat="server" target="iframePage" href="/Forms/FileUpload.aspx">Gestion de Documentación
                                <i class="fas fa-list"></i>
                            </a>
                        </li>
                    </ul>

                    <ul id="side-menu2" class="nav">
                         <%-- Solicitante --%>
                        <li id="liSolicitante" style="" runat="server">
                            <a class="list-group-item list-group-item-action p-3" runat="server" target="iframePage" href="/Forms/Home.aspx">Home
                                <i class="fa fa-home"></i>
                            </a>
                            <a class="list-group-item list-group-item-action p-3" runat="server" target="iframePage" href="/Forms/GestionSolicitudes">Mis solicitudes
                                <i class="fas fa-plane-departure"></i>
                            </a>
                            <a class="list-group-item list-group-item-action p-3" runat="server" target="iframePage" href="/Forms/GestionTripulantes.aspx">Mi tripulación
                                <i class="fas fa-users"></i>
                            </a>
                            <a class="list-group-item list-group-item-action p-3" runat="server" target="iframePage" href="/Forms/GestionVants.aspx">Mis VANTs
                                <i class="fas fa-plane"></i>
                            </a>
                            <a class="list-group-item list-group-item-action p-3" runat="server" target="iframePage" href="/Forms/FileUpload.aspx">Mi documentación                                <i class="fas fa-file-lines"></i>
                                <i class="fas fa-list"></i>
                            </a>
                        </li>
                    </ul>

                    <ul id="side-menu3" class="nav">
                         <%-- Operador --%>
                        <li id="liOperador" style="" runat="server">
                            <a class="list-group-item list-group-item-action p-3" runat="server" target="iframePage" href="/Forms/Home.aspx">Home
                                <i class="fa fa-home"></i>
                            </a>
                            <a class="list-group-item list-group-item-action p-3" runat="server" target="iframePage" href="/Forms/GestionSolicitudes">Gestión de Solicitudes
                                <i class="fas fa-plane-departure"></i>
                            </a>
                            <a class="list-group-item list-group-item-action p-3" runat="server" target="iframePage" href="/Forms/GestionTripulantes.aspx">Gestión de Tripulación
                                <i class="fas fa-users"></i>
                            </a>
                            <a class="list-group-item list-group-item-action p-3" runat="server" target="iframePage" href="/Forms/GestionVants.aspx">Gestión de Vants
                                <i class="fas fa-plane"></i>
                            </a>
                            <a class="list-group-item list-group-item-action p-3" runat="server" target="iframePage" href="/Forms/FileUpload.aspx">Gestion de Documentación
                                <i class="fas fa-list"></i>
                            </a>
                        </li>
                    </ul>
                    
                </div>
            </div>
            <!-- Page content wrapper-->
            <div id="page-content-wrapper">
                <!-- Top navigation-->
                <nav class="navbar navbar-expand-lg navbar-light bg-light border-bottom">
                    <div class="container-fluid">
                        <button class="btn btn-proyecto" id="sidebarToggle">Opciones</button>
                        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation"><span class="navbar-toggler-icon"></span></button>
                        <div class="collapse navbar-collapse" id="navbarSupportedContent">
                            <ul class="navbar-nav ms-auto mt-2 mt-lg-0">
                                <li class="nav-item active"><a runat="server" id="lblUsername" onserverclick="lblUsername_ServerClick" class="nav-link">Perfil de Usuario</a></li>
                                <li class="nav-item"><a onserverclick="btnCerrarSesion_Click" class="nav-link" runat="server">Cerrar Sesión</a></li>
                                <%--<li class="nav-item dropdown">
                                    <a class="nav-link dropdown-toggle" id="navbarDropdown" href="#" role="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Dropdown</a>
                                    <div class="dropdown-menu dropdown-menu-end" aria-labelledby="navbarDropdown">
                                        <a class="dropdown-item" href="#!">Action</a>
                                        <a class="dropdown-item" href="#!">Another action</a>
                                        <div class="dropdown-divider"></div>
                                        <a class="dropdown-item" href="#!">Something else here</a>
                                    </div>
                                </li>--%>
                            </ul>
                        </div>
                    </div>
                </nav>
                <!-- Page content-->
                <div id="content-page" class="container-fluid">
                    <%-- Paginas --%>
                    <iframe id="iframePage" style="height: 91vh; background-color: white;" name="iframePage" src="..\Home.aspx" frameborder="0" width="100%" runat="server"></iframe>
                </div>
            </div>
        </div>
        </form>
        
    </body>
</html>
