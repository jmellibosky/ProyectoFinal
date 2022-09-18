using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
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
                if (Session["Username"] != null)
                {
                    lblUsername.InnerText = Session["Username"].ToString();
                }
                else
                {
                    Response.Redirect("/Forms/UserLogin.aspx");
                }

                //Aca hacemos el get que si o si es un string porque de object a int no deja
                string idUsuario = Session["IdUsuario"].ToString();
                string idRol = Session["IdRol"].ToString();

                //Estos se usan de esta forma porque son ints, ver si hay mejor forma de hacer el set
                int idRolInt = idRol.ToInt();
                int id = idUsuario.ToInt();
             
                if (idRolInt == 3) //Solicitante
                {
                    liAdministrador.Style["display"] = "none";
                    liOperador.Style["display"] = "none";
                }
                else if (idRolInt == 1) //Administrador
                {
                    liSolicitante.Style["display"] = "none";
                    liOperador.Style["display"] = "none";
                }
                else if (idRolInt == 2) //Operador
                {
                    liSolicitante.Style["display"] = "none";
                    liAdministrador.Style["display"] = "none";
                }

            }
            catch
            {
                lblUsername.InnerText = "INGRESO POR EL DEFAULT";
            }
            
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("/Forms/UserLogin.aspx");
        }
    }

}