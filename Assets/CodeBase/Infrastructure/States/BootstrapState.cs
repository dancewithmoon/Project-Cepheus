using CodeBase.Services.Ads;
using CodeBase.StaticData.Service;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IStaticDataService _staticData;
        private readonly IAdsService _adsService;

        public BootstrapState(IGameStateMachine stateMachine, SceneLoader sceneLoader, IStaticDataService staticData, IAdsService adsService)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _staticData = staticData;
            _adsService = adsService;
        }

        public async void Enter()
        {
            await _staticData.Load();
            _adsService.Initialize();
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