using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public interface IBehaviour
    {
        
    }
    public class GameStatements
    {
        public static event Action Behaviour;
        public List<IBehaviour> Behavioures = new List<IBehaviour>();

        public void StartGame()
        {
            if (Input.GetKey("Q"))
            {
                Behaviour += Start;
            }

          
        }

        private void Start()
        {
            Debug.Log("Starting!");
        }
    }
}
    
