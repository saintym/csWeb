using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csWeb
{
    class Node
    {
        //private Dictionary<string, string> mKeyValue { get; set; }

        public Node Parent { get; }
        public string Path { get; }
        public List<Node> Children { get; set; }
        public int Rank { get; }
        public Dictionary<string, int> dictionary { get; set; }

        public Node(Node parent, string path, int rank)
        {
            Parent = parent;
            Path = path;
            Rank = rank;
            Children = new List<Node>();
        }

        public bool IsGetChild(Node node)
        {
            foreach(Node child in Children)
            {
                if (child.Equals(node))
                    return true;
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj is Node)
                return Path == ((Node)obj).Path;

            return false;
        }

    }
}
