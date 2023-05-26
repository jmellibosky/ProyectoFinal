using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using REApp.Models;
using System.IO;

namespace REApp
{
    /* Clase MailController Documentacion
     * 
     * Constructor: recibe tres parámetros
     * * * Asunto: Título del Mail como cadena
     * * * Cuerpo: Cuerpo del Mail como cadena
     * * * HTML: Valor booleano que indica si el Cuerpo del Mail es contenido HTML o texto plano. Por defecto true.
     * 
     * Método Add(Documento): Agrega un documento al listado de documentos a adjuntar al mail.
     * Método Add(Destinatario): Agrega un destinatario al listado de destinatarios a enviar el mail.
     * Método Add(Nombre, Correo): Crea un destinatario y lo agrega al listado de destinatarios a enviar el mail.
     * Método Enviar(): Establece la conexión con el servidor SMTP, crea el mensaje con los atributos y documentos
     *      especificados y envía el mail a cada uno de los destinatarios.
     *      
     * Secuencia de uso de la clase:
     * * * Construir un objeto MailController
     * * * Añadir los destinatarios con el método Add
     * * * Adjuntar los documentos con el método Add
     * * * Enviar mail con el método Enviar
     */
    public class MailController
    {
        // PARAMETROS DE ENVÍO MAIL
        private string NombreOrigen = "REAPP";
        private string CorreoOrigen = "reapp.utn@gmail.com";
        private string PassCorreo = "bjzkdehztdaerdby";

        // ATRIBUTOS DE ENVÍO MAIL
        private string Asunto { get; set; }
        private string Cuerpo { get; set; }
        private string TipoMail { get; set; }
        private List<Destinatario> Destinatarios { get; set; }
        private List<Documento> Documentos { get; set; }

        public MailController(string Asunto, string Cuerpo, bool HTML = true)
        {
            this.Asunto = Asunto;
            this.Cuerpo = Cuerpo;
            TipoMail = (HTML) ? "html" : "plain";
            Destinatarios = new List<Destinatario>();
            Documentos = new List<Documento>();
        }

        public void Add(Destinatario Destinatario)
        {
            Destinatarios.Add(Destinatario);
        }

        public void Add(string NombreDestinatario, string CorreoDestinatario)
        {
            Destinatario Destinatario = new Destinatario()
            {
                Nombre = NombreDestinatario,
                Correo = CorreoDestinatario
            };

            Destinatarios.Add(Destinatario);
        }

        public void Add(Documento Documento)
        {
            Documentos.Add(Documento);
        }

        public bool Enviar()
        {
            bool Exito = true;

            try
            {
                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    smtpClient.Authenticate(CorreoOrigen, PassCorreo);

                    foreach (Destinatario dest in Destinatarios)
                    {
                        MimeMessage mailMessage = new MimeMessage();
                        mailMessage.From.Add(new MailboxAddress(NombreOrigen, CorreoOrigen));
                        mailMessage.To.Add(new MailboxAddress(dest.Nombre, dest.Correo));
                        mailMessage.Subject = Asunto;

                        if (Documentos.Count > 0)
                        {
                            Multipart multipart = new Multipart("mixed");

                            TextPart Body = new TextPart(TipoMail)
                            {
                                Text = Cuerpo
                            };
                            multipart.Add(Body);

                            foreach (Documento doc in Documentos)
                            {
                                MimePart Adjunto = new MimePart(doc.TipoMIME)
                                {
                                    Content = new MimeContent(new MemoryStream(doc.Datos)),
                                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                                    ContentTransferEncoding = ContentEncoding.Base64,
                                    FileName = doc.Nombre,
                                };

                                multipart.Add(Adjunto);
                            }

                            mailMessage.Body = multipart;
                        }
                        else
                        {
                            mailMessage.Body = new TextPart(TipoMail)
                            {
                                Text = Cuerpo
                            };
                        }

                        smtpClient.Send(mailMessage);
                    }

                    smtpClient.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                Exito = false;
            }

            return Exito;
        }
    }
}