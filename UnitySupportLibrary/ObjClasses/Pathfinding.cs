using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnitySupportLibrary;
using UnitySupportLibrary.Game.Components.NativeComponent.Ai;
using UnitySupportLibrary.Game.Managers;
using UnitySupportLibrary.Game.Objects;
using UnitySupportLibrary.Static;

public class QuadTreeNodeWrapper
{
    public enum FLAG_UNIT_COLLISION
    {
        COLLISION_NONE = 0,
        COLLISION_CAN_MOVE = 1,
        COLLISION_NOPATH_ERROR = 2,
    }

    public QuadTreeNode OriginalNode { get; private set; }
    public float GCost { get; set; }
    public float HCost { get; set; }
    public float FCost => GCost + HCost;
    public QuadTreeNodeWrapper Parent { get; set; }

    public QuadTreeNodeWrapper(QuadTreeNode originalNode)
    {
        OriginalNode = originalNode;
        GCost = Mathf.Infinity;
        HCost = Mathf.Infinity;
        Parent = null;
    }

    public bool IsWalkable => OriginalNode.GetWalkable;
    public Vector3 GetPosition() => OriginalNode._position;
    public void SetParent(QuadTreeNodeWrapper parent) => Parent = parent;

    public (FLAG_UNIT_COLLISION, List<Vector2>) isCheckUnitAndWalklable(QuadTreeNodeWrapper nextNode, Pathfinding pathfindObj)
    {
        List<Vector2> CollisionObjs = new List<Vector2>();
        const float CollisionRadius = 2.0f;

        foreach (var obj in GameManager.Instance.GetObjectList)
        {
            if (obj == pathfindObj.owner.gameObject)
                continue;

            bool isCollision = Mathf.IsCircleCollidingWithRect(new Vector2(obj.transform.position.x, obj.transform.position.z), CollisionRadius,
                new Vector2(GetPosition().x, GetPosition().z), OriginalNode._size);

            if (isCollision == true)
            {
                CollisionObjs.Add(new Vector2(obj.transform.position.x, obj.transform.position.z));
            }
        }

        if (CollisionObjs.Count > 0)
        {
            bool[,] isCanWalkable = Mathf.GenerateBlockedGridOneRadius(CollisionObjs, CollisionRadius, new Vector2(GetPosition().x, GetPosition().z), OriginalNode._size, 15, 15); ;

            (int, int) GetClosetPos = GetClosestGridIndexFromParent(15, 15);
            (int, int) GetEndPos = GetClosestGridIndexFromTarget(nextNode, 15, 15);

            List<Vector2> paths = Mathf.GetConnectedPath(isCanWalkable, GetClosetPos.Item1, GetClosetPos.Item2, GetEndPos.Item1, GetEndPos.Item2, new Vector2(GetPosition().x, GetPosition().z), OriginalNode._size);

            if (paths == null)
                return (FLAG_UNIT_COLLISION.COLLISION_NOPATH_ERROR, null);

            return (FLAG_UNIT_COLLISION.COLLISION_CAN_MOVE, paths);
        }

        return (FLAG_UNIT_COLLISION.COLLISION_NONE, null);
    }
    private (int, int) GetClosestGridIndexFromTarget(QuadTreeNodeWrapper targetNode, int gridX, int gridY)
    {
        Vector3 targetPos = targetNode.GetPosition();
        Vector2 rectPos = new Vector2(GetPosition().x, GetPosition().z);
        Vector2 rectSize = OriginalNode._size;

        float cellWidth = rectSize.x / gridX;
        float cellHeight = rectSize.y / gridY;

        float localX = targetPos.x - rectPos.x;
        float localY = targetPos.z - rectPos.y;

        int gx = (int)Mathf.Clamp((int)(localX / cellWidth), 0, gridX - 1);
        int gy = (int)Mathf.Clamp((int)(localY / cellHeight), 0, gridY - 1);

        return (gx, gy);
    }

