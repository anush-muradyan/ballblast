using System;
using DefaultNamespace.UI.View;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DefaultNamespace.Unit
{
    public abstract class AbstractUnit : MonoBehaviour, IDynamicObject
    {
        public event Action OnHit;
        public Text Score;
        private int score;
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

            Spawner.infosCount--;
            Debug.Log(Spawner.infosCount);
            score++;
            Score.text = $"{score}";
        }

    }
}