using System.Collections;
using CodeBase.Data;
using CodeBase.Hero;
using TMPro;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class LootPiece : MonoBehaviour
    {
        private const float DelayToDestroyAfterPickup = 1.5f;
        
        [SerializeField] private GameObject _skull;
        [SerializeField] private GameObject _pickUpFxPrefab;
        [SerializeField] private TextMeshPro _lootText;
        [SerializeField] private GameObject _pickUpPopup;

        private int _targetLayer;
        private Loot _loot;
        private bool _picked;

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

            StartCoroutine(DestroyWithDelay());
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