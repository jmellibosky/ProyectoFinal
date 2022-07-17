<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestionVants.aspx.cs" Inherits="REApp.Forms.GestionarVants" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
	<title>Gestión VANTS</title>
    <link href="/Content/bootstrap.css" rel="stylesheet" />
    <link href="/Content/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/Site.css" rel="stylesheet" />
    <script src="https://kit.fontawesome.com/ae7187225e.js" crossorigin="anonymous"></script>
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
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <%-- Título --%>
            <div class="container">
                <h1 class="row justify-content-center">
                    Gestión VANTS
                </h1>
            </div>

<%--            Tabla VANTS 
            <div class="container" margin-left="50px">
            <button class="button" type="button" title="Agregar"> <i class="fa-solid fa-square-plus"></i> &ensp; Agregar</button>
            </div>
            
            <div class="container" style="margin-top:10px;" >

                <table class="table table-striped table-hover">
                  <thead>
                        <tr>
                            <th class="text-center" width="50px">ID</th>
                            <th class="text-center">Marca</th>
                            <th class="text-center">Tipo</th>
                            <th class="text-center">Modelo</th>
                            <th class="text-center text-nowrap">Acciones</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td class="text-center">Id</td>
                            <td class="text-center">IdMarcaVant</td>
                            <td class="text-center">IdTipoVant </td>
                            <td class="text-center">Modelo</td>
                            <td>
                                <button class="button" type="button"  title="Editar">
                                    <i class="fa-solid fa-pen"></i>
                                </button>
							    <button class="button" type="button"  title="Eliminar">
                                    <i class="fa-solid fa-trash"></i>
                     		    </button>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>--%>

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
                            <asp:LinkButton ID="lnkActualizarVant" runat="server" ><%--OnClick="lnkActualizarVant_Click"
                            CommandArgument='<%# Eval("IdVant") %>'--%> 
                                <i class="fa fa-pencil" aria-hidden="true"></i> <br />Actualizar
                            </asp:LinkButton>  
                        </ItemTemplate>  
                    </asp:TemplateField> 

                    <%--Boton para eliminar archivo de la BD--%>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ControlStyle-Width="75px">  
                        <ItemTemplate>  
                            <asp:LinkButton ID="lnkEliminarVant" runat="server" OnClick="lnkEliminarVant_Click"
                            CommandArgument='<%# Eval("IdVant") %>'>
                                <i class="fa fa-trash" aria-hidden="true"></i> <br />Eliminar
                            </asp:LinkButton>  
                        </ItemTemplate>  
                    </asp:TemplateField>  

                </Columns>  
            </asp:GridView>
            </div>



        </div>
    </form>
</body>
</html>
