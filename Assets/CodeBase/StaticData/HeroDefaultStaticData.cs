using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "HeroData", menuName = "StaticData/Hero")]
    public class HeroDefaultStaticData : ScriptableObject
    {
        [Range(1, 100)] 
        [SerializeField] private float _hp = 50;

        [Range(1, 50)] 
        [SerializeField] private float _damage = 5;

        [Range(0.5f, 1)] 
        [SerializeField] private float _attackPointRadius = 0.7f;

        [SerializeField] private GameObject _prefab;

        public float Hp => _hp;
        public float Damage => _damage;
        public float AttackPointRadius => _attackPointRadius;
        public GameObject Prefab => _prefab;
    }
}