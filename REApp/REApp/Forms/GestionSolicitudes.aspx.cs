﻿using MagicSQL;
using REApp.Models;
using REAPP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using static REApp.Navegacion;

namespace REApp.Forms
{
    public partial class GestionSolicitudes : System.Web.UI.Page
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
            Page.Form.Attributes.Add("enctype", "multipart/form-data");

            //Aca hacemos el get que si o si es un string porque de object a int no deja
            string idUsuario = Session["IdUsuario"].ToString();
            string idRol = Session["IdRol"].ToString();

            //Estos se usan de esta forma porque son ints, ver si hay mejor forma de hacer el set
            int idRolInt = idRol.ToInt();
            int id = idUsuario.ToInt();

            pnlAcciones.Visible = false;

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
                    CargarFiltros();
                    BindGrid();
                    btnAgregarUbicacion.Visible = btnEscanearKML.Visible = fupKML.Visible = false;
                }
                if (idRolInt == 2)
                {
                    CargarFiltros();
                    BindGrid();
                    btnNuevo.Visible = false;
                    btnAgregarUbicacion.Visible = btnEscanearKML.Visible = fupKML.Visible = false;
                }
                //Rol Solicitante
                if (idRolInt == 3)
                {
                    CargarFiltros();
                    ddlSolicitante.SelectedValue = id.ToCryptoID().ToString();
                    ddlSolicitante.Enabled = false;
                    BindGrid();
                    btnNuevo.Visible = true;
                }
            }

            ScriptManager.GetCurrent(Page).RegisterPostBackControl(btnGenerarKMZ);
            VerHistorialSolicitud();
        }

        protected void CargarFiltros()
        {
            CargarComboSolicitante();
            CargarComboEstados();
            CargarComboActividades();
            CargarFiltroComboProvincias();
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
                //upModalABM.Update();
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
                if (!ddlSolicitante.SelectedItem.Value.Equals("#"))
                {
                    parameters.Add(P.Add("IdUsuario", ddlSolicitante.SelectedValue.ToIntID()));
                }
                if (!ddlEstado.SelectedValue.Equals("#"))
                {
                    parameters.Add(P.Add("IdEstado", ddlEstado.SelectedValue.ToIntID()));
                }
                if (!ddlActividad.SelectedValue.Equals("#"))
                {
                    parameters.Add(P.Add("IdActividad", ddlActividad.SelectedValue.ToIntID()));
                }
                if (!ddlFiltroProvincia.SelectedValue.Equals("#"))
                {
                    parameters.Add(P.Add("IdProvincia", ddlFiltroProvincia.SelectedValue.ToIntID()));
                }
                try
                {
                    DateTime d = txtFiltroFechaDesde.Text.ToDateTime();

                    if (d > System.Data.SqlTypes.SqlDateTime.MinValue.Value)
                    {
                        parameters.Add(P.Add("FechaDesde", d));
                    }
                }
                catch (Exception)
                {
                }
                try
                {
                    DateTime h = txtFiltroFechaHasta.Text.ToDateTime();

                    if (h > System.Data.SqlTypes.SqlDateTime.MinValue.Value)
                    {
                        parameters.Add(P.Add("FechaHasta", h));
                    }
                }
                catch (Exception)
                {
                }
                dt = sp.Execute("usp_GetSolicitudes", parameters.ToArray());
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
            ddlModalActividad_SelectedIndexChanged(null, null);
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
        protected void CargarFiltroComboProvincias()
        {
            ddlFiltroProvincia.Items.Clear();
            ddlFiltroProvincia.Items.Add(new ListItem("Todos", "#"));
            List<Provincia> Provincias = new Provincia().Select();

            foreach (Provincia provincia in Provincias)
            {
                ddlFiltroProvincia.Items.Add(new ListItem(provincia.Nombre, provincia.IdProvincia.ToCryptoID()));
            }
        }

        protected void CargarComboActividades()
        {
            ddlActividad.Items.Clear();
            ddlActividad.Items.Add(new ListItem("Todos", "#"));
            List<Actividad> Actividades = new Actividad().Select();

            foreach (Actividad actividad in Actividades)
            {
                ddlActividad.Items.Add(new ListItem(actividad.Nombre, actividad.IdActividad.ToCryptoID()));
            }
        }

        protected void CargarComboEstados()
        {
            ddlEstado.Items.Clear();
            ddlEstado.Items.Add(new ListItem("Todos", "#"));
            List<EstadoSolicitud> Estados = new EstadoSolicitud().Select();

            foreach (EstadoSolicitud estado in Estados)
            {
                ddlEstado.Items.Add(new ListItem(estado.Nombre, estado.IdEstadoSolicitud.ToCryptoID()));
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
            pnlAgregarCircunferencia.Visible = true;
            pnlAgregarPoligono.Visible = false;
            pnlAgregarUbicacion.Visible = false;
            pnlAgregarPuntoGeografico.Visible = false;
            btnAgregarUbicacion.Visible = btnEscanearKML.Visible = fupKML.Visible = true;
            btnAgregarPuntoGeografico.Visible = true;
            rptUbicaciones.DataSource = null;
            rptUbicaciones.DataBind();
            gvVANTs.DataSource = null;
            gvVANTs.DataBind();
            gvTripulacion.DataSource = null;
            gvTripulacion.DataBind();
            //fupKMZ.Attributes.Clear();
        }



        protected void lnkVerDetalles_Click(object sender, EventArgs e)
        {

        }

        protected void lnkDarDeBajaSolicitud_Click(object sender, EventArgs e)
        {

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarGuardar())
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
                            Solicitud.FHUltimaActualizacionEstado = DateTime.Now;

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

                            //if (fupKMZ.HasFile)
                            //{
                            //    string filename = Path.GetFileName(fupKMZ.PostedFile.FileName);
                            //    string extension = Path.GetExtension(fupKMZ.FileName);
                            //    extension = extension.ToLower();
                            //    string contentType = fupKMZ.PostedFile.ContentType;
                            //    int tam = fupKMZ.PostedFile.ContentLength;
                            //    byte[] bytes;
                            //    using (Stream fs = fupKMZ.PostedFile.InputStream)
                            //    {
                            //        using (BinaryReader br = new BinaryReader(fs))
                            //        {
                            //            bytes = br.ReadBytes((Int32)fs.Length);
                            //        }
                            //    }

                            //    Models.Documento Documento = new Models.Documento();
                            //    Documento.Nombre = filename;
                            //    Documento.IdSolicitud = Solicitud.IdSolicitud;
                            //    Documento.Datos = bytes;
                            //    Documento.Extension = extension;
                            //    Documento.FHAlta = DateTime.Now;
                            //    Documento.TipoMIME = contentType;
                            //    Documento.IdTipoDocumento = 5;
                            //    Documento.Insert(tn);
                            //}

                            // RECORRO LAS UBICACIONES DEL VIEWSTATE
                            List<UbicacionRedux> AuxUbicaciones = Ubicaciones;
                            for (int i = 0; i < AuxUbicaciones.Count; i++)
                            {
                                // CREO OBJETO UBICACION
                                Models.Ubicacion Ubicacion = new Models.Ubicacion();

                                // SETEO LOS CAMPOS DEL OBJETO
                                Ubicacion.IdSolicitud = Solicitud.IdSolicitud;
                                Ubicacion.Altura = AuxUbicaciones[i].Altura;
                                Ubicacion.IdProvincia = AuxUbicaciones[i].IdProvincia == 0 ? 1 : AuxUbicaciones[i].IdProvincia;

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

                            Models.SolicitudEstadoHistorial SolicitudEstadoHistorial = new Models.SolicitudEstadoHistorial();
                            SolicitudEstadoHistorial.FHDesde = DateTime.Now;
                            SolicitudEstadoHistorial.IdEstadoSolicitud = 1;
                            SolicitudEstadoHistorial.IdSolicitud = Solicitud.IdSolicitud;
                            SolicitudEstadoHistorial.IdUsuarioCambioEstado = Session["IdUsuario"].ToString().ToInt();
                            SolicitudEstadoHistorial.Insert(tn);

                            tn.Commit();

                            Alert("Solicitud creada con éxito", "Se ha agregado una nueva solicitud.", AlertType.success);
                        }
                        catch (Exception ex)
                        {
                            tn.RollBack();

                            Alert("Error", "Ocurrió un error. Detalles del error: " + ex.Message, AlertType.error);
                        }
                    }
                }
                else
                { // Update
                    using (Tn tn = new Tn("bd_reapp"))
                    {
                        // RECUPERO EL OBJETO SOLICITUD
                        Solicitud = new Models.Solicitud().Select(hdnIdSolicitud.Value.ToInt());

                        // RE-SETEO LOS CAMPOS DE LA SOLICITUD
                        Solicitud.Nombre = txtModalNombreSolicitud.Text;
                        Solicitud.IdModalidad = ddlModalModalidad.SelectedValue.ToIntID();
                        //Solicitud.IdEstadoSolicitud = 1;
                        Solicitud.FHDesde = txtModalFechaDesde.Text.ToDateTime();
                        Solicitud.FHHasta = txtModalFechaHasta.Text.ToDateTime();
                        Solicitud.Observaciones = txtModalObservaciones.Text;

                        // UPDATE EN TABLA SOLICITUD
                        Solicitud.Update(tn);

                        // RECUPERO LOS VANTS ANTES DE LA EDICIÓN
                        DataTable dt = new SP("bd_reapp").Execute("usp_GetVantsDeSolicitud",
                            P.Add("IdSolicitud", hdnIdSolicitud.Value.ToInt()),
                            P.Add("IdUsuario", ddlModalSolicitante.SelectedValue.ToIntID())
                        );

                        // RECORRO LA GRILLA DE VANTS
                        for (int i = 0; i < gvVANTs.Rows.Count; i++)
                        {
                            if (((CheckBox)gvVANTs.Rows[i].FindControl("chkVANTVinculado")).Checked)
                            { // SI ESTÁ CHEQUEADO
                                // ASUMIMOS QUE EL VANT NO ESTÁ VINCULADO
                                bool Existe = false;

                                // RECORRO LOS VANTS ANTES DE LA EDICIÓN
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    if (gvVANTs.Rows[i].Cells[0].Text.Equals(dt.Rows[j]["IdVant"].ToString()))
                                    {
                                        // POR LEGIBILIDAD DESMENUCÉ ESTE IF
                                        if (dt.Rows[j]["Checked"].ToString().Equals("1"))
                                        {
                                            // EL VANT SÍ ESTÁ VINCULADO
                                            Existe = true;
                                            break;
                                        }
                                    }
                                }

                                if (!Existe)
                                { // SI SE RECORRIÓ TODO EL LISTADO Y EL VANT NO ESTÁ VINCULADO
                                    // CREO OBJETO VANTSOLICITUD
                                    Models.VantSolicitud VantSolicitud = new Models.VantSolicitud();

                                    // SETEO LOS CAMPOS DEL OBJETO
                                    VantSolicitud.IdVant = ((HiddenField)gvVANTs.Rows[i].FindControl("hdnIdVant")).Value.ToInt();
                                    VantSolicitud.IdSolicitud = Solicitud.IdSolicitud;

                                    // INSERT EN TABLA VANTSOLICITUD
                                    VantSolicitud.Insert(tn);
                                }
                            }
                            else
                            { // SI NO ESTÁ CHEQUEADO
                                // ASUMIMOS QUE EL VANT ESTÁ VINCULADO
                                bool Existe = true;

                                // RECORRO LOS VANTS ANTES DE LA EDICIÓN
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    if (gvVANTs.Rows[i].Cells[0].Text.Equals(dt.Rows[j]["IdVant"].ToString()))
                                    {
                                        // POR LEGIBILIDAD DESMENUCÉ ESTE IF
                                        if (dt.Rows[j]["Checked"].ToString().Equals("0"))
                                        {
                                            // EL VANT ESTÁ VINCULADO
                                            Existe = false;
                                            break;
                                        }
                                    }
                                }

                                if (Existe)
                                { // SI SE RECORRIÓ TODO EL LISTADO Y EL VANT ESTÁ VINCULADO
                                    int IdVant = ((HiddenField)gvVANTs.Rows[i].FindControl("hdnIdVant")).Value.ToInt();
                                    int IdSolicitud = Solicitud.IdSolicitud;

                                    // RECUPERO OBJETO VANTSOLICITUD
                                    List<Models.VantSolicitud> VantSolicitud = new Models.VantSolicitud().Select($"IdVant = {IdVant} AND IdSolicitud = {IdSolicitud}");

                                    if (VantSolicitud.Count != 0)
                                    {
                                        // SETEO FHFIN
                                        VantSolicitud[0].FHBaja = DateTime.Now;

                                        // UPDATE EN TABLA VANTSOLICITUD
                                        VantSolicitud[0].Update(tn);
                                    } // FIN IF UPDATE VANTSOLICITUD
                                } // FIN IF EXISTE VANT VINCULADO
                            } // FIN ELSE NO ESTÁ CHEQUEADO
                        } // FIN FOR GRILLA VANTS

                        // RECORRO LAS UBICACIONES DEL VIEWSTATE (SON SÓLO LAS NUEVAS)
                        List<UbicacionRedux> AuxUbicaciones = Ubicaciones;
                        for (int i = 0; i < AuxUbicaciones.Count; i++)
                        {
                            // CREO OBJETO UBICACION
                            Models.Ubicacion Ubicacion = new Models.Ubicacion();

                            // SETEO LOS CAMPOS DEL OBJETO
                            Ubicacion.IdSolicitud = Solicitud.IdSolicitud;
                            Ubicacion.Altura = AuxUbicaciones[i].Altura;
                            Ubicacion.IdProvincia = AuxUbicaciones[i].IdProvincia;

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

                        // RECUPERO LOS TRIPULANTES ANTES DE LA EDICIÓN
                        dt = new SP("bd_reapp").Execute("usp_GetTripulacionDeSolicitud",
                            P.Add("IdSolicitud", hdnIdSolicitud.Value.ToInt()),
                            P.Add("IdUsuario", ddlModalSolicitante.SelectedValue.ToIntID())
                        );

                        // RECORRO LA GRILLA DE TRIPULANTES
                        for (int i = 0; i < gvTripulacion.Rows.Count; i++)
                        {
                            if (((CheckBox)gvTripulacion.Rows[i].FindControl("chkTripulacionVinculado")).Checked)
                            { // SI ESTÁ CHEQUEADO
                                // ASUMIMOS QUE EL TRIPULANTE NO ESTÁ VINCULADO
                                bool Existe = false;

                                // RECORRO LOS TRIPULANTES ANTES DE LA EDICIÓN
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    if (gvTripulacion.Rows[i].Cells[0].Text.Equals(dt.Rows[j]["IdTripulacion"].ToString()))
                                    {
                                        // POR LEGIBILIDAD DESMENUCÉ ESTE IF
                                        if (dt.Rows[j]["Checked"].ToString().Equals("1"))
                                        {
                                            // EL TRIPULANTE SÍ ESTÁ VINCULADO
                                            Existe = true;
                                            break;
                                        }
                                    }
                                }

                                if (!Existe)
                                { // SI SE RECORRIÓ TODO EL LISTADO Y EL TRIPULANTE NO ESTÁ VINCULADO
                                    // CREO OBJETO TRIPULACIONSOLICITUD
                                    Models.TripulacionSolicitud TripulacionSolicitud = new Models.TripulacionSolicitud();

                                    // SETEO LOS CAMPOS DEL OBJETO
                                    TripulacionSolicitud.IdTripulacion = ((HiddenField)gvVANTs.Rows[i].FindControl("hdnIdTripulacion")).Value.ToInt();
                                    TripulacionSolicitud.IdSolicitud = Solicitud.IdSolicitud;

                                    // INSERT EN TABLA VANTSOLICITUD
                                    TripulacionSolicitud.Insert(tn);
                                }
                            }
                            else
                            { // SI NO ESTÁ CHEQUEADO
                                // ASUMIMOS QUE EL VANT ESTÁ VINCULADO
                                bool Existe = true;

                                // RECORRO LOS VANTS ANTES DE LA EDICIÓN
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    if (gvTripulacion.Rows[i].Cells[0].Text.Equals(dt.Rows[j]["IdTripulacion"].ToString()))
                                    {
                                        // POR LEGIBILIDAD DESMENUCÉ ESTE IF
                                        if (dt.Rows[j]["Checked"].ToString().Equals("0"))
                                        {
                                            // EL TRIPULANTE ESTÁ VINCULADO
                                            Existe = false;
                                            break;
                                        }
                                    }
                                }

                                if (Existe)
                                { // SI SE RECORRIÓ TODO EL LISTADO Y EL TRIPULANTE ESTÁ VINCULADO
                                    int IdTripulante = ((HiddenField)gvTripulacion.Rows[i].FindControl("hdnIdTripulacion")).Value.ToInt();
                                    int IdSolicitud = Solicitud.IdSolicitud;

                                    // RECUPERO OBJETO VANTSOLICITUD
                                    List<Models.TripulacionSolicitud> TripulacionSolicitud = new Models.TripulacionSolicitud().Select($"IdTripulacion = {IdTripulante} AND IdSolicitud = {IdSolicitud}");

                                    if (TripulacionSolicitud.Count != 0)
                                    {
                                        // SETEO FHFIN
                                        TripulacionSolicitud[0].FHDesvinculacion = DateTime.Now;

                                        // UPDATE EN TABLA VANTSOLICITUD
                                        TripulacionSolicitud[0].Update(tn);
                                    } // FIN IF UPDATE VANTSOLICITUD
                                } // FIN IF EXISTE VANT VINCULADO
                            } // FIN ELSE NO ESTÁ CHEQUEADO
                        } // FIN FOR GRILLA VANTS

                        tn.Commit();
                        Alert("Solicitud modificada con éxito", "Se ha modificado una solicitud existente.", AlertType.success);

                    } // FIN USING TN
                } // FIN ELSE UPDATE

                MostrarListado();
            }
        }
        protected bool ValidarUsuarioEANA()
        {
            bool TieneValidacionEANA = false;

            int IdUsuario = ddlSolicitante.SelectedValue.ToIntID();

            try
            {
                Models.Usuario Usuario = new Models.Usuario().Select(IdUsuario);

                if (Usuario.ValidacionEANA)
                {
                    TieneValidacionEANA = true;
                }

            }
            catch (Exception ex)
            {
                Alert("Error", "Ocurrió un error en la validación del usuario", AlertType.error);
            }

            if (!TieneValidacionEANA)
            {
                Alert("Error", "Su usuario no se encuentro autorizado para crear solicitudes. Su usuario aún  no ha sido validado por el Administrador.", AlertType.error);
            }
            return TieneValidacionEANA;
        }


        protected bool ValidarDocumentacion()
        {
            bool TieneCertificadoMedico = false;
            bool TieneCertificadoCompetencia = false;
            bool TieneCEVANT = false;


            int IdUsuario = ddlSolicitante.SelectedValue.ToIntID();

            try
            {
                List<Documento> DocumentosUsuario = new Documento().Select($"IdUsuario = {IdUsuario}");

                int IdCertificadoMedico = new TipoDocumento().Select("Nombre = 'Certificado Médico'")[0].IdTipoDocumento;
                int IdCertificadoComptencia = new TipoDocumento().Select("Nombre = 'Certificado de Competencia'")[0].IdTipoDocumento;
                int IdCEVANT = new TipoDocumento().Select("Nombre = 'CEVANT'")[0].IdTipoDocumento;

                foreach (Documento Doc in DocumentosUsuario)
                {
                    if (Doc.FHVencimiento.Value > DateTime.Now)
                    {
                        if (Doc.IdTipoDocumento.Value == IdCertificadoMedico)
                        {
                            TieneCertificadoMedico = true;
                        }
                        if (Doc.IdTipoDocumento.Value == IdCertificadoComptencia)
                        {
                            TieneCertificadoCompetencia = true;
                        }
                        if (Doc.IdTipoDocumento.Value == IdCEVANT)
                        {
                            TieneCEVANT = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Alert("Error", "Ocurrió un error en la validación de los documentos.", AlertType.error);
            }

            if (!TieneCertificadoCompetencia)
            {
                Alert("Error", "No se encontró Certificado de Competencia o el mismo ha caducado. Por favor, verifique su documentación.", AlertType.error);
            }
            else if (!TieneCertificadoMedico)
            {
                Alert("Error", "No se encontró Certificado Médico o el mismo ha caducado. Por favor, verifique su documentación.", AlertType.error);
            }
            else if (!TieneCEVANT)
            {
                Alert("Error", "No se encontró CEVANT o el mismo ha caducado. Por favor, verifique su documentación.", AlertType.error);
            }

            return TieneCertificadoMedico && TieneCertificadoCompetencia && TieneCEVANT;
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            // Si el usuario logueado no es de tipo Solicitante O se validó la documentación
            if (!Session["IdRol"].ToString().Equals("3") || ValidarDocumentacion())
            {
                if (ValidarUsuarioEANA())
                {
                    LimpiarModal();
                    OcultarMostrarPanelesABM(false);
                    HabilitarDeshabilitarTxts(true);

                    CargarComboModalSolicitante();
                    CargarComboModalActividades();
                    int idActividad = ddlModalActividad.SelectedItem.Value.ToIntID();
                    CargarComboModalModalidades(idActividad);
                    CargarComboProvincias();

                    ddlModalSolicitante.SelectedValue = ddlSolicitante.SelectedValue;
                    ddlModalSolicitante.Visible = true;
                    ddlModalSolicitante.Enabled = false;
                    btnGuardar.Visible = true;

                    chkVant.Checked = false;
                    chkVant_CheckedChanged(null, null);

                    pnlHistorialSolicitud.Visible = false;
                    btnAgregarUbicacion.Visible = btnEscanearKML.Visible = fupKML.Visible = true;

                    MostrarABM();
                }

            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            MostrarListado();
            hdnIdSolicitud.Value = "";
            hdnIdEstadoAnterior.Value = "";
            if (Session["IdRol"].ToString().ToInt() == 2)
            {//Buscar otra forma de hacer
                btnNuevo.Visible = false;
            }
        }

        protected void MostrarListado()
        {
            pnlListado.Visible = true;
            btnNuevo.Visible = true;
            pnlABM.Visible = false;
            btnVolver.Visible = false;
            btnGenerarKMZ.Visible = false;
            //btnRespuestaPDF.Visible = false;
            btnFiltrar_Click(null, null);
        }

        protected void MostrarABM()
        {
            pnlListado.Visible = false;
            btnNuevo.Visible = false;
            pnlABM.Visible = true;
            btnVolver.Visible = true;

            if (hdnIdSolicitud.Value.Equals(""))
            {
                GetTripulantesDeUsuario(ddlModalSolicitante.SelectedValue.ToIntID());
            }
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
            txtModalFechaDesde.Enabled = valor;
            txtModalFechaHasta.Enabled = valor;
            txtModalNombreSolicitud.Enabled = valor;
            txtModalObservaciones.Enabled = valor;
            btnGuardar.Visible = valor;

            foreach (GridViewRow vant in gvVANTs.Rows)
            {
                ((CheckBox)vant.FindControl("chkVANTVinculado")).Enabled = valor;
            }
            foreach (GridViewRow trip in gvTripulacion.Rows)
            {
                ((CheckBox)trip.FindControl("chkTripulacionVinculado")).Enabled = valor;
            }
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
            CargarComboProvincias();

            ddlModalActividad.SelectedValue = Modalidad.IdActividad.ToCryptoID().ToString();
            ddlModalModalidad.SelectedValue = Solicitud.IdModalidad.ToCryptoID().ToString();

            hdnIdSolicitud.Value = IdSolicitud.ToString();
            txtModalNombreSolicitud.Text = Solicitud.Nombre;

            txtModalObservaciones.Text = Solicitud.Observaciones;
            txtModalEstadoSolicitud.Text = Estado.Nombre;
            int idEstadoActual = Estado.IdEstadoSolicitud;

            txtModalFechaDesde.TextMode = TextBoxMode.SingleLine;
            txtModalFechaHasta.TextMode = TextBoxMode.SingleLine;
            txtModalFechaDesde.Text = Solicitud.FHDesde.ToString();
            txtModalFechaHasta.Text = Solicitud.FHHasta.ToString();
            txtModalFechaHasta.Enabled = false;
            txtModalFechaDesde.Enabled = false;

            //Se recupera el penultimo estado para enviar mails a interesados si estruvo en coordinacion
            DataTable dt = new SP("bd_reapp").Execute("usp_GetPenultimoEstadoSolicitud", P.Add("IdSolicitud", IdSolicitud));
            if (dt.Rows.Count > 0)
            {
                hdnIdEstadoAnterior.Value = dt.Rows[0]["IdEstadoSolicitud"].ToString();
            }

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
                HabilitarDeshabilitarTxts(false);
                pnlHistorialSolicitud.Visible = true;
                btnAgregarUbicacion.Visible = btnEscanearKML.Visible = fupKML.Visible = false;
            }
            if (e.CommandName.Equals("Editar"))
            {
                btnGenerarKMZ.Visible = true;
                btnGuardar.Visible = true;
                HabilitarDeshabilitarTxts(true);
                pnlHistorialSolicitud.Visible = true;
                btnAgregarUbicacion.Visible = btnEscanearKML.Visible = fupKML.Visible = true;

                //Si esta en estado PendienteModificacion, se habilita btnEnviarSolicitud
                if (idEstadoActual == 9)
                {
                    string idRol = Session["IdRol"].ToString();
                    int idRolInt = idRol.ToInt();
                    //Y tiene rol Explotador
                    if (idRolInt == 3)
                    {
                        pnlAcciones.Visible = true;
                    }
                }
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
                    ((HiddenField)rptUbicaciones.Items[i].FindControl("hdnRptIdUbicacion")).Value = dt.Rows[i]["IdUbicacion"].ToString();
                    ((HiddenField)rptUbicaciones.Items[i].FindControl("hdnRptIdProvincia")).Value = dt.Rows[i]["IdProvincia"].ToString();
                }
            }
        }

        protected void ddlModalActividad_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idActividad = ddlModalActividad.SelectedItem.Value.ToIntID();

            if (idActividad > 0)
            {
                Actividad Actividad = new Actividad().Select(idActividad);

                if (Actividad.AdmiteVANTs.HasValue && Actividad.AdmiteVANTs.Value)
                { // Admite VANTs
                    chkVant.Checked = false;
                    chkVant.Enabled = false;
                    pnlVehiculos.Visible = true;
                    chkVant_CheckedChanged(null, null);
                }
                else if (Actividad.AdmiteAeronaves.HasValue && Actividad.AdmiteAeronaves.Value)
                { // Admite Aeronaves
                    chkVant.Checked = true;
                    chkVant.Enabled = false;
                    pnlVehiculos.Visible = true;
                    chkVant_CheckedChanged(null, null);
                }
                else if (Actividad.AdmiteVANTs.HasValue && !Actividad.AdmiteVANTs.Value && Actividad.AdmiteAeronaves.HasValue && !Actividad.AdmiteAeronaves.Value)
                { // No admite ni VANTs ni Aeronaves
                    pnlVehiculos.Visible = false;
                }

                CargarComboModalModalidades(idActividad);
            }
        }

        protected void lnkEditar_Click(object sender, EventArgs e)
        {

        }

        protected void chkVant_CheckedChanged(object sender, EventArgs e)
        {
            pnlSeleccionVants.Visible = !chkVant.Checked;

            DataTable dt = null;
            if (!chkVant.Checked)
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

        protected void btnAgregarUbicacion_Click(object sender, EventArgs e)
        {
            pnlAgregarUbicacion.Visible = true;
            btnAgregarUbicacion.Visible = btnEscanearKML.Visible = fupKML.Visible = false;

            txtCircunferenciaAltura.Text =
            txtCircunferenciaLatitud.Text =
            txtCircunferenciaLongitud.Text =
            txtCircunferenciaRadio.Text = "";
            gvPuntosGeograficos.DataSource = null;
        }

        protected void btnGuardarUbicacion_Click(object sender, EventArgs e)
        {
            if (ValidarGuardarUbicacion())
            {
                pnlAgregarUbicacion.Visible = false;
                pnlAgregarPuntoGeograficoYGrilla.Visible = false;
                btnAgregarUbicacion.Visible = btnEscanearKML.Visible = fupKML.Visible = true;
                AgregarUbicacionRepeater();

                UbicacionRedux Ubicacion = new UbicacionRedux();

                if (chkEsPoligono.Checked)
                {
                    List<Models.PuntoGeografico> PuntosGeograficos = new List<Models.PuntoGeografico>();

                    for (int i = 0; i < rptUbicaciones.Items.Count; i++)
                    {
                        // index 0: Latitud: <latitud>
                        // index 1: Longitud: <longitud>
                        // index 2: Altura: <altura>
                        string[] SplitPuntosGeograficos = ((Label)rptUbicaciones.Items[i].FindControl("lblRptDatos")).Text.Trim().Split('|');
                        Ubicacion.IdUbicacion = ((HiddenField)rptUbicaciones.Items[i].FindControl("hdnRptIdUbicacion")).Value;
                        Ubicacion.IdProvincia = ((HiddenField)rptUbicaciones.Items[i].FindControl("hdnRptIdProvincia")).Value.ToInt();
                        PuntosGeograficos = new List<Models.PuntoGeografico>();

                        for (int j = 0; j < SplitPuntosGeograficos.Length; j++)
                        {
                            string TipoUbicacion = ((Label)rptUbicaciones.Items[i].FindControl("lblRptTipoUbicacion")).Text;
                            if (TipoUbicacion.Equals("Polígono"))
                            {
                                if (!SplitPuntosGeograficos[j].Equals(""))
                                {
                                    string[] SplitMagnitudes = SplitPuntosGeograficos[j].Split('/');
                                    string Latitud = SplitMagnitudes[0].Trim().Split(' ')[1];
                                    string Longitud = SplitMagnitudes[1].Trim().Split(' ')[1];
                                    string Altura = SplitMagnitudes[2].Trim().Split(' ')[1];

                                    if (!Latitud.Equals("") && !Longitud.Equals(""))
                                    {
                                        Ubicacion.Altura = Altura.Replace('.', ',').ToDouble();

                                        Models.PuntoGeografico PuntoGeografico = new Models.PuntoGeografico();
                                        PuntoGeografico.EsPoligono = true;
                                        PuntoGeografico.Latitud = Latitud.Replace('.', ',').ToDouble();
                                        PuntoGeografico.Longitud = Longitud.Replace('.', ',').ToDouble();

                                        PuntosGeograficos.Add(PuntoGeografico);
                                    }
                                }
                            }
                        }
                    }

                    Ubicacion.PuntosGeograficos = PuntosGeograficos;
                }
                else
                {
                    Ubicacion.Altura = txtCircunferenciaAltura.Text.Replace('.', ',').Replace('.', ',').ToDouble();
                    Ubicacion.IdProvincia = ddlProvincia.SelectedValue.ToIntID();

                    List<Models.PuntoGeografico> PuntosGeograficos = new List<Models.PuntoGeografico>();

                    Models.PuntoGeografico PuntoGeografico = new Models.PuntoGeografico();
                    PuntoGeografico.EsPoligono = false;
                    PuntoGeografico.Latitud = txtCircunferenciaLatitud.Text.Replace('.', ',').Replace('.', ',').ToDouble();
                    PuntoGeografico.Longitud = txtCircunferenciaLongitud.Text.Replace('.', ',').Replace('.', ',').ToDouble();
                    PuntoGeografico.Radio = txtCircunferenciaRadio.Text.Replace('.', ',').Replace('.', ',').ToDouble();

                    PuntosGeograficos.Add(PuntoGeografico);

                    Ubicacion.PuntosGeograficos = PuntosGeograficos;
                }

                List<UbicacionRedux> AuxUbicaciones = Ubicaciones;
                AuxUbicaciones.Add(Ubicacion);
                Ubicaciones = AuxUbicaciones;

                gvPuntosGeograficos.DataSource = null;
                gvPuntosGeograficos.DataBind();
            }
        }

        protected void btnAgregarPuntoGeografico_Click(object sender, EventArgs e)
        {
            pnlAgregarPuntoGeografico.Visible = true;
            pnlAgregarPuntoGeograficoYGrilla.Visible = true;
            btnAgregarPuntoGeografico.Visible = false;

            txtPoligonoLatitud.Text =
            txtPoligonoLongitud.Text =
            txtPoligonoAltura.Text = "";
        }

        protected void btnGuardarPuntoGeografico_Click(object sender, EventArgs e)
        {
            if (ValidarGuardarPuntoGeografico())
            {
                pnlAgregarPuntoGeografico.Visible = false;
                btnAgregarPuntoGeografico.Visible = true;
                AgregarPuntoGeograficoGridview();
            }
        }

        protected void chkEsPoligono_CheckedChanged(object sender, EventArgs e)
        {
            pnlAgregarPoligono.Visible = chkEsPoligono.Checked;
            pnlAgregarCircunferencia.Visible = !chkEsPoligono.Checked;
        }

        protected void AgregarPuntoGeograficoGridview()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Latitud");
            dt.Columns.Add("Longitud");
            dt.Columns.Add("Altura");

            for (int i = 0; i < gvPuntosGeograficos.Rows.Count; i++)
            {
                string Latitud = Server.HtmlDecode(gvPuntosGeograficos.Rows[i].Cells[0].Text);
                string Longitud = Server.HtmlDecode(gvPuntosGeograficos.Rows[i].Cells[1].Text);
                string Altura = Server.HtmlDecode(gvPuntosGeograficos.Rows[i].Cells[2].Text);
                dt.Rows.Add(Latitud, Longitud, Altura);
            }

            string NuevaLatitud = txtPoligonoLatitud.Text;
            string NuevaLongitud = txtPoligonoLongitud.Text;
            string NuevaAltura = txtPoligonoAltura.Text;
            dt.Rows.Add(NuevaLatitud, NuevaLongitud, NuevaAltura);

            gvPuntosGeograficos.DataSource = dt;
            gvPuntosGeograficos.DataBind();
        }

        protected void AgregarUbicacionRepeater()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TipoUbicacion");
            dt.Columns.Add("Datos");

            for (int i = 0; i < rptUbicaciones.Items.Count; i++)
            {
                string TipoUbicacion = ((Label)rptUbicaciones.Items[i].FindControl("lblRptTipoUbicacion")).Text;
                string Datos = ((Label)rptUbicaciones.Items[i].FindControl("lblRptDatos")).Text;
                dt.Rows.Add(TipoUbicacion, Datos);
            }

            string NuevaTipoUbicacion = (chkEsPoligono.Checked) ? "Polígono" : "Circunferencia";
            string NuevaDatos = "";
            if (chkEsPoligono.Checked)
            {
                for (int i = 0; i < gvPuntosGeograficos.Rows.Count; i++)
                {
                    string Latitud = Server.HtmlDecode(gvPuntosGeograficos.Rows[i].Cells[0].Text);
                    string Longitud = Server.HtmlDecode(gvPuntosGeograficos.Rows[i].Cells[1].Text);
                    string Altura = Server.HtmlDecode(gvPuntosGeograficos.Rows[i].Cells[2].Text);

                    NuevaDatos += "Latitud: " + Latitud + " / Longitud: " + Longitud + " / Altura: " + Altura + " | ";
                }
            }
            else
            {
                NuevaDatos = "Latitud: " + txtCircunferenciaLatitud.Text + " / Longitud: " + txtCircunferenciaLongitud.Text + " / Altura: " + txtCircunferenciaAltura.Text + " / Radio: " + txtCircunferenciaRadio.Text;
            }
            dt.Rows.Add(NuevaTipoUbicacion, NuevaDatos);

            rptUbicaciones.DataSource = dt;
            rptUbicaciones.DataBind();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ((Label)rptUbicaciones.Items[i].FindControl("lblRptTipoUbicacion")).Text = dt.Rows[i][0].ToString();
                ((Label)rptUbicaciones.Items[i].FindControl("lblRptDatos")).Text = dt.Rows[i][1].ToString();
            }
        }

        protected bool ValidarGuardarPuntoGeografico()
        {
            if (txtPoligonoLatitud.Text.Equals(""))
            {
                Alert("Error", "Por favor, ingrese latitud.", AlertType.error);
                return false;
            }
            if (txtPoligonoLongitud.Text.Equals(""))
            {
                Alert("Error", "Por favor, ingrese longitud.", AlertType.error);
                return false;
            }
            if (txtPoligonoAltura.Text.Equals(""))
            {
                Alert("Error", "Por favor, ingrese altura.", AlertType.error);
                return false;
            }
            return true;
        }

        protected bool ValidarGuardarUbicacion()
        {
            if (!chkEsPoligono.Checked)
            {
                if (txtCircunferenciaAltura.Text.Equals(""))
                {
                    Alert("Error", "Por favor, ingrese altura.", AlertType.error);
                    return false;
                }
                if (txtCircunferenciaLatitud.Text.Equals(""))
                {
                    Alert("Error", "Por favor, ingrese latitud.", AlertType.error);
                    return false;
                }
                if (txtCircunferenciaLongitud.Text.Equals(""))
                {
                    Alert("Error", "Por favor, ingrese longitud.", AlertType.error);
                    return false;
                }
                if (txtCircunferenciaRadio.Text.Equals(""))
                {
                    Alert("Error", "Por favor, ingrese radio.", AlertType.error);
                    return false;
                }
            }
            return true;
        }

        protected bool ValidarGuardar()
        {
            if (txtModalNombreSolicitud.Text.Equals(""))
            {
                Alert("Error", "Por favor, ingrese un nombre representativo para la reserva.", AlertType.error);
                return false;
            }


            if (txtModalFechaDesde.Text.Equals(""))
            {
                Alert("Error", "Por favor, ingrese la fecha y hora desde la que desea solicitar la reserva.", AlertType.error);
                return false;
            }


            if (txtModalFechaHasta.Text.Equals(""))
            {
                Alert("Error", "Por favor, ingrese la fecha y hora hasta la que desea solicitar la reserva.", AlertType.error);
                return false;
            }

            ////Se valida la expresión regular de la fecha de nacimiento
            //string datePattern = @"^(19|20)\d\d[-](0[1-9]|1[012])[-](0[1-9]|[12][0-9]|3[01])$";
            //if ((!Regex.IsMatch(txtModalFechaDesde.Text, datePattern)) || (txtModalFechaDesde.Text.ToDateTimeNull() == null))
            //{
            //    Alert("Error", "Por favor, ingrese una fecha y hora válida desde la que desea solicitar la reserva.", AlertType.error);
            //    return false;
            //}
            //if ((!Regex.IsMatch(txtModalFechaHasta.Text, datePattern)) || (txtModalFechaHasta.Text.ToDateTimeNull() == null))
            //{
            //    Alert("Error", "Por favor, ingrese una fecha y hora válida hasta la que desea solicitar la reserva.", AlertType.error);
            //    return false;
            //}

            DateTime fechaActual = DateTime.Now.Date;

            string fechaDesdeString = txtModalFechaDesde.Text.Replace("-", "/");
            string fechaHastaString = txtModalFechaHasta.Text.Replace("-", "/");

            DateTime fechaDesde = DateTime.Parse(fechaDesdeString, CultureInfo.InvariantCulture);
            DateTime fechaHasta = DateTime.Parse(fechaHastaString, CultureInfo.InvariantCulture);

            if (fechaDesde.Date > fechaHasta.Date)
            {
                Alert("Error", "Por favor, ingrese una Fecha Desde a la que desea solicitar la reserva que sea menor a la Fecha Hasta.", AlertType.error);
                return false;
            }

            if (fechaDesde.Date < fechaActual)
            {
                Alert("Error", "Por favor, ingrese una Fecha Desde a la que desea solicitar la reserva que sea menor a la Fecha Actual.", AlertType.error);
                return false;
            }


            if (pnlSeleccionVants.Visible)
            { // Si no está visible es porque la Actividad elegida no admite ni VANTs ni Aeronaves
                if (!chkVant.Checked)
                {
                    bool TieneVants = false;
                    for (int i = 0; i < gvVANTs.Rows.Count; i++)
                    {
                        if (((CheckBox)gvVANTs.Rows[i].FindControl("chkVANTVinculado")).Checked)
                        {
                            TieneVants = true;
                        }
                    }
                    if (!TieneVants)
                    {
                        Alert("Error", "Por favor, ingrese al menos un VANT.", AlertType.error);
                        return false;
                    }
                }
            }

            bool TieneTripulantes = false;
            for (int i = 0; i < gvTripulacion.Rows.Count; i++)
            {
                if (((CheckBox)gvTripulacion.Rows[i].FindControl("chkTripulacionVinculado")).Checked)
                {
                    TieneTripulantes = true;
                }
            }
            if (!TieneTripulantes)
            {
                Alert("Error", "Por favor, ingrese al menos un Tripulante.", AlertType.error);
                return false;
            }
            if (Ubicaciones.Count == 0)
            {
                //Alert("Error", "Por favor, ingrese al menos una ubicación sobre la que desea solicitar la reserva.", AlertType.error);
                //return false;
            }
            return true;
        }

        protected void gvSolicitud_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Session["idRol"].ToString() == "2")
                {
                    LinkButton lnkBtn = (LinkButton)e.Row.FindControl("lnkEditar");
                    lnkBtn.Visible = false;

                }
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

        protected void btnPrueba_Click(object sender, EventArgs e)
        {
            MailController mail = new MailController("SOLICITUD DE REA ACEPTADA", "SE HA ACEPTADO SU SOLICITUD DE REA", false);

            mail.Add("Solicitante 1", "elwachoiaccultrararo@gmail.com");
            mail.Add("Solicitante 2", "joaxqlmelli@gmail.com");

            mail.Add(new Models.Documento().Select(108));

            bool Exito = mail.Enviar();
        }

        protected void btnVerForo_Click(object sender, EventArgs e)
        {
            //Se obtiene id de solicitud
            int id = int.Parse((sender as LinkButton).CommandArgument);
            //Se crea nuevo form ForoMensajes
            ForoMensajes foroMensajes = new ForoMensajes();

            //Creamos String con la direccion de este form para despues el boton volver nos regrese a este form
            string formRedireccion = "/Forms/GestionSolicitudes/GestionSolicitudes.aspx";

            //Se redirecciona a ForoMensajes pasando por parametro (?parametro=valor) el idSolicitud de la tabla y la direccion de este form
            Response.Redirect("/Forms/ForoMensajes/ForoMensajes.aspx?idSolicitud=" + id + "&formRedireccion=" + formRedireccion);



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

        //btnEnviarSolicitud
        protected void enviarSolicitud_Click(object sender, EventArgs e)
        {
            btnGuardar_Click(null, null);
            // CONFIRMACIÓN CON MENSAJE OPCIONAL
            int IdSolicitud = hdnIdSolicitud.Value.ToInt();
            //int IdEstado = 5;
            int IdEstado = hdnIdEstadoAnterior.Value.ToInt();
            string FrmAnterior = "/Forms/GestionSolicitudes.aspx";

            string url = $"/Forms/CambioEstadoSolicitud.aspx?S={IdSolicitud}&E={IdEstado}&frm={FrmAnterior}";

            Response.Redirect(url);
        }

        protected void txtFiltroFechaDesde_TextChanged(object sender, EventArgs e)
        {
            btnFiltrar_Click(null, null);
        }

        protected void txtFiltroFechaHasta_TextChanged(object sender, EventArgs e)
        {
            btnFiltrar_Click(null, null);
        }

        protected void ddlActividad_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnFiltrar_Click(null, null);
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnFiltrar_Click(null, null);
        }

        protected void btnEscanearKML_Click(object sender, EventArgs e)
        {
            if (fupKML.HasFile)
            {
                using (StreamReader reader = new StreamReader(fupKML.FileContent))
                {
                    string PlainKML = reader.ReadToEnd();
                    List<UbicacionRedux> ListUbicaciones = new KMLController(PlainKML).ParsearKML();

                    Ubicaciones = ListUbicaciones;

                    List<string> UbicacionesStr = new List<string>();

                    foreach (UbicacionRedux Ubicacion in ListUbicaciones)
                    {
                        string PuntosGeograficos = "";

                        foreach (PuntoGeografico Punto in Ubicacion.PuntosGeograficos)
                        {
                            PuntosGeograficos += $"Latitud: {Punto.Latitud} / Longitud: {Punto.Longitud} / Altura: {Ubicacion.Altura} | ";
                        }
                        UbicacionesStr.Add(PuntosGeograficos);
                    }

                    rptUbicaciones.DataSource = UbicacionesStr;
                    rptUbicaciones.DataBind();

                    for (int i = 0; i < rptUbicaciones.Items.Count; i++)
                    {
                        ((Label)rptUbicaciones.Items[i].FindControl("lblRptTipoUbicacion")).Text = "Polígono";
                        ((Label)rptUbicaciones.Items[i].FindControl("lblRptDatos")).Text = UbicacionesStr[i];
                    }
                }
            }
        }
    }

    public class UbicacionRedux
    {
        public double Altura { get; set; }

        public List<Models.PuntoGeografico> PuntosGeograficos { get; set; }

        public string IdUbicacion { get; set; }

        public int IdProvincia { get; set; }
    }
}