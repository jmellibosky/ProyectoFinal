<%@ Page
    Title="Gestión de Vants"
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="GestionVants.aspx.cs"
    MasterPageFile="~/Site.Master"
    Inherits="REApp.Forms.GestionVants" %>


<%--Head--%>
<asp:Content ID="cHead" ContentPlaceHolderID="cphHead" runat="server">
   
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



<%-- Body --%>
<asp:Content ID="cBody" ContentPlaceHolderID="cphBody" runat="server">
        <%-- Encabezado --%>
        <div class="row">
            <div class="col-12">
        <h1 class="row justify-content-center">
            <label class="fw-normal mb-3 pb-2">Gestión de Vants</label>
        </h1>

                    <div style="text-align: end;">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:Button ID="btnNuevo" Text="Nuevo" CssClass="btn btn-primary btn-dark" runat="server" OnClick="btnNuevoVant_Click"/>
                                <asp:Button ID="btnVolver" Text="Volver al Listado" Visible="false" CssClass="btn btn-info btn-dark" runat="server" OnClick="btnVolver_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

            </div>
        </div>

    <asp:UpdatePanel ID="upForm" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlListado" runat="server">
                <%-- Contenido --%>
                <div class="row">
                    <div class="col-12">
                        <br />
                        <div class="col align-self-start">
                                <h6>SOLICITANTES<h6>
                                <asp:DropDownList runat="server" ID="ddlSolicitante" CssClass="form-control select-single" OnSelectedIndexChanged="ddlSolicitante_SelectedIndexChanged" AutoPostBack="true" Width="300px"/>
                            <div class="col-12 text-center" id="divFiltrar" runat="server">
                                <asp:Button ID="btnFiltrar" Text="Filtrar" CssClass="btn btn-info btn-dark" runat="server" OnClick="btnFiltrar_Click" />
                            </div>
                        </div>
                        <br />
                        <div class="panel-body" style="display: flex; justify-content: center; align-items:center">
                            <div class="row" style="overflow: auto;height: 400px; width: 1400px; " >
                                <asp:UpdatePanel ID="upVants" Style="width: 100%;" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>
                                         <asp:GridView 
                                             ID="gvVants" 
                                             runat="server" 
                                             AutoGenerateColumns="false" 
                                             CssClass="mGrid" PagerStyle-CssClass="pgr" RowStyle-Height="40px" OnRowDataBound="gvVants_RowDataBound">
                                                    <AlternatingRowStyle BackColor="white" />
                                                    <HeaderStyle BackColor="#20789f" Font-Bold="true" Font-Size="Large" ForeColor="White" />
                                                    <RowStyle BackColor="#e1dddd" />
                                                    <SelectedRowStyle BackColor="#669999" Font-Bold="true" ForeColor="white" />

                                                        <Columns>  
                                                            <%-- El DataField debe contener el mismo nombre que la columna de la BD, que se recupera en BindGrid()--%>
                                                            <asp:BoundField DataField="IdVant" ItemStyle-Width="10%"  ItemStyle-HorizontalAlign="Center" HeaderText="ID" />
                                                            <asp:BoundField DataField="Fabricante" ItemStyle-Width="20%" HeaderText="FABRICANTE" /> 
                                                            <asp:BoundField DataField="Marca" ItemStyle-Width="20%"  HeaderText="MARCA" />
                                                            <asp:BoundField DataField="Modelo" ItemStyle-Width="20%"  HeaderText="MODELO" />
                                                            <asp:BoundField DataField="Clase" ItemStyle-Width="20%" HeaderText="CLASE" />             
                                                            <asp:BoundField DataField="NumeroSerie" ItemStyle-Width="20%" HeaderText="NÚMERO DE SERIE" />


                                                            <%--Boton para editar vant de la BD --%>
                                                            <asp:TemplateField  ItemStyle-Width="20%" ItemStyle-Wrap="false" HeaderText="ACCIONES">  
                                                                
                                                                <ItemTemplate>  
                                                                    <asp:LinkButton ID="btnModificarVant" CssClass="btn btn-warning" runat="server" OnClick="btnModificarVant_Click" ToolTip='<%# (Eval("VinculadoSolicitud").ToString() == "1") ? "Este VANT se encuentra vinculado a una solicitud vigente." : "" %>' Enabled='<%# (Eval("VinculadoSolicitud").ToString() == "1") ? false : true %>'
                                                                    CommandArgument='<%# Eval("IdVant") %>' >
                                                                        <i class="fa fa-pencil" aria-hidden="true" style='font-size:15px; color:#525252' ></i>
                                                                    </asp:LinkButton> 
                                                                    <asp:LinkButton ID="btnEliminarVant" runat="server" CssClass="btn btn-danger" OnClick="btnEliminarVant_Click" ToolTip='<%# (Eval("VinculadoSolicitud").ToString() == "1") ? "Este VANT se encuentra vinculado a una solicitud vigente." : "" %>' Enabled='<%# (Eval("VinculadoSolicitud").ToString() == "1") ? false : true %>'
                                                                    CommandArgument='<%# Eval("IdVant") %>'>
                                                                        <i class="fa fa-trash-can" aria-hidden="true" style='font-size:15px; color:#525252' ></i> 
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton ID="btnDetalleVant" runat="server" CssClass="btn btn-secondary" OnClick="btnDetalleVant_Click"
                                                                    CommandArgument='<%# Eval("IdVant") %>'>
                                                                        <i class="fa fa-eye" aria-hidden="true" style='font-size:15px; color:#525252' ></i> 
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField> 

                                                       <%--Boton para eliminar archivo de la BD --%>
