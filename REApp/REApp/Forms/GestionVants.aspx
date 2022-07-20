﻿<%@ Page
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
		background: #f5f5f5;
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
            background-color: #fff;
            margin: 5px 0 10px 0;
            border: solid 1px #525252;
            border-collapse: collapse;
        }

            .mGrid td {
                padding: 2px;
                border: solid 1px #c1c1c1;
                color: #717171;
            }

            .mGrid th {
                padding: 4px 2px;
                color: #fff;
                background: #424242 url(grd_head.png) repeat-x top;
                border-left: solid 1px #525252;
                font-size: 0.9em;
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
                    <asp:Label ID="lblTitulo" Text="Gestión de Vants" runat="server" />

                    <div style="text-align: end;">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:Button ID="btnNuevo" Text="Nuevo" CssClass="btn btn-primary" runat="server" OnClick="btnNuevoVant_Click"/>
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
                                <asp:UpdatePanel ID="upVants" Style="width: 100%;" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>
                                         <asp:GridView 
                                             ID="gvVants" 
                                             runat="server" 
                                             AutoGenerateColumns="false" 
                                             CssClass="mGrid" PagerStyle-CssClass="pgr"
                                                     Height="500px" Width="800px">  
                                                    <AlternatingRowStyle BackColor="white" />
                                                    <HeaderStyle BackColor="#20789f" Font-Bold="true" Font-Size="Large" ForeColor="White" />
                                                    <RowStyle BackColor="#e1dddd" />
                                                    <SelectedRowStyle BackColor="#669999" Font-Bold="true" ForeColor="white" />

                                                        <Columns>  
                                                            <%-- El DataField debe contener el mismo nombre que la columna de la BD, que se recupera en BindGrid()--%>
                                                            <asp:BoundField DataField="IdVant" HeaderText="Id" />
                                                            <asp:BoundField DataField="Marca" HeaderText="Marca" /> <%--Vant.IdMarcaVant=MarcaVant.IdMarcaVant--%>
                                                            <asp:BoundField DataField="Tipo" HeaderText="Tipo" /> <%--Vant.IdTipoVant=TipoVant.IdTipoVant--%>
                                                            <asp:BoundField DataField="Modelo" HeaderText="Modelo" />
                                                            <asp:BoundField DataField="FHAlta" HeaderText="Fecha Alta" />             
                                                            <asp:BoundField DataField="FHBaja" HeaderText="Fecha Baja" />


                                                            <%--Boton para editar vant de la BD --%>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ControlStyle-Width="140px" HeaderText="Acciones">  
                                                                <ItemTemplate>  
                                                                    <asp:LinkButton ID="btnModificarVant" runat="server" OnClick="btnModificarVant_Click"
                                                                    CommandArgument='<%# Eval("IdVant") %>' >
                                                                        <i class="fa fa-pencil" aria-hidden="true"></i> <br />Modificar
                                                                    </asp:LinkButton>                                                                 
                                                                </ItemTemplate>  
                                                            </asp:TemplateField> 

                                                            <%--Boton para eliminar archivo de la BD --%>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ControlStyle-Width="75px">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="btnEliminarVant" runat="server" OnClick="btnEliminarVant_Click"
                                                                    CommandArgument='<%# Eval("IdVant") %>' OnClientClick="return confirm('¿Seguro que desea eliminar este VANT?')">
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
                                            <h6>Datos Vant</h6>   
                                        </div>
                                        <asp:HiddenField ID="hdnIdVant" runat="server"/>
                                        </br>
                                        <div class="row">
                                            <asp:Panel ID="pnlModalMarcaVant" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Marca" runat="server" />
                                                <asp:DropDownList runat="server" ID="ddlMarcaVant" CssClass="form-control select-single" />
                                            </asp:Panel>

                                            <asp:Panel ID="pnlModalTipo" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Tipo" runat="server" />
                                                <asp:DropDownList runat="server" ID="ddlTipoVant" CssClass="form-control select-single" />
                                            </asp:Panel>
                                        </div>
                                        <div class="row">
                                            <asp:Panel ID="pnlModalModelo" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                                                <asp:Label Text="Modelo" runat="server" />
                                                <asp:TextBox runat="server" ID="txtModelo" CssClass="form-control" />
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
