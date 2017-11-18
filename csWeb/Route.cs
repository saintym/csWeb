using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csWeb
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    class RouteAttribute : Attribute
    {

        private string mControllerPath = null;

        public string controllerPath
        {
            get { return mControllerPath; }
            set { mControllerPath = value; }
        }

        public RouteAttribute(string path)
        {
            mControllerPath = path;
        }
    }
}
