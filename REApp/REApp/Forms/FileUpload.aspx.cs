using System;
using System.Collections.Generic;
using System.Configuration;
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
            BindGrid();
        }

        private void BindGrid()
        {
            string constr = ConfigurationManager.ConnectionStrings["bd_reapp"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select IdDocumento, Nombre, Extension, TipoMIME, FHAlta from Documento";
                    cmd.Connection = con;
                    con.Open();
                    GridView1.DataSource = cmd.ExecuteReader();
                    GridView1.DataBind();
                    con.Close();
                }
            }

        }

        //Subida de Archivo
        protected void Upload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                //Se obtiene la extension
                string extension = System.IO.Path.GetExtension(FileUpload1.FileName);
                extension = extension.ToLower();

                //Tamaño del archivo en bytes
                int tam = FileUpload1.PostedFile.ContentLength;

                //Se verifica la extension 
                if (extension == ".pdf")
                {
                    //Se verifica el tamaño del archivo en bytes (1000000 = 1Mb)
                    if (tam <= 1000000)
                    {
                        //Se verifica si el archivo ya existe, si no existe se realiza la subida
                        //Esto es para local, ver para bd como hacerlo
                        if (!File.Exists(Server.MapPath("~/Files/" + FileUpload1.FileName)))
                        {
                            string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
                            string contentType = FileUpload1.PostedFile.ContentType;
                            using (Stream fs = FileUpload1.PostedFile.InputStream)
                            {
                                using (BinaryReader br = new BinaryReader(fs))
                                {
                                    byte[] bytes = br.ReadBytes((Int32)fs.Length);
                                    string constr = ConfigurationManager.ConnectionStrings["bd_reapp"].ConnectionString;
                                    using (SqlConnection con = new SqlConnection(constr))
                                    {
                                        //Amoldar a la BD real
                                        string query = "insert into Documento values (NULL, NULL, NULL, @Nombre, @Extension, @TipoMIME, @FHAlta, NULL, @Datos)";
                                        using (SqlCommand cmd = new SqlCommand(query))
                                        {
                                            cmd.Connection = con;
                                            cmd.Parameters.AddWithValue("@Nombre", filename);
                                            cmd.Parameters.AddWithValue("@Extension", extension);
                                            cmd.Parameters.AddWithValue("@TipoMIME", contentType);
                                            cmd.Parameters.AddWithValue("@FHAlta", DateTime.Today);
                                            cmd.Parameters.AddWithValue("@Datos", bytes);
                                            con.Open();
                                            cmd.ExecuteNonQuery();
                                            con.Close();
                                        }
                                    }
                                }
                            }
                            Response.Redirect(Request.Url.AbsoluteUri);
                            BindGrid();
                            //Ver Donde poner para q lo muesrtre!!!
                            LbArchivo.Text = "Archivo subido con éxito!";
                        }

                        else
                        {
                            LbArchivo.Text = "Ya existe un archivo con ese nombre";
                        }
                    }
                    else
                    {
                        LbArchivo.Text = "El tamaño del archivo debe ser menor a 1Mb";
                    }

                }
                else
                {
                    LbArchivo.Text = "Selecciona solo archivos con extension .PDF";
                }
            }
            else
            {
                LbArchivo.Text = "No se selecciono un archivo";
            }
        }

        protected void lnkDownload_Click1(object sender, EventArgs e)
        {
            int id = int.Parse((sender as LinkButton).CommandArgument);
            byte[] bytes;
            string fileName, contentType;
            string constr = ConfigurationManager.ConnectionStrings["bd_reapp"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select Nombre, Datos, TipoMIME from Documento where IdDocumento=@IdDocumento";
                    cmd.Parameters.AddWithValue("@IdDocumento", id);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        sdr.Read();
                        bytes = (byte[])sdr["Datos"];
                        contentType = sdr["TipoMIME"].ToString();
                        fileName = sdr["Nombre"].ToString();
                    }
                    con.Close();
                }
            }
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
            string constr = ConfigurationManager.ConnectionStrings["bd_reapp"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "DELETE from DOCUMENTO WHERE IdDocumento = @IdDocumento";
                    cmd.Parameters.AddWithValue("@IdDocumento", id);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        sdr.Read();
                    }
                    con.Close();
                }
            }
            BindGrid();
        }
    }
}