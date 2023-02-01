using System;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerProgress : IReadOnlyPlayerProgress
    { 
        public HealthData HealthData;
        public AttackData AttackData;
        public LootData LootData;
        public WorldData WorldData;
        public EnemiesData EnemiesData;

        public HealthData HealthReadonly => HealthData;
        public AttackData AttackReadonly => AttackData;
        public IReadonlyLootData LootReadonly => LootData;
        public IReadonlyWorldData WorldDataReadonly => WorldData;
        public IReadonlyEnemiesData EnemiesDataReadonly => EnemiesData;

        public PlayerProgress(string initialLevel)
        {
            HealthData = new HealthData();
            AttackData = new AttackData();
            LootData = new LootData();
            WorldData = new WorldData(initialLevel);
            EnemiesData = new EnemiesData();
        }
    }

    public interface IReadOnlyPlayerProgress
    {
        public HealthData HealthReadonly { get; }
        public AttackData AttackReadonly { get; }
        public IReadonlyLootData LootReadonly { get; }
        public IReadonlyWorldData WorldDataReadonly { get; }
        public IReadonlyEnemiesData EnemiesDataReadonly { get; }
    }
}