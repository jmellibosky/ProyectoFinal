using System;
using System.Web.UI;

namespace REApp
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (Session["Username"] == null)
                {
                    Response.Redirect("/Forms/UserLogin.aspx");
                }
            }
            catch (Exception)
            {
                Response.Redirect("/Forms/UserLogin.aspx");
            }
        }
    }
}