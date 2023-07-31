<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserSendValidationEmail.aspx.cs" Inherits="REApp.Forms.UserSendValidationEmail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="/Content/bootstrap.css" rel="stylesheet" />
    <link href="/Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/dron.png" rel="icon" type="image/x-icon" />
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
          <div class="col col-xl-6">
            <div class="card" style="border-radius: 1rem;">
                <div class="card-body p-4 p-lg-5 text-black">

                <form id="FormForgotPass" runat="server">

                    <div class="d-flex align-items-center">
                        <i class="fas fa-envelope fa-3x me-3 mr-4 mt-4" style="color: #79aaf7; position:relative; left:50%; transform:translate(-50%,-50%);" ></i>
                    </div>

                    <div class="d-flex align-items-center mb-3 pb-1">
                        <span class="h1 fw-bold mb-1 font-weight-bolder">Reenviar correo de validación</span>
                    </div>

                    <div class="form-outline mb-3">
                        <input type="email" id="txt_email" runat="server" class="form-control form-control-lg" required="required" placeholder="Correo Electronico"/>
                    </div>
                     
                    <div class="pt-1">                    
                        <asp:Button ID="login" runat="server" CssClass="btn btn-success btn-lg btn-block" OnClick="EnviarMail" Text="Reenviar Validación"/>
                    </div>

                    <div class="mt-2">
                        <a class="small text-muted" href="#!">¿Olvidó su correo electronico?</a>
                    </div>
                    
                    <div class="mt-2">
                        <a href="#!" class="small text-muted">Terms of use.</a>
                        <a href="#!" class="small text-muted">Privacy policy</a>
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
