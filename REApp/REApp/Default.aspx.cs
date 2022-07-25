using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace REApp
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblUsername.InnerText = Session["Username"].ToString();
            }
            catch
            {
                lblUsername.InnerText = "INGRESO POR EL DEFAULT";
            }
            
        }
    }
}