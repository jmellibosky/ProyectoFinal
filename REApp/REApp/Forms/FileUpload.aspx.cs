using MagicSQL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static REApp.Navegacion;

namespace REApp.Forms
{
    public partial class FileUpload : System.Web.UI.Page
    {
        [Obsolete]
        protected void Page_Load(object sender, EventArgs e)
        {
            //**COMENTAR desde ACA**

            //Aca hacemos el get que si o si es un string porque de object a int no deja
            string idUsuario = Session["IdUsuario"].ToString();
            string idRol = Session["IdRol"].ToString();

            //Estos se usan de esta forma porque son ints, ver si hay mejor forma de hacer el set
            int idRolInt = idRol.ToInt();
            int id = idUsuario.ToInt();
            //**HASTA ACA**

            if (IsPostBack)
            {
                LbArchivo.Text = "";
                if (idRolInt == 1)
                {
                    BindGrid();
                    MostrarPanelAdmin();
                }
                if (idRolInt == 2)
                {
                    BindGrid();
                    MostrarPanelAdmin();
                    pnlFuAdmin.Visible = false;
                }
                if (idRolInt == 3)
                {
                    CargarComboSolicitante();
                    ddlSolicitante.SelectedValue = id.ToCryptoID().ToString();
                    ddlSolicitante.Enabled = false;
                    MostrarPanelSolicitante();
                    //Certificado Medico
                    //CargarDoc(2, gvCertMedico, pnlFuCM, pnlFechaVencimientoCM, pnlBtnSubirArchivoCM, 0);
                    //Cert Competencia
                    //CargarDoc(3, gvCertCompetencia, pnlFUCertCompetencia, pnlFechaVencimientoCertCompetencia, pnlBtnSubirArchivoCertCompetencia, 0);
                    //CEVANT
                    //CargarDoc(4, gvCevant, pnlFUCevant, pnlFechaVencimientoCevant, pnlBtnSubirArchivoCevant, 0);
                    //Poliza
                    //CargarDoc(6, gvSeguroPoliza, pnlFuSeguroPoliza, pnlFechaVencimientoSeguroPoliza, pnlBtnSubirArchivoSeguroPoliza, 0);
                }
                
            }
            if (!IsPostBack)
            {
                if (idRolInt == 1)
                {
                    CargarComboSolicitante();
                    BindGrid();
                    MostrarPanelAdmin();
                }
                if (idRolInt == 2)
                {
                    CargarComboSolicitante();
                    BindGrid();
                    MostrarPanelAdmin();
                    pnlFuAdmin.Visible = false;
                }
                if (idRolInt == 3)
                {
                    CargarComboSolicitante();
                    ddlSolicitante.SelectedValue = id.ToCryptoID().ToString();
                    ddlSolicitante.Enabled = false;
                    MostrarPanelSolicitante();
                    //Certificado Medico
                    CargarDoc(2, gvCertMedico, pnlFuCM, pnlFechaVencimientoCM, pnlBtnSubirArchivoCM, 0);
                    //Cert Competencia
                    CargarDoc(3, gvCertCompetencia, pnlFUCertCompetencia, pnlFechaVencimientoCertCompetencia, pnlBtnSubirArchivoCertCompetencia, 0);
                    //CEVANT
                    CargarDoc(4, gvCevant, pnlFUCevant, pnlFechaVencimientoCevant, pnlBtnSubirArchivoCevant, 0);
                    //Poliza
                    CargarDoc(6, gvSeguroPoliza, pnlFuSeguroPoliza, pnlFechaVencimientoSeguroPoliza, pnlBtnSubirArchivoSeguroPoliza, 0);
                }
            }
        }

        private void MostrarPanelSolicitante()
        {
            PanelSolicitante.Visible = true;
        }

