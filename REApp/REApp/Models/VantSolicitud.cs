using MagicSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REApp.Models
{
    public partial class VantSolicitud : ISUD<VantSolicitud>
    {
        public VantSolicitud() : base(2) { } // base(SPs_Version)

        // Properties

        public int IdVantSolicitud { get; set; }

        public int IdVant { get; set; }

        public int IdSolicitud { get; set; }

        public DateTime? FHAlta { get; set; }

        public DateTime? FHBaja { get; set; }
    }
}