using MagicSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REApp.Models
{
    public partial class Tripulacion : ISUD<Tripulacion>
    {
        public Tripulacion() : base(1) { } // base(SPs_Version)

        // Properties

        public int IdTripulacion { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string DNI { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public string Telefono { get; set; }

        public DateTime FHAlta { get; set; }

        public DateTime? FHBaja { get; set; }

        public string Correo { get; set; }
    }
}