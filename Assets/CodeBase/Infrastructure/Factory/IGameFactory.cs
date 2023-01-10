using System.Collections.Generic;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        GameObject CreateHero(GameObject initialPoint);
        GameObject CreateHud();
        GameObject CreateSavePoint(Vector3 at, Vector3 scale);
        GameObject CreateEnemySpawner(Vector3 at, string spawnerId, EnemyTypeId enemyTypeId);
        GameObject CreateEnemy(EnemyTypeId enemyTypeId, Transform parent);
        LootPiece CreateLoot();
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        void Cleanup();
    }
}