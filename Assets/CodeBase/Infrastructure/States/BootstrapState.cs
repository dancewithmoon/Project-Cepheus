using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Services.Input;
using CodeBase.Services.Randomizer;
using CodeBase.StaticData.Service;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Screens;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly DiContainer _container;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, DiContainer container)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _container = container;

            RegisterServices();
        }

        public void Enter()
        {
            _container.Resolve<IStaticDataService>().Load();
            _sceneLoader.Load(Scenes.Initial, EnterLoadLevel);
        }

        private void EnterLoadLevel()
        {
            _stateMachine.Enter<LoadProgressState>();
        }

        private void RegisterServices()
        {
            _container.Bind<IInputService>().FromMethod(GetInputService);
            _container.Bind<IAssets>().To<ZenjectAssetProvider>().AsSingle();
            _container.Bind<IPersistentProgressService>().To<PersistentProgressService>().AsSingle();
            _container.Bind<IRandomService>().To<UnityRandomService>().AsSingle();
            _container.Bind<IStaticDataService>().To<StaticDataService>().AsSingle();
            _container.Bind<IUIFactory>().To<ZenjectUIFactory>().AsSingle();
            _container.Bind<IScreenService>().To<ScreenService>().AsSingle();
            _container.Bind<IGameFactory>().To<ZenjectGameFactory>().AsSingle();
            _container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();
        }

        public void Exit()
        {
        }

        private static IInputService GetInputService() =>
            Application.isEditor 
                ? (IInputService)new StandaloneInputService() 
                : new MobileInputService();
        
    }
}