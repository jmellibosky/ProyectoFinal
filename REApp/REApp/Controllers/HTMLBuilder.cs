using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace REApp.Controllers
{
    public class HTMLBuilder
    {
        private string Titulo { get; set; }
        private string Descripcion { get; set; }
        private string TemplatePath { get; set; }

        public HTMLBuilder(string Titulo, string TemplatePath)
        {
            this.Titulo = Titulo;
            this.TemplatePath = TemplatePath;
        }

        public void AppendTexto(string texto)
        {
            Descripcion += texto;
        }

        public void AppendSaltoLinea(int cantidad)
        {
            for (int i = 0; i < cantidad; i++)
            {
                Descripcion += "<br />";
            }
        }

        public void AppendURL(string URL, string texto)
        {
            Descripcion += "<a href=\"";
            Descripcion += URL;
            Descripcion += "\">";
            Descripcion += texto;
            Descripcion += "</a>";
        }

        public string ConstruirHTML()
        {
            string Path = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/html/" + TemplatePath);

            if (Path == null)
            {
                Path = System.IO.Path.GetFullPath("../../Content/html/" + TemplatePath);
            }

            string template = File.ReadAllText(Path);

            string html = template.Replace("$TITULO$", Titulo).Replace("$DESCRIPCION$", Descripcion);

            return html;
        }
    }
}