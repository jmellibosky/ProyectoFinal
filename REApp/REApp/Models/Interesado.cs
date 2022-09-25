// Created for MagicSQL using MagicMaker [v.3.77.125.7055]

using System;
using MagicSQL;

namespace bd_reapp
{
    public partial class Interesado : ISUD<Interesado>
    {
        public Interesado() : base(2) { } // base(SPs_Version)

        // Properties

        public int IdInteresado { get; set; }

        public string Nombre { get; set; }

        public int? IdUsuario { get; set; }

        public string Email { get; set; }

        public DateTime FHAlta { get; set; }

        public DateTime? FHBaja { get; set; }

        public int? IdLocalidad { get; set; }
    }
}