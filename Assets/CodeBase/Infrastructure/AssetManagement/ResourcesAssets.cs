using System;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.AssetManagement
{
    public class ResourcesAssets : IAssets
    {
        public Task<GameObject> Load(object source)
        {
            if (source is string path)
            {
                return Task.FromResult(Resources.Load<GameObject>(path));
            }
            if (source is GameObject gameObject)
            {
                return Task.FromResult(gameObject);
            }

            throw new Exception("Source Type mismatch!");
        }
    }
}