using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public static class Utils
    {
        public static List<T> GetInterfaces<T>() where T : class
        {
            var objects = Object.FindObjectsOfType<MonoBehaviour>();
            var holder = new List<T>();
            for (int i = 0; i < objects.Length; i++)
            {
                var component = objects[i].GetComponent<T>();
                if (component == null)
                {
                    continue;
                }

                holder.Add(component);
            }

            return holder;
        }

        public static T InstantiateDynamicObject<T>(T original, Vector3 position, Quaternion rotation)
            where T : Object, IDynamicObject
        {
            var obj = Object.Instantiate(original, position, rotation);
            DynamicObjectsController.Instance.Notify(obj);
            return obj;
        }
        
    }
}