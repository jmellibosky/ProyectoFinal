<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileUpload.aspx.cs" MasterPageFile="~/Site.Master" Inherits="REApp.Forms.FileUpload" %>

<asp:Content ID="cHead" ContentPlaceHolderID="cphHead" runat="server">
    <%-- CSS --%>
    <style>
    </style>

    <%-- JS --%>
    <script>
        //$(document).ready(function () {

        //});
    </script>
</asp:Content>

<asp:Content ID="cBody" ContentPlaceHolderID="cphBody" runat="server">

         <br/>
         <h1 class="row justify-content-center">
             <label class="fw-normal mb-3 pb-2">Gestión de Documentación</label>
        </h1>

        
        <%-- Mostrar Archivos--%>
        <div class="container justify-content-center" style="margin-left:1px;">

            <%--DDL y Carga de Archivos--%>
            <div class="row">
                <div class="col align-self-start">
                    <asp:Label runat="server">Solicitantes</asp:Label>
                    <asp:DropDownList runat="server" ID="ddlSolicitante" CssClass="form-control select-single" OnSelectedIndexChanged="ddlSolicitante_SelectedIndexChanged" AutoPostBack="true" Width="300px"/>
                </div>
                <div class="col align-self-end">
                    <asp:FileUpload ID="FileUpload1" runat="server" />

                        <asp:Button ID="Upload" runat="server" Text="Subir Archivo" OnClick="Upload_Click" CssClass="btn btn-dark"/>
                    <div class="row">
                        <br />
                        <asp:Label ID="LbArchivo" runat="server" Text="" CssClass="alert-danger"></asp:Label>  
                    </div>
                                      
                </div>
            </div>

            <br /><br /><br/>

            <%--Generacion de GridView--%>


                    <asp:GridView ID="gvArchivos" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-condensed table-responsive table-hover"
                     Height="400px" Width="1150px">  
                    <AlternatingRowStyle BackColor="white" />
                    <HeaderStyle BackColor="#20789f" Font-Bold="true" Font-Size="Large" ForeColor="White" />
                    <RowStyle BackColor="#e1dddd" />
                    <SelectedRowStyle BackColor="#669999" Font-Bold="true" ForeColor="white" />

                        <Columns>  
                            <%-- El DataField debe contener el mismo nombre que la columna de la BD, que se recupera en BindGrid()--%>
                            <asp:BoundField DataField="IdDocumento" HeaderText="Id" ItemStyle-Width="100px" ItemStyle-Wrap="false"/>
                            <asp:BoundField DataField="NombreUsuario" HeaderText="Propietario" ItemStyle-Width="150px" ItemStyle-Wrap="false"/>
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" ItemStyle-Width="250px" ItemStyle-Wrap="false"/>
                            <asp:BoundField DataField="Extension" HeaderText="Extensión" ItemStyle-Width="150px" ItemStyle-Wrap="false"/>
                            <asp:BoundField DataField="TipoMIME" HeaderText="Tipo MIME" ItemStyle-Width="150px" ItemStyle-Wrap="false"/>
                            <asp:BoundField DataField="FHAlta" HeaderText="Fecha Alta" ItemStyle-Width="150px" ItemStyle-Wrap="false"/>

                            <%-- Boton con link para descargar archivo--%>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center"  ControlStyle-Width="80px" ItemStyle-Wrap="false">  
                                <ItemTemplate>  
                                    <asp:LinkButton ID="lnkDownload" runat="server" OnClick="lnkDownload_Click1"
                                    CommandArgument='<%# Eval("IdDocumento") %>'>
                                        <i class="fas fa-file-pdf" style='font-size:24px;color:red'></i> Descargar
                                    </asp:LinkButton>      
                                </ItemTemplate>  
                            </asp:TemplateField>

                            <%--Boton para eliminar archivo de la BD--%>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ControlStyle-Width="80px" ItemStyle-Wrap="false">  
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
    
</asp:Content>





