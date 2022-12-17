using System;

namespace CodeBase.Data
{
    [Serializable]
    public class AttackData
    {
        public float AttackPointRadius;
        public float Damage;

        public AttackData Clone()
        {
            return new AttackData
            {
                AttackPointRadius = AttackPointRadius,
                Damage = Damage
            };
        }
    }
}