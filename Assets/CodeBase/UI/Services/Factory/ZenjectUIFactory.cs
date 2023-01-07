using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services;
using CodeBase.StaticData.Service;
using CodeBase.UI.Services.Screens;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Services.Factory
{
    public class ZenjectUIFactory : IUIFactory
    {
        private readonly IAssets _assets;
        private readonly DiContainer _container;
        private readonly IStaticDataService _staticData;
        private Transform _uiRoot;

        public ZenjectUIFactory(IAssets assets, DiContainer container)
        {
            _assets = assets;
            _container = container;
            _staticData = AllServices.Container.Single<IStaticDataService>();
        }

        public void CreateUIRoot()
        {
            _uiRoot = _assets.Instantiate(AssetPath.UIRootPath).transform;
        }
        
        public void CreateShop() =>
            _container.InstantiatePrefab(
                _staticData.GetScreen(ScreenId.Shop), _uiRoot);
    }
}