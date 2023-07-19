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
        <h1 class="row justify-content-center mb-0 pb-0 pt-5">
            <label style="font-family: Azonix; letter-spacing: 3px" class="fw-normal mb-3 pb-2">Gestión de Solicitudes</label>
        </h1>
        <%--Se borra el AutoPostBack porq hay q cargar el dgv de otra forma.--%>
        <br />
        <%--<asp:Button ID="NuevaSolicitud" runat="server" Text="Nueva Solicitud" CssClass="btn btn-dark"/>--%>
    </div>
    <div style="text-align: end;">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:Button ID="btnNuevo" Text="Nueva Solicitud" CssClass="btn btn-primary btn-dark" runat="server" OnClick="btnNuevo_Click" />
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
                                <div class="form-group col-xl-3 col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                    <asp:Label runat="server" Text="Solicitante" />
                                    <asp:DropDownList runat="server" ID="ddlSolicitante" AutoPostBack="true" OnSelectedIndexChanged="ddlSolicitante_SelectedIndexChanged" CssClass="form-control select-single" />
                                </div>
                                <div class="form-group col-xl-3 col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                    <asp:Label runat="server" Text="Estado" />
                                    <asp:DropDownList runat="server" ID="ddlEstado" AutoPostBack="true" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged" CssClass="form-control select-single" />
                                </div>
                                <div class="form-group col-xl-3 col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                    <asp:Label runat="server" Text="Actividad" />
                                    <asp:DropDownList runat="server" ID="ddlActividad" AutoPostBack="true" OnSelectedIndexChanged="ddlActividad_SelectedIndexChanged" CssClass="form-control select-single" />
                                </div>
                                <div class="form-group col-xl-3 col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                    <asp:Label runat="server" Text="Provincia" />
                                    <asp:DropDownList runat="server" ID="ddlFiltroProvincia" AutoPostBack="true" OnSelectedIndexChanged="ddlActividad_SelectedIndexChanged" CssClass="form-control select-single" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-xl-3 col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                    <asp:Label runat="server" Text="Fecha Desde" />
                                    <asp:TextBox TextMode="Date" ID="txtFiltroFechaDesde" runat="server" AutoPostBack="true" OnTextChanged="txtFiltroFechaDesde_TextChanged" CssClass="form-control" />
                                </div>
                                <div class="form-group col-xl-3 col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                    <asp:Label runat="server" Text="Fecha Hasta" />
                                    <asp:TextBox TextMode="Date" ID="txtFiltroFechaHasta" runat="server" AutoPostBack="true" OnTextChanged="txtFiltroFechaHasta_TextChanged" CssClass="form-control" />
                                </div>
                            </div>
                            <br />
                            <div class="text-center">
                                <asp:Button ID="btnFiltrar" Text="Filtrar" CssClass="btn btn-info btn-dark" runat="server" Visible="false" OnClick="btnFiltrar_Click" />
                            </div>
                            <div class="text-center">
                                <asp:Button ID="btnPrueba" Visible="false" Text="Test Mail" CssClass="btn btn-dark" runat="server" OnClick="btnPrueba_Click" />
                            </div>

                            <div class="panel-body" style="display: flex; justify-content: center; align-items: center">
                                <div class="row" style="overflow: auto; height: 425px; width: 1400px;">
                                    <asp:UpdatePanel ID="upSolicitudes" Style="width: 100%;" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                        <ContentTemplate>

                                            <asp:GridView
                                                ID="gvSolicitud"
                                                runat="server"
                                                OnRowCommand="gvSolicitud_RowCommand"
                                                AutoGenerateColumns="false"
                                                CssClass="mGrid" PagerStyle-CssClass="pgr" RowStyle-Height="40px" OnRowDataBound="gvSolicitud_RowDataBound">
                                                <AlternatingRowStyle BackColor="white" />
                                                <HeaderStyle BackColor="#20789f" Font-Bold="true" Font-Size="Large" ForeColor="White" />
                                                <RowStyle BackColor="#e1dddd" />
                                                <SelectedRowStyle BackColor="#669999" Font-Bold="true" ForeColor="white" />

                                                <Columns>
                                                    <%-- El DataField debe contener el mismo nombre que la columna de la BD, que se recupera en BindGrid()--%>
                                                    <asp:BoundField DataField="IdSolicitud" HeaderText="ID" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="NombreUsuario" HeaderText="SOLICITANTE" ItemStyle-Width="20%" />
                                                    <asp:BoundField DataField="Nombre" HeaderText="NOMBRE SOLICITUD" ItemStyle-Width="20%" />
                                                    <asp:BoundField DataField="FHAlta" HeaderText="FECHA ALTA" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="NombreEstado" HeaderText="ESTADO" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" />

                                                    <%-- Boton con link para ver detalles solicitud--%>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="ACCIONES" ItemStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkVerDetalles" ToolTip="Ver Detalles" runat="server" OnClick="lnkVerDetalles_Click" CommandName="Detalle"
                                                                CommandArgument='<%# Eval("IdSolicitud") %>'>
                                                                <i class="fa fa-eye" aria-hidden="true" style='font-size: 15px; color: #525252' /></i>
                                                            </asp:LinkButton>

                                                            <asp:LinkButton ID="lnkEditar" Visible='<%# Eval("Editable").ToString().Equals("1") %>' ToolTip="Editar Solicitud" runat="server" OnClick="lnkEditar_Click" CommandName="Editar"
                                                                CommandArgument='<%# Eval("IdSolicitud") %>'>
                                                                <i class="fa fa-pencil" aria-hidden="true" style='font-size: 15px; color: #525252'></i>
                                                            </asp:LinkButton>

                                                            <asp:LinkButton ID="btnVerForo" ToolTip="Enviar Mensaje" runat="server" OnClick="btnVerForo_Click"
                                                                CommandArgument='<%# Eval("IdSolicitud") %>'>
                                                                <i class="fa fa-comments" aria-hidden="true" style='font-size: 15px; margin-left: 10px; color: #525252'></i>
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
                <asp:Panel runat="server" ID="pnlAcciones" Visible="false">

                    <div class="row">
                        <div class="col-12 alert alert-warning" role="alert">
                            <div class="row">
                                <h5>
                                    <asp:Label runat="server" Text="Acciones" /></h5>
                            </div>
                            <hr />
                            <div class="row justify-content-center">
                                <asp:Button runat="server" Text="Enviar Solicitud" CssClass="btn btn-danger" ID="btnenviarSolicitud" OnClick="enviarSolicitud_Click" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>

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
                                            <asp:HiddenField ID="hdnIdEstadoAnterior" runat="server" />
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
                                                <asp:TextBox runat="server" ID="txtModalFechaDesde" CssClass="form-control" TextMode="DateTimeLocal" />
                                            </asp:Panel>
                                            <asp:Panel CssClass="col-xl-3 col-lg-3 col-md-3 col-sm-3 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Fecha Hasta" runat="server" />
                                                <asp:TextBox runat="server" ID="txtModalFechaHasta" CssClass="form-control" TextMode="DateTimeLocal" />
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

                                        <asp:Panel ID="pnlVehiculos" runat="server">
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
                                                                                            <asp:CheckBox runat="server" ID="chkVANTVinculado" Checked='<%# Eval("Checked").ToString().Equals("0") ? false : true %>' />
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
                                        </asp:Panel>

                                        <%--UBICACIONES--%>
                                        <div class="row">
                                            <h5>Ubicaciones</h5>
                                        </div>
                                        <div class="row">
                                            <%--AGREGAR UBICACIONES--%>
                                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <div class="row">
                                                    <div class="col">
                                                        <asp:UpdatePanel runat="server">
                                                            <ContentTemplate>
                                                                <div class="row">
                                                                    <div class="col">
                                                                        <asp:Button ID="btnAgregarUbicacion" Text="Agregar Ubicación" runat="server" CssClass="btn btn-block btn-success" OnClick="btnAgregarUbicacion_Click" />
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col">
                                                        <div class="row">
                                                            <div class="col">
                                                                <asp:FileUpload runat="server" ID="fupKML" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                        <div class="row">
                                                            <div class="col">
                                                                <asp:Button ID="btnEscanearKML" Text="Escanear KML" runat="server" CssClass="btn btn-success" OnClick="btnEscanearKML_Click" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <br />

                                                <div class="row">
                                                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                        <%--NUEVA UBICACIÓN--%>
                                                        <asp:Panel ID="pnlAgregarUbicacion" class="alert alert-warning" role="alert" runat="server" Visible="false">
                                                            <div class="row">
                                                                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                    <div class="row">
                                                                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-xs-6">
                                                                            <asp:Label Text="Circunferencia" runat="server" />
                                                                            <asp:CheckBox ID="chkEsPoligono" runat="server" AutoPostBack="true" OnCheckedChanged="chkEsPoligono_CheckedChanged" />
                                                                            <asp:Label Text="Polígono" runat="server" />
                                                                            <asp:HiddenField ID="hdnUbicacionId" runat="server" />
                                                                        </div>
                                                                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-xs-6">
                                                                            <asp:Panel ID="pnlProvincia" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-sm-6 form-group" runat="server">
                                                                                <asp:Label Text="Provincia" runat="server" />
                                                                                <asp:DropDownList runat="server" ID="ddlProvincia" CssClass="form-control select-single" />
                                                                            </asp:Panel>
                                                                        </div>
                                                                    </div>
                                                                    <br />
                                                                    <%--NUEVA CIRCUNFERENCIA--%>
                                                                    <asp:Panel ID="pnlAgregarCircunferencia" class="alert alert-primary" role="alert" runat="server" Visible="false">
                                                                        <div class="row">
                                                                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                                <div class="row">
                                                                                    <asp:Panel runat="server" CssClass="col-xl-3 col-lg-3 col-md-3 col-sm-3 col-xs-3 form-group">
                                                                                        <asp:Label Text="Latitud (+/- XX.XXXX)" runat="server" />
                                                                                        <asp:TextBox ID="txtCircunferenciaLatitud" runat="server" CssClass="form-control" TextMode="Number" step="0.00000001" />
                                                                                    </asp:Panel>
                                                                                    <asp:Panel runat="server" CssClass="col-xl-3 col-lg-3 col-md-3 col-sm-3 col-xs-3 form-group">
                                                                                        <asp:Label Text="Longitud (+/- XX.XXXX)" runat="server" />
                                                                                        <asp:TextBox ID="txtCircunferenciaLongitud" runat="server" CssClass="form-control" TextMode="Number" step="0.00000001" />
                                                                                    </asp:Panel>
                                                                                    <asp:Panel runat="server" CssClass="col-xl-3 col-lg-3 col-md-3 col-sm-3 col-xs-3 form-group">
                                                                                        <asp:Label Text="Altura (Ground Level)" runat="server" />
                                                                                        <asp:TextBox ID="txtCircunferenciaAltura" runat="server" CssClass="form-control" TextMode="Number" step="0.00000001" />
                                                                                    </asp:Panel>
                                                                                    <asp:Panel runat="server" CssClass="col-xl-3 col-lg-3 col-md-3 col-sm-3 col-xs-3 form-group">
                                                                                        <asp:Label Text="Radio (km)" runat="server" />
                                                                                        <asp:TextBox ID="txtCircunferenciaRadio" runat="server" CssClass="form-control" TextMode="Number" step="0.00000001" />
                                                                                    </asp:Panel>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </asp:Panel>

                                                                    <%--NUEVO POLIGONO--%>
                                                                    <asp:Panel ID="pnlAgregarPoligono" class="alert alert-primary" role="alert" runat="server" Visible="false">
                                                                        <div class="row">
                                                                            <asp:UpdatePanel runat="server">
                                                                                <ContentTemplate>
                                                                                    <asp:Panel runat="server" CssClass="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12 text-right">
                                                                                        <asp:Button Text="Agregar Punto Geográfico" ID="btnAgregarPuntoGeografico" runat="server" CssClass="btn btn-success" OnClick="btnAgregarPuntoGeografico_Click" />
                                                                                    </asp:Panel>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                            <br />
                                                                        </div>

                                                                        <asp:Panel ID="pnlAgregarPuntoGeograficoYGrilla" runat="server" Visible="false">
                                                                            <div class="row">
                                                                                <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-xs-4">
                                                                                    <asp:Panel ID="pnlAgregarPuntoGeografico" runat="server" Visible="false">
                                                                                        <div class="row">
                                                                                            <asp:Panel runat="server" CssClass="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12 form-group">
                                                                                                <asp:Label Text="Latitud" runat="server" />
                                                                                                <asp:TextBox ID="txtPoligonoLatitud" runat="server" CssClass="form-control" TextMode="Number" step="0.00000001" />
                                                                                            </asp:Panel>
                                                                                            <asp:Panel runat="server" CssClass="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12 form-group">
                                                                                                <asp:Label Text="Longitud" runat="server" />
                                                                                                <asp:TextBox ID="txtPoligonoLongitud" runat="server" CssClass="form-control" TextMode="Number" step="0.00000001" />
                                                                                            </asp:Panel>
                                                                                            <asp:Panel runat="server" CssClass="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12 form-group">
                                                                                                <asp:Label Text="Altura (m.s.n.m.)" runat="server" />
                                                                                                <asp:TextBox ID="txtPoligonoAltura" runat="server" CssClass="form-control" TextMode="Number" step="0.00000001" />
                                                                                            </asp:Panel>
                                                                                        </div>
                                                                                    </asp:Panel>
                                                                                </div>
                                                                                <div class="col-xl-8 col-lg-8 col-md-8 col-sm-8 col-xs-8">
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
                                                                                <div class="col-12 text-right">
                                                                                    <asp:Button Text="Guardar Punto Geográfico" ID="btnGuardarPuntoGeografico" runat="server" CssClass="btn btn-success" OnClick="btnGuardarPuntoGeografico_Click" />
                                                                                </div>
                                                                            </div>
                                                                        </asp:Panel>
                                                                    </asp:Panel>

                                                                    <hr />
                                                                    <div class="row">
                                                                        <div class="col-12 text-right">
                                                                            	<asp:Button Text="Guardar Ubicación" ID="Button1" runat="server" CssClass="btn btn-success" OnClick="btnGuardarUbicacion_Click" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>
                                                </div>
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
                                                                <asp:HiddenField ID="hdnRptIdUbicacion" Value="0" runat="server" />
                                                                <asp:HiddenField ID="hdnRptIdProvincia" Value="0" runat="server" />
                                                                <asp:Label ID="lblRptDatos" runat="server" />
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>
                                        <hr />

                                        <%--TABLA UBICACIONES--%>



                                        <div class="row">
                                            <h5>Prueba Ubicaciones Cargadas</h5>
                                        </div>
                                        </br>
                                                                    </hr>

                                        <%--<asp:GridView ID="gridUbicaciones" runat="server" CssClass="ubicaciones-table" AutoGenerateColumns="False">--%>
                                        <asp:GridView ID="gridUbicaciones" runat="server"
                                            AutoGenerateColumns="false"
                                            CssClass="mGrid" PagerStyle-CssClass="pgr" RowStyle-Height="40px">
                                            <AlternatingRowStyle BackColor="white" />
                                            <HeaderStyle BackColor="#20789f" Font-Bold="true" Font-Size="Large" ForeColor="White" />
                                            <RowStyle BackColor="#e1dddd" />
                                            <SelectedRowStyle BackColor="#669999" Font-Bold="true" ForeColor="white" />
                                            <Columns>
                                                <asp:BoundField DataField="Id" HeaderText="ID" />
                                                <asp:BoundField DataField="IdUbicacion" HeaderText="IdUbicacion" />
                                                <asp:BoundField DataField="Poligono" HeaderText="Poligono" />                                                
                                                <asp:BoundField DataField="Latitud" HeaderText="Latitud" />
                                                <asp:BoundField DataField="Longitud" HeaderText="Longitud" />
                                                <asp:BoundField DataField="Radio" HeaderText="Radio" />
                                                <asp:BoundField DataField="Altura" HeaderText="Altura" />
                                                <asp:BoundField DataField="idProvincia" HeaderText="Provincia" />
                                                <asp:TemplateField HeaderText="Acciones">
                                                    <ItemTemplate>

                                                        <asp:LinkButton ID="btnEditarUbicacion" runat="server" OnClick="lnkModificar_Click" CommandName="Modificar"
                                                            CommandArgument='<%# Eval("Id") %>'>
                                                                <i class="fa fa-pencil" aria-hidden="true" style='font-size: 15px; color: #525252' /></i>
                                                        </asp:LinkButton>


                                                        <asp:LinkButton ID="btnEliminarUbicacion" runat="server" OnClick="lnkEliminar_Click" CommandName="Eliminar"
                                                            CommandArgument='<%# Eval("Id") %>'>
                                                                <i class="fa fa-trash-can" aria-hidden="true" style='font-size: 15px; color: #525252' /></i>
                                                        </asp:LinkButton>

                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>

                                    </div>

                                    </br>
                                    </hr>


		



                                        <%--TRIPULANTES--%>
                                    <div class="row">
                                        <h5>Tripulantes</h5>
                                    </div>
                                    <asp:UpdatePanel ID="upTripulacion" runat="server">
                                        <ContentTemplate>
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
                                                            <asp:BoundField DataField="Telefono" HeaderText="TELEFONO" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" />

                                                            <%-- Boton con link para ver detalles solicitud--%>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="SELECCIONAR" ItemStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <asp:HiddenField Value='<%# Eval("IdTripulacion") %>' runat="server" ID="hdnIdTripulacion" />
                                                                    <asp:CheckBox runat="server" ID="chkTripulacionVinculado" Checked='<%# Eval("Checked").ToString().Equals("0") ? false : true %>' Visible='<%# Eval("Documentacion").ToString().Equals("0") ? false : true %>' />
                                                                    <asp:Panel runat="server" ID="pnlAdvertencia" ToolTip="Este Tripulante no posee documentación aprobada por EANA." Visible='<%# Eval("Documentacion").ToString().Equals("0") ? true : false %>'><i class="fa fa-solid fa-triangle-exclamation"></i></asp:Panel>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>


                                    <%--HISTORIAL DE ESTADOS--%>
                                    <hr />
                                    <asp:Panel ID="pnlHistorialSolicitud" runat="server" Visible="true">
                                        <div class="row">
                                            <h5>Historial de Estados</h5>
                                        </div>
                                        <br />
                                        <div class="row">
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

                                    <hr />
                                    <div class="row">
                                        <div class="col-12 text-right">
                                            <asp:Button ID="btnGuardar" Text="Guardar" CssClass="btn btn-success" runat="server" OnClick="btnGuardar_Click" />
                                        </div>
                                    </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnEscanearKML" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnEscanearKML" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
