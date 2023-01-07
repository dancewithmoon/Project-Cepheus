using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.AssetManagement
{
    public class ZenjectAssetProvider : IAssets
    {
        private readonly DiContainer _container;

        public ZenjectAssetProvider(DiContainer container)
        {
            _container = container;
        }

        public GameObject Instantiate(string path) => 
            _container.InstantiatePrefabResource(path);

        public GameObject Instantiate(string path, Vector3 at) => 
            _container.InstantiatePrefabResource(path, at, Quaternion.identity, null);
    }
}