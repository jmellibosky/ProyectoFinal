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
                            <asp:Button ID="btnNuevo" Text="Nuevo" CssClass="btn btn-primary" runat="server" OnClick="btnNuevo_Click" />
                            <asp:Button ID="btnVolver" Text="Volver al Listado" Visible="false" CssClass="btn btn-info" runat="server" OnClick="btnVolver_Click" />
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
                                <asp:Button ID="btnFiltrar" Text="Filtrar" CssClass="btn btn-info" runat="server" OnClick="btnFiltrar_Click" />
                            </div>
                            <br />
                            <div class="row">
                                <asp:UpdatePanel ID="upTripulantes" Style="width: 100%;" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView
                                            ID="gvTripulantes"
                                            runat="server"
                                            OnRowCommand="gvTripulantes_RowCommand"
                                            AutoGenerateColumns="false"
                                            EmptyDataText="No hay datos."
                                            Width="100%">
                                            <Columns>
                                                <asp:BoundField HeaderText="Nombre" DataField="Nombre" />
                                                <asp:BoundField HeaderText="Apellido" DataField="Apellido" />
                                                <asp:BoundField HeaderText="DNI" DataField="DNI" />
                                                <asp:BoundField HeaderText="Teléfono" DataField="Telefono" />
                                                <asp:TemplateField HeaderText="Ver Detalles">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnVerDetalle" CommandName="Detalle" CommandArgument='<%# Eval("IdTripulacion") %>' runat="server">
                                                            <i class="fa fa-info-circle" aria-hidden="true" />    
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Eliminar">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnEliminar" CommandName="Eliminar" CommandArgument='<%# Eval("IdTripulacion") %>' runat="server">
                                                            <i class="fa fa-times" aria-hidden="true" />    
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
                                            <div style="justify-content:center;">
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
