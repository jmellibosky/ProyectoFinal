<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileUpload.aspx.cs" MasterPageFile="~/Site.Master" Inherits="REApp.Forms.FileUpload" %>

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
                font-size: 15px;
                position: absolute sticky;
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


        button {
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
        //$(document).ready(function () {

        //});
    </script>
</asp:Content>

<asp:Content ID="cBody" ContentPlaceHolderID="cphBody" runat="server">

    <br />
    <h1 class="row justify-content-center">
        <label class="fw-normal mb-3 pb-2">Gestión de Documentación</label>
    </h1>

    <%--Panel Admin--%>
    <asp:Panel ID="PanelAdmin" runat="server" Visible="false">
        <%-- Mostrar Archivos--%>
        

            <div class="container justify-content-center">

                <%--DDL y Carga de Archivos--%>
                <div class="row">
                    <div class="col align-self-start">
                        <asp:Label runat="server">Solicitantes</asp:Label>
                        <asp:DropDownList runat="server" ID="ddlSolicitante" CssClass="form-control select-single" OnSelectedIndexChanged="ddlSolicitante_SelectedIndexChanged" AutoPostBack="true" Width="300px" />
                    </div>
                    <asp:Panel ID="pnlFuAdmin" runat="server">
                    <div class="col align-self-end border">
                        <asp:FileUpload ID="FileUpload1" runat="server" />
                        <br />
                        <hr />
                        <br />
                        <div class="row">
                            <%--<label class="form-label font-weight-bold ml-2" for="txtFechaVencimiento">Fecha de Vencimiento</label>--%>
                            <asp:Label CssClass="width: 50%; text-align: right; text-md-center font-weight-bold" runat="server">Fecha de Vencimiento:&nbsp &nbsp</asp:Label>
                            <input type="date" id="txtFechaVencimientoAdmin" runat="server" />
                            <hr />
                        </div>
                        <hr />
                        <br />
                        <div class="row">
                            <hr />
                            <br />
                            <asp:Button ID="Upload" runat="server" Text="Subir Archivo" OnClick="Upload_Click" CssClass="btn btn-dark" />
                            <asp:Label ID="LbArchivo" runat="server" Text="" CssClass="alert-danger"></asp:Label>
                        </div>
                        <br />
                    </div>
                </div>
            </div>
        </asp:Panel>
        <%--DGV Admin--%>
        <div class="panel-body" style="display: flex; justify-content: center; align-items: center">
            <div class="row" style="overflow: auto; height: 375px; width: 1100px;">
                <asp:Panel ID="upDoc" Style="width: 100%;" runat="server">
                    <asp:GridView ID="gvArchivos"
                        runat="server"
                        AutoGenerateColumns="false"
                        CssClass="mGrid" PagerStyle-CssClass="pgr" RowStyle-Height="40px" OnRowDataBound="gvArchivos_RowDataBound">
                        <AlternatingRowStyle BackColor="white" />
                        <HeaderStyle BackColor="#20789f" Font-Bold="true" Font-Size="Large" ForeColor="White" />
                        <RowStyle BackColor="#e1dddd" />
                        <SelectedRowStyle BackColor="#669999" Font-Bold="true" ForeColor="white" />
                        <Columns>
                            <%-- El DataField debe contener el mismo nombre que la columna de la BD, que se recupera en BindGrid()--%>
                            <asp:BoundField DataField="IdDocumento" HeaderText="ID" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="NombreUsuario" HeaderText="PROPIETARIO" ItemStyle-Width="10%" />
                            <asp:BoundField DataField="Nombre" HeaderText="NOMBRE" ItemStyle-Width="20%" />
                            <asp:BoundField DataField="Extension" HeaderText="EXTENSION" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="IdTipoDocumento" HeaderText="TIPO DOC" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center"/>
                            <asp:BoundField DataField="FHAlta" HeaderText="FECHA ALTA" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="FHVencimiento" HeaderText="FECHA VENCIMIENTO" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />

                            <%-- Boton con link para descargar archivo--%>
                            <asp:TemplateField ItemStyle-Width="10%" ItemStyle-Wrap="false" HeaderText="ACCIONES" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDownload" runat="server" OnClick="lnkDownload_Click1"
                                        CommandArgument='<%# Eval("IdDocumento") %>'>
                                            <i class="fas fa-file-pdf" aria-hidden="true" style='font-size:15px;  color:#525252'></i> 
                                    </asp:LinkButton>
                                    <%--Boton para eliminar archivo de la BD--%>
                                    <asp:LinkButton ID="lnkEliminarArchivo" runat="server" OnClick="lnkEliminarArchivo_Click"
                                        CommandArgument='<%# Eval("IdDocumento") %>'>
                                            <i class="fa fa-trash-can" aria-hidden="true" style='font-size:15px; color:#525252' ></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>

                    </asp:GridView>
                </asp:Panel>
            </div>
        </div>
    </asp:Panel>
    <%--  --%>
    <%--Panel Solicitante--%>

    <asp:Panel ID="PanelSolicitante" runat="server" Visible="false">
        <div class="row">
            <%--FileUpload 2--%>
            <div class="col-sm-6">
                <div class="row">
                    <asp:Label runat="server" CssClass="font-weight-bold" ForeColor="black" ID="lblCertM">Certificado Médico</asp:Label>
                </div>
                <hr />
                <div class="row">
                    <asp:Panel runat="server" ID="pnlFuCM" Visible="false">
                        <asp:FileUpload ID="FileUpload2" runat="server" />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="panelGvCertMedico" Visible="true">
                        <asp:GridView ID="gvCertMedico"
                            runat="server"
                            AutoGenerateColumns="false"
                            CssClass="mGrid" PagerStyle-CssClass="pgr" RowStyle-Height="40px">
                            <AlternatingRowStyle BackColor="white" />
                            <HeaderStyle BackColor="#20789f" Font-Bold="true" Font-Size="XX-Small" ForeColor="White" />
                            <RowStyle BackColor="#e1dddd" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="true" ForeColor="white" />
                            <Columns>
                                <%-- El DataField debe contener el mismo nombre que la columna de la BD, que se recupera en BindGrid()--%>
                                <asp:BoundField DataField="IdDocumento" HeaderText="ID" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="IdTipoDocumento" HeaderText="ID DOC" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Nombre" HeaderText="DOCUMENTO" ItemStyle-Width="10%" />
                                <asp:BoundField DataField="FHAlta" HeaderText="FECHA ALTA" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="FHVencimiento" HeaderText="FECHA VENCIMIENTO" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField ItemStyle-Width="15%" ItemStyle-Wrap="false" HeaderText="ACCIONES" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%-- Boton con link para descargar archivo--%>
                                        <asp:LinkButton ID="lnkDownload" runat="server" OnClick="lnkDownload_Click1"
                                            CommandArgument='<%# Eval("IdDocumento") %>'>
                                                <i class="fas fa-file-pdf" aria-hidden="true" style='font-size: 15px; color: #525252'></i>
                                        </asp:LinkButton>
                                        <%--Boton para eliminar archivo de la BD--%>
                                        <asp:LinkButton ID="lnkEliminarArchivo" runat="server" OnClick="lnkEliminarArchivo_Click"
                                            CommandArgument='<%# Eval("IdDocumento") %>'>
                                                <i class="fa fa-trash-can" aria-hidden="true" style='font-size: 15px; color: #525252'></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </div>
                <br />
                <hr />
                <br />
                <asp:Panel runat="server" ID="pnlFechaVencimientoCM">
                    <div class="row">
                        <asp:Label CssClass="width: 50%; text-align: right; text-md-center font-weight-bold" runat="server">Fecha de Vencimiento:&nbsp &nbsp</asp:Label>
                        <input type="date" id="txtFechaVencimientoCertMedico" runat="server" />
                        <hr />
                    </div>
                </asp:Panel>
                <hr />
                <br />
                <asp:Panel runat="server" ID="pnlBtnSubirArchivoCM">
                    <div class="row">
                        <asp:LinkButton ID="lnkUpload2" runat="server" OnClick="Upload_Click"
                            CommandArgument="2">
                            <asp:Button ID="Button1" runat="server" Text="Subir Archivo" OnClick="Upload_Click" CssClass="btn btn-dark" />
                        </asp:LinkButton>
                    </div>
                </asp:Panel>
                <br />
            </div>
            <%--FileUpload 3--%>
            <div class="col-sm-6">
                <div class="row">
                    <asp:Label runat="server" CssClass="font-weight-bold" ForeColor="black" ID="Label1">Certificado de Competencia</asp:Label>
                </div>
                <hr />
                <div class="row">
                    <asp:Panel runat="server" ID="pnlFUCertCompetencia" Visible="true">
                        <asp:FileUpload ID="FileUpload3" runat="server" />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="panel2" Visible="true">
                        <asp:GridView ID="gvCertCompetencia"
                            runat="server"
                            AutoGenerateColumns="false"
                            CssClass="mGrid" PagerStyle-CssClass="pgr" RowStyle-Height="40px">
                            <AlternatingRowStyle BackColor="white" />
                            <HeaderStyle BackColor="#20789f" Font-Bold="true" Font-Size="XX-Small" ForeColor="White" />
                            <RowStyle BackColor="#e1dddd" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="true" ForeColor="white" />
                            <Columns>
                                <%-- El DataField debe contener el mismo nombre que la columna de la BD, que se recupera en BindGrid()--%>
                                <asp:BoundField DataField="IdDocumento" HeaderText="ID" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="IdTipoDocumento" HeaderText="ID DOC" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Nombre" HeaderText="DOCUMENTO" ItemStyle-Width="10%" />
                                <asp:BoundField DataField="FHAlta" HeaderText="FECHA ALTA" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="FHVencimiento" HeaderText="FECHA VENCIMIENTO" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField ItemStyle-Width="15%" ItemStyle-Wrap="false" HeaderText="ACCIONES" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%-- Boton con link para descargar archivo--%>
                                        <asp:LinkButton ID="lnkDownload" runat="server" OnClick="lnkDownload_Click1"
                                            CommandArgument='<%# Eval("IdDocumento") %>'>
                                                <i class="fas fa-file-pdf" aria-hidden="true" style='font-size: 15px; color: #525252'></i>
                                        </asp:LinkButton>
                                        <%--Boton para eliminar archivo de la BD--%>
                                        <asp:LinkButton ID="lnkEliminarArchivo" runat="server" OnClick="lnkEliminarArchivo_Click"
                                            CommandArgument='<%# Eval("IdDocumento") %>'>
                                                <i class="fa fa-trash-can" aria-hidden="true" style='font-size: 15px; color: #525252'></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>

                </div>
                <br />
                <hr />
                <br />
                <asp:Panel runat="server" ID="pnlFechaVencimientoCertCompetencia">
                    <div class="row">
                        <asp:Label CssClass="width: 50%; text-align: right; text-md-center font-weight-bold" runat="server">Fecha de Vencimiento:&nbsp &nbsp</asp:Label>
                        <input type="date" id="txtFechaVencimientoCertCompetencia" runat="server" />
                        <hr />
                    </div>
                </asp:Panel>
                <hr />
                <br />
                <asp:Panel runat="server" ID="pnlBtnSubirArchivoCertCompetencia">
                    <div class="row">
                        <asp:LinkButton ID="lnkUpload3" runat="server" OnClick="Upload_Click"
                            CommandArgument="3">
                            <asp:Button ID="Button2" runat="server" Text="Subir Archivo" OnClick="Upload_Click" CssClass="btn btn-dark" />
                        </asp:LinkButton>
                    </div>
                </asp:Panel>
                <br />
            </div>

            <%--FileUpload 4--%>
            <div class="col-sm-6">
                <div class="row">
                    <asp:Label runat="server" CssClass="font-weight-bold" ForeColor="black" ID="Label2">CEVANT</asp:Label>
                </div>
                <hr />
                <div class="row">
                    <asp:Panel runat="server" ID="pnlFUCevant" Visible="true">
                        <asp:FileUpload ID="FileUpload4" runat="server" />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="panel3" Visible="true">
                        <asp:GridView ID="gvCevant"
                            runat="server"
                            AutoGenerateColumns="false"
                            CssClass="mGrid" PagerStyle-CssClass="pgr" RowStyle-Height="40px">
                            <AlternatingRowStyle BackColor="white" />
                            <HeaderStyle BackColor="#20789f" Font-Bold="true" Font-Size="XX-Small" ForeColor="White" />
                            <RowStyle BackColor="#e1dddd" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="true" ForeColor="white" />
                            <Columns>
                                <%-- El DataField debe contener el mismo nombre que la columna de la BD, que se recupera en BindGrid()--%>
                                <asp:BoundField DataField="IdDocumento" HeaderText="ID" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="IdTipoDocumento" HeaderText="ID DOC" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Nombre" HeaderText="DOCUMENTO" ItemStyle-Width="10%" />
                                <asp:BoundField DataField="FHAlta" HeaderText="FECHA ALTA" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="FHVencimiento" HeaderText="FECHA VENCIMIENTO" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField ItemStyle-Width="15%" ItemStyle-Wrap="false" HeaderText="ACCIONES" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%-- Boton con link para descargar archivo--%>
                                        <asp:LinkButton ID="lnkDownload" runat="server" OnClick="lnkDownload_Click1"
                                            CommandArgument='<%# Eval("IdDocumento") %>'>
                                                <i class="fas fa-file-pdf" aria-hidden="true" style='font-size: 15px; color: #525252'></i>
                                        </asp:LinkButton>
                                        <%--Boton para eliminar archivo de la BD--%>
                                        <asp:LinkButton ID="lnkEliminarArchivo" runat="server" OnClick="lnkEliminarArchivo_Click"
                                            CommandArgument='<%# Eval("IdDocumento") %>'>
                                                <i class="fa fa-trash-can" aria-hidden="true" style='font-size: 15px; color: #525252'></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>

                </div>
                <br />
                <hr />
                <br />
                <asp:Panel runat="server" ID="pnlFechaVencimientoCevant">
                    <div class="row">
                        <asp:Label CssClass="width: 50%; text-align: right; text-md-center font-weight-bold" runat="server">Fecha de Vencimiento:&nbsp &nbsp</asp:Label>
                        <input type="date" id="txtFechaVencimientoCevant" runat="server" />
                        <hr />
                    </div>
                </asp:Panel>
                <hr />
                <br />
                <asp:Panel runat="server" ID="pnlBtnSubirArchivoCevant">
                    <div class="row">
                        <asp:LinkButton ID="lnkUpload4" runat="server" OnClick="Upload_Click"
                            CommandArgument="4">
                            <asp:Button ID="Button3" runat="server" Text="Subir Archivo" OnClick="Upload_Click" CssClass="btn btn-dark" />
                        </asp:LinkButton>
                    </div>
                </asp:Panel>
                <br />
            </div>

        </div>

    </asp:Panel>


</asp:Content>





