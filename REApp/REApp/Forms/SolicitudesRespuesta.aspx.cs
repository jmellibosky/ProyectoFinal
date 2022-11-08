using MagicSQL;
using REApp.Controllers;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using static REApp.Navegacion;

namespace REApp.Forms
{
    public partial class SolicitudesRespuesta : System.Web.UI.Page
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
                //Rol Admin o Operador
                if (idRolInt == 1)
                {
                    CargarComboSolicitante();
                    BindGrid();
                }
                if (idRolInt == 2)
                {
                    CargarComboSolicitante();
                    BindGrid();
                    btnEstadoOperador.Visible = true;
                }
                //Rol Solicitante
                if (idRolInt == 3)
                {
                    CargarComboSolicitante();
                    ddlSolicitante.SelectedValue = id.ToCryptoID().ToString();
                    ddlSolicitante.Enabled = false;
                    BindGrid();

                    GetTripulantesDeUsuario(id);
                }
            }
            ScriptManager.GetCurrent(Page).RegisterPostBackControl(btnGenerarKMZ);
            ScriptManager.GetCurrent(Page).RegisterPostBackControl(btnRespuestaPDF);
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
                int IdUsuario = ddlModalSolicitante.SelectedValue.ToIntID();

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
                List<KeyValuePair<string, object>> parameters = new List<KeyValuePair<string, object>>();
                parameters.Add(P.Add("IdEstadoSolicitud1", 5));
                parameters.Add(P.Add("IdEstadoSolicitud2", 6));
                parameters.Add(P.Add("IdEstadoSolicitud3", 7));
                parameters.Add(P.Add("IdEstadoSolicitud4", 8));
                parameters.Add(P.Add("IdEstadoSolicitud5", 12));
                parameters.Add(P.Add("IdEstadoSolicitud6", 10));
                if (!ddlSolicitante.SelectedItem.Value.Equals("#"))
                {
                    parameters.Add(P.Add("IdUsuario", ddlSolicitante.SelectedItem.Value.ToIntID()));
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


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Models.Solicitud Solicitud = null;
            if (hdnIdSolicitud.Value.Equals(""))
            { // Insert
                using (Tn tn = new Tn("bd_reapp"))
                {
                    try
                    {
                        // CREO OBJETO SOLICITUD
                        Solicitud = new Models.Solicitud();

                        // SETEO LOS CAMPOS DEL OBJETO
                        Solicitud.Nombre = txtModalNombreSolicitud.Text;
                        Solicitud.IdModalidad = ddlModalModalidad.SelectedValue.ToIntID();
                        Solicitud.IdUsuario = ddlModalSolicitante.SelectedValue.ToIntID();
                        Solicitud.FHAlta = DateTime.Now;
                        Solicitud.FHDesde = txtModalFechaDesde.Text.ToDateTime();
                        Solicitud.FHHasta = txtModalFechaHasta.Text.ToDateTime();
                        Solicitud.IdEstadoSolicitud = 1;
                        Solicitud.Observaciones = txtModalObservaciones.Text;

                        // INSERT EN TABLA SOLICITUD
                        Solicitud.Insert(tn);

                        // RECORRO LA GRILLA DE VANTS
                        for (int i = 0; i < gvVANTs.Rows.Count; i++)
                        {
                            if (((CheckBox)gvVANTs.Rows[i].FindControl("chkVANTVinculado")).Checked)
                            { // SI ESTÁ CHEQUEADO
                              // CREO OBJETO VANTSOLICITUD
                                Models.VantSolicitud VantSolicitud = new Models.VantSolicitud();

                                // SETEO LOS CAMPOS DEL OBJETO
                                VantSolicitud.IdVant = ((HiddenField)gvVANTs.Rows[i].FindControl("hdnIdVant")).Value.ToInt();
                                VantSolicitud.IdSolicitud = Solicitud.IdSolicitud;

                                // INSERT EN TABLA VANTSOLICITUD
                                VantSolicitud.Insert(tn);
                            }
                        }

                        // RECORRO LAS UBICACIONES DEL VIEWSTATE
                        List<UbicacionRedux> AuxUbicaciones = Ubicaciones;
                        for (int i = 0; i < AuxUbicaciones.Count; i++)
                        {
                            // CREO OBJETO UBICACION
                            Models.Ubicacion Ubicacion = new Models.Ubicacion();

                            // SETEO LOS CAMPOS DEL OBJETO
                            Ubicacion.IdSolicitud = Solicitud.IdSolicitud;
                            Ubicacion.Altura = AuxUbicaciones[i].Altura;
                            Ubicacion.IdProvincia = 1; //------------------HARDCODEADO-NI IDEA DE POR QUÉ ESTÁ ESTE CAMPO ACÁ

                            // INSERT EN TABLA UBICACION
                            Ubicacion.Insert(tn);

                            // RECORRO LOS PUNTOS GEOGRÁFICOS DE LA UBICACIÓN
                            for (int j = 0; j < AuxUbicaciones[i].PuntosGeograficos.Count; j++)
                            {
                                // CREO OBJETO PUNTOGEOGRAFICO
                                Models.PuntoGeografico PuntoGeografico = AuxUbicaciones[i].PuntosGeograficos[j];

                                // SETEO LOS CAMPOS EL OBJETO
                                PuntoGeografico.IdUbicacion = Ubicacion.IdUbicacion;

                                // INSERT EN TABLA PUNTOGEOGRAFICO
                                PuntoGeografico.Insert(tn);
                            }
                        }

                        // RECORRO LA GRILLA DE TRIPULANTES
                        for (int i = 0; i < gvTripulacion.Rows.Count; i++)
                        {
                            if (((CheckBox)gvTripulacion.Rows[i].FindControl("chkTripulacionVinculado")).Checked)
                            { // SI ESTÁ CHEQUEADO
                              // CREO OBJETO VANTSOLICITUD
                                Models.TripulacionSolicitud TripulacionSolicitud = new Models.TripulacionSolicitud();

                                // SETEO LOS CAMPOS DEL OBJETO
                                TripulacionSolicitud.FHVinculacion = DateTime.Now;
                                TripulacionSolicitud.IdSolicitud = Solicitud.IdSolicitud;
                                TripulacionSolicitud.IdTripulacion = ((HiddenField)gvTripulacion.Rows[i].FindControl("hdnIdTripulacion")).Value.ToInt();

                                // INSERT EN TABLA VANTSOLICITUD
                                TripulacionSolicitud.Insert(tn);
                            }
                        }

                        tn.Commit();
                    }
                    catch (Exception)
                    {
                        tn.RollBack();
                    }
                }
            }
            else
            { // Update
                using (Tn tn = new Tn("bd_reapp"))
                {
                    Solicitud = new Models.Solicitud().Select(hdnIdSolicitud.Value.ToInt());
                    Solicitud.Nombre = txtModalNombreSolicitud.Text;
                    Solicitud.IdModalidad = ddlModalModalidad.SelectedValue.ToIntID();
                    Solicitud.IdEstadoSolicitud = 1;
                    Solicitud.FHDesde = txtModalFechaDesde.Text.ToDateTime();
                    Solicitud.FHHasta = txtModalFechaHasta.Text.ToDateTime();
                    Solicitud.Observaciones = txtModalObservaciones.Text;
                    Solicitud.FHUltimaActualizacionEstado = DateTime.Now;
                    Solicitud.Update(tn);
                }
            }

            MostrarListado();
        }



        protected void btnVolver_Click(object sender, EventArgs e)
        {
            MostrarListado();
            hdnIdSolicitud.Value = "";
        }

        protected void MostrarListado()
        {
            pnlListado.Visible = true;
            pnlABM.Visible = false;
            btnVolver.Visible = false;
            btnGenerarKMZ.Visible = false;
            btnRespuestaPDF.Visible = false;
            btnEnviarMail.Visible = false;
            btnFiltrar_Click(null, null);
        }

        protected void MostrarABM()
        {
            pnlListado.Visible = false;
            pnlABM.Visible = true;
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

            if (e.CommandName.Equals("Detalle"))
            { // Detalle
                btnGenerarKMZ.Visible = true;
                btnRespuestaPDF.Visible = true;
                btnEnviarMail.Visible = true;
                HabilitarDeshabilitarTxts(false);
                GetInteresadosSoloVinculadosSolicitud(hdnIdSolicitud.Value.ToInt());
                pnlInteresadosVinculados.Visible = true;
            }

            VerHistorialSolicitud();
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
                    P.Add("IdUsuario", ddlModalSolicitante.SelectedValue.ToIntID())
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
            Models.Documento KML = GetKML();

            DescargarKML(Encoding.ASCII.GetString(KML.Datos));
        }

        protected void ddlSolicitante_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnFiltrar_Click(null, null);
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


        protected void EnviarMail(string nombre, string email, int idSolicitud) //List<String> listaEmails
        {

            HTMLBuilder builder = new HTMLBuilder("Solicitud de Reserva de Espacio Aéreo", "GenericMailTemplate.html");

            builder.AppendTexto("Buenas tardes.");
            builder.AppendSaltoLinea(2);
            builder.AppendTexto("La Empresa Argentina de Navegación Aérea les comunica que la REA " + idSolicitud.ToString() + " fue aprobada por REA-CBA. Se adjunta la información relevante para el caso.");

            string cuerpo = builder.ConstruirHTML();

            MailController mail = new MailController("RESPUESTA REA", cuerpo);


            mail.Add(nombre, email);

            DataTable dtDocumento = new SP("bd_reapp").Execute("usp_GetRespuestaSolicitud",
                P.Add("IdSolicitud", hdnIdSolicitud.Value.ToInt())); ;

            int idDoc = (int)dtDocumento.Rows[0][0];

            mail.Add(new Models.Documento().Select(idDoc));


            bool Exito = mail.Enviar();
        }

        protected void btnEstadoOperador_Click(object sender, EventArgs e)
        {
            int idSolicitud = hdnIdSolicitud.Value.ToInt();
            //Envio de mail a cada tripulante
            //for (int i = 0; i < gvTripulacion.Rows.Count; i++)
            //{
            //    if (((CheckBox)gvTripulacion.Rows[i].FindControl("chkTripulacionVinculado")).Checked)
            //    { // SI ESTÁ CHEQUEADO
            //      //Logica Mails
            //        string nombre = ((HiddenField)gvTripulacion.Rows[i].FindControl("hdnNombre")).Value.ToString();
            //        string apellido = ((HiddenField)gvTripulacion.Rows[i].FindControl("hdnApellido")).Value.ToString();
            //        string nombreapellido = nombre + " " + apellido;
            //        int idTripulacion = ((HiddenField)gvTripulacion.Rows[i].FindControl("hdnIdTripulacion")).Value.ToInt();
            //        Models.Tripulacion Tripulacion = new Models.Tripulacion().Select(idTripulacion);
            //        string emailTripulante = Tripulacion.Correo.ToString();
            //        EnviarMail(nombreapellido, emailTripulante, idTripulacion, idSolicitud);
            //    }
            //}
            //for (int i = 0; i < gvSoloInteresadosVinculados.Rows.Count; i++)
            //{
            //    if (((CheckBox)gvSoloInteresadosVinculados.Rows[i].FindControl("chkInteresadoVinculado")).Checked)
            //    { // SI ESTÁ CHEQUEADO
            //      //Logica Mails
            //        string email = ((HiddenField)gvSoloInteresadosVinculados.Rows[i].FindControl("hdnEmail")).Value.ToString();
            //        string nombre = ((HiddenField)gvSoloInteresadosVinculados.Rows[i].FindControl("hdnNombre")).Value.ToString();
            //        int idInteresado = ((HiddenField)gvSoloInteresadosVinculados.Rows[i].FindControl("hdnIdInteresadoVinculado")).Value.ToInt();
            //        EnviarMail(nombre, email, idInteresado, idSolicitud);
            //    }
            //}

            //Models.Solicitud Solicitud = new Models.Solicitud().Select(hdnIdSolicitud.Value.ToInt());
            //int idSolicitante = Solicitud.IdUsuario;
            //Models.Usuario Usuario = new Models.Usuario().Select(idSolicitante);
            //string emailSolicitante = Usuario.Email.ToString();
            //string nombreSolicitante = Usuario.Nombre + " " + Usuario.Apellido;
            //EnviarMail(nombreSolicitante, emailSolicitante, idSolicitante, idSolicitud);

            MostrarListado();

            //VER DE USAR FORM DE CAMBIO DE ESTADO
            new SP("bd_reapp").Execute("usp_ActualizarEstadoSolicitud",
            P.Add("IdSolicitud", hdnIdSolicitud.Value.ToInt()),
            P.Add("IdEstadoSolicitud", 8),
            P.Add("IdUsuarioCambioEstado", Session["IdUsuario"].ToString().ToInt()));
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

            //pnlBtnVerHistorialSolicitud.Visible = true;
        }

        protected void btnVerForo_Click(object sender, EventArgs e)
        {
            //Se obtiene id de solicitud
            int id = int.Parse((sender as LinkButton).CommandArgument);
            //Se crea nuevo form ForoMensajes
            ForoMensajes foroMensajes = new ForoMensajes();

            //Creamos String con la direccion de este form para despues el boton volver nos regrese a este form
            string formRedireccion = "/Forms/SolicitudesRespuesta/SolicitudesRespuesta.aspx";

            //Se redirecciona a ForoMensajes pasando por parametro (?parametro=valor) el idSolicitud de la tabla y la direccion de este form
            Response.Redirect("/Forms/ForoMensajes/ForoMensajes.aspx?idSolicitud=" + id + "&formRedireccion=" + formRedireccion);

        }

        //Boton Generar PDF - Testeando por ahora
        protected void btnRespuestaPDF_Click(object sender, EventArgs e)
        {
            //Llama al metodo GetRespuesta()
            Models.Documento Respuesta = GetRespuesta(true);
        }
        

        protected Models.Documento GetRespuesta(bool Descarga)
        {
            //
            int idSolicitud = hdnIdSolicitud.Value.ToInt();

            Models.Documento Documento;
            List<Models.Documento> Documentos = new SP("bd_reapp").Execute("usp_GetRespuestaSolicitud",
                P.Add("IdSolicitud", hdnIdSolicitud.Value.ToInt())
            ).ToList<Models.Documento>();

            if (Documentos.Count > 0)
            {
                // SI EXISTE, SE RECUPERA DE BD
                Documento = Documentos[0];
                byte[] bytes = Documento.Datos;

                //Si es true la descarga a la respuesta
                if (Descarga)
                {
                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment; filename=\"Respuesta_Solicitud_N" + hdnIdSolicitud.Value + ".pdf\"");
                    Response.BinaryWrite(bytes);
                    Response.ContentEncoding = Encoding.UTF8;
                    Response.End();
                }
                

            }
            else
            {

                HtmlToPdf converter = new HtmlToPdf();
                //string url = "https://www.w3schools.com"; Prueba con w3Schools
                string url = System.Web.Hosting.HostingEnvironment.MapPath("~/Templates/plantilla.html");
                string content = File.ReadAllText(url);

                //Reemplazar parámetros por las variables correspondientes
                string Ubicaciones = "";
                //Reemplazar esta por el id correspondiente, hardcodeado para testear
                //int idSolicitud = 97;
                string anoPresente = DateTime.Today.Year.ToString();
                string fechaHoy = DateTime.Today.ToShortDateString();
                //No esta funcionando correctamente
                string horaHoy = DateTime.Today.ToShortTimeString();

                string nombreSolicitud = "";
                string fechaSolicitud = "";
                string modalidad = "";
                string actividad = "";
                string provincia = "";
                string fechaInicioSoli = "";
                string fechaFinSoli = "";

                //Estos no los tenemos cargados en la base todavia, los hardcodeo
                string horaInicioSoli = "";
                string horaFinSoli = "";

                string nombreSolicitante = "";
                string apellidoSolicitante = "";
                string mailSolicitante = "";

                //Empieza con mayuscula los datos "compuestos" en tablas
                string Interesado = "";
                string Tripulacion = "";
                string Vant = "";

                //Aca creamos las datatables de los datos "compuestos"
                DataTable dtUbi = new SP("bd_reapp").Execute("usp_GetUbicacionesParaPDF", P.Add("IdSolicitud", idSolicitud));
                DataTable dtSolicitud = new SP("bd_reapp").Execute("usp_GetDatosSolicitudParaPDF", P.Add("IdSolicitud", idSolicitud));
                DataTable dtTripulacion = new SP("bd_reapp").Execute("usp_GetDatosTripulacionParaPDF", P.Add("IdSolicitud", idSolicitud));
                DataTable dtVant = new SP("bd_reapp").Execute("usp_GetDatosVantParaPDF", P.Add("IdSolicitud", idSolicitud));
                DataTable dtInteresado = new SP("bd_reapp").Execute("usp_GetDatosInteresadosParaPDF", P.Add("IdSolicitud", idSolicitud));

                //Reemplazo datos simples
                nombreSolicitud = dtSolicitud.Rows[0]["NombreSoli"].ToString().ToUpper();
                fechaSolicitud = dtSolicitud.Rows[0]["FAlta"].ToString().ToDateTime().ToShortDateString().ToUpper();
                modalidad = dtSolicitud.Rows[0]["Modalidad"].ToString().ToUpper();
                actividad = dtSolicitud.Rows[0]["Actividad"].ToString().ToUpper();
                provincia = dtSolicitud.Rows[0]["Provincia"].ToString().ToUpper();
                fechaInicioSoli = dtSolicitud.Rows[0]["FDesde"].ToString().ToDateTime().ToShortDateString().ToUpper();
                horaInicioSoli = dtSolicitud.Rows[0]["FDesde"].ToString().ToDateTime().AddHours(3).ToShortTimeString().ToUpper(); // Se le agregan 3 horas para pasar de utc-3 a utc+0
                fechaFinSoli = dtSolicitud.Rows[0]["FHasta"].ToString().ToDateTime().ToShortDateString().ToUpper();
                horaFinSoli = dtSolicitud.Rows[0]["FHasta"].ToString().ToDateTime().AddHours(3).ToShortTimeString().ToUpper();
                nombreSolicitante = dtSolicitud.Rows[0]["Nombre"].ToString().ToUpper();
                apellidoSolicitante = dtSolicitud.Rows[0]["Apellido"].ToString().ToUpper();
                mailSolicitante = dtSolicitud.Rows[0]["Email"].ToString();

                //Reemplazo tabla Ubicaciones
                for (int i = 0; i < dtUbi.Rows.Count; i++)
                {
                    Ubicaciones += "<tr>";

                    Ubicaciones += $"<td>{dtUbi.Rows[i]["IdUbicacion"].ToString().ToUpper()}</td>";
                    Ubicaciones += $"<td>{dtUbi.Rows[i]["Latitud"]}</td>";
                    Ubicaciones += $"<td>{dtUbi.Rows[i]["Longitud"]}</td>";
                    Ubicaciones += $"<td>{dtUbi.Rows[i]["Radio"]}</td>";
                    Ubicaciones += $"<td>{dtUbi.Rows[i]["Altura"]}</td>";

                    Ubicaciones += "</tr>";
                }

                //Reemplazo tabla Tripulacion
                for (int i = 0; i < dtTripulacion.Rows.Count; i++)
                {
                    Tripulacion += "<tr>";

                    Tripulacion += $"<td>{dtTripulacion.Rows[i]["Apellido"].ToString().ToUpper()}</td>";
                    Tripulacion += $"<td>{dtTripulacion.Rows[i]["Nombre"].ToString().ToUpper()}</td>";
                    Tripulacion += $"<td>{dtTripulacion.Rows[i]["DNI"]}</td>";
                    Tripulacion += $"<td>{dtTripulacion.Rows[i]["Correo"]}</td>";

                    Tripulacion += "</tr>";
                }

                //Reemplazo tabla Vant
                for (int i = 0; i < dtVant.Rows.Count; i++)
                {
                    Vant += "<tr>";

                    Vant += $"<td>VANT-{dtVant.Rows[i]["Vant"]}</td>";
                    Vant += $"<td>{dtVant.Rows[i]["Marca Vant"].ToString().ToUpper()}</td>";
                    Vant += $"<td>{dtVant.Rows[i]["Modelo"].ToString().ToUpper()}</td>";
                    Vant += $"<td>{dtVant.Rows[i]["Serial"].ToString().ToUpper()}</td>";

                    Vant += "</tr>";
                }

                //Reemplazo Interesados
                for (int i = 0; i < dtInteresado.Rows.Count; i++)
                {
                    Interesado += "<br />";

                    Interesado += $"&emsp;&emsp;{dtInteresado.Rows[i]["Nombre"].ToString().ToUpper()}";
                    Interesado += $"&emsp;&emsp;&emsp;&emsp;{dtInteresado.Rows[i]["Email"]}";
                }

                content = content.Replace("$NUMEROSOLICITUD$", idSolicitud.ToString()).Replace("$AÑOPRESENTE$", anoPresente).Replace("$NOMBRESOLICITUD$", nombreSolicitud).Replace("$FECHARESPUESTA$", fechaHoy).Replace("$HORARESPUESTA$", horaHoy).Replace("$NOMBRESOLICITANTE$", nombreSolicitante).Replace("$APELLIDOSOLICITANTE$", apellidoSolicitante).Replace("$MAILSOLICITANTE$", mailSolicitante).Replace("$FECHASOLICITUD$", fechaSolicitud).Replace("$ACTIVIDAD$", actividad).Replace("$MODALIDAD$", modalidad).Replace("$PROVINCIA$", provincia).Replace("$FECHAINICIO$", fechaInicioSoli).Replace("$FECHAFIN$", fechaFinSoli).Replace("$UBICACIONES$", Ubicaciones).Replace("$HORAINICIO$", horaInicioSoli).Replace("$HORAFIN$", horaFinSoli).Replace("$TRIPULACION$", Tripulacion).Replace("$VANT$", Vant).Replace("$INTERESADOS$", Interesado);

                //Aca se puede usar el ConvertHtmlString para que pongamos una cadena de html optimizada en una linea y asi poder modificar mejor el resultado final
                PdfDocument doc = converter.ConvertHtmlString(content);

                //Queda realizar algun tipo de control porque puede tardar unos segundos dependiendo del tamaño del pdf
                //Ver tema de descarga mas que guardado, similar a KMZ
                //doc.Save(@"D:\Usuario\Documents\Universidad\5 - Quinto\Proyecto\Pdf\respuestaPDF.pdf");

                byte[] bytes = doc.Save();

                //

                Documento = new Models.Documento()
                {
                    IdSolicitud = idSolicitud,
                    IdTipoDocumento = 13,
                    Extension = ".pdf",
                    FHAlta = DateTime.Now,
                    TipoMIME = "text/plain",
                    Datos = bytes,
                    Nombre = "Respuesta_Solicitud_N" + idSolicitud.ToString() + ".pdf"
                };
                Documento.Insert();

                //si es false no descarga
                if (Descarga)
                {
                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment; filename=\"Respuesta_Solicitud_N" + hdnIdSolicitud.Value + ".pdf\"");
                    Response.BinaryWrite(bytes);
                    Response.ContentEncoding = Encoding.UTF8;
                    Response.End();
                }

                doc.Close();
            }

            return Documento;
            
        }

        protected void btnEnviarMail_Click(object sender, EventArgs e)
        {
            try
            {
                int idSolicitud = hdnIdSolicitud.Value.ToInt();

                HTMLBuilder builder = new HTMLBuilder("Solicitud de Reserva de Espacio Aéreo", "GenericMailTemplate.html");

                string leftpart = Request.Url.GetLeftPart(UriPartial.Authority);
                string frmValidacion = "/Forms/VistaEstaticaSolicitud.aspx";
                string parameters = $"?S={hdnIdSolicitud.Value.ToInt().ToCryptoID()}";

                string url = $"{leftpart}{frmValidacion}{parameters}";

                builder.AppendTexto("Buenas tardes.");
                builder.AppendSaltoLinea(2);
                builder.AppendTexto("La Empresa Argentina de Navegación Aérea les comunica que la REA " + idSolicitud.ToString() + " fue aprobada por REA-CBA. Se adjunta la información relevante para el caso.");
                builder.AppendURL(url, "Detalles de la Solicitud");

                string cuerpo = builder.ConstruirHTML();

                MailController mail = new MailController("RESPUESTA REA", cuerpo);

                GetRespuesta(false);

                DataTable dtDocumento = new SP("bd_reapp").Execute("usp_GetRespuestaSolicitud",
                    P.Add("IdSolicitud", hdnIdSolicitud.Value.ToInt())); ;


                int idDoc = (int)dtDocumento.Rows[0][0];

                mail.Add(new Models.Documento().Select(idDoc));


                //false porque no descarga la respuesta, solo la genera si no esta y la guarda en la BD



                //Envio de mail a cada tripulante

                for (int i = 0; i < gvSoloInteresadosVinculados.Rows.Count; i++)
                {
                    if (((CheckBox)gvSoloInteresadosVinculados.Rows[i].FindControl("chkInteresadoVinculado")).Checked)
                    { // SI ESTÁ CHEQUEADO
                      //Logica Mails
                        string email = ((HiddenField)gvSoloInteresadosVinculados.Rows[i].FindControl("hdnEmail")).Value.ToString();
                        string nombre = ((HiddenField)gvSoloInteresadosVinculados.Rows[i].FindControl("hdnNombre")).Value.ToString();
                        //EnviarMail(nombre, email, idSolicitud);
                        mail.Add(nombre, email);
                    }
                }

                //Se trae el explotador con su mail

                Models.Solicitud Solicitud = new Models.Solicitud().Select(hdnIdSolicitud.Value.ToInt());
                int idSolicitante = Solicitud.IdUsuario;
                Models.Usuario Usuario = new Models.Usuario().Select(idSolicitante);
                string emailSolicitante = Usuario.Email.ToString();
                string nombreSolicitante = Usuario.Nombre + " " + Usuario.Apellido;
                mail.Add(nombreSolicitante, emailSolicitante);

                bool Exito = mail.Enviar();

                new SP("bd_reapp").Execute("usp_ActualizarEstadoSolicitud",
                P.Add("IdSolicitud", hdnIdSolicitud.Value.ToInt()),
                P.Add("IdEstadoSolicitud", 6),
                P.Add("IdUsuarioCambioEstado", Session["IdUsuario"].ToString().ToInt()));

                Alert("Correos enviados con éxito", "Se han enviado los correos de Respuesta.", AlertType.success, "/Forms/SolicitudesRespuesta.aspx");
            }
            catch (Exception)
            {
            }
        }
    }
}