﻿using MagicSQL;
using REApp.Models;
using System;
using System.Data;
using System.Security.Cryptography;
using static REApp.Navegacion;

namespace REApp.Forms
{
    public partial class UserLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string correo = txt_email.Value;
            string password = txt_password.Value;
            string saltkey;
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            int flagLogin;

            //Podriamos implementar SweetAlerts para dejarlo mas bonito
            using (SP sp = new SP("bd_reapp"))
            {
                dt = sp.Execute("usp_CorreoSaltCheck", P.Add("correo", correo));
                saltkey = dt.Rows[0][0].ToString();
            }

            string hashedpass = SecurityHelper.HashPassword(password, saltkey, 10101, 70);

            using (SP sp2 = new SP("bd_reapp"))
            {
                dt2 = sp2.Execute("usp_CorreoPassCheck", P.Add("correo", correo), P.Add("pass", hashedpass));
                flagLogin = dt2.Rows.Count;
            }

            if (flagLogin == 1)
            {
                //Ver si esto es completamente seguro
                string idUsuario = dt2.Rows[0][0].ToString();

                Usuario usuario = new Usuario().Select(idUsuario.ToInt());

                string nombreuser = usuario.Nombre;
                string apellidouser = usuario.Apellido;
                string idRol = usuario.IdRol.ToString();
                string nombrecompleto = nombreuser + " " + apellidouser;
                bool ValidacionCorreo = usuario.ValidacionCorreo;

                Session["Username"] = nombrecompleto;
                Session["UsuarioCompleto"] = dt2;
                Session["IdUsuario"] = idUsuario;
                Session["IdRol"] = idRol;

                if (ValidacionCorreo)
                {
                    Response.Redirect("/Forms/HomeDash/HomeDash.aspx");
                }
                else
                {
                    Alert("Validación de Cuenta", "Es necesario que valide su cuenta antes de ingresar.", AlertType.error);
                }
            }
            else
            {
                txt_password.Value = "";
                txt_email.Focus();
            }

        }

        //Inicio la funcion general
        public class SecurityHelper
        {
            //Creamos el hash con el salt
            public static string HashPassword(string password, string salt, int nIterations, int nHash)
            {
                var saltBytes = Convert.FromBase64String(salt);

                using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, nIterations))
                {
                    return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(nHash));
                }
            }
        }
    }
}
