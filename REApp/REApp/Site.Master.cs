using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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