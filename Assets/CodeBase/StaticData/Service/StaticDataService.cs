using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.StaticData.Service
{
    public class StaticDataService : IStaticDataService
    {
        private const string EnemiesPath = "StaticData/Enemies";
        
        private Dictionary<EnemyTypeId,EnemyStaticData> _enemies;

        public void LoadEnemies()
        {
            _enemies = Resources
                .LoadAll<EnemyStaticData>(EnemiesPath)
                .ToDictionary(x => x.EnemyType, x => x);
        }

        public EnemyStaticData GetEnemy(EnemyTypeId typeId) =>
            _enemies.TryGetValue(typeId, out EnemyStaticData enemyData) 
                ? enemyData 
                : null;
    }
}