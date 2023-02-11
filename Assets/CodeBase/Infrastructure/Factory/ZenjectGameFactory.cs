using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        
        public ZenjectGameFactory(IAssets assets, IInstantiateService instantiateService, IStaticDataService staticData,
            ContainerService container)
        {
            _assets = assets;
            _instantiateService = instantiateService;
            _staticData = staticData;
            _container = container;
        }

        public async Task WarmUp()
        {
            List<Task> tasks = new List<Task>
            {
                _assets.Load<GameObject>(_staticData.GetHero().PrefabReference),
                _assets.Load<GameObject>(AssetPath.HudPath),
                _assets.Load<GameObject>(AssetPath.SaveTrigger),
                _assets.Load<GameObject>(AssetPath.SpawnerPath),
                _assets.Load<GameObject>(AssetPath.Loot)
            };

            foreach (EnemyTypeId enemyType in Enum.GetValues(typeof(EnemyTypeId)))
            {
                tasks.Add(_assets.Load<GameObject>(_staticData.GetEnemy(enemyType).PrefabReference));
            }

            await Task.WhenAll(tasks);
        }

        public async Task<GameObject> CreateHero()
        {
            GameObject heroPrefab = await _assets.Load<GameObject>(_staticData.GetHero().PrefabReference);
            _hero = InstantiateRegistered(heroPrefab);
            _container.Container.Bind<Transform>().WithId("hero").FromInstance(_hero.transform);
            return _hero;
        }

        public async Task<GameObject> CreateHud()
        {
            GameObject hud = await InstantiateRegistered(AssetPath.HudPath);
            hud.GetComponentInChildren<ActorUI>().Construct(_hero.GetComponent<HeroHealth>());
            return hud;
        }

        public async Task<GameObject> CreateSavePoint(Vector3 at, Vector3 scale)
        {
            GameObject saveTrigger = await InstantiateRegistered(AssetPath.SaveTrigger, at);
            saveTrigger.transform.localScale = scale;
            return saveTrigger;
        }

        public async Task<GameObject> CreateEnemySpawner(Vector3 at, string spawnerId, EnemyTypeId enemyTypeId)
        {
            GameObject spawnerObject = await InstantiateRegistered(AssetPath.SpawnerPath, at);
            EnemySpawner spawner = spawnerObject.GetComponent<EnemySpawner>();
            spawner.Initialize(spawnerId, enemyTypeId);
            return spawner.gameObject;
        }

        public async Task<GameObject> CreateEnemy(EnemyTypeId enemyTypeId, Transform parent)
        {
            EnemyStaticData enemyData = _staticData.GetEnemy(enemyTypeId);

            GameObject prefab = await _assets.Load<GameObject>(enemyData.PrefabReference);

            GameObject enemy = _instantiateService.Instantiate(prefab, parent.position, parent);

            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            enemyHealth.Initialize(enemyData.Hp, enemyData.Hp);

            EnemyAttack enemyAttack = enemy.GetComponent<EnemyAttack>();
            enemyAttack.Initialize(enemyData.Damage, enemyData.AttackPointRadius, enemyData.EffectiveDistance,
                enemyData.AttackCooldown);

            enemy.GetComponent<ActorUI>().Construct(enemyHealth);
            enemy.GetComponent<NavMeshAgent>().speed = enemyData.MovementSpeed;

            LootSpawner lootSpawner = enemy.GetComponentInChildren<LootSpawner>();
            lootSpawner.Initialize(enemyData.MinLoot, enemyData.MaxLoot);

            return enemy;
        }

        public async Task<LootPiece> CreateLoot()
        {
            GameObject loot = await InstantiateRegistered(AssetPath.Loot);
            return loot.GetComponent<LootPiece>();
        }

        public void CleanUp()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }

        private async Task<GameObject> InstantiateRegistered(string path)
        {
            GameObject asset = await _assets.Load<GameObject>(path);
            return InstantiateRegistered(asset);
        }

        private async Task<GameObject> InstantiateRegistered(string path, Vector3 at)
        {
            GameObject asset = await _assets.Load<GameObject>(path);
            return InstantiateRegistered(asset, at);
        }

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