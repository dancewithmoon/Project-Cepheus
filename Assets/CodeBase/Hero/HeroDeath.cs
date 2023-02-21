using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroHealth), typeof(HeroAnimator))]
    public class HeroDeath : MonoBehaviour
    {
        [Header("VFX")] 
        [SerializeField] private GameObject _deathFxPrefab;
        
        [Header("Components")] 
        [SerializeField] private List<Behaviour> _disableOnDeath;

        private HeroHealth _health;
        private HeroAnimator _animator;

        private void Awake()
        {
            _health = GetComponent<HeroHealth>();
            _animator = GetComponent<HeroAnimator>();
        }

        private void Start()
        {
            _health.HealthChanged += OnHealthChanged;
        }

        private void OnDestroy()
        {
            _health.HealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged()
        {
            if (_health.Current > 0)
                return;

            Die();
        }

        private void Die()
        {
            _disableOnDeath.ForEach(component => component.enabled = false);
            _health.enabled = false;
            _animator.PlayDeath();
            PlayDeathFx();
        }

        private void PlayDeathFx() => 
            Instantiate(_deathFxPrefab, transform.position, Quaternion.identity);
    }
}