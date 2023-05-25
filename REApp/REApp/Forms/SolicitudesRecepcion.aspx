<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SolicitudesRecepcion.aspx.cs" Inherits="REApp.Forms.SolicitudesRecepcion" %>


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
        <h1 class="row justify-content-center mb-0 pb-0 pt-5">
            <label style="font-family: Azonix; letter-spacing: 3px" class="fw-normal mb-3 pb-2">Gestión de Solicitudes Recepción</label>
        </h1>
        <%--Se borra el AutoPostBack porq hay q cargar el dgv de otra forma.--%>
        <br />
        <%--<asp:Button ID="NuevaSolicitud" runat="server" Text="Nueva Solicitud" CssClass="btn btn-dark"/>--%>
    </div>
    <div style="text-align: end;">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:Button ID="btnGenerarKMZ" Text="Generar KMZ" Visible="false" CssClass="btn btn-info btn-dark" runat="server" OnClick="btnGenerarKMZ_Click" />
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
                            <div class="row">
                                <div class="form-group col">
                                    <asp:Label runat="server">Solicitantes</asp:Label>
                                    <asp:DropDownList runat="server" ID="ddlSolicitante" CssClass="form-control select-single" Width="300px" AutoPostBack="true" OnSelectedIndexChanged="ddlSolicitante_SelectedIndexChanged" />
                                </div>
                                <div class="form-group col">
                                    <asp:Label runat="server">Ver Solicitudes Eliminadas</asp:Label>
                                    <asp:DropDownList runat="server" ID="ddlVerBajas" CssClass="form-control select-single" Width="300px" AutoPostBack="true" OnSelectedIndexChanged="ddlVerBajas_SelectedIndexChanged">
                                        <asp:ListItem Text="No" Value="0" />
                                        <asp:ListItem Text="Sí" Value="1" />
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <br />

                            <div class="panel-body" style="display: flex; justify-content: center; align-items: center">
                                <div class="row" style="overflow: auto; height: 450px; width: 1400px;">
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
                                                    <asp:BoundField DataField="IdSolicitud" HeaderText="NRO REA (ID)" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="NombreUsuario" HeaderText="SOLICITANTE" ItemStyle-Width="10%" />
                                                    <asp:BoundField DataField="Nombre" HeaderText="NOMBRE SOLICITUD" ItemStyle-Width="15%" />
                                                    <asp:BoundField DataField="NombreModalidad" HeaderText="MODALIDAD" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="NombreActividad" HeaderText="ACTIVIDAD" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="NombreEstado" HeaderText="ESTADO" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="NombreProvincia" HeaderText="PROVINCIA" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="FHDesde" HeaderText="FECHA INICIO" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />

                                                    <%-- Boton con link para ver detalles solicitud--%>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="ACCIONES" ItemStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkVerDetalles" ToolTip="Iniciar Recepción" runat="server" CommandName="Detalle"
                                                                CommandArgument='<%# Eval("IdSolicitud") %>'>
                                                <i class="fa fa-eye" aria-hidden="true" style='font-size:15px;   color:#525252'/>  </i>
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="btnVerForo" ToolTip="Enviar Mensaje" runat="server" OnClick="btnVerForo_Click"
                                                                CommandArgument='<%# Eval("IdSolicitud") %>'>
                                                                    <i class="fa fa-comments" aria-hidden="true" style='font-size: 15px; color: #525252'></i>
                                                            </asp:LinkButton>

                                                            <asp:LinkButton ID="btnEliminar" ToolTip="Eliminar" runat="server" CommandName="Eliminar"
                                                                CommandArgument='<%# Eval("IdSolicitud") %>' Visible='<%# ddlSolicitante.Enabled %>'>
                                                                <i class="fa fa-trash text-danger" aria-hidden="true" style='font-size: 15px; margin-left: 10px'></i>
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
                <br />
                <div class="row">
                    <div class="col-12 alert alert-warning" role="alert">
                        <div class="row">
                            <h5>
                                <asp:Label runat="server" Text="Acciones" /></h5>
                        </div>
                        <hr />
                        <div class="row justify-content-center">
                            <asp:Button runat="server" Text="Analizar Solicitud" CssClass="btn btn-success" ID="btnEstadoOperador" OnClick="btnEstadoOperador_Click" Visible="false" />
                        </div>
                    </div>
                </div>
                <br />
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
                                            <asp:Panel ID="pnlModalActividad" CssClass="col-xl-3 col-lg-3 col-md-3 col-sm-3 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Actividad" runat="server" />
                                                <asp:DropDownList runat="server" ID="ddlModalActividad" CssClass="form-control select-single" AutoPostBack="true" OnSelectedIndexChanged="ddlModalActividad_SelectedIndexChanged" />
                                            </asp:Panel>
                                            <asp:Panel ID="pnlModalModalidad" CssClass="col-xl-3 col-lg-3 col-md-3 col-sm-3 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Modalidad" runat="server" />
                                                <asp:DropDownList runat="server" ID="ddlModalModalidad" CssClass="form-control select-single" />
                                            </asp:Panel>
                                        </div>

                                        <%--ACTIVIDAD MODALIDAD Y FECHA--%>
                                        <div class="row">
                                            <asp:Panel CssClass="col-xl-3 col-lg-3 col-md-3 col-sm-3 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Fecha Desde" runat="server" />
                                                <asp:TextBox runat="server" ID="txtModalFechaDesde" CssClass="form-control" />
                                            </asp:Panel>
                                            <asp:Panel CssClass="col-xl-3 col-lg-3 col-md-3 col-sm-3 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Fecha Hasta" runat="server" />
                                                <asp:TextBox runat="server" ID="txtModalFechaHasta" CssClass="form-control" />
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
                                            <asp:CheckBox ID="chkVant" runat="server" CssClass="switchery" AutoPostBack="true" OnCheckedChanged="chkVant_CheckedChanged" Enabled="false" />
                                            <asp:Label Text="Aeronave" runat="server" />
                                        </div>
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>

                                                <%--AFECTACIÓN DE VANTS (SÓLO SI SE CHKVANT.CHECKED=FALSE)--%>
                                                <asp:Panel ID="pnlSeleccionVants" Visible="true" runat="server">

                                                    <%--VANTS--%>
                                                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                        <div class="container">
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
                                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="SELECCIONAR" ItemStyle-Width="10%">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField Value='<%# Eval("IdVant") %>' runat="server" ID="hdnIdVant" />
                                                                                        <asp:CheckBox runat="server" ID="chkVANTVinculado" Enabled="false" Checked='<%# Eval("Checked").ToString().Equals("0") ? false : true %>' />
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
                                        <hr />

                                        <%--TRIPULANTES--%>
                                        <div class="row">
                                            <h5>Tripulantes</h5>
                                        </div>
                                        <div class="row">
                                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <asp:GridView
                                                    ID="gvTripulacion"
                                                    runat="server"
                                                    AutoGenerateColumns="false"
                                                    CssClass="mGrid" PagerStyle-CssClass="pgr" RowStyle-Height="40px">
                                                    <AlternatingRowStyle BackColor="white" />
                                                    <HeaderStyle BackColor="#20789f" Font-Bold="true" Font-Size="Large" ForeColor="White" />
                                                    <RowStyle BackColor="#e1dddd" />
                                                    <SelectedRowStyle BackColor="#669999" Font-Bold="true" ForeColor="white" />

                                                    <Columns>
                                                        <%-- El DataField debe contener el mismo nombre que la columna de la BD, que se recupera en BindGrid()--%>
                                                        <asp:BoundField DataField="IdTripulacion" HeaderText="ID" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="Nombre" HeaderText="NOMBRE" ItemStyle-Width="20%" />
                                                        <asp:BoundField DataField="Apellido" HeaderText="APELLIDO" ItemStyle-Width="20%" />
                                                        <asp:BoundField DataField="DNI" HeaderText="DNI" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="Telefono" HeaderText="TELÉFONO" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" />

                                                        <%-- Boton con link para ver detalles solicitud--%>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="SELECCIONAR" ItemStyle-Width="10%">
                                                            <ItemTemplate>
                                                                <asp:HiddenField Value='<%# Eval("IdTripulacion") %>' runat="server" ID="hdnIdTripulacion" />
                                                                <asp:CheckBox runat="server" ID="chkTripulacionVinculado" Enabled="false" Checked='<%# Eval("Checked").ToString().Equals("0") ? false : true %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                        <hr />

                                        <%--HISTORIAL DE ESTADOS--%>


                                        <asp:Panel ID="pnlHistorialSolicitud" runat="server" Visible="true">
                                            <div class="row">
                                                <h5>Historial de Estados</h5>

                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12 form-group">
                                                    <%--GRILLA CON HISTORIAL DE CAMBIOS--%>
                                                    <asp:GridView
                                                        ID="gvHistorial"
                                                        runat="server"
                                                        AutoGenerateColumns="false"
                                                        CssClass="mGrid" PagerStyle-CssClass="pgr" RowStyle-Height="40px" Visible="true">
                                                        <AlternatingRowStyle BackColor="white" />
                                                        <HeaderStyle BackColor="#20789f" Font-Bold="true" Font-Size="Large" ForeColor="White" />
                                                        <RowStyle BackColor="#e1dddd" />
                                                        <SelectedRowStyle BackColor="#669999" Font-Bold="true" ForeColor="white" />
                                                        <Columns>
                                                            <%-- El DataField debe contener el mismo nombre que la columna de la BD, que se recupera en BindGrid()--%>
                                                            <asp:BoundField DataField="EstadoAnterior" HeaderText="ESTADO ANTERIOR" ItemStyle-Width="20%" />
                                                            <asp:BoundField DataField="EstadoActual" HeaderText="ESTADO ACTUAL" ItemStyle-Width="20%" />
                                                            <asp:BoundField DataField="FechaCambio" HeaderText="FECHA CAMBIO ESTADO" ItemStyle-Width="20%" />
                                                            <asp:BoundField DataField="Usuario" HeaderText="USUARIO" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField DataField="Observaciones" HeaderText="OBSERVACIONES" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                            <hr />
                                        </asp:Panel>

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
