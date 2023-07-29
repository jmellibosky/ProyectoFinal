<%@ Page Title="REAPP" Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="CoordinacionInteresado.aspx.cs"
    Inherits="REApp.Forms.CoordinacionInteresado" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">


    <style>
        body {
            color: #566787;
            background: #484c64;
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

        .alert-interesado {
            border: 10px solid transparent;
            border-radius: 2rem;
        }
    </style>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <div style="background-color: #484c64;">

        <div class="container" style="margin: 30px;">
            <div class="row justify-content-center">
                <div class="col-6 text-center">
                    <image src="/Assets/EANA3.png" alt="LogoEANA" style="width: 300px; height: fit-content;"></image>
                </div>
            </div>
        </div>
        <br />

        <div class="container alert alert-interesado alert-light">
            <div class="row">
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <div class="row">
                        <h5>Solicitud N°<asp:Label ID="lblIdSolicitud" runat="server" /></h5>
                        <asp:HiddenField runat="server" ID="hdnIdInteresado" />
                        <asp:HiddenField runat="server" ID="hdnIdInteresadoSolicitud" />
                    </div>
                    <br />

                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>

                            <%--PANEL DATOS SOLICITUD--%>
                            <asp:Panel ID="pnlDatosSolicitud" runat="server" Visible="true">
                                <div class="row">
                                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-xs-12 form-group">
                                        <asp:Label runat="server" Text="Solicitante" />
                                        <asp:TextBox runat="server" ID="txtSolicitante" Enabled="false" CssClass="form-control" />
                                    </div>
                                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-xs-12 form-group">
                                        <asp:Label runat="server" Text="Nombre de la Solicitud" />
                                        <asp:TextBox runat="server" ID="txtNombreSolicitud" Enabled="false" CssClass="form-control" />
                                    </div>
                                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-xs-12 form-group">
                                        <asp:Label runat="server" Text="Actividad" />
                                        <asp:TextBox runat="server" ID="txtActividad" Enabled="false" CssClass="form-control" />
                                    </div>
                                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-xs-12 form-group">
                                        <asp:Label runat="server" Text="Modalidad" />
                                        <asp:TextBox runat="server" ID="txtModalidad" Enabled="false" CssClass="form-control" />
                                    </div>
                                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-xs-12 form-group">
                                        <asp:Label runat="server" Text="Fecha Desde" />
                                        <asp:TextBox runat="server" ID="txtFechaDesde" Enabled="false" CssClass="form-control" />
                                    </div>
                                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-xs-12 form-group">
                                        <asp:Label runat="server" Text="Fecha Hasta" />
                                        <asp:TextBox runat="server" ID="txtFechaHasta" Enabled="false" CssClass="form-control" />
                                    </div>
                                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-xs-12 form-group">
                                        <asp:Label runat="server" Text="Estado" />
                                        <asp:TextBox runat="server" ID="txtEstado" Enabled="false" CssClass="form-control" />
                                    </div>
                                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-xs-12 form-group">
                                        <asp:Label runat="server" Text="Última Actualización" />
                                        <asp:TextBox runat="server" ID="txtFechaActualizacion" Enabled="false" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12 form-group">
                                        <asp:Label Text="Observaciones" runat="server" />
                                        <asp:TextBox runat="server" Enabled="false" ID="txtObservaciones" TextMode="MultiLine" CssClass="form-control" />
                                    </div>
                                </div>
                                <hr />
                                <div class="row">
                                    <h5>Ubicaciones</h5>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-12">
                                        <asp:Button ID="btnGenerarKMZ" runat="server" Text="Descargar KMZ" OnClick="btnGenerarKMZ_Click" CssClass="btn btn-dark no-gif" />
                                    </div>
                                </div>
                                <hr />
                                <div class="row">
                                    <h5>Coordinación</h5>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-12">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <asp:Panel ID="pnlAlertRecomendaciones" runat="server" Visible="false" class="alert alert-danger" role="alert">
                                                    <asp:Label Text="Por favor, ingrese sus recomendaciones en el caso de rechazar la solicitud." runat="server" />
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12 form-group">
                                        <asp:TextBox ID="txtRecomendaciones" TextMode="MultiLine" runat="server" CssClass="form-control" placeholder="Ingrese sus recomendaciones aquí..." />
                                    </div>
                                </div>
                                <div class="row justify-content-center">
                                    <div class="col-3 text-center">
                                        <asp:Button ID="btnAprobar" runat="server" Text="Aprobar" CssClass="btn btn-success" OnClick="btnAprobar_Click" />
                                    </div>
                                    <div class="col-3 text-center">
                                        <asp:Button ID="btnRechazar" runat="server" Text="Rechazar" CssClass="btn btn-danger" OnClick="btnRechazar_Click" />
                                    </div>
                                </div>
                            </asp:Panel>

                            <%--PANEL DATOS RESPUESTA--%>
                            <asp:Panel ID="pnlDatosRespuesta" Visible="false" runat="server">
                                <div class="row justify-content-center">
                                    <asp:HiddenField ID="hdnIdCoordinacion" runat="server" />
                                    <h4>Solicitud <asp:Label ID="lblSolicitudRespuesta" runat="server" /></h4>
                                </div>
                                <div class="row justify-content-center">
                                    <h5><asp:Label Text="Se ha registrado correctamente su respuesta." runat="server" /></h5>
                                </div>
                                <hr />
                                <div class="row">
                                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-xs-12 form-group">
                                        <asp:Label runat="server" Text="Respuesta" />
                                        <asp:TextBox runat="server" ID="txtRespuesta" Enabled="false" CssClass="form-control" />
                                    </div>
                                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-xs-12 form-group">
                                        <asp:Label runat="server" Text="Fecha de Respuesta" />
                                        <asp:TextBox runat="server" ID="txtFechaRespuesta" Enabled="false" CssClass="form-control" />
                                    </div>
                                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12 form-group">
                                        <asp:Label runat="server" Text="Recomendaciones" />
                                        <asp:TextBox runat="server" ID="txtRecomendacionesRespuesta" TextMode="MultiLine" Enabled="false" CssClass="form-control" />
                                    </div>
                                </div>
                                <hr />
                                <div class="row">
                                    <div class="col-12">
                                        <asp:Button ID="btnCancelarRespuesta" Text="Cancelar Respuesta" CssClass="btn btn-danger" runat="server" OnClick="btnCancelarRespuesta_Click" />
                                    </div>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
