using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace CodeBase.Enemy
{
    public class AgentMoveToHero : Aggrable
    {
        private const double MinimalDistance = 1f;
        
        [SerializeField] private NavMeshAgent _agent;

        [Inject(Id = "hero")] public Transform HeroTransform { get; set; }

        private void Update()
        {
            if (IsHeroInitialized() && IsAwayFromHero())
            {
                MoveToHero();
            }
        }

        private bool IsHeroInitialized() => HeroTransform != null;

        private bool IsAwayFromHero() => 
            Vector3.Distance(_agent.transform.position, HeroTransform.position) >= MinimalDistance;

        private void MoveToHero()
        {
            _agent.destination = HeroTransform.position;
        }
    }
}