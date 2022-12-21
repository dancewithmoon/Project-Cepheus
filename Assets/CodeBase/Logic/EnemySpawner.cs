using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Logic
{
    [RequireComponent(typeof(UniqueId))]
    public class EnemySpawner : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private EnemyTypeId _enemyTypeId;
        [SerializeField] public bool _slain;
        
        private string _id;
        private IGameFactory _gameFactory;
        private EnemyDeath _enemyDeath;
        
        private void Awake()
        {
            _id = GetComponent<UniqueId>().Id;
            _gameFactory = AllServices.Container.Single<IGameFactory>();
        }

        private void Spawn()
        {
            GameObject enemy = _gameFactory.CreateEnemy(_enemyTypeId, transform);
            _enemyDeath = enemy.GetComponent<EnemyDeath>();
            _enemyDeath.Happened += OnEnemyDeathHappened;
        }

        private void OnEnemyDeathHappened()
        {
            if (_enemyDeath != null)
            {
                _enemyDeath.Happened -= OnEnemyDeathHappened;
            }

            _slain = true;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.EnemiesData.KilledEnemies.Contains(_id))
            {
                _slain = true;
                return;
            }

            Spawn();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (_slain)
            {
                progress.EnemiesData.KilledEnemies.Add(_id);
            }
        }

        private void OnDestroy()
        {
            if (_enemyDeath != null)
            {
                _enemyDeath.Happened -= OnEnemyDeathHappened;
            }
        }
    }
}