using MagicSQL;
using REApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace REApp.Forms
{
    public partial class CoordinacionInteresado : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aca hacemos el get que si o si es un string porque de object a int no deja
            string idUsuario = Session["IdUsuario"].ToString();
            string idRol = Session["IdRol"].ToString();

            //Estos se usan de esta forma porque son ints, ver si hay mejor forma de hacer el set
            int idRolInt = idRol.ToInt();
            int id = idUsuario.ToInt();

            // Parámetro de Entrada (IdInteresadoSolicitud encriptado)

            int IdInteresadoSolicitud = 0;
            if (Request["S"] != null)
            {
                IdInteresadoSolicitud = Request["S"].ToIntID();
            }

            if (!IsPostBack && IdInteresadoSolicitud != 0)
            {
                InteresadoSolicitud InteresadoSolicitud = new InteresadoSolicitud().Select(IdInteresadoSolicitud);
                hdnIdInteresadoSolicitud.Value = IdInteresadoSolicitud.ToCryptoID();

                int IdInteresado = InteresadoSolicitud.IdInteresado;
                hdnIdInteresado.Value = IdInteresado.ToCryptoID();

                int IdSolicitud = InteresadoSolicitud.IdSolicitud;
                lblIdSolicitud.Text = IdSolicitud.ToString();

                GetDatosSolicitud();
            }

            ScriptManager.GetCurrent(Page).RegisterPostBackControl(btnGenerarKMZ);
        }

        protected void GetDatosSolicitud()
        {
            List<Coordinacion> Coordinacion = new Coordinacion().Select("FHCancelacion IS NULL AND IdInteresadoSolicitud = " + hdnIdInteresadoSolicitud.Value.ToIntID().ToString());

            if (Coordinacion.Count > 0)
            {
                // EXISTE AL MENOS UNA COORDINACIÓN NO CANCELADA PARA ESE INTERESADO EN ESA SOLICITUD
                MostrarDatosRespuesta(Coordinacion[0]);
            }
            else
            {
                int IdSolicitud = lblIdSolicitud.Text.ToInt();

                DataTable dt = new SP("bd_reapp").Execute("usp_GetSolicitudParaInteresado", P.Add("IdSolicitud", IdSolicitud));

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    txtSolicitante.Text = dr["Solicitante"].ToString();
                    txtNombreSolicitud.Text = dr["Nombre"].ToString();
                    txtActividad.Text = dr["Actividad"].ToString();
                    txtModalidad.Text = dr["Modalidad"].ToString();
                    txtFechaDesde.Text = dr["FHDesde"].ToString();
                    txtFechaHasta.Text = dr["FHHasta"].ToString();
                    txtEstado.Text = dr["Estado"].ToString();
                    txtFechaActualizacion.Text = dr["FHUltimaActualizacionEstado"].ToString();
                    txtObservaciones.Text = dr["Observaciones"].ToString();
                }
                else
                {
                    // MENSAJE DE ERROR... SIEMPRE DEBERÍA RECUPERARLO
                }
            }
        }

        protected void btnGenerarKMZ_Click(object sender, EventArgs e)
        {
            Documento KML = GetKML();

            DescargarKML(Encoding.ASCII.GetString(KML.Datos));
        }

        protected Documento GetKML()
        {
            Documento Documento;
            List<Documento> Documentos = new SP("bd_reapp").Execute("usp_GetKMLDeSolicitud",
                P.Add("IdSolicitud", lblIdSolicitud.Text.ToInt())
            ).ToList<Documento>();

            if (Documentos.Count > 0)
            {
                // SI EXISTE, SE RECUPERA DE BD
                Documento = Documentos[0];
            }
            else
            {
                // SI NO EXISTE, SE GENERA y REGISTRA EN BD
                KMLController KMLController = new KMLController(new Models.Solicitud().Select(lblIdSolicitud.Text.ToInt()));

                string kml = KMLController.GenerarKML();

                Documento = new Models.Documento()
                {
                    IdSolicitud = lblIdSolicitud.Text.ToInt(),
                    IdTipoDocumento = 5,
                    Extension = ".kml",
                    FHAlta = DateTime.Now,
                    TipoMIME = "text/plain",
                    Datos = Encoding.ASCII.GetBytes(kml),
                    Nombre = "Ubicaciones_Solicitud_N" + lblIdSolicitud.Text + ".kml"
                };
                Documento.Insert();
            }

            return Documento;
        }

        protected void DescargarKML(string kml)
        {
            Response.Clear();
            Response.ClearHeaders();
            Response.AppendHeader("Content-Length", kml.Length.ToString());
            Response.AppendHeader("Content-Disposition", "attachment;filename=\"Ubicaciones_Solicitud_N" + lblIdSolicitud.Text + ".kml\"");
            Response.ContentType = "text/plain";
            Response.Write(kml);
            Response.End();
        }

        protected void btnAprobar_Click(object sender, EventArgs e)
        {
            MostrarDatosRespuesta(CrearCoordinacion(true));
        }

        protected void btnRechazar_Click(object sender, EventArgs e)
        {
            if (txtRecomendaciones.Text.Trim().Equals(""))
            {
                pnlAlertRecomendaciones.Visible = true;
            }
            else
            {
                MostrarDatosRespuesta(CrearCoordinacion(false));
            }
        }

        protected Coordinacion CrearCoordinacion(bool Aprobada)
        {
            Coordinacion Coordinacion = new Coordinacion()
            {
                FHCoordinacion = DateTime.Now,
                IdInteresadoSolicitud = hdnIdInteresadoSolicitud.Value.ToIntID(),
                Recomendaciones = txtRecomendaciones.Text.Trim(),
                Aprobada = Aprobada
            };

            Coordinacion.Insert();

            return Coordinacion;
        }

        protected void MostrarDatosRespuesta(Coordinacion Coordinacion)
        {
            pnlDatosSolicitud.Visible = false;
            pnlDatosRespuesta.Visible = true;

            hdnIdCoordinacion.Value = Coordinacion.IdCoordinacion.ToCryptoID();
            lblSolicitudRespuesta.Text =
            txtRespuesta.Text = (Coordinacion.Aprobada) ? "Aprobada" : "Rechazada";
            txtFechaRespuesta.Text = Coordinacion.FHCoordinacion.ToString();
            txtRecomendacionesRespuesta.Text = Coordinacion.Recomendaciones;
        }

        protected void btnCancelarRespuesta_Click(object sender, EventArgs e)
        {
            Coordinacion Coordinacion = new Coordinacion().Select(hdnIdCoordinacion.Value.ToIntID());

            Coordinacion.FHCancelacion = DateTime.Now;
            Coordinacion.Update();

            GetDatosSolicitud();
            pnlDatosSolicitud.Visible = true;
            pnlDatosRespuesta.Visible = false;
        }
    }
}