using CodeBase.Infrastructure.Services;

namespace CodeBase.StaticData.Service
{
    public interface IStaticDataService : IService
    {
        void Load();
        HeroDefaultStaticData GetHero();
        EnemyStaticData GetEnemy(EnemyTypeId typeId);
        LevelStaticData GetLevel(string levelKey);
    }
}