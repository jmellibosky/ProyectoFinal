using MagicSQL;
using REApp.Models;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Web;
using static REApp.Navegacion;

namespace REApp.Forms
{
    public partial class UserLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string KML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><kml xmlns=\"http://www.opengis.net/kml/2.2\" xmlns:gx=\"http://www.google.com/kml/ext/2.2\" xmlns:kml=\"http://www.opengis.net/kml/2.2\" xmlns:atom=\"http://www.w3.org/2005/Atom\"><Document><name>PruebaParseo.kmz</name><Style id=\"inline\"><LineStyle><color>ff0000ff</color><width>2</width></LineStyle></Style><Style id=\"inline0\"><LineStyle><color>ff0000ff</color><width>2</width></LineStyle></Style><StyleMap id=\"inline1\"><Pair><key>normal</key><styleUrl>#inline0</styleUrl></Pair><Pair><key>highlight</key><styleUrl>#inline</styleUrl></Pair></StyleMap><StyleMap id=\"m_ylw-pushpin\"><Pair><key>normal</key><styleUrl>#s_ylw-pushpin</styleUrl></Pair><Pair><key>highlight</key><styleUrl>#s_ylw-pushpin_hl</styleUrl></Pair></StyleMap><Style id=\"s_ylw-pushpin\"><IconStyle><scale>1.1</scale><Icon><href>http://maps.google.com/mapfiles/kml/pushpin/ylw-pushpin.png</href></Icon><hotSpot x=\"20\" y=\"2\" xunits=\"pixels\" yunits=\"pixels\"/></IconStyle></Style><Style id=\"s_ylw-pushpin_hl\"><IconStyle><scale>1.3</scale><Icon><href>http://maps.google.com/mapfiles/kml/pushpin/ylw-pushpin.png</href></Icon><hotSpot x=\"20\" y=\"2\" xunits=\"pixels\" yunits=\"pixels\"/></IconStyle></Style><Folder><name>PruebaParseo</name><open>1</open><Placemark><name>Medición del círculo </name><styleUrl>#inline1</styleUrl><LineString><tessellate>1</tessellate><coordinates>-62.85551606325716,-38.57919811499237,90.57656836163601 -63.0443518557928,-38.77157631146859,69.13017740896026 -63.2549695231838,-38.95003208966329,56.53171342877483 -63.48580264473681,-39.11309884625256,58.01345158003517 -63.73507998246814,-39.25942585756668,62.85425222620594 -64.00083837513905,-39.38779227689738,70.11616279515026 -64.28093901233825,-39.49712014489771,95.62766942377185 -64.57308698692421,-39.58648616716101,123.8926729007869 -64.87485390786999,-39.65513202244112,149.099915189071 -65.18370323898095,-39.70247298371538,167.7578637392739 -65.49701791908572,-39.72810466227513,183.7668308978919 -65.81212972323834,-39.73180772152128,201.4340608141116 -66.12634974876612,-39.71355045079283,229.0038130392416 -66.43699936002088,-39.67348913847831,264.3357241318134 -66.74144090507771,-39.61196623555255,301.1087101573002 -67.0371075278897,-39.52950635301095,331.0665433109853 -67.321531439797,-39.42681018686851,274.2797567818641 -67.59237008184492,-39.30474651005258,262.9179466371608 -67.84742969925358,-39.16434240960266,349.9215772476551 -68.08468595537886,-39.00677197855487,417.093509211488 -68.30230132761741,-38.83334369378674,353.7501373729898 -68.49863914483382,-38.64548672360499,368.3489143045285 -68.67227423840009,-38.44473641224846,375.6172256057563 -68.82200028120683,-38.23271918354049,376.6877180553515 -68.94683397671855,-38.01113709385234,377.7571666818341 -69.04601633049131,-37.78175224678914,476.2451192266803 -69.11901128823659,-37.54637126017034,623.1939371601767 -69.16550205756151,-37.30682995153166,772.0477016257217 -69.18538544613988,-37.0649783830038,928.5320816843904 -69.17876454932338,-36.82266638131325,1084.280349925293 -69.14594010768822,-36.58172962485359,1214.60766693818 -69.08740083259086,-36.34397636807842,1387.376903485765 -69.00381296835528,-36.11117485439695,1562.652002225756 -68.89600932593056,-35.88504145259278,1728.867101608947 -68.7649779871232,-35.66722953860778,1526.082649033465 -68.61185084280962,-35.45931913423847,1279.939734270782 -68.43789209442258,-35.26280730665133,965.2912379752461 -68.24448681660994,-35.07909932731741,700.0349860628902 -68.03312965101377,-34.90950058562919,615.3188463254736 -67.80541367699851,-34.75520925069925,554.8792249353639 -67.56301948496267,-34.61730967427444,483.6221317884336 -67.30770446147129,-34.49676652797373,447.964483988603 -67.04129228254715,-34.39441966886108,421.8954205932145 -66.76566260165099,-34.31097972843866,416.1139609603819 -66.48274091169613,-34.24702442128644,495.4740398338705 -66.19448855539123,-34.20299557063725,544.6642824301385 -65.90289285481727,-34.17919684907462,533.4298948800009 -65.60995732899246,-34.17579223323386,478.6347586664186 -65.31769196690587,-34.19280517188353,395.7126513382398 -65.02810352283082,-34.23011846710308,335.4168759542028 -64.74318580049118,-34.28747486851877,277.5709197901336 -64.46490989276944,-34.36447838079378,231.2563627301449 -64.19521434415863,-34.46059628486778,187.4215790274205 -63.93599520420459,-34.57516187387913,158.5319669593769 -63.68909594200986,-34.70737790532864,143.7725794133126 -63.45629719480078,-34.85632077187464,130.0507959628565 -63.2393063280068,-35.02094539416544,117.1680424186626 -63.03974679072024,-35.20009084024245,108.9469509924006 -62.85914725928444,-35.39248667716367,101.1951036841404 -62.69893057357821,-35.59676006141633,93.51105881671111 -62.56040248575604,-35.81144357517088,86.1056194845095 -62.44474026010861,-36.03498381517947,93.15536397980078 -62.35298118551728,-36.26575073980019,103.3988120913869 -62.28601108867699,-36.502047776858,113.7737804054728 -62.24455296657252,-36.74212269044632,132.1073869370842 -62.22915589000349,-36.98417919794951,155.1567519397031 -62.24018436527535,-37.22638931918608,183.0966500298139 -62.27780837709528,-37.4669064273743,249.3839966153317 -62.34199437039182,-37.70387895645606,282.5401612570244 -62.43249745995906,-37.93546470119397,289.4578257873629 -62.54885518190464,-38.15984562558945,218.8013999278702 -62.69038311702001,-38.37524307199799,143.1367846045286 -62.85551606325716,-38.57919811499237,90.57656836163601 </coordinates></LineString></Placemark><Placemark><name>Polígono sin título</name><styleUrl>#m_ylw-pushpin</styleUrl><Polygon><tessellate>1</tessellate><outerBoundaryIs><LinearRing><coordinates>-64.13336460480015,-31.41131813524512,0 -68.75435584827586,-32.88999945760718,0 -58.43113552859725,-34.62290161688465,0 -60.62752949767734,-32.96047196973467,0 -64.13336460480015,-31.41131813524512,0 </coordinates></LinearRing></outerBoundaryIs></Polygon></Placemark></Folder></Document></kml>";

