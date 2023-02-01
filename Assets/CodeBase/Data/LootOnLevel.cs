using System;

namespace CodeBase.Data
{
    [Serializable]
    public class LootOnLevel : IReadonlyLootOnLevel
    {
        public SerializableDictionary<string, LootPieceData> Loots = new SerializableDictionary<string, LootPieceData>();
        
        public IReadonlySerializableDictionary<string, LootPieceData> LootsReadonly => Loots;
    }

    public interface IReadonlyLootOnLevel
    {
        public IReadonlySerializableDictionary<string, LootPieceData> LootsReadonly { get; }
    }
}