using System;

namespace CodeBase.Data
{
    [Serializable]
    public class LootOnLevel
    {
        public SerializableDictionary<string, LootPieceData> Loots = new SerializableDictionary<string, LootPieceData>();
    }
}