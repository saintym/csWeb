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
            Server server = new Server(new Router());
            server.Create();
            try
            {
                server.Start();
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