    private (int startX, int startY) GetClosestGridIndexFromParent(int gridX, int gridY)
    {
        if (Parent == null) return (gridX / 2, gridY / 2); // 기본값: 중앙

        Vector3 parentPos = Parent.GetPosition();
        Vector2 rectPos = new Vector2(GetPosition().x, GetPosition().z);
        Vector2 rectSize = OriginalNode._size;

        float cellWidth = rectSize.x / gridX;
        float cellHeight = rectSize.y / gridY;

        float localX = parentPos.x - rectPos.x;
        float localY = parentPos.z - rectPos.y;

        int gx = (int)Mathf.Clamp((int)(localX / cellWidth), 0, gridX - 1);
        int gy = (int)Mathf.Clamp((int)(localY / cellHeight), 0, gridY - 1);

        return (gx, gy);
    }
}

public class Pathfinding
{
    public QuadNavAgent owner;

    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private QuadTreeNode _rootNode;
    private List<QuadTreeNodeWrapper> _openList = new List<QuadTreeNodeWrapper>();
    private HashSet<QuadTreeNodeWrapper> _closedSet = new HashSet<QuadTreeNodeWrapper>();
    private Dictionary<QuadTreeNode, QuadTreeNodeWrapper> _nodeWrapperMap = new Dictionary<QuadTreeNode, QuadTreeNodeWrapper>();

    //Inc Path Members
    private QuadTreeNodeWrapper _currentNode;
    private QuadTreeNodeWrapper _endWrapper;
    private bool _isEndNodeInclude;
    private bool _pathFound;

    public Pathfinding(QuadTreeNode rootNode)
    {
        _rootNode = rootNode;
    }

    public Pathfinding(Vector3 start, Vector3 end, QuadTreeNode rootNode)
    {
        _startPosition = start;
        _endPosition = end;
        _rootNode = rootNode;
    }

    public void SetDestPosInc(Vector3 start, Vector3 end, QuadTreeNode rootNode)
    {
        _startPosition = start;
        _endPosition = end;
        _rootNode = rootNode;

        _openList.Clear();
        _closedSet.Clear();
        _nodeWrapperMap.Clear();
    }

    public void InitPathfinding(Vector3 start, Vector3 end, QuadTreeNode rootNode)
    {
        SetDestPosInc(start, end, rootNode);

        var startNode = FindNodeAtPosition(_startPosition);
        var endNode = FindNodeAtPosition(_endPosition);

        if (startNode == null || endNode == null)
        {
            _pathFound = true;
            return;
        }

        _isEndNodeInclude = true;
        if (!endNode.GetWalkable)
        {
            endNode = FindNearestWalkableNode(endNode);
            _isEndNodeInclude = false;
            if (endNode == null)
            {
                _pathFound = true;
                return;
            }
        }

        var startWrapper = GetOrCreateWrapper(startNode);
        _endWrapper = GetOrCreateWrapper(endNode);

        startWrapper.GCost = 0;
        startWrapper.HCost = CalculateHCost(startWrapper);
        _openList.Add(startWrapper);

        _pathFound = false;
    }

    public List<Vector3> StepPathfinding()
    {
        if (_pathFound || _openList.Count == 0)
            return new List<Vector3>();

        _currentNode = GetLowestFCostNode();

        if (_currentNode == _endWrapper)
        {
            _pathFound = true;
            return RetracePath(_startPosition, _endPosition, GetOrCreateWrapper(FindNodeAtPosition(_startPosition)), _currentNode, _isEndNodeInclude);
        }

        _openList.Remove(_currentNode);
        _closedSet.Add(_currentNode);

        foreach (var neighbor in GetNeighbors(_currentNode))
        {
            if (!neighbor.IsWalkable || _closedSet.Contains(neighbor))
                continue;

            float tentativeGCost = _currentNode.GCost + CalculateGCost(_currentNode, neighbor);

            bool inOpenList = _openList.Contains(neighbor);
            if (!inOpenList || tentativeGCost < neighbor.GCost)
            {
                neighbor.GCost = tentativeGCost;
                neighbor.HCost = CalculateHCost(neighbor);
                neighbor.SetParent(_currentNode);

                if (!inOpenList)
                    _openList.Add(neighbor);
            }
        }

        return null; // 아직 경로를 찾지 못했음
    }


