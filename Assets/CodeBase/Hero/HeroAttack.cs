using CodeBase.Data;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Services.Input;
using CodeBase.Utils;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator), typeof(CharacterController))]
    public class HeroAttack : MonoBehaviour, ISavedProgressReader
    {
        private const int MaxCountOfTargets = 3;

        [SerializeField] private Transform _attackPoint;
        
        [Header("Components")]
        [SerializeField] private HeroAnimator _animator;

        private AttackData _attackData;

        private IInputService _input;

        private static int _layerMask;
        private readonly Collider[] _hits = new Collider[MaxCountOfTargets];

        public float Damage => _attackData.Damage;
        public float AttackPointRadius => _attackData.AttackPointRadius;

        private void Awake()
        {
            _input = AllServices.Container.Single<IInputService>();

            _layerMask = 1 << LayerMask.NameToLayer("Hittable");
        }

        private void Update()
        {
            if (_input.IsAttackButtonUp() && _animator.IsAttacking == false)
            {
                _animator.PlayAttack();
            }
        }

        //animation event
        private void OnAttack()
        {
            if (Hit() > 0)
            {
                PhysicsDebug.DrawDebug(GetAttackPoint(), AttackPointRadius, 1);
                foreach (Collider hit in _hits)
                {
                    if(hit == null)
                        continue;
                    
                    hit.GetComponentInParent<IHealth>().ApplyDamage(Damage);
                }
            }
        }

        private int Hit() => 
            Physics.OverlapSphereNonAlloc(GetAttackPoint(), AttackPointRadius, _hits, _layerMask);

        private Vector3 GetAttackPoint() => _attackPoint.position;

        public void LoadProgress(PlayerProgress progress)
        {
            _attackData = progress.AttackData.Clone();
        }
    }
}