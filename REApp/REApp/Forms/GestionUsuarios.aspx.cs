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
            //Aca hacemos el get que si o si es un string porque de object a int no deja
            string idUsuario = Session["IdUsuario"].ToString();
            string idRol = Session["IdRol"].ToString();

            //Estos se usan de esta forma porque son ints, ver si hay mejor forma de hacer el set
            int idRolInt = idRol.ToInt();
            int id = idUsuario.ToInt();


            if (IsPostBack)
            {
                
            }
            if (!IsPostBack)
            {
                if (idRolInt == 1)
                {
                    CargarComboRol();
                    BindGridAdmin();
                    btnNuevo.Visible = true;
                }
                if (idRolInt == 2)
                {
                    CargarComboRol();
                    BindGridOperador();
                    btnNuevo.Visible = false;
                }

            }

            hdnIdCurrentUser.Value = idUsuario; // <----------- Modificar con el ID de Usuario Logueado (Por ahora le asigno uno de la BD)
        }

        private void BindGridOperador()
        {
            DataTable dt = null;
            using (SP sp = new SP("bd_reapp"))
            {
                if (!ddlModalRol.SelectedItem.Value.Equals("#"))
                {
                    dt = sp.Execute("usp_GetUsuariosRolExplotador");
                }
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

        private void BindGridAdmin()
        {
            DataTable dt = null;
            using (SP sp = new SP("bd_reapp"))
            {
                if (!ddlModalRol.SelectedItem.Value.Equals("#"))
                {
                    dt = sp.Execute("usp_GetUsuariosRol");
                }
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

        protected void CargarComboRol()
        {
            ddlModalRol.Items.Clear();
            using (SP sp = new SP("bd_reapp"))
            {
                DataTable dt = new UsuarioController().GetComboRol();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlModalRol.Items.Add(new ListItem(dt.Rows[i]["Nombre"].ToString(), dt.Rows[i]["IdRol"].ToString().ToInt().ToCryptoID()));
                }
            }
        }


        protected void LimpiarModal()
        {
            txtModalNombreUsuario.Text =
            txtModalApellidoUsuario.Text =
            txtModalDni.Text =
            txtModalFechaNac.Text =
            txtModalCorreo.Text =
            txtModalTelefono.Text =
            txtModalCuit.Text =
            txtModalTipoDni.Text = "";
        }

        //protected void btnGuardar_Click(object sender, EventArgs e)
        //{
        //    Models.Usuario Usuario = null;
        //    if (hdnIdUsuario.Value.Equals(""))
        //    { // Insert
        //        using (Tn tn = new Tn("bd_reapp"))
        //        {
        //            Usuario = new Models.Usuario();
        //            Usuario.Nombre = txtModalNombreUsuario.Text;
        //            Usuario.Email = txtModalCorreo.Text;
        //            Usuario.IdRol = ddlModalRol.SelectedValue.ToIntID();
        //            Usuario.FechaNacimiento = txtModalFechaNac.ToString().ToDateTime();
        //            Usuario.Telefono = txtModalTelefono.Text;
        //            Usuario.Dni = txtModalDni.Text.ToInt();
        //            Usuario.Apellido = txtModalApellidoUsuario.Text;
        //            Usuario.CreatedOn = DateTime.Now;
        //            Usuario.CreatedBy = null; // -----------------------------> Modificar
        //            Usuario.Insert();
        //        }
        //    }
        //    else
        //    { // Update
        //        Usuario = new Models.Usuario().Select(hdnIdUsuario.Value.ToInt());
        //        Usuario.Nombre = txtModalNombreUsuario.Text;
        //        Usuario.Email = txtModalCorreo.Text;
        //        Usuario.IdRol = ddlModalRol.SelectedValue.ToIntID();
        //        Usuario.FechaNacimiento = txtModalFechaNac.ToString().ToDateTime();
        //        Usuario.Telefono = txtModalTelefono.Text;
        //        Usuario.Dni = txtModalDni.Text.ToInt();
        //        Usuario.Apellido = txtModalApellidoUsuario.Text;
        //        Usuario.Update();
        //    }

        //    MostrarListado();
        //    btnFiltrar_Click(null, null);
        //}

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarModal();
            MostrarABM();

        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            MostrarListado();
            hdnIdUsuario.Value = "";
        }

        protected void MostrarListado()
        {
            pnlListado.Visible = true;
            btnNuevo.Visible = true;
            pnlABM.Visible = false;
            btnVolver.Visible = false;
            pnlAlertDeleteUser.Visible = false;
            pnlError.Visible = false;

            if (Session["IdRol"].ToString().ToInt() == 2)
            {//Buscar otra forma de hacer
                btnNuevo.Visible = false;
            }
        }

        protected void MostrarABM()
        {
            pnlListado.Visible = false;
            btnNuevo.Visible = false;
            pnlABM.Visible = true;
            btnVolver.Visible = true;
            btnGuardar.Visible = true;
        }
        protected void MostrarMsgEliminar()
        {
            pnlListado.Visible = false;
            btnNuevo.Visible = false;
            pnlABM.Visible = false;
            btnVolver.Visible = false;
            btnGuardar.Visible = false;
            pnlAlertDeleteUser.Visible = true;
        }


        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            if (Session["IdRol"].ToString().ToInt() == 1)
            {
                CargarComboRol();
                BindGridAdmin();
                btnNuevo.Visible = true;
            }
            if (Session["IdRol"].ToString().ToInt() == 2)
            {
                CargarComboRol();
                BindGridOperador();
                btnNuevo.Visible = false;
            }
        }

        protected void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int IdUsuario = e.CommandArgument.ToString().ToInt();
            Models.Usuario Usuario = new Models.Usuario().Select(IdUsuario);

            if (e.CommandName.Equals("DisplayUser"))
            { // Detalle
                LimpiarModal();
                CargarComboRol();

                ddlModalRol.SelectedValue = ddlModalRol.SelectedValue;
                ddlModalRol.Enabled = false;

                hdnIdUsuario.Value = IdUsuario.ToString();
                ddlModalRol.Enabled = true;

                txtModalNombreUsuario.Text = Usuario.Nombre;
                txtModalApellidoUsuario.Text = Usuario.Apellido;
                ddlModalRol.SelectedValue = Usuario.IdRol.ToCryptoID().ToString();
                txtModalDni.Text = Usuario.Dni.ToString();
                txtModalTipoDni.Text = Usuario.TipoDni;
                txtModalCuit.Text = Usuario.Cuit;
                txtModalFechaNac.Text = Usuario.FechaNacimiento.ToString();
                txtModalCorreo.Text = Usuario.Email;
                txtModalTelefono.Text = Usuario.Telefono;

                MostrarABM();
                habilitarDeshabilitarInputs(false);


            }
            else if (e.CommandName.Equals("UpdateUser"))
            {
                LimpiarModal();
                CargarComboRol();

                MostrarABM();

                ddlModalRol.SelectedValue = ddlModalRol.SelectedValue;
                ddlModalRol.Enabled = false;

                hdnIdUsuario.Value = IdUsuario.ToString();
                ddlModalRol.Enabled = true;

                txtModalNombreUsuario.Text = Usuario.Nombre;
                txtModalApellidoUsuario.Text = Usuario.Apellido;
                ddlModalRol.SelectedValue = Usuario.IdRol.ToCryptoID().ToString();
                txtModalDni.Text = Usuario.Dni.ToString();
                txtModalTipoDni.Text = Usuario.TipoDni;
                txtModalCuit.Text = Usuario.Cuit;
                txtModalFechaNac.Text = Usuario.FechaNacimiento.ToString();
                txtModalCorreo.Text = Usuario.Email;
                txtModalTelefono.Text = Usuario.Telefono;

                habilitarDeshabilitarInputs(true);

            }
            if (e.CommandName.Equals("DeleteUser"))
            {
                MostrarMsgEliminar();
                lblDeleteMessage.Text = "¿Desea confirmar la eliminación del usuario " + Usuario.Nombre + " " + Usuario.Apellido + "?";
                hdnDeleteUserId.Value = Usuario.IdUsuario.ToString();
            }
        }

        private void habilitarDeshabilitarInputs(Boolean Bool)
        {
            ddlModalRol.Enabled = Bool;
            txtModalNombreUsuario.Enabled = Bool;
            txtModalApellidoUsuario.Enabled = Bool;
            txtModalDni.Enabled = Bool;
            txtModalTipoDni.Enabled = Bool;
            txtModalCuit.Enabled = Bool;
            txtModalFechaNac.Enabled = Bool;
            txtModalCorreo.Enabled = Bool;
            txtModalTelefono.Enabled = Bool;
            btnGuardar.Visible = Bool;
        }

        protected void btnGuardar_Click1(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                Models.Usuario UsuarioViejo = null;
                UsuarioViejo = new Models.Usuario().Select(hdnIdUsuario.Value.ToInt());

                // Update
                UsuarioViejo.Nombre = txtModalNombreUsuario.Text;
                UsuarioViejo.Apellido = txtModalApellidoUsuario.Text;
                UsuarioViejo.IdRol = ddlModalRol.SelectedValue.ToString().ToIntID();
                UsuarioViejo.Dni = txtModalDni.Text;
                UsuarioViejo.TipoDni = txtModalTipoDni.Text;
                UsuarioViejo.FechaNacimiento = txtModalFechaNac.Text.ToDateTime();
                UsuarioViejo.Telefono = txtModalTelefono.Text;
                UsuarioViejo.Email = txtModalCorreo.Text;
                UsuarioViejo.Cuit = txtModalCuit.Text;
                UsuarioViejo.Password = UsuarioViejo.Password;
                UsuarioViejo.SaltKey = UsuarioViejo.SaltKey;

                UsuarioViejo.Update();

                hdnIdUsuario.Value = "";
                MostrarListado();
                //BindGrid();
            }
           
        }

        protected bool ValidarCampos()
        {
            if (txtModalNombreUsuario.Text.Equals(""))
            {
                txtErrorHeader.Text = "Error";
                txtErrorBody.Text = "Por favor, ingrese el nombre del usuario.";
                pnlError.Visible = true;
                return false;
            }
            if (txtModalApellidoUsuario.Text.Equals(""))
            {
                txtErrorHeader.Text = "Error";
                txtErrorBody.Text = "Por favor, ingrese el apellido del usuario.";
                pnlError.Visible = true;
                return false;
            }
            if (txtModalDni.Text.Equals(""))
            {
                txtErrorHeader.Text = "Error";
                txtErrorBody.Text = "Por favor, ingrese el DNI del usuario.";
                pnlError.Visible = true;
                return false;
            }
            if (txtModalFechaNac.Text.Equals(""))
            {
                txtErrorHeader.Text = "Error";
                txtErrorBody.Text = "Por favor, ingrese la fecha de nacimiento del usuario.";
                pnlError.Visible = true;
                return false;
            }
            if (txtModalFechaNac.Text.ToDateTimeNull() == null)
            {
                txtErrorHeader.Text = "Error";
                txtErrorBody.Text = "Por favor, ingrese una fecha de nacimiento válida.";
                pnlError.Visible = true;
                return false;
            }
            if (txtModalTelefono.Text.Equals("") && txtModalCorreo.Text.Equals(""))
            {
                txtErrorHeader.Text = "Error";
                txtErrorBody.Text = "Por favor, ingrese al menos un dato de contacto del usuario.";
                pnlError.Visible = true;
                return false;
            }

            pnlError.Visible = false;
            return true;
        }

        protected void btnDeleteCancel_Click(object sender, EventArgs e)
        {
            MostrarListado();
        }

        protected void btnDeleteConfirm_Click(object sender, EventArgs e)
        {
            //Models.Usuario UsuarioAEliminar = new Models.Usuario().Select(hdnDeleteUserId.Value.ToInt());

            UsuarioController userController = new UsuarioController();
            userController.LogicDeleteUser(hdnDeleteUserId.Value.ToInt(), hdnIdCurrentUser.Value.ToInt());
            
            //UsuarioAEliminar.Delete();

            MostrarListado();
        }

        protected void gvUsuarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Session["idRol"].ToString() == "2")
                {
                    LinkButton lnkBtn = (LinkButton)e.Row.FindControl("btnUpdate");
                    lnkBtn.Visible = false;
                    LinkButton lnkBtn2 = (LinkButton)e.Row.FindControl("btnEliminarUsuario");
                    lnkBtn2.Visible = false;
                }
            }
        }
    }
}