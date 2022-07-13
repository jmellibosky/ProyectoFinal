using MagicSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REApp.Models
{
    public partial class Aeronave : ISUD<Aeronave>
    {
        public Aeronave() : base(1) { } // base(SPs_Version)

        // Properties

        public int IdAeronave { get; set; }

        public string Nombre { get; set; }

        public int IdMarcaAeronave { get; set; }

        public int IdTipoAeronave { get; set; }

        public int IdUsuario { get; set; }

        public string Modelo { get; set; }

        public DateTime FHAlta { get; set; }

        public DateTime? FHBaja { get; set; }
    }
}