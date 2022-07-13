using MagicSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REApp.Models
{
    public partial class Documento : ISUD<Documento>
    {
        public Documento() : base(1) { } // base(SPs_Version)

        // Properties

        public int IdDocumento { get; set; }

        public int? IdSolicitud { get; set; }

        public int? IdRespuesta { get; set; }

        public int? IdUsuario { get; set; }

        public string Nombre { get; set; }

        public string Extension { get; set; }

        public string TipoMIME { get; set; }

        public string Datos { get; set; }

        public DateTime FHAlta { get; set; }

        public DateTime? FHBaja { get; set; }
    }
}