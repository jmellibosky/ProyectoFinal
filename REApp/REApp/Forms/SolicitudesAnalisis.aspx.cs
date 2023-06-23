﻿using MagicSQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using static REApp.Navegacion;

namespace REApp.Forms
{
    public partial class SolicitudesAnalisis : System.Web.UI.Page
    {
        public List<UbicacionRedux> Ubicaciones
        {
            get
            {
                if (ViewState["Ubicaciones"] == null)
                {
                    return new List<UbicacionRedux>();
                }
                else
                {
                    return ViewState["Ubicaciones"].ToString().ToList<UbicacionRedux>();
                }
            }
            set
            {
                if (value == null)
                {
                    ViewState["Ubicaciones"] = null;
                }
                else
                {
                    ViewState["Ubicaciones"] = value.ToJson();
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Aca hacemos el get que si o si es un string porque de object a int no deja
            string idUsuario = Session["IdUsuario"].ToString();
            string idRol = Session["IdRol"].ToString();

            //Estos se usan de esta forma porque son ints, ver si hay mejor forma de hacer el set
            int idRolInt = idRol.ToInt();
            int id = idUsuario.ToInt();

            if (IsPostBack)
            {
                BindGrid();
                //LbArchivo.Text = "";
            }
            if (!IsPostBack)
            {

                if (idRolInt == 2 || idRolInt == 1)
                {
                    CargarComboSolicitante();
                    BindGrid();
                    MostrarListado();
                }

            }
            ScriptManager.GetCurrent(Page).RegisterPostBackControl(btnGenerarKMZ);
        }

        protected void GetInteresados()
        {//OBTIENE TODOS LOS INTERESADOS
            using (SP sp = new SP("bd_reapp"))
            {
                DataTable dt = sp.Execute("usp_GetInteresados"
                );

                if (dt.Rows.Count > 0)
                {
                    gvInteresados.DataSource = dt;
                }
                else
                {
                    gvInteresados.DataSource = null;
                }
                gvInteresados.DataBind();
            }
        }


        protected void GetTripulantesDeSolicitud(int IdSolicitud)
        {
            using (SP sp = new SP("bd_reapp"))
            {
                int IdUsuario = ddlModalSolicitante.SelectedValue.ToIntID();
                DataTable dataTableFiltradaTrip = new DataTable();

                DataTable dt = sp.Execute("usp_GetTripulacionDeSolicitud",
                    P.Add("IdSolicitud", IdSolicitud),
                    P.Add("IdUsuario", IdUsuario)
                );

                if (dt.Rows.Count > 0)
                {
                    dataTableFiltradaTrip = dt.AsEnumerable().Where(row => row.Field<int>("Checked") == 1).CopyToDataTable();
                    gvTripulacion.DataSource = dataTableFiltradaTrip;
                }
                else
                {
                    gvTripulacion.DataSource = null;
                }
                gvTripulacion.DataBind();

                //Borrar si no cambiamos el SP de la base
                //if (dt.Rows.Count > 0)
                //{
                //    var dataTableFiltrada = dt.AsEnumerable().Where(row => row.Field<int>("Checked") == 1).CopyToDataTable();
                //    gvTripulacion.DataSource = dt;
                //}
                //else
                //{
                //    gvTripulacion.DataSource = null;
                //}
                //gvTripulacion.DataBind();
            }
        }

        private void BindGrid()
        {
            DataTable dt = null;
            using (SP sp = new SP("bd_reapp"))
            {
                List<KeyValuePair<string, object>> parameters = new List<KeyValuePair<string, object>>();
                parameters.Add(P.Add("IdEstadoSolicitud1", 2));
                parameters.Add(P.Add("IdEstadoSolicitud2", 9));
                if (!ddlSolicitante.SelectedItem.Value.Equals("#"))
                {
                    parameters.Add(P.Add("IdUsuario", ddlSolicitante.SelectedItem.Value.ToIntID()));
                }
                if (ddlVerBajas.SelectedItem.Value.Equals("0"))
                {
                    parameters.Add(P.Add("VerBajas", "0"));
                }
                else
                {
                    parameters.Add(P.Add("VerBajas", "1"));
                }
                dt = sp.Execute("usp_GetSolicitudesPorEstado", parameters.ToArray());
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                gvSolicitud.DataSource = dt;
            }
            else
            {
                gvSolicitud.DataSource = null;
            }

            gvSolicitud.DataBind();
        }

        protected void CargarComboSolicitante()
        {
            ddlSolicitante.Items.Clear();
            ddlSolicitante.Items.Add(new ListItem("Todos", "#"));
            using (SP sp = new SP("bd_reapp"))
            {
                DataTable dt = new UsuarioController().GetComboSolicitante();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlSolicitante.Items.Add(new ListItem(dt.Rows[i]["Nombre"].ToString(), dt.Rows[i]["IdUsuario"].ToString().ToInt().ToCryptoID()));
                }
            }
        }

        protected void CargarComboModalSolicitante()
        {
            ddlModalSolicitante.Items.Clear();
            using (SP sp = new SP("bd_reapp"))
            {
                DataTable dt = new UsuarioController().GetComboSolicitante();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlModalSolicitante.Items.Add(new ListItem(dt.Rows[i]["Nombre"].ToString(), dt.Rows[i]["IdUsuario"].ToString().ToInt().ToCryptoID()));
                }
            }
        }

