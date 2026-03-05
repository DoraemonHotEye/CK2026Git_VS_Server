using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySupportLibrary.Game.Components
{
    public abstract class MonoNetworkComponent
    {
        public abstract void OnUpdate(float DeltaTime);
        public abstract void OnDestroy();
        public abstract void OnEnable();
    }
}
