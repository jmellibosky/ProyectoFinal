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
            //Aca hacemos el get que si o si es un string porque de object a int no deja
            string idUsuario = Session["IdUsuario"].ToString();
            string idRol = Session["IdRol"].ToString();

            //Estos se usan de esta forma porque son ints, ver si hay mejor forma de hacer el set
            int idRolInt = idRol.ToInt();
            int id = idUsuario.ToInt();

            if (!IsPostBack)
            {
                if (idRolInt == 1)
                {
                    CargarComboSolicitante();
                }

                if (idRolInt == 3)
                {
                    CargarComboSolicitante();
                    ddlSolicitante.SelectedValue = id.ToCryptoID().ToString();
                    ddlSolicitante.Enabled = false;
                    filtrarTripulantesXSolicitante();
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
        protected void CargarComboModalSolicitante()
        {
            ddlModalSolicitante.Items.Clear();
            using (SP sp = new SP("bd_reapp"))
            {
                DataTable dt = new UsuarioController().GetComboSolicitante();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlModalSolicitante.Items.Add(new ListItem(dt.Rows[i]["Nombre"].ToString(), dt.Rows[i]["IdUsuario"].ToString().ToInt().ToCryptoID()));
                }
            }
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
        filtrarTripulantesXSolicitante();
        }

        protected void filtrarTripulantesXSolicitante()
        {
            DataTable dt = null;
            using (SP sp = new SP("bd_reapp"))
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
                txtModalCorreo.Text = Tripulacion.Correo;

                MostrarABM();
            }
            else
            { // Eliminar

                lblMensajeEliminacion.Text = "¿Desea confirmar la eliminación del tripulante " + Tripulacion.Nombre + " " + Tripulacion.Apellido + "?";
                hdnEliminar.Value = Tripulacion.IdTripulacion.ToString();
                pnlAlertaEliminar.Visible = true;
            }
        }

        protected bool ValidarCampos()
        {
            if (txtModalNombre.Text.Equals(""))
            {
                txtErrorHeader.Text = "Error";
                txtErrorBody.Text = "Por favor, ingrese el nombre del tripulante.";
                pnlError.Visible = true;
                return false;
            }
            if (txtModalApellido.Text.Equals(""))
            {
                txtErrorHeader.Text = "Error";
                txtErrorBody.Text = "Por favor, ingrese el apellido del tripulante.";
                pnlError.Visible = true;
                return false;
            }
            if (txtModalDNI.Text.Equals(""))
            {
                txtErrorHeader.Text = "Error";
                txtErrorBody.Text = "Por favor, ingrese el DNI del tripulante.";
                pnlError.Visible = true;
                return false;
            }
            if (txtModalFechaNacimiento.Text.Equals(""))
            {
                txtErrorHeader.Text = "Error";
                txtErrorBody.Text = "Por favor, ingrese la fecha de nacimiento del tripulante.";
                pnlError.Visible = true;
                return false;
            }
            if (txtModalFechaNacimiento.Text.ToDateTimeNull() == null)
            {
                txtErrorHeader.Text = "Error";
                txtErrorBody.Text = "Por favor, ingrese una fecha de nacimiento válida.";
                pnlError.Visible = true;
                return false;
            }
            if (txtModalTelefono.Text.Equals("") && txtModalCorreo.Text.Equals(""))
            {
                txtErrorHeader.Text = "Error";
                txtErrorBody.Text = "Por favor, ingrese al menos un dato de contacto del tripulante.";
                pnlError.Visible = true;
                return false;
            }

            pnlError.Visible = false;
            return true;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                Models.Tripulacion Tripulacion = null;
                if (hdnIdTripulacion.Value.Equals(""))
                { // Insert
                    using (Tn tn = new Tn("bd_reapp"))
                    {
                        Tripulacion = new Models.Tripulacion();
                        Tripulacion.Nombre = txtModalNombre.Text;
                        Tripulacion.Apellido = txtModalApellido.Text;
                        Tripulacion.DNI = txtModalDNI.Text;
                        Tripulacion.FechaNacimiento = txtModalFechaNacimiento.Text.ToDateTime();
                        Tripulacion.Telefono = txtModalTelefono.Text;
                        Tripulacion.Correo = txtModalCorreo.Text;
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
                    Tripulacion.Correo = txtModalCorreo.Text;
                    Tripulacion.Update();
                }

                MostrarListado();
                btnFiltrar_Click(null, null);
            }
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
            pnlError.Visible = false;
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

        protected void btnCancelarEliminacion_Click(object sender, EventArgs e)
        {
            pnlAlertaEliminar.Visible = false;
        }

        protected void btnConfirmarEliminacion_Click(object sender, EventArgs e)
        {
            using (SP sp = new SP("bd_reapp"))
            {
                sp.Execute("usp_DarDeBajaTripulacionUsuario",
                    P.Add("IdTripulacion", hdnEliminar.Value.ToInt()),
                    P.Add("IdUsuario", ddlSolicitante.SelectedValue.ToIntID())
                );
            }
            pnlAlertaEliminar.Visible = false;

            btnFiltrar_Click(null, null);
        }


    }
}