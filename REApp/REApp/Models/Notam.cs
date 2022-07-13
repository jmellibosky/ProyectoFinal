using MagicSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REApp.Models
{
    public partial class Notam : ISUD<Notam>
    {
        public Notam() : base(1) { } // base(SPs_Version)

        // Properties

        public int IdNotam { get; set; }

        public string Nombre { get; set; }

        public string Contenido { get; set; }

        public DateTime FHAlta { get; set; }

        public DateTime? FHBaja { get; set; }

        public string Observacion { get; set; }
    }
}