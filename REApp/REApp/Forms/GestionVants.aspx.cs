using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MagicSQL;

namespace REApp.Forms
{
    public partial class GestionarVants : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void BindGrid()
        {
            string constr = ConfigurationManager.ConnectionStrings["bd_reapp"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "Select Vant.IdVant, MarcaVant.Nombre, TipoVant.Nombre, Vant.FHAlta, Vant.FHBaja, Vant.Modelo FROM Vant JOIN MarcaVant ON Vant.IdMarcaVant=MarcaVant.IdMarcaVant JOIN TipoVant ON Vant.IdMarcaVant=TipoVant.IdTipoVant";
                    cmd.Connection = con;
                    con.Open();
                    GridView1.DataSource = cmd.ExecuteReader();
                    GridView1.DataBind();
                    con.Close();
                }
            }

        }

        protected void lnkEliminarVant_Click(object sender, EventArgs e)
        {
            int id = int.Parse((sender as LinkButton).CommandArgument);
            string constr = ConfigurationManager.ConnectionStrings["bd_reapp"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "DELETE FROM Vant WHERE Vant.IdVant = @IdVant";
                    cmd.Parameters.AddWithValue("@IdVant", id);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        sdr.Read();
                    }
                    con.Close();
                }
            }
            BindGrid();
        }

        //protected void lnkActualizarVant_Click(object sender, EventArgs e)
        //{
        //    int id = int.Parse((sender as LinkButton).CommandArgument);
        //    string constr = ConfigurationManager.ConnectionStrings["bd_reapp"].ConnectionString;
        //    using (SqlConnection con = new SqlConnection(constr))
        //    {
        //        using (SqlCommand cmd = new SqlCommand())
        //        {
        //            cmd.CommandText = "UPDATE FROM Vant WHERE Vant.IdVant = @IdVant";
        //            cmd.Parameters.AddWithValue("@IdVant", id);
        //            cmd.Connection = con;
        //            con.Open();
        //            using (SqlDataReader sdr = cmd.ExecuteReader())
        //            {
        //                sdr.Read();
        //            }
        //            con.Close();
        //        }
        //    }
        //    BindGrid();
        //}
    }
}