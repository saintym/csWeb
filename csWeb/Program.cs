using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csWeb
{
    class Program
    {
        static void Main(string[] args)
        {
            WebServer server = new WebServer("http://localhost:8000/");
            server.Initialize();
            try
            {
                server.Run();
                Console.WriteLine("서버 실행중 - - - Enter 누르면 종료");
                Console.ReadLine();
            }
            finally
            {
                server.Stop();
            }
        }
    }
}
