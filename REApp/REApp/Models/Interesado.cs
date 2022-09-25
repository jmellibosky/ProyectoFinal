﻿// Created for MagicSQL using MagicMaker [v.3.77.125.7055]

using System;
using MagicSQL;

namespace REApp.Models
{
    public partial class Interesado : ISUD<Interesado>
    {
        public Interesado() : base(1) { } // base(SPs_Version)

        // Properties

        public int IdInteresado { get; set; }

        public string Nombre { get; set; }

        public int? IdUsuario { get; set; }

        public string Email { get; set; }
    }
}