using CodeBase.Infrastructure.Services.ContainerService;
using UnityEngine;

namespace CodeBase.Infrastructure.AssetManagement
{
    public class ZenjectAssetProvider : IAssets
    {
        private readonly ContainerService _container;

        public ZenjectAssetProvider(ContainerService container)
        {
            _container = container;
        }

        public GameObject Instantiate(string path) => 
            _container.Container.InstantiatePrefabResource(path);

        public GameObject Instantiate(string path, Vector3 at) => 
            _container.Container.InstantiatePrefabResource(path, at, Quaternion.identity, null);
    }
}