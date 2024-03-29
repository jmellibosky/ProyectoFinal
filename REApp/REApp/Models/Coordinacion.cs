﻿// Created for MagicSQL using MagicMaker [v.3.77.125.7055]

using System;
using MagicSQL;

namespace REApp.Models
{
    public partial class Coordinacion : ISUD<Coordinacion>
    {
        public Coordinacion() : base(2) { } // base(SPs_Version)

        // Properties

        public int IdCoordinacion { get; set; }

        public bool Aprobada { get; set; }

        public string Recomendaciones { get; set; }

        public DateTime FHCoordinacion { get; set; }

        public int IdInteresadoSolicitud { get; set; }

        public DateTime? FHCancelacion { get; set; }
    }
}