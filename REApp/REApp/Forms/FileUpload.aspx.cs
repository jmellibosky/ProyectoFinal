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
                    CargarDoc(2, gvCertMedico, pnlFuCM, pnlFechaVencimientoCM, pnlBtnSubirArchivoCM);
                    CargarDoc(3, gvCertCompetencia, pnlFUCertCompetencia, pnlFechaVencimientoCertCompetencia, pnlBtnSubirArchivoCertCompetencia);
                    CargarDoc(4, gvCevant, pnlFUCevant, pnlFechaVencimientoCevant, pnlBtnSubirArchivoCevant);
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
                    CargarDoc(2, gvCertMedico, pnlFuCM, pnlFechaVencimientoCM, pnlBtnSubirArchivoCM);
                    CargarDoc(3, gvCertCompetencia, pnlFUCertCompetencia, pnlFechaVencimientoCertCompetencia, pnlBtnSubirArchivoCertCompetencia);
                    CargarDoc(4, gvCevant, pnlFUCevant, pnlFechaVencimientoCevant, pnlBtnSubirArchivoCevant);
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
                uploadMethod(FileUpload1, 1);
                Alert("Archivo cargado con éxito", "Se ha cargado un nuevo archivo al usuario seleccionado.", AlertType.success, "/Forms/FileUpload.aspx");
            }
            //
            if (FileUpload2.HasFile)
            {
                int idTipoDoc = lnkUpload2.CommandArgument.ToInt();
                uploadMethod(FileUpload2, idTipoDoc);
                Alert("Certificado Médico cargado con éxito", "Se ha vinculado un nuevo Certificado Médico a su usuario.", AlertType.success, "/Forms/FileUpload.aspx");

            }
            if (FileUpload3.HasFile) 
            {
                int idTipoDoc = lnkUpload3.CommandArgument.ToInt();
                uploadMethod(FileUpload3, idTipoDoc);
                Alert("Certificado de Competencia cargado con éxito", "Se ha vinculado un nuevo Certificado de Competencia a su usuario.", AlertType.success, "/Forms/FileUpload.aspx");
            }
            if (FileUpload4.HasFile)
            {
                int idTipoDoc = lnkUpload4.CommandArgument.ToInt();
                uploadMethod(FileUpload4, idTipoDoc);
                Alert("CEVANT cargado con éxito", "Se ha vinculado un nuevo CEVANT a su usuario.", AlertType.success, "/Forms/FileUpload.aspx");
            }
            //CargarDoc(2, gvCertMedico, pnlFuCM, pnlFechaVencimientoCM, pnlBtnSubirArchivoCM);
            //CargarDoc(3, gvCertCompetencia, pnlFUCertCompetencia, pnlFechaVencimientoCertCompetencia, pnlBtnSubirArchivoCertCompetencia);
            //CargarDoc(4, gvCevant, pnlFUCevant, pnlFechaVencimientoCevant, pnlBtnSubirArchivoCevant);

        }

        protected void uploadMethod(System.Web.UI.WebControls.FileUpload FileUpload,int idTipoDoc)
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


                        Documento.Insert();
                    }
                    BindGrid();
                    txtFechaVencimientoAdmin.Value = "";
                    LbArchivo.Text = "El archivo se subió con éxito.";
                    LbArchivo.CssClass = "hljs-string border";
                }
                else
                {
                    LbArchivo.Text = "El tamaño del archivo debe ser menor a 5Mb";
                }
            }
            else
            {
                LbArchivo.Text = "Selecciona solo archivos con extensión .PDF";
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

        private void CargarDoc(int idDoc, GridView gv, Panel panelFileUpload, Panel panelFechaVencimiento, Panel panelBtnSubirArchivo)
        {
            DataTable dt = null;
            //int idUsuario = 6;
            using (SP sp = new SP("bd_reapp"))
            {
                if (!ddlSolicitante.SelectedItem.Value.Equals("#"))
                {
                    dt = sp.Execute("usp_GetDocumentacionPorTipo", 
                        P.Add("IdUsuario", ddlSolicitante.SelectedItem.Value.ToIntID()), 
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
    }
}