using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
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
            InitGame();
        }

        private void InitGame()
        {
            Container.Bind<ICoroutineRunner>().FromMethod(CreateCoroutineRunner).AsSingle();
            Container.Bind<LoadingCurtain>().FromInstance(_loadingCurtain).AsSingle();
            Container.Bind<SceneLoader>().AsSingle();
            
            Container.Bind<Game>().AsSingle();
        }

        public override void Start()
        {
            _game = Container.Resolve<Game>();
            _game.StateMachine.Enter<BootstrapState>();
        }

        private static ICoroutineRunner CreateCoroutineRunner()
        {
            CoroutineRunner coroutineRunner = new GameObject("CoroutineRunner").AddComponent<CoroutineRunner>();
            DontDestroyOnLoad(coroutineRunner);
            return coroutineRunner;
        }
    }
}