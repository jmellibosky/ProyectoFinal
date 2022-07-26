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

namespace REApp.Forms
{
    public partial class FileUpload : System.Web.UI.Page
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
                 LbArchivo.Text = "";

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
                //Se obtienen los datos del documento
                string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
                string extension = System.IO.Path.GetExtension(FileUpload1.FileName);
                extension = extension.ToLower();
                string contentType = FileUpload1.PostedFile.ContentType;
                //(Tamaño del archivo en bytes)
                int tam = FileUpload1.PostedFile.ContentLength;
                byte[] bytes;
                using (Stream fs = FileUpload1.PostedFile.InputStream)
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
                            //Documento.FHBaja = null;
                            if(txtFechaVencimiento.Value != "")
                            {
                                Documento.FHVencimiento = txtFechaVencimiento.Value.ToDateTime();
                            }
                            Documento.Insert();
                        }
                        BindGrid();
                        txtFechaVencimiento.Value = "";
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
            else
            {
                LbArchivo.Text = "No se seleccionó un archivo.";
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
            BindGrid();
        }

        protected void ddlSolicitante_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //protected void btnFiltrar_Click(object sender, EventArgs e)
        //{
        //    BindGrid();
        //}

    }
}