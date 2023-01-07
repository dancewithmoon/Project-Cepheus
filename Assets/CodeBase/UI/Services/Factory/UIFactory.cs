using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services;
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
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;
        private Transform _uiRoot;

        public UIFactory(IAssets assets, IPersistentProgressService progressService)
        {
            _assets = assets;
            _progressService = progressService;
            _staticData = AllServices.Container.Single<IStaticDataService>();
        }

        public void CreateUIRoot()
        {
            _uiRoot = _assets.Instantiate(AssetPath.UIRootPath).transform;
        }
        
        public void CreateShop()
        {
            BaseScreen prefab = _staticData.GetScreen(ScreenId.Shop);
            BaseScreen screen = Object.Instantiate(prefab, _uiRoot);
            screen.Construct(_progressService);
        }
    }
}