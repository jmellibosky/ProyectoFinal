// Created for MagicSQL using MagicMaker [v.3.77.125.7055]

using System;
using MagicSQL;

namespace bd_reapp
{
    public partial class InteresadoSolicitud : ISUD<InteresadoSolicitud>
    {
        public InteresadoSolicitud() : base(1) { } // base(SPs_Version)

        // Properties

        public int IdInteresadoSolicitud { get; set; }

        public int IdInteresado { get; set; }

        public int IdSolicitud { get; set; }

        public DateTime FHVinculacion { get; set; }

        public DateTime? FHDesvinculacion { get; set; }
    }
}