using CodeBase.Infrastructure.Services;

namespace CodeBase.StaticData.Service
{
    public interface IStaticDataService : IService
    {
        void LoadEnemies();
        EnemyStaticData GetEnemy(EnemyTypeId typeId);
    }
}