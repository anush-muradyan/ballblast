using System;
using System.Collections.Generic;
using DefaultNamespace.GameStates;
using DefaultNamespace.UI.View;
using DefaultNamespace.Unit;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    [Serializable]
    public class UnitInfo
    {
        [SerializeField] private AbstractUnit unitPrefab;
        [SerializeField] private Priority priority;
        [SerializeField] private int count;

        public Priority Priority => priority;
        public AbstractUnit UnitPrefab => unitPrefab;

        public int Count => count;

    }

    public class Spawner : MonoBehaviour, IGameStart, IGameEnd, IGamePause, IGameResume, IGameRestart
    {
        [SerializeField] private List<UnitInfo> infos;
        [SerializeField] private Priority priority;
        [SerializeField] private DeathZone zone;
        [SerializeField] private Player player;

        [SerializeField] private GameManager gameManager;

        public List<AbstractUnit> units;
        static public int infosCount = 0;
        public bool gameStarted;
        private bool isWon;
        private bool isLoosed;
        private bool isPaused;
        private Vector3 velocity;
        private bool endGame;

        private void Start()
        {
            velocity = Random.insideUnitCircle;
        }

        private void Update()
        {
            if (!gameStarted)
            {
                return;
            }
            if (isPaused)
            {
                return;
            }

            if (isWon)
            {
                units?.ForEach(unit =>
                {
                    if (unit == null)
                    {
                        return;
                    }

                    destroyItem(unit);
                });
            }

            GameEnd();
            checkBounds();
        }

        public void Init()
        {
            units = new List<AbstractUnit>();
            var values = Enum.GetValues(typeof(Priority));
            for (int i = 0; i < values.Length; i++)
            {
                var name = values.GetValue(i);
                var flag = (Priority) Enum.Parse(typeof(Priority), name.ToString());
                var hasFlag = priority.HasFlag(flag);
                if (!hasFlag)
                {
                    continue;
                }

                Spawn(flag);
            }
        }

        private void Spawn(Priority priority)
        {
           
            var info = infos.Find(unitInfo => unitInfo.Priority == priority);
            if (info == null)
            {
                return;
            }
            
            for (int j = 0; j < info.Count; j++)
            {

                var unit = Utils.InstantiateDynamicObject(info.UnitPrefab, Vector3.up * (1f + Random.value *  1.5f),
                    Quaternion.identity);
                unit.OnRequestPlayerDirection += requestDirectionPlayer;
                unit.SetMoveState(true);
                unit.Init();
                units.Add(unit);
            }

            infosCount = info.Count;
            Debug.Log(infosCount);
        }

        private Vector2 requestDirectionPlayer(AbstractUnit currentUnit) {
            var pos = currentUnit.transform.position;
            var playerPos = player.transform.position;
            var dir = playerPos - pos;
            return dir;
        }

        private void GameEnd()
        {
            var info = infos.Find(unitInfo => unitInfo.Priority == priority);
            if (info == null)
            {
                return;
            }
            if (infosCount <= 0)
            {
                endGame = true;
                gameManager.gameState = GameState.Win;
                
            }
        }

        private void checkBounds()
        {
            units?.ForEach(unit =>
            {
                if (unit == null)
                {
                    return;
                }

                var unitTransform = unit.transform;
                if (Mathf.Abs(unitTransform.position.x) > zone.rightBoundX)
                {
                    velocity.x *= -1f;
                    //destroyItem(unit);
                }

                if (Mathf.Abs(unitTransform.position.y) > zone.sizeY / 2)
                {
                    velocity.y *= -1;
                }

                transform.position += velocity*0.8f;
            });
            units?.RemoveAll(unit => unit == null);
        }

        private void destroyItem(AbstractUnit unit)
        {
            Destroy(unit.gameObject);
        }

        public void StartGame()
        {
            Init();
            gameStarted = true;
        }

        public void WinGame()
        {
            isWon = true;
        }

        public void LooseGame()
        {
            isLoosed = true;
        }
        

        public void PauseGame()
        {
            isPaused = true;
        }

        public void ResumeGame()
        {
            isPaused = false;
        }

        public void RestartGame()
        {
            gameStarted = false;
            isWon = false;
            isLoosed = false;
            isPaused = false;
            
            units.ForEach(unit =>
            {
                if (unit != null)
                {
                    destroyItem(unit);
                }
            });
            units?.RemoveAll(unit => unit == null);

        }

        public void EndGame(GameEnd gameEnd)
        {
            endGame = true;
        }
    }
}