﻿using System.Collections;
using CodeBase.Data;
using CodeBase.Hero;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(UniqueId))]
    public class LootPiece : MonoBehaviour, ISavedProgress
    {
        private const float DelayToDestroyAfterPickup = 1.5f;

        [SerializeField] private GameObject _skull;
        [SerializeField] private GameObject _pickUpFxPrefab;
        [SerializeField] private TextMeshPro _lootText;
        [SerializeField] private GameObject _pickUpPopup;

        [Header("UniqueId")] 
        [SerializeField] private UniqueId _uniqueId;

        private Loot _loot;

        private IPersistentProgressService _progressService;
        private bool _picked;

        private int _targetLayer;

        private LootOnLevel LootOnLevel => _progressService.Progress.WorldData.LootOnLevel;
        
        [Inject]
        public void Construct(IPersistentProgressService progressService)
        {
            _progressService = progressService;
        }

        public void Initialize(Loot loot)
        {
            _loot = loot;
            _targetLayer = LayerMask.NameToLayer("Player");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (IsOnTargetLayer(other) == false)
                return;

            if (other.gameObject.TryGetComponent(out HeroLootPickUp hero)) 
                PickUp(hero);
        }

        public void LoadProgress()
        {
            LootPieceData lootPieceData = LootOnLevel.Loots[_uniqueId.Id];
            transform.position = lootPieceData.PositionOnLevel.Position.AsUnityVector();
            Initialize(lootPieceData.Loot);
        }

        public void UpdateProgress()
        {
            if (_picked)
                return;

            if (LootOnLevel.Loots.ContainsKey(_uniqueId.Id))
                LootOnLevel.Loots.Remove(_uniqueId.Id);

            LootOnLevel.Loots.Add(_uniqueId.Id, new LootPieceData
            {
                Loot = _loot,
                PositionOnLevel = new PositionOnLevel(SceneManager.GetActiveScene().name, transform.position.AsVectorData())
            });
        }

        private void PickUp(HeroLootPickUp hero)
        {
            if (_picked)
                return;

            _picked = true;

            hero.PickUp(_loot);

            HideSkull();
            PlayPickUpFx();
            ShowText();
            RemoveFromLootOnLevel();

            StartCoroutine(DestroyWithDelay());
        }

        private void RemoveFromLootOnLevel() => 
            LootOnLevel.Loots.Remove(_uniqueId.Id);

        private void HideSkull() => 
            _skull.SetActive(false);

        private IEnumerator DestroyWithDelay()
        {
            yield return new WaitForSeconds(DelayToDestroyAfterPickup);
            _pickUpPopup.SetActive(false);
            Destroy(gameObject);
        }

        private void PlayPickUpFx() => 
            Instantiate(_pickUpFxPrefab, transform.position, Quaternion.identity);

        private void ShowText()
        {
            _lootText.text = _loot.Value.ToString();
            _pickUpPopup.SetActive(true);
        }

        private bool IsOnTargetLayer(Component obj) => obj.gameObject.layer == _targetLayer;
    }
}