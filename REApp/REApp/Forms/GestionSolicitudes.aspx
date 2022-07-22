<%@ Page Title="" Language="C#" 
    MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" 
    CodeBehind="GestionSolicitudes.aspx.cs" 
    Inherits="REApp.Forms.GestionSolicitudes" %>

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
            <ContentTemplate>
                <asp:Button ID="btnNuevo" Text="Nueva Solicitud" CssClass="btn btn-primary" runat="server" OnClick="btnNuevo_Click" />
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
                <asp:Label runat="server">Solicitantes</asp:Label>
                <asp:DropDownList runat="server" ID="ddlSolicitante" CssClass="form-control select-single" Width="300px"/>
                <asp:Button ID="btnFiltrar" Text="Filtrar" CssClass="btn btn-info" runat="server" OnClick="btnFiltrar_Click" />

                <asp:UpdatePanel ID="upSolicitudes" Style="width: 100%;" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>

                    <asp:GridView 
                        ID="gvSolicitud" 
                        runat="server" 
                        AutoGenerateColumns="false" 
                        CssClass="table table-bordered table-condensed table-responsive table-hover" 
                        OnRowCommand="gvSolicitud_RowCommand"
                        Height="400px" Width="1000px">  
                        <AlternatingRowStyle BackColor="white" />
                        <HeaderStyle BackColor="#20789f" Font-Bold="true" Font-Size="Large" ForeColor="White" />
                        <RowStyle BackColor="#e1dddd" />
                        <SelectedRowStyle BackColor="#669999" Font-Bold="true" ForeColor="white" />

                        <Columns>  
                            <%-- El DataField debe contener el mismo nombre que la columna de la BD, que se recupera en BindGrid()--%>
                            <asp:BoundField DataField="IdSolicitud" HeaderText="IdSolicitud" ItemStyle-Width="100px" ItemStyle-Wrap="false"/>
                            <asp:BoundField DataField="NombreUsuario" HeaderText="Solicitante" ItemStyle-Width="100px" ItemStyle-Wrap="false"/>
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" ItemStyle-Width="300px" ItemStyle-Wrap="false"/>
                            <asp:BoundField DataField="FHSolicitud" HeaderText="Fecha Alta" ItemStyle-Width="100px" ItemStyle-Wrap="false"/>
                            <asp:BoundField DataField="NombreEstado" HeaderText="Estado" ItemStyle-Width="100px" ItemStyle-Wrap="false"/>

                            <%-- Boton con link para ver detalles solicitud--%>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center"  ControlStyle-Width="80px" ItemStyle-Wrap="false">  
                                <ItemTemplate>  
                                    <asp:LinkButton ID="lnkVerDetalles" runat="server" OnClick="lnkVerDetalles_Click" CommandName="Detalle"
                                    CommandArgument='<%# Eval("IdSolicitud") %>'>
                                        <i class="fa fa-info-circle"/></i> Ver Detalles
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
                                                <asp:Panel ID="pnlModalNombreUsuario" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                                                    <asp:Label Text="Nombre" runat="server" />
                                                    <asp:TextBox runat="server" ID="txtModalNombreUsuario" CssClass="form-control" />
                                                </asp:Panel>
                                                <asp:Panel ID="pnlModalApellidoUsuario" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                                                    <asp:Label Text="Apellido" runat="server" />
                                                    <asp:TextBox runat="server" ID="txtModalApellidoUsuario" CssClass="form-control" />
                                                </asp:Panel>
                                                <asp:Panel ID="pnlModalSolicitante" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                                                    <%--Lo deje invisible al ddlModalSolicitante, ver--%>
                                                    <%--Lo deje invisible al ddlModalSolicitante, ver--%>
                                                    <asp:DropDownList runat="server" ID="ddlModalSolicitante" CssClass="form-control select-single"/>
                                                </asp:Panel>
                                            </div>
                                            <div class="row">
                                                <h6>Datos de Solicitud</h6>
                                            </div>
                                            <div class="row">
                                                <asp:Panel ID="pnlModalActividadModalidad" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                                                    <asp:Label Text="Actividad" runat="server" />
                                                    <asp:DropDownList runat="server" ID="ddlModalActividad" CssClass="form-control select-single" AutoPostBack="true" OnSelectedIndexChanged="ddlModalActividad_SelectedIndexChanged" />
                                                    <asp:Label Text="Modalidad" runat="server" />
                                                    <asp:DropDownList runat="server" ID="ddlModalModalidad" CssClass="form-control select-single"/>
                                                </asp:Panel>
                                            </div>
                                            <div class="row">
                                                <asp:HiddenField ID="hdnIdSolicitud" runat="server" />
                                                <asp:Panel ID="pnlModalNombreSolicitud" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                                                    <asp:Label Text="Nombre Solicitud" runat="server" />
                                                    <asp:TextBox runat="server" ID="txtModalNombreSolicitud" CssClass="form-control font-weight-bold" />
                                                </asp:Panel>
                                                <asp:Panel ID="pnlModalObservaciones" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                                                    <asp:Label Text="Observaciones" runat="server" />
                                                    <asp:TextBox runat="server" ID="txtModalObservaciones" CssClass="form-control" />
                                                </asp:Panel>
                                                <asp:Panel ID="pnlModalEstadoSolicitud" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                                                    <asp:Label Text="Estado de Solicitud" runat="server" />
                                                    <asp:TextBox runat="server" ID="txtModalEstadoSolicitud" CssClass="form-control" />
                                                </asp:Panel>
                                                <asp:Panel ID="pnlModalFechaSolicitud" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                                                    <asp:Label Text="Fecha de Solicitud" runat="server" />
                                                    <asp:TextBox runat="server" ID="txtModalFechaSolicitud" CssClass="form-control" />
                                                </asp:Panel>
                                                <asp:Panel ID="pnlModalFechaUltimaActualizacion" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                                                    <asp:Label Text="Fecha de última actualización." runat="server" />
                                                    <asp:TextBox runat="server" ID="txtModalFechaUltimaActualizacion" CssClass="form-control" />
                                                </asp:Panel>
                                            </div>
                                        </div>
                                        <br />
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
