using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            RegisterServices();
            _sceneLoader.Load(Scenes.Initial, EnterLoadLevel);
        }

        private void RegisterServices()
        {
            Game.InputService = RegisterInputService();
        }

        private void EnterLoadLevel()
        {
            _stateMachine.Enter<LoadLevelState>();
        }

        public void Exit()
        {
        }

        private static IInputService RegisterInputService()
        {
            if (Application.isEditor)
            {
                return new StandaloneInputService();
            }

            return new MobileInputService();
        }
    }
}