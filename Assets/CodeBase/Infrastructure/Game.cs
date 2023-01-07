using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        public GameStateMachine StateMachine { get; }

        public Game(ICoroutineRunner coroutineRunner, LoadingCurtain loadingCurtain, DiContainer container)
        {
            container.Bind<GameStateMachine>().AsSingle();
            StateMachine = container.Resolve<GameStateMachine>();
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), loadingCurtain);
        }
    }
}