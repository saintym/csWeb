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
        private PathTree mPathTree;
        private Ctrl ctrl;

        public Router()
        {
            mPathTree = new PathTree();
            ctrl = new Ctrl();
        }

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

        public void AddController()
        {
            //ctrl.Context = context;

            MethodInfo[] methodInfos = typeof(Ctrl).GetMethods();
            foreach (MethodInfo methodInfo in methodInfos)
            {
                RouteAttribute[] routeAttributes = methodInfo.GetCustomAttributes<RouteAttribute>().ToArray();
                foreach (var attribute in routeAttributes)
                {
                    RouteAttribute path = (RouteAttribute)attribute;
                    mPathTree.Add(path.SubControllerPath);
                    mPathTree.GetPathNode(path.SubControllerPath).ActMethod =
                        (dictionary) => methodInfo.Invoke(ctrl, new object[] { dictionary });
                    /*
                    {
                        List<Dictionary<string, string>> id = new List<Dictionary<string, string>>();
                        ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                        foreach(var parameterInfo in parameterInfos)
                        {
                            parameterInfo.GetCustomAttribute<PathAttribute>().dictionary.
                        }
                        methodInfo.Invoke(ctrl, id.ToArray());
                    };
                    */
                }
            }
        }

        public void ActivateController(HttpListenerContext context)
        {
            ctrl.Context = context;
            Dictionary<string, string> queries = new Dictionary<string, string>();
            object[] routerParameter;
            string[] notSplitedURL = context.Request.RawUrl.Split('?');
            this.url = notSplitedURL[0];


            if (notSplitedURL.Length > 1)
            {
                string query = notSplitedURL[1];
                queries = GetDictionaryKeyValue(query);
                routerParameter = new object[] { queries };
            }
            else
                routerParameter = new object[] { };

            
            if (mPathTree.isExistPathNode(url))
            {
                ActivateCtrlMethodInternal(url, routerParameter);
                return;
            }

            if (mPathTree.GetPathNodeContainId(url) != null)
            {
                Node node = mPathTree.GetPathNodeContainId(url);
                Dictionary<string, string> dictionary = node.dictionary;
                //routerParameter.SetValue(dictionary, routerParameter.Length);
                node.ActMethod(dictionary);
            }
            ctrl.ErrorPage();
        }


        /*
        var routeAttributes = controllerType.GetMethods().Select(info => info.GetCustomAttribute<RouteAttribute>());
        var test = controllerType.GetMethods().Select(info => Tuple.Create(info, info.GetCustomAttributes<RouteAttribute>()));
        */


        private void ActivateCtrlMethodInternal(string path, object[] routerParameter)
        {
            MethodInfo[] methodInfos = typeof(Ctrl).GetMethods();
            foreach (MethodInfo methodInfo in methodInfos)
            {
                RouteAttribute[] routeAttributes = methodInfo.GetCustomAttributes<RouteAttribute>().ToArray();
                foreach (var attribute in routeAttributes)
                {
                    RouteAttribute routerPath = (RouteAttribute)attribute;
                    if (routerPath.SubControllerPath == url)
                    {
                        methodInfo.Invoke(ctrl, routerParameter);
                        return;
                    }
                }
            }
        }

        /**
        public string[] Substring(string path)
        {
            string[] url_dict = new string[2];

            if (path == "")
            {
                url_dict[0] = "/home";
                url_dict[1] = null;
                return url_dict;
            }

            int queryPosition = path.IndexOf('?');
            if(queryPosition == -1)
            {
                url_dict[0] = path;
                url_dict[1] = null;
                return url_dict;
            }
            else
            {
                url_dict[0] = path.Substring(0, queryPosition-1);
                url_dict[1] = path.Substring(queryPosition + 1);
                return url_dict;
            }
            
        }
       */

        private Dictionary<string, string> GetDictionaryKeyValue(string urlQuery)
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
