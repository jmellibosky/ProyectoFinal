using MagicSQL;
using System;
using System.Collections.Generic;
using REApp.Models;

namespace REApp
{
    public class KMLController
    {
        private string KML { get; set; }
        private Solicitud Solicitud { get; set; }
        private bool VerReservasEnPeriodo { get; set; }
        private string NombreCarpeta { get; set; }
        private string NombreArchivo { get; set; }

        public KMLController(Solicitud Solicitud, bool VerReservasEnPeriodo = true)
        {
            KML = "";

            NombreCarpeta = "";
            foreach (string palabra in Solicitud.Nombre.Split(' '))
            {
                NombreCarpeta += palabra.ToLower().Trim();
                NombreCarpeta += "_";
            }
            NombreCarpeta = NombreCarpeta.Substring(0, NombreCarpeta.Length - 1);

            NombreArchivo = NombreCarpeta + ".kml";

            this.Solicitud = Solicitud;
            this.VerReservasEnPeriodo = VerReservasEnPeriodo;
        }

        private string PrincipalRGB = "5a79d6"; // Color para la Solicitud que se está analizando
        private string SecundarioRGB = "ff0055"; // Color para las Solicitudes del período que se está analizando
        
        private string AlphaLinea = "ff"; // Transparencia para el contorno
        private string AlphaArea = "77"; // Transparencia para el área

        public string GenerarKML()
        {
            // HEADER
            KML += AgregarEncabezadoKML();

            // Ubicaciones Solicitud Principal
            using (SP sp = new SP("bd_reapp"))
            {
                List<Ubicacion> Ubicaciones = sp.Execute("usp_GetUbicacionesDeSolicitud",
                    P.Add("IdSolicitud", Solicitud.IdSolicitud)
                ).ToList<Ubicacion>();

                AgregarUbicaciones(Ubicaciones, sp);

                if (VerReservasEnPeriodo)
                {
                    // Ubicaciones Solicitudes en Período
                    Ubicaciones = sp.Execute("usp_GetUbicacionesDeSolicitudesEnPeriodo",
                        P.Add("FechaDesde", Solicitud.FHDesde),
                        P.Add("FechaHasta", Solicitud.FHHasta),
                        P.Add("ExceptoIdSolicitud", Solicitud.IdSolicitud)
                    ).ToList<Ubicacion>();

                    AgregarUbicaciones(Ubicaciones, sp, "style_other");
                }
            }

            // FOOTER
            KML += "</Folder></Document></kml>";

            return KML;
        }

