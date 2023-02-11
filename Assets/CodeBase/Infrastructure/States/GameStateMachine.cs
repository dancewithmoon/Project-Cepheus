using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Logic;
using CodeBase.StaticData.Service;
using CodeBase.UI.Services.Factory;

namespace CodeBase.Infrastructure.States
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, IStaticDataService staticData,
            IPersistentProgressService progress, ISaveLoadService saveLoad, IGameFactory gameFactory, IUIFactory uiFactory)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = 
                    new BootstrapState(this, sceneLoader),

                [typeof(LoadProgressState)] = 
                    new LoadProgressState(this, progress, saveLoad, staticData),

                [typeof(LoadLevelState)] = 
                    new LoadLevelState(this, sceneLoader, loadingCurtain, gameFactory, progress, staticData, uiFactory),

                [typeof(GameLoopState)] = 
                    new GameLoopState(this)
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            TState newState = ChangeState<TState>();
            newState.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState newState = ChangeState<TState>();
            newState.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();
            TState newState = GetState<TState>();
            _activeState = newState;
            return newState;
        }

        private TState GetState<TState>() where TState : class, IExitableState
        {
            return _states[typeof(TState)] as TState;
        }
    }
}