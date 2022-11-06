using MagicSQL;
using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using REApp.Models;
using REApp.Controllers;
using static REApp.Navegacion;

namespace REApp.Forms
{
    public partial class UserRegister : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public class SecurityHelper
        {

            //Creamos la salt -> Valor random que se guardaria con cada pass(Se usa cuando querramos generar una clave nomas)
            public static string GenerateSalt(int nSalt)
            {
                var saltBytes = new byte[nSalt];

                using (var provider = new RNGCryptoServiceProvider())
                {
                    provider.GetNonZeroBytes(saltBytes);
                }

                return Convert.ToBase64String(saltBytes);
            }

            public static string HashPassword(string password, string salt, int nIterations, int nHash)
            {
                var saltBytes = Convert.FromBase64String(salt);

                using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, nIterations))
                {
                    return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(nHash));
                }
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

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            //Ver tema rol, si asignamos de una el Solicitante y despues el admin los eleva, charlar bien esto
            //1 Admin, 2 Operador, 3 Solicitante, 4 Interesado segun la bd_reapp
            int idRol = 3;
            string nombre = txt_nombre.Value;
            string apellido = txt_apellido.Value;
            string telefono = txt_telefono.Value;
            string DNI = txt_dni.Value;
            string TipoDni = txt_tipoDni.Value;
            string correo = txt_email.Value;
            string password = txt_password.Value;
            string passwordcheck = txt_passwordCheck.Value;
            string fechaNac = txt_fec_nac.Value;

            int flagCorreo;
            using (SP spCorreo = new SP("bd_reapp"))
            {
                flagCorreo = spCorreo.Execute("usp_CheckCorreoDup", P.Add("correo", correo)).Rows.Count;
            }

            if (flagCorreo == 0)
            {
                if (password == passwordcheck && nombre != null && apellido != null && DNI != null && TipoDni != null && correo != null && password != null && fechaNac != null)
                {
                    string salt = SecurityHelper.GenerateSalt(70);
                    string hashedpass = SecurityHelper.HashPassword(password, salt, 10101, 70);

                    //Primero se deberia verificar que el mail no esta usado, tengo que crear otro SP
                    using (SP sp = new SP("bd_reapp"))
                    {
                        int IdUsuario = sp.Execute("__UsuarioInsert_v1", P.Add("Nombre", nombre), P.Add("Apellido", apellido), P.Add("Email", correo), P.Add("Dni", DNI), P.Add("TipoDni", TipoDni), P.Add("IdRol", idRol), P.Add("CreatedOn", DateTime.Today), P.Add("CreatedBy", null), P.Add("DeletedOn", null), P.Add("DeletedBy", null), P.Add("FechaNacimiento", fechaNac), P.Add("Telefono", telefono), P.Add("Password", hashedpass), P.Add("SaltKey", salt)).Rows[0][0].ToString().ToInt();
                        EnviarMailConfirmacion(IdUsuario);

                        Alert("Email enviado", "Por favor, revise su casilla de correo y siga los pasos indicados.", AlertType.success);
                    }
                    Response.Redirect("/Forms/UserCorrectRegister.aspx");
                }
                else
                {
                    //Aca iria alerta tipo "Intente de nuevo"

                    //En caso de error porque el mail esta usado o algo asi, hay que limpiar todo
                    txt_nombre.Value = "";
                    txt_apellido.Value = "";
                    txt_email.Value = "";
                    txt_telefono.Value = "";
                    txt_password.Value = "";
                    txt_passwordCheck.Value = "";
                    txt_dni.Value = "";
                    txt_fec_nac.Value = null;
                    txt_nombre.Focus();
                }
            }
            else
            {
                //Respuesta de correo duplicado y podria ser tambien de correo invalido ???
                txt_nombre.Value = "";
                txt_apellido.Value = "";
                txt_email.Value = "";
                txt_telefono.Value = "";
                txt_password.Value = "";
                txt_passwordCheck.Value = "";
                txt_dni.Value = "";
                txt_fec_nac.Value = null;
                txt_nombre.Focus();
            }
        }
    }
}