using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Hero
{
    public class CharacterVelocity : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private CharacterController _characterController;

        public Vector3 Velocity =>
            _characterController.enabled
                ? _characterController.velocity
                : _agent.velocity;
    }
}