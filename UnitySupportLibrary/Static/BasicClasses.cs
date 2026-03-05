using System;
using System.Collections.Generic;

namespace UnitySupportLibrary.Static
{
    public struct Vector2Int
    {
        public int x, y;

        public Vector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public struct Vector3
    {
        public float x, y, z;

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        // Magnitude (벡터의 길이)
        public float magnitude { get { return (float)Math.Sqrt(x * x + y * y + z * z); } }

        // Distance (두 벡터 사이의 거리)
        public static float Distance(Vector3 a, Vector3 b)
        {
            float dx = a.x - b.x;
            float dy = a.y - b.y;
            float dz = a.z - b.z;
            return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public Vector3 Normalize()
        {
            float mag = magnitude;
            if (mag > 0f)
                return new Vector3(x / mag, y / mag, z / mag);
            return new Vector3(0f, 0f, 0f);
        }

        // --- 연산자 오버로딩 ---
        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        public static Vector3 operator -(Vector3 a, Vector3 b) => new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        public static Vector3 operator *(Vector3 a, float d) => new Vector3(a.x * d, a.y * d, a.z * d);
        public static Vector3 operator *(float d, Vector3 a) => new Vector3(a.x * d, a.y * d, a.z * d);
        public static Vector3 operator /(Vector3 a, float d) => new Vector3(a.x / d, a.y / d, a.z / d);

        public static bool operator ==(Vector3 a, Vector3 b) => a.x == b.x && a.y == b.y && a.z == b.z;
        public static bool operator !=(Vector3 a, Vector3 b) => !(a == b);

        public override bool Equals(object obj) => obj is Vector3 v && this == v;
        public override int GetHashCode() => (x, y, z).GetHashCode();
    }

    public struct Vector2
    {
        public float x, y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        // Magnitude (벡터의 길이)
        public float magnitude { get { return (float)Math.Sqrt(x * x + y * y); } }

        // Distance (두 벡터 사이의 거리)
        public static float Distance(Vector2 a, Vector2 b)
        {
            float dx = a.x - b.x;
            float dy = a.y - b.y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public Vector2 Normalize()
        {
            float mag = magnitude;
            if (mag > 0f)
                return new Vector2(x / mag, y / mag);
            return new Vector2(0f, 0f);
        }

        // --- 연산자 오버로딩 ---
        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.x + b.x, a.y + b.y);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.x - b.x, a.y - b.y);
        public static Vector2 operator *(Vector2 a, float d) => new Vector2(a.x * d, a.y * d);
        public static Vector2 operator *(float d, Vector2 a) => new Vector2(a.x * d, a.y * d);
        public static Vector2 operator /(Vector2 a, float d) => new Vector2(a.x / d, a.y / d);

        public static bool operator ==(Vector2 a, Vector2 b) => a.x == b.x && a.y == b.y;
        public static bool operator !=(Vector2 a, Vector2 b) => !(a == b);

        public override bool Equals(object obj) => obj is Vector2 v && this == v;
        public override int GetHashCode() => (x, y).GetHashCode();
    }

    public static class Mathf
    {
        public static readonly float Infinity = float.PositiveInfinity;

        // Clamp: value가 min과 max 사이에 있도록 제한
        public static float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
        public static float Abs(float value)
        {
            return value < 0 ? -value : value;
        }

        public static bool IsCircleCollidingWithRect(
            Vector2 circlePosition, float radius,
            Vector2 rectPosition, Vector2 rectSize)
        {
            // 사각형 내부에서 원의 중심에 가장 가까운 점 계산
            float closestX = Math.Max(rectPosition.x, Math.Min(circlePosition.x, rectPosition.x + rectSize.x));
            float closestY = Math.Max(rectPosition.y, Math.Min(circlePosition.y, rectPosition.y + rectSize.y));

            // 가장 가까운 점과 원의 중심 사이 거리 계산
            float distanceX = circlePosition.x - closestX;
            float distanceY = circlePosition.y - closestY;

            // 거리의 제곱이 반지름의 제곱보다 작거나 같으면 충돌
            return (distanceX * distanceX + distanceY * distanceY) <= (radius * radius);
        }

        public static bool IsCircleTouchingOrCoveringRect(
            Vector2 circlePosition, float radius,
            Vector2 rectPosition, Vector2 rectSize)
        {
            // 사각형의 네 변의 중심 좌표 계산
            float rectLeft = rectPosition.x;
            float rectRight = rectPosition.x + rectSize.x;
            float rectTop = rectPosition.y;
            float rectBottom = rectPosition.y + rectSize.y;

            // 원이 사각형의 모든 변에 닿거나 덮고 있는지 확인
            bool touchesLeft = circlePosition.x - radius <= rectLeft;
            bool touchesRight = circlePosition.x + radius >= rectRight;
            bool touchesTop = circlePosition.y - radius <= rectTop;
            bool touchesBottom = circlePosition.y + radius >= rectBottom;

            return touchesLeft && touchesRight && touchesTop && touchesBottom;
        }

