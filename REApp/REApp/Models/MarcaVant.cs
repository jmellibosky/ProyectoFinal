using MagicSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REApp.Models
{
    public partial class MarcaVant : ISUD<MarcaVant>
    {
        public MarcaVant() : base(1) { } // base(SPs_Version)

        // Properties

        public int IdMarcaVant { get; set; }

        public string Nombre { get; set; }

        public DateTime FHAlta { get; set; }

        public DateTime? FHBaja { get; set; }
    }
}