<%@ Page
    Title="Gestión de Tripulantes"
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="GestionTripulantes.aspx.cs"
    MasterPageFile="~/Site.Master"
    Inherits="REApp.Forms.GestionTripulantes" %>

<%--Head--%>
<asp:Content ID="cHead" ContentPlaceHolderID="cphHead" runat="server">
    <%-- CSS --%>
    <style>
        .btn {
            margin-left: 5px;
        }

        .btn-linkbutton {
            width: 45px;
        }

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

    <%-- JS --%>
    <script>
        function ShowModal() {
            $('#modalABM').modal('show');
        }
    </script>
</asp:Content>

<%-- Body --%>
<asp:Content ID="cBody" ContentPlaceHolderID="cphBody" runat="server">



    <%-- Encabezado --%>
    <div class="row">
        <div class="col-12">

            <h1 class="row justify-content-center">
                <label class="fw-normal mb-3 pb-2">Gestión de Tripulantes</label>
            </h1>
            <br />
            <div style="text-align: end;">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnNuevo" Text="Nuevo" CssClass="btn btn-primary btn-dark" runat="server" OnClick="btnNuevo_Click" />
                        <asp:Button ID="btnVolver" Text="Volver al Listado" Visible="false" CssClass="btn btn-info btn-dark" runat="server" OnClick="btnVolver_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>
    </div>

    <asp:UpdatePanel ID="upForm" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
            <asp:Panel ID="pnlListado" runat="server">
                <%-- Contenido --%>
                <div class="row">
                    <div class="col-12">
                        <br />
                        <div class="panel-body">
                            <div class="row">
                                <%-- Combo Solicitantes --%>
                                <asp:Panel CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-6 col-xs-12 form-group" runat="server" ID="pnlSolicitante">
                                    <asp:Label Text="Solicitantes" runat="server" />
                                    <asp:DropDownList runat="server" ID="ddlSolicitante" CssClass="form-control select-single" AutoPostBack="true" OnSelectedIndexChanged="ddlSolicitante_SelectedIndexChanged" />
                                </asp:Panel>
                                <%-- Combo Documentación --%>
                                <asp:Panel CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-6 col-xs-12 form-group" runat="server" ID="pnlDocumentacion">
                                    <asp:Label Text="Documentación" runat="server" />
                                    <asp:DropDownList runat="server" ID="ddlDocumentación" CssClass="form-control select-single" AutoPostBack="true" OnSelectedIndexChanged="ddlDocumentación_SelectedIndexChanged">
                                        <asp:ListItem Text="Todos" Value="0" />
                                        <asp:ListItem Text="Vigente" Value="1" />
                                        <asp:ListItem Text="Vencida" Value="2" />
                                        <asp:ListItem Text="Faltante" Value="3" />
                                    </asp:DropDownList>
                                </asp:Panel>
                            </div>
                            <br />
                            <div class="col-12 text-center" id="divFiltrar" runat="server">
                                <asp:Button ID="btnFiltrar" Text="Filtrar" CssClass="btn btn-info btn-dark" runat="server" OnClick="btnFiltrar_Click" Visible="false" />
                            </div>
                            <br />
                            <div class="row">
                                <asp:Panel ID="pnlAlertaEliminar" CssClass="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12 form-group" runat="server" Visible="false">
                                    <div class="alert alert-info" role="alert">
                                        <h5>
                                            <asp:Label runat="server" Text="Confirmar Eliminación" />
                                        </h5>
                                        <hr />
                                        <div class="row">
                                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12 form-group">
                                                <asp:HiddenField ID="hdnEliminar" runat="server" />
                                                <asp:Label ID="lblMensajeEliminacion" runat="server" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12 form-group">
                                                <asp:Button ID="btnCancelarEliminacion" CssClass="btn btn-warning" runat="server" Text="Cancelar" OnClick="btnCancelarEliminacion_Click" />
                                                <asp:Button ID="btnConfirmarEliminacion" CssClass="btn btn-danger" runat="server" Text="Confirmar" OnClick="btnConfirmarEliminacion_Click" />
                                            </div>
                                        </div>
                                    </div>
                                    <hr />
                                </asp:Panel>
                            </div>
                            <div class="row">
                                <div class="panel-body" style="display: flex; justify-content: center; align-items: center">
                                    <div class="row" style="overflow: auto; height: 400px; width: 1175px;">
                                        <asp:UpdatePanel ID="upTripulantes" Style="width: 100%;" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                            <ContentTemplate>
                                                <asp:GridView
                                                    ID="gvTripulantes"
                                                    runat="server"
                                                    OnRowCommand="gvTripulantes_RowCommand"
                                                    AutoGenerateColumns="false"
                                                    CssClass="mGrid" PagerStyle-CssClass="pgr" RowStyle-Height="40px" OnRowDataBound="gvTripulantes_RowDataBound">
                                                    <AlternatingRowStyle BackColor="white" />
                                                    <HeaderStyle BackColor="#20789f" Font-Bold="true" Font-Size="Large" ForeColor="White" />
                                                    <RowStyle BackColor="#e1dddd" />
                                                    <SelectedRowStyle BackColor="#669999" Font-Bold="true" ForeColor="white" />
                                                    <Columns>
                                                        <asp:BoundField HeaderText="NOMBRE" DataField="Nombre" ItemStyle-Width="20%" />
                                                        <asp:BoundField HeaderText="APELLIDO" DataField="Apellido" ItemStyle-Width="20%" />
                                                        <asp:BoundField HeaderText="DNI" DataField="DNI" ItemStyle-Width="20%" />
                                                        <asp:BoundField HeaderText="TELÉFONO" DataField="Telefono" ItemStyle-Width="20%" />
                                                        <asp:TemplateField HeaderText="ACCIONES" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <div class="row">
                                                                    <div class="col-12">
                                                                        <asp:LinkButton ID="btnEditar" CssClass="btn btn-warning" CommandName="Editar" ToolTip='<%# (Eval("VinculadoSolicitud").ToString() == "1") ? "Este tripulante se encuentra vinculado a una solicitud vigente." : "" %>' Enabled='<%# (Eval("VinculadoSolicitud").ToString() == "1") ? false : true %>' CommandArgument='<%# Eval("IdTripulacion") %>' runat="server">
                                                            <i class="fa fa-pencil" aria-hidden="true" style='font-size:15px; color:#525252' ></i>    
                                                                        </asp:LinkButton>
                                                                        <asp:LinkButton ID="btnEliminar" CssClass="btn btn-danger" CommandName="Eliminar" ToolTip='<%# (Eval("VinculadoSolicitud").ToString() == "1") ? "Este tripulante se encuentra vinculado a una solicitud vigente." : "" %>' Enabled='<%# (Eval("VinculadoSolicitud").ToString() == "1") ? false : true %>' CommandArgument='<%# Eval("IdTripulacion") %>' runat="server">
                                                            <i class="fa fa-trash-can" aria-hidden="true" style='font-size:15px; color:#525252' ></i>
                                                                        </asp:LinkButton>
                                                                        <asp:LinkButton ID="btnVerDetalle" CssClass="btn btn-secondary" CommandName="Detalle" CommandArgument='<%# Eval("IdTripulacion") %>' runat="server">
                                                            <i class="fa fa-eye" aria-hidden="true" style='font-size:15px;   color:#525252'/>  </i>
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="gvTripulantes" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>


            <%--Panel ABM--%>
            <asp:UpdatePanel runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnlABM" runat="server" Visible="false">
                        <div class="row">
                            <div class="col-12">
                                <br />
                                <div class="panel-body">

                                    <div class="col-12">
                                        <div class="row">
                                            <asp:Panel ID="pnlModalSolicitante" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Solicitante" runat="server" />
                                                <asp:DropDownList runat="server" ID="ddlModalSolicitante" CssClass="form-control select-single" />
                                            </asp:Panel>
                                            <asp:HiddenField ID="hdnIdTripulacion" runat="server" />
                                        </div>
                                        <br />
                                        <div class="row">
                                            <h6>Datos Personales</h6>
                                        </div>
                                        <div class="row">
                                            <asp:Panel ID="pnlModalNombre" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Nombre" runat="server" />
                                                <asp:TextBox runat="server" ID="txtModalNombre" CssClass="form-control" />
                                            </asp:Panel>

                                            <asp:Panel ID="pnlModalApellido" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Apellido" runat="server" />
                                                <asp:TextBox runat="server" ID="txtModalApellido" CssClass="form-control" />
                                            </asp:Panel>
                                        </div>
                                        <div class="row">
                                            <asp:Panel ID="pnlModalDNI" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="DNI" runat="server" />
                                                <asp:TextBox runat="server" ID="txtModalDNI" CssClass="form-control" />
                                            </asp:Panel>

                                            <asp:Panel ID="pnlModalFechaNacimiento" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Fecha de Nacimiento" runat="server" />
                                                <asp:TextBox runat="server" ID="txtModalFechaNacimiento" CssClass="form-control" TextMode="Date" />
                                            </asp:Panel>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <h6>Datos de Contacto</h6>
                                        </div>
                                        <div class="row">
                                            <asp:Panel ID="pnlModalTelefono" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Teléfono" runat="server" />
                                                <asp:TextBox runat="server" ID="txtModalTelefono" CssClass="form-control" />
                                            </asp:Panel>

                                            <asp:Panel ID="pnlModalCorreo" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Correo Electrónico" runat="server" />
                                                <asp:TextBox runat="server" ID="txtModalCorreo" CssClass="form-control" />
                                            </asp:Panel>
                                        </div>
                                        <hr />
                                        <div class="row">
                                            <asp:Panel ID="pnlError" Visible="false" CssClass="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12 form-group" runat="server">
                                                <div class="alert alert-danger" role="alert">
                                                    <h5>
                                                        <asp:Label ID="txtErrorHeader" runat="server" /></h5>
                                                    <hr />
                                                    <asp:Label ID="txtErrorBody" runat="server" />
                                                </div>
                                                <hr />
                                            </asp:Panel>
                                        </div>
                                        <%--Subida de Archivos--%>
                                        <%--File Upload CM--%>
                                        <div class="row">
                                            <div class="col-sm-6">
                                                <asp:Label runat="server" CssClass="font-weight-bold" ForeColor="black" ID="lblCertM">Certificado Médico</asp:Label>
                                                <asp:HiddenField ID="hdnFuCM" runat="server" />
                                                <hr />
                                                <asp:Panel runat="server" ID="pnlFuCMTripulante" Visible="true">
                                                    <div class="row">
                                                        <asp:FileUpload ID="FileUploadCMATrip" runat="server" />
                                                    </div>
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="panelGvCertMedicoTripulante" Visible="true">
                                                    <asp:GridView ID="gvCertMedicoTripulante"
                                                        runat="server"
                                                        AutoGenerateColumns="false"
                                                        CssClass="mGrid" PagerStyle-CssClass="pgr" RowStyle-Height="40px" OnRowDataBound="gvCertMedicoTripulante_RowDataBound">
                                                        <AlternatingRowStyle BackColor="white" />
                                                        <HeaderStyle BackColor="#20789f" Font-Bold="true" Font-Size="XX-Small" ForeColor="White" />
                                                        <RowStyle BackColor="#e1dddd" />
                                                        <SelectedRowStyle BackColor="#669999" Font-Bold="true" ForeColor="white" />
                                                        <Columns>
                                                            <%-- El DataField debe contener el mismo nombre que la columna de la BD, que se recupera en BindGrid()--%>
                                                            <asp:BoundField DataField="IdDocumento" HeaderText="ID" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField DataField="IdTipoDocumento" HeaderText="ID DOC" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField DataField="Nombre" HeaderText="DOCUMENTO" ItemStyle-Width="10%" />
                                                            <asp:BoundField DataField="FHAlta" HeaderText="FECHA ALTA" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField DataField="FHVencimiento" HeaderText="FECHA VENCIMIENTO" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />
                                                            <asp:TemplateField ItemStyle-Width="15%" ItemStyle-Wrap="false" HeaderText="ACCIONES" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <%-- Boton con link para descargar archivo--%>
                                                                    <asp:LinkButton ID="lnkDownload" runat="server" OnClick="lnkDownload_Click"
                                                                        CommandArgument='<%# Eval("IdDocumento") %>'>
                                                <i class="fas fa-file-pdf" aria-hidden="true" style='font-size: 15px; color: #525252'></i>
                                                                    </asp:LinkButton>
                                                                    <%--Boton para eliminar archivo de la BD--%>
                                                                    <asp:LinkButton ID="lnkEliminarArchivo" runat="server" OnClick="lnkEliminarArchivo_Click"
                                                                        CommandArgument='<%# Eval("IdDocumento") %>'>
                                                <i class="fa fa-trash-can" aria-hidden="true" style='font-size: 15px; color: #525252'></i>
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                                <br />
                                                <asp:Panel runat="server" ID="pnlFechaVencimientoCMTripulante">
                                                    <div class="row">
                                                        <asp:Label CssClass="width: 50%; text-align: right; text-md-center font-weight-bold" runat="server">Fecha de Vencimiento:&nbsp &nbsp</asp:Label>
                                                        <input type="date" class="form-control" id="txtFechaVencimientoCertMedicoTripulante" runat="server" />
                                                        <hr />
                                                    </div>
                                                </asp:Panel>
                                                <hr />
                                                <br />
                                                <br />
                                            </div>
                                        </div>
                                        <%--File Upload Certificado Competencia--%>
                                        <div class="row">
                                            <div class="col-sm-6">
                                                <asp:Label runat="server" CssClass="font-weight-bold" ForeColor="black" ID="Label1">Certificado de Competencia</asp:Label>
                                                <asp:HiddenField ID="hdnFuCertComp" runat="server" />
                                                <hr />
                                                <asp:Panel runat="server" ID="pnlFUCertCompetenciaTripulante" Visible="true">
                                                    <div class="row">
                                                        <asp:FileUpload ID="FileUploadCertCompetenciaTripulante" runat="server" />
                                                    </div>
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="pnlCertCompetenciaTripulante" Visible="true">
                                                    <asp:GridView ID="gvCertCompetenciaTripulante"
                                                        runat="server"
                                                        AutoGenerateColumns="false"
                                                        CssClass="mGrid" PagerStyle-CssClass="pgr" RowStyle-Height="40px" OnRowDataBound="gvCertCompetenciaTripulante_RowDataBound">
                                                        <AlternatingRowStyle BackColor="white" />
                                                        <HeaderStyle BackColor="#20789f" Font-Bold="true" Font-Size="XX-Small" ForeColor="White" />
                                                        <RowStyle BackColor="#e1dddd" />
                                                        <SelectedRowStyle BackColor="#669999" Font-Bold="true" ForeColor="white" />
                                                        <Columns>
                                                            <%-- El DataField debe contener el mismo nombre que la columna de la BD, que se recupera en BindGrid()--%>
                                                            <asp:BoundField DataField="IdDocumento" HeaderText="ID" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField DataField="IdTipoDocumento" HeaderText="ID DOC" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField DataField="Nombre" HeaderText="DOCUMENTO" ItemStyle-Width="10%" />
                                                            <asp:BoundField DataField="FHAlta" HeaderText="FECHA ALTA" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField DataField="FHVencimiento" HeaderText="FECHA VENCIMIENTO" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />
                                                            <asp:TemplateField ItemStyle-Width="15%" ItemStyle-Wrap="false" HeaderText="ACCIONES" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <%-- Boton con link para descargar archivo--%>
                                                                    <asp:LinkButton ID="lnkDownload" runat="server" OnClick="lnkDownload_Click"
                                                                        CommandArgument='<%# Eval("IdDocumento") %>'>
                                                <i class="fas fa-file-pdf" aria-hidden="true" style='font-size: 15px; color: #525252'></i>
                                                                    </asp:LinkButton>
                                                                    <%--Boton para eliminar archivo de la BD--%>
                                                                    <asp:LinkButton ID="lnkEliminarArchivo" runat="server" OnClick="lnkEliminarArchivo_Click"
                                                                        CommandArgument='<%# Eval("IdDocumento") %>'>
                                                <i class="fa fa-trash-can" aria-hidden="true" style='font-size: 15px; color: #525252'></i>
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>

                                                <br />
                                                <asp:Panel runat="server" ID="pnlFechaVencimientoCertCompetenciaTripulante">
                                                    <div class="row">
                                                        <asp:Label CssClass="width: 50%; text-align: right; text-md-center font-weight-bold" runat="server">Fecha de Vencimiento:&nbsp &nbsp</asp:Label>
                                                        <input type="date" class="form-control" id="txtFechaDeVencimientoCertCompetenciaTripulante" runat="server" />
                                                        <hr />
                                                    </div>
                                                </asp:Panel>

                                                <hr />
                                                <br />
                                                <br />
                                            </div>
                                        </div>
                                        <%--Boton Guardar  --%>
                                        <div class="row">
                                            <div style="justify-content: center;">
                                                <asp:Button ID="btnGuardar" Text="Guardar" CssClass="btn btn-success" runat="server" OnClick="btnGuardar_Click" />
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnGuardar" />
                    <asp:PostBackTrigger ControlID="gvCertMedicoTripulante" />
                    <asp:PostBackTrigger ControlID="gvCertCompetenciaTripulante" />
                </Triggers>
            </asp:UpdatePanel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnNuevo" />
            <asp:PostBackTrigger ControlID="btnVolver" />
            <asp:PostBackTrigger ControlID="btnFiltrar" />
            <asp:PostBackTrigger ControlID="ddlSolicitante" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>


