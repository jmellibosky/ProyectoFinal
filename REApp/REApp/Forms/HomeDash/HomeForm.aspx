<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomeForm.aspx.cs" Inherits="REApp.Forms.HomeDash.HomeForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
        <meta name="description" content="" />
        <meta name="author" content="" />
        <title>Home Dashboard</title>
        <!-- Favicon y FontAwesome-->
        <link rel="icon" type="image/x-icon" href="assets/favicon.ico" />
        <script src="https://kit.fontawesome.com/8e4807e881.js" crossorigin="anonymous"></script>
        <!-- Bootstrap core JS-->
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
        <!-- Core theme JS-->
        <script src="js/scripts.js"></script>
        <!-- Core theme CSS (includes Bootstrap)-->
        <link href="css/styles.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div runat="server" class="container-sm" style="position: absolute; top: 30%; left: 30%; width: 650px">
            <h1 id="lblUsername" runat="server" class="display-1">Usuario</h1>
            <h1 class="display-6" style="position:absolute; left: 15%">Bienvenido al Sistema REA</h1>
        </div>
    </form>
</body>
</html>