<%--                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ControlStyle-Width="80px">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="btnEliminarVant" runat="server" OnClick="btnEliminarVant_Click"
                                                                    CommandArgument='<%# Eval("IdVant") %>' OnClientClick="return confirm('¿Seguro que desea eliminar este VANT?')">
                                                                        <i class="fa fa-trash" aria-hidden="true" style='font-size:24px;'></i> Eliminar
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField> --%>

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
                                                <h6>SOLICITANTE</h6>
                                                <asp:DropDownList runat="server" ID="ddlModalSolicitante" CssClass="form-control select-single" />
                                            </asp:Panel>
                                        </div>
                                        <br />
                                        <div class="row"> 
                                            <h6>DATOS VANT</h6>   
                                        </div>
                                        <asp:HiddenField ID="hdnIdVant" runat="server"/>
                                        </br>
                                        <div class="row">
                                            <asp:Panel ID="pnlModalFabricante" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Fabricante" runat="server" />
                                                <asp:TextBox runat="server" ID="txtFabricante" CssClass="form-control" />
                                            </asp:Panel>

                                            <asp:Panel ID="pnlModalMarcaVant" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Marca" runat="server" />
                                                <asp:DropDownList runat="server" ID="ddlMarcaVant" CssClass="form-control select-single" />
                                            </asp:Panel>

                                        </div>
                                        <div class="row">
                                            <asp:Panel ID="pnlModalModelo" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Modelo" runat="server" />
                                                <asp:TextBox runat="server" ID="txtModelo" CssClass="form-control" />
                                            </asp:Panel>

                                            <asp:Panel ID="pnlModalNumeroSerie" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Numero de Serie" runat="server" />
                                                <asp:TextBox runat="server" ID="txtNumeroSerie" CssClass="form-control" />
                                            </asp:Panel>

                                       
                                            <asp:Panel ID="pnlModalClase" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Clase" runat="server" />
                                                <asp:DropDownList runat="server" ID="ddlClaseVant" CssClass="form-control select-single" />
                                            </asp:Panel>
                                           
                                        </div>
                                        </br>
                                        <div class="row">
                                            <asp:Panel ID="pnlModalAñoFabricacion" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Año de Fabricacion" runat="server" />
                                                <asp:TextBox runat="server" ID="txtAñoFabricacion" CssClass="form-control" TextMode="Date" />
                                            </asp:Panel>

                                            <asp:Panel ID="pnlModalLugarFabricacion" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Lugar de Fabricacion" runat="server" />
                                                <asp:TextBox runat="server" ID="txtLugarFabricacion" CssClass="form-control" />
                                            </asp:Panel>

                                        </div>
                                        </br>

                                       
                                        <div class="row">
                                            <asp:Panel ID="pnlModalProvincia" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Provincia" runat="server" />
                                                <asp:DropDownList runat="server" ID="ddlProvincia" CssClass="form-control select-single" AutoPostBack="true" OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged"/>
                                            </asp:Panel>

                                            <asp:Panel ID="pnlModalLocalidadPartido" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Localidad/Partido" runat="server" />
                                                <asp:DropDownList runat="server" ID="ddlLocalidadPartido" CssClass="form-control select-single" AutoPostBack="true" />
                                            </asp:Panel>

                                            <asp:Panel ID="pnlModalLugarGuardado" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Lugar de Guardado" runat="server" />
                                                <asp:TextBox runat="server" ID="txtLugarGuardado" CssClass="form-control" />
                                            </asp:Panel>



                                        </div>
                                        <hr />


                                        <div class="row">
                                            <asp:Panel ID="pnlMotivoBaja" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Motivo de Baja" runat="server" />
                                                <asp:TextBox runat="server" ID="txtMotivoBaja" CssClass="form-control" mode="multiline"/>
                                            </asp:Panel>
                                        </div>

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
