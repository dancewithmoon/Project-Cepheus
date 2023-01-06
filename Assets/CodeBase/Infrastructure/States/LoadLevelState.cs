using System.Collections.Generic;
using CodeBase.CameraLogic;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.StaticData;
using CodeBase.StaticData.Service;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Factory;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string InitialPointTag = "InitialPoint";

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progress;
        private readonly IStaticDataService _staticData;
        private readonly IUIFactory _uiFactory;

        private string _sceneName;

        public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
            IGameFactory gameFactory, IPersistentProgressService progress, IStaticDataService staticData, IUIFactory uiFactory)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _progress = progress;
            _staticData = staticData;
            _uiFactory = uiFactory;
        }

        public void Enter(string sceneName)
        {
            _sceneName = sceneName;
            
            _loadingCurtain.Show();
            _gameFactory.Cleanup();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            _loadingCurtain.Hide();
        }

        private void OnLoaded()
        {
            InitUIRoot();
            InitGameWorld();
            InformProgressReaders();
            _stateMachine.Enter<GameLoopState>();
        }

        private void InformProgressReaders()
        {
            _gameFactory.ProgressReaders.ForEach(progressReader => progressReader.LoadProgress(_progress.Progress));
        }

        private void InitUIRoot()
        {
            _uiFactory.CreateUIRoot();
        }

        private void InitGameWorld()
        {
            InitSpawners();
            InitLoot();
            GameObject hero = InitHero();
            InitHud(hero);
            CameraFollow(hero);
        }

        private void InitSpawners()
        {
            LevelStaticData levelData = _staticData.GetLevel(_sceneName);
            foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
            {
                _gameFactory.CreateEnemySpawner(spawnerData.Position, spawnerData.Id, spawnerData.EnemyTypeId);
            }
        }

        private void InitLoot()
        {
            foreach (KeyValuePair<string,LootPieceData> loot in _progress.Progress.WorldData.LootOnLevel.Loots)
            {
                if(loot.Value.PositionOnLevel.Level != _sceneName)
                    continue;

                LootPiece lootPiece = _gameFactory.CreateLoot();
                lootPiece.GetComponent<UniqueId>().Id = loot.Key;
            }
        }

        private GameObject InitHero() => 
            _gameFactory.CreateHero(GameObject.FindWithTag(InitialPointTag));

        private void InitHud(GameObject hero)
        {
            GameObject hud = _gameFactory.CreateHud();
            
            hud.GetComponentInChildren<ActorUI>().Construct(hero.GetComponent<HeroHealth>());
        }

        private static void CameraFollow(GameObject hero)
        {
            Camera.main.GetComponent<CameraFollow>().Follow(hero);
        }
    }
}