using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace.Unit
{
    public class HorizontalMoveUnit : AbstractUnit
    {
        [SerializeField] private float minSize;
        [SerializeField] private float maxSize;
        [SerializeField] private float speed;

        private int dir = 1;

        public override void Move()
        {
            if (transform.position.x > 2f)
            {
                dir = -1;
            }
            else if(transform.position.x<-2f)
            {
                dir = 1;
            }
            
            
            transform.Translate(transform.right * speed * dir * Time.deltaTime, Space.World);
        }


        public override void Init()
        {
            base.Init();
            generateSize();
        }


        private void generateSize()
        {
            var size = Random.Range(minSize, maxSize);
            transform.localScale = Vector3.one * size;
        }
    }
}