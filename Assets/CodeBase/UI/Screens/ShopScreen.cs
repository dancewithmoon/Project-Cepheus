using TMPro;
using UnityEngine;

namespace CodeBase.UI.Screens
{
    public class ShopScreen : BaseScreen
    {
        [SerializeField] private TextMeshProUGUI _lootCount;

        protected override void Initialize()
        {
            OnLootCountChanged();
        }

        protected override void SubscribeOnUpdates()
        {
            Progress.LootData.Changed += OnLootCountChanged;
        }

        private void OnLootCountChanged()
        {
            _lootCount.text = Progress.LootData.Count.ToString();
        }

        protected override void UnsubscribeOnUpdates()
        {
            Progress.LootData.Changed -= OnLootCountChanged;
        }
    }
}