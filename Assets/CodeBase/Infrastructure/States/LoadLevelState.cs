using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeBase.CameraLogic;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.StaticData;
using CodeBase.StaticData.Service;
using CodeBase.UI.Services.Factory;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progress;
        private readonly IStaticDataService _staticData;
        private readonly IUIFactory _uiFactory;

        private string _sceneName;
        private LevelStaticData _levelData;
        private Task _warmUp;

        public LoadLevelState(IGameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
            IGameFactory gameFactory, IPersistentProgressService progress, IStaticDataService staticData,
            IUIFactory uiFactory)
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
            _gameFactory.CleanUp();

            _levelData = _staticData.GetLevel(_sceneName);
            _warmUp = WarmUp();

            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit() => 
            _loadingCurtain.Hide();

        private async Task WarmUp()
        {
            IEnumerable<EnemyTypeId> enemyTypesOnLevel =
                _levelData.EnemySpawners.Select(spawner => spawner.EnemyTypeId).Distinct();

            await Task.WhenAll(
                _gameFactory.WarmUp(enemyTypesOnLevel),
                _uiFactory.WarmUp());
        }

        private void InitUIRoot() => 
            _uiFactory.CreateUIRoot();

        private async void OnLoaded()
        {
            await _warmUp;
            InitUIRoot();
            await InitGameWorld();
            InformProgressReaders();
            _stateMachine.Enter<GameLoopState>();
        }

        private async Task InitGameWorld()
        {
            InitSpawners(_levelData.EnemySpawners);
            InitSavePoints(_levelData.SavePoints);
            InitLoot();
            GameObject hero = await InitHero();
            InitHud();
            CameraFollow(hero);
        }

        private void InitSpawners(IEnumerable<EnemySpawnerData> enemySpawners)
        {
            foreach (EnemySpawnerData spawnerData in enemySpawners)
            {
                _gameFactory.CreateEnemySpawner(spawnerData.Position, spawnerData.Id, spawnerData.EnemyTypeId);
            }
        }

        private void InitSavePoints(IEnumerable<SavePointData> savePoints)
        {
            foreach (SavePointData savePointData in savePoints)
            {
                _gameFactory.CreateSavePoint(savePointData.Position, savePointData.Scale);
            }
        }

        private async void InitLoot()
        {
            foreach (KeyValuePair<string, LootPieceData> loot in _progress.Progress.WorldData.LootOnLevel.Loots)
            {
                if (loot.Value.PositionOnLevel.Level != _sceneName)
                    continue;

                LootPiece lootPiece = await _gameFactory.CreateLoot();
                lootPiece.GetComponent<UniqueId>().Id = loot.Key;
            }
        }

        private async Task<GameObject> InitHero()
        {
            if (_progress.Progress.WorldData.PositionOnLevel.Position == null)
            {
                _progress.Progress.WorldData.PositionOnLevel.Position = _levelData.InitialHeroPoint.AsVectorData();
            }

            return await _gameFactory.CreateHero();
        }

        private void InitHud() =>
            _gameFactory.CreateHud();

        private void InformProgressReaders() => 
            _gameFactory.ProgressReaders.ForEach(progressReader => progressReader.LoadProgress());

        private static void CameraFollow(GameObject hero) => 
            Camera.main.GetComponent<CameraFollow>().Follow(hero);
    }
}