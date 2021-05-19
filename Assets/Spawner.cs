using System;
using System.Collections.Generic;
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

    public class Spawner : MonoBehaviour
    {
        [SerializeField] private List<UnitInfo> infos;
        [SerializeField] private Priority priority;
        [SerializeField] private Board board;

        private List<AbstractUnit> units;


        private void Start()
        {
            init();
        }

        private void Update()
        {
            checkBounds();
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

        public void Spawn(Priority priority)
        {
            var info = infos.Find(unitInfo => unitInfo.Priority == priority);
            if (info == null)
            {
                return;
            }

            for (int j = 0; j < info.Count; j++)
            {
                var unit = Instantiate(info.UnitPrefab, Vector3.up * (2f + Random.value * j * 2),
                    quaternion.identity);
                unit.SetMoveState(true);
                unit.Init();
                units.Add(unit);
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
                if (Mathf.Abs(unitTransform.position.x) > 10 || Mathf.Abs(unitTransform.position.y) > 10f)
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
    }
}