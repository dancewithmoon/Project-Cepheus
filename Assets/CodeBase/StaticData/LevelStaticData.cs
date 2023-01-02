using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "StaticData/Level")]
    public class LevelStaticData : ScriptableObject
    {
        [SerializeField] private string _levelKey;
        [SerializeField] private List<EnemySpawnerData> _enemySpawners;

        public string LevelKey => _levelKey;

        public IReadOnlyList<EnemySpawnerData> EnemySpawners => _enemySpawners;
    }
}