        public string AgregarEncabezadoKML()
        {
            string kml = "";
            kml +=  "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                        "<kml xmlns=\"http://www.opengis.net/kml/2.2\" xmlns:gx=\"http://www.google.com/kml/ext/2.2\" xmlns:kml=\"http://www.opengis.net/kml/2.2\" xmlns:atom=\"http://www.w3.org/2005/Atom\">" +
                            "<Document>" +
                                "<name>" + NombreArchivo + "</name>" +
                                // ESTILO SOLICITUD PRINCIPAL    
                                "<StyleMap id=\"style_main\">" +
                                    "<Pair>" +
                                        "<key>normal</key>" +
                                        "<styleUrl>#sn_ylw-pushpin</styleUrl>" +
                                    "</Pair>" +
                                    "<Pair>" +
                                        "<key>highlight</key>" +
                                        "<styleUrl>#sh_ylw-pushpin</styleUrl>" +
                                    "</Pair>" +
                                "</StyleMap>" +
                                "<Style id=\"sh_ylw-pushpin\">" +
                                    "<IconStyle>" +
                                        "<scale>1.3</scale>" +
                                        "<Icon>" +
                                            "<href>http://maps.google.com/mapfiles/kml/pushpin/ylw-pushpin.png</href>" +
                                        "</Icon>" +
                                        "<hotSpot x=\"20\" y=\"2\" xunits=\"pixels\" yunits=\"pixels\"/>" +
                                    "</IconStyle>" +
                                    "<LineStyle>" +
                                        "<color>" + AlphaLinea + PrincipalRGB + "</color>" +
                                    "</LineStyle>" +
                                    "<PolyStyle>" +
                                        "<color>" + AlphaArea + PrincipalRGB + "</color>" +
                                    "</PolyStyle>" +
                                "</Style>" +
                                "<Style id=\"sn_ylw-pushpin\">" +
                                    "<IconStyle>" +
                                        "<scale>1.1</scale>" +
                                        "<Icon>" +
                                            "<href>http://maps.google.com/mapfiles/kml/pushpin/ylw-pushpin.png</href>" +
                                        "</Icon>" +
                                        "<hotSpot x=\"20\" y=\"2\" xunits=\"pixels\" yunits=\"pixels\"/>" +
                                    "</IconStyle>" +
                                    "<LineStyle>" +
                                        "<color>" + AlphaLinea + PrincipalRGB + "</color>" +
                                    "</LineStyle>" +
                                    "<PolyStyle>" +
                                        "<color>" + AlphaArea + PrincipalRGB + "</color>" +
                                    "</PolyStyle>" +
                                "</Style>" +
                                // ESTILO SOLICITUDES SECUNDARIAS
                                "<StyleMap id=\"style_other\">" +
                                    "<Pair>" +
                                        "<key>normal</key>" +
                                        "<styleUrl>#zn_ylw-pushpin</styleUrl>" +
                                    "</Pair>" +
                                    "<Pair>" +
                                        "<key>highlight</key>" +
                                        "<styleUrl>#zh_ylw-pushpin</styleUrl>" +
                                    "</Pair>" +
                                "</StyleMap>" +
                                "<Style id=\"zh_ylw-pushpin\">" +
                                    "<IconStyle>" +
                                        "<scale>1.3</scale>" +
                                        "<Icon>" +
                                            "<href>http://maps.google.com/mapfiles/kml/pushpin/ylw-pushpin.png</href>" +
                                        "</Icon>" +
                                        "<hotSpot x=\"20\" y=\"2\" xunits=\"pixels\" yunits=\"pixels\"/>" +
                                    "</IconStyle>" +
                                    "<LineStyle>" +
                                        "<color>" + AlphaLinea + SecundarioRGB + "</color>" +
                                    "</LineStyle>" +
                                    "<PolyStyle>" +
                                        "<color>" + AlphaArea + SecundarioRGB + "</color>" +
                                    "</PolyStyle>" +
                                "</Style>" +
                                "<Style id=\"zn_ylw-pushpin\">" +
                                    "<IconStyle>" +
                                        "<scale>1.1</scale>" +
                                        "<Icon>" +
                                            "<href>http://maps.google.com/mapfiles/kml/pushpin/ylw-pushpin.png</href>" +
                                        "</Icon>" +
                                        "<hotSpot x=\"20\" y=\"2\" xunits=\"pixels\" yunits=\"pixels\"/>" +
                                    "</IconStyle>" +
                                    "<LineStyle>" +
                                        "<color>" + AlphaLinea + SecundarioRGB + "</color>" +
                                    "</LineStyle>" +
                                    "<PolyStyle>" +
                                        "<color>" + AlphaArea + SecundarioRGB + "</color>" +
                                    "</PolyStyle>" +
                                "</Style>" +
                                // NOMBRE CARPETA KMZ
                                "<Folder><name>" + NombreCarpeta +"</name><open>1</open>";

            return kml;
        }

