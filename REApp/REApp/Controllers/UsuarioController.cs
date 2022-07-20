using System.Data;
using MagicSQL;

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

    }
}