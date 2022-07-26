// Created for MagicSQL using MagicMaker [v.3.77.125.7055]

using System;
using MagicSQL;

namespace REApp.Models
{
    public partial class Usuario : ISUD<Usuario>
    {
        public Usuario() : base(1) { } // base(SPs_Version)

        // Properties

        public int IdUsuario { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Email { get; set; }

        public int Dni { get; set; }

        public string TipoDni { get; set; }

        public int IdRol { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? DeletedOn { get; set; }

        public int? DeletedBy { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public string Telefono { get; set; }

        public string Password { get; set; }

        public string SaltKey { get; set; }

        public int? Cuit { get; set; }
    }
}