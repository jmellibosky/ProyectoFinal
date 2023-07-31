<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CambioEstadoSolicitud.aspx.cs" Inherits="REApp.Forms.CambioEstadoSolicitud" %>

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
</asp:Content>

<asp:Content ID="cBody" ContentPlaceHolderID="cphBody" runat="server">
    <%-- Encabezado --%>
    <div class="container">
        <h1 class="row justify-content-center">
            <label style="font-style: italic; font-family: 'REM', sans-serif; letter-spacing: 3px" class="fw-normal mb-3 pb-2">Cambiar Estado de Solicitud</label>
        </h1>
        <br />
    </div>
    <%--Grilla interesados para envio de mail en caso de Coordinacion--%>
    <asp:Panel runat="server" ID="pnlGvInteresado" CssClass="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-sm-12 form-group">
        <asp:Label Text="Al pasar de estado se enviará el email correspondiente a los siguientes interesados:" runat="server" />
        <asp:GridView
            ID="gvSoloInteresadosVinculados"
            runat="server"
            AutoGenerateColumns="false"
            CssClass="mGrid" PagerStyle-CssClass="pgr" RowStyle-Height="40px">
            <AlternatingRowStyle BackColor="white" />
            <HeaderStyle BackColor="#20789f" Font-Bold="true" Font-Size="Large" ForeColor="White" />
            <RowStyle BackColor="#e1dddd" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="true" ForeColor="white" />

            <Columns>
                <%-- El DataField debe contener el mismo nombre que la columna de la BD, que se recupera en BindGrid()--%>
                <asp:BoundField DataField="IdInteresado" HeaderText="ID INTERESADO" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="Nombre" HeaderText="NOMBRE" ItemStyle-Width="20%" />
                <asp:BoundField DataField="IdUsuario" HeaderText="ID USUARIO" ItemStyle-Width="20%" Visible="false" />
                <asp:BoundField DataField="Email" HeaderText="EMAIL" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" />

                <%-- Boton con link para ver detalles solicitud--%>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="-" ItemStyle-Width="10%" >
                    <ItemTemplate>
                        <asp:HiddenField Value='<%# Eval("IdInteresado") %>' runat="server" ID="hdnIdInteresadoVinculado" />
                        <asp:HiddenField Value='<%# Eval("Email") %>' runat="server" ID="hdnEmail" />
                        <asp:HiddenField Value='<%# Eval("Nombre") %>' runat="server" ID="hdnNombre" />
                        <asp:CheckBox Enabled="false" runat="server" ID="chkInteresadoVinculado" Checked='<%# Eval("Checked").ToString().Equals("0") ? false : true %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

    </asp:Panel>
    <%-- Ingresar Observación--%>
    <hr />
    <br />

    <asp:Panel ID="pnlNuevoEstado" runat="server" CssClass="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-sm-12 form-group">
        <div class="row">
            <h3>
                <asp:Label ID="lblEstado" runat="server" /></h3>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlNuevoMensaje" CssClass="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-sm-12 form-group" runat="server">
        <div class="row">
            <div class="col-12">
                <asp:TextBox runat="server" TextMode="MultiLine" ID="txtObservaciones" CssClass="form-control" placeHolder="Ingrese observaciones." />
            </div>
        </div>

        <br />
        <div class="row">
            <div class="col-12">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlAlertObservaciones" runat="server" Visible="false" class="alert alert-danger" role="alert">
                            <asp:Label Text="Por favor, ingrese una observación." runat="server" />
                        </asp:Panel>
                        <br />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <div class="row">
            <div class="col-12">
                <asp:Button ID="btnConfirmar" Text="Confirmar" CssClass="btn btn-success" runat="server" OnClick="btnConfirmar_Click" />
                <asp:Button ID="btnCancelar" Text="Cancelar" CssClass="btn btn-secondary" runat="server" OnClick="btnCancelar_Click" />
            </div>
        </div>
    </asp:Panel>


</asp:Content>
