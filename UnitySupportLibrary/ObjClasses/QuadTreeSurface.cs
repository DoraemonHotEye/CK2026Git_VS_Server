using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySupportLibrary
{
    public class QuadTreeSurface
    {
        private QuadTreeNode _rootNode;

        public QuadTreeSurface(QuadTreeNode rootNode)
        {
            _rootNode = rootNode;
        }

        public QuadTreeNode rootNode { get { return _rootNode; } }
    }
}
