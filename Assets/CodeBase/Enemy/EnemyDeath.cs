using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class EnemyDeath : MonoBehaviour
    {
        private const int TimeToDestroy = 3;

        [Header("Components")]
        [SerializeField] private EnemyHealth _health;
        [SerializeField] private AgentMoveToHero _move;
        [SerializeField] private EnemyAttack _attack;
        [SerializeField] private EnemyAnimator _animator;

        [Header("VFX")] 
        [SerializeField] private GameObject _deathFxPrefab;

        public event Action Happened;
        
        private void Start()
        {
            _health.HealthChanged += OnHealthChanged;
        }

        private void OnHealthChanged()
        {
            if (_health.Current > 0)
                return;

            Die();
        }

        private void Die()
        {
            _health.HealthChanged -= OnHealthChanged;
            
            _health.enabled = false;
            _move.enabled = false;
            _attack.enabled = false;
            _animator.PlayDeath();

            PlayDeathFx();
            
            Happened?.Invoke();
            
            StartCoroutine(DestroyWithDelay());
        }
        
        private void PlayDeathFx()
        {
            Instantiate(_deathFxPrefab, transform.position, Quaternion.identity);
        }

        private IEnumerator DestroyWithDelay()
        {
            yield return new WaitForSeconds(TimeToDestroy);
            Destroy(gameObject);
        }
        
        private void OnDestroy()
        {
            _health.HealthChanged -= OnHealthChanged;
        }
    }
}