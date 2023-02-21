using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(NavMeshAgent), typeof(CharacterController))]
    public class CharacterVelocity : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private CharacterController _characterController;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _characterController = GetComponent<CharacterController>();
        }

        public Vector3 Velocity =>
            _characterController.enabled
                ? _characterController.velocity
                : _agent.velocity;
    }
}