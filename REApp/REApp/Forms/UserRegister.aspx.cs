using MagicSQL;
using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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

        protected void btnRegister_Click(object sender, EventArgs e)
        {

            //Ver tema rol, si asignamos de una el Solicitante y despues el admin los eleva, charlar bien esto
            int idRol = 1;
            string nombre = txt_nombre.Value;
            string apellido = txt_apellido.Value;
            string DNI = txt_dni.Value;
            string correo = txt_email.Value;
            string password = txt_password.Value;
            string passwordcheck = txt_passwordCheck.Value;
            string fechaNac = txt_fec_nac.Value; 

            string salt = SecurityHelper.GenerateSalt(70);
            string hashedpass = SecurityHelper.HashPassword(password, salt, 10101, 70);

            //Primero se deberia verificar que el mail no esta usado, tengo que crear otro SP
            using (SP sp = new SP("bd_reapp"))
            {
                sp.Execute("__UsuarioInsert_v1", P.Add("Nombre", nombre), P.Add("Apellido", apellido), P.Add("Email", correo), P.Add("Dni", DNI), P.Add("TipoDni", "A"), P.Add("IdRol", idRol), P.Add("CreatedOn", DateTime.Today), P.Add("CreatedBy", null), P.Add("DeletedOn", null), P.Add("DeletedBy", null), P.Add("FechaNacimiento", fechaNac), P.Add("Telefono", null), P.Add("Contraseña", hashedpass), P.Add("SaltKey", salt));
            }

            //En caso de error porque el mail esta usado o algo asi, hay que limpiar todo
            //txt_nombre.Value = "";
            //txt_apellido.Value = "";
            //txt_email.Value = "";
            //txt_password.Value = "";
            //txt_passwordCheck.Value = "";
            //txt_dni.Value = "";
            //txt_fec_nac.Value = null;

            //Esta redireccion la voy a hacer a un formulario diciendo que se hizo de forma correcta
            Response.Redirect("/Forms/UserCorrectRegister.aspx");

        }
    }
}