using System.Net;

namespace csWeb
{
    public class WebServer
    {
        private HttpListener mlistener;
        private string mURI;


        public string uri
        {
            get { return mURI; }
            set
            {
                mURI = value.Trim();
                mlistener.Prefixes.Add(mURI);
            }
        }

        public WebServer(string uri)
        {
            mlistener = new HttpListener();
            this.uri = uri;
        }

        public void Initialize() { mlistener.Start(); }
        public void Stop() { mlistener.Stop(); }

        public async void Run()
        {
            Router router = new Router();
            router.AddController();
            

            while (true)
            {
                HttpListenerContext context = await mlistener.GetContextAsync();
                router.ActivateController(context);

                // using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
                //     await writer.WriteAsync(context.Request.RawUrl);
            }
        }
    }
}
