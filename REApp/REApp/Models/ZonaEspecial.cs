using MagicSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REApp.Models
{
    public partial class ZonaEspecial : ISUD<ZonaEspecial>
    {
        public ZonaEspecial() : base(1) { } // base(SPs_Version)

        // Properties

        public int IdZonaEspecial { get; set; }

        public int IdProvincia { get; set; }

        public double Latitud { get; set; }

        public double Longitud { get; set; }

        public double Radio { get; set; }

        public DateTime FHAlta { get; set; }

        public DateTime? FHBaja { get; set; }

        public int IdTipoZonaEspecial { get; set; }
    }
}