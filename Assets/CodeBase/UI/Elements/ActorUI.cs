using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class ActorUI : MonoBehaviour
    {
        [SerializeField] private HealthBarView _healthBar;
        private IHealth _health;

        protected virtual void OnDestroy()
        {
            if (_health != null) 
                _health.HealthChanged -= OnHealthChanged;
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
    }
}