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
            Node lastExistPathNode = GetLastExistNode(path);
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

        private Node GetLastExistNode(string path)
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

        public Node GetPathNode(string path)
        {
            if (GetLastExistNode(path) == null)
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
            else
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













        // 아래 추가, 검색은 다른 클래스로? 잘라야하지아늘까
        // 가독성이 하늘나라로 떠나고 말았다

        /*public void AddLeaf(string path)
        {
            string[] addingPath = path.Split('/'); // { , member }
            Node lastLeaf = GetLastLeafOfBranch(path);
            if (lastLeaf.Parent == null)


            if (isThereLeaf(path))
                throw new Exception("Already Exist!");
            else if (addingPath[addingPath.Length - 2] != GetLastLeafOfBranch(path).Parent.Path)
                throw new Exception("There is no PreLeaf!");
            else
            {
                Node valueLeaf = new Node(GetLastLeafOfBranch(path).Parent, addingPath[addingPath.Length - 1]);
                GetLastLeafOfBranch(path).Parent.Children.Add(valueLeaf);
            }
        }

        public void DeleteLeaf(string path) // 삭제가 필요한가?
        {
            string deletingPath = path.Split('/')[path.Split('/').Length - 1];
            if (isThereLeaf(path))
            {
                GetLastLeafOfBranch(path).Parent.Children.RemoveAt(
                    GetLastLeafOfBranch(path).Parent.Children.FindIndex(p => p.Path == deletingPath)
                );
            }
            else
                throw new Exception("There is no Leaf that have path!");

        }
        

        public bool isThereLeaf(string path)
        {
            string[] pathArgs = path.Split('/');
            Node leaf = this.Root;
            int TreeRankCount = 1;

            while (true) // 아ㅡ 무조건 무한반복 구데기죠?
            {
                if (leaf != GetPostLeaves(pathArgs[TreeRankCount], leaf))
                {
                    leaf = GetPostLeaves(pathArgs[TreeRankCount++], leaf);
                }
                else
                    return false;

                if (TreeRankCount == pathArgs.Length)
                    return true;
            }
        }
        

        public Node GetLastLeafOfBranch(string path)
        {
            string[] pathArgs = path.Split('/');
            Node leaf = this.Root;
            int TreeRankCount = 1;

            while (true) // 아ㅡ 무조건 무한반복 구데기죠?
            {
                if (leaf != GetPostLeaves(pathArgs[TreeRankCount], leaf))
                {
                    leaf = GetPostLeaves(pathArgs[TreeRankCount++], leaf);
                }
                else
                    return leaf;

                if (TreeRankCount == pathArgs.Length)
                    return leaf;
            }
        }

        public Node GetPostLeaves(string path, Node leaf)
        {
            if (leaf.Children.Capacity == 0)
                return leaf;

            Node retLeaf = new Node(); // 변수명 어려웡
            retLeaf = leaf.Children.FirstOrDefault(p => p.Path == path);

            if (retLeaf == null)
                return leaf;
            else
                return retLeaf;
        }

        /**
        foreach (Leaf Postleaf in leaf.PostLeaves)
        {
            if (path == Postleaf.Path)
                return Postleaf;
        }
        */



        /*
        public bool SearchLeaf(string path, Leaf leaf)
        {
            if (path == leaf.Path)
                return true;
            else
                return false;
        }
        */






    }
}
