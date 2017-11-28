using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace csWeb
{
    [Controller("/home")]
    class Ctrl
    {
        private HttpListenerContext mContext;

        public HttpListenerContext Context
        {
            get { return mContext; }
            set { mContext = value; }
        }


        public Ctrl(HttpListenerContext context)
        {
            this.mContext = context;
        }


        //[Route("/")]
        [Route("/home")]
        public void Home()
        {
            using (StreamWriter writer = new StreamWriter(Context.Response.OutputStream))
                writer.WriteAsync("This place is Home~");
        }


        [Route("/dashboard")]
        public void Dashboard()
        {
            using (StreamWriter writer = new StreamWriter(Context.Response.OutputStream))
                writer.WriteAsync("Stop!");
        }


        [Route("/board")]
        public void Board(Dictionary<string, string> queries)
        {
            using (StreamWriter writer = new StreamWriter(Context.Response.OutputStream))
            {
                foreach (KeyValuePair<string, string> kv in queries)
                {
                    writer.WriteAsync($"Key : {kv.Key} , Value : {kv.Value}\n");
                }
            }
        }


        [Route("/member/{id}")]
        public void Member(Dictionary<string, string> paths)
        {
            using (StreamWriter writer = new StreamWriter(Context.Response.OutputStream))
            {
                foreach (KeyValuePair<string, string> kv in paths)
                {
                    writer.WriteAsync($"Key : {kv.Key} , Value : {kv.Value}\n");
                }
                //writer.WriteAsync("Member, Huhh?");
            }
        }
        

        [Route("/ErrorPage")]
        public void ErrorPage()
        {
            using (StreamWriter writer = new StreamWriter(Context.Response.OutputStream))
                writer.WriteAsync("Freaking Error Man");
        }


    }
}
