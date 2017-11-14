using System.IO;
using System.Net;

namespace csWeb.Controller
{
    class ErrorPage
    {
        //public ErrorPage() { }
        public ErrorPage(HttpListenerContext context)
        {
            using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
                writer.WriteAsync("Get The Fuck out of Here !");
        }
    }
}
