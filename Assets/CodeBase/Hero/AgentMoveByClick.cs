using System.Collections.Generic;
using CodeBase.Services.UserInput;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Zenject;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentMoveByClick : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private NavMeshAgent _agent;
        
        private IInputService _inputService;
        private int _groundLayer;

        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;
            _inputService.EnvironmentClicked += OnClick;

            _groundLayer = LayerMask.NameToLayer("Ground");
        }

        private void OnDestroy()
        {
            _inputService.EnvironmentClicked -= OnClick;
        }

        private void OnClick(GameObject obj, Vector3 position)
        {
            if (obj.layer == _groundLayer)
            {
                _agent.SetDestination(position);
                _characterController.enabled = false;
            }
        }
    }
}