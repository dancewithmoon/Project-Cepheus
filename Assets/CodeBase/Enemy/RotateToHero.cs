using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class RotateToHero : Aggrable
    {
        [SerializeField] private float _speed;

        private IGameFactory _gameFactory;
        private Transform _heroTransform;
        private Vector3 _positionToLook;
        
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
            if (IsHeroInitialized())
            {
                RotateTowardsHero();
            }
        }

        private void RotateTowardsHero()
        {
            UpdatePositionToLook();

            transform.rotation = GetSmoothedRotation(transform.rotation, _positionToLook);
        }

        private void UpdatePositionToLook()
        {
            Vector3 positionDiff = _heroTransform.position - transform.position;
            _positionToLook = new Vector3(positionDiff.x, transform.position.y, positionDiff.z);
        }

        private Quaternion GetSmoothedRotation(Quaternion rotation, Vector3 positionToLook) => 
            Quaternion.Lerp(rotation, GetTargetRotation(positionToLook), GetSpeedFactor());
        
        private Quaternion GetTargetRotation(Vector3 positionToLook) => Quaternion.LookRotation(positionToLook);
        
        private float GetSpeedFactor() => _speed * Time.deltaTime;

        private bool IsHeroExist() => _gameFactory.HeroGameObject != null;
        private bool IsHeroInitialized() => _heroTransform != null;

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