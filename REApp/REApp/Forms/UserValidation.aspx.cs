using MagicSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using REApp.Models;

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
                }
            }
        }

        protected void btnVolverInicio_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Forms/UserLogin.aspx");
        }
    }
}