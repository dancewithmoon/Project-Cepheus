using CodeBase.Infrastructure.States;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        public IGameStateMachine StateMachine { get; }

        public Game(IGameStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }
    }
}