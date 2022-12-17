using System;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public HealthData HeroHealthData;
        public AttackData AttackData;
        public WorldData WorldData;

        public PlayerProgress(string initialLevel)
        {
            HeroHealthData = new HealthData();
            AttackData = new AttackData();
            WorldData = new WorldData(initialLevel);
        }
    }
}