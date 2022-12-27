using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Services.Input;
using CodeBase.Services.Randomizer;
using CodeBase.StaticData.Service;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            
            RegisterServices();
        }

        public void Enter()
        {
            _sceneLoader.Load(Scenes.Initial, EnterLoadLevel);
        }

        private void EnterLoadLevel()
        {
            _stateMachine.Enter<LoadProgressState>();
        }

        private void RegisterServices()
        {
            _services.RegisterSingle<IInputService>(GetInputService());
            _services.RegisterSingle<IRandomService>(new UnityRandomService());
            _services.RegisterSingle<IAssets>(new AssetProvider());
            RegisterStaticData();
            
            _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());

            _services.RegisterSingle<IGameFactory>(
                new GameFactory(
                    _services.Single<IAssets>(),
                    _services.Single<IStaticDataService>(),
                    _services.Single<IRandomService>()));

            _services.RegisterSingle<ISaveLoadService>(
                new SaveLoadService(
                    _services.Single<IPersistentProgressService>(), 
                    _services.Single<IGameFactory>()));
        }

        private void RegisterStaticData()
        {
            IStaticDataService staticData = new StaticDataService();
            staticData.LoadHero();
            staticData.LoadEnemies();
            _services.RegisterSingle<IStaticDataService>(staticData);
        }

        public void Exit()
        {
        }

        private static IInputService GetInputService()
        {
            if (Application.isEditor)
            {
                return new StandaloneInputService();
            }

            return new MobileInputService();
        }
    }
}