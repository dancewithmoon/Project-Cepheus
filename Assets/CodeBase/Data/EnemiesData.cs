using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
    public class EnemiesData : IReadonlyEnemiesData
    {
        public List<string> KilledEnemies = new List<string>();
        public IReadOnlyList<string> KilledEnemiesReadonly => KilledEnemies;
    }

    public interface IReadonlyEnemiesData
    {
        public IReadOnlyList<string> KilledEnemiesReadonly { get; }
    }
}