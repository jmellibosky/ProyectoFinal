// Created for MagicSQL using MagicMaker [v.3.77.125.7055]

using System;
using MagicSQL;

namespace REAPP.Models
{
    public partial class Actividad : ISUD<Actividad>
    {
        public Actividad() : base(2) { } // base(SPs_Version)

        // Properties

        public int IdActividad { get; set; }

        public string Nombre { get; set; }

        public DateTime FHAlta { get; set; }

        public DateTime? FHBaja { get; set; }

        public bool? AdmiteVANTs { get; set; }

        public bool? AdmiteAeronaves { get; set; }
    }
}