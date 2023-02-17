using MagicSQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static REApp.Navegacion;

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
            //validacion user
            btnValidar.Visible = false;
        }

        protected void MostrarABM()
        {
            pnlListado.Visible = false;
            btnNuevo.Visible = false;
            pnlABM.Visible = true;
            btnVolver.Visible = true;
            btnGuardar.Visible = true;
            //validacion user
            btnValidar.Visible = true;

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
                Alert("Usuario actualizado con éxito", "Se ha actualizado el usuario seleccionado.", AlertType.success, "/Forms/GestionUsuarios.aspx");
                //MostrarListado();
                //BindGrid();
            }
           
        }

        protected bool ValidarCampos()
        {
            if (txtModalNombreUsuario.Text.Equals(""))
            {
                Alert("Error", "Por favor, ingrese el nombre del usuario.", AlertType.error);
                return false;
            }
            if (txtModalApellidoUsuario.Text.Equals(""))
            {
                Alert("Error", "Por favor, ingrese el apellido del usuario.", AlertType.error);
                return false;
            }
            if (txtModalDni.Text.Equals(""))
            {
                Alert("Error", "Por favor, ingrese el DNI del usuario.", AlertType.error);
                return false;
            }
            string numberPattern = @"^\d+$";


            if (!Regex.IsMatch(txtModalDni.Text, numberPattern))
            {
                Alert("Error", "Por favor, ingrese una número de DNI válido.", AlertType.error);
                return false;
            }

            if(txtModalDni.Text.Length != 8)
            {
                Alert("Error", "Por favor, ingrese una número de DNI válido.", AlertType.error);
                return false;
            }



            if (txtModalFechaNac.Text.Equals(""))
            {
                Alert("Error", "Por favor, ingrese el fecha de nacimiento del usuario.", AlertType.error);
                return false;
            }

            //Se valida la expresión regular de la fecha de nacimiento
            //string datePattern = @"^(19|20)\d\d[-](0[1-9]|1[012])[-](0[1-9]|[12][0-9]|3[01])$";
            //if ((!Regex.IsMatch(txtModalFechaNac.Text, datePattern)) || (txtModalFechaNac.Text.ToDateTimeNull() == null))
            //{
            //    Alert("Error", "Por favor, ingrese una Fecha de Nacimiento válida.", AlertType.error);
            //    return false;
            //}
            //Para que la fecha no supere a la actual
            //DateTime fechaActual = DateTime.Now.Date;
            //string fechaNac = txtModalFechaNac.Text.Replace("-", "/");
            //DateTime fecha = DateTime.ParseExact(fechaNac, "yyyy/MM/dd", CultureInfo.InvariantCulture);
            //if (fecha.Date > fechaActual)
            //{
            //    Alert("Error", "Por favor, ingrese una Fecha de Nacimiento menor a la Fecha Actual.", AlertType.error);
            //    return false;
            //}

            if (txtModalTelefono.Text.Equals("") && txtModalCorreo.Text.Equals(""))
            {
                Alert("Error", "Por favor, ingrese al menos un dato de contacto del usuario.", AlertType.error);
                return false;
            }

            if(txtModalCuit.Text.Equals(""))
            {
                Alert("Error", "Por favor, ingrese el número de CUIT del usuario.", AlertType.error);
                return false;
            }

            string regexCUIT = @"^\d{2}\-\d{8}\-\d{1}$";
            if (!Regex.IsMatch(txtModalCuit.Text, regexCUIT) && txtModalCuit.Text.Length != 11)
            {
                Alert("Error", "Por favor, ingrese un número de CUIT válido.", AlertType.error);
                return false;
            }

            if(Regex.IsMatch(txtModalCuit.Text, regexCUIT))
            {
                txtModalCuit.Text = txtModalCuit.Text.Replace("-", "");
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

            Alert("Usuario dado de baja con éxito", "Se dado de baja el usuario seleccionado.", AlertType.success, "/Forms/GestionUsuarios.aspx");
            //MostrarListado();
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
                //if (Session["idRol"].ToString() == "1")
                //{
                //    if (e.Row.RowType == DataControlRowType.Header)
                //    {
                //        e.Row.Cells[0].Visible = true;
                //    }
                //    if (e.Row.RowType == DataControlRowType.DataRow)
                //    {
                //        e.Row.Cells[0].Visible = true;
                //    }
                //}
            }
        }

        protected void btnValidar_Click(object sender, EventArgs e)
        {
            //Models.Usuario UsuarioViejo = null;
            Models.Usuario UsuarioViejo = new Models.Usuario().Select(hdnIdUsuario.Value.ToInt());

            // Update
            UsuarioViejo.ValidacionEANA = true;
            UsuarioViejo.Update();

            hdnIdUsuario.Value = "";
            Alert("Usuario validado con éxito", "Se ha validado el usuario seleccionado.", AlertType.success, "/Forms/GestionUsuarios.aspx");

        }
    }
}