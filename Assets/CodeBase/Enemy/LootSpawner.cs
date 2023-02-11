using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Services.Randomizer;
using UnityEngine;
using Zenject;

namespace CodeBase.Enemy
{
    public class LootSpawner : MonoBehaviour
    {
        [SerializeField] private EnemyDeath _death;
        private IGameFactory _factory;
        private IRandomService _random;

        private int _lootMin;
        private int _lootMax;

        [Inject]
        public void Construct(IGameFactory factory, IRandomService randomService)
        {
            _factory = factory;
            _random = randomService;
        }

        public void Initialize(int min, int max)
        {
            _lootMin = min;
            _lootMax = max;
        }

        private void Start()
        {
            _death.Happened += OnDeathHappened;
        }

        private void OnDestroy()
        {
            _death.Happened -= OnDeathHappened;
        }
        
        private void OnDeathHappened() => SpawnLoot();

        private async void SpawnLoot()
        {
            LootPiece lootPiece = await _factory.CreateLoot();
            lootPiece.GetComponent<UniqueId>().Generate();
            lootPiece.transform.position = transform.position;

            lootPiece.Initialize(GenerateLoot());
        }

        private Loot GenerateLoot() => 
            new Loot(_random.Next(_lootMin, _lootMax));
    }
}