using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitySupportLibrary.Static;

namespace GameSupportLibrary.MapDatas
{
    public class TilemapNode
    {
        public Vector2 size;
        public Vector2 position;
        public bool isWalk = true;

        public TilemapNode(Vector2 inSize, Vector2 inPosition)
        {
            size = inSize;
            position = inPosition;
        }
    }

    public class TilemapSurface
    {
        public int Width;
        public int Height; 

        public Vector2 MasterPosition;

        public int GetWidth => Width;
        public int GetHeight => Height;

        public List<List<TilemapNode>> Tilemap = new List<List<TilemapNode>>();

        public void Clear() => Tilemap.Clear();

        public TilemapNode GetNode(int x, int y) => Tilemap[x][y];

        public Vector2Int GetClosestNodeIndex(Vector2 targetPos)
        {
            float minDist = float.MaxValue;
            int closestX = -1;
            int closestY = -1;

            for (int i = 0; i < Tilemap.Count; ++i)
            {
                for (int j = 0; j < Tilemap[i].Count; ++j)
                {
                    float dist = Vector2.Distance(targetPos, Tilemap[i][j].position);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        closestX = i;
                        closestY = j;
                    }
                }
            }

            return new Vector2Int(closestX, closestY);
        }

    }
}
