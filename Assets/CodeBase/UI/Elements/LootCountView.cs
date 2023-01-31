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
        private IReadonlyProgressService _progressService;

        private IReadonlyLootData LootReadonly => _progressService.ProgressReadonly.LootReadonly; 
        
        [Inject]
        public void Construct(IReadonlyProgressService progressService)
        {
            _progressService = progressService;
            LootReadonly.Changed += OnLootDataChanged;
            UpdateCounter();
        }

        private void OnDestroy()
        {
            LootReadonly.Changed -= OnLootDataChanged;
        }

        private void OnLootDataChanged() => UpdateCounter();

        private void UpdateCounter() => 
            _count.text = LootReadonly.CountReadonly.ToString();
    }
}