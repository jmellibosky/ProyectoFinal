<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ForoMensajes.aspx.cs" Inherits="REApp.Forms.ForoMensajes" %>

<asp:Content ID="cHead" ContentPlaceHolderID="cphHead" runat="server">
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





</asp:Content>

<asp:Content ID="cBody" ContentPlaceHolderID="cphBody" runat="server">
            <%-- Encabezado --%>

        <div class="container">
        <h1 class="row justify-content-center mb-0 pb-0 pt-5">
            <label style="font-family:Azonix; letter-spacing: 3px" class="fw-normal mb-3 pb-2">Foro de Mensajes</label>
        </h1>
        <%--Se borra el AutoPostBack porq hay q cargar el dgv de otra forma.--%>
        <br />
            </div>
            
    
                      
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="col-12 text-right">
                    <asp:Button ID="btnVolver" Text="Volver al Listado" Visible="true" CssClass="btn btn-info btn-dark" runat="server" OnClick="btnVolver_Click" />
                </div>
           </ContentTemplate>
        </asp:UpdatePanel>
                   
    <hr />

    <%-- Tabla Mensajes--%>

     <asp:Panel ID="pnlMensajes" runat="server" Visible="true">
                <%-- Contenido --%>
                <div class="row">
                    <div class="col-12">
                        <div class="panel-body" style="display: flex; justify-content: center; align-items:center">
                            <div class="row" style="overflow: auto; width: 1400px; " >
                                <asp:UpdatePanel ID="upMensajes" Style="width: 100%;" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>
                                         <asp:GridView 
                                             ID="gvMensajes" 
                                             runat="server" 
                                             AutoGenerateColumns="false" 
                                             CssClass="mGrid" PagerStyle-CssClass="pgr" RowStyle-Height="40px" >
                                                    <AlternatingRowStyle BackColor="white" />
                                                    <HeaderStyle BackColor="#20789f" Font-Bold="true" Font-Size="Large" ForeColor="White" />
                                                    <RowStyle BackColor="#e1dddd" />
                                                    <SelectedRowStyle BackColor="#669999" Font-Bold="true" ForeColor="white" />

                                                        <Columns>  
                                                            <%-- El DataField debe contener el mismo nombre que la columna de la BD, que se recupera en BindGrid()--%>
                                                            <asp:BoundField DataField="FHMensaje" ItemStyle-Width="10%"  ItemStyle-HorizontalAlign="Center" HeaderText="FECHA Y HORA" />
                                                            <asp:BoundField DataField="Contenido" ItemStyle-Width="50%" HeaderText="MENSAJE" /> 
                                                            <asp:BoundField DataField="Nombre" ItemStyle-Width="10%"  ItemStyle-HorizontalAlign="Center" HeaderText="EMISOR" />
                                                            <asp:BoundField DataField="Rol" ItemStyle-Width="10%"  ItemStyle-HorizontalAlign="Center" HeaderText="ROL" />
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

    <hr />
    <br />

            <asp:Panel ID="pnlNuevoMensaje" CssClass="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-sm-12 form-group" runat="server">
                <div>
                    <asp:TextBox runat="server" ID="txtNuevoMensaje" CssClass="form-control" placeHolder="Ingrese un nuevo mensaje"/>
                </div>

                   <br />
                <div>
                    <asp:Button ID="btnGuardar" Text="Enviar Mensaje" CssClass="btn btn-success" runat="server" OnClick="btnGuardar_Click"  />
                </div>

                
            </asp:Panel>
           

    <%-- Panel de Error --%>

            <asp:Panel ID="pnlError" Visible="false" CssClass="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-xs-12 form-group" runat="server">
            <div class="alert alert-danger" role="alert">
                <h5>
                    <asp:Label ID="txtErrorHeader" runat="server" /></h5>
                <hr />
                <asp:Label ID="txtErrorBody" runat="server" />
            </div>
            <hr />
        </asp:Panel>


</asp:Content>
