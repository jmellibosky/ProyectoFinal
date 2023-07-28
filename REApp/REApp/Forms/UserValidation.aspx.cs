using MagicSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using REApp.Models;
using REApp.Controllers;

namespace REApp.Forms
{
    public partial class UserValidation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int IdUsuario = 0;
            if (Request["U"] != null)
            {
                try
                {
                    IdUsuario = Request["U"].ToIntID();
                }
                catch (Exception)
                {
                    IdUsuario = 0;
                }
            }

            if (!IsPostBack)
            {
                if (IdUsuario == 0)
                { // ERROR
                    pnlTituloExito.Visible =
                    pnlMensajeExito.Visible = false;
                    pnlTituloError.Visible =
                    pnlMensajeError.Visible = true;
                }
                else
                { // ÉXITO
                    pnlTituloExito.Visible =
                    pnlMensajeExito.Visible = true;
                    pnlTituloError.Visible =
                    pnlMensajeError.Visible = false;

                    Usuario usuario = new Usuario().Select(IdUsuario);
                    usuario.ValidacionCorreo = true;
                    usuario.Update();

                    NotificarAdmins();
                }
            }
        }

        protected void NotificarAdmins()
        {
            try
            {
                List<Usuario> Usuarios = new Usuario().Select("IdRol = 1 AND DeletedOn IS NULL");

                HTMLBuilder builder = new HTMLBuilder("Nuevo Usuario Registrado", "GenericMailTemplate.html");

                builder.AppendTexto($"Buenos días.");
                builder.AppendSaltoLinea(2);
                builder.AppendTexto("Se ha registrado un nuevo usuario en REAPP. Recuerde validarlo para permitirle continuar su gestión.");
                builder.AppendSaltoLinea(1);
                builder.AppendTexto("Saludos.");
                builder.AppendSaltoLinea(1);
                builder.AppendTexto("Equipo de REApp.");
                string cuerpo = builder.ConstruirHTML();

                MailController mail = new MailController("Confirmación de Usuario", cuerpo);

                foreach (Usuario usuario in Usuarios)
                {
                    mail.Add($"{usuario.Nombre} {usuario.Apellido}", usuario.Email);
                }

                mail.Enviar();
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnVolverInicio_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Forms/UserLogin.aspx");
        }
    }
}