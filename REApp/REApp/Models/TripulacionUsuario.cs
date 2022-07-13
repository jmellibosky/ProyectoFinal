using MagicSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REApp.Models
{
    public partial class TripulacionUsuario : ISUD<TripulacionUsuario>
    {
        public TripulacionUsuario() : base(1) { } // base(SPs_Version)

        // Properties

        public int IdTripulacionUsuario { get; set; }

        public int IdTripulacion { get; set; }

        public int IdUsuario { get; set; }

        public DateTime FHAlta { get; set; }

        public DateTime? FHBaja { get; set; }
    }
}