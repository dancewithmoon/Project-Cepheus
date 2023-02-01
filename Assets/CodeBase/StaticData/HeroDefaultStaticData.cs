using System;
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

        [SerializeField] private SerializableDictionary<string, Vector3> _initialPoints;
        
        [SerializeField] private GameObject _prefab;

        public float Hp => _hp;
        public float Damage => _damage;
        public float AttackPointRadius => _attackPointRadius;
        public GameObject Prefab => _prefab;

        public Vector3 GetInitialPoint(string level)
        {
            if (_initialPoints.TryGetValue(level, out Vector3 initialPoint))
                return initialPoint;

            throw new NullReferenceException($"Level [{level}] wasn't found in the initial points dictionary!");
        }
    }
}