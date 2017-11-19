using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace csWeb
{
    public class Router
    {
        private string mURL;
        private object mController;
        private Type mControllerType;


        public string url
        {
            get { return mURL; }
            set { mURL = value; }
        }

        public object controller
        {
            get { return mController; }
            set { mController = value; }
        }

        public Type controllerType
        {
            get { return mControllerType; }
            set { mControllerType = value; }
        }


        public void ActivateController(HttpListenerContext context)
        {
            Ctrl ctrl = new Ctrl();
            Dictionary<string, string> queries = new Dictionary<string, string>();
            object[] routerParameter;

            string[] notSplitedURL = context.Request.RawUrl.Split('?');

            this.url = notSplitedURL[0];

            if (notSplitedURL.Length > 1)
            {
                string query = notSplitedURL[1];
                queries = GetDictionary(query);
                routerParameter = new object[] { context, queries };
            }
            else
                routerParameter = new object[] { context };


            MethodInfo[] methodInfos = typeof(Ctrl).GetMethods();
            foreach (MethodInfo methodInfo in methodInfos)
            {
                RouteAttribute[] routeAttributes = methodInfo.GetCustomAttributes<RouteAttribute>().ToArray();
                foreach (var attribute in routeAttributes)
                {
                    RouteAttribute path = (RouteAttribute)attribute;
                    if (path != null)
                    {
                        if (path?.controllerPath == url)
                        {
                            methodInfo.Invoke(ctrl, routerParameter);
                            return;
                        }
                    }
                }
            }
            ctrl.ErrorPage(context);

            /*
            var routeAttributes = controllerType.GetMethods().Select(info => info.GetCustomAttribute<RouteAttribute>());
            var test = controllerType.GetMethods().Select(info => Tuple.Create(info, info.GetCustomAttributes<RouteAttribute>()));
            */
        }



        private Dictionary<string, string> GetDictionary(string urlQuery)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string[] splitedUrl = urlQuery.Split('&');

            foreach (string KeyValue in splitedUrl)
            {
                string[] key_value = KeyValue.Split('=');
                dictionary.Add(key_value[0], key_value[1]);
            }

            return dictionary;
        }



        private string GetControllerNameInternal()
        {
            if (url == "/")
                return ".Home";

            string controllerName = url.Replace('/', '.');


            return controllerName;
        }
    }
}