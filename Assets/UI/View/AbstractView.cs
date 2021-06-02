using System;
using UnityEngine;

namespace DefaultNamespace.UI.View
{
    public abstract class AbstractView : MonoBehaviour
    {
        public abstract void Show();
        public abstract void Close();

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
        }
    }
}