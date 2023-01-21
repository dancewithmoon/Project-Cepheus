using System;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Infrastructure.AssetManagement
{
    public interface IAssets : IService
    {
        GameObject Load(string path);
        void Load(string path, Action<GameObject> onLoaded);
    }
}