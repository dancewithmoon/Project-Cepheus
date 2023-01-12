using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Services.Input;
using CodeBase.Utils;
using UnityEngine;
using Zenject;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator), typeof(HeroAnimationEventHandler), typeof(CharacterController))]
    public class HeroAttack : MonoBehaviour, ISavedProgressReader
    {
        private const int MaxCountOfTargets = 3;
        
        [SerializeField] private Transform _attackPoint;

        [Header("Components")] 
        [SerializeField] private HeroAnimator _animator;
        [SerializeField] private HeroAnimationEventHandler _animationEvents;
        
        private AttackData _attackData;
        private IInputService _inputService;
        
        private static int _layerMask;
        private readonly Collider[] _hits = new Collider[MaxCountOfTargets];

        public float Damage => _attackData.Damage;
        public float AttackPointRadius => _attackData.AttackPointRadius;

        [Inject]
        public void Construct(IInputService inputService)
        {
            _inputService = inputService;

            _layerMask = 1 << LayerMask.NameToLayer("Hittable");

            _animationEvents.Attacked += ApplyAttack;
        }

        private void Update()
        {
            if (_inputService.IsAttackButtonUp() && _animator.IsAttacking == false) 
                _animator.PlayAttack();
        }

        private void ApplyAttack()
        {
            if (Hit() > 0)
            {
                PhysicsDebug.DrawDebug(GetAttackPoint(), AttackPointRadius, 1);
                foreach (Collider hit in _hits)
                {
                    if (hit == null)
                        continue;

                    hit.GetComponentInParent<IHealth>().ApplyDamage(Damage);
                }
            }
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _attackData = progress.AttackData.Clone();
        }

        private int Hit() => 
            Physics.OverlapSphereNonAlloc(GetAttackPoint(), AttackPointRadius, _hits, _layerMask);

        private Vector3 GetAttackPoint() => _attackPoint.position;
    }
}