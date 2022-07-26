﻿using MagicSQL;
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

            //Aca hacemos el get que si o si es un string porque de object a int no deja
            string idUsuario = Session["IdUsuario"].ToString();
            string idRol = Session["IdRol"].ToString();

            //Estos se usan de esta forma porque son ints, ver si hay mejor forma de hacer el set
            int idRolInt = idRol.ToInt();
            int id = idUsuario.ToInt();

            if (IsPostBack)
            {
                BindGrid();
                //BindGrid();
                //LbArchivo.Text = "";
            }
            if (!IsPostBack)
            {
                if (idRolInt == 1)
                {
                    CargarComboSolicitante();
                    BindGrid();
                }
                if (idRolInt == 3)
                {
                    CargarComboSolicitante();
                    ddlSolicitante.SelectedValue = id.ToCryptoID().ToString();
                    ddlSolicitante.Enabled = false;
                    BindGrid();
                }
                //CargarComboSolicitante();
                //BindGrid();

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
            txtModalNombreUsuario.Enabled = false;
            txtModalApellidoUsuario.Enabled = false;
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
    }
}