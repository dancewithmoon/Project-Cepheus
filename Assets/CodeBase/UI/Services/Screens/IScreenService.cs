using CodeBase.Infrastructure.Services;

namespace CodeBase.UI.Services.Screens
{
    public interface IScreenService : IService
    {
        void Open(ScreenId screenId);
    }
}