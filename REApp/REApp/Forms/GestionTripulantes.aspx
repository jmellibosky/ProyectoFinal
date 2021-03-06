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
        .btn
        {
            margin-left: 5px;
        }

        .btn-linkbutton
        {
            width:45px;
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
                font-size:15px;
                position:absolute sticky;
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


        button{
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
            <h2 class="page-header">
                <asp:Label ID="lblTitulo" Text="Gestión de Tripulantes" runat="server" />

                <div style="text-align: end;">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnNuevo" Text="Nuevo" CssClass="btn btn-primary btn-dark" runat="server" OnClick="btnNuevo_Click" />
                            <asp:Button ID="btnVolver" Text="Volver al Listado" Visible="false" CssClass="btn btn-info btn-dark" runat="server" OnClick="btnVolver_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </h2>
        </div>
    </div>

    <asp:UpdatePanel ID="upForm" runat="server">
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
                                    <asp:DropDownList runat="server" ID="ddlSolicitante" CssClass="form-control select-single" />
                                </asp:Panel>
                            </div>
                            <br />
                            <div class="col-12 text-center" id="divFiltrar" runat="server">
                                <asp:Button ID="btnFiltrar" Text="Filtrar" CssClass="btn btn-info btn-dark" runat="server" OnClick="btnFiltrar_Click" />
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
                        <div class="panel-body" style="display: flex; justify-content: center; align-items:center">
                            <div class="row" style="overflow: auto;height: 500px; width: 1200px; " >
                                <asp:UpdatePanel ID="upTripulantes" Style="width: 100%;" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView
                                            ID="gvTripulantes"
                                            runat="server"
                                            OnRowCommand="gvTripulantes_RowCommand"
                                             AutoGenerateColumns="false" 
                                             CssClass="mGrid" PagerStyle-CssClass="pgr" RowStyle-Height="40px">
                                            <AlternatingRowStyle BackColor="white" />
                                            <HeaderStyle BackColor="#20789f" Font-Bold="true" Font-Size="Large" ForeColor="White" />
                                            <RowStyle BackColor="#e1dddd" />
                                            <SelectedRowStyle BackColor="#669999" Font-Bold="true" ForeColor="white" />
                                            <Columns>
                                                <asp:BoundField HeaderText="NOMBRE" DataField="Nombre" ItemStyle-Width="20%"/>
                                                <asp:BoundField HeaderText="APELLIDO" DataField="Apellido" ItemStyle-Width="20%"/>
                                                <asp:BoundField HeaderText="DNI" DataField="DNI" ItemStyle-Width="20%"/>
                                                <asp:BoundField HeaderText="TELEFONO" DataField="Telefono" ItemStyle-Width="20%"/>
                                                <asp:TemplateField HeaderText="ACCIONES" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnVerDetalle" CommandName="Detalle" CommandArgument='<%# Eval("IdTripulacion") %>' runat="server">
                                                            <i class="fa fa-pencil" aria-hidden="true" style='font-size:15px; margin-left: 10px; color:#525252' ></i>    
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="btnEliminar"  CommandName="Eliminar" CommandArgument='<%# Eval("IdTripulacion") %>' runat="server">
                                                            <i class="fa fa-trash-can" aria-hidden="true" style='font-size:15px; margin-left: 25px; color:#525252' ></i>
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
                                                <asp:TextBox runat="server" ID="txtModalFechaNacimiento" CssClass="form-control" />
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
                                        <div class="row">
                                            <div style="justify-content: center;">
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
