using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentVelocity : CharacterVelocity
    {
        [SerializeField] private NavMeshAgent _agent;
        public override Vector3 Velocity => _agent.velocity;
    }
}