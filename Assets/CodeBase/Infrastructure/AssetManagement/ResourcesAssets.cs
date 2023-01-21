using System;
using UnityEngine;

namespace CodeBase.Infrastructure.AssetManagement
{
    public class ResourcesAssets : IAssets
    {
        public GameObject Load(string path) => 
            Resources.Load<GameObject>(path);

        public void Load(string path, Action<GameObject> onLoaded)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            onLoaded?.Invoke(prefab);
        }
    }
}