using MagicSQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;

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

                if (idRolInt == 2)
                {
                    CargarComboSolicitante();
                    BindGrid();
                    MostrarListado();
                }

            }
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

        protected void GetInteresadosSoloVinculadosSolicitud1(int idSolicitud)
        {//OBTIENE SOLO LOS INTERESADOS VINCULADOS A UNA SOLICITUD
            using (SP sp = new SP("bd_reapp"))
            {
                DataTable dt = sp.Execute("usp_GetInteresadosSoloVinculadosSolicitud",
                    P.Add("IdSolicitud", idSolicitud));

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

        protected void GetInteresados()
        {//OBTIENE TODOS LOS INTERESADOS
            using (SP sp = new SP("bd_reapp"))
            {
                int IdUsuario = ddlSolicitante.SelectedValue.ToIntID();

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

        protected void GetInteresadosDeSolicitud(int IdSolicitud)
        {//OBTIENE LOS INTERESADOS VINCULADOS A UNA SOLICITUD Y LOS NO VINCULADOS
            using (SP sp = new SP("bd_reapp"))
            {
                int IdUsuario = ddlSolicitante.SelectedValue.ToIntID();

                DataTable dt = sp.Execute("usp_GetInteresadosVinculadosSolicitud",
                    P.Add("IdSolicitud", IdSolicitud)
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

        protected void GetTripulantesDeUsuario(int IdUsuario)
        {
            using (SP sp = new SP("bd_reapp"))
            {
                DataTable dt = sp.Execute("usp_GetTripulacionDeUsuario", P.Add("IdUsuario", IdUsuario));

                if (dt.Rows.Count > 0)
                {
                    gvTripulacion.DataSource = dt;
                }
                else
                {
                    gvTripulacion.DataSource = null;
                }
                gvTripulacion.DataBind();
            }
        }

        protected void GetTripulantesDeSolicitud(int IdSolicitud)
        {
            using (SP sp = new SP("bd_reapp"))
            {
                int IdUsuario = ddlSolicitante.SelectedValue.ToIntID();

                DataTable dt = sp.Execute("usp_GetTripulacionDeSolicitud",
                    P.Add("IdSolicitud", IdSolicitud),
                    P.Add("IdUsuario", IdUsuario)
                );

                if (dt.Rows.Count > 0)
                {
                    gvTripulacion.DataSource = dt;
                }
                else
                {
                    gvTripulacion.DataSource = null;
                }
                gvTripulacion.DataBind();
            }
        }

        private void BindGrid()
        {
            DataTable dt = null;
            using (SP sp = new SP("bd_reapp"))
            {
                if (!ddlSolicitante.SelectedItem.Value.Equals("#"))
                {
                    int s = ddlSolicitante.SelectedItem.Value.ToIntID();
                    dt = sp.Execute("usp_GetSolicitudesPorEstado",
                        P.Add("IdUsuario", ddlSolicitante.SelectedItem.Value.ToIntID()),
                        P.Add("IdEstadoSolicitud1", 2),
                        P.Add("IdEstadoSolicitud2", 9),
                        P.Add("IdEstadoSolicitud3", null),
                        P.Add("IdEstadoSolicitud4", null),
                        P.Add("IdEstadoSolicitud5", null),
                        P.Add("IdEstadoSolicitud6", null)
                        );
                }
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
        }

        protected void MostrarABM()
        {
            pnlInteresadosVinculados.Visible = false;
            pnlListado.Visible = false;
            pnlABM.Visible = true;
            btnVolver.Visible = true;
            pnlInteresados.Visible = false;
        }

        protected void MostrarInteresados()
        {
            pnlInteresadosVinculados.Visible = false;
            pnlInteresados.Visible = true;
            pnlListado.Visible = false;
            pnlABM.Visible = false;
            btnVolver.Visible = true;
        }

        protected void MostrarInteresadosVinculados()
        {
            pnlInteresadosVinculados.Visible = true;
            pnlInteresados.Visible = false;
            pnlListado.Visible = false;
            pnlABM.Visible = false;
            btnVolver.Visible = true;
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
            ddlModalSolicitante.SelectedValue = ddlSolicitante.SelectedValue;
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
            }
            //Para vinculacion de interesados
            if (e.CommandName.Equals("VincularInteresados"))
            {
                hdnIdSolicitudInteresado.Value = e.CommandArgument.ToString();
                GetInteresados();
                //GetInteresadosSoloVinculadosSolicitud1(hdnIdSolicitudInteresado.Value.ToInt());
                MostrarInteresados();
                
            }
            if (e.CommandName.Equals("PasarACoordinacion"))
            {
                hdnIdSolicitudInteresadosVinculados.Value = e.CommandArgument.ToString();
                GetInteresadosSoloVinculadosSolicitud(hdnIdSolicitudInteresadosVinculados.Value.ToInt());
                MostrarInteresadosVinculados();
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

            DataTable dt = null;
            if (!chkVant.Checked)
            {
                //La Solicitud ya está creada
                //Por ende, recupero los Vants de la Solicitud + los del Usuario
                dt = new SP("bd_reapp").Execute("usp_GetVantsDeSolicitud",
                    P.Add("IdSolicitud", hdnIdSolicitud.Value.ToInt()),
                    P.Add("IdUsuario", ddlSolicitante.SelectedValue.ToIntID())
                );

                if (dt.Rows.Count > 0)
                {
                    gvVANTs.DataSource = dt;
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
            KMLController KMLController = new KMLController(new Models.Solicitud().Select(hdnIdSolicitud.Value.ToInt()));

            string kml = KMLController.GenerarKML();
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
                }
            }
            MostrarListado();
            btnFiltrar_Click(null, null);
        }

        protected void btnPasarACoordinacion_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gvSoloInteresadosVinculados.Rows.Count; i++)
            {

                if (((CheckBox)gvSoloInteresadosVinculados.Rows[i].FindControl("chkInteresadoVinculado")).Checked)
                { // SI ESTÁ CHEQUEADO
                  //Logica Mails
                    string email = ((HiddenField)gvSoloInteresadosVinculados.Rows[i].FindControl("hdnEmail")).Value.ToString();
                    string nombre= ((HiddenField)gvSoloInteresadosVinculados.Rows[i].FindControl("hdnNombre")).Value.ToString();
                    int idSoli = hdnIdSolicitudInteresadosVinculados.Value.ToInt();
                    int idInteresado = ((HiddenField)gvSoloInteresadosVinculados.Rows[i].FindControl("hdnIdInteresadoVinculado")).Value.ToInt();
                    PasarSolicitudACoordinacion(idSoli);
                    EnviarMail(nombre, email, idInteresado, idSoli);                   
                }
            }
            MostrarListado();

        }

        protected void EnviarMail(string nombre, string email, int idInteresado, int idSolicitud)
        {

            string url = "https://localhost:44355/Forms/HomeDash/HomeDash/Forms/SolicitudesCoodinacion.aspx?idSolicitud=" + idSolicitud + "&idInteresado=" + idInteresado;

            string cuerpo = "Por favor conteste la recomendacion de REA: " + url;

            MailController mail = new MailController("RECOMENDACION REA", cuerpo, false);
            
            mail.Add(nombre, email);

            bool Exito = mail.Enviar();
        }


        protected void PasarSolicitudACoordinacion(int idSolicitud)
        {
            new SP("bd_reapp").Execute("usp_ActualizarEstadoSolicitud",
                P.Add("IdSolicitud", idSolicitud),
                P.Add("IdEstadoSolicitud", 3),
                P.Add("IdUsuarioCambioEstado", Session["IdUsuario"].ToString().ToInt())
            );
        }
    }
}