    public List<Vector3> FindPath()
    {
        _openList.Clear();
        _closedSet.Clear();
        _nodeWrapperMap.Clear();

        var startNode = FindNodeAtPosition(_startPosition);
        var endNode = FindNodeAtPosition(_endPosition);

        if (startNode == null || endNode == null)
        {
            return new List<Vector3>();
        }

        bool isEndNodeInclude = true;
        if (!endNode.GetWalkable)
        {
            endNode = FindNearestWalkableNode(endNode);
            isEndNodeInclude = false;
            if (endNode == null)
            {
                return new List<Vector3>();
            }
        }

        var startWrapper = GetOrCreateWrapper(startNode);
        var endWrapper = GetOrCreateWrapper(endNode);

        startWrapper.GCost = 0;
        startWrapper.HCost = CalculateHCost(startWrapper);
        _openList.Add(startWrapper);

        QuadTreeNodeWrapper bestGuess = null;
        float closestDistance = Mathf.Infinity;

        while (_openList.Count > 0)
        {
            var currentNode = GetLowestFCostNode();

            if (currentNode == endWrapper)
                return RetracePath(_startPosition, _endPosition, startWrapper, currentNode, isEndNodeInclude);

            float distToEnd = Vector3.Distance(currentNode.GetPosition(), endWrapper.GetPosition());
            if (distToEnd < closestDistance)
            {
                closestDistance = distToEnd;
                bestGuess = currentNode;
            }

            _openList.Remove(currentNode);
            _closedSet.Add(currentNode);

            foreach (var neighbor in GetNeighbors(currentNode))
            {
                if (!neighbor.IsWalkable || _closedSet.Contains(neighbor))
                    continue;

                float tentativeGCost = currentNode.GCost + CalculateGCost(currentNode, neighbor);

                bool inOpenList = _openList.Contains(neighbor);
                if (!inOpenList || tentativeGCost < neighbor.GCost)
                {
                    neighbor.GCost = tentativeGCost;
                    neighbor.HCost = CalculateHCost(neighbor);
                    neighbor.SetParent(currentNode);

                    if (!inOpenList)
                        _openList.Add(neighbor);
                }
            }
        }

        if (bestGuess != null)
        {
            return RetracePath(_startPosition, _endPosition, startWrapper, bestGuess, false);
        }

        return new List<Vector3>();
    }

    private QuadTreeNodeWrapper GetOrCreateWrapper(QuadTreeNode node)
    {
        if (_nodeWrapperMap.TryGetValue(node, out var wrapper))
            return wrapper;

        wrapper = new QuadTreeNodeWrapper(node);
        _nodeWrapperMap[node] = wrapper;
        return wrapper;
    }

    public QuadTreeNode FindNodeAtPosition(Vector3 position)
    {
        return SearchNode(_rootNode, position);
    }

    private QuadTreeNode SearchNode(QuadTreeNode node, Vector3 position)
    {
        Vector3 halfSize = new Vector3(node._size.x / 2f, 0f, node._size.y / 2f);
        if (!IsInside(position, node._position, halfSize))
            return null;

        if (node.childs.Count == 0)
            return node;

        foreach (var child in node.childs)
        {
            var found = SearchNode(child, position);
            if (found != null)
                return found;
        }

        return null;
    }

    private bool IsInside(Vector3 point, Vector3 center, Vector3 halfSize)
    {
        return Mathf.Abs(point.x - center.x) <= halfSize.x &&
               Mathf.Abs(point.z - center.z) <= halfSize.z;
    }

    private List<QuadTreeNodeWrapper> GetNeighbors(QuadTreeNodeWrapper node)
    {
        List<QuadTreeNodeWrapper> neighbors = new List<QuadTreeNodeWrapper>();
        foreach (var neighborNode in node.OriginalNode._neighbors)
        {
            var wrapper = GetOrCreateWrapper(neighborNode);
            if (_closedSet.Contains(wrapper)) continue;
            if (wrapper.IsWalkable)
                neighbors.Add(wrapper);
        }
        return neighbors;
    }


