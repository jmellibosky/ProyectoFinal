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
    public partial class CambioEstadoSolicitud : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int IdEstado = Request["E"].ToInt(); // -1 para el estado anterior; sino IdEstadoSolicitud

            //Aca hacemos el get que si o si es un string porque de object a int no deja
            string idUsuario = Session["IdUsuario"].ToString();
            string idRol = Session["IdRol"].ToString();

            //Estos se usan de esta forma porque son ints, ver si hay mejor forma de hacer el set
            int idRolInt = idRol.ToInt();
            int id = idUsuario.ToInt();

            if (!IsPostBack)
            {
                EstadoSolicitud es = new EstadoSolicitud().Select(IdEstado);

                lblEstado.Text = $"La Solicitud pasará a estado {es.Nombre}.";
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            string frm = Request["frm"].ToString();
            Response.Redirect(frm);
        }

        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            int IdSolicitud = Request["S"].ToInt();
            int IdEstado = Request["E"].ToInt();
            string frm = Request["frm"].ToString();

            if (IdEstado == -1 || IdEstado == 9)
            { // Si es un Estado "Negativo" (regreso al estado anterior o envío a pendiente de modificaciones)
                // Entonces obligamos que se ingrese una observación
                if (txtObservaciones.Text.Trim().Equals(""))
                {
                    pnlAlertObservaciones.Visible = true;
                    return;
                }
            }

            if (IdEstado == -1)
            { // VOLVER AL ESTADO ANTERIOR
                new SP("bd_reapp").Execute("usp_DevolverEstadoAnterior",
                    P.Add("IdSolicitud", IdSolicitud),
                    P.Add("IdUsuarioCambioEstado", Session["IdUsuario"].ToString().ToInt()),
                    P.Add("Observacion", txtObservaciones.Text)
                );
            }
            else
            { // TRASLADAR A ESTADO
                new SP("bd_reapp").Execute("usp_ActualizarEstadoSolicitud",
                    P.Add("IdSolicitud", IdSolicitud),
                    P.Add("IdEstadoSolicitud", IdEstado),
                    P.Add("IdUsuarioCambioEstado", Session["IdUsuario"].ToString().ToInt()),
                    P.Add("Observacion", txtObservaciones.Text)
                );
            }

            Response.Redirect(frm);
        }
    }
}