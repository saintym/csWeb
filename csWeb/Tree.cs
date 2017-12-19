using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csWeb
{
    class PathTree
    {
        private List<Node> Roots { get; set; }

        public PathTree()
        {
            Roots = new List<Node>();
        }

        public void Add(string path)
        {
            Node lastExistPathNode = GetLastExistNodeInternal(path);
            if (lastExistPathNode == null)
                return;

            if (!IsExistPathRoot(path))
                Roots.Add(lastExistPathNode);

            string[] dividedPaths = GetDividedPathInternal(path);
            for (int i = lastExistPathNode.Rank + 1; i < dividedPaths.Length; i++)
            {
                Node newNode = new Node(lastExistPathNode, dividedPaths[i], i);
                lastExistPathNode.Children.Add(newNode);
                lastExistPathNode = newNode;
            }
        }

        private Node GetLastExistNodeInternal(string path)
        {
            if (path == null || path == "")
                return null;

            List<Node> currentNodes = Roots;
            string[] dividedPaths = GetDividedPathInternal(path);
            Node lastExistPathNode = new Node(null, dividedPaths[0], 0);

            bool isExistNode = false;

            foreach (string nodePath in dividedPaths)
            {
                foreach (Node node in currentNodes)
                {
                    if (nodePath == node.Path)
                    {
                        currentNodes = node.Children;
                        lastExistPathNode = node;
                        isExistNode = true;
                        break;
                    }
                }
                if (!isExistNode)
                    return lastExistPathNode;
                else
                    isExistNode = false;
            }

            return null;
        }
        
        public bool isExistPathNode(string path)
        {
            /** Node 를 반환시키는 방식
                List<Node> currentNodes = Roots;
                string[] dividedPaths = GetDividedPathInternal(path);
                Node lastExistPathNode = new Node(null, dividedPaths[0], 0);
                bool isExistNode = false;

                foreach (string nodePath in dividedPaths)
                {
                    foreach (Node node in currentNodes)
                    {
                        if (nodePath == node.Path)
                        {
                            currentNodes = node.Children;
                            lastExistPathNode = node;
                            isExistNode = true;
                            break;
                        }
                    }
                    if (!isExistNode)
                        return null;
                    else
                        isExistNode = false;
                }

                return lastExistPathNode;
            */
            return GetLastExistNodeInternal(path) == null;
        }

        public Node GetPathNode(string path)
        {
            List<Node> currentNodes = Roots;
            string[] dividedPaths = GetDividedPathInternal(path);
            Node lastExistPathNode = new Node(null, dividedPaths[0], 0);
            bool isExistNode = false;

            foreach (string nodePath in dividedPaths)
            {
                foreach (Node node in currentNodes)
                {
                    if (nodePath == node.Path)
                    {
                        currentNodes = node.Children;
                        lastExistPathNode = node;
                        isExistNode = true;
                        break;
                    }
                }
                if (!isExistNode)
                    return null;
                else
                    isExistNode = false;
            }

            return lastExistPathNode;
        }
        
        public Node GetDictionaryPathNode(string path) // /member/15
        {
            if (path == "/favicon.ico") // 이새끼 뭐임 ;;
                return null;

            Node idNode = GetLastExistNodeInternal(path);

            if (idNode != null)
            {
                idNode = idNode.Children.Find(p => p.Path.Contains("{"));
                if (idNode == null)
                    return null;

                idNode.dictionary = new Dictionary<string, string>();

                idNode.dictionary.Add(idNode.Path, GetDividedPathInternal(path)[idNode.Rank]);

                return idNode;
            }
            else
                return null;
        }

        /*public Node GetDictionaryPathNode(string path, List<Node> list) 
        {
            if (path == "/favicon.ico") // 이새끼 뭐임 ;;
                return null;

            Node idNode = GetLastExistNodeInternal(path, list);

            if (idNode != null)
            {
                idNode = idNode.Children.Find(p => p.Path.Contains("{"));
                idNode.dictionary = new Dictionary<string, string>();

                idNode.dictionary.Add(idNode.Path, GetDividedPathInternal(path)[idNode.Rank]);

                return idNode;
            }
            else
                return null;
        }*/

        public Node GetPathNodeContainId(string url)
        {
            bool isExistIdNode = true;
            string thePath = url;
            Dictionary<string, string> Dic = new Dictionary<string, string>();

            while (isExistIdNode)
            {
                Node node = new Node();
                isExistIdNode = false;

                if (this.GetDictionaryPathNode(thePath) != null)
                {
                    isExistIdNode = true;
                    node = this.GetDictionaryPathNode(thePath);
                    string key = node.dictionary.Single().Key;
                    string value = node.dictionary.Single().Value;
                    Dic.Add(key, value);

                    if (node.Rank == url.Split('/').Length - 2) // node 가 마지막 path 일 시
                    {
                        node.dictionary = Dic;
                        return node;
                    }

                    //int index = url.IndexOf(value);
                    //thePath = url.Remove(index, 1).Insert(index, node.Path);
                    thePath = url.Replace(value, node.Path);
                }

                if (this.isExistPathNode(thePath))
                {
                    node = this.GetPathNode(thePath);
                    node.dictionary = Dic;
                    return node;
                }
            }

            return null;

        }

        public bool IsExistPathRoot(string path)
        {
            if (path == null)
                return false;

            string rootPath = GetDividedPathInternal(path)[0];
            foreach (Node root in Roots)
            {
                if (root.Path == rootPath)
                    return true;
            }

            return false;
        }

        private string[] GetDividedPathInternal(string path)
        {
            string[] result = path.Split('/');
            result = result.Skip(1).ToArray();

            return result;
        }

    }
}
