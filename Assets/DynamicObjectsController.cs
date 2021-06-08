using System;
using DefaultNamespace.Unit;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

namespace DefaultNamespace
{
    public abstract class SingleMonoBehaviour<T> : MonoBehaviour where T : Component
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance != null) return instance;
                instance = createInstance();
                return instance;
            }
            private set
            {
                if (instance != null)
                {
                    DestroyImmediate(value.gameObject);
                    return;
                }

                instance = value;
            }
        }

        protected virtual void Awake()
        {
            Instance = this as T;
        }

        private static T createInstance()
        {
            var go = new GameObject(typeof(T).Name);
            var component = go.AddComponent<T>();

            return component;
        }
    }

    public class DynamicObjectsController : SingleMonoBehaviour<DynamicObjectsController>
    {
        [NonSerialized] public UnityEvent<IDynamicObject> OnNotify = new UnityEvent<IDynamicObject>();

        public void Notify<T>(T obj) where T : IDynamicObject
        {
            OnNotify?.Invoke(obj);
        }
    }

}