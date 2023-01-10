using System.Collections.Generic;
using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Logic;
using CodeBase.Logic.Spawner;
using CodeBase.Services.Input;
using CodeBase.Services.Randomizer;
using CodeBase.StaticData;
using CodeBase.StaticData.Service;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Screens;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssets _assets;
        private readonly IStaticDataService _staticData;
        private readonly IRandomService _randomService;
        private readonly IPersistentProgressService _progressService;
        private readonly IScreenService _screenService;
        private readonly IInputService _inputService;
        private readonly ISaveLoadService _saveLoadService;
        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

        private GameObject _hero;

        public GameFactory(IAssets assets, IStaticDataService staticData, IRandomService randomService,
            IPersistentProgressService progressService, IScreenService screenService, IInputService inputService, ISaveLoadService saveLoadService)
        {
            _assets = assets;
            _staticData = staticData;
            _randomService = randomService;
            _progressService = progressService;
            _screenService = screenService;
            _inputService = inputService;
            _saveLoadService = saveLoadService;
        }

        public GameObject CreateHero(GameObject initialPoint)
        {
            _hero = InstantiateRegistered(AssetPath.HeroPath, initialPoint.transform.position);
            _hero.GetComponent<HeroLootPickUp>().Construct(_progressService);
            _hero.GetComponent<HeroMove>().Construct(_inputService);
            _hero.GetComponent<HeroAttack>().Construct(_inputService);
            return _hero;
        }

        public GameObject CreateHud()
        {
            GameObject hud = InstantiateRegistered(AssetPath.HudPath);
            hud.GetComponentInChildren<ActorUI>().Construct(_hero.GetComponent<HeroHealth>());
            hud.GetComponentInChildren<LootCountView>().Construct(_progressService);
            foreach (OpenScreenButton button in hud.GetComponentsInChildren<OpenScreenButton>())
            {
                button.Construct(_screenService);
            }
            return hud;
        }

        public GameObject CreateSavePoint(Vector3 at, Vector3 scale)
        {
            SaveTrigger saveTrigger = InstantiateRegistered(AssetPath.SaveTrigger, at).GetComponent<SaveTrigger>();
            saveTrigger.Construct(_saveLoadService);
            saveTrigger.transform.localScale = scale;
            return saveTrigger.gameObject;
        }

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
            enemyAttack.HeroTransform = _hero.transform;
            enemyAttack.Initialize(enemyData.Damage, enemyData.AttackPointRadius, enemyData.EffectiveDistance, enemyData.AttackCooldown);
            
            enemy.GetComponent<ActorUI>().Construct(enemyHealth);
            enemy.GetComponent<AgentMoveToHero>().HeroTransform = _hero.transform;
            
            enemy.GetComponent<NavMeshAgent>().speed = enemyData.MovementSpeed;

            LootSpawner lootSpawner = enemy.GetComponentInChildren<LootSpawner>();
            lootSpawner.Construct(this, _randomService);
            lootSpawner.Initialize(enemyData.MinLoot, enemyData.MaxLoot);

            return enemy;
        }

        public LootPiece CreateLoot()
        {
            LootPiece lootPiece = InstantiateRegistered(AssetPath.Loot).GetComponent<LootPiece>();
            lootPiece.Construct(_progressService);
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

        private GameObject InstantiateRegistered(string path, Vector3 at)
        {
            GameObject gameObject = _assets.Instantiate(path, at);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(string path)
        {
            GameObject gameObject = _assets.Instantiate(path);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private void RegisterProgressWatchers(GameObject hero)
        {
            foreach (ISavedProgressReader progressReader in hero.GetComponentsInChildren<ISavedProgressReader>())
            {
                Register(progressReader);
            }
        }
    }
}