using System.Collections;
using System.Linq;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class Attack : MonoBehaviour
    {
        [SerializeField] private float _attackCooldown = 3f;
        [SerializeField] private float _cleavage = 0.5f;
        [SerializeField] private float _effectiveDistance = 0.5f;
        [SerializeField] private float _damage = 10f;

        [Header("References")]
        [SerializeField] private EnemyAnimator _animator;

        private IGameFactory _gameFactory;
        private Transform _heroTransform;
        private bool _isAttacking;
        private int _layerMask;
        private bool _isAttackEnabled;

        private readonly Collider[] _hits = new Collider[1];

        private void Awake()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();
            _layerMask = 1 << LayerMask.NameToLayer("Player");
            
            if (IsHeroExist())
            {
                StartAttackLoop();
            }
            else
            {
                _gameFactory.HeroCreated += OnHeroCreated;
            }
        }

        private void OnHeroCreated()
        {
            _gameFactory.HeroCreated -= OnHeroCreated;
            StartAttackLoop();
        }

        private void StartAttackLoop()
        {
            InitializeHeroTransform();
            StartCoroutine(AttackLoop());
        }

        private IEnumerator AttackLoop()
        {
            var cooldown = new WaitForSeconds(_attackCooldown);
            while (this)
            {
                yield return new WaitUntil(IsAttackEnabled);
                StartAttack();
                yield return new WaitWhile(IsAttacking);
                yield return cooldown;
            }
        }

        public void EnableAttack()
        {
            _isAttackEnabled = true;
        }

        public void DisableAttack()
        {
            _isAttackEnabled = false;
        }

        private void StartAttack()
        {
            _isAttacking = true;
            
            transform.LookAt(_heroTransform);
            _animator.PlayAttack();
        }

        private void OnAttack()
        {
            if (Hit(out Collider hit))
            {
                PhysicsDebug.DrawDebug(GetAttackPoint(), _cleavage, 1);
                hit.transform.GetComponent<HeroHealth>().ApplyDamage(_damage);
            }
        }

        private void OnAttackEnded()
        {
            _isAttacking = false;
        }

        private bool Hit(out Collider hit)
        {
            int hitCount = Physics.OverlapSphereNonAlloc(GetAttackPoint(), _cleavage, _hits, _layerMask);
            hit = _hits.FirstOrDefault();
            return hitCount > 0;
        }

        private Vector3 GetAttackPoint()
        {
            Vector3 attackPoint = transform.position + transform.forward * _effectiveDistance;
            attackPoint.y += 0.5f;
            return attackPoint;
        }
        
        private bool IsAttacking() => _isAttacking;
        private bool IsAttackEnabled() => _isAttackEnabled;

        private bool IsHeroExist() => _gameFactory.HeroGameObject != null;
        
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