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
            url = context.Request.RawUrl;

            //controllerType = Type.GetType("csWeb" + GetControllerNameInternal()); // csWeb.Controller.protected


            MethodInfo[] methodInfos = typeof(Ctrl).GetMethods();

            foreach (MethodInfo methodInfo in methodInfos)
            {
                RouteAttribute[] routeAttributes = methodInfo.GetCustomAttributes<RouteAttribute>().ToArray();

                foreach (var attribute in routeAttributes)
                {
                    RouteAttribute path = (RouteAttribute)attribute;

                    if (path != null)
                        if (path.controllerPath.Equals(url))
                        {
                            methodInfo.Invoke(ctrl, new object[] { context });
                            return;
                        }
                }
            }

            ctrl.ErrorPage(context);

            /*
            var routeAttributes = controllerType.GetMethods().Select(info => info.GetCustomAttribute<RouteAttribute>());
            var test = controllerType.GetMethods().Select(info => Tuple.Create(info, info.GetCustomAttributes<RouteAttribute>()));
            */

            /*
            try
            {
                controller = Activator.CreateInstance(controllerType, context); //CreateInstance(controllerType);
            }
            catch (ArgumentNullException)
            {
                controller = Activator.CreateInstance(Type.GetType("csWeb.ErrorPage"), context);
            }
            */
        }

        /*
        private string GetControllerNameInternal()
        {
            if (url == "/")
                return ".Controller.Home";

            string controllerName = (".Controller" + url).Replace('/', '.');
            

            return controllerName;
        }
        */

        private string GetControllerNameInternal()
        {
            if (url == "/")
                return ".Home";

            string controllerName = url.Replace('/', '.');


            return controllerName;
        }
    }
}