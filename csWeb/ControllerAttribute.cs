using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csWeb
{
    class ControllerAttribute : Attribute
    {
        private string mControllerPath = null;

        public string ControllerPath
        {
            get { return mControllerPath; }
            set { mControllerPath = value; }
        }

        public ControllerAttribute(string path)
        {
            ControllerPath = path;
        }
    }
}
