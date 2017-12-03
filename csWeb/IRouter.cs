using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace csWeb
{
    public interface IRouter
    {
        void Route(HttpListenerContext context, string url);

        void RegisterController(Ctrl ctrl);
    }
}