        private void MostrarPanelAdmin()
        {
            PanelAdmin.Visible = true;
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

        private void BindGrid()
        {
            DataTable dt = null;
            using (SP sp = new SP("bd_reapp"))
            {
                if (!ddlSolicitante.SelectedItem.Value.Equals("#"))
                {
                    dt = sp.Execute("usp_GetDocumentosSolicitante", P.Add("IdUsuario", ddlSolicitante.SelectedItem.Value.ToIntID()));
                }
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                gvArchivos.DataSource = dt;
            }
            else
            {
                gvArchivos.DataSource = null;
            }
            gvArchivos.DataBind();
        }
        
        protected void Upload_Click(object sender, EventArgs e)
        {

            if (FileUpload1.HasFile)
            {
                //En panel admin se hardcodea con tipoDoc 1 hasta q se haga el codigo
                //int idTipoDoc = lnkUpload1.CommandArgument.ToInt();
                uploadMethod(FileUpload1, 1, "Varios");
                //Alert("Archivo cargado con éxito", "Se ha cargado un nuevo archivo al usuario seleccionado.", AlertType.success, "/Forms/FileUpload.aspx");
            }
            //
            if (FileUpload2.HasFile)
            {
                int idTipoDoc = lnkUpload2.CommandArgument.ToInt();
                uploadMethod(FileUpload2, idTipoDoc, "Certificado Médico");
                //Alert("Certificado Médico cargado con éxito", "Se ha vinculado un nuevo Certificado Médico a su usuario.", AlertType.success, "/Forms/FileUpload.aspx");

            }
            if (FileUpload3.HasFile) 
            {
                int idTipoDoc = lnkUpload3.CommandArgument.ToInt();
                uploadMethod(FileUpload3, idTipoDoc, "Certificado de Competencia");
                //Alert("Certificado de Competencia cargado con éxito", "Se ha vinculado un nuevo Certificado de Competencia a su usuario.", AlertType.success, "/Forms/FileUpload.aspx");
            }
            if (FileUpload4.HasFile)
            {
                int idTipoDoc = lnkUpload4.CommandArgument.ToInt();
                uploadMethod(FileUpload4, idTipoDoc, "CEVANT");
                //Alert("CEVANT cargado con éxito", "Se ha vinculado un nuevo CEVANT a su usuario.", AlertType.success, "/Forms/FileUpload.aspx");
            }
            if (FileUpload6.HasFile)
            {
                int idTipoDoc = lnkUpload6.CommandArgument.ToInt();
                uploadMethod(FileUpload6, idTipoDoc, "Seguro/Póliza");
                //Alert("Seguro/Poliza cargado con éxito", "Se ha vinculado un nuevo Seguro/Poliza a su usuario.", AlertType.success, "/Forms/FileUpload.aspx");
            }
            //CargarDoc(2, gvCertMedico, pnlFuCM, pnlFechaVencimientoCM, pnlBtnSubirArchivoCM);
            //CargarDoc(3, gvCertCompetencia, pnlFUCertCompetencia, pnlFechaVencimientoCertCompetencia, pnlBtnSubirArchivoCertCompetencia);
            //CargarDoc(4, gvCevant, pnlFUCevant, pnlFechaVencimientoCevant, pnlBtnSubirArchivoCevant);

        }

        protected void uploadMethod(System.Web.UI.WebControls.FileUpload FileUpload,int idTipoDoc, string titulo)
        {
            //Se obtienen los datos del documento
            string filename = Path.GetFileName(FileUpload.PostedFile.FileName);
            string extension = System.IO.Path.GetExtension(FileUpload.FileName);
            extension = extension.ToLower();
            string contentType = FileUpload.PostedFile.ContentType;
            //(Tamaño del archivo en bytes)
            int tam = FileUpload.PostedFile.ContentLength;
            byte[] bytes;
            var a = Convert.ToBase64String(FileUpload.FileBytes);
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
                        Documento.FHAlta = DateTime.Now;
                        Documento.TipoMIME = contentType;
                        Documento.IdTipoDocumento = idTipoDoc;
                        //Documento.FHBaja = null;
                        if (idTipoDoc == 1)
                        {
                            if (txtFechaVencimientoAdmin.Value != "")
                            {
                                Documento.FHVencimiento = txtFechaVencimientoAdmin.Value.ToDateTime();
                            }
                        }

                        //Certificado Medico
                        if (idTipoDoc == 2)
                        {
                            if (txtFechaVencimientoCertMedico.Value != "")
                            {
                                Documento.FHVencimiento = txtFechaVencimientoCertMedico.Value.ToDateTime();
                            }
                        }
                        //Certificado Competencia
                        if (idTipoDoc == 3)
                        {
                            if (txtFechaVencimientoCertCompetencia.Value != "")
                            {
                                Documento.FHVencimiento = txtFechaVencimientoCertCompetencia.Value.ToDateTime();
                            }
                        }
                        //Cevant
                        if (idTipoDoc == 4)
                        {
                            if (txtFechaVencimientoCevant.Value != "")
                            {
                                Documento.FHVencimiento = txtFechaVencimientoCevant.Value.ToDateTime();
                            }

                        }
                        //Seguro/Póliza (En Bd idTipoDoc=6 es para Poliza Vant)
                        if (idTipoDoc == 6)
                        {
                            if (txtFechaVencimientoSeguroPoliza.Value != "")
                            {
                                Documento.FHVencimiento = txtFechaVencimientoSeguroPoliza.Value.ToDateTime();
                            }
                        }


                        Documento.Insert();
                    }
                    BindGrid();
                    txtFechaVencimientoAdmin.Value = "";
                    string tituloAlert = "El Archivo " + titulo + " se ha subido con éxito";
                    Alert(tituloAlert, "Se ha vinculado el documento a su usuario.", AlertType.success, "/Forms/FileUpload.aspx");
                }
                else
                {
                    string tituloAlert1 = "El Archivo " + titulo + " no cumple con las especificaciones";
                    Alert(tituloAlert1, "El tamaño del archivo debe ser menor a 5Mb", AlertType.error);
                }
            }
            else
            {
                string tituloAlert2 = "El Archivo " + titulo + " no cumple con las especificaciones";
                Alert(tituloAlert2, "Selecciona solo archivos con extensión .PDF", AlertType.error);
            }
        }

        protected void lnkDownload_Click1(object sender, EventArgs e)
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
                //baja logica, agrega FHBaja
                sp.Execute("__DocumentoDelete_v1",
                    P.Add("IdDocumento", Documento.IdDocumento)
                );
            }
            Alert("Documento eliminado con éxito", "Se ha eliminado el documento seleccionado.", AlertType.success, "/Forms/FileUpload.aspx");
            //CargarDoc(2, gvCertMedico, pnlFuCM, pnlFechaVencimientoCM, pnlBtnSubirArchivoCM);
            //CargarDoc(3, gvCertCompetencia, pnlFUCertCompetencia, pnlFechaVencimientoCertCompetencia, pnlBtnSubirArchivoCertCompetencia);
            //CargarDoc(4, gvCevant, pnlFUCevant, pnlFechaVencimientoCevant, pnlBtnSubirArchivoCevant);
            //BindGrid();
        }

        protected void ddlSolicitante_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void CargarDoc(int idDoc, GridView gv, Panel panelFileUpload, Panel panelFechaVencimiento, Panel panelBtnSubirArchivo, int MostrarHistorial)
        {
            DataTable dt = null;
            //int idUsuario = 6;
            using (SP sp = new SP("bd_reapp"))
            {
                if (!ddlSolicitante.SelectedItem.Value.Equals("#"))
                {
                    int IdUsuario = ddlSolicitante.SelectedItem.Value.ToIntID();

                    dt = sp.Execute("usp_GetDocumentacionPorTipo", 
                        P.Add("IdUsuario", IdUsuario), 
                        P.Add("IdTipoDocumento", idDoc),
                        //Con 0 no muestra historial, con 1 si
                        P.Add("MostrarDocumentosEliminados", MostrarHistorial));
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
                panelBtnSubirArchivo.Visible = true;
            }
            else
            {
                gv.Visible = true;
                panelFileUpload.Visible = false;
                panelFechaVencimiento.Visible = false;
                panelBtnSubirArchivo.Visible = false;
            }
        }



        protected void gvArchivos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Session["idRol"].ToString() == "2")
                {
                    LinkButton lnkBtn = (LinkButton)e.Row.FindControl("lnkEliminarArchivo");
                    lnkBtn.Visible = false;
                }
            }
        }



        protected void verHistorialCevant_Click(object sender, EventArgs e)
        {
            CargarDoc(4, gvCevant, pnlFUCevant, pnlFechaVencimientoCevant, pnlBtnSubirArchivoCevant, 1);
            // Iterar a través de todas las filas de la grilla
            foreach (GridViewRow row in gvCevant.Rows)
            {
                // Buscar el LinkButton dentro de la fila actual
                LinkButton linkButton = (LinkButton)row.FindControl("lnkEliminarArchivo");
                LinkButton linkButton2 = (LinkButton)row.FindControl("verHistorialCevant");

                // Ocultar el LinkButton en la fila actual
                linkButton.Visible = false;
                linkButton2.Visible = false;
            }
        }

        protected void gvCevant_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                foreach (GridViewRow row in gvCevant.Rows)
                {
                    // Buscar el LinkButton dentro de la fila actual
                    LinkButton linkButton = (LinkButton)e.Row.FindControl("lnkDownload");

                    // Asociar el evento Click al LinkButton
                    linkButton.Click += new EventHandler(lnkDownload_Click1);
                }
            }
        }

        protected void verHistorialCertCompetencia_Click(object sender, EventArgs e)
        {
            CargarDoc(3, gvCertCompetencia, pnlFUCertCompetencia, pnlFechaVencimientoCertCompetencia, pnlBtnSubirArchivoCertCompetencia, 1);
            // Iterar a través de todas las filas de la grilla
            foreach (GridViewRow row in gvCertCompetencia.Rows)
            {
                // Buscar el LinkButton dentro de la fila actual
                LinkButton linkButton = (LinkButton)row.FindControl("lnkEliminarArchivo");
                LinkButton linkButton2 = (LinkButton)row.FindControl("verHistorialCertCompetencia");

                // Ocultar el LinkButton en la fila actual
                linkButton.Visible = false;
                linkButton2.Visible = false;
            }
        }

        protected void verHistorialCM_Click(object sender, EventArgs e)
        {
            CargarDoc(2, gvCertMedico, pnlFuCM, pnlFechaVencimientoCM, pnlBtnSubirArchivoCM, 1);
            // Iterar a través de todas las filas de la grilla
            foreach (GridViewRow row in gvCertMedico.Rows)
            {
                // Buscar el LinkButton dentro de la fila actual
                LinkButton linkButton = (LinkButton)row.FindControl("lnkEliminarArchivo");
                LinkButton linkButton2 = (LinkButton)row.FindControl("verHistorialCM");

                // Ocultar el LinkButton en la fila actual
                linkButton.Visible = false;
                linkButton2.Visible = false;
            }
        }

        protected void verHistorialSeguroPoliza_Click(object sender, EventArgs e)
        {
            CargarDoc(6, gvSeguroPoliza, pnlFuSeguroPoliza, pnlFechaVencimientoSeguroPoliza, pnlBtnSubirArchivoSeguroPoliza, 1);
            // Iterar a través de todas las filas de la grilla
            foreach (GridViewRow row in gvSeguroPoliza.Rows)
            {
                // Buscar el LinkButton dentro de la fila actual
                LinkButton linkButton = (LinkButton)row.FindControl("lnkEliminarArchivo");
                LinkButton linkButton2 = (LinkButton)row.FindControl("verHistorialSeguroPoliza");

                // Ocultar el LinkButton en la fila actual
                linkButton.Visible = false;
                linkButton2.Visible = false;
            }
        }

        protected void gvSeguroPoliza_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                foreach (GridViewRow row in gvSeguroPoliza.Rows)
                {
                    // Buscar el LinkButton dentro de la fila actual
                    LinkButton linkButton = (LinkButton)e.Row.FindControl("lnkDownload");

                    // Asociar el evento Click al LinkButton
                    linkButton.Click += new EventHandler(lnkDownload_Click1);
                }
            }
        }

        protected void gvCertCompetencia_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                foreach (GridViewRow row in gvCertCompetencia.Rows)
                {
                    // Buscar el LinkButton dentro de la fila actual
                    LinkButton linkButton = (LinkButton)e.Row.FindControl("lnkDownload");

                    // Asociar el evento Click al LinkButton
                    linkButton.Click += new EventHandler(lnkDownload_Click1);
                }
            }
        }

        protected void gvCertMedico_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                foreach (GridViewRow row in gvCertMedico.Rows)
                {
                    // Buscar el LinkButton dentro de la fila actual
                    LinkButton linkButton = (LinkButton)e.Row.FindControl("lnkDownload");

                    // Asociar el evento Click al LinkButton
                    linkButton.Click += new EventHandler(lnkDownload_Click1);
                }
            }
        }
    }
}