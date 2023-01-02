using System.Collections;
using CodeBase.Data;
using CodeBase.Hero;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using TMPro;
using UnityEngine;

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
        
        private int _targetLayer;
        private Loot _loot;
        private bool _picked;

        private LootOnLevel _lootOnLevel;

        public void Construct(LootOnLevel lootOnLevel)
        {
            _lootOnLevel = lootOnLevel;
        }

        public void Initialize(Loot loot)
        {
            _loot = loot;
            _targetLayer = LayerMask.NameToLayer("Player");
        }

        public void LoadProgress(PlayerProgress progress)
        {
            LootPieceData lootPieceData = _lootOnLevel.Loots[_uniqueId.Id];
            transform.position = lootPieceData.PositionOnLevel.Position.AsUnityVector();
            Initialize(lootPieceData.Loot);
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if(_picked)
                return;
            
            if (progress.WorldData.LootOnLevel.Loots.ContainsKey(_uniqueId.Id))
            {
                progress.WorldData.LootOnLevel.Loots.Remove(_uniqueId.Id);
            }
            
            progress.WorldData.LootOnLevel.Loots.Add(_uniqueId.Id, new LootPieceData()
            {
                Loot = _loot,
                PositionOnLevel = new PositionOnLevel(gameObject.scene.name, transform.position.AsVectorData())
            });
        }

        private void OnTriggerEnter(Collider other)
        {
            if (IsOnTargetLayer(other) == false) 
                return;

            if (other.gameObject.TryGetComponent(out HeroLootPickUp hero))
            {
                PickUp(hero);
            }
        }

        private void PickUp(HeroLootPickUp hero)
        {
            if(_picked)
                return;
            
            _picked = true;

            hero.PickUp(_loot);

            HideSkull();
            PlayPickUpFx();
            ShowText();
            RemoveFromLootOnLevel();

            StartCoroutine(DestroyWithDelay());
        }

        private void RemoveFromLootOnLevel()
        {
            _lootOnLevel.Loots.Remove(_uniqueId.Id);
        }

        private void HideSkull()
        {
            _skull.SetActive(false);
        }

        private IEnumerator DestroyWithDelay()
        {
            yield return new WaitForSeconds(DelayToDestroyAfterPickup);
            _pickUpPopup.SetActive(false);
            Destroy(gameObject);
        }

        private void PlayPickUpFx() => Instantiate(_pickUpFxPrefab, transform.position, Quaternion.identity);

        private void ShowText()
        {
            _lootText.text = _loot.Value.ToString();
            _pickUpPopup.SetActive(true);
        }

        private bool IsOnTargetLayer(Component obj) => obj.gameObject.layer == _targetLayer;
    }
}