using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public interface IInterface
    {
        
    }
    
    
    public class GameStatements
    {
        public static event Action Behaviour;
        public List<Bullet> behavioures = new List<Bullet>();
        public void StartGame()
        {
            if (Input.GetKey("Q"))
            {
                //Behaviour += Start();
            }
            Behaviour?.Invoke();
        }

        private void Start()
        {
           Debug.Log("Starting!");
        }
    }
}