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
                MostrarUsuario(Usuario);
            }
            else if (e.CommandName.Equals("UpdateUser"))
            {
                MostrarModificacionUsuario(Usuario);
            }
            else
            {
                EliminarUsuario(Usuario);
            }
            
        }

        //Alta Usuario

        protected void btnNuevoUsuario()
        {

        }

        protected void btnAgregarUsuario()
        {

        }

        //Eliminar Usuario
        protected void EliminarUsuario(Models.Usuario Usuario)
        {
            using (SP sp = new SP("bd_reapp"))
            {
                sp.Execute("usp_DeleteUser",
                    P.Add("IdUsuario", Usuario.IdUsuario)
                );
            }
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

        //Mostrar Usuario
        protected void MostrarUsuario(Models.Usuario Usuario)
        {
            string NombreRol = GetRolUsuario(Usuario);

            txtNombre.Text = Usuario.Nombre;
            txtApellido.Text = Usuario.Apellido;
            txtRol.Text = NombreRol;
            txtDNI.Text = Usuario.Dni.ToString();
            txtTipoDni.Text = Usuario.TipoDni;
            txtFechaNacimiento.Text = Usuario.FechaNacimiento.ToString();
            txtCorreo.Text = Usuario.Email;
            txtTelefono.Text = Usuario.Telefono;

            MostrarABMUsuario();
        }

        //Visualización
        protected void MostrarABMUsuario()
        {
            pnlABM.Visible = true;
            btnGuardar.Visible = false;
            btnNuevo.Visible = false;
            btnVolver.Visible = true;
            pnlGvUsuarios.Visible = false;

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
        }

        protected void MostrarModificacionUsuario(Models.Usuario Usuario)
        {
            //string NombreRol = GetRolUsuario(Usuario);

            //txtNombre.Text = Usuario.Nombre;
            //txtApellido.Text = Usuario.Apellido;
            //txtRol.Text = NombreRol;
            //txtDNI.Text = Usuario.Dni.ToString();
            //txtTipoDni.Text = Usuario.TipoDni;
            //txtFechaNacimiento.Text = Usuario.FechaNacimiento.ToString();
            //txtCorreo.Text = Usuario.Email;
            //txtTelefono.Text = Usuario.Telefono;

            //MostrarModificacionUsuario();
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
    }
}