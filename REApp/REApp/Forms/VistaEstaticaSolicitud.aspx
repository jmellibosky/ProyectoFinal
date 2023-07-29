<%@ Page Title="REAPP" Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="VistaEstaticaSolicitud.aspx.cs"
    Inherits="REApp.Forms.VistaEstaticaSolicitud" %>


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
                                <hr />
                                <div class="row">
                                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                        <h5>Tripulación</h5>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <asp:DataList ID="dtlTripulacion" runat="server" class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <ItemTemplate>
                                                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <div class="row">
                                                        <div class="form-group col-xl-4 col-lg-4 col-md-4 col-sm-4 col-xs-4">
                                                            <asp:Label Text="Nombre" runat="server" />
                                                            <asp:TextBox Enabled="false" CssClass="form-control" ID="lblNombreTripulacion" runat="server" Text='<%# Eval("NombreTripulacion") %>' />
                                                        </div>
                                                        <div class="form-group col-xl-4 col-lg-4 col-md-4 col-sm-4 col-xs-4">
                                                            <asp:Label Text="DNI" runat="server" />
                                                            <asp:TextBox Enabled="false" CssClass="form-control" ID="lblDNITripulacion" runat="server" Text='<%# Eval("DNITripulacion") %>' />
                                                        </div>
                                                        <div class="form-group col-xl-4 col-lg-4 col-md-4 col-sm-4 col-xs-4">
                                                            <asp:Label Text="Contacto" runat="server" />
                                                            <asp:TextBox Enabled="false" CssClass="form-control" ID="lblContactoTripulacion" runat="server" Text='<%# Eval("ContactoTripulacion") %>' />
                                                        </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </div>
                                </div>
                                <hr />
                                <div class="row">
                                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                        <h5>Ubicaciones</h5>
                                    </div>
                                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-xs-12 text-right">
                                        <asp:Button ID="btnGenerarKMZ" runat="server" Text="Descargar KMZ" OnClick="btnGenerarKMZ_Click" CssClass="btn btn-dark text-right no-gif" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <asp:DataList ID="dtlUbicaciones" runat="server" class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <ItemTemplate>
                                                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <div class="row">
                                                        <div class="form-group col-xl-2 col-lg-2 col-md-2 col-sm-2 col-xs-2">
                                                            <asp:Label Text="ID" runat="server" />
                                                            <asp:TextBox Enabled="false" CssClass="form-control" ID="lblIdUbicacion" runat="server" Text='<%# Eval("IdUbicacion") %>' />
                                                        </div>
                                                        <div class="form-group col-xl-2 col-lg-2 col-md-2 col-sm-2 col-xs-2">
                                                            <asp:Label Text="Latitud" runat="server" />
                                                            <asp:TextBox Enabled="false" CssClass="form-control" ID="lblLatitudUbicacion" runat="server" Text='<%# Eval("Latitud") %>' />
                                                        </div>
                                                        <div class="form-group col-xl-2 col-lg-2 col-md-2 col-sm-2 col-xs-2">
                                                            <asp:Label Text="Longitud" runat="server" />
                                                            <asp:TextBox Enabled="false" CssClass="form-control" ID="lblLongitudUbicacion" runat="server" Text='<%# Eval("Longitud") %>' />
                                                        </div>
                                                        <div class="form-group col-xl-2 col-lg-2 col-md-2 col-sm-2 col-xs-2">
                                                            <asp:Label Text="Altura" runat="server" />
                                                            <asp:TextBox Enabled="false" CssClass="form-control" ID="lblAlturaUbicacion" runat="server" Text='<%# Eval("Altura") %>' />
                                                        </div>
                                                        <div class="form-group col-xl-2 col-lg-2 col-md-2 col-sm-2 col-xs-2">
                                                            <asp:Label Text="Radio" runat="server" />
                                                            <asp:TextBox Enabled="false" CssClass="form-control" ID="lblRadioUbicacion" runat="server" Text='<%# Eval("Radio") %>' />
                                                        </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </div>
                                </div>
                                <br />
                                <hr />
                                <div class="row">
                                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                        <h5>VANTs</h5>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <asp:DataList ID="dtlVants" runat="server" class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <ItemTemplate>
                                                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <div class="row">
                                                        <div class="form-group col-xl-3 col-lg-3 col-md-3 col-sm-3 col-xs-3">
                                                            <asp:Label Text="ID" runat="server" />
                                                            <asp:TextBox Enabled="false" CssClass="form-control" ID="lblIdVant" runat="server" Text='<%# Eval("IdVant") %>' />
                                                        </div>
                                                        <div class="form-group col-xl-3 col-lg-3 col-md-3 col-sm-3 col-xs-3">
                                                            <asp:Label Text="Marca y Modelo" runat="server" />
                                                            <asp:TextBox Enabled="false" CssClass="form-control" ID="lblMarcaModeloVant" runat="server" Text='<%# Eval("MarcaModelo") %>' />
                                                        </div>
                                                        <div class="form-group col-xl-3 col-lg-3 col-md-3 col-sm-3 col-xs-3">
                                                            <asp:Label Text="Clase" runat="server" />
                                                            <asp:TextBox Enabled="false" CssClass="form-control" ID="lblClaseVant" runat="server" Text='<%# Eval("Clase") %>' />
                                                        </div>
                                                        <div class="form-group col-xl-3 col-lg-3 col-md-3 col-sm-3 col-xs-3">
                                                            <asp:Label Text="Numero de Serie" runat="server" />
                                                            <asp:TextBox Enabled="false" CssClass="form-control" ID="lblNumeroSerieVant" runat="server" Text='<%# Eval("NumeroSerie") %>' />
                                                        </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:DataList>
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
