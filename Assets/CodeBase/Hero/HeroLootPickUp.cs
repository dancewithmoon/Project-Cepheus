using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using Zenject;

namespace CodeBase.Hero
{
    public class HeroLootPickUp : MonoBehaviour
    {
        private IPersistentProgressService _progressService;

        [Inject]
        public void Construct(IPersistentProgressService progressService)
        {
            _progressService = progressService;
        }
        
        public void PickUp(Loot loot)
        {
            _progressService.Progress.LootData.Collect(loot);
        }
    }
}