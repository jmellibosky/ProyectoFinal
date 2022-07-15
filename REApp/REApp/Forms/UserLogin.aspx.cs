using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace REApp.Forms
{
    public partial class UserLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txt_email.Value == "testing@hotmail.com" && txt_password.Value == "1234")
            {
                Response.Redirect("/Default.aspx");
            }
            else
            {
                return;
            }
        }

    }
}