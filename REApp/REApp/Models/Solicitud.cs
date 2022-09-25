﻿// Created for MagicSQL using MagicMaker [v.3.77.125.7055]

using System;
using MagicSQL;

namespace bd_reapp
{
    public partial class Solicitud : ISUD<Solicitud>
    {
        public Solicitud() : base(2) { } // base(SPs_Version)

        // Properties

        public int IdSolicitud { get; set; }

        public int? IdAeronave { get; set; }

        public int? IdAerodromo { get; set; }

        public int IdModalidad { get; set; }

        public int IdUsuario { get; set; }

        public int? IdRespuesta { get; set; }

        public int? IdEstadoSolicitud { get; set; }

        public DateTime? FHUltimaActualizacionEstado { get; set; }

        public DateTime FHAlta { get; set; }

        public string Observaciones { get; set; }

        public string Nombre { get; set; }

        public DateTime? FHBaja { get; set; }

        public DateTime FHDesde { get; set; }

        public DateTime FHHasta { get; set; }

        public int? IdLocalidad { get; set; }
    }
}