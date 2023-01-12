using System;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class EnemyAnimationEventHandler : MonoBehaviour
    {
        public event Action Attacked;
        public event Action AttackEnded;

        private void OnAttack()
        {
            Attacked?.Invoke();
        }

        private void OnAttackEnded()
        {
            AttackEnded?.Invoke();
        }
    }
}