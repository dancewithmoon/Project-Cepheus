using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "StaticData/Level")]
    public class LevelStaticData : ScriptableObject
    {
        [SerializeField] private string _levelKey;
        [SerializeField] private List<EnemySpawnerData> _enemySpawners;
        [SerializeField] private List<SavePointData> _savePoints;

        public string LevelKey => _levelKey;

        public IReadOnlyList<EnemySpawnerData> EnemySpawners => _enemySpawners;
        public IReadOnlyList<SavePointData> SavePoints => _savePoints;
    }
}