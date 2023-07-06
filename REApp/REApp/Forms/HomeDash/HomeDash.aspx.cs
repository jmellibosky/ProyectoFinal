using System;

namespace REApp.Forms.HomeDash
{
    public partial class HomeDash : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblUsername.InnerText = Session["Username"].ToString();

                //Aca hacemos el get que si o si es un string porque de object a int no deja
                string idUsuario = Session["IdUsuario"].ToString();
                string idRol = Session["IdRol"].ToString();

                //Estos se usan de esta forma porque son ints, ver si hay mejor forma de hacer el set
                int idRolInt = idRol.ToInt();
                int id = idUsuario.ToInt();

                if (idRolInt == 3) //Solicitante
                {
                    nombreRol.InnerText = "Solicitante";
                    sidebarOperador.Style["display"] = "none";
                    sidebarAdmin.Style["display"] = "none";
                }
                else if (idRolInt == 1) //Administrador
                {
                    nombreRol.InnerText = "Administrador";
                    sidebarOperador.Style["display"] = "none";
                    sidebarSolicitante.Style["display"] = "none";
                }
                else if (idRolInt == 2) //Operador
                {
                    nombreRol.InnerText = "Operador";
                    sidebarSolicitante.Style["display"] = "none";
                    sidebarAdmin.Style["display"] = "none";
                }

            }
            catch
            {
                lblUsername.InnerText = "TESTING";
            }
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            lblUsername.InnerText = "TESTEAD";
            Response.Redirect("/Forms/UserLogin.aspx");
        }

        protected void lblUsername_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("/Forms/GestionUsuarios.aspx");
        }
    }
}