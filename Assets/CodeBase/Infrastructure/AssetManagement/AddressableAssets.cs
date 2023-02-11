using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Infrastructure.AssetManagement
{
    public class AddressableAssets : IAssets
    {
        public async Task<GameObject> Load(object source)
        {
            if (source is string path)
            {
                return await Task.FromResult(Resources.Load<GameObject>(path));
            }

            return await Addressables.LoadAssetAsync<GameObject>(source).Task;
        }
    }
}