using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitySupportLibrary.Game.Managers;
using UnitySupportLibrary.Static;

namespace UnitySupportLibrary.Game.Components.NativeComponent.Ai
{
    public abstract class AiNavParent : MonoNetworkComponent
    {
        public abstract void OnEnableAi();
        public abstract void OnDestroyAi();
        public abstract void OnUpdateAi(float DeltaTime);

        public override void OnDestroy()
        {
            AiManager.Instance.RemoveObject(this);
            OnDestroyAi();
        }
        public override void OnEnable()
        {
            OnEnableAi();
        }

        public override void OnUpdate(float DeltaTime)
        {
            OnUpdateAi(DeltaTime);
        }
    }
}
