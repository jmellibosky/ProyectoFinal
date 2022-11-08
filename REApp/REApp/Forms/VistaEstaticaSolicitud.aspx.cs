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
    public partial class VistaEstaticaSolicitud : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int IdSolicitud = 0;
            if (Request["S"] != null)
            {
                IdSolicitud = Request["S"].ToIntID();
            }

            if (!IsPostBack && IdSolicitud != 0)
            {
                InteresadoSolicitud InteresadoSolicitud = new InteresadoSolicitud().Select(IdSolicitud);
                hdnIdInteresadoSolicitud.Value = IdSolicitud.ToCryptoID();
                lblIdSolicitud.Text = IdSolicitud.ToString();

                GetDatosSolicitud();
            }

            ScriptManager.GetCurrent(Page).RegisterPostBackControl(btnGenerarKMZ);
        }

        protected void GetDatosSolicitud()
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
            }

            DataTable dt2 = new SP("bd_reapp").Execute("usp_GetTripulacionDeSolicitudParaVistaEstatica", P.Add("IdSolicitud", IdSolicitud));

            if (dt2.Rows.Count > 0)
            {
                dtlTripulacion.DataSource = dt2;
                dtlTripulacion.DataBind();
            }

            DataTable dt3 = new SP("bd_reapp").Execute("usp_GetVantsDeSolicitudParaVistaEstatica", P.Add("IdSolicitud", IdSolicitud));

            if (dt3.Rows.Count > 0)
            {
                dtlVants.DataSource = dt3;
                dtlVants.DataBind();
            }

            DataTable dt4 = new SP("bd_reapp").Execute("usp_GetUbicacionesParaPDF", P.Add("IdSolicitud", IdSolicitud));

            if (dt4.Rows.Count > 0)
            {
                dtlUbicaciones.DataSource = dt4;
                dtlUbicaciones.DataBind();
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
    }
}