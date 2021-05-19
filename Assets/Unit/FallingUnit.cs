using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace.Unit
{
    public class FallingUnit : AbstractUnit
    {
        [SerializeField] private float speed;
        [SerializeField] private float minSize;
        [SerializeField] private float maxSize;

        [Range(-270f, -210f), SerializeField] private float minRotationRange;
        [Range(-150f, -90f), SerializeField] private float maxRotationRange;

        public override void Init()
        {
            base.Init();
            generateSize();
            randomRotation();
        }

        public override void Move()
        {
            transform.Translate(transform.up * speed * Time.deltaTime, Space.World);
        }

        private void generateSize()
        {
            var size = Random.Range(minSize, maxSize);
            transform.localScale = Vector3.one * size;
        }

        private void randomRotation()
        {
            var angle = Random.Range(minRotationRange, maxRotationRange);
            transform.localRotation = Quaternion.Euler(Vector3.forward * angle);
        }
    }
}