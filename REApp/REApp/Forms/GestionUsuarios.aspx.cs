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
    public partial class GestionUsuarios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            cargarGvUsuarios();
            CargarComboRol();
        }

        //Carga de datos

        protected void cargarGvUsuarios()
        {
            DataTable dt = null;
            using (SP sp = new SP("bd_reapp"))
            {

                dt = sp.Execute("usp_GetUsuariosRol");

            }
            if (dt != null && dt.Rows.Count > 0)
            {
                gvUsuarios.DataSource = dt;
            }
            else
            {
                gvUsuarios.DataSource = null;
            }
            gvUsuarios.DataBind();

        }

        // Acciones ejecutadas por pnlGvUsuarios
        protected void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            int IdUsuario = e.CommandArgument.ToString().ToInt();
            Models.Usuario Usuario = new Models.Usuario().Select(IdUsuario);

            if (e.CommandName.Equals("DisplayUser"))
            {
                CargarDatosUsuario(Usuario);
                MostrarABMUsuario();
            }
            else if (e.CommandName.Equals("UpdateUser"))
            {
                CargarDatosUsuario(Usuario);
                MostrarModificacionUsuario();
            }
            else
            {
                EliminarUsuario(Usuario);
            }
            
        }

        //Eliminar Usuario
        protected void EliminarUsuario(Models.Usuario Usuario)
        {
            using (SP sp = new SP("bd_reapp"))
            {
                sp.Execute("__UsuarioDelete_v1",
                    P.Add("IdUsuario", Usuario.IdUsuario)
                );
            }

            cargarGvUsuarios();
        }

        //Modificar Usuario
        protected void ModificarUsuario(Models.Usuario Usuario)
        {
            using (SP sp = new SP("bd_reapp"))
            {
                sp.Execute("usp_UpdateUser",
                    P.Add("IdUsuario", Usuario.IdUsuario)
                );
            }
        }

        // Cargar datos usuario
        protected void CargarDatosUsuario(Models.Usuario Usuario)
        {
            //string NombreRol = GetRolUsuario(Usuario);
            hdnIdRol.Value = Usuario.IdRol.ToCryptoID().ToString();

            txtNombre.Text = Usuario.Nombre;
            txtApellido.Text = Usuario.Apellido;
            ddlRol.SelectedValue = Usuario.IdRol.ToCryptoID().ToString(); // comboBox
            txtDNI.Text = Usuario.Dni.ToString();
            txtTipoDni.Text = Usuario.TipoDni;
            txtFechaNacimiento.Text = Usuario.FechaNacimiento.ToString();
            txtCorreo.Text = Usuario.Email;
            txtTelefono.Text = Usuario.Telefono;
            hdnIdUsuario.Value = Usuario.IdUsuario.ToString();
        }

        //Visualización
        protected void MostrarABMUsuario()
        {
            pnlABM.Visible = true;
            btnGuardar.Visible = false;
            btnNuevo.Visible = false;
            btnVolver.Visible = true;
            pnlGvUsuarios.Visible = false;

            //Habilitacion de campos
            txtNombre.Enabled =
            txtApellido.Enabled =
            ddlRol.Enabled =
            txtDNI.Enabled =
            txtTipoDni.Enabled =
            txtFechaNacimiento.Enabled =
            txtCorreo.Enabled =
            txtTelefono.Enabled = false;

        }

        protected void OcultarABMUsuario()
        {
            pnlABM.Visible = false;
            btnGuardar.Visible = false;
            btnNuevo.Visible = true;
            btnVolver.Visible = false;
            pnlGvUsuarios.Visible = true;
        }

        protected void MostrarModificacionUsuario()
        {
            pnlABM.Visible = true;
            btnGuardar.Visible = true;
            btnNuevo.Visible = false;
            btnVolver.Visible = true;
            pnlGvUsuarios.Visible = false;

            //Habilitacion de campos
            txtNombre.Enabled =
            txtApellido.Enabled =
            txtDNI.Enabled =
            txtTipoDni.Enabled =
            txtFechaNacimiento.Enabled =
            txtCorreo.Enabled =
            txtTelefono.Enabled = true;

            ddlRol.Enabled = true;
            //int numId = ddlRol.SelectedValue.ToIntID();
        }

        protected string GetRolUsuario(Models.Usuario Usuario)
        {
            int IdRol = Usuario.IdRol;
            Models.Rol Rol = new Models.Rol().Select(IdRol);

            return Rol.Nombre;
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            OcultarABMUsuario();
        }

        protected void CargarComboRol()
        {
            ddlRol.Items.Clear();
            using (SP sp = new SP("bd_reapp"))
            {
                DataTable dt = new UsuarioController().GetComboRol();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlRol.Items.Add(new ListItem(dt.Rows[i]["Nombre"].ToString(), dt.Rows[i]["IdRol"].ToString().ToInt().ToCryptoID()));
                }
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            MostrarModificacionUsuario();

            //Setear campos
            txtNombre.Text =
            txtApellido.Text =
            txtDNI.Text =
            txtTipoDni.Text =
            txtCorreo.Text =
            txtTelefono.Text =
            txtFechaNacimiento.Text = "";
            ddlRol.SelectedIndex = 0;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Models.Usuario Usuario = null;

            int rolID = ddlRol.SelectedValue.ToInt();


            if (hdnIdUsuario.Value.Equals(""))
            {
                using (Tn tn = new Tn("bd_reapp"))
                {
                    Usuario = new Models.Usuario();
                    Usuario.Nombre = txtNombre.Text;
                    Usuario.Apellido = txtApellido.Text;
                    Usuario.IdRol = ddlRol.SelectedValue.ToIntID();
                    Usuario.Dni = txtDNI.Text.ToInt();
                    Usuario.TipoDni = txtTipoDni.Text;
                    Usuario.FechaNacimiento = txtFechaNacimiento.Text.ToDateTime();
                    Usuario.Telefono = txtTelefono.Text;
                    Usuario.Email = txtCorreo.Text;
                    Usuario.CreatedOn = DateTime.Now;
                    Usuario.CreatedBy = 1; // -------------------------------------------> Modificar para que lo cree el usuario que este logueado
                    Usuario.DeletedOn = null;
                    Usuario.DeletedBy = null;
                    Usuario.Insert();

                    OcultarABMUsuario();
                    cargarGvUsuarios();
                }
            }
            else
            {
                // Update
                Usuario = new Models.Usuario().Select(hdnIdUsuario.Value.ToInt());
                Usuario.Nombre = txtNombre.Text;
                Usuario.Apellido = txtApellido.Text;
                //Usuario.IdRol = ddlRol.SelectedValue.ToIntID();
                Usuario.IdRol = hdnIdRol.Value.ToInt();
                Usuario.Dni = txtDNI.Text.ToInt();
                Usuario.TipoDni = txtTipoDni.Text;
                Usuario.FechaNacimiento = txtFechaNacimiento.Text.ToDateTime();
                Usuario.Telefono = txtTelefono.Text;
                Usuario.Email = txtCorreo.Text;
                Usuario.Update();

                hdnIdUsuario.Value = "";
                OcultarABMUsuario();
                cargarGvUsuarios();
            }
        }

        protected void ddlRol_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdnIdRol.Value = ddlRol.SelectedValue.ToString();
        }
    }
}