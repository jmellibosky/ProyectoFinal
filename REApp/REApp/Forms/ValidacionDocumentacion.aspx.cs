using MagicSQL;
using REApp.Controllers;
using REApp.Models;
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
            // Guardar el DataTable en ViewState para acceder a él en el evento RowDataBound
            ViewState["DocumentosData"] = dt;

            // Asignar el DataTable como origen de datos del GridView
            gvArchivos.DataSource = dt;
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

            //Envio de mail
            int idDoc = (int)Documento.IdTipoDocumento;
            TipoDocumento TipoDoc = new TipoDocumento().Select(idDoc);
            string nombreTipoDoc = TipoDoc.Nombre;
            int idUsuario = ddlSolicitante.SelectedItem.Value.ToIntID();
            Usuario Usuario = new Usuario().Select(idUsuario);
            EnviarMailAprobacionDocumento(Usuario, Documento, nombreTipoDoc);

            Alert("Documento aceptado con éxito", "Se ha actualizado su documento y se ha guardado la fecha y usuario que lo actualizó. Se envió email al usuario informando la aprobación del mismo.", AlertType.success, "/Forms/ValidacionDocumentacion.aspx");
        }

        protected void lnkRechazarArchivo_Click(object sender, EventArgs e)
        {
            
            int id = int.Parse((sender as LinkButton).CommandArgument);
            Models.Documento Documento = new Models.Documento().Select(id);
            Documento.FHRechazo = DateTime.Now;
            Documento.IdUsuarioRechazadoPor = Session["IdUsuario"].ToString().ToInt();
            Documento.Update();

            //Envio de mail
            int idDoc = (int)Documento.IdTipoDocumento;
            TipoDocumento TipoDoc = new TipoDocumento().Select(idDoc);
            string nombreTipoDoc = TipoDoc.Nombre;
            int idUsuario = ddlSolicitante.SelectedItem.Value.ToIntID();
            Usuario Usuario = new Usuario().Select(idUsuario);
            EnviarMailRechazoDocumento(Usuario, Documento, nombreTipoDoc);
            Alert("Documento rechazado con éxito", "Se ha actualizado su documento y se ha guardado la fecha y usuario que lo actualizó. Se envió email al usuario informando el rechazo del mismo.", AlertType.error, "/Forms/ValidacionDocumentacion.aspx");
        }

        protected void EnviarMailRechazoDocumento(Models.Usuario usuario, Documento documento, string nombreTipoDoc)
        {
            HTMLBuilder builder = new HTMLBuilder("Su " + nombreTipoDoc + " ha sido rechazado.", "GenericMailTemplate.html");

            builder.AppendTexto($"Hola {usuario.Nombre} {usuario.Apellido}.");
            builder.AppendSaltoLinea(3);
            builder.AppendTexto("Nos comunicamos de REAPP, queriamos informarle que su " + nombreTipoDoc + " con nombre: " + documento.Nombre + " ha sido rechazado por un operador de EANA.");
            builder.AppendSaltoLinea(2);
            if (documento.IdTripulacion != null)
            {
                int idTripulante = (int)documento.IdTripulacion;
                Tripulacion tripulante = new Tripulacion().Select(idTripulante);
                builder.AppendTexto("El documento pertence a su tripulante: " + tripulante.Apellido + ", "+ tripulante.Nombre + ".");
                builder.AppendSaltoLinea(2);
            }
            builder.AppendTexto("Por favor verifique que haya subido un documento valido y que la fecha de vencimiento coincida con la especificada dentro del mismo.");
            builder.AppendSaltoLinea(2);
            builder.AppendTexto("Se ha habilitado para que pueda subir un " + nombreTipoDoc + " nuevo.");
            builder.AppendSaltoLinea(4);
            builder.AppendTexto("Saludos.");
            builder.AppendSaltoLinea(2);
            builder.AppendTexto("Equipo de REApp.");
            string cuerpo = builder.ConstruirHTML();

            MailController mail = new MailController("Su " + nombreTipoDoc + " ha sido rechazado - REAPP", cuerpo);
            mail.Add($"{usuario.Nombre} {usuario.Apellido}", usuario.Email);
            mail.Enviar();
        }

        protected void EnviarMailAprobacionDocumento(Models.Usuario usuario, Documento documento, string nombreTipoDoc)
        {
            HTMLBuilder builder = new HTMLBuilder("Su " + nombreTipoDoc + " ha sido aprobado.", "GenericMailTemplate.html");

            builder.AppendTexto($"Hola {usuario.Nombre} {usuario.Apellido}.");
            builder.AppendSaltoLinea(3);
            builder.AppendTexto("Nos comunicamos de REAPP, queriamos informarle que su " + nombreTipoDoc + " con nombre: '" + documento.Nombre + "' ha sido aprobado por un operador de EANA.");
            builder.AppendSaltoLinea(2);
            if (documento.IdTripulacion != null)
            {
                int idTripulante = (int)documento.IdTripulacion;
                Tripulacion tripulante = new Tripulacion().Select(idTripulante);
                builder.AppendTexto("El documento pertence a su tripulante: " + tripulante.Apellido + ", " + tripulante.Nombre + ".");
                builder.AppendSaltoLinea(2);
            }
            builder.AppendTexto("En caso de que todos los documentos subidos al sistema se encuentren vigentes y estén aprobado por un operador de EANA, Ud. se encuentra habilitado para crear nuevas solicitudes de REA.");
            builder.AppendSaltoLinea(4);
            builder.AppendTexto("Saludos.");
            builder.AppendSaltoLinea(2);
            builder.AppendTexto("Equipo de REApp.");
            string cuerpo = builder.ConstruirHTML();

            MailController mail = new MailController("Su " + nombreTipoDoc + " ha sido aprobado - REAPP", cuerpo);
            mail.Add($"{usuario.Nombre} {usuario.Apellido}", usuario.Email);
            mail.Enviar();
        }



        //Metodo para actualizar los botones en enable false or true en grilla segun estado de documento
        protected void gvArchivos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Obtener el DataTable guardado en ViewState
            DataTable dt = ViewState["DocumentosData"] as DataTable;

            // Verificar si el DataTable existe y la fila es de tipo DataRow
            if (dt != null && e.Row.RowType == DataControlRowType.DataRow)
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