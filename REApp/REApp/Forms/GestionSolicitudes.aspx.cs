using MagicSQL;
using REApp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                    btnNuevo.Visible = false;
                    btnEstadoOperador.Visible = true;
                }
                //Rol Solicitante
                if (idRolInt == 3)
                {
                    CargarComboSolicitante();
                    ddlSolicitante.SelectedValue = id.ToCryptoID().ToString();
                    ddlSolicitante.Enabled = false;
                    BindGrid();

                    btnNuevo.Visible = true;
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
                    dt = sp.Execute("usp_GetSolicitudes", P.Add("IdUsuario", ddlSolicitante.SelectedItem.Value.ToIntID()));
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
            pnlAgregarCircunferencia.Visible = true;
            pnlAgregarPoligono.Visible = false;
            pnlAgregarUbicacion.Visible = false;
            pnlAgregarPuntoGeografico.Visible = false;
            btnAgregarUbicacion.Visible = true;
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
                btnFiltrar_Click(null, null);
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarModal();
            OcultarMostrarPanelesABM(false);
            HabilitarDeshabilitarTxts(true);

            CargarComboModalSolicitante();
            CargarComboModalActividades();
            int idActividad = ddlModalActividad.SelectedItem.Value.ToIntID();
            CargarComboModalModalidades(idActividad);

            ddlModalSolicitante.SelectedValue = ddlSolicitante.SelectedValue;
            ddlModalSolicitante.Visible = true;
            ddlModalSolicitante.Enabled = false;
            btnGuardar.Visible = true;

            chkVant.Checked = false;
            chkVant_CheckedChanged(null, null);

            MostrarABM();
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            MostrarListado();
            hdnIdSolicitud.Value = "";
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
        }

        protected void MostrarABM()
        {
            pnlListado.Visible = false;
            btnNuevo.Visible = false;
            pnlABM.Visible = true;
            btnVolver.Visible = true;

            GetTripulantesDeUsuario(Session["IdUsuario"].ToString().ToInt());
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

            if (e.CommandName.Equals("Detalle"))
            { // Detalle
                btnGenerarKMZ.Visible = true;
                HabilitarDeshabilitarTxts(false);
            }
            if (e.CommandName.Equals("Editar"))
            {
                btnGenerarKMZ.Visible = true;
                btnGuardar.Visible = true;
                HabilitarDeshabilitarTxts(true);
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
                        P.Add("IdUsuario", ddlSolicitante.SelectedValue.ToIntID())
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
            btnAgregarUbicacion.Visible = false;

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
                btnAgregarUbicacion.Visible = true;
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
                                        Ubicacion.Altura = Altura.ToDouble();

                                        Models.PuntoGeografico PuntoGeografico = new Models.PuntoGeografico();
                                        PuntoGeografico.EsPoligono = true;
                                        PuntoGeografico.Latitud = Latitud.ToDouble();
                                        PuntoGeografico.Longitud = Longitud.ToDouble();

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
                    Ubicacion.Altura = txtCircunferenciaAltura.Text.ToDouble();

                    List<Models.PuntoGeografico> PuntosGeograficos = new List<Models.PuntoGeografico>();

                    Models.PuntoGeografico PuntoGeografico = new Models.PuntoGeografico();
                    PuntoGeografico.EsPoligono = false;
                    PuntoGeografico.Latitud = txtCircunferenciaLatitud.Text.ToDouble();
                    PuntoGeografico.Longitud = txtCircunferenciaLongitud.Text.ToDouble();
                    PuntoGeografico.Radio = txtCircunferenciaRadio.Text.ToDouble();

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
                return false;
            }
            if (txtPoligonoLongitud.Text.Equals(""))
            {
                return false;
            }
            if (txtPoligonoAltura.Text.Equals(""))
            {
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
                    return false;
                }
                if (txtCircunferenciaLatitud.Text.Equals(""))
                {
                    return false;
                }
                if (txtCircunferenciaLongitud.Text.Equals(""))
                {
                    return false;
                }
                if (txtCircunferenciaRadio.Text.Equals(""))
                {
                    return false;
                }
            }
            return true;
        }

        protected bool ValidarGuardar()
        {
            if (txtModalNombreSolicitud.Text.Equals(""))
            {
                return false;
            }
            if (txtModalFechaDesde.Text.Equals(""))
            {
                return false;
            }
            if (txtModalFechaHasta.Text.Equals(""))
            {
                return false;
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

            ////Aca meto codigo temporal para generar un archivo en el disco C en mi escritorio, cambien porque no les van a andar
            //string path = @"C:\Users\benja\Desktop\kmls\Testing.kml";
            //try
            //{
            //    using (FileStream fileSystemTest = File.Create(path))
            //    {
            //        //Uso todo el System porque no me lo deja usar en el comienzo del archivo ???
            //        byte[] info = System.Text.Encoding.ASCII.GetBytes(kml);
            //        fileSystemTest.Write(info, 0, info.Length);

            //    }
            //}
            //catch
            //{

            //}
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
    }

    public class UbicacionRedux
    {
        public double Altura { get; set; }

        public List<Models.PuntoGeografico> PuntosGeograficos { get; set; }
    }
}