﻿using System.Collections;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class Aggro : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private Aggrable _aggrable;

        [SerializeField] private float _cooldown;
        private Coroutine _cooldownCoroutine;

        private bool _hasTarget; //flag is needed to avoid multiple trigger events

        private void Start()
        {
            _triggerObserver.TriggerEnter += TriggerEnter;
            _triggerObserver.TriggerExit += TriggerExit;

            StopFollow();
        }

        private void OnDestroy()
        {
            _triggerObserver.TriggerEnter -= TriggerEnter;
            _triggerObserver.TriggerExit -= TriggerExit;
        }

        private void TriggerEnter(Collider obj)
        {
            if (_hasTarget)
                return;

            _hasTarget = true;
            StopCooldownCoroutine();
            StartFollow();
        }

        private void TriggerExit(Collider obj)
        {
            if (_hasTarget == false)
                return;

            _hasTarget = false;
            _cooldownCoroutine = StartCoroutine(StopFollowAfterCooldown());
        }

        private void StartFollow()
        {
            _aggrable.enabled = true;
        }

        private void StopFollow()
        {
            _aggrable.enabled = false;
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
    }
}