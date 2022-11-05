﻿using MagicSQL;
using System;
using System.Security.Cryptography;

namespace REApp.Forms
{
    public partial class CambiarContrasena : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //Esto podriamos globalizarlo, porque lo repetimos en el registro de usuario
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

        protected void Pass_Click(object sender, EventArgs e)
        {
            //Falta encriptacion si fuese necesario, se prueba primero funcionalidad
            int idUsuario = 0;
            if (Request["ID"] != null)
            {
                idUsuario = Request["ID"].ToInt();
            }

            if (txt_pass.Value == txt_passcheck.Value)
            {
                string password = txt_pass.Value;

                //Usamos el SP para cambiar clave segun ID
                string salt = SecurityHelper.GenerateSalt(70);
                string hashedpass = SecurityHelper.HashPassword(password, salt, 10101, 70);
                using (SP sp = new SP("bd_reapp"))
                {
                    sp.Execute("usp_RestaurarContrasena", P.Add("idusuario", idUsuario), P.Add("contrasenaHash", hashedpass), P.Add("contrasenaSalt", salt));
                }

                Response.Redirect("/Forms/UserRestauracionFinalizada.aspx");

            }
            else
            {
                //Aca faltaria el SweetAlert
                txt_pass.Value = "";
                txt_passcheck.Value = "";
                txt_pass.Focus();
            }
        }
    }
}