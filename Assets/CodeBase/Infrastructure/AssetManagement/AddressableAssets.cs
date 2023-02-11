using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.AssetManagement
{
    public class AddressableAssets : IAssets
    {
        private readonly Dictionary<string, AsyncOperationHandle> _completedCache =
            new Dictionary<string, AsyncOperationHandle>();

        private readonly Dictionary<string, AsyncOperationHandle> _currentlyLoading =
            new Dictionary<string, AsyncOperationHandle>();

        public AddressableAssets() =>
            Addressables.InitializeAsync();

        public async Task<T> Load<T>(object source) where T : Object
        {
            return source switch
            {
                string path => await LoadByPath<T>(path),
                AssetReference reference => await LoadByReference<T>(reference),
                _ => throw new Exception("Source Type mismatch!")
            };
        }

        private async Task<T> LoadByPath<T>(string path) where T : Object
        {
            return await LoadAsset<T>(path, path);
        }

        private async Task<T> LoadByReference<T>(AssetReference reference) where T : Object
        {
            return await LoadAsset<T>(reference.AssetGUID, reference);
        }

        private async Task<T> LoadAsset<T>(string key, object source) where T : Object
        {
            if (_completedCache.TryGetValue(key, out AsyncOperationHandle completedHandle))
                return completedHandle.Result as T;

            if (_currentlyLoading.TryGetValue(key, out AsyncOperationHandle loadingHandle))
                return await loadingHandle.Task as T;

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(source);
            _currentlyLoading.Add(key, handle);
            await handle.Task;
            _currentlyLoading.Remove(key);
            _completedCache.Add(key, handle);
            return handle.Result;
        }
    }
}