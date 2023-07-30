<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PerfilUsuario.aspx.cs" Inherits="REApp.Forms.PefilUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">

    <%-- CSS --%>
    <style>
        body {
            color: #566787;
            background: #fff;
            font-family: sans-serif;
            font-size: 15px;
        }

        * {
            padding: 0;
            margin: 0;
        }

        body {
            font: 11px Tahoma;
        }

        h1 {
            font: bold 32px Times;
            color: #666;
            text-align: center;
            padding: 20px 0;
        }

        #container {
            width: 700px;
            margin: 10px auto;
        }

        .mGrid {
            width: 100%;
            background-color: #525252;
            margin: 5px 0 10px 0;
            border: solid 3px #525252;
            border-collapse: collapse;
            border-radius: 1rem 0 0 1rem;
            /*-webkit-border-radius: 50px;*/
            /*-moz-border-radius: 50px;*/
            border-radius: 8px;
            overflow: hidden;
        }

            .mGrid td {
                padding: 2px;
                border: solid 1px #c1c1c1;
                color: #525252;
            }

            .mGrid th {
                padding: 4px 2px;
                color: #fff;
                background: #424242 url(grd_head.png) repeat-x top;
                border-left: solid 1px #525252;
                font-size: 0.9em;
                text-align: center;
                font-size: 15px;
                position: absolute sticky;
            }

            .mGrid .alt {
                background: #fcfcfc url(grd_alt.png) repeat-x top;
            }

            .mGrid .pgr {
                background: #424242 url(grd_pgr.png) repeat-x top;
            }

                .mGrid .pgr table {
                    margin: 5px 0;
                }

                .mGrid .pgr td {
                    border-width: 0;
                    padding: 0 6px;
                    border-left: solid 1px #666;
                    font-weight: bold;
                    color: #fff;
                    line-height: 12px;
                }

                .mGrid .pgr a {
                    color: #666;
                    text-decoration: none;
                }

                    .mGrid .pgr a:hover {
                        color: #000;
                        text-decoration: none;
                    }


        button {
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
    </style>

</asp:Content>


<asp:Content ID="cBody" ContentPlaceHolderID="cphBody" runat="server">


    <%-- Encabezado --%>
    <div class="row">
        <div class="col-12">

            <h1 class="row justify-content-center mb-0 pb-0 pt-5">
                <label style="font-style: italic; font-family: 'REM', sans-serif; letter-spacing: 3px" class="fw-normal mb-3 pb-2">Perfil de Usuario</label>
            </h1>
            <br />
            <div style="text-align: end;">
                <asp:Button ID="btnVolver" Text="Volver al Listado" Visible="false" CssClass="btn btn-info btn-dark" runat="server" OnClick="btnVolver_Click" />
            </div>
        </div>
    </div>


    <asp:Panel ID="pnlABM" runat="server">
        <div class="row">
            <div class="col-12">
                <br />
                <div class="panel-body">
                    <asp:UpdatePanel ID="upModalABM" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                        <ContentTemplate>
                            <div class="col-12">
                                <div class="row">
                                    <h6>Datos Personales</h6>
                                </div>
                                <div class="row">
                                    <asp:Panel ID="pnlModalNombreUsuario" CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-sm-12 form-group" runat="server">
                                        <asp:Label Text="Nombre" runat="server" />
                                        <asp:TextBox runat="server" ID="txtModalNombreUsuario" CssClass="form-control" />
                                    </asp:Panel>
                                    <asp:Panel ID="pnlModalApellidoUsuario" CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-sm-12 form-group" runat="server">
                                        <asp:Label Text="Apellido" runat="server" />
                                        <asp:TextBox runat="server" ID="txtModalApellidoUsuario" CssClass="form-control" />
                                    </asp:Panel>
                                    <asp:Panel ID="pnlModalRol" CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-sm-12 form-group" runat="server">
                                        <%--Lo deje invisible al ddlModalSolicitante, ver--%>
                                        <asp:Label Text="Rol" runat="server" />
                                        <asp:DropDownList runat="server" ID="ddlModalRol" CssClass="form-control select-single" Enabled="false" />
                                    </asp:Panel>
                                </div>
                                <div class="row">
                                    <asp:Panel ID="pnlModalDni" CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-sm-12 form-group" runat="server">
                                        <asp:Label Text="DNI" runat="server" />
                                        <asp:TextBox runat="server" ID="txtModalDni" CssClass="form-control" />
                                    </asp:Panel>
                                    <asp:Panel ID="pnlModalTipoDni" CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-sm-12 form-group" runat="server">
                                        <asp:Label Text="Tipo DNI" runat="server" />
                                        <asp:TextBox runat="server" ID="txtModalTipoDni" CssClass="form-control" />
                                    </asp:Panel>
                                    <asp:Panel ID="pnlModalFechaNacimiento" CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-sm-12 form-group" runat="server">
                                        <asp:Label Text="Fecha de Nacimiento" runat="server" />
                                        <asp:TextBox runat="server" ID="txtModalFechaNac" CssClass="form-control" />
                                        <%--TextMode="DateTime"--%>
                                    </asp:Panel>
                                </div>
                                <div class="row">
                                    <asp:Panel ID="pnlCuil" CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-sm-12 form-group" runat="server">
                                        <asp:Label Text="CUIT" runat="server" />
                                        <asp:TextBox runat="server" ID="txtModalCuit" CssClass="form-control" />
                                    </asp:Panel>
                                </div>
                                <div class="row">
                                    <h6>Datos de Contacto</h6>
                                </div>
                                <div class="row">
                                    <asp:Panel ID="pnlCorreo" CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-sm-12 form-group" runat="server">
                                        <asp:Label Text="Correo Electrónico" runat="server" />
                                        <asp:TextBox runat="server" ID="txtModalCorreo" CssClass="form-control" />
                                    </asp:Panel>
                                    <asp:Panel ID="pnlTelefono" CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-sm-12 form-group" runat="server">
                                        <asp:Label Text="Teléfono" runat="server" />
                                        <asp:TextBox runat="server" ID="txtModalTelefono" CssClass="form-control" />
                                    </asp:Panel>
                                </div>

                                <div class="row">
                                    <asp:HiddenField ID="hdnIdUsuario" runat="server" />
                                    <asp:HiddenField ID="hdnIdCurrentUser" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <asp:Panel ID="pnlError" Visible="false" CssClass="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12 form-group" runat="server">
                                    <div class="alert alert-danger" role="alert">
                                        <h5>
                                            <asp:Label ID="txtErrorHeader" runat="server" />
                                        </h5>
                                        <hr />
                                        <asp:Label ID="txtErrorBody" runat="server" />
                                    </div>
                                    <hr />
                                </asp:Panel>
                            </div>
                            <div class="row">
                                <div style="justify-content: center;text-align: center;">
                                    <asp:Button ID="btnGuardar" Text="Guardar" CssClass="btn btn-success" runat="server" OnClick="btnGuardar_Click1" />
                                    <hr />

                                </div>
                            </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div style="text-align: end;">
                        <asp:Button ID="btnCambioPassword" Text="Cambiar contraseña ->" CssClass="btn btn-info btn-dark" runat="server" OnClick="btnCambioPassword_Click" />
                    </div>

                </div>
            </div>
        </div>
    </asp:Panel>

    <%--Panel actualizacion Password--%>
    <asp:Panel ID="pnlCambioPassword" runat="server" Visible="false">
        <div class="row">
            <h6>Desea cambiar su contraseña?</h6>
        </div>
        <div class="row">
                <asp:Panel ID="pnlPassword" CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-sm-12 form-group" runat="server">
                    <asp:Label Text="Contraseña actual:" runat="server" />
                    <asp:TextBox runat="server" ID="txtModalPasswordActual" CssClass="form-control" TextMode="Password" />
                    <hr />
                    <asp:Label Text="Contraseña Nueva" runat="server" />
                    <asp:TextBox runat="server" ID="txtModalPassword" CssClass="form-control" TextMode="Password" />
                    <div class="form-outline mb-3">
                        <span class="form-label ml-2" >*La contraseña debe tener al menos 8 caracteres, una mayúscula y un número.</span>
                    </div>
                    <asp:Label Text="Confirmar Contraseña" runat="server" />
                    <asp:TextBox runat="server" ID="txtModalConfirmarPassword" CssClass="form-control" TextMode="Password"  />
                    <asp:Label ID="lblRequisitos" runat="server" Text=""></asp:Label>
                </asp:Panel>
        </div>

        <div style="justify-content: center;">
            <hr />
            <asp:Button ID="btnActualizarPassword" Text="Actualizar Contraseña" CssClass="btn btn-success" runat="server" OnClick="btnActualizarPassword_Click" />
        </div>
    </asp:Panel>

</asp:Content>
