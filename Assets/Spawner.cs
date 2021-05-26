using System;
using System.Collections.Generic;
using DefaultNamespace.GameStates;
using DefaultNamespace.Unit;
using Unity.Mathematics;
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

    public class Spawner : MonoBehaviour, IGameStart, IGameWin, IGameLoose, IGamePause, IGameResume
    {
        [SerializeField] private List<UnitInfo> infos;
        [SerializeField] private Priority priority;
        [SerializeField] private DeathZone zone;

        private List<AbstractUnit> units;

        private bool gameStarted;
        private bool isWon;
        private bool isLoosed;
        private bool isPaused;
        private bool Started;

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

            checkBounds();
        }

        private void Start()
        {
            Started = true;
        }

        private void init()
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

            if (!Started)
            {
                return;
            }

            for (int j = 0; j < info.Count; j++)
            {
                // var unit = Instantiate(info.UnitPrefab, Vector3.up * (2f + Random.value * j * 2),
                //     quaternion.identity);

                var unit = Utils.InstantiateDynamicObject(info.UnitPrefab, Vector3.up * (2f + Random.value * j * 2),
                    Quaternion.identity);
                unit.SetMoveState(true);
                unit.Init();
                units.Add(unit);
            }

            Started = false;
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
                if (Mathf.Abs(unitTransform.position.x) > zone.rightBoundX ||
                    Mathf.Abs(unitTransform.position.y) > zone.sizeY / 2)
                {
                    destroyItem(unit);
                }
            });
            units?.RemoveAll(unit => unit == null);
        }

        private void destroyItem(AbstractUnit unit)
        {
            Destroy(unit.gameObject);
        }

        public void StartGame()
        {
            gameStarted = true;
            updateStatus();
        }

        public void WinGame()
        {
            isWon = true;
        }

        private void updateStatus()
        {
            init();
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
    }
}