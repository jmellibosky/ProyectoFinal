using System.Data;
using MagicSQL;
using System.Security.Cryptography;
using System.Collections.Generic;
using System;

namespace REApp
{
    public class UsuarioController {
        public DataTable GetComboSolicitante()
        {
            DataTable dt = null;
            using (SP sp = new SP("bd_reapp"))
            {
                dt = sp.Execute("usp_GetComboUsuariosSolicitantes");
            }
            return dt;
        }

        public DataTable GetComboMarcaVant()
        {
            DataTable dt = null;
            using (SP sp = new SP("bd_reapp"))
            {
                dt = sp.Execute("usp_GetComboMarcaVant");
            }
            return dt;
        }

        public DataTable GetComboTipoVant()
        {
            DataTable dt = null;
            using (SP sp = new SP("bd_reapp"))
            {
                dt = sp.Execute("usp_GetComboTipoVant");
            }
            return dt;
        }

        public DataTable GetComboRol()
        {
            DataTable dt = null;
            using (SP sp = new SP("bd_reapp"))
            {
                dt = sp.Execute("usp_GetComboRol");
            }
            return dt;
        }
        public DataTable GetComboActividades()
        {
            DataTable dt = null;
            using (SP sp = new SP("bd_reapp"))
            {
                dt = sp.Execute("usp_GetComboActividad");
            }
            return dt;
        }

        public DataTable GetComboModalidades(int IdActividad)
        {
            DataTable dt = null;
            using (SP sp = new SP("bd_reapp"))
            {
                dt = sp.Execute("usp_GetComboModalidadDeActividad", 
                    P.Add("IdActividad", IdActividad)
                    );
            }
            return dt;
        }

        public DataTable GetComboProvincias()
        {
            DataTable dt = null;
            using (SP sp = new SP("bd_reapp"))
            {
                dt = sp.Execute("usp_GetComboProvincias");
            }
            return dt;
        }

        public DataTable GetComboLocalidadPartido(int IdProvincia)
        {
            DataTable dt = null;
            using (SP sp = new SP("bd_reapp"))
            {
                dt = sp.Execute("usp_GetComboLocalidadPartido",
                    P.Add("IdProvincia", IdProvincia)
                    );
            }
            return dt;
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

    }
}