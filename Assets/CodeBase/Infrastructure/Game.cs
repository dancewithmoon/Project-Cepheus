using CodeBase.Infrastructure.States;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        public GameStateMachine StateMachine { get; }

        public Game(GameStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }
    }
}