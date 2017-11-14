using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace csWeb
{
    public class Router
    {
        private string mURL;
        private object mController;
        private Type mControllerType;


        public Router(HttpListenerContext httpListenerContext)
        {
            url = httpListenerContext.Request.RawUrl;
            //mhttpListener = httpListenerContext;
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


        public void ActivateController(HttpListenerContext context)
        {
            controllerType = Type.GetType("csWeb" + GetControllerNameInternal()); // WebServer.Controller.protected

            try
            {
                controller = Activator.CreateInstance(controllerType, context); //CreateInstance(controllerType);
            }
            catch (ArgumentNullException)
            {
                controller = Activator.CreateInstance(Type.GetType("csWeb.Controller.ErrorPage"), context);
            }
        }


        private string GetControllerNameInternal()
        {
            if (url == "/")
                return ".Controller.Home";

            string[] splitedURL = ("Controller" + url).Split('/');

            string controllerName = null;
            foreach (string url in splitedURL)
            {
                controllerName += "." + url;
            }

            return controllerName;
        }

    }
}