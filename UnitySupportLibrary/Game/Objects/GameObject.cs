using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitySupportLibrary.Game.Components;
using UnitySupportLibrary.Game.Components.NativeComponent;

namespace UnitySupportLibrary.Game.Objects
{
    public abstract class GameObject
    {
        private Transform _transform = new Transform();

        public Transform transform
        {
            get { return _transform; }
        }

        public List<MonoNetworkComponent> components = new List<MonoNetworkComponent>();

        public void NativeUpdate(float DeltaTime)
        {
            transform.OnUpdate(DeltaTime);

            foreach (var component in components)
            {
                component.OnUpdate(DeltaTime);
            }

            OnUpdate(DeltaTime);
        }

        public void NativeOnEnable()
        {
            foreach (var component in components)
            {
                component.OnEnable();
            }

            OnEnable();
        }

        public void NativeOnDestroy()
        {
            foreach (var component in components)
            {
                component.OnDestroy();
            }

            OnDestroy();
        }

        public abstract void OnUpdate(float DeltaTime);
        public abstract void OnDestroy();
        public abstract void OnEnable();
    }
}
