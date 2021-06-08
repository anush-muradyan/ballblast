using System;
using DefaultNamespace.GameStates;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace.Unit
{
    public class FallingUnit : AbstractUnit, IGamePause, IGameResume
    {
        [SerializeField] private float speed;
        [SerializeField] private float minSize;
        [SerializeField] private float maxSize;
        
        [Range(-270f, -210f), SerializeField] private float minRotationRange;
        [Range(-150f, -90f), SerializeField] private float maxRotationRange;
        //[SerializeField] private Transform board;
        private bool isPaused ;
        private bool isResumed ;

        private float direction = 1f;
        
        public override void Init()
        {
            base.Init();
            generateSize();
            randomRotation();
        }

        public override void Move()
        {
            if (!isPaused)
            {
                transform.Translate(transform.up * speed * direction *Time.deltaTime, Space.World);
            }
            if (isPaused)
            {
                if (isResumed)
                {
                    isResumed = false;
                    transform.Translate(transform.up * speed * direction * Time.deltaTime, Space.World);
                }
            }
           
        }

        public void PauseGame()
        {
            Debug.LogError("PauseGame");
            isPaused = true;
        }

        public void ResumeGame()
        {
            Debug.LogError("ResumeGame");
            isResumed = true;
            isPaused = false;
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            direction *= -1;
        }
    }
}