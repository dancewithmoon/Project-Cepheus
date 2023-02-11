using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        Task<GameObject> CreateHero();
        Task<GameObject> CreateHud();
        Task<GameObject> CreateSavePoint(Vector3 at, Vector3 scale);
        Task<GameObject> CreateEnemySpawner(Vector3 at, string spawnerId, EnemyTypeId enemyTypeId);
        Task<GameObject> CreateEnemy(EnemyTypeId enemyTypeId, Transform parent);
        Task<LootPiece> CreateLoot();
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        void Cleanup();
    }
}