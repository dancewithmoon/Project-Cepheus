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
        private IGameFactory _gameFactory;
        private IPersistentProgressService _progressService;

        private string _id;
        private EnemyTypeId _enemyTypeId;

        private EnemyDeath _enemyDeath;

        private EnemiesData EnemiesData => _progressService.Progress.EnemiesData;
        
        [Inject]
        public void Construct(IGameFactory gameFactory, IPersistentProgressService progressService)
        {
            _gameFactory = gameFactory;
            _progressService = progressService;
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

        public void LoadProgress()
        {
            if (EnemiesData.KilledEnemies.Contains(_id))
            {
                _slain = true;
                return;
            }

            Spawn();
        }

        public void UpdateProgress()
        {
            if (_slain)
            {
                EnemiesData.KilledEnemies.Add(_id);
            }
        }
        
        private void OnEnemyDeathHappened()
        {
            if (_enemyDeath != null)
            {
                _enemyDeath.Happened -= OnEnemyDeathHappened;
            }

            _slain = true;
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