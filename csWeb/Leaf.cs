using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csWeb
{
    class Leaf
    {
        private Leaf mPreLeaf;
        private string mPath;
        private List<Leaf> mPostLeaves;
        //private Dictionary<string, string> mKeyValue { get; set; }

        public Leaf PreLeaf
        {
            get { return mPreLeaf; }
            set { mPreLeaf = value; }
        }
        public string Path
        {
            get { return mPath; }
            set { mPath = value; }
        }
        public List<Leaf> PostLeaves
        {
            get { return mPostLeaves; }
            set { mPostLeaves = value; }
        }

        public Leaf() { }

        public Leaf(Leaf preLeaf, string path)
        {
            PreLeaf = preLeaf;
            Path = path;
        }

    }
}
