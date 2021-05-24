using System;
using System.Collections.Generic;
using DefaultNamespace.GameStates;
using UnityEngine;
using Object = UnityEngine.Object;

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
        private List<IGameWin> winGame;
        private List<IGameLoose> looseGame;
        private List<IGamePause> pause;
        private List<IGameResume> resume;
        private List<IDynamicObject> enemy;
        
        private void Start()
        {
            Prepare();
        }

        private void OnEnable()
        {
            DynamicObjectsController.Instance.OnNotify.AddListener(handleDynamicObjects);
        }

        private void OnDisable()
        {
            DynamicObjectsController.Instance.OnNotify.RemoveListener(handleDynamicObjects);
        }

        public void Prepare()
        {
            gameStarts = Utils.GetInterfaces<IGameStart>();
            winGame = Utils.GetInterfaces<IGameWin>();
            looseGame = Utils.GetInterfaces<IGameLoose>();
            pause = Utils.GetInterfaces<IGamePause>();
            resume = Utils.GetInterfaces<IGameResume>();
            //enemy = Utils.GetInterfaces<IDynamicObject>();
        }

        private void handleDynamicObjects(IDynamicObject dynamicObject)
        {
            addInterface(dynamicObject, gameStarts);
            addInterface(dynamicObject, winGame);
            addInterface(dynamicObject, looseGame);
            addInterface(dynamicObject, pause);
            addInterface(dynamicObject, resume);
            //addInterface(dynamicObject, enemy);

        }

        private void addInterface<T>(IDynamicObject dynamicObject, List<T> interfaces)
        {
            var inter = dynamicObject.GetInterface<T>();
            if (inter == null)
            {
                return;
            }

            interfaces?.Add(inter);
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
            if (Input.GetKeyDown(KeyCode.U))
            {
                creatEnemy();
            }
        }

        private void creatEnemy()
        {
            throw new NotImplementedException();
        }

        private void startGame()
        {
            gameState = GameState.Started;
            gameStarts?.ForEach(start => start.StartGame());
        }

        private void pauseGame()
        {
            gameState = GameState.Pause;
            pause?.ForEach(pause => pause.PauseGame());
        }

        private void resumeGame()
        {
            gameState = GameState.Resume;
            resume?.ForEach(resume => resume.ResumeGame());
        }

        private void restartGame()
        {
            gameState = GameState.Restart;
        }

        private void gameWin()
        {
            gameState = GameState.Win;
            winGame?.ForEach(win => win.WinGame());
        }

        private void gameLoose()
        {
            gameState = GameState.Loose;
            resumeGame();
            if (Input.GetKeyDown(KeyCode.M))
            {
                resumeGame();
            }
        }
    }
}