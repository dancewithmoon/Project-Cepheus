using CodeBase.Infrastructure.States;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic
{
    public class LevelTransferTrigger : MonoBehaviour
    {
        private const string PlayerTag = "Player";
    
        [SerializeField] private string _transferTo; //TODO: Add selector of existing scenes
        private IGameStateMachine _stateMachine;

        private bool _triggered;
    
        [Inject]
        public void Construct(IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_triggered) 
                return;
        
            if (other.CompareTag(PlayerTag))
            {
                _stateMachine.Enter<LoadLevelState, string>(_transferTo);
                _triggered = true;
            }
        }
    }
}
