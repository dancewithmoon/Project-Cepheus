using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Infrastructure.AssetManagement
{
    public class AddressableAssets : IAssets
    {
        public async Task<T> Load<T>(object source) where T : Object
        {
            if (source is string path)
            {
                return await Task.FromResult(Resources.Load<T>(path));
            }

            return await Addressables.LoadAssetAsync<T>(source).Task;
        }
    }
}