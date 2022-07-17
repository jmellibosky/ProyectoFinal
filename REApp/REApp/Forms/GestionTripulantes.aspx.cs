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
    public partial class GestionTripulantes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CargarComboSolicitante();
        }

        protected void CargarComboSolicitante()
        {
            ddlSolicitante.Items.Clear();
            using (SP sp = new SP("ProyectoFinal"))
            {
                DataTable dt = new UsuarioController().GetComboSolicitante();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlSolicitante.Items.Add(new ListItem(dt.Rows[i]["Nombre"].ToString(), dt.Rows[i]["IdUsuario"].ToString().ToInt().ToCryptoID()));
                }
            }
        }
        protected void CargarComboModalSolicitante()
        {
            ddlModalSolicitante.Items.Clear();
            using (SP sp = new SP("ProyectoFinal"))
            {
                DataTable dt = new UsuarioController().GetComboSolicitante();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlModalSolicitante.Items.Add(new ListItem(dt.Rows[i]["Nombre"].ToString(), dt.Rows[i]["IdUsuario"].ToString().ToInt().ToCryptoID()));
                }
            }
        }

        protected void gvTripulantes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Convierte el Id del modelo en CryptoID.
                string plainId = e.Row.Cells[1].Text;
                e.Row.Cells[1].Text = plainId.ToInt().ToCryptoID();
            }
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            using (SP sp = new SP("ProyectoFinal"))
            {
                if (!ddlSolicitante.SelectedValue.Equals("#"))
                {
                    dt = sp.Execute("usp_GetTripulacionDeUsuario", P.Add("IdUsuario", ddlSolicitante.SelectedValue.ToIntID()));
                }
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                gvTripulantes.DataSource = dt;
            }
            else
            {
                gvTripulantes.DataSource = null;
            }
            gvTripulantes.DataBind();
        }

        protected void gvTripulantes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int IdTripulacion = e.CommandArgument.ToString().ToInt();
            Models.Tripulacion Tripulacion = new Models.Tripulacion().Select(IdTripulacion);

            if (e.CommandName.Equals("Detalle"))
            { // Detalle
                LimpiarModal();

                CargarComboModalSolicitante();

                ddlModalSolicitante.SelectedValue = ddlSolicitante.SelectedValue;
                ddlModalSolicitante.Enabled = false;

                hdnIdTripulacion.Value = IdTripulacion.ToString();
                txtModalApellido.Text = Tripulacion.Apellido;
                txtModalNombre.Text = Tripulacion.Nombre;
                txtModalDNI.Text = Tripulacion.DNI;
                txtModalFechaNacimiento.Text = Tripulacion.FechaNacimiento.ToString();
                txtModalTelefono.Text = Tripulacion.Telefono;
                //txtModalCorreo.Text = Tripulacion.Correo;

                MostrarABM();
            }
            else
            { // Eliminar
                using (SP sp = new SP("ProyectoFinal"))
                {
                    sp.Execute("usp_DarDeBajaTripulacionUsuario",
                        P.Add("IdTripulacion", Tripulacion.IdTripulacion),
                        P.Add("IdUsuario", ddlSolicitante.SelectedValue.ToIntID())
                    );
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Models.Tripulacion Tripulacion = null;
            if (hdnIdTripulacion.Value.Equals(""))
            { // Insert
                using (Tn tn = new Tn("ProyectoFinal"))
                {
                    Tripulacion = new Models.Tripulacion();
                    Tripulacion.Nombre = txtModalNombre.Text;
                    Tripulacion.Apellido = txtModalApellido.Text;
                    Tripulacion.DNI = txtModalDNI.Text;
                    Tripulacion.FechaNacimiento = txtModalFechaNacimiento.Text.ToDateTime();
                    Tripulacion.Telefono = txtModalTelefono.Text;
                    //Tripulacion.Correo = txtModalCorreo.Text;
                    Tripulacion.FHAlta = DateTime.Now;
                    Tripulacion.Insert();

                    Models.TripulacionUsuario TripulacionUsuario = new Models.TripulacionUsuario();
                    TripulacionUsuario.IdUsuario = ddlModalSolicitante.SelectedValue.ToIntID();
                    TripulacionUsuario.IdTripulacion = Tripulacion.IdTripulacion;
                    TripulacionUsuario.FHAlta = DateTime.Now;
                    TripulacionUsuario.Insert();
                }
            }
            else
            { // Update
                Tripulacion = new Models.Tripulacion().Select(hdnIdTripulacion.Value.ToInt());
                Tripulacion.Nombre = txtModalNombre.Text;
                Tripulacion.Apellido = txtModalApellido.Text;
                Tripulacion.DNI = txtModalDNI.Text;
                Tripulacion.FechaNacimiento = txtModalFechaNacimiento.Text.ToDateTime();
                Tripulacion.Telefono = txtModalTelefono.Text;
                //Tripulacion.Correo = txtModalCorreo.Text;
                Tripulacion.Update();
            }

            MostrarListado();
            btnFiltrar_Click(null, null);
        }

        protected void LimpiarModal()
        {
            ddlModalSolicitante.Items.Clear();
            txtModalApellido.Text =
            txtModalNombre.Text =
            txtModalDNI.Text =
            txtModalFechaNacimiento.Text =
            txtModalTelefono.Text =
            txtModalCorreo.Text = "";
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarModal();

            CargarComboModalSolicitante();

            ddlModalSolicitante.SelectedValue = ddlSolicitante.SelectedValue;
            ddlModalSolicitante.Enabled = false;

            MostrarABM();
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