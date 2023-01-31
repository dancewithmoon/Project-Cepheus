using System;

namespace CodeBase.Data
{
    [Serializable]
    public class LootData : IReadonlyLootData
    {
        public int Count;

        public int CountReadonly => Count;
        
        public event Action Changed;

        public void Collect(Loot loot)
        {
            Count += loot.Value;
            Changed?.Invoke();
        }

        public void Add(int loot)
        {
            Count += loot;
            Changed?.Invoke();
        }
    }

    public interface IReadonlyLootData
    {
        int CountReadonly { get; }
        event Action Changed;
    }
}