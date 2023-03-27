<%@ Page
    Title="Gestión de Interesados"
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="GestionInteresados.aspx.cs"
    MasterPageFile="~/Site.Master"
    Inherits="REApp.Forms.GestionInteresados" %>

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

            <h1 class="row justify-content-center mb-0 pb-0 pt-5">
                <label style="font-family:Azonix; letter-spacing: 3px" class="fw-normal mb-3 pb-2">Gestión de Interesados</label>
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
                                        <asp:UpdatePanel ID="upInteresados" Style="width: 100%;" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                            <ContentTemplate>
                                                <asp:GridView
                                                    ID="gvInteresados"
                                                    runat="server"
                                                    OnRowCommand="gvInteresados_RowCommand"
                                                    AutoGenerateColumns="false"
                                                    CssClass="mGrid" PagerStyle-CssClass="pgr" RowStyle-Height="40px">
                                                    <AlternatingRowStyle BackColor="white" />
                                                    <HeaderStyle BackColor="#20789f" Font-Bold="true" Font-Size="Large" ForeColor="White" />
                                                    <RowStyle BackColor="#e1dddd" />
                                                    <SelectedRowStyle BackColor="#669999" Font-Bold="true" ForeColor="white" />
                                                    <Columns>
                                                        <asp:BoundField HeaderText="NOMBRE" DataField="Nombre" ItemStyle-Width="20%" />
                                                        <asp:BoundField HeaderText="APELLIDO" DataField="Apellido" ItemStyle-Width="20%" />
                                                        <asp:BoundField HeaderText="EMAIL" DataField="Email" ItemStyle-Width="20%" />
                                                        <asp:BoundField HeaderText="TELÉFONO" DataField="Telefono" ItemStyle-Width="20%" />
                                                        <asp:TemplateField HeaderText="ACCIONES" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="btnEditar" CommandName="Editar" CommandArgument='<%# Eval("IdInteresado") %>' runat="server">
                                                            <i class="fa fa-pencil" aria-hidden="true" style='font-size:15px; color:#525252' ></i>    
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnEliminar" CommandName="Eliminar" CommandArgument='<%# Eval("IdInteresado") %>' runat="server">
                                                            <i class="fa fa-trash-can" aria-hidden="true" style='font-size:15px; color:#525252' ></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnVerDetalle" CommandName="Detalle" CommandArgument='<%# Eval("IdInteresado") %>' runat="server">
                                                            <i class="fa fa-eye" aria-hidden="true" style='font-size:15px;   color:#525252'/>  </i>
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="gvInteresados" />
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
                                    <asp:HiddenField runat="server" ID="hdnIdInteresado" />
                                    <div class="col-12">
                                        <div class="row">
                                            <div class="col-xl-4 col-lg-4 col-md-6 col-12 form-group">
                                                <asp:Label Text="Nombre" runat="server" />
                                                <asp:TextBox runat="server" ID="txtNombre" CssClass="form-control" />
                                            </div>
                                            <div class="col-xl-4 col-lg-4 col-md-6 col-12 form-group">
                                                <asp:Label Text="Apellido" runat="server" />
                                                <asp:TextBox runat="server" ID="txtApellido" CssClass="form-control" />

                                            </div>
                                            <div class="col-xl-4 col-lg-4 col-md-6 col-12 form-group">
                                                <asp:Label Text="Email" runat="server" />
                                                <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" />

                                            </div>
                                            <div class="col-xl-4 col-lg-4 col-md-6 col-12 form-group">
                                                <asp:Label Text="Teléfono" runat="server" />
                                                <asp:TextBox runat="server" ID="txtTelefono" CssClass="form-control" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-12 form-group">
                                                <asp:Label Text="Observaciones" runat="server" />
                                                <asp:TextBox runat="server" ID="txtObservaciones" CssClass="form-control" TextMode="MultiLine" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-12">

                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        <div class="row">

                                                            <div class="col-xl-4 col-lg-4 col-md-6 col-12 form-group">
                                                                <asp:Label Text="Provincia" runat="server" />
                                                                <asp:DropDownList CssClass="form-control select-single" ID="ddlProvincia" AutoPostBack="true" OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged" runat="server" />
                                                            </div>
                                                            <div class="col-xl-4 col-lg-4 col-md-6 col-12 form-group">
                                                                <asp:Label Text="Localidad" runat="server" />
                                                                <asp:DropDownList CssClass="form-control select-single" ID="ddlLocalidad" runat="server" />
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <%--Boton Guardar  --%>
                                        <div class="row">
                                            <div style="justify-content: center;">
                                                <asp:Button ID="btnGuardar" Text="Guardar" CssClass="btn btn-success float-end" runat="server" OnClick="btnGuardar_Click" />
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
                </Triggers>
            </asp:UpdatePanel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnNuevo" />
            <asp:PostBackTrigger ControlID="btnVolver" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
