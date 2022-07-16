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
            using (SP sp = new SP("bd_reapp"))
            {
                DataTable dt = sp.Execute("usp_GetComboActividad");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlComboBD.Items.Add(new ListItem(dt.Rows[i]["Nombre"].ToString(), dt.Rows[i]["IdActividad"].ToString().ToInt().ToCryptoID()));
                }
            }
        }

        protected void gvGrilla_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Convierte el Id del modelo en CryptoID.
                string plainId = e.Row.Cells[1].Text;
                e.Row.Cells[1].Text = plainId.ToInt().ToCryptoID();
            }
        }
    }
}