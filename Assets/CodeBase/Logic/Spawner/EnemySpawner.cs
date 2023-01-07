using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic.Spawner
{
    public class EnemySpawner : MonoBehaviour, ISavedProgress
    {
        [SerializeField] public bool _slain;

        private string _id;
        private EnemyTypeId _enemyTypeId;
        
        private IGameFactory _gameFactory;
        private EnemyDeath _enemyDeath;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public void Initialize(string id, EnemyTypeId enemyType)
        {
            _id = id;
            _enemyTypeId = enemyType;
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