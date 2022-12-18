using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.UI
{
    public class ActorUI : MonoBehaviour
    {
        [SerializeField] private HealthBar _healthBar;
        private IHealth _health;

        private void Start()
        {
            var health = GetComponent<IHealth>();

            if (health != null)
            {
                Construct(health);
            }
        }

        public void Construct(IHealth health)
        {
            _health = health;
            _health.HealthChanged += OnHealthChanged;
        }

        private void OnHealthChanged()
        {
            _healthBar.SetValue(_health.Current, _health.Max);
        }

        private void OnDestroy()
        {
            if (_health != null)
            {
                _health.HealthChanged -= OnHealthChanged;
            }
        }
    }
}