using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class Aggro : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private AgentMoveToHero _follow;

        [SerializeField] private float _cooldown;
        private Coroutine _cooldownCoroutine;
        
        private bool _hasTarget; //flag is needed to avoid multiple trigger events
        
        private void Start()
        {
            _triggerObserver.TriggerEnter += TriggerEnter;
            _triggerObserver.TriggerExit += TriggerExit;

            StopFollow();
        }

        private void TriggerEnter(Collider obj)
        {
            if(_hasTarget)
                return;

            _hasTarget = true;
            StopCooldownCoroutine();
            StartFollow();
        }

        private void TriggerExit(Collider obj)
        {
            if(_hasTarget == false)
                return;
            
            _hasTarget = false;
            _cooldownCoroutine = StartCoroutine(StopFollowAfterCooldown());
        }

        private void StartFollow()
        {
            _follow.enabled = true;
        }

        private void StopFollow()
        {
            _follow.enabled = false;
        }

        private IEnumerator StopFollowAfterCooldown()
        {
            yield return new WaitForSeconds(_cooldown);
            StopFollow();
        }

        private void StopCooldownCoroutine()
        {
            if (_cooldownCoroutine != null)
            {
                StopCoroutine(_cooldownCoroutine);
                _cooldownCoroutine = null;
            }
        }

        private void OnDestroy()
        {
            _triggerObserver.TriggerEnter -= TriggerEnter;
            _triggerObserver.TriggerExit -= TriggerExit;
        }
    }
}