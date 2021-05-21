using System;
using System.Collections.Generic;
using DefaultNamespace.Unit;
using UnityEngine;

namespace DefaultNamespace
{

    public class GameStatements:MonoBehaviour
    {
        [SerializeField] private Bullet b;
        public interface IBehaviour
        {
            
        }
        
        public static event Action Behaviour;
        public static List<IBehaviour> Behavioures = new List<IBehaviour>();

        public void StartGame()
        {
            if (Input.GetKey("Q"))
            {
                Behaviour += Starting;
            }
        }

        private static void Starting()
        {
            Debug.Log("Starting!");
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var unit = other.GetComponent<AbstractUnit>();
            if (unit == null)
            {
                return;
            }  
            StartGame();
            Behaviour?.Invoke();
           
            
        }
        
    }
}
    
