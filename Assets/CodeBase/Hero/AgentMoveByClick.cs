using CodeBase.Services.UserInput;
using CodeBase.StaticData.Service;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentMoveByClick : MonoBehaviour
    {
        private IInputService _inputService;
        private int _groundLayer;

        private NavMeshAgent _agent;
        private CharacterController _characterController;

        [Inject]
        private void Construct(IInputService inputService, IStaticDataService staticDataService)
        {
            _inputService = inputService;
            _inputService.EnvironmentClicked += OnClick;

            _groundLayer = LayerMask.NameToLayer("Ground");
            
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = staticDataService.GetHero().Speed;
            
            _characterController = GetComponent<CharacterController>();
        }

        private void OnDestroy()
        {
            _inputService.EnvironmentClicked -= OnClick;
        }

        private void OnClick(GameObject obj, Vector3 position)
        {
            bool IsClickedOnGround() => obj.layer == _groundLayer;

            if(enabled == false)
                return;
            
            if (IsClickedOnGround() == false)
                return;

            DisableCharacterController();
            EnableNavMeshAgent();
            _agent.SetDestination(position);
        }

        private void DisableCharacterController()
        {
            if (_characterController == null)
                return;

            _characterController.enabled = false;
        }

        private void EnableNavMeshAgent()
        {
            _agent.enabled = true;
        }
    }
}