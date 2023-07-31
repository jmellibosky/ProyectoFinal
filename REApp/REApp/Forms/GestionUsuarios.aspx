<%@ Page Title="" Language="C#" 
    MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" 
    CodeBehind="GestionUsuarios.aspx.cs" 
    Inherits="REApp.Forms.GestionUsuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">

    <%-- CSS --%>
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


<asp:Content ID="cBody" ContentPlaceHolderID="cphBody" runat="server">

    

    <%-- Encabezado --%>
        <div class="row">
            <div class="col-12">
                <h1 class="row justify-content-center mb-0 pb-0 pt-5">
                    <label style="font-style: italic; font-family:'REM', sans-serif; letter-spacing: 3px" class="fw-normal mb-3 pb-2">Gestión de Usuarios</label>
                </h1>

                    <div style="text-align: end;">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:Button ID="btnNuevo" Text="Nuevo" CssClass="btn btn-primary btn-dark" runat="server" OnClick="btnNuevo_Click"/>
                                <asp:Button ID="btnVolver" Text="Volver al Listado" Visible="false" CssClass="btn btn-info btn-dark" runat="server" OnClick="btnVolver_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

            </div>
        </div>

<asp:UpdatePanel ID="upForm" runat="server">
<ContentTemplate>

    <%-- DeleteAlert --%>
    <div class="row">
        <asp:Panel ID=pnlAlertDeleteUser CssClass="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12 form-group" visible="false" runat="server">
            <div class="alert alert-info" role="alert">
                <h5>
                    <asp:Label Text="Confirme la Eliminación" runat="server" />
                </h5>
                <hr />
                <div class="row">
                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12 form-group">
                        <asp:HiddenField ID="hdnDeleteUserId" runat="server"/>
                        <asp:Label ID="lblDeleteMessage" Text="" runat="server" />
                    </div>
                </div>
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12 form-group">
                    <div>
                        <asp:Button ID="btnDeleteConfirm" CssClass="btn btn-danger" Text="Confirmar" OnClick="btnDeleteConfirm_Click" runat="server" />
                        <asp:Button ID="btnDeleteCancel" CssClass="btn btn-warning" Text="Cancelar" OnClick="btnDeleteCancel_Click" runat="server" />
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>

<asp:Panel ID="pnlListado" runat="server" Visible="true">
        <%-- Contenido --%>
        <div class="row">
            <div class="col-12">
                <div class="panel-body" style="display: flex; justify-content: center; align-items:center">
                    <div class="row" style="width:100%; " >
                <asp:UpdatePanel ID="upUsuarios" Style="width: 100%;" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>

                    <asp:GridView 
                        ID="gvUsuarios" 
                        runat="server" OnRowCommand="gvUsuarios_RowCommand"
                        AutoGenerateColumns="false" 
                        CssClass="mGrid" PagerStyle-CssClass="pgr" RowStyle-Height="40px" OnRowDataBound="gvUsuarios_RowDataBound">  
                        <AlternatingRowStyle BackColor="white" />
                        <HeaderStyle BackColor="#20789f" Font-Bold="true" Font-Size="Large" ForeColor="White" />
                        <RowStyle BackColor="#e1dddd" />
                        <SelectedRowStyle BackColor="#669999" Font-Bold="true" ForeColor="white" />

                        <Columns>  
                            <%-- El DataField debe contener el mismo nombre que la columna de la BD, que se recupera en BindGrid()--%>
                            <%--<asp:BoundField DataField="IdUsuario" HeaderText="ID USUARIO"   ItemStyle-Width="5%"/>--%>
                            <asp:BoundField DataField="Nombre" HeaderText="NOMBRE"          ItemStyle-Width="15%"/>
                            <asp:BoundField DataField="Apellido" HeaderText="APELLIDO"      ItemStyle-Width="15%"/>
                            <asp:BoundField DataField="Email" HeaderText="EMAIL"            ItemStyle-Width="20%"/>
                            <asp:BoundField DataField="NombreRol" HeaderText="ROL"          ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center"/>
                            <asp:BoundField DataField="Dni" HeaderText="DNI"                ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center"/>
                            <asp:BoundField DataField="Telefono" HeaderText="TELÉFONO"      ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center"/>
                            <asp:BoundField DataField="ValidacionEANAtabla" HeaderText="ACTIVO"  ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center"/>
                            <asp:BoundField DataField="Cuit" HeaderText="CUIT"              ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center"/>

                            <%-- Boton con link para ver detalles solicitud--%>
                            <asp:TemplateField  ItemStyle-Width="25%" ItemStyle-Wrap="false" HeaderText="ACCIONES" ItemStyle-HorizontalAlign="Center">                
                                <ItemTemplate>  
                                    <asp:LinkButton ID="btnUpdate" CommandName="UpdateUser" CommandArgument='<%# Eval("IdUsuario") %>' runat="server">
                                        <i class="fa fa-pencil" aria-hidden="true" style='font-size:15px; color:#525252' ></i>
                                    </asp:LinkButton> 
                                    <asp:LinkButton ID="btnEliminarUsuario" CommandName="DeleteUser" CommandArgument='<%# Eval("IdUsuario") %>' runat="server">
                                        <i class="fa fa-trash-can" aria-hidden="true" style='font-size:15px; color:#525252' ></i> 
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnDetalle" CommandName="DisplayUser" CommandArgument='<%# Eval("IdUsuario") %>' runat="server">
                                        <i class="fa fa-eye" aria-hidden="true" style='font-size:15px; color:#525252' ></i> 
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnActivarUsuario" CommandName="ActivarUsuario" CommandArgument='<%# Eval("IdUsuario") %>' Visible='<%# Eval("ValidacionEANA").ToString().Equals("False") %>' runat="server" >
                                        <i class="fa fa-check" aria-hidden="true" style='font-size:15px; color:#525252' ></i> 
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnDesactivarUsuario" CommandName="DesactivarUsuario" CommandArgument='<%# Eval("IdUsuario") %>' Visible='<%# Eval("ValidacionEANA").ToString().Equals("True") %>' runat="server" >
                                        <i class="fa fa-x" aria-hidden="true" style='font-size:15px; color:#525252' ></i> 
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
                                                    <asp:TextBox runat="server" ID="txtModalFechaNac" CssClass="form-control" /> <%--TextMode="DateTime"--%>
                                                </asp:Panel>
                                            </div>
                                            <div class="row">
                                                <asp:Panel ID="pnlCuil" CssClass="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-sm-12 form-group" runat="server">
                                                    <asp:Label Text="CUIT" runat="server" />
                                                    <asp:TextBox runat="server" ID="txtModalCuit" CssClass="form-control" />
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
                                                <asp:HiddenField ID="hdnIdCurrentUser" runat="server" />
                                            </div>
                                        </div>
                                        <br />
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
                                            <div style="justify-content:center;">
                                                <asp:Button ID="btnGuardar" Text="Guardar" CssClass="btn btn-success" runat="server" OnClick="btnGuardar_Click1" />
                                                <%--<asp:Button ID="btnValidar" Text="Validar Usuario" Visible="false" CssClass="btn btn-info btn-dark" runat="server" OnClick="btnValidar_Click" />--%>
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

