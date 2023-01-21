using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Instantiating;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData.Service;
using CodeBase.UI.Screens;
using CodeBase.UI.Services.Screens;
using UnityEngine;

namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private readonly IAssets _assets;
        private readonly IInstantiateService _instantiateService;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;
        private Transform _uiRoot;

        public UIFactory(IAssets assets, IInstantiateService instantiateService, IStaticDataService staticData, IPersistentProgressService progressService)
        {
            _assets = assets;
            _instantiateService = instantiateService;
            _staticData = staticData;
            _progressService = progressService;
        }

        public void CreateUIRoot()
        {
            _uiRoot = _instantiateService.Instantiate(_assets.Load(AssetPath.UIRootPath)).transform;
        }
        
        public void CreateShop()
        {
            BaseScreen prefab = _staticData.GetScreen(ScreenId.Shop);
            BaseScreen screen = _instantiateService.Instantiate(prefab.gameObject, _uiRoot).GetComponent<BaseScreen>();
            screen.Construct(_progressService);
        }
    }
}