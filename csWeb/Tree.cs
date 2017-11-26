using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csWeb
{
    class Tree
    {
        private Leaf mLeaf;
        //private Dictionary<string, string> mkeyValue;

        public Leaf Leaf
        {
            get { return mLeaf; }
            set { mLeaf = value; }
        }

        public Tree()
        {
            Leaf.PreLeaf = null;
            Leaf.Path = "";
            Leaf.PostLeaves = new List<Leaf> { };
        }


        // 아래 추가, 검색은 다른 클래스로? 잘라야하지아늘까
        // 가독성이 하늘나라로 떠나고 말았다

        public void AddLeaf(string path)
        {
            string[] addingPath = path.Split('/'); // { , member }
            
            if (isThereLeaf(path))
                throw new Exception("Already Exist!");
            else if (addingPath[addingPath.Length - 2] != GetLastLeafOfBranch(path).PreLeaf.Path)
                throw new Exception("There is no PreLeaf!");
            else
            {
                Leaf valueLeaf = new Leaf(GetLastLeafOfBranch(path).PreLeaf, addingPath[addingPath.Length - 1]);
                GetLastLeafOfBranch(path).PreLeaf.PostLeaves.Add(valueLeaf);
            }
            
        }
        
        public bool isThereLeaf(string path)
        {
            string[] pathArgs = path.Split('/');
            Leaf leaf = this.Leaf;
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
        
        public Leaf GetLastLeafOfBranch(string path)
        {
            string[] pathArgs = path.Split('/');
            Leaf leaf = this.Leaf;
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

        public Leaf GetPostLeaves(string path, Leaf leaf)
        {
            if (leaf.PostLeaves.Capacity == 0)
                return leaf;

            Leaf retLeaf = new Leaf(); // 변수명 어려웡
            retLeaf = leaf.PostLeaves.FirstOrDefault(p => p.Path == path);

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
