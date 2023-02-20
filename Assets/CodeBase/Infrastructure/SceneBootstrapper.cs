using CodeBase.Infrastructure.Services.ContainerService;
using CodeBase.Services.UserInput;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class SceneBootstrapper : MonoInstaller
    {
        [Inject] public ContainerService ContainerService;
        
        public override void InstallBindings()
        {
            ContainerService.Container = Container;
            Container.Bind<IInputService>().To<StandaloneInputService>().AsSingle();
        }
    }
}