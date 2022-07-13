using MagicSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REApp.Models
{
    public partial class Rol : ISUD<Rol>
    {
        public Rol() : base(1) { } // base(SPs_Version)

        // Properties

        public int IdRol { get; set; }

        public string Nombre { get; set; }
    }
}