using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.StaticData.Service
{
    public class StaticDataService : IStaticDataService
    {
        private const string HeroPath = "StaticData/HeroData";
        private const string EnemiesPath = "StaticData/Enemies";

        private HeroDefaultStaticData _hero;
        private Dictionary<EnemyTypeId,EnemyStaticData> _enemies;

        public void LoadHero()
        {
            _hero = Resources.Load<HeroDefaultStaticData>(HeroPath);
        }

        public void LoadEnemies()
        {
            _enemies = Resources
                .LoadAll<EnemyStaticData>(EnemiesPath)
                .ToDictionary(x => x.EnemyType, x => x);
        }

        public HeroDefaultStaticData GetHero() => _hero;

        public EnemyStaticData GetEnemy(EnemyTypeId typeId) =>
            _enemies.TryGetValue(typeId, out EnemyStaticData enemyData) 
                ? enemyData 
                : null;
    }
}