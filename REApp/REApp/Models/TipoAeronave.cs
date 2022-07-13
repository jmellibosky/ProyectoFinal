using MagicSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REApp.Models
{
    public partial class TipoAeronave : ISUD<TipoAeronave>
    {
        public TipoAeronave() : base(1) { } // base(SPs_Version)

        // Properties

        public int IdTipoAeronave { get; set; }

        public string Nombre { get; set; }

        public DateTime FHAlta { get; set; }

        public DateTime? FHBaja { get; set; }
    }
}