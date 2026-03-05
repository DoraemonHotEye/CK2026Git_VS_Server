using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnitySupportLibrary.Debug;
using UnitySupportLibrary.Game.Components.NativeComponent.Ai;
using UnitySupportLibrary.Game.Managers;

namespace UnitySupportLibrary.Game.Objects.Childs
{
    public class Entity : GameObject
    {
        public QuadNavAgent navAgent = null;

        public override void OnDestroy()
        {
            navAgent.OnDestroy();
        }

        public override void OnEnable()
        {
            navAgent = (QuadNavAgent)AiManager.Instance.AddObject<QuadNavAgent>();
            navAgent.gameObject = this;
        }

        public override void OnUpdate(float DeltaTime)
        {

        }
    }
}
