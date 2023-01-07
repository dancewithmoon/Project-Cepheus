using System.Collections.Generic;
using CodeBase.Enemy;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic.Spawner;
using CodeBase.Services.Randomizer;
using CodeBase.StaticData;
using CodeBase.StaticData.Service;
using CodeBase.UI.Elements;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace CodeBase.Infrastructure.Factory
{
    public class ZenjectGameFactory : IGameFactory
    {
        private readonly IAssets _assets;
        private readonly IStaticDataService _staticData;
        private readonly IRandomService _randomService;
        private readonly IPersistentProgressService _progressService;
        private readonly DiContainer _container;
        
        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

        private GameObject _hero;

        public ZenjectGameFactory(IAssets assets, IPersistentProgressService progressService, DiContainer container)
        {
            _assets = assets;
            _progressService = progressService;
            _container = container;

            _staticData = AllServices.Container.Single<IStaticDataService>();
            _randomService = AllServices.Container.Single<IRandomService>();
        }

        public GameObject CreateHero(GameObject initialPoint)
        {
            _hero = InstantiateRegistered(AssetPath.HeroPath, initialPoint.transform.position);
            return _hero;
        }

        public GameObject CreateHud() => 
            InstantiateRegistered(AssetPath.HudPath);

        public GameObject CreateEnemySpawner(Vector3 at, string spawnerId, EnemyTypeId enemyTypeId)
        {
            EnemySpawner spawner = InstantiateRegistered(AssetPath.SpawnerPath, at).GetComponent<EnemySpawner>();
            spawner.Construct(this);
            spawner.Initialize(spawnerId, enemyTypeId);
            return spawner.gameObject;
        }

        public GameObject CreateEnemy(EnemyTypeId enemyTypeId, Transform parent)
        {
            EnemyStaticData enemyData = _staticData.GetEnemy(enemyTypeId);
            GameObject enemy = Object.Instantiate(enemyData.Prefab, parent.position, Quaternion.identity, parent);
            
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            enemyHealth.Initialize(enemyData.Hp, enemyData.Hp);

            EnemyAttack enemyAttack = enemy.GetComponent<EnemyAttack>();
            enemyAttack.Construct(_hero.transform);
            enemyAttack.Initialize(enemyData.Damage, enemyData.AttackPointRadius, enemyData.EffectiveDistance, enemyData.AttackCooldown);
            
            enemy.GetComponent<ActorUI>().Construct(enemyHealth);
            enemy.GetComponent<AgentMoveToHero>().Construct(_hero.transform);
            
            enemy.GetComponent<NavMeshAgent>().speed = enemyData.MovementSpeed;

            LootSpawner lootSpawner = enemy.GetComponentInChildren<LootSpawner>();
            lootSpawner.Construct(this, _randomService);
            lootSpawner.Initialize(enemyData.MinLoot, enemyData.MaxLoot);

            return enemy;
        }

        public LootPiece CreateLoot()
        {
            LootPiece lootPiece = InstantiateRegistered(AssetPath.Loot).GetComponent<LootPiece>();
            lootPiece.Construct(_progressService.Progress.WorldData.LootOnLevel);
            return lootPiece;
        }

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }

        private void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
            {
                ProgressWriters.Add(progressWriter);
            }
            ProgressReaders.Add(progressReader);
        }
        
        private GameObject InstantiateRegistered(string path)
        {
            GameObject gameObject = _assets.Instantiate(path);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }
        
        private GameObject InstantiateRegistered(string path, Vector3 at)
        {
            GameObject gameObject = _assets.Instantiate(path, at);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
            {
                Register(progressReader);
            }
        }
    }
}