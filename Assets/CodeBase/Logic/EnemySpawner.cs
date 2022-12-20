using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Logic
{
    [RequireComponent(typeof(UniqueId))]
    public class EnemySpawner : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private MonsterTypeId _monsterTypeId;
        [SerializeField] public bool _slain;
        
        private string _id;

        private void Awake()
        {
            _id = GetComponent<UniqueId>().Id;
        }

        private void Spawn()
        {
            
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.EnemiesData.KilledEnemies.Contains(_id))
            {
                _slain = true;
                return;
            }

            Spawn();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (_slain)
            {
                progress.EnemiesData.KilledEnemies.Add(_id);
            }
        }
    }
}