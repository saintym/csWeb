using System.Net;

namespace csWeb
{
    public class Server
    {
        public const string URL = "http://localhost:8000/";
        
        private HttpListener mlistener;
        private Router router;
        private Ctrl ctrl; 

        public Server(IRouter router)
        {
            mlistener = new HttpListener();
            mlistener.Prefixes.Add(URL);
        }

        //public void Initialize() { mlistener.Start(); }
        public void Stop() { mlistener.Stop(); }

        public async void Create()
        {
            router = new Router();
            router.RegisterController(ctrl);
        }

        public async void Start()
        {
            mlistener.Start();
            while (true)
            {
                HttpListenerContext context = await mlistener.GetContextAsync();
                string url = context.Request.RawUrl;
                router.Route(context, url);
            }
        }
    }
}
