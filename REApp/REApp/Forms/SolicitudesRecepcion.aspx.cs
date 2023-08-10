﻿using MagicSQL;
using REApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using static REApp.Navegacion;

namespace REApp.Forms
{
    public partial class SolicitudesRecepcion : System.Web.UI.Page
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

        public List<PuntoGeograficoRedux> PuntosGeograficos
        {
            get
            {
                if (ViewState["PuntosGeograficos"] == null)
                {
                    return new List<PuntoGeograficoRedux>();
                }
                else
                {
                    return ViewState["PuntosGeograficos"].ToString().ToList<PuntoGeograficoRedux>();
                }
            }
            set
            {
                if (value == null)
                {
                    ViewState["PuntosGeograficos"] = null;
                }
                else
                {
                    ViewState["PuntosGeograficos"] = value.ToJson();
                }
            }
        }

        private static List<UbicacionRedux> listaUbicaciones = new List<UbicacionRedux>();

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
                    btnEstadoOperador.Visible = true;
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
                parameters.Add(P.Add("IdEstadoSolicitud1", 1));
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
            listaUbicaciones.Clear();
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
                                
                                //Models.PuntoGeografico PuntoGeografico = AuxUbicaciones[i].PuntosGeograficos[j];

                                // SETEO LOS CAMPOS EL OBJETO
                                //PuntoGeografico.IdUbicacion = Ubicacion.IdUbicacion;

