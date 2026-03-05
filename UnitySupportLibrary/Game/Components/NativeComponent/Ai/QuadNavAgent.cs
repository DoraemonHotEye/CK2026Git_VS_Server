using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnitySupportLibrary.Debug;
using UnitySupportLibrary.Game.Managers;
using UnitySupportLibrary.Game.Objects;
using UnitySupportLibrary.Static;

namespace UnitySupportLibrary.Game.Components.NativeComponent.Ai
{
    public class QuadNavAgent : AiNavParent
    {
        Pathfinding pathfinding;
        public GameObject gameObject;

        public QuadNavAgent()
        {
            if (QuadTreeSurfaceManager.Instance != null)
            {
                if (QuadTreeSurfaceManager.Instance.surface != null)
                {
                    if (QuadTreeSurfaceManager.Instance.surface.rootNode != null)
                    {
                        pathfinding = new Pathfinding(QuadTreeSurfaceManager.Instance.surface.rootNode);
                        pathfinding.owner = this;
                    }
                }
            }
        }

        List<Vector3> coners = null;
        public List<Vector3> Coners { get { return coners; } }

        ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();

        public override void OnEnableAi()
        {
            pathfinding.InitPathfinding(new Vector3(0.519f, 0.0f, -0.406f), new Vector3(6.18f, 0.00f, 0.45f), QuadTreeSurfaceManager.Instance.surface.rootNode);
            coners = pathfinding.FindPath();

            Console.WriteLine($"{gameObject}");
            foreach (var path in coners)
            {
                Console.WriteLine($"x: {path.x}, y: {path.y}, z: {path.z}");
            }
        }

        public override void OnDestroyAi()
        {

        }

        public override void OnUpdateAi(float DeltaTime)
        {

        }
    }
}
