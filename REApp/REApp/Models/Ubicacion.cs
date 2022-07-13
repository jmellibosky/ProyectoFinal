using MagicSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REApp.Models
{
    public partial class Ubicacion : ISUD<Ubicacion>
    {
        public Ubicacion() : base(1) { } // base(SPs_Version)

        // Properties

        public int IdUbicacion { get; set; }

        public int IdSolicitud { get; set; }

        public double Altura { get; set; }

        public int IdProvincia { get; set; }
    }
}