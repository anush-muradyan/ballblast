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
        
        
        private bool isPaused;
        private bool isResumed;

        private int direction=1;
        private Vector3 size;
        private Vector3 leftSide;

        private void Awake()
        {
            //direction = transform.up;
        }

        public override void Init()
        {
            base.Init();
            generateSize();
            randomRotation();
            
        }

        public override void Move()
        {
            // if (!isPaused)
            // {
            //     transform.Translate(direction*speed * Time.deltaTime, Space.World);
            // }
            // if (isPaused)
            // {
            //     if (isResumed)
            //     {
            //         isResumed = false;
            //         transform.Translate(direction * speed * Time.deltaTime, Space.World);
            //     }
            // }


            //transform.Translate(transform.up * speed * Time.deltaTime, Space.World);
            //var hit = Physics2D.Raycast(transform.position, transform.up);
            //Debug.LogError(hit.collider.name);
            //Debug.DrawRay();
             transform.Translate(transform.up * speed * direction * Time.deltaTime, Space.World);
            // if (Mathf.Abs(transform.position.x)>=2 || Mathf.Abs(transform.position.y)>=5f)
            // {
            //     direction = -direction;
            // }

            
            var XRightYUp=transform.Find("XrightYUp");
            var XRightYDown=transform.Find("XRightYDown");
            var a = transform.position - XRightYDown.position;

            var disatance = (a.x - transform.position.x) * (a.x - transform.position.x) + (a.y - transform.position.y) * (a.y - transform.position.y);
            var hit=Physics2D.Raycast(a, a+new Vector3(0.1f,0f),Mathf.Sqrt(disatance));
            

            Debug.DrawRay(transform.position, a,Color.red);
           
            if (hit.collider)
            {
                
                Debug.Log("Hit");
                transform.Rotate(0f,0f,-3f);
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
            Debug.Log(Quaternion.Euler(leftSide));
        }
    }
}