using System;
using System.Web;
using System.Web.UI;

namespace REApp
{
    public static class Navegacion
    {
        public static void JSFunction(string FunctionName, string Arguments = "")
        {
            Page page = HttpContext.Current.CurrentHandler as Page;
            if (page != null)
            {
                Guid key = Guid.NewGuid();
                ScriptManager.RegisterStartupScript(page, page.GetType(), key.ToString(), FunctionName + "(" + Arguments + ");\n", true);
            }
        }

        public enum AlertType
        {
            info, question, success, warning, error
        }

        public static void Alert(string Titulo, string Cuerpo, AlertType Tipo, string Redirect = "")
        {
            Cuerpo = Cuerpo.Replace("'", @"\'");
            string Timeout = "0";

            Page page = HttpContext.Current.CurrentHandler as Page;
            if (page != null)
            {
                string jsFnCode = @"setTimeout(function(){
                Swal.fire({
                    title: '{0}',
                    html: '{1}',
                    icon: '{2}',
                    confirmButtonText: 'Aceptar'
                })";

                if (Redirect.Equals(""))
                {
                    jsFnCode = jsFnCode + ";}, " + Timeout + ");";
                }
                else
                {
                    jsFnCode = jsFnCode + @"
                .then((result) => {
                    window.location.href = '{3}';
                });}, " + Timeout + ");";
                }

                jsFnCode = jsFnCode.Replace("{0}", Titulo).Replace("{1}", Cuerpo).Replace("{2}", Tipo.ToString()).Replace("{3}", Redirect);
                Guid key = Guid.NewGuid();
                ScriptManager.RegisterStartupScript(page, page.GetType(), key.ToString(), jsFnCode, true);
            }
        }
    }
}