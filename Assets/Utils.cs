using System.Collections.Generic;
using System.Linq;
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
    }
}