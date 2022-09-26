using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MagicSQL;
using REApp.Models;

namespace REApp.Forms
{
    public partial class ForoMensajes : System.Web.UI.Page
    {




        protected void Page_Load(object sender, EventArgs e)
        {

            //Se traen los idsolicitud y FormRedireccion obtenido en form anterior

            int idSolicitud = Request["idSolicitud"].ToInt();
            string formRedireccion = Request["formRedireccion"].ToString();


            ////Aca hacemos el get que si o si es un string porque de object a int no deja
            string idUsuario = Session["IdUsuario"].ToString();
            string idRol = Session["IdRol"].ToString();

            ////Estos se usan de esta forma porque son ints, ver si hay mejor forma de hacer el set
            int idRolInt = idRol.ToInt();
            int id = idUsuario.ToInt();

            

            if (IsPostBack)
            {
                cargarGvMensajes();
                if (idRolInt == 2)
                {
                    cargarGvMensajes();

                }

            }
            if (!IsPostBack)
            {
                //Si tiene rol Admin o Operador
                if (idRolInt == 1)
                {

                    cargarGvMensajes();
                }
                if (idRolInt == 2)
                {

                    cargarGvMensajes();

                }
                if (idRolInt == 3)
                {
                    cargarGvMensajes();
                }
            }

        }

        //Cargar Tabla de Mensajes


        protected void cargarGvMensajes()
        {

            int idSolicitud = Request["idSolicitud"].ToInt();

            DataTable dt = null;
            using (SP sp = new SP("bd_reapp"))
            {
                dt = sp.Execute("usp_GetMensajes", P.Add("IdSolicitud", idSolicitud));  //CAMBIAR ID SOLIDICUT
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                gvMensajes.DataSource = dt;
            }
            else
            {
                gvMensajes.DataSource = null;
            }
            gvMensajes.DataBind();

            cambiarColoresSegunRol();
        }

        //Cambiar colores de filas segun Rol


        private void cambiarColoresSegunRol()
        {
            foreach (GridViewRow row in gvMensajes.Rows)
            {

                string rol = row.Cells[3].Text;
                if(rol == "Operador")
                {
                    row.BackColor = System.Drawing.Color.FromArgb(225, 221, 221);
                }
                if(rol == "Explotador")
                {
                    row.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                }

            }
        }


        // Guardar Nuevo Mensaje

        protected void btnGuardar_Click(object sender, EventArgs e)
        {

            //SE TRAEN DATOS COMO IDSOLICITUD, ID USUARIO Y ROL
            int idSolicitud = Request["idSolicitud"].ToInt();

            string nuevoMensaje = txtNuevoMensaje.Text;

            ////Aca hacemos el get que si o si es un string porque de object a int no deja
            string idUsuario = Session["IdUsuario"].ToString();
            string idRol = Session["IdRol"].ToString();

            ////Estos se usan de esta forma porque son ints, ver si hay mejor forma de hacer el set
            int idRolInt = idRol.ToInt();
            int id = idUsuario.ToInt();


            //Validamos que tenga un mensaje cargado
            if (txtNuevoMensaje.Equals(""))
            {
                txtErrorHeader.Text = "Error";
                txtErrorBody.Text = "Por favor, escriba un nuevo mensaje.";
                pnlError.Visible = true;
            }
            else
            {
                //Se carga nuevo mensaje, por las dudas se esconde panel de error y se vuelve a cargar la tabla
                pnlError.Visible = false;

                Mensaje mensaje = new Mensaje();

                

                mensaje.Contenido = nuevoMensaje;
                mensaje.IdUsuarioEmisor = id;
                mensaje.IdUsuarioReceptor = 1; //VER BIEN QUE HACEMOS CON EL RECEPTOR!!!!! POR AHORA ESTA EN HARDCODEADOE EN 1!!!
                mensaje.IdSolicitud = idSolicitud; //TAMBIEN HAY QUE CAMBIAR PORQUE NO SE PUEDE XD
                mensaje.FHMensaje = DateTime.Now;
                mensaje.Insert();

                cargarGvMensajes();
            }

        }


        //Botón Volver a Formulacio Anterior

        protected void btnVolver_Click(object sender, EventArgs e)


        {
            //Se regresa a form anterior que fue pasado por parametro

            string formRedireccion = Request["formRedireccion"].ToString();
            Response.Redirect(formRedireccion);
        }


    }
}