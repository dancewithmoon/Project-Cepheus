using System;

namespace CodeBase.Data
{
    [Serializable]
    public class LootData
    {
        public int Count;
        
        public event Action Changed;

        public void Collect(Loot loot)
        {
            Count += loot.Value;
            Changed?.Invoke();
        }
    }
}