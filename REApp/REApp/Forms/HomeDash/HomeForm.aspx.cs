using System;

namespace REApp.Forms.HomeDash
{
    public partial class HomeForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string nombre = Session["Nombre"].ToString();
            //string apellido = Session["Apellido"].ToString();

            string user = Session["Username"].ToString();

            lblUsername.InnerText = user;
        }
    }
}