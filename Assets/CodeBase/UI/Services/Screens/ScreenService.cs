using System;
using CodeBase.UI.Services.Factory;

namespace CodeBase.UI.Services.Screens
{
    public class ScreenService : IScreenService
    {
        private readonly IUIFactory _uiFactory;

        public ScreenService(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public void Open(ScreenId screenId)
        {
            switch (screenId)
            {
                case ScreenId.Unknown:
                    break;
                case ScreenId.Shop:
                    _uiFactory.CreateShop();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(screenId), screenId, null);
            }
        }
    }
}