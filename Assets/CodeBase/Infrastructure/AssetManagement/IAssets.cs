using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Infrastructure.AssetManagement
{
    public interface IAssets : IService
    {
        Task<T> Load<T>(object source) where T : Object;
        Task<IEnumerable<T>> LoadAll<T>(string path) where T : Object;
        void CleanUp();
    }
}