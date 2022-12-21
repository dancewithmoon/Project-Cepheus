using CodeBase.Infrastructure.Services;

namespace CodeBase.StaticData.Service
{
    public interface IStaticDataService : IService
    {
        void LoadHero();
        void LoadEnemies();
        HeroDefaultStaticData GetHero();
        EnemyStaticData GetEnemy(EnemyTypeId typeId);
    }
}