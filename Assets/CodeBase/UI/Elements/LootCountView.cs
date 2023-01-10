using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Elements
{
    public class LootCountView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _count;
        private LootData _lootData;

        [Inject]
        public void Construct(IPersistentProgressService progressService)
        {
            _lootData = progressService.Progress.LootData;

            _lootData.Changed += OnLootDataChanged;
            UpdateCounter();
        }

        private void OnDestroy()
        {
            _lootData.Changed -= OnLootDataChanged;
        }

        private void OnLootDataChanged() => UpdateCounter();

        private void UpdateCounter()
        {
            _count.text = _lootData.Count.ToString();
        }
    }
}