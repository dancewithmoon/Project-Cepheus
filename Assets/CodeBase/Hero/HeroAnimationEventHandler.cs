using System;
using UnityEngine;

namespace CodeBase.Hero
{
    public class HeroAnimationEventHandler : MonoBehaviour
    {
        public event Action Attacked;

        private void OnAttack() => Attacked?.Invoke();
    }
}