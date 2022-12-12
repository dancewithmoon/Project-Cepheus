using System;
using System.Collections;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class Attack : MonoBehaviour
    {
        [SerializeField] private float _attackCooldown = 3f;
        
        [SerializeField] private EnemyAnimator _animator;

        private IGameFactory _gameFactory;
        private Transform _heroTransform;
        private bool _isAttacking;
        
        private void Awake()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();
            
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
                StartAttack();
                yield return new WaitWhile(IsAttacking);
                yield return cooldown;
            }
        }

        private void StartAttack()
        {
            _isAttacking = true;
            
            transform.LookAt(_heroTransform);
            _animator.PlayAttack();
        }
        
        private void OnAttack()
        {
            Debug.Log("ATTACK");
        }

        private void OnAttackEnded()
        {
            _isAttacking = false;
        }

        private bool IsAttacking() => _isAttacking;
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