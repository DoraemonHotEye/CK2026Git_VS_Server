using GameSupportLibrary.MapDatas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitySupportLibrary.Static;

public sealed class AStarGrid
{
    public readonly int Width;
    public readonly int Height;

    public Vector2 TileSize;

    // 외부에서 "이 칸이 이동 가능한가?" 제공
    private readonly Func<int, int, bool> _isWalkable;

    // 옵션
    private readonly bool _allowDiagonal;
    private readonly bool _preventCornerCutting;

    // 재사용용 노드 배열(할당 최소화)
    private readonly Node[] _nodes;

    public AStarGrid(
        int width,
        int height,
        Func<int, int, bool> isWalkable,
        bool allowDiagonal = true,
        bool preventCornerCutting = true)
    {
        if (width <= 0 || height <= 0) throw new ArgumentException("Invalid grid size.");
        Width = width;
        Height = height;
        _isWalkable = isWalkable ?? throw new ArgumentNullException(nameof(isWalkable));
        _allowDiagonal = allowDiagonal;
        _preventCornerCutting = preventCornerCutting;

        _nodes = new Node[Width * Height];
        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                _nodes[IndexOf(x, y)] = new Node(x, y);
    }


    public AStarGrid(
        TilemapSurface surface,
        bool allowDiagonal = true,
        bool preventCornerCutting = true)
    {
        if (surface.GetWidth <= 0 || surface.GetHeight <= 0)
            throw new ArgumentException("Invalid grid size.");

        Width = surface.GetWidth;
        Height = surface.GetHeight;

        _allowDiagonal = allowDiagonal;
        _preventCornerCutting = preventCornerCutting;

        _nodes = new Node[Width * Height];

        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                _nodes[IndexOf(x, y)] = new Node(x, y);

        // 여기서 isWalkable 정의
        _isWalkable = (x, y) =>
        {
            // 범위 체크
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return false;

            // TilemapSurface 내부 노드 가져오기
            TilemapNode node = surface.GetNode(x, y);

            // 예시: Collider와 겹치지 않으면 walkable
            // (실제 구현에서는 충돌 여부, 장애물 여부 등을 체크)
            return node.isWalk;
        };
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
        => FindPath(start.x, start.y, goal.x, goal.y);

    public List<Vector2Int> FindPath(int sx, int sy, int gx, int gy)
    {
        if (!InBounds(sx, sy) || !InBounds(gx, gy)) return null;
        if (!_isWalkable(sx, sy)) return null;
        if (sx == gx && sy == gy) return new List<Vector2Int>();

        ResetNodes();

        Node startNode = GetNode(sx, sy);
        Node goalNode = GetNode(gx, gy);

        var open = new MinHeap<Node>(Comparer<Node>.Create((a, b) =>
        {
            int f = a.F.CompareTo(b.F);
            if (f != 0) return f;
            int h = a.H.CompareTo(b.H);
            if (h != 0) return h;
            return b.G.CompareTo(a.G);
        }));

        startNode.G = 0;
        startNode.H = Heuristic(startNode, goalNode, _allowDiagonal);
        startNode.Parent = null;
        startNode.State = NodeState.Open;

        open.Push(startNode);

        // 목표에 가장 가까운 후보 노드 추적
        Node bestNode = startNode;

        while (open.Count > 0)
        {
            Node current = open.Pop();
            if (current.State == NodeState.Closed) continue;
            current.State = NodeState.Closed;

            // 목표 도달
            if (current == goalNode)
                return ReconstructPath(goalNode, startNode);

            // 목표에 더 가까운 노드 갱신
            if (current.H < bestNode.H)
                bestNode = current;

            foreach (var nb in GetNeighbors(current))
            {
                if (nb == null) continue;
                if (nb.State == NodeState.Closed) continue;
                if (!_isWalkable(nb.X, nb.Y)) continue;

                if (_allowDiagonal && _preventCornerCutting)
                {
                    int dx = nb.X - current.X;
                    int dy = nb.Y - current.Y;
                    if (dx != 0 && dy != 0)
                    {
                        if (!_isWalkable(current.X + dx, current.Y) || !_isWalkable(current.X, current.Y + dy))
                            continue;
                    }
                }

                int stepCost = MoveCost(current, nb);
                int tentativeG = current.G + stepCost;

                if (nb.State != NodeState.Open || tentativeG < nb.G)
                {
                    nb.Parent = current;
                    nb.G = tentativeG;
                    nb.H = Heuristic(nb, goalNode, _allowDiagonal);

                    nb.State = NodeState.Open;
                    open.Push(nb);
                }
            }
        }

        // 목표에 도달하지 못했을 경우, bestNode까지의 경로 반환
        return ReconstructPath(bestNode, startNode);
    }

    // 점진적 한 스텝 함수
    private bool FindPathStep(MinHeap<Node> open, ref Node bestNode, Node goalNode, bool allowDiagonal, bool preventCornerCutting)
    {
        if (open.Count == 0)
            return false;

        Node current = open.Pop();
        if (current.State == NodeState.Closed) return false;
        current.State = NodeState.Closed;

        // 목표 도달
        if (current == goalNode)
            return true;

        // 목표에 더 가까운 노드 갱신
        if (current.H < bestNode.H)
            bestNode = current;

        foreach (var nb in GetNeighbors(current))
        {
            if (nb == null) continue;
            if (nb.State == NodeState.Closed) continue;
            if (!_isWalkable(nb.X, nb.Y)) continue;

            if (allowDiagonal && preventCornerCutting)
            {
                int dx = nb.X - current.X;
                int dy = nb.Y - current.Y;
                if (dx != 0 && dy != 0)
                {
                    if (!_isWalkable(current.X + dx, current.Y) || !_isWalkable(current.X, current.Y + dy))
                        continue;
                }
            }

            int stepCost = MoveCost(current, nb);
            int tentativeG = current.G + stepCost;

            if (nb.State != NodeState.Open || tentativeG < nb.G)
            {
                nb.Parent = current;
                nb.G = tentativeG;
                nb.H = Heuristic(nb, goalNode, allowDiagonal);

                nb.State = NodeState.Open;
                open.Push(nb);
            }
        }

        return false;
    }

    // 최종 경로 반환
    private List<Vector2Int> GetPathResult(Node goalNode, Node startNode, Node bestNode)
    {
        if (goalNode.Parent != null)
            return ReconstructPath(goalNode, startNode);

        return ReconstructPath(bestNode, startNode);
    }

    // 점진적 A* 전체 실행 함수
    public List<Vector2Int> FindPathIncremental(int sx, int sy, int gx, int gy, bool allowDiagonal, bool preventCornerCutting)
    {
        if (!InBounds(sx, sy) || !InBounds(gx, gy)) return null;
        if (!_isWalkable(sx, sy)) return null;
        if (sx == gx && sy == gy) return new List<Vector2Int>();

        ResetNodes();

        Node startNode = GetNode(sx, sy);
        Node goalNode = GetNode(gx, gy);

        var open = new MinHeap<Node>(Comparer<Node>.Create((a, b) =>
        {
            int f = a.F.CompareTo(b.F);
            if (f != 0) return f;
            int h = a.H.CompareTo(b.H);
            if (h != 0) return h;
            return b.G.CompareTo(a.G);
        }));

        startNode.G = 0;
        startNode.H = Heuristic(startNode, goalNode, allowDiagonal);
        startNode.Parent = null;
        startNode.State = NodeState.Open;
        open.Push(startNode);

        Node bestNode = startNode;

        // 점진적 탐색 루프
        while (open.Count > 0)
        {
            bool reached = FindPathStep(open, ref bestNode, goalNode, allowDiagonal, preventCornerCutting);
            if (reached) break;
        }

        // 최종 경로 반환
        return GetPathResult(goalNode, startNode, bestNode);
    }

    // -----------------------------
    // 내부 구현
    // -----------------------------

    private enum NodeState : byte { None, Open, Closed }

    private sealed class Node
    {
        public readonly int X;
        public readonly int Y;

        public int G;  // start -> node
        public int H;  // node  -> goal
        public int F => G + H;

        public Node Parent;
        public NodeState State;

        public Node(int x, int y)
        {
            X = x;
            Y = y;
            G = int.MaxValue;
            H = 0;
            Parent = null;
            State = NodeState.None;
        }
    }

    private void ResetNodes()
    {
        for (int i = 0; i < _nodes.Length; i++)
        {
            var n = _nodes[i];
            n.G = int.MaxValue;
            n.H = 0;
            n.Parent = null;
            n.State = NodeState.None;
        }
    }

    private IEnumerable<Node> GetNeighbors(Node n)
    {
        // 4방향
        yield return TryGetNode(n.X + 1, n.Y);
        yield return TryGetNode(n.X - 1, n.Y);
        yield return TryGetNode(n.X, n.Y + 1);
        yield return TryGetNode(n.X, n.Y - 1);

        if (!_allowDiagonal) yield break;

        // 대각선
        yield return TryGetNode(n.X + 1, n.Y + 1);
        yield return TryGetNode(n.X + 1, n.Y - 1);
        yield return TryGetNode(n.X - 1, n.Y + 1);
        yield return TryGetNode(n.X - 1, n.Y - 1);
    }

    private Node TryGetNode(int x, int y)
        => InBounds(x, y) ? GetNode(x, y) : null;

    private Node GetNode(int x, int y) => _nodes[IndexOf(x, y)];

    private int IndexOf(int x, int y) => y * Width + x;

    private bool InBounds(int x, int y) => (uint)x < (uint)Width && (uint)y < (uint)Height;

    // 비용: 직선 10, 대각 14(근사)
    private static int MoveCost(Node a, Node b)
    {
        int dx = Math.Abs(a.X - b.X);
        int dy = Math.Abs(a.Y - b.Y);
        return (dx + dy == 1) ? 10 : 14;
    }

    // 휴리스틱
    // - 대각 허용 시: Octile distance (10,14 기반)
    // - 대각 불가 시: Manhattan (10 기반)
    private static int Heuristic(Node a, Node b, bool diagonal)
    {
        int dx = Math.Abs(a.X - b.X);
        int dy = Math.Abs(a.Y - b.Y);

        if (!diagonal)
            return 10 * (dx + dy);

        int min = Math.Min(dx, dy);
        int max = Math.Max(dx, dy);
        return 14 * min + 10 * (max - min);
    }

    private static List<Vector2Int> ReconstructPath(Node goal, Node start)
    {
        var path = new List<Vector2Int>();
        Node cur = goal;

        // start는 제외하고 goal까지
        while (cur != null && cur != start)
        {
            path.Add(new Vector2Int(cur.X, cur.Y));
            cur = cur.Parent;
        }

        path.Reverse();
        return path;
    }

    // -----------------------------
    // 간단 Min-Heap 우선순위 큐
    // -----------------------------
    private sealed class MinHeap<T>
    {
        private readonly List<T> _data = new List<T>();
        private readonly IComparer<T> _cmp;

        public int Count => _data.Count;

        public MinHeap(IComparer<T> comparer) => _cmp = comparer ?? Comparer<T>.Default;

        public void Push(T item)
        {
            _data.Add(item);
            SiftUp(_data.Count - 1);
        }

        public T Pop()
        {
            int last = _data.Count - 1;
            T root = _data[0];
            _data[0] = _data[last];
            _data.RemoveAt(last);
            if (_data.Count > 0) SiftDown(0);
            return root;
        }

        private void SiftUp(int i)
        {
            while (i > 0)
            {
                int p = (i - 1) >> 1;
                if (_cmp.Compare(_data[i], _data[p]) >= 0) break;
                (_data[i], _data[p]) = (_data[p], _data[i]);
                i = p;
            }
        }

        private void SiftDown(int i)
        {
            int count = _data.Count;
            while (true)
            {
                int l = i * 2 + 1;
                if (l >= count) break;

                int r = l + 1;
                int best = (r < count && _cmp.Compare(_data[r], _data[l]) < 0) ? r : l;

                if (_cmp.Compare(_data[best], _data[i]) >= 0) break;
                (_data[i], _data[best]) = (_data[best], _data[i]);
                i = best;
            }
        }
    }
}