                                // INSERT EN TABLA PUNTOGEOGRAFICO
                                //PuntoGeografico.Insert(tn);
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
            btnFiltrar_Click(null, null);
        }

        protected void MostrarABM()
        {
            pnlListado.Visible = false;
            pnlABM.Visible = true;
            btnVolver.Visible = true;
            upModalABM.Update();
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

                rbVant.Checked = !Solicitud.IdAeronave.HasValue;
                rbAeronave.Checked = Solicitud.IdAeronave.HasValue;
                chkVant_CheckedChanged(null, null);

                CargarComboProvincias();
                GetUbicacionesDeSolicitud(IdSolicitud);
                tablaUbicacionesConsulta();

                GetTripulantesDeSolicitud(IdSolicitud);


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

        protected void ddlModalActividad_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idActividad = ddlModalActividad.SelectedItem.Value.ToIntID();
            CargarComboModalModalidades(idActividad);
        }

        //BOTON PARA GENERAR KMZ

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

        //Cargar Historial de Estados


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
            string formRedireccion = "/Forms/SolicitudesRecepcion/SolicitudesRecepcion.aspx";

            //Se redirecciona a ForoMensajes pasando por parametro (?parametro=valor) el idSolicitud de la tabla y la direccion de este form
            Response.Redirect("/Forms/ForoMensajes/ForoMensajes.aspx?idSolicitud=" + id + "&formRedireccion=" + formRedireccion);

        }

        protected void ddlSolicitante_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnFiltrar_Click(null, null);
        }

        protected void btnEstadoOperador_Click(object sender, EventArgs e)
        {
            // CONFIRMACIÓN CON MENSAJE OPCIONAL
            int IdSolicitud = hdnIdSolicitud.Value.ToInt();
            int IdEstado = 2;
            string FrmAnterior = "/Forms/SolicitudesAnalisis.aspx";

            string url = $"/Forms/CambioEstadoSolicitud.aspx?S={IdSolicitud}&E={IdEstado}&frm={FrmAnterior}";

            Response.Redirect(url);
        }

        protected void ddlVerBajas_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnFiltrar_Click(null, null);
        }

        protected void chkVant_CheckedChanged(object sender, EventArgs e)
        {
            pnlSeleccionVants.Visible = !rbAeronave.Checked;

            DataTable dt = null;
            if (!rbAeronave.Checked)
            {
                if (hdnIdSolicitud.Value.Equals(""))
                { // Si el txtModalFechaSolicitud no está visible, entonces estamos creando Solicitud
                    // Por ende, recupero los Vants del Usuario
                    dt = new SP("bd_reapp").Execute("usp_VantConsultar", P.Add("IdUsuario", ddlSolicitante.SelectedValue.ToIntID()));
                }
                else
                { // Si el txtModalFechaSolicitud está visible, entonces la Solicitud ya está creada
                    // Por ende, recupero los Vants de la Solicitud + los del Usuario

                    dt = new SP("bd_reapp").Execute("usp_GetVantsDeSolicitud",
                        P.Add("IdSolicitud", hdnIdSolicitud.Value.ToInt()),
                        P.Add("IdUsuario", ddlModalSolicitante.SelectedValue.ToIntID())
                    );
                }

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

        protected void GetUbicacionesDeSolicitud(int IdSolicitud)
        {
            DataTable dtUbicaciones = new SP("bd_reapp").Execute("usp_GetUbicacionesDeSolicitud", P.Add("IdSolicitud", IdSolicitud)); //OBTENEMOS UBICACIONES

            if (dtUbicaciones.Rows.Count > 0)
            {
                for (int i = 0; i < dtUbicaciones.Rows.Count; i++) //RECORREMOS SEGUN CANTIDAD DE UBICACIONES OBTENIDAS
                {
                    UbicacionRedux ubicacion = new UbicacionRedux();

                    ubicacion.IdUbicacion = dtUbicaciones.Rows[i]["IdUbicacion"].ToString();
                    ubicacion.Altura = dtUbicaciones.Rows[i]["Altura"].ToString().ToDouble();
                    ubicacion.IdProvincia = dtUbicaciones.Rows[i]["IdProvincia"].ToString().ToInt();
                    ubicacion.IdUbicacionGrupo = GetNextGrupoUbicacion();
                    ubicacion.estadoUbicacion = 2;
                    int? id = GetNextUbicacionId();
                    List<PuntoGeograficoRedux> puntosGeograficos = new List<PuntoGeograficoRedux>();

                    DataTable dtPuntosGeograficos = new SP("bd_reapp").Execute("usp_GetPuntosGeograficosDeUbicacion", P.Add("IdUbicacion", dtUbicaciones.Rows[i]["IdUbicacion"]));
                    if (dtPuntosGeograficos.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtPuntosGeograficos.Rows.Count; j++)
                        {
                            PuntoGeograficoRedux puntoGeografico = new PuntoGeograficoRedux();

                            puntoGeografico.Id = id;
                            puntoGeografico.IdPuntoGeografico = dtPuntosGeograficos.Rows[j]["IdPuntoGeografico"].ToString().ToInt();
                            puntoGeografico.Longitud = dtPuntosGeograficos.Rows[j]["Longitud"].ToString().ToDouble();
                            puntoGeografico.Latitud = dtPuntosGeograficos.Rows[j]["Latitud"].ToString().ToDouble();
                            if (dtPuntosGeograficos.Rows[j]["EsPoligono"].ToString().ToInt() == 1 || dtPuntosGeograficos.Rows[j]["EsPoligono"].ToString() == "True")
                            {
                                puntoGeografico.EsPoligono = true;
                            }
                            else
                            {
                                puntoGeografico.EsPoligono = false;
                                puntoGeografico.Radio = dtPuntosGeograficos.Rows[j]["Radio"].ToString().ToDouble();
                                id++;
                            }
                            puntoGeografico.eliminarBD = false;
                            puntosGeograficos.Add(puntoGeografico);

                        }
                    }
                    ubicacion.PuntosGeograficos = puntosGeograficos;

                    listaUbicaciones.Add(ubicacion);
                }

            }
        }

        private int? GetNextUbicacionId()
        {
            // Genera un nuevo ID automáticamente para una ubicación
            // En este ejemplo, incrementamos un contador y lo devolvemos

            int? maxId = 0;

            foreach (UbicacionRedux ubicacion in listaUbicaciones)
            {
                foreach (PuntoGeograficoRedux puntoGeografico in ubicacion.PuntosGeograficos)
                {
                    if (puntoGeografico.Id > maxId)
                    {
                        maxId = puntoGeografico.Id;
                    }
                }

            }

            return maxId + 1;
        }

        private int GetNextGrupoUbicacion()
        {
            int maxGrupo = 0;
            foreach (UbicacionRedux ubicacion in listaUbicaciones)
            {
                if (ubicacion.IdUbicacionGrupo > maxGrupo)
                {
                    maxGrupo = ubicacion.IdUbicacionGrupo;
                }
            }

            return maxGrupo + 1;

        }

        private void tablaUbicacionesConsulta()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("IdUbicacion");
            dt.Columns.Add("IdUbicacionGrupo");
            dt.Columns.Add("Poligono");
            dt.Columns.Add("Latitud");
            dt.Columns.Add("Longitud");
            dt.Columns.Add("Radio");
            dt.Columns.Add("Altura");
            dt.Columns.Add("idProvincia");
            dt.Columns.Add("estadoUbicacion");
            dt.Columns.Add("IdPuntoGeografico");
            dt.Columns.Add("EliminarBD");
            foreach (UbicacionRedux ubicacion in listaUbicaciones)
            {
                foreach (PuntoGeograficoRedux puntoGeografico in ubicacion.PuntosGeograficos)
                {
                    if (!puntoGeografico.eliminarBD)
                    {
                        string nuevoId = puntoGeografico.Id.ToString(); //id de tabla
                        string nuevoIdUbicacion = null; //id de ubicacion bd
                        if (ubicacion.IdUbicacion != null)
                        {
                            nuevoIdUbicacion = ubicacion.IdUbicacion;
                        }
                        string nuevoPoligono = puntoGeografico.EsPoligono.ToString();
                        string nuevoLatitud = puntoGeografico.Latitud.ToString();
                        string nuevoLonguitud = puntoGeografico.Longitud.ToString();
                        string nuevoRadio = null;
                        if (puntoGeografico.EsPoligono == false)
                        {
                            nuevoRadio = puntoGeografico.Radio.ToString();
                        }
                        string nuevoAltura = ubicacion.Altura.ToString();
                        string nuevoIdProvincia = ubicacion.IdProvincia.ToString();
                        string nuevoIdUbicacionGrupo = ubicacion.IdUbicacionGrupo.ToString();
                        string nuevoEstadoUbicacion = ubicacion.estadoUbicacion.ToString();
                        string nuevoIdPuntoGeografico = puntoGeografico.IdPuntoGeografico.ToString(); //id de ubicacion de bd
                        string nuevoEliminarBD = puntoGeografico.eliminarBD.ToString();


                        dt.Rows.Add(nuevoId, nuevoIdUbicacion, nuevoIdUbicacionGrupo, nuevoPoligono, nuevoLatitud, nuevoLonguitud, nuevoRadio, nuevoAltura, nuevoIdProvincia, nuevoEstadoUbicacion, nuevoIdPuntoGeografico, nuevoEliminarBD);

                    }

                }
            }
            gridUbicacionesConsulta.DataSource = dt;
            gridUbicacionesConsulta.DataBind();

            //Formato de tabla

            foreach (GridViewRow row in gridUbicacionesConsulta.Rows)
            {

                int idProvincia = Convert.ToInt32(row.Cells[8].Text); // Obtener el número de idProvincia

                string provincia = ddlProvincia.Items[idProvincia - 1].Text;//Obtener el valor string 

                row.Cells[8].Text = provincia;


                int idUbicacionGrupo = Convert.ToInt32(row.Cells[2].Text);



                if (idUbicacionGrupo % 2 == 0)
                {
                    row.BackColor = System.Drawing.Color.FromArgb(193, 193, 193);
                }
                else
                {
                    row.BackColor = System.Drawing.Color.White;
                }


                if (row.Cells[3].Text == "False")
                {
                    row.Cells[3].Text = "Circunferencia";
                }
                else
                {
                    row.Cells[3].Text = "Poligono";
                }


            }
        }

        protected void CargarComboProvincias()
        {
            ddlProvincia.Items.Clear();
            List<Provincia> Provincias = new Provincia().Select();

            foreach (Provincia provincia in Provincias)
            {
                ddlProvincia.Items.Add(new ListItem(provincia.Nombre, provincia.IdProvincia.ToCryptoID()));
            }
        }
    }
}