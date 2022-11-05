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

        public static void Alert(string Titulo, string Cuerpo, AlertType Tipo)
        {
            Page page = HttpContext.Current.CurrentHandler as Page;
            if (page != null)
            {
                ScriptManager.RegisterStartupScript(
                    page,
                    page.GetType(),
                    "alert",
                    "Swal.fire({ " +
                        $"title: '{Titulo}',  " +
                        $"text: '{Cuerpo}',  " +
                        $"icon: '{Tipo.ToString()}',  " +
                        "confirmButtonText: 'OK'}); ",
                    true
                );
            }
        }
    }
}