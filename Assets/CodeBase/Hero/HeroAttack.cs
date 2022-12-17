using CodeBase.Infrastructure.Services;
using CodeBase.Services.Input;
using CodeBase.Utils;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator), typeof(CharacterController))]
    public class HeroAttack : MonoBehaviour
    {
        private const int MaxCountOfTargets = 3;
        
        [SerializeField] private float _effectiveDistance;
        [SerializeField] private float _cleavage;
        
        [Header("References")]
        [SerializeField] private HeroAnimator _animator;
        [SerializeField] private CharacterController _characterController;
        
        private IInputService _input;

        private static int _layerMask;
        private readonly Collider[] _hits = new Collider[MaxCountOfTargets];

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

        private void OnAttack()
        {
            if (Hit() > 0)
            {
                PhysicsDebug.DrawDebug(GetAttackPoint(), _cleavage, 1);
            }
            else
            {
                PhysicsDebug.DrawDebug(GetAttackPoint(), _cleavage, 0.5f);
            }
        }

        private int Hit() => 
            Physics.OverlapSphereNonAlloc(GetAttackPoint(), _cleavage, _hits, _layerMask);

        private Vector3 GetAttackPoint()
        {
            Vector3 attackPoint = transform.position + transform.forward * _effectiveDistance;
            attackPoint.y = _characterController.center.y / 2;
            return attackPoint;
        }
    }
}