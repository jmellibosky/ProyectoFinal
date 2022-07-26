using MagicSQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace REApp.Forms
{
    public partial class GestionSolicitudes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                BindGrid();
            }
            if (!IsPostBack)
            {
                CargarComboSolicitante();
                BindGrid();
            }


            //CargarComboSolicitante();
            //BindGrid();
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
            txtModalNombreUsuario.Text =
            txtModalApellidoUsuario.Text =
            txtModalNombreSolicitud.Text =
            txtModalEstadoSolicitud.Text =
            txtModalObservaciones.Text = "";
            txtModalFechaUltimaActualizacion.Text = "-";
            txtModalFechaUltimaActualizacion.Enabled = false;
            txtModalFechaSolicitud.Enabled = false;
            txtModalEstadoSolicitud.Enabled = false;
            pnlAgregarCircunferencia.Visible = true;
            pnlAgregarPoligono.Visible = false;
            pnlAgregarUbicacion.Visible = false;
            pnlAgregarPuntoGeografico.Visible = false;
            rptUbicaciones.DataSource = null;
        }



        protected void lnkVerDetalles_Click(object sender, EventArgs e)
        {

        }

        protected void lnkDarDeBajaSolicitud_Click(object sender, EventArgs e)
        {

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Models.Solicitud Solicitud = null;
            if (hdnIdSolicitud.Value.Equals(""))
            { // Insert
                using (Tn tn = new Tn("bd_reapp"))
                {
                    Solicitud = new Models.Solicitud();
                    Solicitud.Nombre = txtModalNombreSolicitud.Text;
                    //Ver si funciona:
                    Solicitud.IdModalidad = ddlModalModalidad.SelectedValue.ToIntID();
                    Solicitud.IdUsuario = ddlModalSolicitante.SelectedValue.ToIntID();
                    Solicitud.FHSolicitud = DateTime.Now;
                    Solicitud.IdEstadoSolicitud = 1;
                    Solicitud.Observaciones = txtModalObservaciones.Text;
                    Solicitud.Insert();
                }
            }
            else
            { // Update
                Solicitud = new Models.Solicitud().Select(hdnIdSolicitud.Value.ToInt());
                Solicitud.Nombre = txtModalNombreSolicitud.Text;
                //Ver si funciona:
                Solicitud.IdModalidad = ddlModalModalidad.SelectedValue.ToIntID();

                Solicitud.IdEstadoSolicitud = 1;
                Solicitud.Observaciones = txtModalObservaciones.Text;
                Solicitud.FHUltimaActualizacionEstado = DateTime.Now.ToString();
                Solicitud.Update();
            }

            MostrarListado();
            btnFiltrar_Click(null, null);
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
        }

        protected void MostrarListado()
        {
            pnlListado.Visible = true;
            btnNuevo.Visible = true;
            pnlABM.Visible = false;
            btnVolver.Visible = false;
        }

        protected void MostrarABM()
        {
            pnlListado.Visible = false;
            btnNuevo.Visible = false;
            pnlABM.Visible = true;
            btnVolver.Visible = true;
        }

        //True p/visible, False p/ invisible
        protected void OcultarMostrarPanelesABM(bool valor)
        {
            pnlModalEstadoSolicitud.Visible = valor;
            pnlModalFechaSolicitud.Visible = valor;
            pnlModalFechaUltimaActualizacion.Visible = valor;
            pnlModalApellidoUsuario.Visible = valor;
            pnlModalNombreUsuario.Visible = valor;
            ddlModalSolicitante.Visible = !valor;
        }

        //True para Habilitar, False p/ Deshabilitar
        protected void HabilitarDeshabilitarTxts(bool valor)
        {
            //Ocultar en detalle y Mostrar en nuevo
            ddlModalActividad.Enabled = valor;
            ddlModalModalidad.Enabled = valor;
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

            if (e.CommandName.Equals("Detalle"))
            { // Detalle
                LimpiarModal();
                OcultarMostrarPanelesABM(true);
                HabilitarDeshabilitarTxts(false);

                CargarComboModalSolicitante();
                ddlModalSolicitante.SelectedValue = ddlSolicitante.SelectedValue;
                ddlModalSolicitante.Enabled = false;

                CargarComboModalActividades();
                CargarComboModalSoloModalidades();

                ddlModalActividad.SelectedValue = Modalidad.IdActividad.ToCryptoID().ToString();
                ddlModalModalidad.SelectedValue = Solicitud.IdModalidad.ToCryptoID().ToString();

                hdnIdSolicitud.Value = IdSolicitud.ToString();
                txtModalNombreSolicitud.Text = Solicitud.Nombre;

                txtModalNombreUsuario.Text = Usuario.Nombre;
                txtModalApellidoUsuario.Text = Usuario.Apellido;
                txtModalObservaciones.Text = Solicitud.Observaciones;
                txtModalEstadoSolicitud.Text = Estado.Nombre;

                //Se deshabilita los txts q faltan


                string FHActualiz = Solicitud.FHUltimaActualizacionEstado;
                if(FHActualiz != null)
                if (FHActualiz != null)
                {
                    txtModalFechaUltimaActualizacion.Text = FHActualiz;
                }
                txtModalFechaSolicitud.Text = Solicitud.FHSolicitud.ToString();

                MostrarABM();
            }
            if (e.CommandName.Equals("Editar"))
            {
                btnGuardar.Visible = true;
                LimpiarModal();
                OcultarMostrarPanelesABM(true);
                HabilitarDeshabilitarTxts(true);

                CargarComboModalSolicitante();
                ddlModalSolicitante.SelectedValue = ddlSolicitante.SelectedValue;
                ddlModalSolicitante.Enabled = false;

                CargarComboModalActividades();
                CargarComboModalSoloModalidades();

                ddlModalActividad.SelectedValue = Modalidad.IdActividad.ToCryptoID().ToString();
                ddlModalModalidad.SelectedValue = Solicitud.IdModalidad.ToCryptoID().ToString();

                hdnIdSolicitud.Value = IdSolicitud.ToString();
                txtModalNombreSolicitud.Text = Solicitud.Nombre;

                txtModalNombreUsuario.Text = Usuario.Nombre;
                txtModalApellidoUsuario.Text = Usuario.Apellido;
                txtModalObservaciones.Text = Solicitud.Observaciones;
                txtModalEstadoSolicitud.Text = Estado.Nombre;

                string FHActualiz = Solicitud.FHUltimaActualizacionEstado;
                if (FHActualiz != null)
                {
                    txtModalFechaUltimaActualizacion.Text = FHActualiz;
                }
                txtModalFechaSolicitud.Text = Solicitud.FHSolicitud.ToString();

                MostrarABM();
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

            if (!chkVant.Checked)
            {
                using (SP sp = new SP("bd_reapp"))
                {
                    DataTable dt = sp.Execute("usp_VantConsultar", P.Add("IdUsuario", ddlSolicitante.SelectedValue.ToIntID()));
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
        }

        protected void btnAgregarUbicacion_Click(object sender, EventArgs e)
        {
            pnlAgregarUbicacion.Visible = true;
        }

        protected void btnGuardarUbicacion_Click(object sender, EventArgs e)
        {
            pnlAgregarUbicacion.Visible = false;
            AgregarUbicacionRepeater();
        }

        protected void btnAgregarPuntoGeografico_Click(object sender, EventArgs e)
        {
            pnlAgregarPuntoGeografico.Visible = true;
        }

        protected void btnGuardarPuntoGeografico_Click(object sender, EventArgs e)
        {
            AgregarPuntoGeograficoGridview();
            txtPoligonoLatitud.Text =
            txtPoligonoLongitud.Text =
            txtPoligonoAltura.Text = "";
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
                string Latitud = gvPuntosGeograficos.Rows[i].Cells[0].Text;
                string Longitud = gvPuntosGeograficos.Rows[i].Cells[1].Text;
                string Altura = gvPuntosGeograficos.Rows[i].Cells[2].Text;
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
                    string Latitud = gvPuntosGeograficos.Rows[i].Cells[0].Text;
                    string Longitud = gvPuntosGeograficos.Rows[i].Cells[1].Text;
                    string Altura = gvPuntosGeograficos.Rows[i].Cells[2].Text;

                    NuevaDatos += "Latitud: " + Latitud + " - Longitud: " + Longitud + " - Altura: " + Altura + " | ";
                }
            }
            else
            {
                NuevaDatos = "Latitud: " + txtCircunferenciaLatitud.Text + " - Longitud: " + txtCircunferenciaLongitud.Text + " - Altura: " + txtCircunferenciaAltura.Text + " - Radio: " + txtCircunferenciaRadio.Text;
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
    }
}