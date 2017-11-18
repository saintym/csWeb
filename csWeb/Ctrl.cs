using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace csWeb
{

    class Ctrl
    {
        [Route("/")]
        [Route("/home")]
        public void Home(HttpListenerContext context)
        {
            using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
                writer.WriteAsync("This place is Home~");
        }



        [Route("/dashboard")]
        public void Dashboard(HttpListenerContext context)
        {
            using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
                writer.WriteAsync("Stop!");
        }



        [Route("/ErrorPage")]
        public void ErrorPage(HttpListenerContext context)
        {
            using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
                writer.WriteAsync("Freaking Error Man");
        }

    }
}
