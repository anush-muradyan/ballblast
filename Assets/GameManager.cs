using System;
using System.Collections.Generic;
using DefaultNamespace.GameStates;
using UnityEngine;

namespace DefaultNamespace
{
    public enum GameState
    {
        None,
        Started,
        Pause,
        Resume,
        Restart,
        Win,
        Loose
    }

    public class GameManager : MonoBehaviour
    {
        private GameState gameState = GameState.None;

        private List<IGameStart> gameStarts;

        private void Start()
        {
            Prepare();
        }

        public void Prepare()
        {
            gameStarts = Utils.GetInterfaces<IGameStart>();
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                startGame();
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                pauseGame();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                resumeGame();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                restartGame();
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                gameWin();
            }

            if (Input.GetKeyDown(KeyCode.Y))
            {
                gameLoose();
            }
        }

        private void startGame()
        {
            gameState = GameState.Started;
            gameStarts?.ForEach(start => start.StartGame());
        }

        private void pauseGame()
        {
            gameState = GameState.Pause;
        }

        private void resumeGame()
        {
            gameState = GameState.Resume;
        }

        private void restartGame()
        {
            gameState = GameState.Restart;
        }

        private void gameWin()
        {
            gameState = GameState.Win;
        }

        private void gameLoose()
        {
            gameState = GameState.Loose;
        }
    }
}