using MagicSQL;
using REApp.Models;
using REAPP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
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
        private static List<PuntoGeograficoRedux> listaPuntosGeograficos = new List<PuntoGeograficoRedux>();

        protected int GetRol()
        {
            return Session["IdRol"].ToString().ToInt();
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
            }
            if (!IsPostBack)
            {
                //Rol Admin o Operador
                if (idRolInt == 1)
                {
                    btnNuevo.Visible = false;
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
            Ubicaciones = null;
            listaUbicaciones.Clear();
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
                            foreach (UbicacionRedux ubicacion in listaUbicaciones)
                            {
                                Models.Ubicacion Ubicacion = new Models.Ubicacion();


                                Ubicacion.IdSolicitud = Solicitud.IdSolicitud;
                                Ubicacion.Altura = ubicacion.Altura;
                                Ubicacion.IdProvincia = ubicacion.IdProvincia;

                                Ubicacion.Insert(tn);

                                foreach (PuntoGeograficoRedux puntoGeografico in ubicacion.PuntosGeograficos)
                                {
                                    Models.PuntoGeografico PuntoGeografico = new Models.PuntoGeografico();
                                    PuntoGeografico.IdUbicacion = Ubicacion.IdUbicacion;
                                    PuntoGeografico.EsPoligono = puntoGeografico.EsPoligono;
                                    if (!puntoGeografico.EsPoligono)
                                    {
                                        PuntoGeografico.Radio = puntoGeografico.Radio;
                                    }
                                    PuntoGeografico.Latitud = puntoGeografico.Latitud;
                                    PuntoGeografico.Longitud = puntoGeografico.Longitud;

                                    //// INSERT EN TABLA PUNTOGEOGRAFICO
                                    PuntoGeografico.Insert(tn);
                                }
                            }

                            // RECORRO LAS UBICACIONES DE LAS IMPORTADAS EN KML
                            List<UbicacionRedux> AuxUbicaciones = Ubicaciones;
                            foreach (UbicacionRedux ubicacion in AuxUbicaciones)
                            {
                                Models.Ubicacion Ubicacion = new Models.Ubicacion();


                                Ubicacion.IdSolicitud = Solicitud.IdSolicitud;
                                Ubicacion.Altura = ubicacion.Altura;
                                Ubicacion.IdProvincia = (ubicacion.IdProvincia == 0) ? 1 : ubicacion.IdProvincia;

                                Ubicacion.Insert(tn);

                                foreach (PuntoGeograficoRedux puntoGeografico in ubicacion.PuntosGeograficos)
                                {
                                    Models.PuntoGeografico PuntoGeografico = new Models.PuntoGeografico();
                                    PuntoGeografico.IdUbicacion = Ubicacion.IdUbicacion;
                                    PuntoGeografico.EsPoligono = puntoGeografico.EsPoligono;
                                    if (!puntoGeografico.EsPoligono)
                                    {
                                        PuntoGeografico.Radio = puntoGeografico.Radio;
                                    }
                                    PuntoGeografico.Latitud = puntoGeografico.Latitud;
                                    PuntoGeografico.Longitud = puntoGeografico.Longitud;

                                    //// INSERT EN TABLA PUNTOGEOGRAFICO
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
                        foreach (UbicacionRedux ubicacion in listaUbicaciones)
                        {
                            Models.Ubicacion Ubicacion = new Models.Ubicacion();

                            if (ubicacion.estadoUbicacion == 1 || ubicacion.estadoUbicacion == 3) // Si es 1, es nuevo, por lo tanto se hace el insert, si es 3 viene de bd y fue modificado, se debe hacer el update. Si es 2 no hace falta hacer el control, vino de bd pero no se modificó
                            {
                                Ubicacion.IdSolicitud = Solicitud.IdSolicitud;
                                Ubicacion.Altura = ubicacion.Altura;
                                Ubicacion.IdProvincia = ubicacion.IdProvincia;

                                if (ubicacion.estadoUbicacion == 1)
                                {
                                    Ubicacion.Insert(tn);
                                }
                                else
                                {
                                    Ubicacion.IdUbicacion = ubicacion.IdUbicacion.ToInt();
                                    Ubicacion.Update(tn);
                                }

                                int contadorEliminar = 0; // Si se eliminan los todos los puntos geograficos de la ubicación, se elimina la ubicacion
                                int contadorPuntosGeograficos = ubicacion.PuntosGeograficos.Count();

                                foreach (PuntoGeograficoRedux puntoGeografico in ubicacion.PuntosGeograficos)
                                {
                                    Models.PuntoGeografico PuntoGeografico = new Models.PuntoGeografico();
                                    PuntoGeografico.IdUbicacion = Ubicacion.IdUbicacion;
                                    PuntoGeografico.EsPoligono = puntoGeografico.EsPoligono;



                                    if (!puntoGeografico.EsPoligono)
                                    {
                                        PuntoGeografico.Radio = puntoGeografico.Radio;
                                    }
                                    PuntoGeografico.Latitud = puntoGeografico.Latitud;
                                    PuntoGeografico.Longitud = puntoGeografico.Longitud;

                                    //// INSERT EN TABLA PUNTOGEOGRAFICO
                                    if (ubicacion.estadoUbicacion == 1)
                                    {
                                        PuntoGeografico.Insert(tn);
                                    }
                                    else
                                    {
                                        if (!puntoGeografico.eliminarBD)
                                        {
                                            PuntoGeografico.IdPuntoGeografico = puntoGeografico.IdPuntoGeografico;
                                            PuntoGeografico.Update(tn);
                                        }
                                        else
                                        {
                                            PuntoGeografico.Delete(tn);
                                            contadorEliminar++;
                                        }

                                    }

                                }

                                if (contadorEliminar == contadorPuntosGeograficos)
                                {
                                    Ubicacion.Delete(tn);
                                }


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
                Alert("Error", "Su usuario no se encuentra autorizado para crear solicitudes. Su usuario aún  no ha sido validado por el Administrador.", AlertType.error);
            }
            return TieneValidacionEANA;
        }

        protected bool ValidarDocumentacion()
        {
            //Para la validacion, se tiene en cuenta que si tenga al menos un tipo de documento que este aprobado y con la fecha de vencimiento bien

            bool TieneCertificadoMedico = false;
            bool TieneCertificadoCompetencia = false;
            bool TieneCEVANT = false;
            bool TienePoliza = false;
            bool EstaAprobadoCMedico = false;
            bool EstaAprobadoCCompetencia = false;
            bool EstaAprobadoCEVANT = false;
            bool EstaAprobadoPoliza = false;


            int IdUsuario = ddlSolicitante.SelectedValue.ToIntID();

            try
            {
                DataTable dtCertificadoMedico = new SP("bd_reapp").Execute("usp_GetDocumentacionDeUsuario",
                        P.Add("IdUsuario", IdUsuario),
                        P.Add("NombreDocumento", "Certificado Médico")
                );
                DataTable dtCertificadoCompetencia = new SP("bd_reapp").Execute("usp_GetDocumentacionDeUsuario",
                        P.Add("IdUsuario", IdUsuario),
                        P.Add("NombreDocumento", "Certificado de Competencia")
                );
                DataTable dtCEVANT = new SP("bd_reapp").Execute("usp_GetDocumentacionDeUsuario",
                        P.Add("IdUsuario", IdUsuario),
                        P.Add("NombreDocumento", "CEVANT")
                );
                DataTable dtPolizaVANT = new SP("bd_reapp").Execute("usp_GetDocumentacionDeUsuario",
                        P.Add("IdUsuario", IdUsuario),
                        P.Add("NombreDocumento", "Póliza VANT")
                );
                if (dtCertificadoMedico.Rows.Count > 0)
                {
                    TieneCertificadoMedico = true;
                    EstaAprobadoCMedico = dtCertificadoMedico.Rows[0]["Documento"].ToString().ToInt() == 2;
                }
                if (dtCertificadoCompetencia.Rows.Count > 0)
                {
                    TieneCertificadoCompetencia = true;
                    EstaAprobadoCCompetencia = dtCertificadoMedico.Rows[0]["Documento"].ToString().ToInt() == 2;
                }
                if (dtCEVANT.Rows.Count > 0)
                {
                    TieneCEVANT = true;
                    EstaAprobadoCEVANT = dtCertificadoMedico.Rows[0]["Documento"].ToString().ToInt() == 2;
                }
                if (dtPolizaVANT.Rows.Count > 0)
                {
                    TienePoliza = true;
                    EstaAprobadoPoliza = dtCertificadoMedico.Rows[0]["Documento"].ToString().ToInt() == 2;
                }
            }
            catch (Exception ex)
            {
                Alert("Error", "Ocurrió un error en la validación de los documentos.", AlertType.error);
            }
            //Mensajes validacion Fecha de Vencimiento
            if (!TieneCertificadoMedico)
            {
                Alert("Error", "No se encontró Certificado Médico o el mismo ha caducado. Por favor, verifique su documentación.", AlertType.error);
            }
            else if (!EstaAprobadoCMedico)
            {
                Alert("Error", "Su Certificado Medico aún no ha sido aprobado por un operador de EANA. Ud. no podrá crear una solicitud hasta que se realice la validación correspondiente.", AlertType.error);
            }
            else if (!TieneCertificadoCompetencia)
            {
                Alert("Error", "No se encontró Certificado de Competencia o el mismo ha caducado. Por favor, verifique su documentación.", AlertType.error);
            }
            else if (!EstaAprobadoCCompetencia)
            {
                Alert("Error", "Su Certificado de Competencia aún no ha sido aprobado por un operador de EANA. Ud. no podrá crear una solicitud hasta que se realice la validación correspondiente.", AlertType.error);
            }
            else if (!TieneCEVANT)
            {
                Alert("Error", "No se encontró CEVANT o el mismo ha caducado. Por favor, verifique su documentación.", AlertType.error);
            }
            else if (!EstaAprobadoCEVANT)
            {
                Alert("Error", "Su CEVANT aún no ha sido aprobado por un operador de EANA. Ud. no podrá crear una solicitud hasta que se realice la validación correspondiente.", AlertType.error);
            }
            else if (!TienePoliza)
            {
                Alert("Error", "No se encontró Póliza o la misma ha caducado. Por favor, verifique su documentación.", AlertType.error);
            }
            else if (!EstaAprobadoPoliza)
            {
                Alert("Error", "Su Seguro/Póliza aún no ha sido aprobado por un operador de EANA. Ud. no podrá crear una solicitud hasta que se realice la validación correspondiente.", AlertType.error);
            }

            return TieneCertificadoMedico && TieneCertificadoCompetencia && TieneCEVANT && TienePoliza && EstaAprobadoCMedico && EstaAprobadoCCompetencia && EstaAprobadoCEVANT && EstaAprobadoPoliza;
        }


        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            // Si el usuario logueado no es de tipo Solicitante O se validó la documentación
            if (!Session["IdRol"].ToString().Equals("3") || ValidarUsuarioEANA())
            {
                if (ValidarDocumentacion())
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

                    listaPuntosGeograficos.Clear();
                    listaUbicaciones.Clear();


                    MostrarABM();
                }

            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            listaPuntosGeograficos.Clear();
            listaUbicaciones.Clear();
            MostrarListado();
            hdnIdSolicitud.Value = "";
            hdnIdEstadoAnterior.Value = "";
            hdnPoligono.Value = "";
            if (Session["IdRol"].ToString().ToInt() == 2)
            {//Buscar otra forma de hacer
                btnNuevo.Visible = false;
            }
            if (Session["IdRol"].ToString().ToInt() == 1)
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
            if (e.CommandName.Equals("Detalle") || e.CommandName.Equals("Editar"))
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

                txtModalFechaDesde.TextMode = TextBoxMode.Date;
                txtModalFechaHasta.TextMode = TextBoxMode.Date;
                txtModalFechaDesde.Text = Solicitud.FHDesde.ToString("yyyy-MM-dd");
                txtModalFechaHasta.Text = Solicitud.FHHasta.ToString("yyyy-MM-dd");
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
            else if (e.CommandName.Equals("EliminarExplotador"))
            {
                // ELIMINACIÓN POR PARTE DEL EXPLOTADOR
                // NO SOLICITA OBSERVACIÓN
                int IdSolicitud = e.CommandArgument.ToString().ToInt();

                Solicitud Solicitud = new Solicitud().Select(IdSolicitud);

                if (Solicitud != null)
                {
                    Solicitud.FHBaja = DateTime.Now;
                    Solicitud.Update();

                    Alert("Éxito", "La Solicitud ha sido eliminada.", AlertType.success);
                    btnFiltrar_Click(null, null);
                }
            }
            else if (e.CommandName.Equals("EliminarBaja"))
            {
                Solicitud Solicitud = new Solicitud().Select(e.CommandArgument.ToString().ToInt());

                // ESTADOS DONDE EL OPERADOR PUEDE DAR DE BAJA
                List<int> EstadosBaja = new List<int>() { 1, 2, 3, 4, 5, 9 };

                if (EstadosBaja.Contains(Solicitud.IdEstadoSolicitud.Value))
                {
                    // ELIMINACIÓN POR PARTE DEL OPERADOR ANTES DE QUE HAYA SIDO APROBADA
                    int IdSolicitud = Solicitud.IdSolicitud;
                    int IdEstado = 12;
                    string FrmAnterior = "/Forms/GestionSolicitudes.aspx";

                    string url = $"/Forms/CambioEstadoSolicitud.aspx?S={IdSolicitud}&E={IdEstado}&frm={FrmAnterior}";

                    Response.Redirect(url);
                }
                else
                {
                    Alert("Error", "No es posible dar de baja la solicitud en este estado.", AlertType.error);
                }
            }
            else if (e.CommandName.Equals("EliminarBajaExcepcional"))
            {
                // ELIMINACIÓN POR PARTE DEL ADMINISTRADOR
                int IdSolicitud = e.CommandArgument.ToString().ToInt();
                int IdEstado = 13;
                string FrmAnterior = "/Forms/GestionSolicitudes.aspx";

                string url = $"/Forms/CambioEstadoSolicitud.aspx?S={IdSolicitud}&E={IdEstado}&frm={FrmAnterior}";

                Response.Redirect(url);
            }
        }

        protected void GetUbicacionesDeSolicitud(int IdSolicitud)
        {
            //DataTable dt = new SP("bd_reapp").Execute("usp_GetPuntosGeograficosDeSolicitud", P.Add("IdSolicitud", IdSolicitud));

            //if (dt.Rows.Count > 0)
            //{

            //    rptUbicaciones.DataSource = dt;
            //    rptUbicaciones.DataBind();

            //    for (int i = 0; i < rptUbicaciones.Items.Count; i++)
            //    {
            //        ((Label)rptUbicaciones.Items[i].FindControl("lblRptTipoUbicacion")).Text = dt.Rows[i]["TipoUbicacion"].ToString();
            //        ((Label)rptUbicaciones.Items[i].FindControl("lblRptDatos")).Text = dt.Rows[i]["Datos"].ToString();
            //        ((HiddenField)rptUbicaciones.Items[i].FindControl("hdnRptIdUbicacion")).Value = dt.Rows[i]["IdUbicacion"].ToString();
            //        ((HiddenField)rptUbicaciones.Items[i].FindControl("hdnRptIdProvincia")).Value = dt.Rows[i]["IdProvincia"].ToString();
            //    }
            //}



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
                            if (dtPuntosGeograficos.Rows[j]["EsPoligono"].ToString().ToInt() == 1)
                            {
                                puntoGeografico.EsPoligono = true;
                            }
                            else
                            {
                                puntoGeografico.EsPoligono = false;
                                puntoGeografico.Radio = dtPuntosGeograficos.Rows[j]["Radio"].ToString().ToInt();
                                id++;
                            }
                            puntoGeografico.eliminarBD = false;
                            puntosGeograficos.Add(puntoGeografico);

                        }
                    }
                    ubicacion.PuntosGeograficos = puntosGeograficos;

                    listaUbicaciones.Add(ubicacion);
                }
                UpdateUbicacionesTable();


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

            txtAltura.Text =
            txtCircunferenciaLatitud.Text =
            txtCircunferenciaLongitud.Text =
            txtCircunferenciaRadio.Text = "";
            gvPuntosGeograficos.DataSource = null;
        }
        protected void btnCancelarUbicacion_Click(object sender, EventArgs e)
        {
            listaPuntosGeograficos.Clear();
            LimpiarTextBox();

            pnlAgregarUbicacion.Visible = false;
            pnlAgregarPuntoGeograficoYGrilla.Visible = false;

            btnAgregarUbicacion.Visible = btnEscanearKML.Visible = fupKML.Visible = true;

        }

        protected void btnGuardarUbicacion_Click(object sender, EventArgs e)
        {
            if (ValidarGuardarUbicacion())
            {
                if (hdnUbicacionId.Value == "")
                {
                    int idProvincia = ddlProvincia.SelectedValue.ToIntID();


                    UbicacionRedux NuevaUbicacion = new UbicacionRedux();
                    NuevaUbicacion.IdProvincia = ddlProvincia.SelectedValue.ToIntID();
                    NuevaUbicacion.IdUbicacionGrupo = GetNextGrupoUbicacion();
                    NuevaUbicacion.estadoUbicacion = 1; // porque son todas nuevas ubicaciones
                    NuevaUbicacion.Altura = txtAltura.Text.Replace('.', ',').Replace('.', ',').ToDouble();

                    if (chkEsPoligono.Checked)
                    {
                        List<PuntoGeograficoRedux> PuntosGeograficos = new List<PuntoGeograficoRedux>();
                        int? proximoId = GetNextUbicacionId();
                        foreach (PuntoGeograficoRedux puntoGeografico in listaPuntosGeograficos)
                        {
                            PuntoGeograficoRedux agregarPuntoGeografico = new PuntoGeograficoRedux();
                            //agregarPuntoGeografico = puntoGeografico;
                            agregarPuntoGeografico.Id = proximoId;
                            agregarPuntoGeografico.Latitud = puntoGeografico.Latitud;
                            agregarPuntoGeografico.Longitud = puntoGeografico.Longitud;
                            agregarPuntoGeografico.EsPoligono = true;
                            PuntosGeograficos.Add(agregarPuntoGeografico);
                            proximoId++;
                        }

                        NuevaUbicacion.PuntosGeograficos = PuntosGeograficos;
                        listaUbicaciones.Add(NuevaUbicacion);

                    }
                    else
                    {
                        NuevaUbicacion.Altura = txtAltura.Text.Replace('.', ',').Replace('.', ',').ToDouble();

                        List<PuntoGeograficoRedux> PuntosGeograficos = new List<PuntoGeograficoRedux>();

                        PuntoGeograficoRedux PuntoGeografico = new PuntoGeograficoRedux();
                        PuntoGeografico.EsPoligono = false;
                        PuntoGeografico.Latitud = txtCircunferenciaLatitud.Text.Replace('.', ',').Replace('.', ',').ToDouble();
                        PuntoGeografico.Longitud = txtCircunferenciaLongitud.Text.Replace('.', ',').Replace('.', ',').ToDouble();
                        PuntoGeografico.Radio = txtCircunferenciaRadio.Text.Replace('.', ',').Replace('.', ',').ToDouble();
                        PuntoGeografico.Id = GetNextUbicacionId();

                        PuntosGeograficos.Add(PuntoGeografico);

                        NuevaUbicacion.PuntosGeograficos = PuntosGeograficos;
                        listaUbicaciones.Add(NuevaUbicacion);
                    }
                }
                else
                {
                    foreach (UbicacionRedux ubicacionModificar in listaUbicaciones)
                    {
                        foreach (PuntoGeograficoRedux puntoGeograficoModificar in ubicacionModificar.PuntosGeograficos)
                        {
                            if (puntoGeograficoModificar.Id == Convert.ToInt32(hdnUbicacionId.Value)) //Buscamos el objeto Ubicación y Punto Geografico
                            {
                                ubicacionModificar.IdProvincia = ddlProvincia.SelectedValue.ToIntID();
                                ubicacionModificar.Altura = txtAltura.Text.Replace('.', ',').Replace('.', ',').ToDouble();

                                if (!puntoGeograficoModificar.EsPoligono) //Si es Circunferencia
                                {
                                    puntoGeograficoModificar.Latitud = txtCircunferenciaLatitud.Text.Replace('.', ',').Replace('.', ',').ToDouble();
                                    puntoGeograficoModificar.Longitud = txtCircunferenciaLongitud.Text.Replace('.', ',').Replace('.', ',').ToDouble();
                                    puntoGeograficoModificar.Radio = txtCircunferenciaRadio.Text.Replace('.', ',').Replace('.', ',').ToDouble();

                                }
                                else //Si es Poligono
                                {
                                    puntoGeograficoModificar.Latitud = txtPoligonoLatitud.Text.Replace('.', ',').Replace('.', ',').ToDouble();
                                    puntoGeograficoModificar.Longitud = txtPoligonoLongitud.Text.Replace('.', ',').Replace('.', ',').ToDouble();
                                }
                                if ((ubicacionModificar.estadoUbicacion == 2) || (ubicacionModificar.estadoUbicacion == 3))
                                {
                                    ubicacionModificar.estadoUbicacion = 3; //Viene de bd y fue modificado
                                }
                            }
                        }
                    }
                }


                listaPuntosGeograficos.Clear();
                AgregarPuntoGeograficoGridview();
                UpdateUbicacionesTable();

                // Limpiar los campos de entrada
                LimpiarTextBox();

                pnlAgregarUbicacion.Visible = false;
                pnlAgregarPuntoGeograficoYGrilla.Visible = false;

                btnAgregarUbicacion.Visible = btnEscanearKML.Visible = fupKML.Visible = true;
            }
        }

        protected void btnAgregarPuntoGeografico_Click(object sender, EventArgs e)
        {
            pnlAgregarPuntoGeografico.Visible = true;
            pnlAgregarPuntoGeograficoYGrilla.Visible = true;
            btnAgregarPuntoGeografico.Visible = false;
            btnGuardarPuntoGeografico.Visible = true;

            txtPoligonoLatitud.Text =
            txtPoligonoLongitud.Text =
            txtAltura.Text = "";
        }

        protected void btnGuardarPuntoGeografico_Click(object sender, EventArgs e)
        {
            if (ValidarGuardarPuntoGeografico())
            {
                pnlAgregarPuntoGeografico.Visible = false;
                btnAgregarPuntoGeografico.Visible = true;
                btnGuardarPuntoGeografico.Visible = false;
                chkEsPoligono.Enabled = true;

                if (hdnPoligono.Value == "")
                {
                    PuntoGeograficoRedux puntoGeografico = new PuntoGeograficoRedux();
                    puntoGeografico.Latitud = txtPoligonoLatitud.Text.Replace('.', ',').ToDouble();
                    puntoGeografico.Longitud = txtPoligonoLongitud.Text.Replace('.', ',').ToDouble();
                    puntoGeografico.EsPoligono = true;
                    puntoGeografico.Id = GetNextPuntoGeograficoParcialId();

                    listaPuntosGeograficos.Add(puntoGeografico);
                }
                else
                {
                    foreach (PuntoGeograficoRedux puntoGeografico in listaPuntosGeograficos)
                    {
                        if (puntoGeografico.Id == Convert.ToInt32(hdnPoligono.Value))
                        {
                            puntoGeografico.Latitud = txtPoligonoLatitud.Text.Replace('.', ',').ToDouble();
                            puntoGeografico.Longitud = txtPoligonoLongitud.Text.Replace('.', ',').ToDouble();
                        }
                    }

                    hdnPoligono.Value = string.Empty;
                }



                AgregarPuntoGeograficoGridview();

            }
        }

        protected void chkEsPoligono_CheckedChanged(object sender, EventArgs e)
        {
            pnlAgregarPoligono.Visible = chkEsPoligono.Checked;
            pnlAgregarCircunferencia.Visible = !chkEsPoligono.Checked;
            listaPuntosGeograficos.Clear();
        }

        protected void AgregarPuntoGeograficoGridview()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Latitud");
            dt.Columns.Add("Longitud");

            foreach (PuntoGeograficoRedux puntoGeografico in listaPuntosGeograficos)
            {
                string Id = puntoGeografico.Id.ToString();
                string Latitud = puntoGeografico.Latitud.ToString();
                string Longitud = puntoGeografico.Longitud.ToString();
                dt.Rows.Add(Id, Latitud, Longitud);
            }

            gvPuntosGeograficos.DataSource = dt;
            gvPuntosGeograficos.DataBind();
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
            return true;
        }

        protected bool ValidarGuardarUbicacion()
        {
            if (!chkEsPoligono.Checked)
            {
                if (txtAltura.Text.Equals(""))
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
            else
            {
                if (txtAltura.Text.Equals(""))
                {
                    Alert("Error", "Por favor, ingrese altura.", AlertType.error);
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

                        foreach (PuntoGeograficoRedux Punto in Ubicacion.PuntosGeograficos)
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

        ////////////////////TABLA UBICACIONES/////////////////////


        protected void lnkModificar_Click(object sender, EventArgs e)
        {

            LinkButton btnModificar = (LinkButton)sender;
            int ubicacionId = Convert.ToInt32(btnModificar.CommandArgument);
            UbicacionRedux ubicacion = ObtenerUbicacion(ubicacionId);

            pnlAgregarUbicacion.Visible = true;
            pnlAgregarPuntoGeograficoYGrilla.Visible = true;
            btnAgregarUbicacion.Visible = false;

            txtAltura.Text = ubicacion.Altura.ToString();
            ddlProvincia.SelectedValue = ubicacion.IdProvincia.ToCryptoID();
            hdnUbicacionId.Value = ubicacion.PuntosGeograficos[0].Id.ToString();

            if (!ubicacion.PuntosGeograficos[0].EsPoligono) //Obtenemos el primer punto geografico de la ubicación para saber si es Circunferencia o Poligono. En este caso si la condición es true, se trata de Circunferencia
            {
                txtCircunferenciaLatitud.Text = ubicacion.PuntosGeograficos[0].Latitud.ToString();
                txtCircunferenciaLongitud.Text = ubicacion.PuntosGeograficos[0].Longitud.ToString();
                txtCircunferenciaRadio.Text = ubicacion.PuntosGeograficos[0].Radio.ToString();
            }
            else
            {
                foreach (PuntoGeograficoRedux puntoGeografico in ubicacion.PuntosGeograficos)
                {
                    if (puntoGeografico.Id == ubicacionId)
                    {

                        mostrarModificarPoligono();

                        txtPoligonoLatitud.Text = puntoGeografico.Latitud.ToString();
                        txtPoligonoLongitud.Text = puntoGeografico.Longitud.ToString();
                        break;
                    }
                }

            }

            chkEsPoligono.Enabled = false;
        }

        protected void mostrarModificarPoligono()
        {
            pnlAgregarUbicacion.Visible = true;
            btnAgregarUbicacion.Visible = btnEscanearKML.Visible = fupKML.Visible = false;

            gvPuntosGeograficos.DataSource = null;


            pnlAgregarPuntoGeograficoYGrilla.Visible = true;
            pnlAgregarPuntoGeografico.Visible = true;
            btnGuardarPuntoGeografico.Visible = true;
            btnAgregarPuntoGeografico.Visible = false;
            btnGuardarPuntoGeografico.Visible = false;
        }

        protected void lnkEliminar_Click(object sender, EventArgs e)
        {
            LinkButton btnEliminar = (LinkButton)sender;
            int ubicacionId = Convert.ToInt32(btnEliminar.CommandArgument);
            EliminarUbicacion(ubicacionId);

            UpdateUbicacionesTable();
        }

        private void EliminarUbicacion(int? puntoGeograficoId)
        {
            bool encontrado = false;

            for (int i = listaUbicaciones.Count - 1; i >= 0; i--)
            {
                if (!encontrado)
                {
                    UbicacionRedux ubicacion = listaUbicaciones[i];

                    for (int j = ubicacion.PuntosGeograficos.Count - 1; j >= 0; j--)
                    {
                        PuntoGeograficoRedux puntoGeografico = ubicacion.PuntosGeograficos[j];

                        if (puntoGeografico.Id == puntoGeograficoId)
                        {
                            if (ubicacion.estadoUbicacion == 1) // Si es nuevo, se lo elimina de la lista
                            {
                                ubicacion.PuntosGeograficos.RemoveAt(j);

                                if (ubicacion.PuntosGeograficos.Count == 0)
                                {
                                    listaUbicaciones.RemoveAt(i);
                                }
                            }
                            else //si viene de la bd, se marca que cambia el estado de la ubicación y se prende la bandera en el punto geografico a eliminar
                            {
                                ubicacion.estadoUbicacion = 3;
                                puntoGeografico.eliminarBD = true;
                            }
                            encontrado = true;
                            break;
                        }
                    }

                }
            }
            ActualizarIndicesTabla();
            UpdateUbicacionesTable();

            Alert("¡ATENCIÓN!", "Punto Geográfico Eliminado", AlertType.warning);
        }

        private void ActualizarIndicesTabla()
        {
            int? nuevoId = 1;

            foreach (UbicacionRedux ubicacion in listaUbicaciones)
            {
                foreach (PuntoGeograficoRedux puntoGeografico in ubicacion.PuntosGeograficos)
                {
                    if (puntoGeografico.eliminarBD)
                    {
                        puntoGeografico.Id = null;
                    }
                    else
                    {
                        puntoGeografico.Id = nuevoId;
                        nuevoId++;
                    }

                }
            }
        }

        private void UpdateUbicacionesTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("IdUbicacion");
            dt.Columns.Add("Poligono");
            dt.Columns.Add("Latitud");
            dt.Columns.Add("Longitud");
            dt.Columns.Add("Radio");
            dt.Columns.Add("Altura");
            dt.Columns.Add("idProvincia");
            dt.Columns.Add("IdUbicacionGrupo");
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


                        dt.Rows.Add(nuevoId, nuevoIdUbicacion, nuevoPoligono, nuevoLatitud, nuevoLonguitud, nuevoRadio, nuevoAltura, nuevoIdProvincia, nuevoIdUbicacionGrupo, nuevoEstadoUbicacion, nuevoIdPuntoGeografico, nuevoEliminarBD);

                    }

                }
            }
            gridUbicaciones.DataSource = dt;
            gridUbicaciones.DataBind();

            //Formato de tabla
            foreach (GridViewRow row in gridUbicaciones.Rows)
            {
                int idProvincia = Convert.ToInt32(row.Cells[7].Text); // Obtener el número de idProvincia

                string provincia = ddlProvincia.Items[idProvincia - 1].Text;//Obtener el valor string 

                row.Cells[7].Text = provincia;

                if (row.Cells[2].Text == "False")
                {
                    row.Cells[2].Text = "Circunferencia";
                }
                else
                {
                    row.Cells[2].Text = "Poligono";
                }

                if (Convert.ToInt32(row.Cells[8].Text) % 2 == 0)
                {
                    row.BackColor = System.Drawing.Color.FromArgb(193, 193, 193);
                }
                else
                {
                    row.BackColor = System.Drawing.Color.White;
                }


                if (row.Cells[9].Text == "1")
                {
                    row.Cells[9].Text = "Nuevo";
                }
                else if (row.Cells[9].Text == "2")
                {
                    row.Cells[9].Text = "Base de Datos";
                }
                else
                {
                    row.Cells[9].Text = "Base de Datos Modificado";
                }


            }
        }

        private void LimpiarTextBox()
        {
            chkEsPoligono.Enabled = true;
            txtCircunferenciaLatitud.Text = string.Empty;
            txtCircunferenciaLongitud.Text = string.Empty;
            txtCircunferenciaRadio.Text = string.Empty;
            txtAltura.Text = string.Empty;
            hdnUbicacionId.Value = string.Empty;
            hdnPoligono.Value = string.Empty;
            txtAltura.Text = string.Empty;
            txtPoligonoLatitud.Text = string.Empty;
            txtPoligonoLongitud.Text = string.Empty;
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

        private int? GetNextPuntoGeograficoParcialId()
        {
            // Genera un nuevo ID automáticamente para una ubicación
            // En este ejemplo, incrementamos un contador y lo devolvemos

            int? maxId = 0;

            foreach (PuntoGeograficoRedux puntoGeografico in listaPuntosGeograficos)
            {
                if (puntoGeografico.Id > maxId)
                {
                    maxId = puntoGeografico.Id;
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
        private UbicacionRedux ObtenerUbicacion(int puntoGeograficoId)
        {
            // Busca una ubicación por su ID en la lista de ubicaciones
            foreach (UbicacionRedux ubicacion in listaUbicaciones)
            {
                foreach (PuntoGeograficoRedux puntoGeografico in ubicacion.PuntosGeograficos)
                {
                    if (puntoGeografico.Id == puntoGeograficoId)
                    {
                        return ubicacion;
                    }

                }
            }

            return null;

        }

        protected void lnkModificarPuntoGeografico_Click(object sender, EventArgs e)
        {
            LinkButton btnModificarPuntoGeografico = (LinkButton)sender;
            int puntoGeoIdParcial = Convert.ToInt32(btnModificarPuntoGeografico.CommandArgument);
            ModificarPuntoGeograficoParcial(puntoGeoIdParcial);
        }

        private void ModificarPuntoGeograficoParcial(int puntoGeoIdParcial)
        {
            foreach (PuntoGeograficoRedux puntoGeografico in listaPuntosGeograficos)
            {
                if (puntoGeografico.Id == puntoGeoIdParcial)
                {

                    pnlAgregarPuntoGeografico.Visible = true;
                    btnAgregarPuntoGeografico.Visible = false;
                    btnGuardarPuntoGeografico.Visible = true;
                    txtPoligonoLatitud.Text = puntoGeografico.Latitud.ToString();
                    txtPoligonoLongitud.Text = puntoGeografico.Longitud.ToString();
                    hdnPoligono.Value = puntoGeografico.Id.ToString();
                    chkEsPoligono.Enabled = false;
                    break;

                }
            }
        }

        protected void lnkEliminarPuntoGeografico_Click(object sender, EventArgs e)
        {
            LinkButton btnEliminarPuntoGeografico = (LinkButton)sender;
            int puntoGeoIdParcial = Convert.ToInt32(btnEliminarPuntoGeografico.CommandArgument);
            EliminarPuntoGeograficoParcial(puntoGeoIdParcial);
        }

        private void EliminarPuntoGeograficoParcial(int puntoGeoIdParcial)
        {
            for (int i = listaPuntosGeograficos.Count - 1; i >= 0; i--)
            {
                PuntoGeograficoRedux puntoGeografico = listaPuntosGeograficos[i];

                if (puntoGeografico.Id == puntoGeoIdParcial)
                {
                    listaPuntosGeograficos.RemoveAt(i);
                    break;
                }

            }

            ActualizarIndicesTablaPuntosGeograficos();
            AgregarPuntoGeograficoGridview();
        }

        private void ActualizarIndicesTablaPuntosGeograficos()
        {
            int nuevoId = 1;

            foreach (PuntoGeograficoRedux puntoGeografico in listaPuntosGeograficos)
            {
                puntoGeografico.Id = nuevoId;
                nuevoId++;
            }

        }
    }

    public class UbicacionRedux
    {

        public double Altura { get; set; }

        public List<PuntoGeograficoRedux> PuntosGeograficos { get; set; }

        public string IdUbicacion { get; set; } //este es el estado que va a venir de la bd

        public int IdProvincia { get; set; }

        public int estadoUbicacion { get; set; } //1 si es nuevo, 2 si viene de la bd, 3 si hay que cambiar de la bd

        public int IdUbicacionGrupo { get; set; } //para las nuevas ubicaciones, se tiene un id para identificar el grupo, el cual va aumentando
    }

    public class PuntoGeograficoRedux
    {
        public int? Id { get; set; } //Id de la lista
        public int IdPuntoGeografico { get; set; }

        public int IdUbicacion { get; set; }

        public bool EsPoligono { get; set; }

        public double Radio { get; set; }

        public double Latitud { get; set; }

        public double Longitud { get; set; }

        public bool eliminarBD { get; set; } //false si no se elimina, true si elimina
    }
}