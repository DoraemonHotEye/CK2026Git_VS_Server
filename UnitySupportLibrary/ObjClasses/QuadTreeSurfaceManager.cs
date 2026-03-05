using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitySupportLibrary.Debug;
using UnitySupportLibrary.Static;

namespace UnitySupportLibrary
{
    #region InputNode Class
    public class InPutJsonQuadNode
    {
        public int NodeNumber;
        public int ParentNumber;
        public List<int> ChildNumbers = new List<int>();
        public Vector2 _size;
        public Vector3 _position;
        public bool _isWalkable = true;
        public List<int> _neighborsNumbers = new List<int>();
    }
    #endregion

    public enum MSG_TYPE
    {
        None = 0,
        Succ = 1,
        Failed = 2,
    }

    public class QuadTreeSurfaceManager
    {
        public delegate void OnSurfaceHandler(object sender, MSG_TYPE compeletedmsg, string msg = "Succ");

        public OnSurfaceHandler OnloadedCallback;

        private static QuadTreeSurfaceManager Inst;
        public static QuadTreeSurfaceManager Instance => Inst ?? (Inst = new QuadTreeSurfaceManager());

        QuadTreeSurface _surface;
        public QuadTreeSurface surface { get { return _surface; } }

        public async Task GenerateJson(string JsonPath)
        {
            List<InPutJsonQuadNode> ParseNodeClasses = null;

            // 현재 실행 중인 프로그램의 디렉터리 기준으로 상대 경로를 절대 경로로 변환
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, JsonPath);

            if (File.Exists(fullPath))
            {
                string jsonContent = File.ReadAllText(fullPath);
                ParseNodeClasses = JsonConvert.DeserializeObject<List<InPutJsonQuadNode>>(jsonContent);

                UnitysSupportsDebug.PrintWriteLine($"Load Json Compeleted Count: {ParseNodeClasses.Count}");

                await ConvertAllInputNodesAsync(ParseNodeClasses);
            }
            else
            {
                if (OnloadedCallback != null)
                    OnloadedCallback.Invoke(this, MSG_TYPE.Failed, "쿼드트리 Surface Json 파일 경로 없음..");
            }
        }

        private async Task ConvertAllInputNodesAsync(List<InPutJsonQuadNode> nodes)
        {
            List<QuadTreeNode> realnodes = new List<QuadTreeNode>();

            await Task.Run(() =>
            {
                try
                {
                    UnitysSupportsDebug.PrintWriteLine("QuadTree 모든 노드 로드중...");
                    foreach (var node in nodes)
                    {
                        QuadTreeNode quadTreeNode = new QuadTreeNode
                        {
                            NodeNumber = node.NodeNumber,
                            _position = node._position,
                            _size = node._size,
                           _isWalkable = node._isWalkable,
                        };

                        lock (realnodes)
                        {
                            realnodes.Add(quadTreeNode);
                        }

                        if (quadTreeNode.NodeNumber == 0)
                            _surface = new QuadTreeSurface(quadTreeNode);
                    }
                }
                catch (Exception ex)
                {
                    if (OnloadedCallback != null)
                        OnloadedCallback.Invoke(this, MSG_TYPE.Failed, ex.Message);
                }
            });

            await Task.Run(() =>
            {
                try
                {
                    UnitysSupportsDebug.PrintWriteLine("QuadTree 모든 노드 변환중...");
                    foreach (var node in nodes)
                    {
                        QuadTreeNode realNode = realnodes.Find(x => x.NodeNumber == node.NodeNumber);

                        if (realNode != null)
                        {
                            QuadTreeNode parent = realnodes.Find(x => x.NodeNumber == node.ParentNumber);
                            if (parent != null)
                                realNode.parent = parent;

                            List<QuadTreeNode> childs = FindQuery(node.ChildNumbers, realnodes);
                            realNode.childs = childs;

                            List<QuadTreeNode> neigh = FindQuery(node._neighborsNumbers, realnodes);
                            realNode._neighbors = neigh;
                        }
                    }
                    UnitysSupportsDebug.PrintWriteLine("QuadTree 모든 변환 완료");

                    if (OnloadedCallback != null)
                        OnloadedCallback.Invoke(this, MSG_TYPE.Succ);
                }
                catch (Exception ex)
                {
                    if (OnloadedCallback != null)
                        OnloadedCallback.Invoke(this, MSG_TYPE.Failed, ex.Message);
                }
            });
        }

        private List<QuadTreeNode> FindQuery(List<int> nodeNumbers, List<QuadTreeNode> leafnodes)
        {
            List<QuadTreeNode> findedNodes = new List<QuadTreeNode>();

            foreach (var number in nodeNumbers)
            {
                QuadTreeNode node = leafnodes.Find(x => x.NodeNumber == number);
            
                if (node != null)
                    findedNodes.Add(node);
            }

            return findedNodes;
        }
    }
}
