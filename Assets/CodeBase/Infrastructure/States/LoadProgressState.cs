using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.StaticData;
using CodeBase.StaticData.Service;

namespace CodeBase.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IStaticDataService _staticDataService;

        public LoadProgressState(GameStateMachine stateMachine, IPersistentProgressService progressService,
            ISaveLoadService saveLoadService, IStaticDataService staticDataService)
        {
            _stateMachine = stateMachine;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _staticDataService = staticDataService;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();

            _stateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.PositionOnLevel.Level);
        }

        public void Exit()
        {
        }

        private void LoadProgressOrInitNew()
        {
            _progressService.Progress = _saveLoadService.LoadProgress() ?? InitNewProgress();
        }

        private PlayerProgress InitNewProgress()
        {
            PlayerProgress progress = new PlayerProgress("Main");

            HeroDefaultStaticData heroDefaultData = _staticDataService.GetHero();

            progress.WorldData.PositionOnLevel.Position = heroDefaultData.InitialPoint.AsVectorData();
            
            progress.AttackData.Damage = heroDefaultData.Damage;
            progress.AttackData.AttackPointRadius = heroDefaultData.AttackPointRadius;

            progress.HealthData.MaxHp = heroDefaultData.Hp;
            progress.HealthData.ResetHp();
            return progress;
        }
    }
}