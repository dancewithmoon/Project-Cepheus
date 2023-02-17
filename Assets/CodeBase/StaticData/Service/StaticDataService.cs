using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.StaticData.Ads;
using CodeBase.UI.Screens;
using CodeBase.UI.Services.Screens;

namespace CodeBase.StaticData.Service
{
    public class StaticDataService : IStaticDataService
    {
        private const string HeroPath = "HeroData";
        private const string EnemiesPath = "EnemyStaticData";
        private const string LevelsPath = "LevelStaticData";
        private const string ScreensPath = "Screens";
        private const string UnityAdsPath = "UnityAds";

        private readonly IAssets _assets;
        
        private HeroDefaultStaticData _hero;
        private Dictionary<string, LevelStaticData> _levels;
        private Dictionary<ScreenId, BaseScreen> _screens;
        private Dictionary<EnemyTypeId, EnemyStaticData> _enemies;
        private UnityAdsStaticData _unityAds;

        public StaticDataService(IAssets assets)
        {
            _assets = assets;
        }

        public async Task Load()
        {
            await Task.WhenAll(
                LoadHero(),
                LoadEnemies(),
                LoadLevels(),
                LoadScreens(),
                LoadAds());
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

        public UnityAdsStaticData GetUnityAdsData() => _unityAds;

        private async Task LoadHero() => 
            _hero = await _assets.Load<HeroDefaultStaticData>(HeroPath);
        
        private async Task LoadEnemies()
        {
            IEnumerable<EnemyStaticData> loaded = await _assets.LoadAll<EnemyStaticData>(EnemiesPath);
            _enemies = loaded.ToDictionary(x => x.EnemyType, x => x);
        }

        private async Task LoadLevels()
        {
            IEnumerable<LevelStaticData> loaded = await _assets.LoadAll<LevelStaticData>(LevelsPath);
            _levels = loaded.ToDictionary(x => x.LevelKey, x => x);
        }

        private async Task LoadScreens()
        {
            ScreenStaticData screensData = await _assets.Load<ScreenStaticData>(ScreensPath);
            _screens = new Dictionary<ScreenId, BaseScreen>(screensData.Screens);
        }
        
        private async Task LoadAds() => 
            _unityAds = await _assets.Load<UnityAdsStaticData>(UnityAdsPath);
    }
}