using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Hero
{
    public class HeroLootPickUp : MonoBehaviour, ISavedProgress
    {
        public LootData LootData { get; } = new LootData();
        
        public void PickUp(Loot loot)
        {
            LootData.Collect(loot);
        }

        public void LoadProgress(PlayerProgress progress)
        {
            LootData.Count = progress.LootData.Count;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.LootData.Count = LootData.Count;
        }
    }
}