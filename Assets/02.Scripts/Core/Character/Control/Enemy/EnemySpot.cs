using System.Collections.Generic;

using UnityEngine;

using ProjectZ.Manager;
using ProjectZ.Core.Characters;

namespace ProjectZ.Core
{
    public class EnemySpot : MonoBehaviour
    {
        [SerializeField] private ObjectPool _enemyPool;
        [SerializeField] private int _maxSpawnAmount;
        [Range(0f, 5f)]
        [SerializeField] private float _spawnRange;

        private List<EnemyControls> _enemyList = new();

        private void Awake()
        {
            for (int index = 0; index < _maxSpawnAmount; index++)
            {
                var tempEnemy = _enemyPool.GetObject();

                if (tempEnemy.TryGetComponent(out EnemyControls controls))
                {
                    controls.gameObject.SetActive(true);
                    controls.transform.localPosition = Vector3.zero;

                    _enemyList.Add(controls);
                }
            }
        }

        private void Update()
        {
            for (int index = 0; index < _enemyList.Count; index++)
                _enemyList[index].DoUpdate();
        }
    }
}
