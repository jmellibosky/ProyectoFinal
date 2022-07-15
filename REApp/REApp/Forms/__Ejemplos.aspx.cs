using MagicSQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace REApp.Forms
{
    public partial class __Ejemplos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CargarComboBD();
        }

        protected void CargarComboBD()
        {
            ddlComboBD.Items.Clear();

            // Si queremos un valor predeterminado
            ddlComboBD.Items.Add(new ListItem("Todos", "#"));
            using (SP sp = new SP("ProyectoFinal"))
            {
                DataTable dt = sp.Execute("usp_GetComboActividad");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlComboBD.Items.Add(new ListItem(dt.Rows[i]["Nombre"].ToString(), dt.Rows[i]["IdActividad"].ToString().ToInt().ToCryptoID()));
                }
            }
        }
    }
}