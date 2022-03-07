using System;
using System.Collections.Generic;
using UnityEngine;

namespace ECS
{
    public class Entity : ECSObject, IIndex
    {
        [SerializeField]
        internal List<ECSComponent> Components = new List<ECSComponent>();
        internal List<IUpdate> Updates = new List<IUpdate>();
        internal bool InActive;
        internal bool CanUpdate;
        public int Index { get; set; } = -1;

        public Entity()
        {
            System = ECSSystem.Instance;
        }

        internal void Execute()
        {
            for (int i = 0; i < Updates.Count; i++)
            {
                Updates[i].Update();
            }
        }

        public T AddComponent<T>() where T : ECSComponent, new()
        {
#if UNITY_EDITOR
            CheckDebugInfo();
#endif
            T component = System.Get<T>();
            component.Entity = this;
            component.System = System;
            Components.Add(component);
            component.Awake();
            if (component is IUpdate update)
            {
                Updates.Add(update);
            }
            CanUpdate = Updates.Count > 0;
            return component;
        }

        public ECSComponent AddComponent(ECSComponent component)
        {
#if UNITY_EDITOR
            CheckDebugInfo();
#endif
            component.Entity = this;
            component.System = System;
            Components.Add(component);
            component.Awake();
            if (component is IUpdate update)
            {
                Updates.Add(update);
            }
            CanUpdate = Updates.Count > 0;
            return component;
        }

        public T GetComponent<T>() where T : ECSComponent
        {
            for (int i = 0; i < Components.Count; i++)
            {
                if (Components[i] is T type)
                {
                    return type;
                }
            }

            return null;
        }

        public ECSComponent GetComponent(Type componentType)
        {
            for (int i = 0; i < Components.Count; i++)
            {
                if (Components[i].GetType() == componentType)
                {
                    return Components[i];
                }
            }

            return null;
        }

        public T[] GetComponents<T>() where T : ECSComponent
        {
            List<T> elements = new List<T>();
            for (int i = 0; i < Components.Count; i++)
            {
                if (Components[i] is T type)
                {
                    elements.Add(type);
                }
            }
            return elements.ToArray();
        }

        public List<T> GetComponentsList<T>() where T : ECSComponent
        {
            List<T> elements = new List<T>();
            for (int i = 0; i < Components.Count; i++)
            {
                if (Components[i] is T type)
                {
                    elements.Add(type);
                }
            }
            return elements;
        }

        public ECSComponent[] GetComponents(Type comType)
        {
            List<ECSComponent> elements = new List<ECSComponent>();
            for (int i = 0; i < Components.Count; i++)
            {
                {
                    if (Components[i].GetType() == comType)
                    {
                        elements.Add(Components[i]);
                    }
                }
            }
            return elements.ToArray();
        }


        public override string ToString()
        {
            string str = "[";
            for (int i = 0; i < Components.Count; i++)
            {
                str += Components[i].GetType().Name + ",";
            }
            str = str.TrimEnd(',');
            str += "]";
            return $"{GetType().Name} Components: {str}";
        }


        public void CheckDebugInfo(GameObject gameObject)
        {
#if UNITY_EDITOR
            if (gameObject == null)
            {
                return;
            }

            var debugBehaviour = gameObject.AddComponent<ECSDebugBehaviour>();
            debugBehaviour.m_ECSInfo.Clear();
            for (int i = 0; i < this.Components.Count; i++)
            {
                var component = this.Components[i];
                var cmptName = component.GetType().Name;
                debugBehaviour.SetDebugInfo(cmptName, "", "");
            }
#endif
        }
        public void CheckDebugInfo()
        {
#if UNITY_EDITOR
            var gameObjectCmpt = GetComponent<ECSGameObjectCmpt>();

            if (gameObjectCmpt == null)
            {
                return;
            }
            if (gameObjectCmpt.gameObject == null)
            {
                return;
            }

            var debugBehaviour = UnityUtil.AddMonoBehaviour<ECSDebugBehaviour>(gameObjectCmpt.gameObject);
            debugBehaviour.m_ECSInfo.Clear();
            for (int i = 0; i < this.Components.Count; i++)
            {
                var component = this.Components[i];
                var cmptName = component.GetType().Name;
                debugBehaviour.SetDebugInfo(cmptName, "", "");
            }
#endif
        }
    }
}