            KMLController KMLCtrl = new KMLController(KML);
            KMLCtrl.ParsearKML();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string correo = txt_email.Value;
                string password = txt_password.Value;
                string saltkey;
                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();
                int flagLogin;

                bool flagCaptcha = false;


                //Checkeamos que el captcha este correcto
                if (String.IsNullOrEmpty(Recaptcha.Response))
                {
                    //Aca iria una alerta para mostrar que se tiene que no tiene que estar vacio
                    flagCaptcha = false;
                    Alert("Error", "Por favor, complete el captcha.", AlertType.error);
                }
                else
                {
                    if (Recaptcha.Verify().Success)
                    {
                        //Funciona, no hace falta mostrar mas nada, solo prender la bandera o redireccionar
                        flagCaptcha = true;
                    }
                    else
                    {
                        //Mostrar error diciendo que no es success, con nuestro tipo de captcha capaz no hace falta 

                        flagCaptcha = false;
                        Alert("Error", "Por favor, complete el captcha.", AlertType.error);
                    }
                }

                //Podriamos implementar SweetAlerts para dejarlo mas bonito
                using (SP sp = new SP("bd_reapp"))
                {
                    dt = sp.Execute("usp_CorreoSaltCheck", P.Add("correo", correo));
                    if (dt.Rows.Count > 0)
                    {
                        saltkey = dt.Rows[0][0].ToString();
                    }
                    else
                    {
                        saltkey = "";
                    }
                }

