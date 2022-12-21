using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    public class AgentMoveToHero : Aggrable
    {
        private const double MinimalDistance = 1f;
        
        [SerializeField] private NavMeshAgent _agent;

        private Transform _heroTransform;

        public void Construct(Transform hero)
        {
            _heroTransform = hero;
        }

        private void Update()
        {
            if (IsHeroInitialized() && IsAwayFromHero())
            {
                MoveToHero();
            }
        }

        private bool IsHeroInitialized() => _heroTransform != null;

        private bool IsAwayFromHero() => 
            Vector3.Distance(_agent.transform.position, _heroTransform.position) >= MinimalDistance;

        private void MoveToHero()
        {
            _agent.destination = _heroTransform.position;
        }
    }
}