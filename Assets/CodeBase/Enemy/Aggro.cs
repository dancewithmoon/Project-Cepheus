using UnityEngine;

namespace CodeBase.Enemy
{
    public class Aggro : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private AgentMoveToHero _follow;

        private void Start()
        {
            _triggerObserver.TriggerEnter += TriggerEnter;
            _triggerObserver.TriggerExit += TriggerExit;

            StopFollow();
        }

        private void TriggerEnter(Collider obj) => StartFollow();
        private void TriggerExit(Collider obj) => StopFollow();

        private void StartFollow()
        {
            _follow.enabled = true;
        }

        private void StopFollow()
        {
            _follow.enabled = false;
        }

        private void OnDestroy()
        {
            _triggerObserver.TriggerEnter -= TriggerEnter;
            _triggerObserver.TriggerExit -= TriggerExit;
        }
    }
}