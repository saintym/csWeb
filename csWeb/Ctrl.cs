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
    public class Ctrl
    {
        private HttpListenerContext mContext;

        public HttpListenerContext Context
        {
            get { return mContext; }
            set { mContext = value; }
        }

        public Ctrl() { }

        public void SetContext(HttpListenerContext context)
        {
            this.mContext = context;
        }


        [Route("/member/{member_id}/post/{post_id}")]
        public void Member([Path("{member_id}")] string memberID, [Path("{post_id}")] string postID)
        {
            using (StreamWriter writer = new StreamWriter(Context.Response.OutputStream))
            {
                writer.WriteAsync($"Member ID is {memberID}, Post ID is {postID}");
            }
        }
        

        [Route("/member/dafuq")]
        public void MemberDafuq()
        {
            using (StreamWriter writer = new StreamWriter(Context.Response.OutputStream))
                writer.WriteAsync("Dafuq, man! haha!");
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
        public void Board([Query("post")] string postID)
        {
            using (StreamWriter writer = new StreamWriter(Context.Response.OutputStream))
            {
                writer.WriteAsync($"Query the PostID is {postID}");
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
