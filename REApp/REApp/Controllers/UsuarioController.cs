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
    }
}