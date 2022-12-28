using CodeBase.Data;
using TMPro;
using UnityEngine;

namespace CodeBase.UI
{
    public class LootCountView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _count;
        private LootData _lootData;

        public void Construct(LootData lootData)
        {
            _lootData = lootData;
            _lootData.Changed += OnLootDataChanged;
            UpdateCounter();
        }

        private void OnLootDataChanged() => UpdateCounter();

        private void UpdateCounter()
        {
            _count.text = _lootData.Count.ToString();
        }

        private void OnDestroy()
        {
            _lootData.Changed -= OnLootDataChanged;
        }
    }
}