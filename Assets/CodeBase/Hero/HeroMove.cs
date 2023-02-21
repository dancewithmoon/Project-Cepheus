using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services.UserInput;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(CharacterController))]
    public class HeroMove : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed = 4.0f;
        [SerializeField] private CharacterController _characterController;
        private IInputService _inputService;
        private Camera _camera;
        private NavMeshAgent _agent;

        [Inject]
        public void Construct(IInputService inputService, IPersistentProgressService progressService)
        {
            _inputService = inputService;
            
            _camera = Camera.main;
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            Vector3 movementVector = Vector3.zero;

            if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
            {
                DisableNavMeshAgent();
                EnableCharacterController();
                
                movementVector = _camera.transform.TransformDirection(_inputService.Axis);
                movementVector.y = 0;
                movementVector.Normalize();

                transform.forward = movementVector;
            }

            movementVector += Physics.gravity;

            _characterController.Move(_movementSpeed * movementVector * Time.deltaTime);
        }

        private void DisableNavMeshAgent()
        {
            if(_agent == null)
                return;

            _agent.enabled = false;
        }

        private void EnableCharacterController()
        {
            _characterController.enabled = true;
        }
    }
}