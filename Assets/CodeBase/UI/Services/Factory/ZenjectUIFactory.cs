using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Instantiating;
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
        private Transform _uiRoot;

        public ZenjectUIFactory(IAssets assets, IInstantiateService instantiateService, IStaticDataService staticData)
        {
            _assets = assets;
            _instantiateService = instantiateService;
            _staticData = staticData;
        }

        public void CreateUIRoot()
        {
            _uiRoot = _instantiateService.Instantiate(_assets.Load(AssetPath.UIRootPath)).transform;
        }

        public void CreateShop() =>
            _instantiateService.Instantiate(_staticData.GetScreen(ScreenId.Shop).gameObject, _uiRoot);
    }
}