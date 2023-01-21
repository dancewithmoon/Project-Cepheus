using System.Collections.Generic;
using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Instantiating;
using CodeBase.Infrastructure.Services.ContainerService;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic.Spawner;
using CodeBase.StaticData;
using CodeBase.StaticData.Service;
using CodeBase.UI.Elements;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Infrastructure.Factory
{
    public class ZenjectGameFactory : IGameFactory
    {
        private readonly IAssets _assets;
        private readonly IInstantiateService _instantiateService;
        private readonly IStaticDataService _staticData;
        private readonly ContainerService _container;
        
        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

        private GameObject _hero;

        public ZenjectGameFactory(IAssets assets, IInstantiateService instantiateService, IStaticDataService staticData, ContainerService container)
        {
            _assets = assets;
            _instantiateService = instantiateService;
            _staticData = staticData;
            _container = container;
        }

        public GameObject CreateHero(GameObject initialPoint)
        {
            _hero = InstantiateRegistered(_staticData.GetHero().Prefab, initialPoint.transform.position);
            _container.Container.Bind<Transform>().WithId("hero").FromInstance(_hero.transform);
            return _hero;
        }

        public GameObject CreateHud()
        {
            GameObject hud = InstantiateRegistered(AssetPath.HudPath);
            hud.GetComponentInChildren<ActorUI>().Construct(_hero.GetComponent<HeroHealth>());
            return hud;
        }
        
        public GameObject CreateSavePoint(Vector3 at, Vector3 scale)
        {
            GameObject saveTrigger = InstantiateRegistered(AssetPath.SaveTrigger, at);
            saveTrigger.transform.localScale = scale;
            return saveTrigger;
        }

        public GameObject CreateEnemySpawner(Vector3 at, string spawnerId, EnemyTypeId enemyTypeId)
        {
            EnemySpawner spawner = InstantiateRegistered(AssetPath.SpawnerPath, at).GetComponent<EnemySpawner>();
            spawner.Initialize(spawnerId, enemyTypeId);
            return spawner.gameObject;
        }

        public GameObject CreateEnemy(EnemyTypeId enemyTypeId, Transform parent)
        {
            EnemyStaticData enemyData = _staticData.GetEnemy(enemyTypeId);
            GameObject enemy = _instantiateService.Instantiate(enemyData.Prefab, parent.position, parent);
            
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            enemyHealth.Initialize(enemyData.Hp, enemyData.Hp);

            EnemyAttack enemyAttack = enemy.GetComponent<EnemyAttack>();
            enemyAttack.Initialize(enemyData.Damage, enemyData.AttackPointRadius, enemyData.EffectiveDistance, enemyData.AttackCooldown);
            
            enemy.GetComponent<ActorUI>().Construct(enemyHealth);
            enemy.GetComponent<NavMeshAgent>().speed = enemyData.MovementSpeed;

            LootSpawner lootSpawner = enemy.GetComponentInChildren<LootSpawner>();
            lootSpawner.Initialize(enemyData.MinLoot, enemyData.MaxLoot);

            return enemy;
        }

        public LootPiece CreateLoot() => 
            InstantiateRegistered(AssetPath.Loot).GetComponent<LootPiece>();

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }

        private GameObject InstantiateRegistered(string path) => 
            InstantiateRegistered(_assets.Load(path));

        private GameObject InstantiateRegistered(string path, Vector3 at) => 
            InstantiateRegistered(_assets.Load(path), at);

        private GameObject InstantiateRegistered(GameObject prefab)
        {
            GameObject gameObject = _instantiateService.Instantiate(prefab);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(GameObject prefab, Vector3 at)
        {
            GameObject gameObject = _instantiateService.Instantiate(prefab, at);
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

        private void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
            {
                ProgressWriters.Add(progressWriter);
            }
            ProgressReaders.Add(progressReader);
        }
    }
}