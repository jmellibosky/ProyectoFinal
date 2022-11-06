using MagicSQL;
using REApp.Controllers;
using REApp.Models;
using System;
using System.Data;
using static REApp.Navegacion;

namespace REApp.Forms
{
    public partial class UserSendValidationEmail : System.Web.UI.Page
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

            using (SP sp = new SP("bd_reapp"))
            {
                dt = sp.Execute("usp_IdCheck", P.Add("email", email));
            }

            if (dt.Rows.Count > 0)
            {
                idUsuario = dt.Rows[0][0].ToString();

                EnviarMailConfirmacion(idUsuario.ToInt());

                Alert("Email enviado", "Por favor, revise su casilla de correo y siga los pasos indicados.", AlertType.success, "/Forms/UserLogin.aspx");
            }
            else
            {
                Alert("Error", "El correo ingresado no corresponde con ningún usuario.", AlertType.error);
            }
        }
        protected void EnviarMailConfirmacion(int IdUsuario)
        {
            Usuario usuario = new Usuario().Select(IdUsuario);

            HTMLBuilder builder = new HTMLBuilder("Confirmación de Usuario", "GenericMailTemplate.html");

            string leftpart = Request.Url.GetLeftPart(UriPartial.Authority);
            string frmValidacion = "/Forms/UserValidation.aspx";
            string parameters = $"?U={usuario.IdUsuario.ToCryptoID()}";

            string url = $"{leftpart}{frmValidacion}{parameters}";

            builder.AppendTexto($"Hola {usuario.Nombre} {usuario.Apellido}.");
            builder.AppendSaltoLinea(2);
            builder.AppendTexto("Bienvenido a REApp, la plataforma de la Empresa Argentina de Navegación Aérea para la gestión integral de Reservas de Espacio Aéreo.");
            builder.AppendSaltoLinea(1);
            builder.AppendTexto("Para validar su email y comenzar a utilizar el sistema, ingrese al siguiente enlace de verificación: ");
            builder.AppendURL(url, "Validar Email");
            builder.AppendSaltoLinea(2);
            builder.AppendTexto("Si usted no ha solicitado registrarse en REApp, simplemente ignore este correo.");
            builder.AppendSaltoLinea(2);
            builder.AppendTexto("Saludos.");
            builder.AppendSaltoLinea(1);
            builder.AppendTexto("Equipo de REApp.");
            string cuerpo = builder.ConstruirHTML();

            MailController mail = new MailController("Confirmación de Usuario", cuerpo);
            mail.Add($"{usuario.Nombre} {usuario.Apellido}", usuario.Email);
            mail.Enviar();
        }
    }
}