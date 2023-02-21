using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services.UserInput;
using CodeBase.StaticData.Service;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(CharacterController))]
    public class HeroMove : MonoBehaviour
    {
        private float _movementSpeed = 4.0f;
        private IInputService _inputService;
        private Camera _camera;
        private CharacterController _characterController;
        private NavMeshAgent _agent;

        [Inject]
        public void Construct(IInputService inputService, IStaticDataService staticDataService, IPersistentProgressService progressService)
        {
            _inputService = inputService;
            
            _camera = Camera.main;
            _characterController = GetComponent<CharacterController>();
            _agent = GetComponent<NavMeshAgent>();
            _movementSpeed = staticDataService.GetHero().Speed;
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

            if (_characterController.enabled)
            {
                _characterController.Move(_movementSpeed * movementVector * Time.deltaTime);
            }
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