        public static bool[,] GenerateBlockedGrid(
            List<Vector2> circlePositions, List<float> radii,
            Vector2 rectPosition, Vector2 rectSize,
            int gridX, int gridY)
        {
            float cellWidth = rectSize.x / gridX;
            float cellHeight = rectSize.y / gridY;
            bool[,] blocked = new bool[gridX, gridY];

            for (int gx = 0; gx < gridX; gx++)
            {
                for (int gy = 0; gy < gridY; gy++)
                {
                    float cx = rectPosition.x + gx * cellWidth + cellWidth / 2f;
                    float cy = rectPosition.y + gy * cellHeight + cellHeight / 2f;
                    Vector2 cellCenter = new Vector2(cx, cy);

                    blocked[gx, gy] = false;

                    for (int i = 0; i < circlePositions.Count; i++)
                    {
                        Vector2 circlePos = circlePositions[i];
                        float r = radii[i];
                        float dx = cellCenter.x - circlePos.x;
                        float dy = cellCenter.y - circlePos.y;

                        if (dx * dx + dy * dy <= r * r)
                        {
                            blocked[gx, gy] = true;
                            break;
                        }
                    }
                }
            }

            return blocked;
        }

        public static bool[,] GenerateBlockedGridOneRadius(
            List<Vector2> circlePositions,
            float radius,
            Vector2 rectPosition, Vector2 rectSize,
            int gridX, int gridY)
        {
            float cellWidth = rectSize.x / gridX;
            float cellHeight = rectSize.y / gridY;
            bool[,] blocked = new bool[gridX, gridY];

            for (int gx = 0; gx < gridX; gx++)
            {
                for (int gy = 0; gy < gridY; gy++)
                {
                    float cx = rectPosition.x + gx * cellWidth + cellWidth / 2f;
                    float cy = rectPosition.y + gy * cellHeight + cellHeight / 2f;
                    Vector2 cellCenter = new Vector2(cx, cy);

                    blocked[gx, gy] = false;

                    foreach (var circlePos in circlePositions)
                    {
                        float dx = cellCenter.x - circlePos.x;
                        float dy = cellCenter.y - circlePos.y;

                        if (dx * dx + dy * dy <= radius * radius)
                        {
                            blocked[gx, gy] = true;
                            break;
                        }
                    }
                }
            }

            return blocked;
        }

        public static List<Vector2> GetConnectedPath(
            bool[,] blockedGrid,
            int startX, int startY,
            int endX, int endY,
            Vector2 rectPosition, Vector2 rectSize)
        {
            int gridX = blockedGrid.GetLength(0);
            int gridY = blockedGrid.GetLength(1);
            float cellWidth = rectSize.x / gridX;
            float cellHeight = rectSize.y / gridY;

            bool[,] visited = new bool[gridX, gridY];
            Vector2[,] parent = new Vector2[gridX, gridY];
            Queue<(int x, int y)> queue = new Queue<(int x, int y)>();

            int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };

            if (blockedGrid[startX, startY] || blockedGrid[endX, endY])
                return new List<Vector2>(); // 경로 없음

            queue.Enqueue((startX, startY));
            visited[startX, startY] = true;
            parent[startX, startY] = new Vector2(-1, -1); // 시작점

            while (queue.Count > 0)
            {
                var (x, y) = queue.Dequeue();

                if (x == endX && y == endY)
                    break;

                for (int i = 0; i < 8; i++) // 대각선 포함 8방향
                {
                    int nx = x + dx[i];
                    int ny = y + dy[i];

                    if (nx >= 0 && ny >= 0 && nx < gridX && ny < gridY &&
                        !blockedGrid[nx, ny] && !visited[nx, ny])
                    {
                        visited[nx, ny] = true;
                        parent[nx, ny] = new Vector2(x, y);
                        queue.Enqueue((nx, ny));
                    }
                }
            }

            // 경로 추적
            List<Vector2> path = new List<Vector2>();
            if (!visited[endX, endY])
                return null; // 도달 못함

            int cx = endX;
            int cy = endY;

            while (cx != -1 && cy != -1)
            {
                float px = rectPosition.x + cx * cellWidth + cellWidth / 2f;
                float py = rectPosition.y + cy * cellHeight + cellHeight / 2f;
                path.Add(new Vector2(px, py));

                Vector2 p = parent[cx, cy];
                cx = (int)p.x;
                cy = (int)p.y;
            }

            path.Reverse(); // 시작점부터 끝점 순서로
            return path;
        }
    }
}