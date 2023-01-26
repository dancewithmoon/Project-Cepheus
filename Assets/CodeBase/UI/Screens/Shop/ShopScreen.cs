using TMPro;
using UnityEngine;

namespace CodeBase.UI.Screens.Shop
{
    public class ShopScreen : BaseScreen
    {
        [SerializeField] private TextMeshProUGUI _lootCount;
        [SerializeField] private RewardedAdItem _rewardedItem;
        
        protected override void Initialize()
        {
            _rewardedItem.Initialize();
            OnLootCountChanged();
        }

        protected override void Subscribe()
        {
            _rewardedItem.Subscribe();
            Progress.LootData.Changed += OnLootCountChanged;
        }

        private void OnLootCountChanged()
        {
            _lootCount.text = Progress.LootData.Count.ToString();
        }

        protected override void Cleanup()
        {
            _rewardedItem.Cleanup();
            Progress.LootData.Changed -= OnLootCountChanged;
        }
    }
}