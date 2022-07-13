using MagicSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REApp.Models
{
    public partial class PuntoGeografico : ISUD<PuntoGeografico>
    {
        public PuntoGeografico() : base(1) { } // base(SPs_Version)

        // Properties

        public int IdPuntoGeografico { get; set; }

        public int? IdUbicacion { get; set; }

        public bool? EsPoligono { get; set; }

        public double? Radio { get; set; }

        public double Latitud { get; set; }

        public double Longitud { get; set; }
    }
}