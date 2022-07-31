// Created for MagicSQL using MagicMaker [v.3.77.125.7055]

using System;
using MagicSQL;

namespace bd_reapp
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

        public DateTime FHAlta { get; set; }

        public DateTime? FHBaja { get; set; }

        public byte[] Datos { get; set; }

        public DateTime? FHVencimiento { get; set; }

        public int? IdVant { get; set; }

        public int? IdTripulacion { get; set; }

        public int? IdTipoDocumento { get; set; }
    }
}