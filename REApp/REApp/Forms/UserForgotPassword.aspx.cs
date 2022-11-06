using MagicSQL;
using System;
using System.Data;

namespace REApp.Forms
{
    public partial class UserForgotPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void EnviarMail(object sender, EventArgs e)
        {
            //Muy parecido todo a lo que hacemos con el login, al llamar el SP
            string email = txt_email.Value;
            DataTable dt = new DataTable();
            //Usar excepcion try catch luego
            string idUsuario;
            string nombre = "Nombre";

            using (SP sp = new SP("bd_reapp"))
            {
                dt = sp.Execute("usp_IdCheck", P.Add("email", email));
                //Aca falta excepcion try por si el mail no existe
                idUsuario = dt.Rows[0][0].ToString();
                nombre = dt.Rows[0][1].ToString();
            }

            string leftpart = Request.Url.GetLeftPart(UriPartial.Authority);
            string frmValidacion = "/Forms/CambiarContrasena.aspx";
            string parameters = $"?ID={idUsuario.ToInt().ToCryptoID()}";

            string url = $"{leftpart}{frmValidacion}{parameters}";

            Controllers.HTMLBuilder builder = new Controllers.HTMLBuilder("Olvidé mi contraseña - REApp", "GenericMailTemplate.html");

            builder.AppendTexto("Buen día " + nombre + ".");
            builder.AppendSaltoLinea(2);
            builder.AppendTexto("Para recuperar su contraseña, ingrese al siguiente enlace por favor.");
            builder.AppendSaltoLinea(1);
            builder.AppendURL(url, "Cambio de contraseña.");
            builder.AppendSaltoLinea(1);
            builder.AppendTexto("Recuerde como buenas practicas utilizar símbolos, mayúsculas, minúsculas y números en su contraseña.");

            string cuerpo = builder.ConstruirHTML();

            MailController mail = new MailController("Recuperación de Contraseña", cuerpo);

            mail.Add(nombre, email);

            bool Exito = mail.Enviar();

            //Aca estaria bueno un SweetAlert diciendo que revise el mail
            Response.Redirect("/Forms/UserLogin.aspx");
        }
    }
}