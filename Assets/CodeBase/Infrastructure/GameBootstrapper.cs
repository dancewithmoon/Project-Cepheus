using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Instantiating;
using CodeBase.Infrastructure.Services.ContainerService;
using CodeBase.Infrastructure.Services.CoroutineRunner;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Services.Ads;
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
            Container.Bind<IGameStateMachine>().To<GameStateMachine>().AsSingle();
            
            Container.Bind<Game>().AsSingle();
        }

        private void BindServices()
        {
            Container.Bind<ContainerService>().AsSingle();
            Container.Bind<ICoroutineRunner>().FromMethod(GetCoroutineRunner).AsSingle();
            Container.Bind<SceneLoader>().AsSingle();
            Container.Bind<IInstantiateService>().To<ZenjectInstantiateService>().AsSingle();
            Container.Bind<IInputService>().FromMethod(GetInputService);
            Container.Bind<IAssets>().To<AddressableAssets>().AsSingle();

            PersistentProgressService progressService = new PersistentProgressService();
            Container.Bind<IPersistentProgressService>().FromInstance(progressService).AsSingle();
            Container.Bind<IReadonlyProgressService>().FromInstance(progressService).AsSingle();
            
            Container.Bind<IRandomService>().To<UnityRandomService>().AsSingle();
            
            Container.Bind<IStaticDataService>().To<StaticDataService>().AsSingle();
            Container.Resolve<IStaticDataService>().Load();

            Container.Bind<IUIFactory>().To<ZenjectUIFactory>().AsSingle();
            Container.Bind<IScreenService>().To<ScreenService>().AsSingle();
            Container.Bind<IGameFactory>().To<ZenjectGameFactory>().AsSingle();
            Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();
            Container.Bind<IAdsService>().To<UnityAdsService>().AsSingle();
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