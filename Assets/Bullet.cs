using System;
using System.Collections;
using DefaultNamespace.Unit;
using UnityEngine;

namespace DefaultNamespace
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed;

        private Vector2 dir;
        //public event Action OnHit;

        public void Shoot(Vector2 dir)
        {
            this.dir = dir;
        }

        private void Update()
        {
            transform.Translate(dir * speed * Time.deltaTime);
        }

       /* private void OnTriggerEnter2D(Collider2D other)
        {
            var unit = other.GetComponent<AbstractUnit>();
            if (unit == null)
            {
                return;
            }
           
            OnHit?.Invoke();
            Destroy(unit.gameObject);
        }
        */
    }
}