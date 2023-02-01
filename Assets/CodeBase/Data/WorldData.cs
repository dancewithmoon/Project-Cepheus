using System;

namespace CodeBase.Data
{
    [Serializable]
    public class WorldData : IReadonlyWorldData
    {
        public PositionOnLevel PositionOnLevel;
        public LootOnLevel LootOnLevel;

        public IReadonlyPositionOnLevel PositionOnLevelReadonly => PositionOnLevel;
        public IReadonlyLootOnLevel LootOnLevelReadonly => LootOnLevel;

        public WorldData(string initialLevel)
        {
            PositionOnLevel = new PositionOnLevel(initialLevel);
            LootOnLevel = new LootOnLevel();
        }
    }

    public interface IReadonlyWorldData
    {
        public IReadonlyPositionOnLevel PositionOnLevelReadonly { get; }
        public IReadonlyLootOnLevel LootOnLevelReadonly { get; }
    }
}