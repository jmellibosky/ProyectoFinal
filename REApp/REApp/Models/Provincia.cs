using MagicSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REApp.Models
{
    public partial class Provincia : ISUD<Provincia>
    {
        public Provincia() : base(1) { } // base(SPs_Version)

        // Properties

        public int IdProvincia { get; set; }

        public int? IdSubregional { get; set; }

        public string Nombre { get; set; }
    }
}