using System;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.AssetManagement
{
    public class ResourcesAssets : IAssets
    {
        public Task<T> Load<T>(object source) where T : Object
        {
            if (source is string path)
            {
                return Task.FromResult(Resources.Load<T>(path));
            }
            if (source is T sourceObject)
            {
                return Task.FromResult(sourceObject);
            }

            throw new Exception("Source Type mismatch!");
        }
    }
}