                string hashedpass = SecurityHelper.HashPassword(password, saltkey, 10101, 70);

                using (SP sp2 = new SP("bd_reapp"))
                {
                    dt2 = sp2.Execute("usp_CorreoPassCheck", P.Add("correo", correo), P.Add("pass", hashedpass));
                    flagLogin = dt2.Rows.Count;
                }

                if (flagLogin == 1 && flagCaptcha == true)
                {
                    //Ver si esto es completamente seguro
                    string idUsuario = dt2.Rows[0][0].ToString();

                    Usuario usuario = new Usuario().Select(idUsuario.ToInt());

                    string nombreuser = usuario.Nombre;
                    string apellidouser = usuario.Apellido;
                    string idRol = usuario.IdRol.ToString();
                    string nombrecompleto = nombreuser + " " + apellidouser;
                    bool ValidacionCorreo = usuario.ValidacionCorreo;

                    Session["Username"] = nombrecompleto;
                    Session["UsuarioCompleto"] = dt2;
                    Session["IdUsuario"] = idUsuario;
                    Session["IdRol"] = idRol;

                    if (ValidacionCorreo)
                    {
                        CreateCookie(nombrecompleto, idUsuario);
                        Response.Redirect("/Forms/HomeDash/HomeDash.aspx");
                    }
                    else
                    {
                        Alert("Validación de Cuenta", "Es necesario que valide su cuenta antes de ingresar.", AlertType.error);
                    }
                }
                else
                {
                    txt_password.Value = "";
                    txt_email.Value = "";
                    txt_email.Focus();

                }
                if (flagLogin == 0)
                {
                    Alert("Datos erróneos.", "Se ha producido un problema al iniciar sesión. Comprueba el correo electrónico y la contraseña o crea una cuenta.", AlertType.error, "/Forms/UserLogin.aspx");
                }
                if (flagCaptcha == false)
                {
                    Alert("Error", "Por favor, verificar Captcha para iniciar sesión.", AlertType.error);
                }
            }
            catch (Exception)
            {
                Alert("Datos erróneos.", "Se ha producido un problema al iniciar sesión. Comprueba el correo electrónico y la contraseña o crea una cuenta.", AlertType.error, "/Forms/UserLogin.aspx");
                //throw;
                
            }

        }

        //Inicio la funcion general
        public class SecurityHelper
        {
            //Creamos el hash con el salt
            public static string HashPassword(string password, string salt, int nIterations, int nHash)
            {
                var saltBytes = Convert.FromBase64String(salt);

                using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, nIterations))
                {
                    return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(nHash));
                }
            }
        }

        public static void CreateCookie(string cookieName, string value)
        {
            HttpCookie httpCookie = new HttpCookie(cookieName);
            value = Encrypt(value);
            httpCookie.Value = value;
            httpCookie.Expires = DateTime.Now.AddYears(50);
            HttpContext.Current.Response.Cookies.Add(httpCookie);
        }

        //Encripta la cookie, falta ver q metodo de encriptacion se usa
        public static string Encrypt(string value)
        {
            //encriptacion
            return value;
        }

    }
}
