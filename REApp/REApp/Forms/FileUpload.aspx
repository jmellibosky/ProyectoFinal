<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileUpload.aspx.cs" Inherits="REApp.Forms.FileUpload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link href="/Content/bootstrap.css" rel="stylesheet" />
<link href="/Content/bootstrap.min.css" rel="stylesheet" />
<link href="/Content/Site.css" rel="stylesheet" />
<script src="https://kit.fontawesome.com/8e4807e881.js" crossorigin="anonymous"></script>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

         <h1 class="row justify-content-center">
             <label class="fw-normal mb-3 pb-2">Subida de Archivos</label>
        </h1>


        <%-- Mostrar Archivos--%>
        <div class="container justify-content-center" style="margin-left:150px;" >
            <%--Generacion de GridView--%>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-condensed table-responsive table-hover"
             Height="500px" Width="900px">  
            <AlternatingRowStyle BackColor="white" />
            <HeaderStyle BackColor="#20789f" Font-Bold="true" Font-Size="Large" ForeColor="White" />
            <RowStyle BackColor="#e1dddd" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="true" ForeColor="white" />

                <Columns>  
                    <%-- El DataField debe contener el mismo nombre que la columna de la BD, que se recupera en BindGrid()--%>
                    <asp:BoundField DataField="IdDocumento" HeaderText="Id Documento" />
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                    <asp:BoundField DataField="Extension" HeaderText="Extension" />
                    <asp:BoundField DataField="TipoMIME" HeaderText="Tipo MIME" />
                    <asp:BoundField DataField="FHAlta" HeaderText="Fecha Alta" />

                    <%-- Boton con link para descargar archivo--%>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center"  ControlStyle-Width="120px">  
                        <ItemTemplate>  
                            <asp:LinkButton ID="lnkDownload" runat="server" OnClick="lnkDownload_Click1"
                            CommandArgument='<%# Eval("IdDocumento") %>'>
                                <i class="fas fa-file-pdf" style='font-size:24px;color:red'></i> Descargar
                            </asp:LinkButton>      
                        </ItemTemplate>  
                    </asp:TemplateField>

                    <%--Boton para eliminar archivo de la BD--%>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ControlStyle-Width="75px">  
                        <ItemTemplate>  
                            <asp:LinkButton ID="lnkEliminarArchivo" runat="server" OnClick="lnkEliminarArchivo_Click"
                            CommandArgument='<%# Eval("IdDocumento") %>'>
                                <i class="fa fa-trash" aria-hidden="true" style='font-size:24px;'></i> Eliminar
                            </asp:LinkButton>  
                        </ItemTemplate>  
                    </asp:TemplateField>  

                </Columns>  
            </asp:GridView>
            </div>

         <%--Subida de Archivo--%>
        <div class="row justify-content-center">
            <br/><br/>
            <asp:FileUpload ID="FileUpload1" runat="server" />
            <br/>
            <asp:Button ID="Upload" runat="server" Text="Subir Archivo" OnClick="Upload_Click" CssClass="btn btn-dark"/>
            <br/><br/>
            <asp:Label ID="LbArchivo" runat="server" Text=""></asp:Label>
        </div>

    </form>
</body>
</html>
