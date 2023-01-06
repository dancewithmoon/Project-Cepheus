using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Logic;
using CodeBase.StaticData.Service;
using CodeBase.UI.Services.Factory;

namespace CodeBase.Infrastructure.States
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, AllServices services)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, services),
                
                [typeof(LoadProgressState)] = new LoadProgressState(this, 
                    services.Single<IPersistentProgressService>(), services.Single<ISaveLoadService>(), 
                    services.Single<IStaticDataService>()),
                
                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, loadingCurtain, 
                    services.Single<IGameFactory>(), 
                    services.Single<IPersistentProgressService>(), 
                    services.Single<IStaticDataService>(),
                    services.Single<IUIFactory>()),
                
                [typeof(GameLoopState)] = new GameLoopState(this)
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            var newState = ChangeState<TState>();
            newState.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            var newState = ChangeState<TState>();
            newState.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();
            var newState = GetState<TState>();
            _activeState = newState;
            return newState;
        }

        private TState GetState<TState>() where TState : class, IExitableState
        {
            return _states[typeof(TState)] as TState;
        }
    }
}