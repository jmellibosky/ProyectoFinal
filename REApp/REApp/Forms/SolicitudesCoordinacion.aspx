<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SolicitudesCoordinacion.aspx.cs" Inherits="REApp.Forms.SolicitudesCoordinacion" %>

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
            <label style="font-family:Azonix; letter-spacing: 3px" class="fw-normal mb-3 pb-2">Gestión de Solicitudes en Coordinación</label>
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
                            <asp:Label runat="server">Solicitantes</asp:Label>
                            <asp:DropDownList runat="server" ID="ddlSolicitante" CssClass="form-control select-single" Width="300px" AutoPostBack="true" OnSelectedIndexChanged="ddlSolicitante_SelectedIndexChanged" />
                            <br />

                            <div class="panel-body" style="display: flex; justify-content: center; align-items: center">
                                <div class="row" style="overflow: auto; height: 450px; width: 1450px;">
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
                                                    <asp:BoundField DataField="NombreUsuarioModificacion" HeaderText="MODIFICADO POR" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="FHUltimaActualizacionEstado" HeaderText="ULTIMA MODIFICACIÓN" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="NombreProvincia" HeaderText="PROVINCIA" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="FHDesde" HeaderText="FECHA INICIO" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="DuracionDias" HeaderText="DURACIÓN DIAS" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />

                                                    <%-- Boton con link para ver detalles solicitud--%>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="ACCIONES" ItemStyle-Width="15%">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkVerDetalles" runat="server" CommandName="Detalle"
                                                                CommandArgument='<%# Eval("IdSolicitud") %>'>
                                                <i class="fa fa-eye" aria-hidden="true" style='font-size:15px;   color:#525252'/>  </i>
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="btnVerForo" ToolTip="Enviar Mensaje" runat="server" OnClick="btnVerForo_Click"
                                                                CommandArgument='<%# Eval("IdSolicitud") %>'>
                                                                    <i class="fa fa-comments" aria-hidden="true" style='font-size: 15px; color: #525252'></i>
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
                            <h5><asp:Label runat="server" Text="Acciones" /></h5>
                        </div>
                        <hr />
                        <div class="row justify-content-center">
                            <asp:Button runat="server" Text="Habilitar Modificación" CssClass="btn btn-danger" ID="Button1" OnClick="btnHabilitarModificacion_Click" />
                            &nbsp;
                            <asp:Button runat="server" Text="Devolver Solicitud" CssClass="btn btn-danger" ID="Button2" OnClick="btnDevolver_Click" />
                            &nbsp;
                            <asp:Button ID="Button3" Text="Aprobar Solicitud" CssClass="btn btn-success" runat="server" OnClick="btnAprobar_Click" />
                        </div>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-12">
                        <br />
                        <div class="panel-body">
                            <asp:UpdatePanel ID="upModalABM" runat="server">
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

                                        <%--FECHAS--%>
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
                                            <asp:Panel ID="pnlModalEstadoSolicitud" CssClass="col-xl-3 col-lg-3 col-md-3 col-sm-3 col-xs-12 form-group" runat="server">
                                                <asp:Label Text="Estado de Solicitud" runat="server" />
                                                <asp:TextBox runat="server" ID="txtModalEstadoSolicitud" CssClass="form-control" />
                                            </asp:Panel>
                                            <asp:Panel ID="pnlModalFechaUltimaActualizacion" CssClass="col-xl-3 col-lg-3 col-md-3 col-sm-3 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Fecha de Última Actualización" runat="server" />
                                                <asp:TextBox runat="server" ID="txtModalFechaUltimaActualizacion" CssClass="form-control" />
                                            </asp:Panel>
                                            <%--<asp:Panel ID="pnlBtnVerHistorialSolicitud" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-xs-12 text-right" runat="server">
                                                <asp:Button Text="Ver Historial" ID="btnVerHistorialSolicitud" CssClass="btn btn-info btn-dark" runat="server" OnClick="btnVerHistorialSolicitud_Click" />
                                            </asp:Panel>--%>
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
                                                        <asp:BoundField DataField="Nombre" HeaderText="NOMBRE" ItemStyle-Width="20%" />
                                                        <asp:BoundField DataField="Apellido" HeaderText="APELLIDO" ItemStyle-Width="20%" />
                                                        <asp:BoundField DataField="DNI" HeaderText="DNI" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="Telefono" HeaderText="TELÉFONO" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" />
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
                                            <div class="row">
                                                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12 form-group">
                                                    <%--GRILLA CON HISTORIAL DE CAMBIOS--%>
                                                    <asp:GridView
                                                        ID="gvHistorial"
                                                        runat="server"
                                                        AutoGenerateColumns="false"
                                                        CssClass="mGrid" PagerStyle-CssClass="pgr" RowStyle-Height="40px">
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

                                        <%--AFECTADOS ASOCIADOS A LA REA--%>
                                        <div class="row">
                                            <h5>Afectados asociados a la REA</h5>
                                        </div>
                                        <div class="row">
                                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12 form-group">
                                                <asp:GridView
                                                    ID="gvAfectados"
                                                    runat="server"
                                                    AutoGenerateColumns="false"
                                                    OnRowCommand="gvAfectados_RowCommand"
                                                    CssClass="mGrid" PagerStyle-CssClass="pgr" RowStyle-Height="40px">
                                                    <AlternatingRowStyle BackColor="white" />
                                                    <HeaderStyle BackColor="#20789f" Font-Bold="true" Font-Size="Large" ForeColor="White" />
                                                    <RowStyle BackColor="#e1dddd" />
                                                    <SelectedRowStyle BackColor="#669999" Font-Bold="true" ForeColor="white" />

                                                    <Columns>
                                                        <%-- El DataField debe contener el mismo nombre que la columna de la BD, que se recupera en BindGrid()--%>
                                                        <asp:BoundField DataField="IdInteresado" HeaderText="NRO. APROBADOR" ItemStyle-Width="20%" />
                                                        <asp:BoundField DataField="NombreInteresado" HeaderText="INTERESADO" ItemStyle-Width="20%" />
                                                        <asp:BoundField DataField="NombreProvincia" HeaderText="PROVINCIA" ItemStyle-Width="20%" />
                                                        <asp:BoundField DataField="Estado" HeaderText="NOMBREESTADO" ItemStyle-Width="20%" />
                                                        <asp:BoundField DataField="FHCoordinacion" HeaderText="FECHA APROBACIÓN" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:TemplateField HeaderText="ACCIONES" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%--BOTÓN PULGAR ARRIBA - SÓLO VISIBLE CUANDO EL INTERESADO RECHAZÓ O AÚN NO RESPONDE--%>
                                                                <asp:LinkButton ToolTip="Brindar Aprobación en lugar de Interesado" ID="lnkAprobarInteresado" runat="server" Visible='<%# !Eval("Estado").ToString().Equals("Aprobado") %>' CommandName="Aprobar" CommandArgument='<%# Eval("IdInteresadoSolicitud") %>'>
                                                                    <i class="fa fa-thumbs-up" aria-hidden="true" style='font-size:15px;color:#525252'/>  </i>
                                                                </asp:LinkButton>

                                                                <%--BOTÓN REENVIAR MAIL - SÓLO VISIBLE CUANDO EL INTERESADO AÚN NO RESPONDE--%>
                                                                <asp:LinkButton ToolTip="Reenviar Mail a Interesado" ID="lnkReenviarMail" runat="server" Visible='<%# Eval("Estado").ToString().Equals("Pendiente de Respuesta") %>' CommandName="Reenviar" CommandArgument='<%# Eval("IdInteresado") %>'>
                                                                    <i class="fa fa-reply" aria-hidden="true" style='font-size:15px;color:#525252'/>  </i>
                                                                </asp:LinkButton>

                                                                <%--BOTÓN LIMPIAR RESPUESTA - SÓLO VISIBLE CUANDO EL INTERESADO YA RESPONDIÓ--%>
                                                                <asp:LinkButton ToolTip="Limpiar la Respuesta del Interesado" ID="lnkLimpiarRespuesta" runat="server" Visible='<%# !Eval("Estado").ToString().Equals("Pendiente de Respuesta") %>' CommandName="Limpiar" CommandArgument='<%# Eval("IdCoordinacion") %>'>
                                                                    <i class="fa fa-arrows-repeat" aria-hidden="true" style='font-size:15px;color:#525252'/>  </i>
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                        <hr />

                                        <div class="row">
                                            <h5>Mensajes de Afectados</h5>
                                        </div>
                                        <div class="row">
                                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12 form-group">
                                                <asp:Panel runat="server" ID="pnlMensajes" class="alert alert-primary" role="alert">
                                                    <asp:Repeater ID="rptMensajes" runat="server">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRptNombreInteresado" Font-Bold="true" runat="server" Text='<%# Eval("NombreInteresado").ToString() + ", " + Eval("FHCoordinacion").ToString() %>' />
                                                            <asp:HiddenField ID="hdnRptIdInteresado" Value='<%# Eval("IdInteresado") %>' runat="server" />
                                                            <br />
                                                            <asp:Label ID="lblRptMensajeInteresado" runat="server" Text='<%# Eval("MensajeInteresado").ToString() %>' />
                                                            <hr />
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                        <hr />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <br />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
