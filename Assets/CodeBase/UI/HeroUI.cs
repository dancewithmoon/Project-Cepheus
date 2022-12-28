using CodeBase.Data;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.UI
{
    public class HeroUI : ActorUI
    {
        [SerializeField] private LootCountView _lootCount;
        
        public void Construct(IHealth health, LootData lootData)
        {
            base.Construct(health);
            _lootCount.Construct(lootData);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}