// Created for MagicSQL using MagicMaker [v.3.77.125.7055]

using System;
using MagicSQL;

namespace REApp.Models
{
    public partial class SolicitudEstadoHistorial : ISUD<SolicitudEstadoHistorial>
    {
        public SolicitudEstadoHistorial() : base(1) { } // base(SPs_Version)

        // Properties

        public int IdSolicitudEstadoHistorial { get; set; }

        public int IdSolicitud { get; set; }

        public int IdEstadoSolicitud { get; set; }

        public DateTime FHDesde { get; set; }

        public DateTime? FHHasta { get; set; }

        public int IdUsuarioCambioEstado { get; set; }

        public int? IdSolicitudEstadoHistorialAnterior { get; set; }
    }
}