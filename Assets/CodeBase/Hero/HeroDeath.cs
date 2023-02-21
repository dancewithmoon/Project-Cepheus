using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroHealth), typeof(HeroMove), typeof(HeroAnimator))]
    public class HeroDeath : MonoBehaviour
    {
        [Header("Components")] 
        [SerializeField] private List<Behaviour> _disableOnDeath;
        [SerializeField] private HeroHealth _health;
        [SerializeField] private HeroAnimator _animator;

        [Header("VFX")] 
        [SerializeField] private GameObject _deathFxPrefab;

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