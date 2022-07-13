using MagicSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REApp.Models
{
    public partial class TipoZonaEspecial : ISUD<TipoZonaEspecial>
    {
        public TipoZonaEspecial() : base(1) { } // base(SPs_Version)

        // Properties

        public int IdTipoZonaEspecial { get; set; }

        public string Nombre { get; set; }
    }
}