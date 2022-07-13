using MagicSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REApp.Models
{
    public partial class FIR : ISUD<FIR>
    {
        public FIR() : base(1) { } // base(SPs_Version)

        // Properties

        public int IdFIR { get; set; }

        public string Nombre { get; set; }

        public string Codigo { get; set; }
    }
}