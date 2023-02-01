using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using UnityEngine;
using Zenject;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroHealth : MonoBehaviour, ISavedProgress, IHealth
    {
        [SerializeField] private HeroAnimator _animator;
        private IPersistentProgressService _progressService;
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

        [Inject]
        public void Construct(IPersistentProgressService progressService)
        {
            _progressService = progressService;
        }
        
        public void ApplyDamage(float damage)
        {
            if (enabled == false)
                return;

            Current -= damage;
            _animator.PlayHit();
        }

        public void LoadProgress()
        {
            _health = _progressService.Progress.HealthData;
            HealthChanged?.Invoke();
        }

        public void UpdateProgress()
        {
            _progressService.Progress.HealthData.CurrentHp = Current;
            _progressService.Progress.HealthData.MaxHp = Max;
        }
    }
}