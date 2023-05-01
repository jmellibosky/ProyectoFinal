using MagicSQL;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using static REApp.Navegacion;
using static REApp.UsuarioController;

namespace REApp.Forms
{
    public partial class PefilUsuario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                cargarDatosUsuario();
                CargarComboRol();
            }

        }

        protected void cargarDatosUsuario()
        {
            string IdUsuario = Session["IdUsuario"].ToString();
            Models.Usuario Usuario = new Models.Usuario().Select(IdUsuario.ToInt());

            hdnIdUsuario.Value = IdUsuario.ToString();

            txtModalNombreUsuario.Text = Usuario.Nombre;
            txtModalApellidoUsuario.Text = Usuario.Apellido;
            ddlModalRol.SelectedValue = Usuario.IdRol.ToCryptoID().ToString();
            txtModalDni.Text = Usuario.Dni.ToString();
            txtModalTipoDni.Text = Usuario.TipoDni;
            txtModalCuit.Text = Usuario.Cuit;
            txtModalFechaNac.TextMode = TextBoxMode.SingleLine;
            txtModalFechaNac.Text = Usuario.FechaNacimiento.ToString();
            txtModalCorreo.Text = Usuario.Email;
            txtModalTelefono.Text = Usuario.Telefono;
        }

        protected void btnGuardar_Click1(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                if (hdnIdUsuario.Value != "")
                {
                    Models.Usuario UsuarioViejo = null;
                    UsuarioViejo = new Models.Usuario().Select(hdnIdUsuario.Value.ToInt());

                    // Update
                    UsuarioViejo.Nombre = txtModalNombreUsuario.Text;
                    UsuarioViejo.Apellido = txtModalApellidoUsuario.Text;
                    UsuarioViejo.IdRol = Session["IdRol"].ToString().ToInt();
                    UsuarioViejo.Dni = txtModalDni.Text;
                    UsuarioViejo.TipoDni = txtModalTipoDni.Text;
                    UsuarioViejo.FechaNacimiento = txtModalFechaNac.Text.ToDateTime();
                    UsuarioViejo.Telefono = txtModalTelefono.Text;
                    UsuarioViejo.Email = txtModalCorreo.Text;
                    UsuarioViejo.Cuit = txtModalCuit.Text;

                    
                    string validacionPasswordActual = generarHash(UsuarioViejo.SaltKey, txtModalPasswordActual.Text);
                    //Se modifica la contrseña si tiene algo dentro del textbox
                    if ((txtModalPassword.Text != ""))
                    {
                        //Si las contraseñas nueva que escribio y su confirmacion son correctas
                        if ((txtModalPassword.Text == txtModalConfirmarPassword.Text))
                        {
                            //Si la contraseña actual ingresada es correcta
                            if (validacionPasswordActual == UsuarioViejo.Password)
                            {
                                string salt = SecurityHelper.GenerateSalt(70);
                                string password = txtModalPassword.Text;
                                UsuarioViejo.Password = generarHash(salt, password);
                                UsuarioViejo.SaltKey = salt;
                                Alert("Contraseña modificada", "Se cambió con éxito su contraseña.", AlertType.success);
                            }
                            Alert("Error", "La contraseña actual no es correcta", AlertType.error);
                        }
                        else
                        {
                            Alert("Error", "Las contraseñas no coinciden.", AlertType.error);
                        }
                    }
                    UsuarioViejo.Update();

                    hdnIdUsuario.Value = "";
                    Alert("Usuario actualizado con éxito", "Se ha actualizado su usuario.", AlertType.success, "/Forms/PerfilUsuario.aspx");
                }
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

            if (txtModalDni.Text.Length != 8)
            {
                Alert("Error", "Por favor, ingrese una número de DNI válido.", AlertType.error);
                return false;
            }

            if (txtModalTelefono.Text.Equals("") && txtModalCorreo.Text.Equals(""))
            {
                Alert("Error", "Por favor, ingrese al menos un dato de contacto del usuario.", AlertType.error);
                return false;
            }

            //Se saca CUIT xq no es obligatorio tener
            //if (txtModalCuit.Text.Equals(""))
            //{
            //    Alert("Error", "Por favor, ingrese el número de CUIT del usuario.", AlertType.error);
            //    return false;
            //}

            //string regexCUIT = @"^\d{2}\-\d{8}\-\d{1}$";
            //if (!Regex.IsMatch(txtModalCuit.Text, regexCUIT) && txtModalCuit.Text.Length != 11)
            //{
            //    Alert("Error", "Por favor, ingrese un número de CUIT válido.", AlertType.error);
            //    return false;
            //}

            //if (Regex.IsMatch(txtModalCuit.Text, regexCUIT))
            //{
            //    txtModalCuit.Text = txtModalCuit.Text.Replace("-", "");
            //}

            pnlError.Visible = false;
            return true;
        }

        protected string generarHash(string salt, string password)
        {
            string hashedpass = SecurityHelper.HashPassword(password, salt, 10101, 70);
            return hashedpass;
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

    }
}