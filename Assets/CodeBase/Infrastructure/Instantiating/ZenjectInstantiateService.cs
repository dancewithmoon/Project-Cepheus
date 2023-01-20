using CodeBase.Infrastructure.Services.ContainerService;
using UnityEngine;

namespace CodeBase.Infrastructure.Instantiating
{
    public class ZenjectInstantiateService : IInstantiateService
    {
        private readonly ContainerService _container;

        public ZenjectInstantiateService(ContainerService container)
        {
            _container = container;
        }
        
        public GameObject Instantiate(GameObject prefab) => 
            _container.Container.InstantiatePrefab(prefab);

        public GameObject Instantiate(GameObject prefab, Vector3 at) =>
            Instantiate(prefab, at, null);

        public GameObject Instantiate(GameObject prefab, Vector3 at, Transform parent) => 
            _container.Container.InstantiatePrefab(prefab, at, Quaternion.identity, parent);
    }
}