using MagicSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REApp.Models
{
    public partial class Respuesta : ISUD<Respuesta>
    {
        public Respuesta() : base(1) { } // base(SPs_Version)

        // Properties

        public int IdRespuesta { get; set; }

        public int IdUsuario { get; set; }

        public DateTime FHRespuesta { get; set; }

        public bool Aprobada { get; set; }

        public int? IdNotam { get; set; }

        public string Observaciones { get; set; }
    }
}