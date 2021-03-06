<%@ Page Title="" Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="GestionSolicitudes.aspx.cs"
    Inherits="REApp.Forms.GestionSolicitudes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">


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


<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">

    <div class="container">
        <h1 class="row justify-content-center">
            <label class="fw-normal mb-3 pb-2">Gestión de Solicitudes</label>
        </h1>
        <%--Se borra el AutoPostBack porq hay q cargar el dgv de otra forma.--%>
        <br />
        <%--<asp:Button ID="NuevaSolicitud" runat="server" Text="Nueva Solicitud" CssClass="btn btn-dark"/>--%>
    </div>
    <div style="text-align: end;">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:Button ID="btnNuevo" Text="Nueva Solicitud" CssClass="btn btn-primary btn-dark" runat="server" OnClick="btnNuevo_Click" />
                <asp:Button ID="btnVolver" Text="Volver al Listado" Visible="false" CssClass="btn btn-info btn-dark" runat="server" OnClick="btnVolver_Click" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <br />
    <br />

    <asp:UpdatePanel ID="upForm" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlListado" runat="server" Visible="true">
                <div class="conteiner">
                    <div class="row">
                        <div class="col">
                            <asp:Label runat="server">Solicitantes</asp:Label>
                            <asp:DropDownList runat="server" ID="ddlSolicitante" CssClass="form-control select-single" Width="300px" />
                            <br />
                            <asp:Button ID="btnFiltrar" Text="Filtrar" CssClass="btn btn-info btn-dark" runat="server" OnClick="btnFiltrar_Click" />

                            <div class="panel-body" style="display: flex; justify-content: center; align-items: center">
                                <div class="row" style="overflow: auto; height: 500px; width: 1400px;">
                                    <asp:UpdatePanel ID="upSolicitudes" Style="width: 100%;" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                        <ContentTemplate>

                                            <asp:GridView
                                                ID="gvSolicitud"
                                                runat="server"
                                                OnRowCommand="gvSolicitud_RowCommand"
                                                AutoGenerateColumns="false"
                                                CssClass="mGrid" PagerStyle-CssClass="pgr" RowStyle-Height="40px">
                                                <AlternatingRowStyle BackColor="white" />
                                                <HeaderStyle BackColor="#20789f" Font-Bold="true" Font-Size="Large" ForeColor="White" />
                                                <RowStyle BackColor="#e1dddd" />
                                                <SelectedRowStyle BackColor="#669999" Font-Bold="true" ForeColor="white" />

                                                <Columns>
                                                    <%-- El DataField debe contener el mismo nombre que la columna de la BD, que se recupera en BindGrid()--%>
                                                    <asp:BoundField DataField="IdSolicitud" HeaderText="ID" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="NombreUsuario" HeaderText="SOLICITANTE" ItemStyle-Width="20%" />
                                                    <asp:BoundField DataField="Nombre" HeaderText="NOMBRE SOLICITUD" ItemStyle-Width="20%" />
                                                    <asp:BoundField DataField="FHSolicitud" HeaderText="FECHA ALTA" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="NombreEstado" HeaderText="ESTADO" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" />

                                                    <%-- Boton con link para ver detalles solicitud--%>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="ACCIONES" ItemStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkVerDetalles" runat="server" OnClick="lnkVerDetalles_Click" CommandName="Detalle"
                                                                CommandArgument='<%# Eval("IdSolicitud") %>'>
                                                <i class="fa fa-eye" aria-hidden="true" style='font-size:15px;   color:#525252'/>  </i>
                                                            </asp:LinkButton>

                                                            <asp:LinkButton ID="lnkEditar" runat="server" OnClick="lnkEditar_Click" CommandName="Editar"
                                                                CommandArgument='<%# Eval("IdSolicitud") %>'>
                                                <i class="fa fa-pencil" aria-hidden="true" style='font-size:15px;  color:#525252' >  </i>
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
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlABM" runat="server" Visible="false">
                <div class="row">
                    <div class="col-12">
                        <br />
                        <div class="panel-body">
                            <asp:UpdatePanel ID="upModalABM" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                <ContentTemplate>
                                    <div class="col-12">
                                        <%--SOLICITANTE Y NOMBRE DE SOLICITUD--%>
                                        <div class="row">
                                            <asp:Panel ID="pnlModalSolicitante" CssClass="col-xl-3 col-lg-3 col-md-3 col-sm-3 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Solicitante" runat="server" />
                                                <asp:DropDownList runat="server" ID="ddlModalSolicitante" CssClass="form-control select-single" />
                                            </asp:Panel>
                                            <asp:HiddenField ID="hdnIdSolicitud" runat="server" />
                                            <asp:Panel ID="pnlModalNombreSolicitud" CssClass="col-xl-3 col-lg-3 col-md-3 col-sm-3 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Nombre de la Solicitud" runat="server" />
                                                <asp:TextBox runat="server" ID="txtModalNombreSolicitud" CssClass="form-control font-weight-bold" />
                                            </asp:Panel>
                                        </div>

                                        <%--ACTIVIDAD MODALIDAD Y FECHA--%>
                                        <div class="row">
                                            <asp:Panel ID="pnlModalActividad" CssClass="col-xl-3 col-lg-3 col-md-3 col-sm-3 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Actividad" runat="server" />
                                                <asp:DropDownList runat="server" ID="ddlModalActividad" CssClass="form-control select-single" AutoPostBack="true" OnSelectedIndexChanged="ddlModalActividad_SelectedIndexChanged" />
                                            </asp:Panel>
                                            <asp:Panel ID="pnlModalModalidad" CssClass="col-xl-3 col-lg-3 col-md-3 col-sm-3 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Modalidad" runat="server" />
                                                <asp:DropDownList runat="server" ID="ddlModalModalidad" CssClass="form-control select-single" />
                                            </asp:Panel>
                                            <asp:Panel ID="pnlModalFechaSolicitud" CssClass="col-xl-3 col-lg-3 col-md-3 col-sm-3 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Fecha de Solicitud" runat="server" />
                                                <asp:TextBox runat="server" ID="txtModalFechaSolicitud" CssClass="form-control" />
                                            </asp:Panel>
                                        </div>

                                        <%--OBSERVACIONES--%>
                                        <div class="row">
                                            <asp:Panel ID="pnlModalObservaciones" CssClass="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Observaciones" runat="server" />
                                                <asp:TextBox runat="server" ID="txtModalObservaciones" TextMode="MultiLine" CssClass="form-control" />
                                            </asp:Panel>
                                        </div>

                                        <%--INFO ADICIONAL (SÓLO PARA MODO DETALLES)--%>
                                        <div class="row">
                                            <asp:Panel ID="pnlModalEstadoSolicitud" CssClass="col-xl-3 col-lg-3 col-md-3 col-sm-3 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Estado de Solicitud" runat="server" />
                                                <asp:TextBox runat="server" ID="txtModalEstadoSolicitud" CssClass="form-control" />
                                            </asp:Panel>
                                            <asp:Panel ID="pnlModalFechaUltimaActualizacion" CssClass="col-xl-3 col-lg-3 col-md-3 col-sm-3 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Fecha de Última Actualización" runat="server" />
                                                <asp:TextBox runat="server" ID="txtModalFechaUltimaActualizacion" CssClass="form-control" />
                                            </asp:Panel>
                                        </div>
                                        <hr />

                                        <%--VEHÍCULOS AÉREOS--%>
                                        <div class="row">
                                            <h5>Vehículos Aéreos</h5>
                                        </div>
                                        <div class="row">
                                            <asp:Label Text="VANT" runat="server" />
                                            <asp:CheckBox ID="chkVant" runat="server" CssClass="switchery" AutoPostBack="true" OnCheckedChanged="chkVant_CheckedChanged" />
                                            <asp:Label Text="Aeronave" runat="server" />
                                        </div>
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>

                                                <%--AFECTACIÓN DE VANTS (SÓLO SI SE CHKVANT.CHECKED=FALSE)--%>
                                                <asp:Panel ID="pnlSeleccionVants" Visible="true" runat="server">

                                                    <%--VANTS--%>
                                                    <div class="col-12">
                                                        <div class="conteiner">
                                                            <div class="row">
                                                                <div class="col">
                                                                    <div class="row">
                                                                        <asp:GridView
                                                                            ID="gvVANTs"
                                                                            runat="server"
                                                                            AutoGenerateColumns="false"
                                                                            CssClass="mGrid" PagerStyle-CssClass="pgr" RowStyle-Height="40px">
                                                                            <AlternatingRowStyle BackColor="white" />
                                                                            <HeaderStyle BackColor="#20789f" Font-Bold="true" Font-Size="Large" ForeColor="White" />
                                                                            <RowStyle BackColor="#e1dddd" />
                                                                            <SelectedRowStyle BackColor="#669999" Font-Bold="true" ForeColor="white" />

                                                                            <Columns>
                                                                                <%-- El DataField debe contener el mismo nombre que la columna de la BD, que se recupera en BindGrid()--%>
                                                                                <asp:BoundField DataField="IdVant" HeaderText="ID" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                                                                <asp:BoundField DataField="Marca" HeaderText="MARCA" ItemStyle-Width="20%" />
                                                                                <asp:BoundField DataField="Modelo" HeaderText="MODELO" ItemStyle-Width="20%" />
                                                                                <asp:BoundField DataField="Clase" HeaderText="CLASE" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" />
                                                                                <asp:BoundField DataField="NumeroSerie" HeaderText="NRO. SERIE" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" />

                                                                                <%-- Boton con link para ver detalles solicitud--%>
                                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="ACCIONES" ItemStyle-Width="10%">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField Value='<%# Eval("IdVant") %>' runat="server" ID="hdnIdVant" />
                                                                                        <asp:CheckBox runat="server" ID="chkVinculado" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <hr />

                                        <%--UBICACIONES--%>
                                        <div class="row">
                                            <h5>Ubicaciones</h5>
                                        </div>
                                        <div class="row">
                                            <%--AGREGAR UBICACIONES--%>
                                            <div class="col-6">
                                                <div class="row">
                                                    <div class="col-12 text-center">
                                                        <asp:Button ID="btnAgregarUbicacion" Text="Agregar Ubicación" runat="server" CssClass="btn btn-success" OnClick="btnAgregarUbicacion_Click" />
                                                    </div>
                                                </div>

                                                <%--NUEVA UBICACIÓN--%>
                                                <asp:Panel ID="pnlAgregarUbicacion" runat="server" Visible="false">
                                                    <div class="row">
                                                        <div class="col-12">
                                                            <asp:Label Text="Circunferencia" runat="server" />
                                                            <asp:CheckBox ID="chkEsPoligono" runat="server" AutoPostBack="true" OnCheckedChanged="chkEsPoligono_CheckedChanged" />
                                                            <asp:Label Text="Polígono" runat="server" />

                                                            <%--NUEVA CIRCUNFERENCIA--%>
                                                            <asp:Panel ID="pnlAgregarCircunferencia" runat="server" Visible="false">
                                                                <div class="row">
                                                                    <asp:Panel runat="server" CssClass="col-6 form-group">
                                                                        <asp:Label Text="Latitud" runat="server" />
                                                                        <asp:TextBox ID="txtCircunferenciaLatitud" runat="server" CssClass="form-control" />
                                                                    </asp:Panel>
                                                                    <asp:Panel runat="server" CssClass="col-6 form-group">
                                                                        <asp:Label Text="Longitud" runat="server" />
                                                                        <asp:TextBox ID="txtCircunferenciaLongitud" runat="server" CssClass="form-control" />
                                                                    </asp:Panel>
                                                                </div>
                                                                <div class="row">
                                                                    <asp:Panel runat="server" CssClass="col-6 form-group">
                                                                        <asp:Label Text="Altura" runat="server" />
                                                                        <asp:TextBox ID="txtCircunferenciaAltura" runat="server" CssClass="form-control" />
                                                                    </asp:Panel>
                                                                    <asp:Panel runat="server" CssClass="col-6 form-group">
                                                                        <asp:Label Text="Radio" runat="server" />
                                                                        <asp:TextBox ID="txtCircunferenciaRadio" runat="server" CssClass="form-control" />
                                                                    </asp:Panel>
                                                                </div>
                                                            </asp:Panel>

                                                            <%--NUEVO POLIGONO--%>
                                                            <asp:Panel ID="pnlAgregarPoligono" runat="server" Visible="false">
                                                                <div class="row">
                                                                    <asp:Panel runat="server" CssClass="col-12 text-center">
                                                                        <asp:Button Text="Agregar Punto Geográfico" ID="btnAgregarPuntoGeografico" runat="server" CssClass="btn btn-success" OnClick="btnAgregarPuntoGeografico_Click" />
                                                                    </asp:Panel>
                                                                </div>

                                                                <asp:Panel ID="pnlAgregarPuntoGeografico" runat="server" Visible="false">
                                                                    <div class="row">
                                                                        <div class="col-4">
                                                                            <div class="row">
                                                                                <asp:Panel runat="server" CssClass="col-12 form-group">
                                                                                    <asp:Label Text="Latitud" runat="server" />
                                                                                    <asp:TextBox ID="txtPoligonoLatitud" runat="server" CssClass="form-control" />
                                                                                </asp:Panel>
                                                                            </div>
                                                                            <div class="row">
                                                                                <asp:Panel runat="server" CssClass="col-12 form-group">
                                                                                    <asp:Label Text="Longitud" runat="server" />
                                                                                    <asp:TextBox ID="txtPoligonoLongitud" runat="server" CssClass="form-control" />
                                                                                </asp:Panel>
                                                                            </div>
                                                                            <div class="row">
                                                                                <asp:Panel runat="server" CssClass="col-12 form-group">
                                                                                    <asp:Label Text="Altura" runat="server" />
                                                                                    <asp:TextBox ID="txtPoligonoAltura" runat="server" CssClass="form-control" />
                                                                                </asp:Panel>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-8">
                                                                            <asp:GridView
                                                                                ID="gvPuntosGeograficos"
                                                                                runat="server"
                                                                                AutoGenerateColumns="false"
                                                                                CssClass="mGrid" PagerStyle-CssClass="pgr" RowStyle-Height="40px">
                                                                                <AlternatingRowStyle BackColor="white" />
                                                                                <HeaderStyle BackColor="#20789f" Font-Bold="true" Font-Size="Large" ForeColor="White" />
                                                                                <RowStyle BackColor="#e1dddd" />
                                                                                <SelectedRowStyle BackColor="#669999" Font-Bold="true" ForeColor="white" />

                                                                                <Columns>
                                                                                    <%-- El DataField debe contener el mismo nombre que la columna de la BD, que se recupera en BindGrid()--%>
                                                                                    <asp:BoundField DataField="Latitud" HeaderText="LATITUD" ItemStyle-Width="20%" />
                                                                                    <asp:BoundField DataField="Longitud" HeaderText="LONGITUD" ItemStyle-Width="20%" />
                                                                                    <asp:BoundField DataField="Altura" HeaderText="ALTURA" ItemStyle-Width="20%" />
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </div>

                                                                    <hr />
                                                                    <div class="row">
                                                                        <div class="col-12 text-center">
                                                                            <asp:Button Text="Guardar Punto Geográfico" ID="btnGuardarPuntoGeografico" runat="server" CssClass="btn btn-success" OnClick="btnGuardarPuntoGeografico_Click" />
                                                                        </div>
                                                                    </div>
                                                                </asp:Panel>
                                                            </asp:Panel>

                                                            <hr />
                                                            <div class="row">
                                                                <div class="col-12 text-center">
                                                                    <asp:Button Text="Guardar Ubicación" ID="btnGuardarUbicacion" runat="server" CssClass="btn btn-success" OnClick="btnGuardarUbicacion_Click" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </div>

                                            <%--UBICACIONES AGREGADAS--%>
                                            <div class="col-6">
                                                <asp:Repeater ID="rptUbicaciones" runat="server">
                                                    <ItemTemplate>
                                                        <div class="row">
                                                            <div class="col-12 alert alert-success" role="alert">
                                                                <h5>
                                                                    <asp:Label ID="lblRptTipoUbicacion" runat="server" />
                                                                </h5>
                                                                <hr />
                                                                <asp:Label ID="lblRptDatos" runat="server" />
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>

                                        <br />
                                        <div class="row">
                                            <div class="col-12 text-center">
                                                <asp:Button ID="btnGuardar" Text="Guardar" CssClass="btn btn-success" runat="server" OnClick="btnGuardar_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
