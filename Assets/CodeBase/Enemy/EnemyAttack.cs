using System.Collections;
using System.Linq;
using CodeBase.Logic;
using CodeBase.Utils;
using UnityEngine;
using Zenject;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyAttack : MonoBehaviour
    {
        [SerializeField] private EnemyAnimator _animator;

        private float _attackPointRadius;
        private float _cooldown;

        private float _damage;
        private float _effectiveDistance;
        
        private bool _isAttackEnabled;
        private bool _isAttacking;

        private LayerMask _layerMask;

        private readonly Collider[] _hits = new Collider[1];
        
        [Inject(Id = "hero")] public Transform HeroTransform { get; set; }

        public void Initialize(float damage, float attackPointRadius, float effectiveDistance, float cooldown)
        {
            _damage = damage;
            _attackPointRadius = attackPointRadius;
            _effectiveDistance = effectiveDistance;
            _cooldown = cooldown;

            _layerMask = LayerMask.GetMask("Player");

            StartCoroutine(AttackLoop());
        }

        private IEnumerator AttackLoop()
        {
            WaitForSeconds cooldown = new WaitForSeconds(_cooldown);
            while (this)
            {
                yield return new WaitUntil(IsAttackEnabled);
                StartAttack();
                yield return new WaitWhile(IsAttacking);
                yield return cooldown;
            }
        }

        public void EnableAttack()
        {
            _isAttackEnabled = true;
        }

        public void DisableAttack()
        {
            _isAttackEnabled = false;
        }

        private void StartAttack()
        {
            _isAttacking = true;

            transform.LookAt(HeroTransform);
            _animator.PlayAttack();
        }

        //animation event
        private void OnAttack()
        {
            if (Hit(out Collider hit))
            {
                PhysicsDebug.DrawDebug(GetAttackPoint(), _attackPointRadius, 1);
                hit.transform.GetComponent<IHealth>().ApplyDamage(_damage);
            }
        }

        //animation event
        private void OnAttackEnded()
        {
            _isAttacking = false;
        }

        private bool Hit(out Collider hit)
        {
            int hitCount = Physics.OverlapSphereNonAlloc(GetAttackPoint(), _attackPointRadius, _hits, _layerMask);
            hit = _hits.FirstOrDefault();
            return hitCount > 0;
        }

        private Vector3 GetAttackPoint()
        {
            Vector3 attackPoint = transform.position + transform.forward * _effectiveDistance;
            attackPoint.y += 0.5f;
            return attackPoint;
        }
        
        private bool IsAttacking() => _isAttacking;
        private bool IsAttackEnabled() => _isAttackEnabled;
    }
}