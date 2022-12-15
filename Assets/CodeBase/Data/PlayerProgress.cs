using System;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public HealthData HeroHealthData;
        public WorldData WorldData;

        public PlayerProgress(string initialLevel)
        {
            HeroHealthData = new HealthData();
            WorldData = new WorldData(initialLevel);
        }
    }
}