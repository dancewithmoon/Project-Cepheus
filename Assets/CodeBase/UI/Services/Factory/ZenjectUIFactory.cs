using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Instantiating;
using CodeBase.Services.Ads;
using CodeBase.StaticData.Service;
using CodeBase.UI.Services.Screens;
using UnityEngine;

namespace CodeBase.UI.Services.Factory
{
    public class ZenjectUIFactory : IUIFactory
    {
        private readonly IAssets _assets;
        private readonly IInstantiateService _instantiateService;
        private readonly IStaticDataService _staticData;
        private readonly IAdsService _ads;
        private Transform _uiRoot;

        public ZenjectUIFactory(IAssets assets, IInstantiateService instantiateService, IStaticDataService staticData, IAdsService ads)
        {
            _assets = assets;
            _instantiateService = instantiateService;
            _staticData = staticData;
            _ads = ads;
        }

        public async void CreateUIRoot()
        {
            GameObject rootPrefab = await _assets.Load(AssetPath.UIRootPath);
            _uiRoot = _instantiateService.Instantiate(rootPrefab).transform;
        }

        public void CreateShop() =>
            _instantiateService.Instantiate(_staticData.GetScreen(ScreenId.Shop).gameObject, _uiRoot);
    }
}