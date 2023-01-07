using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using Zenject;

namespace CodeBase.Hero
{
    public class HeroLootPickUp : MonoBehaviour, ISavedProgress
    {
        private LootData _lootData;

        [Inject]
        public void Construct(IPersistentProgressService progressService)
        {
            _lootData = progressService.Progress.LootData;
        }
        
        public void PickUp(Loot loot)
        {
            _lootData.Collect(loot);
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _lootData.Count = progress.LootData.Count;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.LootData.Count = _lootData.Count;
        }
    }
}