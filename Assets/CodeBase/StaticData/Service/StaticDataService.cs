using System.Collections.Generic;
using System.Linq;
using CodeBase.UI.Screens;
using CodeBase.UI.Services.Screens;
using UnityEngine;

namespace CodeBase.StaticData.Service
{
    public class StaticDataService : IStaticDataService
    {
        private const string HeroPath = "StaticData/HeroData";
        private const string EnemiesPath = "StaticData/Enemies";
        private const string LevelsPath = "StaticData/Levels";
        private const string ScreensPath = "StaticData/Screens";

        private HeroDefaultStaticData _hero;
        private Dictionary<string, LevelStaticData> _levels;
        private Dictionary<ScreenId, BaseScreen> _screens;
        private Dictionary<EnemyTypeId, EnemyStaticData> _enemies;

        public void Load()
        {
            _hero = Resources.Load<HeroDefaultStaticData>(HeroPath);

            _enemies = Resources.LoadAll<EnemyStaticData>(EnemiesPath)
                .ToDictionary(x => x.EnemyType, x => x);

            _levels = Resources.LoadAll<LevelStaticData>(LevelsPath)
                .ToDictionary(x => x.LevelKey, x => x);

            _screens = new Dictionary<ScreenId, BaseScreen>(Resources.Load<ScreenStaticData>(ScreensPath).Screens);
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

        public BaseScreen GetScreen(ScreenId screenId) =>
            _screens.TryGetValue(screenId, out BaseScreen screen)
                ? screen
                : null;
    }
}