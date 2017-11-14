using System.IO;
using System.Net;

namespace csWeb.Controller
{
    class Home
    {
        //public Home() { }
        public Home(HttpListenerContext context)
        {
            using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
                writer.WriteAsync("This place is Home~");
        }
    }
}
