using System.IO;
using System.Net;


namespace csWeb.Controller
{
    class Protected
    {
        //public Protected() { }
        public Protected(HttpListenerContext context)
        {

            using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
                writer.Write("This Place is Protected by HW");
            // test git
        }
    }
}
