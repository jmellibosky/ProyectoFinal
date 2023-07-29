<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ValidacionDocumentacion.aspx.cs" Inherits="REApp.Forms.ValidacionDocumentacion" %>

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
        function confirmarAccion() {
            Swal.fire({
                title: '¿Estás seguro?',
                text: '¿Deseas aceptar este documento?',
                icon: 'question',
                showCancelButton: true,
                confirmButtonText: 'Aceptar',
                cancelButtonText: 'Cancelar'
            }).then((result) => {
                if (result.isConfirmed) {
                    // Envia una solicitud AJAX al servidor
                    var xhr = new XMLHttpRequest();
                    xhr.open('POST', 'ValidacionDocumentacion.aspx', true);
                    xhr.setRequestHeader('Content-type', 'application/x-www-form-urlencoded');
                    xhr.onreadystatechange = function () {
                        if (xhr.readyState === XMLHttpRequest.DONE && xhr.status === 200) {
                            // Recibe la respuesta del servidor
                            var response = xhr.responseText;
                            if (response === 'success') {
                                Swal.fire('¡Documento aceptado!', '', 'success');
                            } else {
                                Swal.fire('Error al aceptar el documento', '', 'error');
                            }
                        }
                    };
                    xhr.send();
                }
            });
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">

    <%--Titulo--%>

    <br />
    <h1 class="row justify-content-center mb-0 pb-0 pt-5">
        <label style="font-style: italic; font-family: 'REM', sans-serif; letter-spacing: 3px" class="fw-normal mb-3 pb-2">Validación de Documentación</label>
    </h1>

    <%--Panel Admin-Operador--%>

    <asp:Panel ID="PanelAdmin" runat="server">
        <%-- Mostrar Archivos--%>


        <div class="container justify-content-center">

            <%--DDL y Carga de Archivos--%>
            <div class="row">
                <div class="col align-self-start">
                    <asp:Label runat="server">Solicitantes</asp:Label>
                    <asp:DropDownList runat="server" ID="ddlSolicitante" CssClass="form-control select-single" AutoPostBack="true" OnSelectedIndexChanged="ddlSolicitante_SelectedIndexChanged" />
                </div>
            </div>
        </div>
    </asp:Panel>

    <%--Grilla Validacion Documentacion--%>

    <div class="panel-body" style="display: flex; justify-content: center; align-items: center">
        <div class="row" style="overflow: auto; height: 375px; width: 1100px;">
            <asp:UpdatePanel runat="server" ID="upGrilla" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="upDoc" Style="width: 100%;" runat="server">
                        <asp:GridView ID="gvArchivos"
                            runat="server"
                            AutoGenerateColumns="false"
                            CssClass="mGrid" PagerStyle-CssClass="pgr" RowStyle-Height="40px" OnRowDataBound="gvArchivos_RowDataBound" DataKeyNames="IdDocumento">
                            <AlternatingRowStyle BackColor="white" />
                            <HeaderStyle BackColor="#20789f" Font-Bold="true" Font-Size="Large" ForeColor="White" />
                            <RowStyle BackColor="#e1dddd" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="true" ForeColor="white" />
                            <Columns>
                                <%-- El DataField debe contener el mismo nombre que la columna de la BD, que se recupera en BindGrid()--%>
                                <asp:BoundField DataField="IdDocumento" HeaderText="ID" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="NombreUsuario" HeaderText="PROPIETARIO" ItemStyle-Width="10%" />
                                <asp:BoundField DataField="Nombre" HeaderText="NOMBRE" ItemStyle-Width="20%" />
                                <asp:BoundField DataField="NombreTipoDoc" HeaderText="TIPO DOC" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="AsociadoATripulante" HeaderText="ESTA ASOCIADO A TRIPULANTE" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="NombreTripulante" HeaderText="TRIPULANTE" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="FHAlta" HeaderText="FECHA ALTA" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="FHVencimiento" HeaderText="FECHA VENCIMIENTO" ItemStyle-Width="15%" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="FHBaja" HeaderText="FECHA BAJA" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="VinculadoSolicitud" HeaderText="Vinculado a Solicitud" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="FHAprobacion" HeaderText="FECHA APROBACION" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="NombreUsuarioAprobadoPor" HeaderText="USUARIO QUE APROBO" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="FHRechazo" HeaderText="FECHA RECHAZO" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="NombreUsuarioRechazadoPor" HeaderText="USUARIO QUE RECHAZO" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />

                                <%-- Boton con link para descargar archivo--%>
                                <asp:TemplateField ItemStyle-Width="10%" ItemStyle-Wrap="false" HeaderText="ACCIONES" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <div class="row">
                                            <div class="col-12">
                                                <asp:LinkButton ID="lnkDownload" runat="server" CssClass="btn btn-info no-gif"
                                                    OnClick="lnkDownload_Click"
                                                    CommandArgument='<%# Eval("IdDocumento") %>'>
                                            <i class="fas fa-file-pdf" aria-hidden="true" style='font-size: 15px; color: #525252'></i>
                                                </asp:LinkButton>
                                                <%--Boton para aceptar archivo en la BD--%>
                                                <asp:LinkButton ID="lnkAceptarArchivo" runat="server" CssClass="btn" OnClick="lnkAceptarArchivo_Click"
                                                    CommandArgument='<%# Eval("IdDocumento") %>'> 
                                            <i class="fa-solid fa-square-check" aria-hidden="true" style='font-size:25px; color:lawngreen' ></i>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="lnkRechazarArchivo" runat="server" CssClass="btn" OnClick="lnkRechazarArchivo_Click"
                                                    CommandArgument='<%# Eval("IdDocumento") %>'>
                                            <i class="fa-solid fa-square-xmark" aria-hidden="true" style='font-size:25px; color:red' ></i>
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>
