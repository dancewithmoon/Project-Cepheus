namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly SceneLoader _sceneLoader;
        private readonly IGameStateMachine _stateMachine;

        public BootstrapState(IGameStateMachine stateMachine, SceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            _sceneLoader.Load(Scenes.Initial, EnterLoadLevel);
        }

        public void Exit()
        {
        }

        private void EnterLoadLevel()
        {
            _stateMachine.Enter<LoadProgressState>();
        }
    }
}