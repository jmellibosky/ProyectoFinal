using MagicSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REApp.Models
{
    public partial class Mensaje : ISUD<Mensaje>
    {
        public Mensaje() : base(1) { } // base(SPs_Version)

        // Properties

        public int IdMensaje { get; set; }

        public int IdUsuarioEmisor { get; set; }

        public int IdUsuarioReceptor { get; set; }

        public int IdSolicitud { get; set; }

        public DateTime FHMensaje { get; set; }

        public string Contenido { get; set; }
    }
}