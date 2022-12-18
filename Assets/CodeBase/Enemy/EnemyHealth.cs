using System;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        [Header("Parameters")]
        [SerializeField] private float _current;
        [SerializeField] private float _max;
        
        [Header("Components")]
        [SerializeField] private EnemyAnimator _animator;

        public float Current => _current;
        public float Max => _max;

        public event Action HealthChanged;

        public void ApplyDamage(float damage)
        {
            _current -= damage;
            _animator.PlayHit();
            HealthChanged?.Invoke();
        }
    }
}