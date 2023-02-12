using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.AssetManagement
{
    public class ResourcesAssets : IAssets
    {
        private readonly Dictionary<string, Object> _completedCache =
            new Dictionary<string, Object>();

        public Task<T> Load<T>(object source) where T : Object
        {
            if (source is string path)
            {
                T asset = Resources.Load<T>(path);
                _completedCache.Add(path, asset);
                return Task.FromResult(asset);
            }

            if (source is T sourceObject)
            {
                _completedCache.Add(sourceObject.name, sourceObject);
                return Task.FromResult(sourceObject);
            }

            throw new Exception("Source Type mismatch!");
        }

        public void CleanUp()
        {
            foreach (Object asset in _completedCache.Values)
            {
                Resources.UnloadAsset(asset);
            }
            _completedCache.Clear();
        }
    }
}