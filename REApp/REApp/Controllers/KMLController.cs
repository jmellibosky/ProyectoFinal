using MagicSQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace REApp
{
    public class KMLController
    {
        private string ColorHexRed = "ff0000ff";
        private string NombreArchivo = "test.kmz";
        private string NombreCarpeta = "test";

        public string GenerarKML(int IdSolicitud)
        {
            string kml = "";

            // HEADER
            kml += AgregarEncabezadoKML();

            //BD
            List<Models.Ubicacion> Ubicaciones;
            using (SP sp = new SP("bd_reapp"))
            {
                Ubicaciones = sp.Execute("usp_GetUbicacionesDeSolicitud", P.Add("IdSolicitud", IdSolicitud)).ToList<Models.Ubicacion>();

                foreach (Models.Ubicacion ubicacion in Ubicaciones)
                {
                    List<Models.PuntoGeografico> PuntosGeograficos = sp.Execute("usp_GetPuntosGeograficosDeUbicacion", P.Add("IdUbicacion", ubicacion.IdUbicacion)).ToList<Models.PuntoGeografico>();

                    if (PuntosGeograficos[0].EsPoligono.Value)
                    {
                        AgregarPoligono(ubicacion.IdUbicacion, ubicacion.Altura, PuntosGeograficos);
                    }
                    else
                    {
                        AgregarCircunferencia(ubicacion.IdUbicacion, ubicacion.Altura, PuntosGeograficos);
                    }
                }
            }

            // FOOTER
            kml += "</Folder></Document></kml>";

            return "";
        }

        public string AgregarEncabezadoKML()
        {
            string kml = "";
            kml += "<?xml version=\"1.0\" encoding=\"UTF-8\"?><kml xmlns=\"http://www.opengis.net/kml/2.2\" xmlns:gx=\"http://www.google.com/kml/ext/2.2\" xmlns:kml=\"http://www.opengis.net/kml/2.2\" xmlns:atom=\"http://www.w3.org/2005/Atom\"><Document>";
            kml += "<name>" + NombreArchivo + "</name>";
            kml += "<StyleMap id=\"m_ylw-pushpin\"><Pair><key>normal</key><styleUrl>#s_ylw-pushpin</styleUrl></Pair><Pair><key>highlight</key><styleUrl>#s_ylw-pushpin_hl</styleUrl></Pair></StyleMap><Style id=\"s_ylw-pushpin_hl\"><IconStyle><scale>1.3</scale><Icon><href>http://maps.google.com/mapfiles/kml/pushpin/ylw-pushpin.png</href></Icon><hotSpot x=\"20\" y=\"2\" xunits=\"pixels\" yunits=\"pixels\"/></IconStyle></Style><StyleMap id=\"inline\"><Pair><key>normal</key><styleUrl>#inline0</styleUrl></Pair><Pair><key>highlight</key><styleUrl>#inline1</styleUrl></Pair></StyleMap><Style id=\"s_ylw-pushpin\"><IconStyle><scale>1.1</scale><Icon><href>http://maps.google.com/mapfiles/kml/pushpin/ylw-pushpin.png</href></Icon><hotSpot x=\"20\" y=\"2\" xunits=\"pixels\" yunits=\"pixels\"/></IconStyle></Style>";
            kml += "<Style id=\"inline0\"><LineStyle><color>" + ColorHexRed + "</color><width>2</width></LineStyle></Style><Style id=\"inline1\"><LineStyle><color>" + ColorHexRed + "</color><width>2</width></LineStyle></Style>";
            kml += "<Folder><name>" + NombreCarpeta +"</name><open>1</open>";

            return kml;
        }

        public string AgregarCircunferencia(int idUbicacion, double altura, List<Models.PuntoGeografico> puntos)
        {
            double latitudInicial = puntos[0].Latitud;
            double longitudInicial = puntos[0].Longitud;
            double radio = puntos[0].Radio.Value;

            string kml = "";
            kml += "<Placemark><name>Circunferencia N°" + idUbicacion + " </name><styleUrl>#inline</styleUrl><LineString><tessellate>1</tessellate><coordinates>";
            
            // ITERA GENERANDO UN PUNTO CADA 6° (PUEDE GENERARSE CON MAYOR O MENOR PRECISIÓN PERO SIMEPRE UTILIZANDO DIVISORES DE 360)
            for (int g = 0; g > -360; g -= 6)
            {
                double latitudN = GetLatitud(latitudInicial, radio, g);
                double longitudN = GetLongitud(longitudInicial, latitudN, radio, g);
                
                kml += longitudN.ToString() + "," + latitudN.ToString() + "," + altura.ToString() + " ";
            }
            kml = kml.Substring(0, kml.Length - 1); // ELIMINA EL ÚLTIMO ESPACIO AGREGADO

            kml += "</coordinates></LineString></Placemark>";
            return kml;
        }

        public double GetLatitud(double latitudInicial, double radio, double grados)
        {
            return latitudInicial + Math.Sin(GetRadianes(grados)) * radio / 110.574;
        }

        public double GetLongitud(double longitudInicial, double latitud, double radio, double grados)
        {
            return longitudInicial + Math.Cos(GetRadianes(grados)) * radio / (110.574 * Math.Cos(latitud));
        }

        public double GetRadianes(double grados)
        {
            return grados * Math.PI / 180;
        }

        public string AgregarPoligono(int idUbicacion, double altura, List<Models.PuntoGeografico> puntos)
        {
            string kml = "";
            kml += "<Placemark><name>Polígono N°" + idUbicacion + "</name><styleUrl>#m_ylw-pushpin</styleUrl><Polygon><tessellate>1</tessellate><outerBoundaryIs><LinearRing><coordinates>";

            foreach (Models.PuntoGeografico punto in puntos)
            {
                kml += punto.Longitud.ToString() + "," + punto.Latitud.ToString() + "," + altura.ToString() + " ";
            }
            // AGREGAR PUNTO INICIAL
            kml += puntos[0].Longitud.ToString() + "," + puntos[0].Latitud.ToString() + "," + altura.ToString();

            kml += "</coordinates></LinearRing></outerBoundaryIs></Polygon></Placemark>";
            return kml;
        }
    }
}