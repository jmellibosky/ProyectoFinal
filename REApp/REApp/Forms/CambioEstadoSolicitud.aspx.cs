using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MagicSQL;
using REApp.Models;
using REApp.Controllers;

namespace REApp.Forms
{
    public partial class CambioEstadoSolicitud : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int IdEstado = Request["E"].ToInt(); // -1 para el estado anterior; sino IdEstadoSolicitud
            int IdSolicitud = Request["S"].ToInt();

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

                if (IdEstado == 3)
                {
                    //Se carga la grilla de interesados, para mandar mail a c/u
                    GetInteresadosSoloVinculadosSolicitud(IdSolicitud);
                    //Si es explotador, no se le muestra la grilla de interesados
                    if (idRolInt ==3)
                    {
                        pnlGvInteresado.Visible = false;
                    }
                    else
                    {
                        pnlGvInteresado.Visible = true;
                    }  
                }
                else
                {
                    pnlGvInteresado.Visible = false;
                }
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

                if (IdEstado == 9)
                { // Si pasa a PendienteModificacion, entonces se envía mail
                    Usuario u = new Usuario().Select(new Solicitud().Select(IdSolicitud).IdUsuario);

                    HTMLBuilder builder = new HTMLBuilder("Solicitud de Reserva de Espacio Aéreo", "GenericMailTemplate.html");

                    builder.AppendTexto("Buenas tardes.");
                    builder.AppendSaltoLinea(2);
                    builder.AppendTexto("La Empresa Argentina de Navegación Aérea solicita se realicen cambios en su reciente Solicitud de Reserva de Espacio Aéreo. Un Operador especializado ha realizado la siguiente observación respecto de su Solicitud:");
                    builder.AppendSaltoLinea(2);
                    builder.AppendTexto(txtObservaciones.Text);
                    builder.AppendSaltoLinea(2);
                    builder.AppendTexto("Por favor, ingrese al sistema REAPP para realizar los cambios solicitados.");

                    string cuerpo = builder.ConstruirHTML();

                    MailController mail = new MailController("OBSERVACIONES REA", cuerpo);
                    mail.Add(u.Nombre + u.Apellido, u.Email);

                    bool Exito = mail.Enviar();
                }
                //Si pasa al estado enCoordinacion
                if (IdEstado == 3)
                {
                    //Se envia mail los interesados, cargados en la grilla q se muestra solo al operador.
                    for (int i = 0; i < gvSoloInteresadosVinculados.Rows.Count; i++)
                    {
                        if (((CheckBox)gvSoloInteresadosVinculados.Rows[i].FindControl("chkInteresadoVinculado")).Checked)
                        { // SI ESTÁ CHEQUEADO
                          //Logica Mails
                            string email = ((HiddenField)gvSoloInteresadosVinculados.Rows[i].FindControl("hdnEmail")).Value.ToString();
                            string nombre = ((HiddenField)gvSoloInteresadosVinculados.Rows[i].FindControl("hdnNombre")).Value.ToString();
                            int idInteresado = ((HiddenField)gvSoloInteresadosVinculados.Rows[i].FindControl("hdnIdInteresadoVinculado")).Value.ToInt();
                            EnviarMailCoordinacion(nombre, email, idInteresado, IdSolicitud);
                        }
                    }
                }
            }
            Response.Redirect(frm);
        }

        //Mismo mail que se envia en Analisis
        protected void EnviarMailCoordinacion(string nombre, string email, int idInteresado, int idSolicitud)
        {
            List<Models.InteresadoSolicitud> InteresadoSolicitud = new Models.InteresadoSolicitud().Select($"IdInteresado = {idInteresado} AND IdSolicitud = {idSolicitud}");

            int IdInteresadoSolicitud = (InteresadoSolicitud.Count > 0) ? InteresadoSolicitud[0].IdInteresadoSolicitud : 0;

            string leftpart = Request.Url.GetLeftPart(UriPartial.Authority);
            string frmValidacion = "/Forms/CoordinacionInteresado.aspx";
            string parameters = $"?S={IdInteresadoSolicitud.ToCryptoID()}";

            string url = $"{leftpart}{frmValidacion}{parameters}";

            Controllers.HTMLBuilder builder = new Controllers.HTMLBuilder("Solicitud de Reserva de Espacio Aéreo", "GenericMailTemplate.html");

            builder.AppendTexto("Buenas tardes.");
            builder.AppendSaltoLinea(2);
            builder.AppendTexto("La Empresa Argentina de Navegación Aérea solicita sus recomendaciones para la coordinación de esta solicitud de Reserva de Espacio Aéreo.");
            builder.AppendSaltoLinea(1);
            builder.AppendTexto("Por favor, ingrese en el siguiente enlace para ver los detalles de esta solicitud y brindar sus recomendaciones.");
            builder.AppendSaltoLinea(2);
            builder.AppendURL(url, "Solicitud de Reserva de Espacio Aéreo"); ;

            string cuerpo = builder.ConstruirHTML();

            MailController mail = new MailController("RECOMENDACION REA", cuerpo);

            mail.Add(nombre, email);

            bool Exito = mail.Enviar();
        }

        protected void GetInteresadosSoloVinculadosSolicitud(int idSolicitud)
        {//OBTIENE SOLO LOS INTERESADOS VINCULADOS A UNA SOLICITUD
            using (SP sp = new SP("bd_reapp"))
            {
                DataTable dt = sp.Execute("usp_GetInteresadosSoloVinculadosSolicitud",
                    P.Add("IdSolicitud", idSolicitud));

                if (dt.Rows.Count > 0)
                {
                    gvSoloInteresadosVinculados.DataSource = dt;
                }
                else
                {
                    gvSoloInteresadosVinculados.DataSource = null;
                }
                gvSoloInteresadosVinculados.DataBind();
            }
        }
    }
}