    private List<QuadTreeNode> GetAllLeafNodes(QuadTreeNode root)
    {
        List<QuadTreeNode> result = new List<QuadTreeNode>();
        Queue<QuadTreeNode> queue = new Queue<QuadTreeNode>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current.childs.Count == 0)
                result.Add(current);
            else
            {
                foreach (var child in current.childs)
                    queue.Enqueue(child);
            }
        }

        return result;
    }

    private QuadTreeNode FindNearestWalkableNode(QuadTreeNode blockedNode)
    {
        List<QuadTreeNode> leafNodes = GetAllLeafNodes(_rootNode);
        QuadTreeNode nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var node in leafNodes)
        {
            if (!node.GetWalkable || node == blockedNode)
                continue;

            float dist = Vector3.Distance(node._position, blockedNode._position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = node;
            }
        }

        return nearest;
    }

    private QuadTreeNodeWrapper GetLowestFCostNode()
    {
        QuadTreeNodeWrapper lowest = _openList[0];
        foreach (var node in _openList)
        {
            if (node.FCost < lowest.FCost || (node.FCost == lowest.FCost && node.HCost < lowest.HCost))
                lowest = node;
        }
        return lowest;
    }

    private float CalculateGCost(QuadTreeNodeWrapper from, QuadTreeNodeWrapper to)
    {
        return Vector3.Distance(from.GetPosition(), to.GetPosition());
    }

    private float CalculateHCost(QuadTreeNodeWrapper node)
    {
        return Vector3.Distance(node.GetPosition(), _endPosition);
    }

    private List<Vector3> RetracePath(Vector3 startpos, Vector3 endpos, QuadTreeNodeWrapper start, QuadTreeNodeWrapper end, bool includeLast)
    {
        List<Vector3> path = new List<Vector3>();
        QuadTreeNodeWrapper currentNode = end;

        if (currentNode.Parent == null)
        {
            path.Add(startpos);
            path.Add(endpos);

            return path;
        }

        Vector3 lastNodePos = currentNode.GetPosition();
        float distToEnd = Vector3.Distance(lastNodePos, endpos);

        // 기준 거리: 노드 크기의 절반 정도가 적당
        float threshold = currentNode.OriginalNode._size.magnitude * 0.5f;

        if (distToEnd > threshold)
        {
            // 충분히 멀면 endpos를 포함
            if (includeLast == true)
                path.Add(endpos);

            path.Add(lastNodePos);
        }
        else
        {
            // 가까우면 endpos만 포함
            if (includeLast == true)
                path.Add(endpos);
        }

        currentNode = currentNode.Parent;

        while (currentNode != start)
        {
            var Result = currentNode.isCheckUnitAndWalklable(currentNode.Parent, this);

            if (Result.Item1 == QuadTreeNodeWrapper.FLAG_UNIT_COLLISION.COLLISION_NONE)
            {
                path.Add(currentNode.GetPosition());
                currentNode = currentNode.Parent;
            }
            else if (Result.Item1 == QuadTreeNodeWrapper.FLAG_UNIT_COLLISION.COLLISION_NOPATH_ERROR)
            {
                break;
            }
            else if (Result.Item1 == QuadTreeNodeWrapper.FLAG_UNIT_COLLISION.COLLISION_CAN_MOVE)
            {
                List<Vector2> subPath = Result.Item2; // 세부 경로가 있다고 가정

                if (subPath != null && subPath.Count > 0)
                {
                    foreach (var p in subPath)
                    {
                        Vector3 worldPos = new Vector3(p.x, startpos.y, p.y); // Y축은 기존 높이 유지
                        path.Add(worldPos);
                    }
                }

                currentNode = currentNode.Parent;
            }
        }

        path.Add(startpos);
        path.Reverse();
        return path;
    }
}
