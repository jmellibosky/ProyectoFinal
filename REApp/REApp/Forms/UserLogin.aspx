<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserLogin.aspx.cs" Inherits="REApp.Forms.UserLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="/Content/bootstrap.css" rel="stylesheet" />
    <link href="/Content/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/Site.css" rel="stylesheet" />
    <script src="https://kit.fontawesome.com/8e4807e881.js" crossorigin="anonymous"></script>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>REA EANA 2022</title>
</head>

<body>
    <section class="vh-100" style="background-color: #e8f4fa;">
      <div class="container py-5 h-100">
        <div class="row d-flex justify-content-center align-items-center h-100">
          <div class="col col-xl-10">
            <div class="card" style="border-radius: 1rem;">
              <div class="row g-0">
                <div class="col-md-6 col-lg-5 d-none d-md-block">
                  <img src="/Assets/LoginBanner.jpg"
                    alt="login form" class="img-fluid" style="border-radius: 1rem 0 0 1rem;" />
                </div>
                <div class="col-md-6 col-lg-7 d-flex align-items-center">
                  <div class="card-body p-4 p-lg-5 text-black">

                    <form>

                      <div class="d-flex align-items-center mb-1 pb-1">
                        <i class="fas fa-plane fa-2x me-3 mr-4 mt-4" style="color: #79aaf7;"></i>
                        <span class="h1 fw-bold mb-1 font-weight-bolder">EANA REA</span>
                      </div>

                      <h5 class="fw-normal mb-3 pb-2" style="letter-spacing: 1px;">Bienvenido al Sistema REA</h5>

                      <div class="form-outline mb-4">
                        <input type="email" id="form2Example17" class="form-control form-control-lg" />
                        <label class="form-label mt-2" for="form2Example17">Dirección de Email</label>
                      </div>

                      <div class="form-outline mb-4">
                        <input type="password" id="form2Example27" class="form-control form-control-lg" />
                        <label class="form-label mt-2" for="form2Example27">Contraseña</label>
                      </div>

                      <div class="pt-1 mb-4">
                        <button class="btn btn-dark btn-lg btn-block" type="button">Ingresar</button>
                      </div>

                      <a class="small text-muted" href="#!">Olvido su contraseña?</a>
                      <p class="mb-5 pb-lg-2" style="color: #393f81;">No tiene una cuenta? <a href="#!"
                          style="color: #393f81;">Registrese aquí</a></p>
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
