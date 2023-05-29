using MagicSQL;
using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using static REApp.Navegacion;

namespace REApp.Forms
{
    public partial class ValidacionDocumentacion : System.Web.UI.Page
    {
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
                if (idRolInt == 1 || idRolInt == 2)
                {
                    BindGrid();
                }

            }
            if (!IsPostBack)
            {
                if (idRolInt == 1 || idRolInt == 2)
                {
                    CargarComboSolicitante();
                    BindGrid();
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
                    dt = sp.Execute("usp_GetDocumentosValidacion", P.Add("IdUsuario", ddlSolicitante.SelectedItem.Value.ToIntID()));
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

        protected void lnkAceptarArchivo_Click(object sender, EventArgs e)
        {
            
            int id = int.Parse((sender as LinkButton).CommandArgument);
            Models.Documento Documento = new Models.Documento().Select(id);
            Documento.FHAprobacion = DateTime.Now;
            Documento.IdUsuarioAprobadoPor = Session["IdUsuario"].ToString().ToInt();
            Documento.Update();
            Alert("Documento aceptado con éxito", "Se ha actualizado su documento y se ha guardado la fecha y usuario que lo actualizó.", AlertType.success, "/Forms/ValidacionDocumentacion.aspx");
        }

        protected void lnkRechazarArchivo_Click(object sender, EventArgs e)
        {

            int id = int.Parse((sender as LinkButton).CommandArgument);
            Models.Documento Documento = new Models.Documento().Select(id);
            Documento.FHRechazo = DateTime.Now;
            Documento.IdUsuarioRechazadoPor = Session["IdUsuario"].ToString().ToInt();
            Documento.Update();
            Alert("Documento rechazado con éxito", "Se ha actualizado su documento y se ha guardado la fecha y usuario que lo actualizó.", AlertType.error, "/Forms/ValidacionDocumentacion.aspx");
        }

        protected void gvArchivos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = null;
            using (SP sp = new SP("bd_reapp"))
            {
                if (!ddlSolicitante.SelectedItem.Value.Equals("#"))
                {
                    dt = sp.Execute("usp_GetFHAprobacionRechazoDocumentosxIdUsuario", P.Add("IdUsuario", ddlSolicitante.SelectedItem.Value.ToIntID()));
                }
            }

            // Itera por las filas en las que el documento está Aprobado y Rechazado para establecer los Enable de los botones
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Obtener el ID del documento desde el DataKey del GridView
                int id;
                if (int.TryParse(gvArchivos.DataKeys[e.Row.RowIndex].Value.ToString(), out id))
                {
                    // Obtener el DataRow correspondiente al ID del documento
                    DataRow[] rows = dt.Select("iddocumento = " + id);

                    if (rows.Length > 0)
                    {
                        DataRow row = rows[0];

                        LinkButton lnkBtnRechazo = (LinkButton)e.Row.FindControl("lnkRechazarArchivo");
                        LinkButton lnkBtnAprobado = (LinkButton)e.Row.FindControl("lnkAceptarArchivo");

                        // Si el documento está Aprobado, deshabilitar el botón de rechazo
                        if (!string.IsNullOrEmpty(row["FHAprobacion"].ToString()))
                        {
                            lnkBtnAprobado.Enabled = false;
                        }

                        // Si el documento está Rechazado, deshabilitar los botones de aprobación y rechazo
                        if (!string.IsNullOrEmpty(row["FHRechazo"].ToString()))
                        {
                            lnkBtnAprobado.Enabled = false;
                            lnkBtnRechazo.Enabled = false;
                        }
                    }
                }
            }
        }
    }
}