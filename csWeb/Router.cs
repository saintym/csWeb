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
    public class Router : IRouter
    {
        private string mURL;
        private object mController;
        private Type mControllerType;
        private PathTree mPathTree;
        private Ctrl mCtrl;

        public Router()
        {
            mPathTree = new PathTree();
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


        public void Route(HttpListenerContext context, string url)
        {
            mCtrl.SetContext(context);
            Dictionary<string, string> queries = new Dictionary<string, string>();
            object[] routerParameter;
            string[] notSplitedURL = url.Split('?');
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
                ActivateCtrlInternal(url, routerParameter);
                return;
            }

            if (mPathTree.GetPathNodeContainId(url) != null)
            {
                Node node = mPathTree.GetPathNodeContainId(url);
                node.ActMethod(url);
            }

            mCtrl.ErrorPage();
        }

        public void RegisterController(Ctrl ctrl)
        {
            ctrl = new Ctrl();
            mCtrl = ctrl;

            MethodInfo[] methodInfos = typeof(Ctrl).GetMethods();
            foreach (MethodInfo methodInfo in methodInfos)
            {
                RouteAttribute[] routeAttributes = methodInfo.GetCustomAttributes<RouteAttribute>().ToArray();
                foreach (var attribute in routeAttributes)
                {
                    RouteAttribute path = (RouteAttribute)attribute;
                    mPathTree.Add(path.SubControllerPath);

                    string[] divdPath = GetDividedURLInternal(path.SubControllerPath);

                    mPathTree.GetPathNode(path.SubControllerPath).ActMethod = (string url) =>
                    {
                        int indexListNum = 0;
                        List<int> indexList = new List<int>();
                        foreach (string PathSegment in divdPath)
                        {
                            if (PathSegment.Contains("{"))
                            {
                                indexList.Add(indexListNum);
                            }
                            indexListNum++;
                        }

                        string[] divdURL = GetDividedURLInternal(url);
                        ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                        List<string> valueList = new List<string>();
                        int indexArrayNum = 0;
                        List<Dictionary<string, string>> dicList = new List<Dictionary<string, string>>();
                        foreach (var parameterInfo in parameterInfos)
                        {
                            string dicKey = parameterInfo.GetCustomAttribute<PathAttribute>().Key;
                            Dictionary<string, string> paramDic = parameterInfo.GetCustomAttribute<PathAttribute>().dictionary;
                            paramDic.Add(dicKey, divdURL[indexList.ToArray()[indexArrayNum++]]);
                            dicList.Add(paramDic);
                            valueList.Add(paramDic[dicKey]);
                        }


                        methodInfo.Invoke(mCtrl, valueList.ToArray());
                    };
                }
            }
            //(Dictionary) => methodInfo.Invoke(ctrl, Dictionary.Values.ToArray());
        }

        private string[] GetDividedURLInternal(string path)
        {
            string[] result = path.Split('/');
            result = result.Skip(1).ToArray();

            return result;
        }

        /*
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

                    string[] divdPath = GetDividedURLInternal(path.SubControllerPath);

                    mPathTree.GetPathNode(path.SubControllerPath).ActMethod = (string url) =>
                    {
                        int indexListNum = 0;
                        List<int> indexList = new List<int>();
                        foreach (string PathSegment in divdPath)
                        {
                            if (PathSegment.Contains("{"))
                            {
                                indexList.Add(indexListNum);
                            }
                            indexListNum++;
                        }

                        string[] divdURL = GetDividedURLInternal(url);
                        ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                        List<string> valueList = new List<string>();
                        int indexArrayNum = 0;
                        List<Dictionary<string, string>> dicList = new List<Dictionary<string, string>>();
                        foreach (var parameterInfo in parameterInfos)
                        {
                            string dicKey = parameterInfo.GetCustomAttribute<PathAttribute>().Key;
                            Dictionary<string, string> paramDic = parameterInfo.GetCustomAttribute<PathAttribute>().dictionary;
                            paramDic.Add(dicKey, divdURL[indexList.ToArray()[indexArrayNum++]]);
                            dicList.Add(paramDic);
                            valueList.Add(paramDic[dicKey]);
                        }
                        
                        
                        methodInfo.Invoke(ctrl, valueList.ToArray());
                    };
                }
            }
            //(Dictionary) => methodInfo.Invoke(ctrl, Dictionary.Values.ToArray());
        }
        */
        /*
        public void ActivateController(HttpListenerContext context, string url)
        {
            ctrl.Context = context;
            Dictionary<string, string> queries = new Dictionary<string, string>();
            object[] routerParameter;
            string[] notSplitedURL = url.Split('?');
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
                //Dictionary<string, string> dictionary = node.dictionary;
                //List<string> list = new List<string>();
                node.ActMethod(url);
            }

            ctrl.ErrorPage();
        }
        */

        /*
        var routeAttributes = controllerType.GetMethods().Select(info => info.GetCustomAttribute<RouteAttribute>());
        var test = controllerType.GetMethods().Select(info => Tuple.Create(info, info.GetCustomAttributes<RouteAttribute>()));
        */

        private void ActivateCtrlInternal(string path, object[] routerParameter)
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
                        methodInfo.Invoke(mCtrl, routerParameter);
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