        public void AgregarUbicaciones(List<Ubicacion> Ubicaciones, SP sp, string Estilo = "style_main")
        {
            foreach (Ubicacion ubicacion in Ubicaciones)
            {
                List<PuntoGeografico> PuntosGeograficos = sp.Execute("usp_GetPuntosGeograficosDeUbicacion", P.Add("IdUbicacion", ubicacion.IdUbicacion)).ToList<PuntoGeografico>();

                if (PuntosGeograficos.Count > 0)
                {
                    if (PuntosGeograficos[0].EsPoligono.Value)
                    {
                        KML += AgregarPoligono(ubicacion, PuntosGeograficos, Estilo);
                    }
                    else
                    {
                        KML += AgregarCircunferencia(ubicacion, PuntosGeograficos, Estilo);
                    }
                }
            }
        }

        public string AgregarCircunferencia(Ubicacion ubicacion, List<PuntoGeografico> puntos, string Estilo)
        {
            double latitudInicial = puntos[0].Latitud;
            double longitudInicial = puntos[0].Longitud;
            double radio = puntos[0].Radio.Value;

            string kml = "";
            kml += "<Placemark><name>Solicitud N°" + ubicacion.IdSolicitud + " Circunferencia N°" + ubicacion.IdUbicacion + " </name><styleUrl>#" + Estilo + "</styleUrl><Polygon><tessellate>1</tessellate><outerBoundaryIs><LinearRing><coordinates>";
            
            // ITERA GENERANDO UN PUNTO CADA 6° (PUEDE GENERARSE CON MAYOR O MENOR PRECISIÓN PERO SIMEPRE UTILIZANDO DIVISORES DE 360)
            for (int g = 0; g > -360; g -= 6)
            {
                double latitudN = GetLatitud(latitudInicial, radio, g);
                double longitudN = GetLongitud(longitudInicial, latitudN, radio, g);
                
                kml += longitudN.ToString().Replace(',', '.') + "," + latitudN.ToString().Replace(',', '.') + "," + ubicacion.Altura.ToString().Replace(',', '.') + " ";
            }
            double latitudF = GetLatitud(latitudInicial, radio, 0);
            double longitudF = GetLongitud(longitudInicial, latitudF, radio, 0);

            kml += longitudF.ToString().Replace(',', '.') + "," + latitudF.ToString().Replace(',', '.') + "," + ubicacion.Altura.ToString().Replace(',', '.');

            kml += "</coordinates></LinearRing></outerBoundaryIs></Polygon></Placemark>";
            return kml;
        }

        public double GetLatitud(double latitudInicial, double radio, double grados)
        {
            return latitudInicial + Math.Sin(GetRadianes(grados)) * radio / 110.574;
        }

        public double GetLongitud(double longitudInicial, double latitud, double radio, double grados)
        {
            return longitudInicial + Math.Cos(GetRadianes(grados)) * radio / (110.32 * Math.Cos(GetRadianes(latitud)));
        }

        public double GetRadianes(double grados)
        {
            return grados * Math.PI / 180;
        }

        public string AgregarPoligono(Ubicacion ubicacion, List<PuntoGeografico> puntos, string Estilo)
        {
            string kml = "";
            kml += "<Placemark><name>Solicitud N°" + ubicacion.IdSolicitud + " Polígono N°" + ubicacion.IdUbicacion + "</name><styleUrl>#" + Estilo + "</styleUrl><Polygon><tessellate>1</tessellate><outerBoundaryIs><LinearRing><coordinates>";

            foreach (PuntoGeografico punto in puntos)
            {
                kml += punto.Longitud.ToString().Replace(',', '.') + "," + punto.Latitud.ToString().Replace(',', '.') + "," + ubicacion.Altura.ToString().Replace(',', '.') + " ";
            }
            // AGREGAR PUNTO INICIAL
            kml += puntos[0].Longitud.ToString().Replace(',', '.') + "," + puntos[0].Latitud.ToString().Replace(',', '.') + "," + ubicacion.Altura.ToString().Replace(',', '.');

            kml += "</coordinates></LinearRing></outerBoundaryIs></Polygon></Placemark>";
            return kml;
        }
    }
}