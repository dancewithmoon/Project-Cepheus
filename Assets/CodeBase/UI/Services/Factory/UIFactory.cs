using CodeBase.Infrastructure.AssetManagement;
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
        private Transform _uiRoot;
        
        public UIFactory(IAssets assets, IStaticDataService staticData)
        {
            _assets = assets;
            _staticData = staticData;
        }

        public void CreateUIRoot()
        {
            _uiRoot = _assets.Instantiate(AssetPath.UIRootPath).transform;
        }
        
        public void CreateShop()
        {
            BaseScreen screen = _staticData.GetScreen(ScreenId.Shop);
            Object.Instantiate(screen, _uiRoot);
        }
    }
}