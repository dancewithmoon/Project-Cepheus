using System;

namespace CodeBase.Data
{
    [Serializable]
    public class HealthData
    {
        public float CurrentHp;
        public float MaxHp;

        public void ResetHp()
        {
            CurrentHp = MaxHp;
        }

        public HealthData Clone() =>
            new HealthData
            {
                CurrentHp = CurrentHp,
                MaxHp = MaxHp
            };
    }
}