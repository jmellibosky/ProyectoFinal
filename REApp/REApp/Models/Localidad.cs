using MagicSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REApp.Models
{
    public partial class Localidad : ISUD<Localidad>
    {
        public Localidad() : base(1) { } // base(SPs_Version)

        // Properties

        public int IdLocalidad { get; set; }

        public int IdProvincia { get; set; }

        public string NombreLocalidad { get; set; }
    }
}