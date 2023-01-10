using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.ContainerService;
using CodeBase.Infrastructure.Services.CoroutineRunner;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Services.Input;
using CodeBase.Services.Randomizer;
using CodeBase.StaticData.Service;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Screens;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class GameBootstrapper : MonoInstaller, ICoroutineRunner
    {
        [SerializeField] private LoadingCurtain _loadingCurtain;
        private Game _game;

        public override void InstallBindings()
        {
            Application.targetFrameRate = 60;
            
            BindServices();

            Container.Bind<LoadingCurtain>().FromInstance(_loadingCurtain).AsSingle();
            Container.Bind<GameStateMachine>().AsSingle();
            
            Container.Bind<Game>().AsSingle();
        }

        private void BindServices()
        {
            Container.Bind<ContainerService>().AsSingle();
            Container.Bind<ICoroutineRunner>().FromMethod(GetCoroutineRunner).AsSingle();
            Container.Bind<SceneLoader>().AsSingle();
            Container.Bind<IInputService>().FromMethod(GetInputService);
            Container.Bind<IAssets>().To<ZenjectAssetProvider>().AsSingle();
            Container.Bind<IPersistentProgressService>().To<PersistentProgressService>().AsSingle();
            Container.Bind<IRandomService>().To<UnityRandomService>().AsSingle();
            Container.Bind<IStaticDataService>().To<StaticDataService>().AsSingle();
            Container.Bind<IUIFactory>().To<ZenjectUIFactory>().AsSingle();
            Container.Bind<IScreenService>().To<ScreenService>().AsSingle();
            Container.Bind<IGameFactory>().To<ZenjectGameFactory>().AsSingle();
            Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();
        }
        
        public override void Start()
        {
            _game = Container.Resolve<Game>();
            _game.StateMachine.Enter<BootstrapState>();
        }

        private static ICoroutineRunner GetCoroutineRunner()
        {
            CoroutineRunner coroutineRunner = new GameObject("CoroutineRunner").AddComponent<CoroutineRunner>();
            DontDestroyOnLoad(coroutineRunner);
            return coroutineRunner;
        }
        
        private static IInputService GetInputService()
        {
            return Application.isEditor
                ? (IInputService)new StandaloneInputService()
                : new MobileInputService();
        }
    }
}