<%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
    <ContentTemplate>
        <div class="modal-content">
            <div class="modal-header">
                <h3>
                    <asp:Label Text="Agregar Multimedia" runat="server" /></h3>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <div class="row divTipoMultimedia" id="divTipoMultimedia">
                        <asp:Label runat="server" Text="Subir Imagen o Audio" />
                        &nbsp;
                                           
                        <asp:CheckBox runat="server" AutoPostBack="true" ID="chkTipoMultimedia" CssClass="switch" OnCheckedChanged="chkTipoMultimedia_CheckedChanged" />
                        &nbsp;
                                           
                        <asp:Label runat="server" Text="Subir Video" />
                    </div>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlSubirArchivo" runat="server">
                                <div class="row">
                                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <asp:Label Text="Subir Imagen o Audio" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <asp:FileUpload CssClass="form-control file" ID="fileArchivo" runat="server" accept="image/x-png,image/jpeg,audio/mpeg" data-show-preview="false" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <asp:Label Text="(Tamaño máximo: 10MB)" CssClass="pull-right" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnlSubirVideo" Visible="false" runat="server">
                                <div class="row">
                                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <asp:Label Text="Subir Video" runat="server"></asp:Label>
                                        <asp:TextBox Placeholder="Ingresar Link..." ID="txtLinkVideo" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="row" style="margin-top: 10px;">
                        <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <center>
                                <asp:Button ID="btnCancelarSubirArchivo" CssClass="btn btn-info" Text="Cancelar" runat="server" OnClick="btnCancelarSubirArchivo_Click" />
                                <asp:Button ID="btnGuardarArchivo" CssClass="btn btn-success" Text="Guardar" runat="server" OnClick="btnGuardarArchivo_Click" />
                            </center>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnGuardarArchivo" />
    </Triggers>
</asp:UpdatePanel>--%>
