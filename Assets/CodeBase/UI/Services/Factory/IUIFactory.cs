using System.Threading.Tasks;
using CodeBase.Infrastructure.Services;

namespace CodeBase.UI.Services.Factory
{
    public interface IUIFactory : IService
    {
        Task WarmUp();
        void CreateUIRoot();
        void CreateShop();
    }
}