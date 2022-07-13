﻿using MagicSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REApp.Models
{
    public partial class EstadoSolicitud : ISUD<EstadoSolicitud>
    {
        public EstadoSolicitud() : base(1) { } // base(SPs_Version)

        // Properties

        public int IdEstadoSolicitud { get; set; }

        public string Nombre { get; set; }
    }
}