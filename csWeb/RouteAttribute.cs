using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csWeb
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    class RouteAttribute : Attribute
    {

        private string mSubControllerPath = null;


        public string SubControllerPath
        {
            get { return mSubControllerPath; }
            set { mSubControllerPath = value; }
        }

        public RouteAttribute(string path)
        {
            SubControllerPath = path;
        }





    }
}
