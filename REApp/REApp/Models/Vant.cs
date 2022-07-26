using MagicSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REApp.Models
{
    public partial class Vant : ISUD<Vant>
    {
        public Vant() : base(1) { } // base(SPs_Version)

        // Properties

        public int IdVant { get; set; }

        public int IdMarcaVant { get; set; }

        public int IdTipoVant { get; set; }

        public DateTime FHAlta { get; set; }

        public DateTime? FHBaja { get; set; }

        public string Modelo { get; set; }

        public int IdUsuario { get; set; }

        public string Fabricante { get; set; }

        public string NumeroSerie { get; set; }

        public DateTime AñoFabricacion { get; set; }

        public string LugarFabricacion { get; set; }

        public string LugarGuardado { get; set; }


    }
}