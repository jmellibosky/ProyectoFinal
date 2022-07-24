<%@ Page Title="" Language="C#" 
    MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" 
    CodeBehind="GestionUsuarios.aspx.cs" 
    Inherits="REApp.Forms.GestionUsuarios2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
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
<<<<<<< Updated upstream
            <ContentTemplate>
                <asp:Button ID="btnNuevo111" Text="Nueva Solicitud" CssClass="btn btn-primary" runat="server" OnClick="btnNuevo_Click" />
=======
            <ContentTemplate> <%-- Cambio nombre--%>
                <asp:Button ID="btnNew" Text="Nueva Solicitud" CssClass="btn btn-primary" runat="server" OnClick="btnNuevo_Click" />
>>>>>>> Stashed changes
                <asp:Button ID="btnVolver" Text="Volver al Listado" Visible="false" CssClass="btn btn-info" runat="server" OnClick="btnVolver_Click" />
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
                <asp:Label runat="server">Listado de Usuarios</asp:Label>

                <asp:UpdatePanel ID="upUsuarios" Style="width: 100%;" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>

                    <asp:GridView 
                        ID="gvUsuarios" 
                        runat="server" OnRowCommand="gvUsuarios_RowCommand"
                        AutoGenerateColumns="false" 
                        CssClass="table table-bordered table-condensed table-responsive table-hover"
                        Height="400px" Width="1000px">  
                        <AlternatingRowStyle BackColor="white" />
                        <HeaderStyle BackColor="#20789f" Font-Bold="true" Font-Size="Large" ForeColor="White" />
                        <RowStyle BackColor="#e1dddd" />
                        <SelectedRowStyle BackColor="#669999" Font-Bold="true" ForeColor="white" />

                        <Columns>  
                            <%-- El DataField debe contener el mismo nombre que la columna de la BD, que se recupera en BindGrid()--%>
                            <asp:BoundField DataField="IdUsuario" HeaderText="IdUsuario" ItemStyle-Width="100px" ItemStyle-Wrap="false"/>
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" ItemStyle-Width="100px" ItemStyle-Wrap="false"/>
                            <asp:BoundField DataField="Apellido" HeaderText="Apellido" ItemStyle-Width="300px" ItemStyle-Wrap="false"/>
                            <asp:BoundField DataField="Email" HeaderText="Email" ItemStyle-Width="100px" ItemStyle-Wrap="false"/>
                            <asp:BoundField DataField="NombreRol" HeaderText="NombreRol" ItemStyle-Width="100px" ItemStyle-Wrap="false"/>
                            <asp:BoundField DataField="Dni" HeaderText="NombreRol" ItemStyle-Width="100px" ItemStyle-Wrap="false"/>
                            <asp:BoundField DataField="Telefono" HeaderText="Telefono" ItemStyle-Width="100px" ItemStyle-Wrap="false"/>

                            <%-- Boton con link para ver detalles solicitud--%>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center"  ControlStyle-Width="80px" ItemStyle-Wrap="false" HeaderText="Acciones">  
                                <ItemTemplate>  
                                    <asp:LinkButton ID="btnDetalle" CommandName="DisplayUser" CommandArgument='<%# Eval("IdUsuario") %>' runat="server">
                                        <i class="fa fa-eye"/></i> Ver Detalles
                                    </asp:LinkButton>      
                                    <asp:LinkButton ID="btnUpdate" CommandName="UpdateUser" CommandArgument='<%# Eval("IdUsuario") %>' runat="server">
                                        <i class="fa fa-pen"/></i> Modificar
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnDelete" CommandName="Eliminar" CommandArgument='<%# Eval("IdUsuario") %>' runat="server">
                                        <i class="fa fa-trash" aria-hidden="true" style='font-size:24px;'></i> Eliminar
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
</asp:Panel>

<asp:Panel ID="pnlABM" runat="server" Visible="false">
                <div class="row">
                    <div class="col-12">
                        <br />
                        <div class="panel-body">
                            <asp:UpdatePanel ID="upModalABM" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                <ContentTemplate>
                                    <div class="col-12">
                                        <div class="col-12">
                                            <div class="row">
                                                <h6>Datos Personales</h6>
                                            </div>
                                            <div class="row">
                                                <asp:Panel ID="pnlModalNombreUsuario" CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-sm-12 form-group" runat="server">
                                                    <asp:Label Text="Nombre" runat="server" />
                                                    <asp:TextBox runat="server" ID="txtModalNombreUsuario" CssClass="form-control" />
                                                </asp:Panel>
                                                <asp:Panel ID="pnlModalApellidoUsuario" CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-sm-12 form-group" runat="server">
                                                    <asp:Label Text="Apellido" runat="server" />
                                                    <asp:TextBox runat="server" ID="txtModalApellidoUsuario" CssClass="form-control" />
                                                </asp:Panel>
                                                <asp:Panel ID="pnlModalRol" CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-sm-12 form-group" runat="server">
                                                    <%--Lo deje invisible al ddlModalSolicitante, ver--%>
                                                    <asp:Label Text="Rol" runat="server" />
                                                    <asp:DropDownList runat="server" ID="ddlModalRol" CssClass="form-control select-single"/>
                                                </asp:Panel>
                                            </div>
                                            <div class="row">
                                                <asp:Panel ID="pnlModalDni" CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-sm-12 form-group" runat="server">
                                                    <asp:Label Text="DNI" runat="server" />
                                                    <asp:TextBox runat="server" ID="txtModalDni" CssClass="form-control" />
                                                </asp:Panel>
                                                <asp:Panel ID="pnlModalTipoDni" CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-sm-12 form-group" runat="server">
                                                    <asp:Label Text="Tipo DNI" runat="server" />
                                                    <asp:TextBox runat="server" ID="txtModalTipoDni" CssClass="form-control" />
                                                </asp:Panel>
                                                <asp:Panel ID="pnlModalFechaNacimiento" CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-sm-12 form-group" runat="server">
                                                    <asp:Label Text="Fecha de Nacimiento" runat="server" />
                                                    <asp:TextBox runat="server" ID="txtModalFechaNac" CssClass="form-control" />
                                                </asp:Panel>
                                            </div>
                                            <div class="row">
                                                <h6>Datos de Contacto</h6>
                                            </div>
                                            <div class="row">
                                                <asp:Panel ID="pnlCorreo" CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-sm-12 form-group" runat="server">
                                                    <asp:Label Text="Correo Electrónico" runat="server" />
                                                    <asp:TextBox runat="server" ID="txtModalCorreo" CssClass="form-control" />
                                                </asp:Panel>
                                                <asp:Panel ID="pnlTelefono" CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-sm-12 form-group" runat="server">
                                                    <asp:Label Text="Teléfono" runat="server" />
                                                    <asp:TextBox runat="server" ID="txtModalTelefono" CssClass="form-control" />
                                                </asp:Panel>
                                            </div>
                                            <div class="row">
                                                <asp:HiddenField ID="hdnIdUsuario" runat="server" />
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div style="justify-content:center;">
                                                <asp:Button ID="btnGuardar" Text="Guardar" CssClass="btn btn-success" runat="server" OnClick="btnGuardar_Click1" />
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

