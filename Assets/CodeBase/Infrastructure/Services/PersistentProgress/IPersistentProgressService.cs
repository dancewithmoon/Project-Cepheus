using CodeBase.Data;

namespace CodeBase.Infrastructure.Services.PersistentProgress
{
    public interface IPersistentProgressService : IService
    {
        PlayerProgress Progress { get; set; }
    }

    public interface IReadonlyProgressService : IService
    {
        IReadOnlyPlayerProgress ProgressReadonly { get; }
    }
}