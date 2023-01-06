using CodeBase.Infrastructure.Services;
using CodeBase.UI.Screens;
using CodeBase.UI.Services.Screens;

namespace CodeBase.StaticData.Service
{
    public interface IStaticDataService : IService
    {
        void Load();
        HeroDefaultStaticData GetHero();
        EnemyStaticData GetEnemy(EnemyTypeId typeId);
        LevelStaticData GetLevel(string levelKey);
        BaseScreen GetScreen(ScreenId screenId);
    }
}