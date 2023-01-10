using CodeBase.Infrastructure.Services.ContainerService;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class SceneBootstrapper : MonoInstaller
    {
        [Inject] public ContainerService ContainerService;
        
        public override void InstallBindings()
        {
            ContainerService.Container = Container;
        }
    }
}