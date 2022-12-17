using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    public class AgentMoveToHero : Aggrable
    {
        private const double MinimalDistance = 1f;
        
        [SerializeField] private NavMeshAgent _agent;

        private IGameFactory _gameFactory;
        private Transform _heroTransform;

        private void Awake()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();
            
            if (IsHeroExist())
            {
                InitializeHeroTransform();
            }
            else
            {
                _gameFactory.HeroCreated += OnHeroCreated;
            }
        }

        private void Update()
        {
            if (IsHeroInitialized() && IsAwayFromHero())
            {
                MoveToHero();
            }
        }
        
        private bool IsHeroExist() => _gameFactory.HeroGameObject != null;
        private bool IsHeroInitialized() => _heroTransform != null;
        private bool IsAwayFromHero() => 
            Vector3.Distance(_agent.transform.position, _heroTransform.position) >= MinimalDistance;

        private void MoveToHero()
        {
            _agent.destination = _heroTransform.position;
        }
        
        private void OnHeroCreated()
        {
            InitializeHeroTransform();
            _gameFactory.HeroCreated -= OnHeroCreated;
        }

        private void InitializeHeroTransform()
        {
            _heroTransform = _gameFactory.HeroGameObject.transform;
        }

        private void OnDestroy()
        {
            _gameFactory.HeroCreated -= OnHeroCreated;
        }
    }
}