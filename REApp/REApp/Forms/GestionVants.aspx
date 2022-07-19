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
		background: #f5f5f5;
		font-family: sans-serif;
		font-size: 15px;
		}
		table {
		  table-layout:fixed;
          border: 1px solid black;
		  border-collapse: separate;
		  border-spacing: 2px;
		  border-color: gray;
          
          width:50%;
		}
        thead th:nth-child(1) {
          width: 5%;
        }
        thead th:nth-child(2) {
          width: 20%;
        }
        thead th:nth-child(3) {
          width: 20%;
        }
        thead th:nth-child(4) {
          width: 20%;
        }
        thead th:nth-child(5) {
          width: 10%;
        }
        th, td {
          padding: 2px;
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
                                <asp:Button ID="btnNuevo" Text="Nuevo" CssClass="btn btn-primary" runat="server" OnClick="btnNuevoVant_Click" />
                                <asp:Button ID="btnVolver" Text="Volver al Listado" Visible="false" CssClass="btn btn-info" runat="server" OnClick="btnVolver_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </h2>
            </div>
        </div>



        <div class="container justify-content-center" style="margin-left:350px;" >
            <%--Generacion de GridView--%>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-condensed table-responsive table-hover"
             Height="500px" Width="800px">  
            <AlternatingRowStyle BackColor="white" />
            <HeaderStyle BackColor="#20789f" Font-Bold="true" Font-Size="Large" ForeColor="White" />
            <RowStyle BackColor="#e1dddd" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="true" ForeColor="white" />

                <Columns>  
                    <%-- El DataField debe contener el mismo nombre que la columna de la BD, que se recupera en BindGrid()--%>
                    <asp:BoundField DataField="IdVant" HeaderText="Id" />
                    <asp:BoundField DataField="Nombre" HeaderText="Marca" /> <%--Vant.IdMarcaVant=MarcaVant.IdMarcaVant--%>
                    <asp:BoundField DataField="Nombre" HeaderText="Tipo" /> <%--Vant.IdTipoVant=TipoVant.IdTipoVant--%>
                    <asp:BoundField DataField="Modelo" HeaderText="Modelo" />
                    <asp:BoundField DataField="FHAlta" HeaderText="Fecha Alta" />             
                    <asp:BoundField DataField="FHBaja" HeaderText="Fecha Baja" />


                    <%--Boton para editar vant de la BD ---- VER SI SE ABRE NUEVA PANTALLA O COMO SE EDITA--%>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ControlStyle-Width="75px">  
                        <ItemTemplate>  
                            <asp:LinkButton ID="btnActualizarVant" runat="server" ><%--OnClick="btnActualizarVant_Click"
                            CommandArgument='<%# Eval("IdVant") %>'--%> 
                                <i class="fa fa-pencil" aria-hidden="true"></i> <br />Actualizar
                            </asp:LinkButton>  
                        </ItemTemplate>  
                    </asp:TemplateField> 

                    <%--Boton para eliminar archivo de la BD--%>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ControlStyle-Width="75px">  
                        <ItemTemplate>  
                            <asp:LinkButton ID="btnEliminarVant" runat="server" OnClick="btnEliminarVant_Click"
                            CommandArgument='<%# Eval("IdVant") %>'>
                                <i class="fa fa-trash" aria-hidden="true"></i> <br />Eliminar
                            </asp:LinkButton>  
                        </ItemTemplate>  
                    </asp:TemplateField>  

                </Columns>  
            </asp:GridView>
         </div>

 </asp:Content>
