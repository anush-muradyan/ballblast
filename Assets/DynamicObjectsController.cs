using System;
using UnityEngine;
using UnityEngine.Events;

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
        }

        protected virtual void Awake()
        {
            instance = this as T;
        }

        private static T createInstance()
        {
            var go = new GameObject(typeof(T).Name);
            return go.AddComponent<T>();
        }
    }

    public class DynamicObjectsController : SingleMonoBehaviour<DynamicObjectsController>
    {
        [NonSerialized]public UnityEvent<IDynamicObject> OnNotify = new UnityEvent<IDynamicObject>();
        public void Notify<T>(T obj) where T : IDynamicObject
        {
            OnNotify?.Invoke(obj);
        }
    }
}