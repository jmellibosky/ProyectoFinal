// Created for MagicSQL using MagicMaker [v.3.77.125.7055]

using System;
using MagicSQL;

namespace REApp.Models
{
    public partial class Interesado : ISUD<Interesado>
    {
        public Interesado() : base(3) { } // base(SPs_Version)

        // Properties

        public int IdInteresado { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public int? IdUsuario { get; set; }

        public string Email { get; set; }

        public DateTime FHAlta { get; set; }

        public DateTime? FHBaja { get; set; }

        public int? IdLocalidad { get; set; }

        public int? IdProvincia { get; set; }

        public string Telefono { get; set; }

        public string Observacion { get; set; }
    }
}