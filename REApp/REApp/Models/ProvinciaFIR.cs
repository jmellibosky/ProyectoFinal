using MagicSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REApp.Models
{
    public partial class ProvinciaFIR : ISUD<ProvinciaFIR>
    {
        public ProvinciaFIR() : base(1) { } // base(SPs_Version)

        // Properties

        public int IdProvinciaFIR { get; set; }

        public int IdProvincia { get; set; }

        public int IdFIR { get; set; }
    }
}