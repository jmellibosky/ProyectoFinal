using MagicSQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;

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
                        P.Add("IdEstadoSolicitud1", 1),
                        P.Add("IdEstadoSolicitud2", 11),
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
            btnFiltrar_Click(null, null);
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

        protected void ddlSolicitante_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnFiltrar_Click(null, null);
        }

        protected void btnEstadoOperador_Click(object sender, EventArgs e)
        {
            new SP("bd_reapp").Execute("usp_ActualizarEstadoSolicitud",
            P.Add("IdSolicitud", hdnIdSolicitud.Value.ToInt()),
            P.Add("IdEstadoSolicitud", 2),
            P.Add("IdUsuarioCambioEstado", Session["IdUsuario"].ToString().ToInt())
            );
            MostrarListado();
        }
    }
}