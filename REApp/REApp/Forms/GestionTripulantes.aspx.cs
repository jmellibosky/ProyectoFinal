using MagicSQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace REApp.Forms
{
    public partial class GestionTripulantes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aca hacemos el get que si o si es un string porque de object a int no deja
            string idUsuario = Session["IdUsuario"].ToString();
            string idRol = Session["IdRol"].ToString();

            //Estos se usan de esta forma porque son ints, ver si hay mejor forma de hacer el set
            int idRolInt = idRol.ToInt();
            int id = idUsuario.ToInt();

            if (!IsPostBack)
            {
                //Si tiene rol Admin 
                if ( idRolInt == 1)
                {
                    CargarComboSolicitante();
                    btnNuevo.Visible = false;
                }
                //Si tiene rol Operador
                if (idRolInt == 2)
                {
                    CargarComboSolicitante();
                    btnNuevo.Visible = false;
                }
                //Si tiene rol Solicitante
                if (idRolInt == 3)
                {
                    CargarComboSolicitante();
                    ddlSolicitante.SelectedValue = id.ToCryptoID().ToString();
                    ddlSolicitante.Enabled = false;
                    filtrarTripulantesXSolicitante();
                    MostrarListado();
                    btnNuevo.Visible = true;
                }
            }           
        }

        protected void gvTripulantes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Session["idRol"].ToString() == "2")
                {
                    LinkButton lnkBtn = (LinkButton)e.Row.FindControl("btnEliminar");
                    lnkBtn.Visible = false;
                    LinkButton lnkBtn2 = (LinkButton)e.Row.FindControl("btnEditar");
                    lnkBtn2.Visible = false;
                }
            }
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

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
        filtrarTripulantesXSolicitante();
        }

        protected void filtrarTripulantesXSolicitante()
        {
            DataTable dt = null;
            using (SP sp = new SP("bd_reapp"))
            {
                if (!ddlSolicitante.SelectedValue.Equals("#"))
                {
                    dt = sp.Execute("usp_GetTripulacionDeUsuario", P.Add("IdUsuario", ddlSolicitante.SelectedValue.ToIntID()));
                }
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                gvTripulantes.DataSource = dt;
            }
            else
            {
                gvTripulantes.DataSource = null;
            }
            gvTripulantes.DataBind();
        }



        protected void gvTripulantes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int IdTripulacion = e.CommandArgument.ToString().ToInt();
            Models.Tripulacion Tripulacion = new Models.Tripulacion().Select(IdTripulacion);

            if (e.CommandName.Equals("Editar"))
            { // Detalle
                LimpiarModal();

                CargarComboModalSolicitante();

                ddlModalSolicitante.SelectedValue = ddlSolicitante.SelectedValue;
                ddlModalSolicitante.Enabled = false;

                hdnIdTripulacion.Value = IdTripulacion.ToString();
                txtModalApellido.Text = Tripulacion.Apellido;
                txtModalNombre.Text = Tripulacion.Nombre;
                txtModalDNI.Text = Tripulacion.DNI;
                txtModalFechaNacimiento.Text = Tripulacion.FechaNacimiento.ToString();
                txtModalTelefono.Text = Tripulacion.Telefono;
                txtModalCorreo.Text = Tripulacion.Correo;
                int idTripulacion = Tripulacion.IdTripulacion;

                CargarDoc(2, gvCertMedicoTripulante, pnlFuCMTripulante, pnlFechaVencimientoCMTripulante, idTripulacion);
                CargarDoc(3, gvCertCompetenciaTripulante, pnlFUCertCompetenciaTripulante, pnlFechaVencimientoCertCompetenciaTripulante, idTripulacion);

                MostrarABM();
                habilitarDeshabilitarInputs(true);
            }
            if (e.CommandName.Equals("Detalle"))
            {//Detalle
                LimpiarModal();

                CargarComboModalSolicitante();

                ddlModalSolicitante.SelectedValue = ddlSolicitante.SelectedValue;
                ddlModalSolicitante.Enabled = false;

                hdnIdTripulacion.Value = IdTripulacion.ToString();
                txtModalApellido.Text = Tripulacion.Apellido;
                txtModalNombre.Text = Tripulacion.Nombre;
                txtModalDNI.Text = Tripulacion.DNI;
                txtModalFechaNacimiento.Text = Tripulacion.FechaNacimiento.ToString();
                txtModalTelefono.Text = Tripulacion.Telefono;
                txtModalCorreo.Text = Tripulacion.Correo;
                int idTripulacion = Tripulacion.IdTripulacion;

                CargarDoc(2, gvCertMedicoTripulante, pnlFuCMTripulante, pnlFechaVencimientoCMTripulante, idTripulacion);
                CargarDoc(3, gvCertCompetenciaTripulante, pnlFUCertCompetenciaTripulante, pnlFechaVencimientoCertCompetenciaTripulante, idTripulacion);

                MostrarABM();
                habilitarDeshabilitarInputs(false);


            }
            if (e.CommandName.Equals("Eliminar"))
            { // Eliminar

                lblMensajeEliminacion.Text = "¿Desea confirmar la eliminación del tripulante " + Tripulacion.Nombre + " " + Tripulacion.Apellido + "?";
                hdnEliminar.Value = Tripulacion.IdTripulacion.ToString();
                pnlAlertaEliminar.Visible = true;
            }
        }

        protected void habilitarDeshabilitarInputs(bool Bool)
        {
            txtModalApellido.Enabled = Bool;
            txtModalNombre.Enabled = Bool;
            txtModalDNI.Enabled = Bool;
            txtModalFechaNacimiento.Enabled = Bool;
            txtModalTelefono.Enabled = Bool;
            txtModalCorreo.Enabled = Bool;
            btnGuardar.Visible = Bool;
        }


        //Detalle de Documentacion
        private void CargarDoc(int idDoc, GridView gv, Panel panelFileUpload, Panel panelFechaVencimiento, int idTripulacion)
        {
            DataTable dt = null;
            //int idUsuario = 6;
            using (SP sp = new SP("bd_reapp"))
            {
                if (!ddlSolicitante.SelectedItem.Value.Equals("#"))
                {
                    dt = sp.Execute("usp_GetDocumentacionPorTripulanteYTipo",
                        P.Add("IdUsuario", ddlSolicitante.SelectedItem.Value.ToIntID()),
                        P.Add("IdTripulacion", idTripulacion),
                        P.Add("IdTipoDocumento", idDoc));
                }
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                gv.DataSource = dt;
            }
            else
            {
                gv.DataSource = null;
            }
            gv.DataBind();

            if (gv.Rows.Count == 0)
            {
                gv.Visible = false;
                panelFileUpload.Visible = true;
                panelFechaVencimiento.Visible = true;
            }
            else
            {
                gv.Visible = true;
                panelFileUpload.Visible = false;
                panelFechaVencimiento.Visible = false;
            }
        }



        protected bool ValidarCampos()
        {
            if (txtModalNombre.Text.Equals(""))
            {
                txtErrorHeader.Text = "Error";
                txtErrorBody.Text = "Por favor, ingrese el nombre del tripulante.";
                pnlError.Visible = true;
                return false;
            }
            if (txtModalApellido.Text.Equals(""))
            {
                txtErrorHeader.Text = "Error";
                txtErrorBody.Text = "Por favor, ingrese el apellido del tripulante.";
                pnlError.Visible = true;
                return false;
            }
            if (txtModalDNI.Text.Equals(""))
            {
                txtErrorHeader.Text = "Error";
                txtErrorBody.Text = "Por favor, ingrese el DNI del tripulante.";
                pnlError.Visible = true;
                return false;
            }
            if (txtModalFechaNacimiento.Text.Equals(""))
            {
                txtErrorHeader.Text = "Error";
                txtErrorBody.Text = "Por favor, ingrese la fecha de nacimiento del tripulante.";
                pnlError.Visible = true;
                return false;
            }
            if (txtModalFechaNacimiento.Text.ToDateTimeNull() == null)
            {
                txtErrorHeader.Text = "Error";
                txtErrorBody.Text = "Por favor, ingrese una fecha de nacimiento válida.";
                pnlError.Visible = true;
                return false;
            }
            if (txtModalTelefono.Text.Equals("") && txtModalCorreo.Text.Equals(""))
            {
                txtErrorHeader.Text = "Error";
                txtErrorBody.Text = "Por favor, ingrese al menos un dato de contacto del tripulante.";
                pnlError.Visible = true;
                return false;
            }

            pnlError.Visible = false;
            return true;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                Models.Tripulacion Tripulacion = null;
                if (hdnIdTripulacion.Value.Equals(""))
                { // Insert
                    using (Tn tn = new Tn("bd_reapp"))
                    {
                        Tripulacion = new Models.Tripulacion();

                        Tripulacion.Nombre = txtModalNombre.Text;
                        Tripulacion.Apellido = txtModalApellido.Text;
                        Tripulacion.DNI = txtModalDNI.Text;
                        Tripulacion.FechaNacimiento = txtModalFechaNacimiento.Text.ToDateTime();
                        Tripulacion.Telefono = txtModalTelefono.Text;
                        Tripulacion.Correo = txtModalCorreo.Text;
                        Tripulacion.FHAlta = DateTime.Now;
                        Tripulacion.Insert();

                        

                        Models.TripulacionUsuario TripulacionUsuario = new Models.TripulacionUsuario();
                        TripulacionUsuario.IdUsuario = ddlModalSolicitante.SelectedValue.ToIntID();
                        TripulacionUsuario.IdTripulacion = Tripulacion.IdTripulacion;
                        TripulacionUsuario.FHAlta = DateTime.Now;
                        TripulacionUsuario.Insert();


                        //Subida de Archivos
                        int idTripulacion = Tripulacion.IdTripulacion;
                        if (FileUploadCMATrip.HasFile)
                        {
                            uploadMethod(FileUploadCMATrip, 2, idTripulacion);
                        }
                        if (FileUploadCertCompetenciaTripulante.HasFile)
                        {
                            uploadMethod(FileUploadCertCompetenciaTripulante, 3, idTripulacion);
                        }


                    }
                }
                else
                { // Update
                    Tripulacion = new Models.Tripulacion().Select(hdnIdTripulacion.Value.ToInt());
                    Tripulacion.Nombre = txtModalNombre.Text;
                    Tripulacion.Apellido = txtModalApellido.Text;
                    Tripulacion.DNI = txtModalDNI.Text;
                    Tripulacion.FechaNacimiento = txtModalFechaNacimiento.Text.ToDateTime();
                    Tripulacion.Telefono = txtModalTelefono.Text;
                    Tripulacion.Correo = txtModalCorreo.Text;
                    Tripulacion.Update();

                    int idTripulacion = Tripulacion.IdTripulacion;

                    if (FileUploadCMATrip.HasFile)
                    {
                        uploadMethod(FileUploadCMATrip, 2, idTripulacion);
                    }
                    if (FileUploadCertCompetenciaTripulante.HasFile)
                    {
                        uploadMethod(FileUploadCertCompetenciaTripulante, 3, idTripulacion);
                    }
                }

                MostrarListado();
                btnFiltrar_Click(null, null);
            }
        }

        protected void LimpiarModal()
        {
            ddlModalSolicitante.Items.Clear();
            txtModalApellido.Text =
            txtModalNombre.Text =
            txtModalDNI.Text =
            txtModalFechaNacimiento.Text =
            txtModalTelefono.Text =
            txtModalCorreo.Text = "";
            pnlError.Visible = false;
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarModal();

            CargarComboModalSolicitante();

            ddlModalSolicitante.SelectedValue = ddlSolicitante.SelectedValue;
            ddlModalSolicitante.Enabled = false;

            LimpiarGv(gvCertMedicoTripulante, pnlFuCMTripulante, pnlFechaVencimientoCMTripulante);
            LimpiarGv(gvCertCompetenciaTripulante, pnlFUCertCompetenciaTripulante, pnlFechaVencimientoCertCompetenciaTripulante);
            txtFechaDeVencimientoCertCompetenciaTripulante.Value = "";
            txtFechaVencimientoCertMedicoTripulante.Value = "";
            btnGuardar.Visible = true;
            MostrarABM();
        }


        private void LimpiarGv(GridView gv, Panel panelFileUpload, Panel panelFechaVencimiento)
        {
            gvCertCompetenciaTripulante.DataSource = null;
            gvCertCompetenciaTripulante.DataBind();
            gvCertMedicoTripulante.DataSource = null;
            gvCertMedicoTripulante.DataBind();

            if (gv.Rows.Count == 0)
            {
                gv.Visible = false;
                panelFileUpload.Visible = true;
                panelFechaVencimiento.Visible = true;
            }
            else
            {
                gv.Visible = true;
                panelFileUpload.Visible = false;
                panelFechaVencimiento.Visible = false;
            }
        }

        protected void MostrarListado()
        {
            pnlListado.Visible = true;
            btnNuevo.Visible = true;
            pnlABM.Visible = false;
            btnVolver.Visible = false;
            btnGuardar.Visible = true;
        }

        protected void MostrarABM()
        {
            pnlListado.Visible = false;
            btnNuevo.Visible = false;
            pnlABM.Visible = true;
            btnVolver.Visible = true;
            btnGuardar.Visible = true;
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            MostrarListado();
            LimpiarGv(gvCertMedicoTripulante, pnlFuCMTripulante, pnlFechaVencimientoCMTripulante);
            LimpiarGv(gvCertCompetenciaTripulante, pnlFUCertCompetenciaTripulante, pnlFechaVencimientoCertCompetenciaTripulante);
            txtFechaDeVencimientoCertCompetenciaTripulante.Value = "";
            txtFechaVencimientoCertMedicoTripulante.Value = "";
        }

        protected void btnCancelarEliminacion_Click(object sender, EventArgs e)
        {
            pnlAlertaEliminar.Visible = false;
        }

        protected void btnConfirmarEliminacion_Click(object sender, EventArgs e)
        {
            using (SP sp = new SP("bd_reapp"))
            {
                sp.Execute("usp_DarDeBajaTripulacionUsuario",
                    P.Add("IdTripulacion", hdnEliminar.Value.ToInt()),
                    P.Add("IdUsuario", ddlSolicitante.SelectedValue.ToIntID())
                );
            }
            pnlAlertaEliminar.Visible = false;

            btnFiltrar_Click(null, null);
        }

        //Subida de Archivos
        protected void uploadMethod(System.Web.UI.WebControls.FileUpload FileUpload, int idTipoDoc, int idTripulacion)
        {
            //Se obtienen los datos del documento
            string filename = Path.GetFileName(FileUpload.PostedFile.FileName);
            string extension = System.IO.Path.GetExtension(FileUpload.FileName);
            extension = extension.ToLower();
            string contentType = FileUpload.PostedFile.ContentType;
            //(Tamaño del archivo en bytes)
            int tam = FileUpload.PostedFile.ContentLength;
            byte[] bytes;
            using (Stream fs = FileUpload.PostedFile.InputStream)
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    bytes = br.ReadBytes((Int32)fs.Length);
                }
            }
            if (extension == ".pdf")
            {
                //Tamaño de archivo menor a 1Mb
                if (tam <= 5000000)
                {
                    //Insert MagicSQL
                    Models.Documento Documento = null;
                    using (Tn tn = new Tn("bd_reapp"))
                    {
                        //Se crea y guardan los campos de documento
                        Documento = new Models.Documento();

                        Documento.Nombre = filename;
                        Documento.IdSolicitud = null;
                        Documento.Datos = bytes;
                        Documento.Extension = extension;
                        Documento.IdUsuario = ddlSolicitante.SelectedValue.ToIntID();
                        Documento.FHAlta = DateTime.Today;
                        Documento.TipoMIME = contentType;
                        Documento.IdTipoDocumento = idTipoDoc;
                        Documento.IdTripulacion = idTripulacion;
                        //Documento.FHBaja = null;

                        //Certificado Medico
                        if (idTipoDoc == 2)
                        {
                            if (txtFechaVencimientoCertMedicoTripulante.Value != "")
                            {
                                Documento.FHVencimiento = txtFechaVencimientoCertMedicoTripulante.Value.ToDateTime();
                            }
                        }
                        if (idTipoDoc == 3)
                        {
                            if (txtFechaDeVencimientoCertCompetenciaTripulante.Value != "")
                            {
                                Documento.FHVencimiento = txtFechaDeVencimientoCertCompetenciaTripulante.Value.ToDateTime();
                            }
                        }

                        Documento.Insert();
                    }
                    //BindGrid();
                    //txtFechaVencimientoAdmin.Value = "";
                    //LbArchivo.Text = "El archivo se subió con éxito.";
                    //LbArchivo.CssClass = "hljs-string border";
                }
                else
                {
                    //LbArchivo.Text = "El tamaño del archivo debe ser menor a 5Mb";
                }
            }
            else
            {
                //LbArchivo.Text = "Selecciona solo archivos con extensión .PDF";
            }
        }

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            int id = int.Parse((sender as LinkButton).CommandArgument);
            DataTable dt;
            Models.Documento Documento = new Models.Documento().Select(id);
            using (SP sp = new SP("bd_reapp"))
            {
                dt = sp.Execute("__DocumentoSelect_v1",
                    P.Add("IdDocumento", Documento.IdDocumento)
                );
            }
            //Segun la columna en la que trae los datos se pone el segundo []
            string contentType = dt.Rows[0][5].ToString();
            string fileName = dt.Rows[0][4].ToString();
            byte[] bytes = (byte[])dt.Rows[0][7];

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = contentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }

        protected void lnkEliminarArchivo_Click(object sender, EventArgs e)
        {
            int id = int.Parse((sender as LinkButton).CommandArgument);

            Models.Documento Documento = new Models.Documento().Select(id);

            using (SP sp = new SP("bd_reapp"))
            {
                sp.Execute("__DocumentoDelete_v1",
                    P.Add("IdDocumento", Documento.IdDocumento)
                );
            }
        }

    }
}