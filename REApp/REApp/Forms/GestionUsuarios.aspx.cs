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


        protected void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            //int IdUsuario = e.CommandArgument.ToString().ToInt();
            Models.Usuario Usuario = new Models.Usuario().Select(3);

            if (e.CommandName.Equals("DisplayUser"))
            {
                MostrarUsuario(Usuario);
            }
            else if (e.CommandName.Equals("UpdateUser"))
            {
                ModificarUsuario(Usuario);
            }
            else
            {
                EliminarUsuario(Usuario);
            }

            OcultarGv();
            
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
            //hdnIdTripulacion.Value = IdUsuario.ToString();
            //txtModalApellido.Text = Usuario.Apellido;
            //txtModalNombre.Text = Tripulacion.Nombre;
            //txtModalDNI.Text = Tripulacion.DNI;
            //txtModalFechaNacimiento.Text = Tripulacion.FechaNacimiento.ToString();
            //txtModalTelefono.Text = Tripulacion.Telefono;
            //txtModalCorreo.Text = Tripulacion.Correo;

            //MostrarABM();
        }

        //Visualización
        protected void MostrarAbmUsuarios()
        {

        }

        protected void OcultarGv()
        {
            pnlListado.Visible = false;
        }

    }
}