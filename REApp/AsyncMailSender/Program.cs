﻿using MagicSQL;
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
            while (true)
            {
                TimeSpan HoraActual = DateTime.Now.TimeOfDay;
                TimeSpan HoraInicioProceso = new TimeSpan(13, 10, 0); // Por defecto a las 8 AM
                TimeSpan HoraFinProceso = new TimeSpan(13, 30, 0);

                int MilisegundosEspera = (int)HoraFinProceso.Subtract(HoraInicioProceso).TotalSeconds * 1000;

                Console.WriteLine($"Hora Actual: {HoraActual.ToString()}\nHora Inicio Proceso: {HoraInicioProceso.ToString()}\nHora Fin Proceso: {HoraFinProceso.ToString()}");
                if (HoraActual > HoraInicioProceso && HoraActual < HoraFinProceso)
                {
                    Console.WriteLine("Actualizando estados...");

                    // Una vez por día, según lo indicado en HoraInicioProceso y HoraFinProceso, ejecuta.

                    // Actualiz el Estado de las Solicitudes según hayan
                    //      iniciado su actividad (EnCurso),
                    //      finalizado su actividad (Finalizada),
                    //      o vencido (Vencida).
                    ActualizarEstadoSolicitud();

                    // Envía un email advirtiendo sobre el vencimiento de documentación vinculada a
                    //      explotador,
                    //      tripulantes
                    //      o VANTs.
                    ValidarVigenciaDocumentos();

                    Console.WriteLine("Fin Proceso.");

                    Console.WriteLine("\n\n\n-------o------\n\n\n");

                    Console.WriteLine($"Esperando {MilisegundosEspera / 1000} segundos.");
                    Thread.Sleep(MilisegundosEspera);

                }
                else
                {
                    Console.WriteLine($"Esperando {MilisegundosEspera/1000} segundos.");
                    Thread.Sleep(MilisegundosEspera); // Espera e intenta de nuevo
                }
            }
        }

        public static void ActualizarEstadoSolicitud()
        {
            int Cantidad = new SP("bd_reapp").Execute("usp_ActualizarAutomaticoEstadosSolicitud").Rows[0][0].ToString().ToInt();
            if (Cantidad > 0)
            {
                Console.WriteLine($"{Cantidad} estado/s actualizado/s.");
            }
            else
            {
                Console.WriteLine("No hay estados para actualizar.");
            }
        }

        public static void ValidarVigenciaDocumentos()
        {
            DataTable dt = new SP("bd_reapp").Execute("usp_GetEntidadesVencimiento");
            Console.Write("Enviando mails de vencimiento.");

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Console.Write(".");

                    HTMLBuilder HTML = new HTMLBuilder("REAPP - Vencimiento de Documentacion", "GenericMailTemplate.html");
                    HTML.AppendTexto("Buenas tardes.");
                    HTML.AppendSaltoLinea(2);
                    HTML.AppendTexto($"Le informamos que la documentación legal de su {dr["Tipo"].ToString()} en el sistema REAPP ha vencido. Por favor renueve su documentación lo antes posible para poder continuar solicitando REAs.");

                    string cuerpo = HTML.ConstruirHTML();

                    MailController mail = new MailController("REAPP - Vencimiento de Documentación", cuerpo);
                    mail.Add(dr["Nombre"].ToString(), dr["Email"].ToString());

                    bool Exito = mail.Enviar();
                }

                Console.WriteLine($"\n{dt.Rows.Count} mails enviados.");
            }
            else
            {
                Console.WriteLine("\nNo hay mails para enviar.");
            }
        }
    }
}
