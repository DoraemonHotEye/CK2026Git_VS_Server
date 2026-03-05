using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitySupportLibrary.Static;

namespace UnitySupportLibrary
{
    public class QuadTreeNode
    {
        public int NodeNumber;
        public QuadTreeNode parent;
        public List<QuadTreeNode> childs;
        public Vector2 _size;
        public Vector3 _position;
        public bool _isWalkable = true;
        public List<QuadTreeNode> _neighbors;
        public bool GetWalkable {  get { return _isWalkable; } }
    }
}
