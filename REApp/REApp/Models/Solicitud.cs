using MagicSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REApp.Models
{
    public partial class Solicitud : ISUD<Solicitud>
    {
        public Solicitud() : base(1) { } // base(SPs_Version)

        // Properties

        public int IdSolicitud { get; set; }

        public int? IdAeronave { get; set; }

        public int? IdAerodromo { get; set; }

        public int IdModalidad { get; set; }

        public int IdUsuario { get; set; }

        public int? IdRespuesta { get; set; }

        public int? IdEstadoSolicitud { get; set; }

        public string FHUltimaActualizacionEstado { get; set; }

        public DateTime FHSolicitud { get; set; }

        public string Observaciones { get; set; }

        public string Nombre { get; set; }
    }
}