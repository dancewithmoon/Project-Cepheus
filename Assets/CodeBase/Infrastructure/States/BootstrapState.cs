using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Infrastructure.Factory;
using CodeBase.UI.Services.Factory;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IGameFactory _gameFactory;
        private readonly IUIFactory _uiFactory;

        public BootstrapState(IGameStateMachine stateMachine, SceneLoader sceneLoader, IGameFactory gameFactory,
            IUIFactory uiFactory)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _uiFactory = uiFactory;
        }

        public void Enter()
        {
            _sceneLoader.Load(Scenes.Initial, EnterLoadLevel);
        }

        public void Exit()
        {
        }

        private async void EnterLoadLevel()
        {
            await Task.WhenAll(
                _gameFactory.WarmUp(), 
                _uiFactory.WarmUp());
            
            _stateMachine.Enter<LoadProgressState>();
        }
    }
}