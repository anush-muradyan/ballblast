using System;
using System.Collections.Generic;
using DefaultNamespace.GameStates;
using DefaultNamespace.Unit;
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
        [SerializeField] private GameObject winPanel;
        private GameState gameState = GameState.None;

        private List<IGameStart> gameStarts;
        private List<IGameWin> winGame;
        private List<IGameLoose> looseGame;
        private List<IGamePause> pause;
        private List<IGameResume> resume;
        private List<IGameRestart> restart;
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
            restart = Utils.GetInterfaces<IGameRestart>();
        }

        private void handleDynamicObjects(IDynamicObject dynamicObject)
        {
            addInterface(dynamicObject, gameStarts);
            addInterface(dynamicObject, winGame);
            addInterface(dynamicObject, looseGame);
            addInterface(dynamicObject, pause);
            addInterface(dynamicObject, resume);
            
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
                restartGame();//TODO: implement ME!
              
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                gameWin();//TODO: Need to be fixed
            }

            if (Input.GetKeyDown(KeyCode.Y))
            {
                gameLoose();
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                Debug.Log("enemy created");
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
            pause?.ForEach(pause => pause.PauseGame());
        }

        private void resumeGame()
        {
            gameState = GameState.Resume;
            resume?.ForEach(resume => resume.ResumeGame());
        }

        private void restartGame()
        {
            winPanel.SetActive(false);
            gameState = GameState.Restart;
            restart?.ForEach(restart => restart.RestartGame());
            
        }

        private void gameWin()
        {
            gameState = GameState.Win;
            winGame?.ForEach(win => win.WinGame());
            winPanel.SetActive(true);
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
        
        private void creatEnemy()
        {
            Debug.Log("Enemy Created");
            /*var unit = Utils.InstantiateDynamicObject(UnitPrefab, Vector3.up * (2f + System.Random.value  * 2),
                Quaternion.identity);
            unit.SetMoveState(true);
            unit.Init();*/
        }
    }
}