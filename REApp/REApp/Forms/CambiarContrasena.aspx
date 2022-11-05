<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CambiarContrasena.aspx.cs" Inherits="REApp.Forms.CambiarContrasena" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="/Content/bootstrap.css" rel="stylesheet" />
    <link href="/Content/bootstrap.min.css" rel="stylesheet" />
    <%--<link href="/Content/Site.css" rel="stylesheet" />--%>
    <script src="https://kit.fontawesome.com/8e4807e881.js" crossorigin="anonymous"></script>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Cambiar Contraseña</title>
</head>

<body>
    <section class="vh-100" style="background-color: #e8f4fa;">
      <div class="container py-5 h-100">
        <div class="row d-flex justify-content-center align-items-center h-100">
          <div class="col col-xl-6">
            <div class="card" style="border-radius: 1rem;">
                <div class="card-body p-4 p-lg-5 text-black">

                <form id="FormCambiarPass" runat="server">

                    <div class="d-flex align-items-center">
                        <i class="fas fa-lock fa-3x me-3 mr-4 mt-4" style="color: #79aaf7; position:relative; left:50%; transform:translate(-50%,-50%);" ></i>
                    </div>

                    <div class="d-flex align-items-center mb-3 pb-1">
                        <span class="h1 fw-bold mb-1 font-weight-bolder" >Restaurar Contraseña</span>
                    </div>

                    <div class="form-outline mb-3">
                        <input type="password" id="txt_pass" runat="server" class="form-control form-control-lg" required="required" placeholder="Ingrese su nueva contraseña"/>
                        <%--<label class="form-label mt-2" for="txt_email">Dirección de Email</label>--%>
                    </div>

                    <div class="form-outline mb-3">
                        <input type="password" id="txt_passcheck" runat="server" class="form-control form-control-lg" required="required" placeholder="Repita la contraseña"/>
                        <%--<label class="form-label mt-2" for="txt_email">Dirección de Email</label>--%>
                    </div>
                     
                    <div class="pt-1">                    
                        <asp:Button ID="login" runat="server" CssClass="btn btn-primary btn-lg btn-block" OnClick="Pass_Click" Text="Cambiar Contraseña"/>
                    </div>        
                </form>
                </div>
                </div>
              </div>
            </div>
          </div>
    </section>
</body>
</html>

