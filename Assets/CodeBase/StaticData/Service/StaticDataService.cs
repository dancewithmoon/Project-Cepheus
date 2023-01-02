using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.StaticData.Service
{
    public class StaticDataService : IStaticDataService
    {
        private const string HeroPath = "StaticData/HeroData";
        private const string EnemiesPath = "StaticData/Enemies";
        private const string LevelsPath = "StaticData/Levels";

        private HeroDefaultStaticData _hero;
        private Dictionary<EnemyTypeId,EnemyStaticData> _enemies;
        private Dictionary<string,LevelStaticData> _levels;

        public void Load()
        {
            _hero = Resources.Load<HeroDefaultStaticData>(HeroPath);        
            
            _enemies = Resources.LoadAll<EnemyStaticData>(EnemiesPath)
                .ToDictionary(x => x.EnemyType, x => x);      
            
            _levels = Resources.LoadAll<LevelStaticData>(LevelsPath)
                .ToDictionary(x => x.LevelKey, x => x);
        }

        public HeroDefaultStaticData GetHero() => _hero;

        public EnemyStaticData GetEnemy(EnemyTypeId typeId) =>
            _enemies.TryGetValue(typeId, out EnemyStaticData enemyData) 
                ? enemyData 
                : null;

        public LevelStaticData GetLevel(string levelKey) => 
            _levels.TryGetValue(levelKey, out LevelStaticData levelData) 
                ? levelData 
                : null;
    }
}