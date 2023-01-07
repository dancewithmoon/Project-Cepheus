using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Logic;
using CodeBase.StaticData.Service;
using CodeBase.UI.Services.Factory;
using Zenject;

namespace CodeBase.Infrastructure.States
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, DiContainer container)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, container),
                
                [typeof(LoadProgressState)] = new LoadProgressState(this, 
                    container.Resolve<IPersistentProgressService>(), 
                    container.Resolve<ISaveLoadService>(), 
                    container.Resolve<IStaticDataService>()),
                
                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, loadingCurtain, 
                    container.Resolve<IGameFactory>(), 
                    container.Resolve<IPersistentProgressService>(), 
                    container.Resolve<IStaticDataService>(),
                    container.Resolve<IUIFactory>()),
                
                [typeof(GameLoopState)] = new GameLoopState(this)
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