using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.Unit
{
    public abstract class AbstractUnit : MonoBehaviour, IDynamicObject
    {
        public event Action OnHit;
        public Text Score;
        private int score;
        public bool CanMove { get; private set; }

        public event Func<AbstractUnit,Vector2> OnRequestPlayerDirection; 

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
            var dir = OnRequestPlayerDirection?.Invoke(this);
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

            Spawner.infosCount--;
            score++;
           // Score.text = $"{score}";
        }

    }
}