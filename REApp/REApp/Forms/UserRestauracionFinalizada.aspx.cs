using System;

namespace REApp.Forms
{
    public partial class UserRestauracionFinalizada : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnVolverInicio_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Forms/UserLogin.aspx");
        }

    }
}