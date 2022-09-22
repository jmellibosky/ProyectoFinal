<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ForoMensajes.aspx.cs" Inherits="REApp.Forms.ForoMensajes" %>

<asp:Content ID="cHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>

<asp:Content ID="cBody" ContentPlaceHolderID="cphBody" runat="server">
            <%-- Encabezado --%>
        <div class="row">
            <div class="col-12">
                <h2 class="page-header">
                    <asp:Label style="text-align:center" class="h1" ID="lblTitulo" Text="Foro de Mensajes" runat="server" />

                    
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:Button ID="btnVolver" Text="Volver al Listado" Visible="false" CssClass="btn btn-info btn-dark" runat="server" OnClick="btnVolver_Click" />
                             </ContentTemplate>
                        </asp:UpdatePanel>
                   
                </h2>
            </div>
        </div>

    <%-- Tabla Mensajes--%>

     <asp:Panel ID="pnlMensajes" runat="server">
                <%-- Contenido --%>
                <div class="row">
                    <div class="col-12">
                        <div class="panel-body" style="display: flex; justify-content: center; align-items:center">
                            <div class="row" style="overflow: auto;height: 500px; width: 1400px; " >
                                <asp:UpdatePanel ID="upMensajes" Style="width: 100%;" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>
                                         <asp:GridView 
                                             ID="gvMensajes" 
                                             runat="server" 
                                             AutoGenerateColumns="false" 
                                             CssClass="mGrid" PagerStyle-CssClass="pgr" RowStyle-Height="40px" OnRowDataBound="gvMensajes_RowDataBound">
                                                    <AlternatingRowStyle BackColor="white" />
                                                    <HeaderStyle BackColor="#20789f" Font-Bold="true" Font-Size="Large" ForeColor="White" />
                                                    <RowStyle BackColor="#e1dddd" />
                                                    <SelectedRowStyle BackColor="#669999" Font-Bold="true" ForeColor="white" />

                                                        <Columns>  
                                                            <%-- El DataField debe contener el mismo nombre que la columna de la BD, que se recupera en BindGrid()--%>
                                                            <asp:BoundField DataField="FHMensaje" ItemStyle-Width="10%"  ItemStyle-HorizontalAlign="Center" HeaderText="FECHA Y HORA" />
                                                            <asp:BoundField DataField="Contenido" ItemStyle-Width="20%" HeaderText="MENSAJE" /> 
                                                            <asp:BoundField DataField="IdUsuarioEmisor" ItemStyle-Width="20%"  HeaderText="EMISOR" />
                                                            <asp:BoundField DataField="Rol" ItemStyle-Width="20%"  HeaderText="ROL" />
                                                        </Columns>  
                                           </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div> 
            </asp:Panel>



    <%-- Nuevo Mensaje--%>


            <asp:Panel ID="pnlNuevoMensaje" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                <asp:TextBox Text="Ingrese un nuevo Mensaje" runat="server" ID="txtNuevoMensaje" CssClass="form-control" />
            </asp:Panel>
            
    <br />

            <div class="row">
                <div style="justify-content:center;">
                    <asp:Button ID="btnGuardar" Text="Guardar" CssClass="btn btn-success" runat="server" OnClick="btnGuardar_Click" />
                </div>
            </div>


</asp:Content>
