<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserLogin.aspx.cs" Inherits="REApp.Forms.UserLogin" %>
<%@ Register Assembly="Recaptcha.Web" Namespace="Recaptcha.Web.UI.Controls" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="/Content/bootstrap.css" rel="stylesheet" />
    <link href="/Content/bootstrap.min.css" rel="stylesheet" />
    <%--<link href="/Content/Site.css" rel="stylesheet" />--%>
    <script src="https://kit.fontawesome.com/8e4807e881.js" crossorigin="anonymous"></script>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>REA EANA 2022</title>
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
    <script src="//cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="sweetalert2.all.min.js"></script>
    <script src="sweetalert2.min.js"></script>

    <%-- Sweet Alert--%>
    <link href="Content/sweetalert.css" rel="stylesheet" />
    <link rel="stylesheet" href="sweetalert2.min.css" />
</head>

<body>
    <section class="vh-100" style="background-color: #e8f4fa;">
      <div class="container py-5 h-100">
        <div class="row d-flex justify-content-center align-items-center h-100">
          <div class="col col-xl-10">
            <div class="card" style="border-radius: 1rem;">
              <div class="row g-0">
                <div class="col-md-6 col-lg-5 d-none d-md-block">
                  <img src="/Assets/LoginBannerReapp.png"
                    alt="login form" class="img-fluid" style="border-radius: 1rem 0 0 1rem; width:100%; height:100%;" />
                </div>
                <div class="col-md-6 col-lg-7 d-flex align-items-center">
                  <div class="card-body p-4 p-lg-5 text-black">

                    <form id="FormLogin" runat="server">

                      <div class="d-flex align-items-center mb-1 pb-1">
                        <i class="fas fa-plane fa-2x me-3 mr-4" style="color: #79aaf7;"></i>
                        <span class="h1 fw-bold mb-1 font-weight-bolder">EANA REA</span>
                      </div>

                      <h5 class="fw-normal mb-3 pb-2" style="letter-spacing: 1px;">Bienvenido al Sistema REA</h5>

                      <div class="form-outline mb-3">
                        <input type="email" id="txt_email" runat="server" class="form-control form-control-lg" required="required" placeholder="Correo electronico"/>
                        <%--<label class="form-label mt-2" for="txt_email">Dirección de Email</label>--%>
                       
                      </div>

                      <div class="form-outline mb-3">
                        <input type="password" id="txt_password" runat="server" required="required" placeholder="Contraseña" class="form-control form-control-lg" />
                        <%--<label class="form-label mt-2" for="txt_password">Contraseña</label>--%>
                        
                      </div>
                       
                      <div class="pt-1 mb-4">
                        <%--<button class="btn btn-dark btn-lg btn-block" runat="server" onclick="Validate_Email" type="button">Ingresar</button>--%>
                          <asp:Button ID="login" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-primary btn-lg btn-block" Text="Ingresar"/>
                      </div>

                      <cc1:RecaptchaWidget ID="Recaptcha" runat="server" />
                      
                      <a class="small text-muted" href="/Forms/UserForgotPassword.aspx">¿Olvidó su contraseña?</a>
                      <a class="small text-muted" href="/Forms/UserSendValidationEmail.aspx">Reenviar correo de validación</a>
                      <p class="mb-1 pb-lg-1" style="color: #393f81;">¿No tiene una cuenta? <a href="/Forms/UserRegister.aspx"
                          style="color: #393f81; font-weight:500">Registrese aquí</a></p>
                      <a href="#!" class="small text-muted">Terms of use.</a>
                      <a href="#!" class="small text-muted">Privacy policy</a>
                      
                    </form>

                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
</body>
</html>
