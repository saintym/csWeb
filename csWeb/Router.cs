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
            string spliturl = notSplitedURL[0];
            
            if (notSplitedURL.Length > 1)
            {
                string query = notSplitedURL[1];
                queries = GetDictionaryKeyValue(query);
                routerParameter = new object[] { queries };
            }
            else
                routerParameter = new object[] { };
                
                
            if (mPathTree.isExistPathNode(spliturl))
            {
                Node node = mPathTree.GetPathNode(spliturl);
                node.ActMethod(url);
            }

            if (mPathTree.GetPathNodeContainId(url) != null)
            {
                Node node = mPathTree.GetPathNodeContainId(spliturl);
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
                        Dictionary<string, string> QueryDic = new Dictionary<string, string>();
                        string queries = null;
                        foreach (string PathSegment in divdPath)
                        {
                            if (PathSegment.Contains("{"))
                            {
                                indexList.Add(indexListNum);
                            }
                            indexListNum++;
                        }

                        if (url.Contains("?"))
                        {
                            queries = url.Split('?')[1];
                            url = url.Split('?')[0];
                            QueryDic = DivideQueries(queries);
                        }

                        

                        string[] divdURL = GetDividedURLInternal(url);
                        ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                        List<string> valueList = new List<string>();


                        int indexArrayNum = 0;
                        foreach (var parameterInfo in parameterInfos)
                        {
                            //object attr = parameterInfo.GetCustomAttribute<PathAttribute>() ?? parameterInfo.GetCustomAttribute<QueryAttribute>();
                            if (parameterInfo.GetCustomAttribute<PathAttribute>() != null)
                            {
                                string dicKey = parameterInfo.GetCustomAttribute<PathAttribute>().Key;
                                Dictionary<string, string> paramDic = parameterInfo.GetCustomAttribute<PathAttribute>().dictionary;
                                paramDic.Add(dicKey, divdURL[indexList.ToArray()[indexArrayNum++]]);
                                valueList.Add(paramDic[dicKey]);
                            }
                            else if(parameterInfo.GetCustomAttribute<QueryAttribute>() != null)
                            {
                                string dicKey = parameterInfo.GetCustomAttribute<QueryAttribute>().Key;
                                Dictionary<string, string> paramDic = parameterInfo.GetCustomAttribute<QueryAttribute>().dictionary;
                                foreach (string key in QueryDic.Keys)
                                {
                                    if(dicKey == key)
                                        paramDic.Add(dicKey, QueryDic[key]);
                                }
                                valueList.Add(paramDic[dicKey]);
                            }
                        }

                        methodInfo.Invoke(mCtrl, valueList.ToArray());
                    };
                }
            }
            
        }

        private Dictionary<string, string> DivideQueries(string url)
        {
            string[] divdurl = url.Split('&');
            Dictionary<string, string> DividedQueries = new Dictionary<string, string>();
            foreach (string divdurlSegm in divdurl)
            {
                string[] KeyValue = divdurlSegm.Split('=');
                DividedQueries.Add(KeyValue[0], KeyValue[1]);
            }
            return DividedQueries;
        }

        private string[] GetDividedURLInternal(string path)
        {
            string[] result = path.Split('/');
            result = result.Skip(1).ToArray();

            return result;
        }
        
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

