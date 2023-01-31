namespace CodeBase.Infrastructure.Services.PersistentProgress
{
    public interface ISavedProgressReader
    {
        void LoadProgress();
    }

    public interface ISavedProgress : ISavedProgressReader
    {
        void UpdateProgress();
    }
}