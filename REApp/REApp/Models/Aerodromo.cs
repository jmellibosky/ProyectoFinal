using MagicSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REApp.Models
{
    public partial class Aerodromo : ISUD<Aerodromo>
    {
        public Aerodromo() : base(1) { } // base(SPs_Version)

        // Properties

        public int IdAerodromo { get; set; }

        public int IdProvincia { get; set; }

        public DateTime FHAlta { get; set; }

        public DateTime? FHBaja { get; set; }

        public string Ubicacion { get; set; }

        public string DesignacionLocal { get; set; }

        public string DesignacionOACI { get; set; }

        public string DesignacionIATA { get; set; }
    }
}