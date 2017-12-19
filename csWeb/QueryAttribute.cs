using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csWeb
{
    [AttributeUsage(AttributeTargets.Parameter)]
    class QueryAttribute : Attribute
    {
        private string mKey;
        public Dictionary<string, string> dictionary = new Dictionary<string, string>();

        public string Key
        {
            get { return mKey; }
            set { mKey = value; }
        }

        public QueryAttribute(string key)
        {
            Key = key;
        }
    }
}
