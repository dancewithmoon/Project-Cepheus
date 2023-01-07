using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "StaticData/Enemy")]
    public class EnemyStaticData : ScriptableObject
    {
        [SerializeField] private EnemyTypeId _enemyType;

        [Range(1, 10)] 
        [SerializeField] private float _movementSpeed = 3;

        [Range(1, 100)] 
        [SerializeField] private int _hp;

        [Range(1, 30)] 
        [SerializeField] private int _damage;

        [Range(0.5f, 1)] 
        [SerializeField] private float _attackPointRadius = 0.5f;

        [Range(0.5f, 1)] 
        [SerializeField] private float _effectiveDistance = 0.5f;

        [Range(0.1f, 10f)] 
        [SerializeField] private float _attackCooldown = 2f;

        [SerializeField] private int _minLoot;
        [SerializeField] private int _maxLoot;

        [SerializeField] private GameObject _prefab;

        public EnemyTypeId EnemyType => _enemyType;
        public float MovementSpeed => _movementSpeed;
        public int Hp => _hp;
        public int Damage => _damage;
        public float AttackPointRadius => _attackPointRadius;
        public float EffectiveDistance => _effectiveDistance;
        public float AttackCooldown => _attackCooldown;
        public int MinLoot => _minLoot;

        public int MaxLoot => _maxLoot;

        public GameObject Prefab => _prefab;
    }
}