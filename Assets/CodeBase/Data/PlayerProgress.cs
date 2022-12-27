using System;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public HealthData HeroHealthData;
        public AttackData AttackData;
        public LootData LootData;
        public WorldData WorldData;
        public EnemiesData EnemiesData;

        public PlayerProgress(string initialLevel)
        {
            HeroHealthData = new HealthData();
            AttackData = new AttackData();
            LootData = new LootData();
            WorldData = new WorldData(initialLevel);
            EnemiesData = new EnemiesData();
        }
    }
}