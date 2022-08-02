// Created for MagicSQL using MagicMaker [v.3.77.125.7055]

using System;
using MagicSQL;

namespace REApp.Models
{
    public partial class TripulacionSolicitud : ISUD<TripulacionSolicitud>
    {
        public TripulacionSolicitud() : base(1) { } // base(SPs_Version)

        // Properties

        public int IdTripulacionSolicitud { get; set; }

        public int IdTripulacion { get; set; }

        public int IdSolicitud { get; set; }

        public DateTime FHVinculacion { get; set; }

        public DateTime? FHDesvinculacion { get; set; }
    }
}