        protected void CargarComboModalActividades()
        {
            ddlModalActividad.Items.Clear();
            using (SP sp = new SP("bd_reapp"))
            {
                DataTable dt = new UsuarioController().GetComboActividades();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlModalActividad.Items.Add(new ListItem(dt.Rows[i]["Nombre"].ToString(), dt.Rows[i]["IdActividad"].ToString().ToInt().ToCryptoID()));
                }
            }
        }

        protected void CargarComboModalModalidades(int idActividad)
        {
            ddlModalModalidad.Items.Clear();
            using (SP sp = new SP("bd_reapp"))
            {
                DataTable dt = new UsuarioController().GetComboModalidades(idActividad);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlModalModalidad.Items.Add(new ListItem(dt.Rows[i]["Nombre"].ToString(), dt.Rows[i]["IdModalidad"].ToString().ToInt().ToCryptoID()));
                }
            }
        }
        protected void CargarComboModalSoloModalidades()
        {
            ddlModalModalidad.Items.Clear();

            using (SP sp = new SP("bd_reapp"))
            {
                DataTable dt = sp.Execute("usp_GetComboModalidades");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlModalModalidad.Items.Add(new ListItem(dt.Rows[i]["Nombre"].ToString(), dt.Rows[i]["IdModalidad"].ToString().ToInt().ToCryptoID()));
                }
            }
        }


        protected void LimpiarModal()
        {
            ddlModalSolicitante.Items.Clear();
            ddlModalModalidad.Items.Clear();
            ddlModalActividad.Items.Clear();
            txtModalNombreSolicitud.Text =
            txtModalEstadoSolicitud.Text =
            txtModalFechaDesde.Text =
            txtModalFechaHasta.Text =
            txtModalObservaciones.Text = "";
            txtModalFechaUltimaActualizacion.Text = "-";
            txtModalFechaUltimaActualizacion.Enabled = false;
            txtModalFechaSolicitud.Enabled = false;
            txtModalEstadoSolicitud.Enabled = false;
            rptUbicaciones.DataSource = null;
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            MostrarListado();
            hdnIdSolicitud.Value = "";
            hdnIdSolicitudInteresado.Value = "";
            hdnIdSolicitudInteresadosVinculados.Value = "";
        }

        protected void MostrarListado()
        {
            pnlInteresadosVinculados.Visible = false;
            pnlListado.Visible = true;
            pnlABM.Visible = false;
            btnVolver.Visible = false;
            btnGenerarKMZ.Visible = false;
            pnlInteresados.Visible = false;
            btnFiltrar_Click(null, null);
        }

        protected void MostrarABM()
        {
            pnlInteresadosVinculados.Visible = false;
            pnlListado.Visible = false;
            pnlABM.Visible = true;
            btnVolver.Visible = true;
            pnlInteresados.Visible = false;
            btnGenerarKMZ.Visible = true;
        }

        protected void MostrarInteresados()
        {
            pnlInteresadosVinculados.Visible = false;
            pnlInteresados.Visible = true;
            pnlListado.Visible = false;
            pnlABM.Visible = false;
            btnVolver.Visible = true;
            btnGenerarKMZ.Visible = false;
        }

        //True p/visible, False p/ invisible
        protected void OcultarMostrarPanelesABM(bool valor)
        {
            pnlModalEstadoSolicitud.Visible = valor;
            pnlModalFechaSolicitud.Visible = valor;
            pnlModalFechaUltimaActualizacion.Visible = valor;
            ddlModalSolicitante.Visible = valor; //Aca cambie el !valor
        }

        //True para Habilitar, False p/ Deshabilitar
        protected void HabilitarDeshabilitarTxts(bool valor)
        {
            //Ocultar en detalle y Mostrar en nuevo
            ddlModalActividad.Enabled = valor;
            ddlModalModalidad.Enabled = valor;
            txtModalNombreSolicitud.Enabled = valor;
            txtModalObservaciones.Enabled = valor;
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void gvSolicitud_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Detalle"))
            { // Detalle
                int IdSolicitud = e.CommandArgument.ToString().ToInt();
                Models.Solicitud Solicitud = new Models.Solicitud().Select(IdSolicitud);

                int IdUsuario = Solicitud.IdUsuario;
                Models.Usuario Usuario = new Models.Usuario().Select(IdUsuario);

                int IdEstado = (int)Solicitud.IdEstadoSolicitud;
                Models.EstadoSolicitud Estado = new Models.EstadoSolicitud().Select(IdEstado);

                int IdModalidad = Solicitud.IdModalidad;
                Models.Modalidad Modalidad = new Models.Modalidad().Select(IdModalidad);

                LimpiarModal();
                OcultarMostrarPanelesABM(true);

                CargarComboModalSolicitante();
                ddlModalSolicitante.SelectedValue = IdUsuario.ToCryptoID();
                ddlModalSolicitante.Enabled = false;

                CargarComboModalActividades();
                CargarComboModalSoloModalidades();

                ddlModalActividad.SelectedValue = Modalidad.IdActividad.ToCryptoID().ToString();
                ddlModalModalidad.SelectedValue = Solicitud.IdModalidad.ToCryptoID().ToString();

                hdnIdSolicitud.Value = IdSolicitud.ToString();
                txtModalNombreSolicitud.Text = Solicitud.Nombre;

                txtModalObservaciones.Text = Solicitud.Observaciones;
                txtModalEstadoSolicitud.Text = Estado.Nombre;

                txtModalFechaDesde.Text = Solicitud.FHDesde.ToString();
                txtModalFechaHasta.Text = Solicitud.FHHasta.ToString();
                txtModalFechaHasta.Enabled = false;
                txtModalFechaDesde.Enabled = false;

                DateTime FHActualiz = (DateTime)Solicitud.FHUltimaActualizacionEstado;
                if (FHActualiz != null)
                {
                    txtModalFechaUltimaActualizacion.Text = FHActualiz.ToString();
                }
                txtModalFechaSolicitud.Text = Solicitud.FHAlta.ToString();

                chkVant.Checked = Solicitud.IdAeronave.HasValue;
                chkVant_CheckedChanged(null, null);

                GetTripulantesDeSolicitud(IdSolicitud);
                GetUbicacionesDeSolicitud(IdSolicitud);

                MostrarABM();


                btnGenerarKMZ.Visible = true;
                HabilitarDeshabilitarTxts(false);

                VerHistorialSolicitud();
            }
            else if (e.CommandName.Equals("Eliminar"))
            {
                int IdSolicitud = e.CommandArgument.ToString().ToInt();

                Models.Solicitud Solicitud = new Models.Solicitud().Select(IdSolicitud);

                if (Solicitud != null)
                {
                    Solicitud.FHBaja = DateTime.Now;
                    Solicitud.Update();

                    Alert("Éxito", "La Solicitud ha sido eliminada.", AlertType.success);
                    btnFiltrar_Click(null, null);
                }
            }
        }

        protected void GetUbicacionesDeSolicitud(int IdSolicitud)
        {
            DataTable dt = new SP("bd_reapp").Execute("usp_GetPuntosGeograficosDeSolicitud", P.Add("IdSolicitud", IdSolicitud));

            if (dt.Rows.Count > 0)
            {
                rptUbicaciones.DataSource = dt;
                rptUbicaciones.DataBind();

                for (int i = 0; i < rptUbicaciones.Items.Count; i++)
                {
                    ((Label)rptUbicaciones.Items[i].FindControl("lblRptTipoUbicacion")).Text = dt.Rows[i]["TipoUbicacion"].ToString();
                    ((Label)rptUbicaciones.Items[i].FindControl("lblRptDatos")).Text = dt.Rows[i]["Datos"].ToString();
                }
            }
        }

        protected void ddlModalActividad_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idActividad = ddlModalActividad.SelectedItem.Value.ToIntID();
            CargarComboModalModalidades(idActividad);
        }


        protected void chkVant_CheckedChanged(object sender, EventArgs e)
        {
            pnlSeleccionVants.Visible = !chkVant.Checked;

            DataTable dataTableFiltradaVants = new DataTable();

            DataTable dt = null;
            if (!chkVant.Checked)
            {
                //La Solicitud ya está creada
                //Por ende, recupero los Vants de la Solicitud + los del Usuario
                dt = new SP("bd_reapp").Execute("usp_GetVantsDeSolicitud",
                    P.Add("IdSolicitud", hdnIdSolicitud.Value.ToInt()),
                    P.Add("IdUsuario", ddlModalSolicitante.SelectedValue.ToIntID())
                );

                //Para que muestre solo los Vants activos utilizamos la tabla auxiliar, si cambiamos el SP tambien tenemos que cambiar aca.

                if (dt.Rows.Count > 0)
                {
                    dataTableFiltradaVants = dt.AsEnumerable().Where(row => row.Field<int>("Checked") == 1).CopyToDataTable();
                    gvVANTs.DataSource = dataTableFiltradaVants;
                }
                else
                {
                    gvVANTs.DataSource = null;
                }
                gvVANTs.DataBind();
            }
        }

        protected void btnGenerarKMZ_Click(object sender, EventArgs e)
        {
            Models.Documento KML = GetKML();

            DescargarKML(Encoding.ASCII.GetString(KML.Datos));
        }

        protected Models.Documento GetKML()
        {
            Models.Documento Documento;
            List<Models.Documento> Documentos = new SP("bd_reapp").Execute("usp_GetKMLDeSolicitud",
                P.Add("IdSolicitud", hdnIdSolicitud.Value.ToInt())
            ).ToList<Models.Documento>();

            if (Documentos.Count > 0)
            {
                // SI EXISTE, SE RECUPERA DE BD
                Documento = Documentos[0];
            }
            else
            {
                // SI NO EXISTE, SE GENERA y REGISTRA EN BD
                KMLController KMLController = new KMLController(new Models.Solicitud().Select(hdnIdSolicitud.Value.ToInt()));

                string kml = KMLController.GenerarKML();

                Documento = new Models.Documento()
                {
                    IdSolicitud = hdnIdSolicitud.Value.ToInt(),
                    IdTipoDocumento = 5,
                    Extension = ".kml",
                    FHAlta = DateTime.Now,
                    TipoMIME = "text/plain",
                    Datos = Encoding.ASCII.GetBytes(kml),
                    Nombre = "Ubicaciones_Solicitud_N" + hdnIdSolicitud.Value + ".kml"
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
            Response.AppendHeader("Content-Disposition", "attachment;filename=\"Ubicaciones_Solicitud_N" + hdnIdSolicitud.Value + ".kml\"");
            Response.ContentType = "text/plain";
            Response.Write(kml);
            Response.End();
        }

        protected void ddlSolicitante_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnFiltrar_Click(null, null);
        }

        protected void btnGuardarInteresados_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gvInteresados.Rows.Count; i++)
            {

                if (((CheckBox)gvInteresados.Rows[i].FindControl("chkInteresadoVinculado")).Checked)
                { // SI ESTÁ CHEQUEADO
                  // CREO OBJETO INTERESADO SOLICITUD
                    Models.InteresadoSolicitud InteresadoSolicitud = new Models.InteresadoSolicitud();

                    // SETEO LOS CAMPOS DEL OBJETO
                    InteresadoSolicitud.FHVinculacion = DateTime.Now;
                    InteresadoSolicitud.IdSolicitud = hdnIdSolicitudInteresado.Value.ToInt();
                    InteresadoSolicitud.IdInteresado = ((HiddenField)gvInteresados.Rows[i].FindControl("hdnIdInteresado")).Value.ToInt();

                    // INSERT EN TABLA INTERESADOSOLICITUD
                    InteresadoSolicitud.Insert();
                    Alert("Se actualizaron los interesados de la solicitud", "Los mismos seran notificados por mail al pasar la solicitud al siguiente estado.", AlertType.success, "/Forms/SolicitudesAnalisis.aspx");
                }
            }
            MostrarABM();
            btnFiltrar_Click(null, null);
        }


        protected void btnPasarACoordinacion_Click(object sender, EventArgs e)
        {
        }

        protected void PasarSolicitudACoordinacion(int idSolicitud)
        {
            // CONFIRMACIÓN CON MENSAJE OPCIONAL
            int IdSolicitud = idSolicitud;
            int IdEstado = 3;
            string FrmAnterior = "/Forms/SolicitudesAnalisis.aspx";

            string url = $"/Forms/CambioEstadoSolicitud.aspx?S={IdSolicitud}&E={IdEstado}&frm={FrmAnterior}";

            Response.Redirect(url);
        }

        protected void btnHabilitarModificacion_Click(object sender, EventArgs e)
        {
            // CONFIRMACIÓN CON MENSAJE OPCIONAL
            int IdSolicitud = hdnIdSolicitud.Value.ToInt();
            int IdEstado = 9;
            string FrmAnterior = "/Forms/SolicitudesCoordinacion.aspx";

            string url = $"/Forms/CambioEstadoSolicitud.aspx?S={IdSolicitud}&E={IdEstado}&frm={FrmAnterior}";

            Response.Redirect(url);
        }

        protected void btnDevolver_Click(object sender, EventArgs e)
        {
            // CONFIRMACIÓN CON MENSAJE OPCIONAL
            int IdSolicitud = hdnIdSolicitud.Value.ToInt();
            int IdEstado = -1;
            string FrmAnterior = "/Forms/SolicitudesCoordinacion.aspx";

            string url = $"/Forms/CambioEstadoSolicitud.aspx?S={IdSolicitud}&E={IdEstado}&frm={FrmAnterior}";

            Response.Redirect(url);
        }

        protected void VerHistorialSolicitud()
        {
            // ACCESO A DATOS
            DataTable dt = new SP("bd_reapp").Execute("usp_GetHistorialEstadoDeSolicitud",
                P.Add("IdSolicitud", hdnIdSolicitud.Value)
            );

            if (dt.Rows.Count > 0)
            {
                gvHistorial.DataSource = dt;
            }
            else
            {
                gvHistorial.DataSource = null;
            }
            gvHistorial.DataBind();


        }

        protected void btnVerForo_Click(object sender, EventArgs e)
        {
            //Se obtiene id de solicitud
            int id = int.Parse((sender as LinkButton).CommandArgument);
            //Se crea nuevo form ForoMensajes
            ForoMensajes foroMensajes = new ForoMensajes();

            //Creamos String con la direccion de este form para despues el boton volver nos regrese a este form
            string formRedireccion = "/Forms/SolicitudesAnalisis/SolicitudesAnalisis.aspx";

            //Se redirecciona a ForoMensajes pasando por parametro (?parametro=valor) el idSolicitud de la tabla y la direccion de este form
            Response.Redirect("/Forms/ForoMensajes/ForoMensajes.aspx?idSolicitud=" + id + "&formRedireccion=" + formRedireccion);

        }

        protected void btnVincularInteresados_Click(object sender, EventArgs e)
        {
            hdnIdSolicitudInteresado.Value = hdnIdSolicitud.Value;
            GetInteresados();
            MostrarInteresados();
        }

        protected void btnPasarCoordinacion_Click(object sender, EventArgs e)
        {
            PasarSolicitudACoordinacion(hdnIdSolicitud.Value.ToInt());
        }

        protected void ddlVerBajas_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnFiltrar_Click(null, null);
        }
    }
}