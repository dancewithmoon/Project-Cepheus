using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroHealth : MonoBehaviour, ISavedProgress, IHealth
    {
        [SerializeField] private HeroAnimator _animator;
        private HealthData _health;

        public event Action HealthChanged;
        
        public float Current
        {
            get => _health.CurrentHp;
            set
            {
                if (_health.CurrentHp != value)
                {
                    _health.CurrentHp = value;
                    HealthChanged?.Invoke();
                }
            }
        }

        public float Max
        {
            get => _health.MaxHp;
            set => _health.MaxHp = value;
        }

        public void ApplyDamage(float damage)
        {
            if (enabled == false)
                return;
            
            Current -= damage;
            _animator.PlayHit();
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _health = progress.HeroHealthData.Clone();
            HealthChanged?.Invoke();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.HeroHealthData.CurrentHp = Current;
            progress.HeroHealthData.MaxHp = Max;
        }
    }
}