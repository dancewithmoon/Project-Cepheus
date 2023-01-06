using System;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private EnemyAnimator _animator;

        public event Action HealthChanged;

        public float Current { get; set; }
        public float Max { get; set; }

        public void Initialize(float current, float max)
        {
            Current = current;
            Max = max;
        }

        public void ApplyDamage(float damage)
        {
            Current -= damage;
            _animator.PlayHit();
            HealthChanged?.Invoke();
        }
    }
}