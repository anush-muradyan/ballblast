using System;
using DefaultNamespace.GameStates;
using UnityEngine;

namespace DefaultNamespace.Unit
{
    public abstract class AbstractUnit : MonoBehaviour, IDynamicObject
    {
        public event Action OnHit;
        public int countUnits;
        
        public bool CanMove { get; private set; }
        
        public virtual T GetInterface<T>()
        {
            return GetComponent<T>();
        }

        public virtual void Init()
        {
        }

        public abstract void Move();

        public virtual void Update()
        {
            if (!CanMove)
            {
                return;
            }

            Move();
        }

        public void SetMoveState(bool state)
        {
            CanMove = state;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            var unit = other.GetComponent<Bullet>();
            if (unit == null)
            {
                return;
            }
           
            OnHit?.Invoke();
            Destroy(gameObject);
            Destroy(unit.gameObject);
            countUnits--;
        }
        
    }
}