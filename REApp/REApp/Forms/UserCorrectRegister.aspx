<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserCorrectRegister.aspx.cs" Inherits="REApp.Forms.UserCorrectRegister" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="/Content/bootstrap.css" rel="stylesheet" />
    <link href="/Content/bootstrap.min.css" rel="stylesheet" />
    <%--<link href="/Content/Site.css" rel="stylesheet" />--%>
    <script src="https://kit.fontawesome.com/8e4807e881.js" crossorigin="anonymous"></script>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>REA EANA 2022</title>
</head>

<body>
    <section class="vh-100" style="background-color: #e8f4fa;">
      <div class="container py-5 h-100">
        <div class="row d-flex justify-content-center align-items-center h-100">
          <div class="col col-xl-6">
            <div class="card" style="border-radius: 1rem;">
                <div class="card-body p-4 p-lg-5 text-black">

                <form id="FormCorrectRegister" runat="server">

                    <div class="d-flex align-items-center">
                        <i class="fas fa-thumbs-up fa-3x me-3 mr-4 mt-4" style="color: #79aaf7; position:relative; left:50%; transform:translate(-50%,-50%);" ></i>
                    </div>

                    <div class="d-flex align-items-center mb-3 pb-1">
                        <span class="h1 fw-bold mb-1 font-weight-bolder text-center" >¡Registro realizado exitosamente!</span>   
                    </div>

                    <div class="d-flex mb-3 pb-1">
                        <label class="form-label mt-1 text-secondary">Se envió un mail de confirmación a su correo electrónico para verificar la cuenta asociada.</label>
                    </div>
                     
                    <div class="pt-1">                    
                        <asp:Button ID="login" runat="server" CssClass="btn btn-success btn-lg btn-block" OnClick="btnVolverInicio_Click" Text="Volver al inicio"/>
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
