<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserRegister.aspx.cs" Inherits="REApp.Forms.UserRegister" %>
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
          <div class="col col-xl-6">
            <div class="card" style="border-radius: 1rem;">
                <div class="card-body p-4 p-lg-5 text-black">

                <form id="FormRegister" runat="server">

                    <%--<div class="d-flex align-items-center">
                        <i class="fas fa-lock fa-3x me-3 mr-4 mt-4" style="color: #79aaf7; position:relative; left:50%; transform:translate(-50%,-50%);" ></i>
                    </div>--%>

                    <div class="d-flex align-items-center mb-3 pb-1">
                        <i class="fas fa-id-card fa-2x me-3 mr-3 mt-2" style="color: #79aaf7;" ></i>
                        <span class="h1 fw-bold mb-1 font-weight-bolder">Registro Usuario</span>
                    </div>

                    <div class="form-outline mb-3">
                        <input type="text" id="txt_nombre" runat="server" class="form-control form-control-lg" required="required" placeholder="Nombre"/>
                        <%--<label class="form-label mt-2" for="txt_email">Dirección de Email</label>--%>
                    </div>

                    <div class="form-outline mb-3">
                        <input type="text" id="txt_apellido" runat="server" class="form-control form-control-lg" required="required" placeholder="Apellido"/>
                    </div>

                    <div class="form-outline mb-3">
                        <input type="text" id="txt_telefono" runat="server" class="form-control form-control-lg" required="required" placeholder="Número de Teléfono (con 0 y 15)"/>
                    </div>

                    <%--Dos textbox por si lo necesitamos para caracteristica y numero por separado--%>
                    <%--<div class="input-group form-outline mb-3" >
                        <span class="input-group-addon">Between</span>
                        <input type="text" class="form-control form-control-lg" placeholder="Type something..." />
                        <span class="input-group-addon" style="border-left: 0; border-right: 0;">and</span>
                        <input type="text" class="form-control form-control-lg" placeholder="Type something..." />
                    </div>--%>

                    <div class="form-outline mb-3">
                        <input type="email" id="txt_email" runat="server" class="form-control form-control-lg" required="required" placeholder="Correo Electronico"/>
                    </div>

                    <div class="form-outline mb-3">
                        <input type="password" id="txt_password" minlength="8" maxlength="32" runat="server" class="form-control form-control-lg" required="required" placeholder="Contraseña"/>
                    </div>
                    <div class="form-outline mb-3">
                        <span class="form-label ml-2" >*La contraseña debe tener al menos 8 caracteres, una mayúscula y un número.</span>
                    </div>

                    <div class="form-outline mb-3">
                        <input type="password" id="txt_passwordCheck" runat="server" minlength="8" maxlength="32" class="form-control form-control-lg" required="required" placeholder="Repita su contraseña"/>
                    </div>
                    
                    <div class="container p-0">
                        <div class="row">
                            <div class="col-sm">
                                <span class="form-label font-weight-bold ml-2">Número DNI</span>
                            </div>
                            <div class="col-sm">
                                <span class="form-label font-weight-bold ml-2" style="border-left: 0; border-right: 0;">Tipo DNI</span>
                            </div>
                        </div>
                        <div class="row p-1">
                            <div class="col-sm">
                                <input type="number" id="txt_dni" runat="server" class="form-control form-control-lg" required="required" placeholder="DNI"/>
                            </div>
                            <div class="col-sm">
                                <select class="form-control form-control-lg" id="txt_tipoDni" runat="server" required="required">
                                 <option>A</option>
                                 <option>B</option>
                                 <option>C</option>
                                 <option>D</option>
                                 <option>E</option>
                         </select> 
                            </div>
                        </div>
                    </div>

                    <%--<div class="input-group form-outline">
                        <span class="mr-3 mt-2">Numero DNI</span>
                        <span class="ml-3 mt-2" style="border-left: 0; border-right: 0;">Tipo DNI</span>
                    </div>--%>

                    <%--<div class="input-group form-outline mb-3">                  
                        <input type="number" id="txt_dni" runat="server" class="form-control form-control-lg mr-3" required="required" placeholder="DNI"/>                        
                        <select class="form-control form-control-lg" id="txt_tipoDni" runat="server" required="required">
                             <option>A</option>
                             <option>B</option>
                             <option>C</option>
                             <option>D</option>
                             <option>E</option>
                         </select> 
                    </div>--%>

                     <%--<div class="form-outline mb-3">
                         <select class="form-control form-control-lg" id="txt_tipoDni" runat="server" required="required">
                             <option>A</option>
                             <option>B</option>
                             <option>C</option>
                             <option>D</option>
                             <option>E</option>
                         </select> 
                    </div>--%>

                    <div class="form-outline mb-3">
                        <label class="form-label font-weight-bold ml-2" for="txt_fec_nac">Fecha de Nacimiento</label>
                        <input type="date" id="txt_fec_nac" runat="server" class="form-control form-control-lg" required="required" placeholder="Correo Electronico"/>
                    </div>

                    <cc1:RecaptchaWidget ID="Recaptcha" runat="server" />

                    <div class="pt-1">                    
                        <asp:Button ID="login" runat="server" CssClass="btn btn-primary btn-lg btn-block" OnClick="btnRegister_Click" Text="Registrarse"/>
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
