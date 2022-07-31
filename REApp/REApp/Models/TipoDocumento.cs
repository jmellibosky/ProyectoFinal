// Created for MagicSQL using MagicMaker [v.3.77.125.7055]

using System;
using MagicSQL;

namespace bd_reapp
{
    public partial class TipoDocumento : ISUD<TipoDocumento>
    {
        public TipoDocumento() : base(1) { } // base(SPs_Version)

        // Properties

        public int IdTipoDocumento { get; set; }

        public string Nombre { get; set; }

        public DateTime FHAlta { get; set; }

        public DateTime? FHBaja { get; set; }
    }
}