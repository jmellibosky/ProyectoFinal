using System;
using System.Web;
using System.Web.UI;

namespace REApp
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReadCookie((string)Session["Username"]);
            try
            {
                if (Session["Username"] == null)
                {
                    Response.Redirect("/Forms/UserLogin.aspx");
                }
            }
            catch (Exception)
            {
                Response.Redirect("/Forms/UserLogin.aspx");
            }

            
        }

        //Gestion de Cookies
        public static string ReadCookie(string cookieName)
        {
            HttpCookie httpCookie = HttpContext.Current.Request.Cookies[cookieName];
            string value = null;
            if (httpCookie != null)
            {
                value = httpCookie.Value;
                try
                {
                    value = Decrypt(value);
                }
                catch (Exception)
                {
                    DeleteCookie(cookieName);
                    value = null;
                }
            }
            return value;
        }
        public static void DeleteCookie(string cookieName)
        {
            HttpCookie httpCookie = new HttpCookie(cookieName);
            httpCookie.Value = "";
            httpCookie.Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies.Add(httpCookie);
        }

        //Decripta para leer la cookie
        public static string Decrypt(string value)
        {
            //encriptacion
            return value;
        }
    }
}