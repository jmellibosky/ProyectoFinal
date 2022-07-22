using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MagicSQL;
using REApp.Models;

namespace REApp.Forms
{
    public partial class GestionVants : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            cargarGvVants();
        }

        protected void cargarGvVants()
        {
            DataTable dt = null;
            using (SP sp = new SP("bd_reapp"))
            {
                dt = sp.Execute("usp_VantConsultar");
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                gvVants.DataSource = dt;
            }
            else
            {
                gvVants.DataSource = null;
            }
            gvVants.DataBind();

        }

        protected void btnNuevoVant_Click(object sender, EventArgs e)
        {
            hdnIdVant.Value = "";
            LimpiarModal();
            CargarComboMarcaVant();
            CargarComboTipoVant();
            CargarComboSolicitante();
            MostrarABM();
        }

        protected void CargarComboMarcaVant()
        {
            ddlMarcaVant.Items.Clear();
            using (SP sp = new SP("bd_reapp"))
            {
                DataTable dt = new UsuarioController().GetComboMarcaVant();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlMarcaVant.Items.Add(new ListItem(dt.Rows[i]["Nombre"].ToString(), dt.Rows[i]["IdMarcaVant"].ToString().ToInt().ToCryptoID()));
                }
            }
        }

        protected void CargarComboTipoVant()
        {
            ddlTipoVant.Items.Clear();
            using (SP sp = new SP("bd_reapp"))
            {
                DataTable dt = new UsuarioController().GetComboTipoVant();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlTipoVant.Items.Add(new ListItem(dt.Rows[i]["Nombre"].ToString(), dt.Rows[i]["IdTipoVant"].ToString().ToInt().ToCryptoID()));
                }
            }
        }

        protected void CargarComboSolicitante()
        {
            ddlSolicitante.Items.Clear();
            using (SP sp = new SP("bd_reapp"))
            {
                DataTable dt = new UsuarioController().GetComboSolicitante();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlSolicitante.Items.Add(new ListItem(dt.Rows[i]["Nombre"].ToString(), dt.Rows[i]["IdUsuario"].ToString().ToInt().ToCryptoID()));
                }
            }
        }

        protected void btnEliminarVant_Click(object sender, EventArgs e)
        {
            int id = int.Parse((sender as LinkButton).CommandArgument);

            Models.Vant Vant = new Models.Vant().Select(id);

            if(Vant.FHBaja is null)
            {
                using (SP sp = new SP("bd_reapp"))
                {
                    sp.Execute("usp_DarDeBajaVant",
                        P.Add("IdVant", Vant.IdVant)
                    );
                }
                cargarGvVants();
            }
            else
            {
                
            }

        }

        protected void btnModificarVant_Click(object sender, EventArgs e)
        {

            int id = int.Parse((sender as LinkButton).CommandArgument);
            Models.Vant Vant = new Models.Vant().Select(id);
            hdnIdVant.Value = id.ToString();
            LimpiarModal();
            CargarComboMarcaVant();
            CargarComboTipoVant();
            MostrarABM();

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Models.Vant Vant = null;
            if (hdnIdVant.Value.Equals(""))
            { // Insert
                using (Tn tn = new Tn("bd_reapp"))
                {
                    Vant = new Models.Vant();
                    Vant.IdMarcaVant = ddlMarcaVant.SelectedValue.ToIntID();
                    Vant.IdTipoVant = ddlTipoVant.SelectedValue.ToIntID();
                    Vant.Modelo = txtModelo.Text;
                    Vant.FHAlta = DateTime.Now;
                    Vant.IdUsuario = ddlSolicitante.SelectedValue.ToIntID();
                    Vant.Insert();

                }
            }
            else
            { // Update

                Vant = new Models.Vant().Select(hdnIdVant.Value.ToInt());

                
                Vant.IdMarcaVant = ddlMarcaVant.SelectedValue.ToIntID();
                Vant.IdTipoVant = ddlTipoVant.SelectedValue.ToIntID();
                Vant.Modelo = txtModelo.Text;
                Vant.Update();
            }

            MostrarListado();
            cargarGvVants();
        }

        protected void LimpiarModal()
        {
            ddlMarcaVant.Items.Clear();
            ddlTipoVant.Items.Clear();
            txtModelo.Text = "";
        }


        protected void MostrarListado()
        {
            pnlListado.Visible = true;
            btnNuevo.Visible = true;
            pnlABM.Visible = false;
            btnVolver.Visible = false;
        }

        protected void MostrarABM()
        {
            pnlListado.Visible = false;
            btnNuevo.Visible = false;
            pnlABM.Visible = true;
            btnVolver.Visible = true;
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            MostrarListado();
        }

    }
}