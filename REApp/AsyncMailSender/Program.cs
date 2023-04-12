using MagicSQL;
using REApp;
using REApp.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncMailSender
{
    internal static class Program
    {
        static void Main()
        {
            TimeSpan HoraActual = DateTime.UtcNow.TimeOfDay;
            TimeSpan HoraInicioProceso = new TimeSpan(2, 30, 0); // Por defecto a las 2:30 AM
            TimeSpan HoraFinProceso = new TimeSpan(3, 30, 0);
            
            if (HoraActual > HoraInicioProceso && HoraActual < HoraFinProceso)
            {
                ActualizarEstadoSolicitud();

                ValidarVigenciaDocumentos();

                Thread.Sleep(3600000);
            }
            else
            {
                Thread.Sleep(3600000); // Espera una hora e intenta de nuevo
            }
        }

        public static void ActualizarEstadoSolicitud()
        {
            new SP("bd_reapp").Execute("usp_ActualizarAutomaticoEstadosSolicitud");
        }

        public static void ValidarVigenciaDocumentos()
        {
            DataTable dt = new SP("bd_reapp").Execute("usp_GetEntidadesVencimiento");

            foreach (DataRow dr in dt.Rows)
            {
                HTMLBuilder HTML = new HTMLBuilder("REAPP - Vencimiento de Documentacion", "GenericMailTemplate.html");
                HTML.AppendTexto("Buenas tardes.");
                HTML.AppendSaltoLinea(2);
                HTML.AppendTexto($"Le informamos que la documentación legal de su {dr["Tipo"].ToString()} en el sistema REAPP ha vencido. Por favor renueve su documentación lo antes posible para poder continuar solicitando REAs.");

                string cuerpo = HTML.ConstruirHTML();

                MailController mail = new MailController("REAPP - Vencimiento de Documentación", cuerpo);
                mail.Add(dr["Nombre"].ToString(), dr["Email"].ToString());

                bool Exito = mail.Enviar();
            }
        }
    }
}
