using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Hero
{
    public class HeroLootPickUp : MonoBehaviour, ISavedProgress
    {
        private LootData _lootData;
        
        public void PickUp(Loot loot)
        {
            _lootData.Collect(loot);
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _lootData = progress.LootData.Clone();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.LootData.Collected = _lootData.Collected;
        }
    }
}