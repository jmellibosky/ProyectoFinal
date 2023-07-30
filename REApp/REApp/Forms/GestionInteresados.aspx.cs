using MagicSQL;
using REApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using static REApp.Navegacion;

namespace REApp.Forms
{
    public partial class GestionInteresados : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aca hacemos el get que si o si es un string porque de object a int no deja
            string idUsuario = Session["IdUsuario"].ToString();
            string idRol = Session["IdRol"].ToString();

            //Estos se usan de esta forma porque son ints, ver si hay mejor forma de hacer el set
            int idRolInt = idRol.ToInt();
            int id = idUsuario.ToInt();

            CargarInteresados();
            if (!IsPostBack)
            {
            }
        }

        protected void gvInteresados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int IdInteresado = e.CommandArgument.ToString().ToInt();
            Interesado Interesado = new Interesado().Select(IdInteresado);

            LimpiarModal();

            CargarProvincias();

            hdnIdInteresado.Value = IdInteresado.ToString();
            txtApellido.Text = Interesado.Apellido;
            txtNombre.Text = Interesado.Nombre;
            txtTelefono.Text = Interesado.Telefono;
            txtEmail.Text = Interesado.Email;
            txtObservaciones.Text = Interesado.Observacion;
            if (Interesado.IdProvincia.HasValue)
            {
                ddlProvincia.SelectedValue = Interesado.IdProvincia.Value.ToCryptoID();
                ddlProvincia_SelectedIndexChanged(null, null);
            }
            if (Interesado.IdLocalidad.HasValue)
            {
                ddlLocalidad.SelectedValue = Interesado.IdLocalidad.Value.ToCryptoID();
            }
            MostrarABM();

            if (e.CommandName.Equals("Editar"))
            { // Editar
                HabilitarCampos(true);
            }
            if (e.CommandName.Equals("Detalle"))
            {//Detalle
                HabilitarCampos(false);
            }
            if (e.CommandName.Equals("Eliminar"))
            { // Eliminar
                lblMensajeEliminacion.Text = "¿Desea confirmar la eliminación del Interesado " + Interesado.Nombre + " " + Interesado.Apellido + "?";
                hdnEliminar.Value = Interesado.IdInteresado.ToString();
                pnlAlertaEliminar.Visible = true;
            }
        }

        protected void HabilitarCampos(bool Bool)
        {
            txtApellido.Enabled =
            txtNombre.Enabled =
            txtTelefono.Enabled =
            txtEmail.Enabled =
            txtObservaciones.Enabled =
            ddlLocalidad.Enabled =
            ddlProvincia.Enabled =
            btnGuardar.Visible = Bool;
        }

        protected bool ValidarCampos()
        {
            if (txtNombre.Text.Equals(""))
            {
                Alert("Error", "Por favor, ingrese el nombre del Interesado.", AlertType.error);
                return false;
            }

            if (txtApellido.Text.Equals(""))
            {
                Alert("Error", "Por favor, ingrese el apellido del Interesado", AlertType.error);
                return false;
            }

            if (txtTelefono.Text.Equals("") && txtEmail.Text.Equals(""))
            {
                Alert("Error", "Por favor, ingrese al menos un dato de contacto del Interesado.", AlertType.error);
                return false;

            }

            if (!txtEmail.Text.Equals(""))
            {
                string emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
                if (!Regex.IsMatch(txtEmail.Text, emailPattern))
                {
                    Alert("Error", "Por favor, ingrese un correo electronico válido.", AlertType.error);
                    return false;
                }
            }


            return true;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                Interesado Interesado = null;
                if (hdnIdInteresado.Value.Equals(""))
                { // Insert
                    using (Tn tn = new Tn("bd_reapp"))
                    {
                        try
                        {
                            Interesado = new Interesado();

                            Interesado.FHAlta = DateTime.Now;
                            Interesado.Nombre = txtNombre.Text;
                            Interesado.Apellido = txtApellido.Text;
                            Interesado.Email = txtEmail.Text;
                            Interesado.Telefono = txtTelefono.Text;
                            Interesado.Observacion = txtObservaciones.Text;

                            if (!ddlLocalidad.SelectedValue.Equals("#"))
                            {
                                Interesado.IdLocalidad = ddlLocalidad.SelectedValue.ToIntID();
                            }
                            if (!ddlProvincia.SelectedValue.Equals("#"))
                            {
                                Interesado.IdProvincia = ddlProvincia.SelectedValue.ToIntID();
                            }

                            Interesado.Insert();

                            tn.Commit();

                            Alert("Interesado creado con éxito", "Se ha agregado un nuevo Interesado.", AlertType.success, "/Forms/GestionInteresados.aspx");
                        }
                        catch (Exception ex)
                        {
                            tn.RollBack();
                        }
                    }
                }
                else
                { // Update
                    using (Tn tn = new Tn("bd_reapp"))
                    {
                        try
                        {
                            Interesado = new Interesado().Select(hdnIdInteresado.Value.ToInt());

                            Interesado.FHAlta = DateTime.Now;
                            Interesado.Nombre = txtNombre.Text;
                            Interesado.Apellido = txtApellido.Text;
                            Interesado.Email = txtEmail.Text;
                            Interesado.Telefono = txtTelefono.Text;
                            Interesado.Observacion = txtObservaciones.Text;

                            if (!ddlLocalidad.SelectedValue.Equals("#"))
                            {
                                Interesado.IdLocalidad = ddlLocalidad.SelectedValue.ToIntID();
                            }
                            if (!ddlProvincia.SelectedValue.Equals("#"))
                            {
                                Interesado.IdProvincia = ddlProvincia.SelectedValue.ToIntID();
                            }

                            Interesado.Update();

                            tn.Commit();
                            Alert("Interesado actualizado con éxito", "Se han guardado los datos del Interesado.", AlertType.success, "/Forms/GestionTripulantes.aspx");
                        }
                        catch (Exception)
                        {
                            tn.RollBack();
                        }
                    }
                }
            }
        }

        protected void LimpiarModal()
        {
            ddlLocalidad.Items.Clear();
            ddlProvincia.Items.Clear();
            txtNombre.Text =
            txtApellido.Text =
            txtEmail.Text =
            txtTelefono.Text =
            txtObservaciones.Text = "";
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarModal();
            HabilitarCampos(true);
            CargarProvincias();
            MostrarABM();
        }

        protected void MostrarListado()
        {
            pnlListado.Visible = true;
            btnNuevo.Visible = true;
            pnlABM.Visible = false;
            btnVolver.Visible = false;
            btnGuardar.Visible = true;
        }

        protected void MostrarABM()
        {
            pnlListado.Visible = false;
            btnNuevo.Visible = false;
            pnlABM.Visible = true;
            btnVolver.Visible = true;
            btnGuardar.Visible = true;
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
            Interesado Interesado = new Interesado().Select(hdnEliminar.Value.ToInt());
            Interesado.FHBaja = DateTime.Now;
            Interesado.Update();

            pnlAlertaEliminar.Visible = false;
            Alert("Interesado dado de baja con éxito", "Se ha dado de baja el Interesado seleccionado.", AlertType.success, "/Forms/GestionTripulantes.aspx");
        }

        protected void ddlProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ddlProvincia.SelectedValue.Equals("#"))
            {
                List<Localidad> Localidades = new Localidad().Select($"IdProvincia = {ddlProvincia.SelectedValue.ToIntID()}");

                ddlLocalidad.Items.Clear();
                ddlLocalidad.Items.Add(new ListItem("Seleccionar", "#"));
                foreach (Localidad l in Localidades)
                {
                    ddlLocalidad.Items.Add(new ListItem(l.NombreLocalidad, l.IdLocalidad.ToCryptoID()));
                }
            }
        }

        protected void CargarProvincias()
        {
            List<Provincia> Provincias = new Provincia().Select();

            ddlProvincia.Items.Clear();
            ddlProvincia.Items.Add(new ListItem("Seleccionar", "#"));
            foreach (Provincia p in Provincias)
            {
                ddlProvincia.Items.Add(new ListItem(p.Nombre, p.IdProvincia.ToCryptoID()));
            }
        }

        protected void CargarInteresados()
        {//OBTIENE TODOS LOS INTERESADOS
            using (SP sp = new SP("bd_reapp"))
            {
                DataTable dt = sp.Execute("usp_GetInteresados"
                );

                if (dt.Rows.Count > 0)
                {
                    gvInteresados.DataSource = dt;
                }
                else
                {
                    gvInteresados.DataSource = null;
                }
                gvInteresados.DataBind();
            }
        }

    }
}